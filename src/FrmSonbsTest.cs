using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Planet.Core.Shared.Request;
using Planet.IntegrationEvents.LiveEvent;
using RabbitMQ.Client;
using Sonbs.Sc6200mh.TcpClient;
using Sonbs.Sc6200mh.TcpClient.Models;
using System.Text;
using System.Text.Json;

namespace SonbsTest;

// TODO try-catch

public sealed partial class FrmSonbsTest : Form
{
    private readonly IConfigurationRoot _config;
    private IChannel? _eventChannel;
    private IGaravotApi? _garavotApi;
    private readonly StatusStripLogger _logger;
    private Sc6200mhTcpClient? _sonbsClient;
    private ScanResult? _sonbsDevs;

    public FrmSonbsTest()
    {
        _config = new ConfigurationBuilder().AddUserSecrets<FrmSonbsTest>().Build();
        InitializeComponent();
        _logger = new StatusStripLogger(tslLog);
        tslLog.Text = "Ready";
    }

    private async void btnConnectGaravot_Click(object __, EventArgs _)
    {
        await ConnectGaravotAsync();
        await ConnectRabbitAsync();
    }

    private async void btnVotazioneStart_Click(object __, EventArgs _)
    {
        if (cmbVotazioneTipi.SelectedValue == null) { tslLog.Text = "inizia votazione: nessuna votazione selezionata"; return; }
        if (_sonbsClient == null) { tslLog.Text = "inizia votazione: sonbs non inizializzato"; return; }

        var mode = Enum.Parse<VotingMode>((string)cmbVotazioneTipi.SelectedValue);
        await _sonbsClient.StartVoting2Async(mode, default);
        tslLog.Text = $"votazione {mode} avviata";
        // TODO stato: in votazione
    }

    private async void cmdConnectSonbs_Click(object __, EventArgs _)
    {
        await ConnectSonbsAsync();

        _sonbsDevs = await _sonbsClient!.ScanDevicesAsync(default);
        viewSonbs.Clear();
        foreach (var dev in _sonbsDevs.T31)
            viewSonbs.Items.Add(new ListViewItem([dev.Id.ToString(), dev.IsChairman.ToString(), dev.MicState.ToString(), string.Empty]));
    }

    async Task ConnectGaravotAsync()
    {
        string accessToken;
        {
            using var hc = new HttpClient();
            var test = await hc.RequestTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = GaravotOpenIdUrl,
                GrantType = "client_credentials",
                ClientId = GaravotClientId,
                ClientSecret = GaravotClientSecret,
                Scope = "email profile roles",
            });
            if (string.IsNullOrEmpty(test.AccessToken)) { tslLog.Text = "ERRORE garavot token non ottenuto"; return; }
            accessToken = test.AccessToken;
        }

        try
        {
            using var ghc = new HttpClient();
            ghc.BaseAddress = new(GaravotApiUri);
            ghc.SetBearerToken(accessToken);

            _garavotApi = GaravotApiFactory.Create(ghc);
            var delegateResponse = await _garavotApi.GetDelegatesAsync();
            var delegateGroupsResponse = await _garavotApi.GetDelegateGroupsAsync();
            var delegateGroupDelegatesResponseTask = _garavotApi.GetDelegateGroupDelegatesAsync(new DelegateGroupDelegateSearchRequest
            {
                Page = 1,
                PageSize = short.MaxValue,
                SortBy = [new SortCriteria<DelegateGroupDelegateSortField> {
                        Direction = SortDirection.Ascending,
                        SortField = DelegateGroupDelegateSortField.Start }]
            });
            var delegateGroupDelegatesResponse = await delegateGroupDelegatesResponseTask;
            foreach (var deleg in delegateResponse.Content.Data.Items)
            {
                //var firstDgd = delegateGroupDelegatesResponse.Content.Data.Items
                //    .Where(dgd => dgd.DelegateId == deleg.DelegateId)
                //    .Where(dgd => dgd.IsDeleted == false) // dovrei fare join per i delegate group attivi?
                //    .First();
                //var delegateGroup = delegateGroupsResponse.Content.Data.Items
                //    .Where(_ => _.DelegateGroupId == firstDgd.GovernmentBodyId)
                //    .First().Acronym;

                var delegateGroup = delegateGroupsResponse.Content.Data.Items
                    .Where(_ => deleg.DelegateGroupIds.Contains(_.DelegateGroupId))
                    .First().Acronym;
                viewDelegates.Items.Add(new ListViewItem([deleg.FirstName, deleg.LastName, delegateGroup, "?", "-"]));
            }
            // TODO dovrei ancora assegnare mic a delegate
        }
        catch (Exception garavotErr)
        {
            MessageBox.Show(garavotErr.Message, "Garavot connection failed");
        }
    }

    async Task ConnectRabbitAsync()
    {
        var rabbitmqConnectionFactory = new ConnectionFactory()
        {
            HostName = RabbitMqHost,
            UserName = RabbitMqUser,
            Password = RabbitMqPass,
        };

        // TODO rabbitmqConnection e _eventChannel sono ovviamente log-lived e da fare dispose
        var rabbitmqConnection = await rabbitmqConnectionFactory.CreateConnectionAsync();
        _eventChannel = await rabbitmqConnection.CreateChannelAsync();
        /// NO QUEUE await _eventChannel.QueueBindAsync("queue", "exchange", "routing");
        await _eventChannel.BasicPublishAsync("exchange", "routing", true, body: null);

        //var consumer = new AsyncEventingBasicConsumer(_eventChannel);
        //consumer.ReceivedAsync += RabbitEventReceivedAsync;
        //await _eventChannel.BasicConsumeAsync(queue: "task_queue", autoAck: true, consumer);

        tslLog.Text = "rabbit connesso";
    }

    private async Task ConnectSonbsAsync()
    {
        tslLog.Text = "provo a connettere sonbs";
        _sonbsClient = new Sc6200mhTcpClient(new StatusStripLogger2<Sc6200mhTcpClient>(_logger), new Sc6200mhTcpClientSettings(SonbsHost));
        await _sonbsClient.StartAsync();
        tslLog.Text = "sonbs connesso";
    }

    private async void btnSignInEnd_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { tslLog.Text = "sign in: sonbs non inizializzato"; return; }
        await _sonbsClient.StopSignInAsync(default);
    }

    private async void btnSignInStart_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { tslLog.Text = "sign in: sonbs non inizializzato"; return; }
        tslLog.Text = "ATTENZIONE SIGN IN INIZIATO -- VERRÀ ATTESA CONFERMA DAI MICROFONI";
        // TODO meccanismo stop? potrei fare pulsante signin a doppio stato?
        var x = await _sonbsClient.WaitSignInAsync(_sonbsDevs.T31.Count, default);
        // TODO QUESTO CORRISPONDE AD UNO STATO? devo bloccare operazioni finché non finisce
    }

    private void btnVotazioneEnd_Click(object __, EventArgs _)
    {
        // TODO non posso stoppare una votazione se non è cominciata
        if (_sonbsClient == null) { tslLog.Text = "votazione: sonbs non inizializzato"; return; }
    }

    private async void cmdSittingStart_Click(object __, EventArgs _)
    {
        if (_eventChannel == null) { tslLog.Text = "inizia seduta: rabbit non inizializzato"; return; }

    }

    private async void cmdSendTopic_Click(object __, EventArgs _)
    {
        if (_eventChannel == null) { tslLog.Text = "invia topic: rabbit non inizializzato"; return; }
        if (viewOrdini.SelectedItems.Count != 1) { tslLog.Text = "topic: selezionare un ordine"; return; }

        var ev = new TopicChangedEto { Account = "ac103", Title = "" };
        var body = JsonSerializer.Serialize(ev);
        var bytes = Encoding.UTF8.GetBytes(body);
        await _eventChannel.BasicPublishAsync("", "", true, bytes);
    }

    private async void cmdSittingStop_Click(object __, EventArgs _)
    {
        if (_eventChannel == null) { tslLog.Text = "fine seduta: rabbit non inizializzato"; return; }

    }

    private void cmdSendTalkOn_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { tslLog.Text = "apri parola: sonbs non inizializzato"; return; }
        if (_eventChannel == null) { tslLog.Text = "apri parola: rabbit non inizializzato"; return; }

        var ev = new TalkStartedEto
        {
            Account = "",
            CardNumber = "",
            SeatNumber = "",
            MicrophoneStatus = TalkMicrophoneStatus.Open,
        };
    }

    private void cmdSendTalkOff_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { tslLog.Text = "chiudi parola: sonbs non inizializzato"; return; }
        if (_eventChannel == null) { tslLog.Text = "chiudi parola: rabbit non inizializzato"; return; }

        var ev = new TalkStartedEto
        {
            Account = "",
            CardNumber = "",
            SeatNumber = "",
            MicrophoneStatus = TalkMicrophoneStatus.Close,
        };
    }

    private void cmdCloseAllMics_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { tslLog.Text = "chiudi tutti mic: sonbs non inizializzato"; return; }
        if (_eventChannel == null) { tslLog.Text = "chiudi tutti mic: rabbit non inizializzato"; return; }

        var ev = new AllMicrophonesOffEto { Account = "" };
    }

    private void btnConfirmTalkRequest_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { tslLog.Text = "conferma richiesta parola: sonbs non inizializzato"; return; }

    }

    private void btnRefuteTalkRequest_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { tslLog.Text = "nega richiesta parola: sonbs non inizializzato"; return; }

    }

    private void viewOrdini_SelectedIndexChanged(object __, EventArgs _)
    {
        cmdSendTopic.Enabled = viewOrdini.SelectedItems.Count == 1;
    }

    private async void FrmSonbsTest_FormClosingAsync(object sender, FormClosingEventArgs e)
    {
        if (_sonbsClient != null) await _sonbsClient.StopAsync();
        if (_eventChannel != null) await _eventChannel.CloseAsync();
    }

    //private async Task RabbitEventReceivedAsync(object __, BasicDeliverEventArgs @event){}
}
