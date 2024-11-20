using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Planet.Core.Shared.Request;
using Planet.IntegrationEvents.LiveEvent;
using RabbitMQ.Client;
using Sonbs.Sc6200mh.TcpClient;
using Sonbs.Sc6200mh.TcpClient.Models;
using System.Text;
using System.Text.Json;

namespace SonbsTest;

// TODO try-catch

// architettura
// dovremmo quindi avere rabbit che invia eventi metadati sia a garavot che director?
// che significa avere exchange direct con bindate queue garavot + queue director con routing key stessi eventi
// però non penso di poterlo fare tra istanze diverse di rabbit
// quindi dovrei connettere tutti ad uno stesso rabbit? tipo 205 o demo stesso?

// TODO devo fare un client tipo servizio per keycloak

public sealed partial class FrmSonbsTest : Form
{
    private readonly IConfigurationRoot _config;
    private readonly SonbsToGaravot[] _delegatesMic;
    private IChannel? _eventChannel;
    private IGaravotApi? _garavotApi;
    private GovernmentData? _governmentData;
    private readonly StatusStripLogger _logger;
    private Sc6200mhTcpClient? _sonbsClient;
    private ScanResult? _sonbsDevs;

    public FrmSonbsTest()
    {
        _config = new ConfigurationBuilder().AddUserSecrets<FrmSonbsTest>().Build();
        InitializeComponent();
        _logger = new StatusStripLogger(tslLog);
        cmbVotazioneTipi.SelectedIndex = 0;
        _logger.LogInformation("Ready");

        // TEMP
        _delegatesMic = new[]
        {
            new SonbsToGaravot(17, new RecvMessageId(1, UnitIdType.Wireless)), // pieropan
            new SonbsToGaravot(23, new RecvMessageId(2, UnitIdType.Wireless)), // turrini
            new SonbsToGaravot(137, new RecvMessageId(1, UnitIdType.Wired)), // de santis, presidente
            new SonbsToGaravot(29, new RecvMessageId(2, UnitIdType.Wired)), // castellini
            new SonbsToGaravot(35, new RecvMessageId(3, UnitIdType.Wired)), // costantino
        };
    }

    private void btnConfirmTalkRequest_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { _logger.LogError("conferma richiesta parola: sonbs non inizializzato"); return; }

    }

    private async void btnConnectGaravot_Click(object __, EventArgs _)
    {
        await ConnectRabbitAsync();
        await ConnectGaravotAsync();
    }

    private void btnRefuteTalkRequest_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { _logger.LogError("nega richiesta parola: sonbs non inizializzato"); return; }

    }

    private async void btnSignInEnd_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { _logger.LogError("sign in: sonbs non inizializzato"); return; }
        await _sonbsClient.StopSignInAsync(default);
    }

    private async void btnSignInStart_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { _logger.LogError("sign in: sonbs non inizializzato"); return; }
        _logger.LogError("ATTENZIONE SIGN IN INIZIATO -- VERRÀ ATTESA CONFERMA DAI MICROFONI");
        // TODO meccanismo stop? potrei fare pulsante signin a doppio stato?
        var x = await _sonbsClient.WaitSignInAsync(_sonbsDevs.T31.Count, default);
        // TODO QUESTO CORRISPONDE AD UNO STATO? devo bloccare operazioni finché non finisce
    }

    private async void btnVotazioneEnd_Click(object __, EventArgs _)
    {
        // TODO non posso stoppare una votazione se non è cominciata
        if (_sonbsClient == null) { _logger.LogError("votazione: sonbs non inizializzato"); return; }
        if (_governmentData == null) { _logger.LogError("votazione: garavot non inizializzato"); return; }

        var voti = await _sonbsClient.StopVotingAsync();
        _logger.LogInformation($"votazione terminata: {voti} voti");
        viewVoteResult.Items.Clear();
        foreach (var voto in voti)
        {
            var delegateMicWhere = _delegatesMic.Where(_ => _.SonbsId == voto.MicId);
            if (delegateMicWhere.Any() == false)
            {
                _logger.LogWarning($"voto non associato a delegate: {voto.MicId}");
                continue;
            }
            var delegateMic = delegateMicWhere.First();
            var dlgt = _governmentData.Delegates.Where(_ => _.DelegateId == delegateMic.GaravotId).First();
            viewVoteResult.Items.Add(new ListViewItem([$"{dlgt.FirstName} {dlgt.LastName}", ((int)voto.Vote).ToString()]));
        }
    }

    private async void btnVotazioneStart_Click(object __, EventArgs _)
    {
        if (cmbVotazioneTipi.SelectedItem == null) { _logger.LogError("inizia votazione: nessuna votazione selezionata"); return; }
        if (_sonbsClient == null) { _logger.LogError("inizia votazione: sonbs non inizializzato"); return; }
        if (_governmentData == null) { _logger.LogError("inizia votazione: garavot non inizializzato"); return; }

        var mode = Enum.Parse<VotingMode>((string)cmbVotazioneTipi.SelectedItem);
        await _sonbsClient.StartVoting2Async(mode, default);
        _logger.LogInformation($"votazione {mode} avviata");
        // TODO stato: in votazione
        // TODO non c'è endpoint per eventi di votazione vero? raccoglie tutto alla fine? non ricordo
    }

    private async void cmdCloseAllMics_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { _logger.LogError("chiudi tutti mic: sonbs non inizializzato"); return; }
        if (_eventChannel == null) { _logger.LogError("chiudi tutti mic: rabbit non inizializzato"); return; }

        var ev = new AllMicrophonesOffEto { Account = GaravotAccount };
        await SendEventAsync(ev);
        _logger.LogInformation("inviato evento All Mics Off");
    }

    private async void cmdConnectSonbs_Click(object __, EventArgs _)
    {
        await ConnectSonbsAsync();

        _sonbsDevs = await _sonbsClient!.ScanDevicesAsync(default);
        viewSonbs.Items.Clear();
        foreach (var dev in _sonbsDevs.T31)
            viewSonbs.Items.Add(new ListViewItem([dev.Id.Id.ToString(), dev.Id.Target.ToString(), dev.IsChairman.ToString(), dev.MicState.ToString()]));
    }

    private async void cmdSendTalkOff_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { _logger.LogError("chiudi parola: sonbs non inizializzato"); return; }
        if (_eventChannel == null) { _logger.LogError("chiudi parola: rabbit non inizializzato"); return; }

        var selected = GetIdFromDelegatesTab();
        if (selected == null) return;

        var ev = new TalkStartedEto
        {
            Account = GaravotAccount,
            SeatNumber = selected.Value.Seat.ToString(),
            MicrophoneStatus = TalkMicrophoneStatus.Close,
        };
        await SendEventAsync(ev);
    }

    private async void cmdSendTalkOn_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { _logger.LogError("apri parola: sonbs non inizializzato"); return; }
        if (_eventChannel == null) { _logger.LogError("apri parola: rabbit non inizializzato"); return; }

        var selected = GetIdFromDelegatesTab();
        var ev = new TalkStartedEto
        {
            Account = GaravotAccount,
            SeatNumber = selected.Value.Seat.ToString(),
            MicrophoneStatus = TalkMicrophoneStatus.Open,
        };
        await SendEventAsync(ev);
    }

    private async void cmdSendTopic_Click(object __, EventArgs _)
    {
        if (_eventChannel == null) { _logger.LogError("invia topic: rabbit non inizializzato"); return; }
        if (viewOrdini.SelectedItems.Count != 1) { _logger.LogError("topic: selezionare un ordine"); return; }

        var ev = new TopicChangedEto { Account = "ac103", Title = "" };
        await SendEventAsync(ev);
    }

    private async void cmdSittingStart_Click(object __, EventArgs _)
    {
        if (_eventChannel == null) { _logger.LogError("inizia seduta: rabbit non inizializzato"); return; }

    }

    private async void cmdSittingStop_Click(object __, EventArgs _)
    {
        if (_eventChannel == null) { _logger.LogError("fine seduta: rabbit non inizializzato"); return; }

    }

    private async Task ConnectGaravotAsync()
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
            if (string.IsNullOrEmpty(test.AccessToken)) { _logger.LogError("ERRORE garavot token non ottenuto"); return; }
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
            var delegateGroupDelegatesResponse = await _garavotApi.GetDelegateGroupDelegatesAsync(new DelegateGroupDelegateSearchRequest
            {
                Page = 1,
                PageSize = short.MaxValue,
                SortBy = [new SortCriteria<DelegateGroupDelegateSortField> {
                    Direction = SortDirection.Ascending,
                    SortField = DelegateGroupDelegateSortField.Start }]
            });
            _governmentData = new GovernmentData(delegateResponse.Content.Data.Items, delegateGroupsResponse.Content.Data.Items,
                delegateGroupDelegatesResponse.Content.Data.Items);

            foreach (var deleg in delegateResponse.Content.Data.Items)
            {
                //var firstDgd = delegateGroupDelegatesResponse.Content.Data.Items
                //    .Where(dgd => dgd.DelegateId == deleg.DelegateId)
                //    .Where(dgd => dgd.IsDeleted == false) // dovrei fare join per i delegate group attivi?
                //    .First();
                //var delegateGroup = delegateGroupsResponse.Content.Data.Items
                //    .Where(_ => _.DelegateGroupId == firstDgd.GovernmentBodyId)
                //    .First().Acronym;

                var delegateGroupQuery = delegateGroupsResponse.Content.Data.Items.Where(_ => deleg.DelegateGroupIds.Contains(_.DelegateGroupId));
                if (delegateGroupQuery.Any() == false) continue;
                var delegateGroup = delegateGroupQuery.First().Acronym;

                // non c'è seat/card da api quindi prendo sonbs
                var micData = _delegatesMic.Where(_ => _.GaravotId == deleg.DelegateId);
                var micFirst = micData.FirstOrDefault();
                var micId = micData.Any()
                    ? micFirst.SonbsId.Id.ToString() + (micFirst.SonbsId.Target == UnitIdType.Wireless ? 'W' : 'C')
                    : " ";

                viewDelegates.Items.Add(new ListViewItem([deleg.FirstName, deleg.LastName, delegateGroup, micId, "-"]));
            }
        }
        catch (Exception garavotErr)
        {
            MessageBox.Show(garavotErr.Message, "Garavot connection failed");
        }
    }

    private async Task ConnectRabbitAsync()
    {
        var rabbitmqConnectionFactory = new ConnectionFactory()
        {
            HostName = RabbitMqHost,
            UserName = RabbitMqUser,
            Password = RabbitMqPass,
        };

        // TODO rabbitmqConnection e _eventChannel sono ovviamente long-lived e da fare dispose
        var rabbitmqConnection = await rabbitmqConnectionFactory.CreateConnectionAsync();
        _eventChannel = await rabbitmqConnection.CreateChannelAsync();

        /// NO QUEUE await _eventChannel.QueueBindAsync("queue", "exchange", "routing");
        //var consumer = new AsyncEventingBasicConsumer(_eventChannel);
        //consumer.ReceivedAsync += RabbitEventReceivedAsync;
        //await _eventChannel.BasicConsumeAsync(queue: "task_queue", autoAck: true, consumer);

        _logger.LogInformation("rabbit connesso");
    }

    private async Task ConnectSonbsAsync()
    {
        _logger.LogInformation("provo a connettere sonbs");
        _sonbsClient = new Sc6200mhTcpClient(new StatusStripLogger2<Sc6200mhTcpClient>(_logger), new Sc6200mhTcpClientSettings(SonbsHost));
        _sonbsClient.MicStatusChangedAsync = SonbsMicEventReceivedAsync;
        _sonbsClient.EmitPowerAsync = SonbsPowerEmittedAsync;
        await _sonbsClient.StartAsync();
        _logger.LogInformation("sonbs connesso");
    }

    private async void FrmSonbsTest_FormClosingAsync(object sender, FormClosingEventArgs e)
    {
        if (_sonbsClient != null) await _sonbsClient.StopAsync();
        if (_eventChannel != null) await _eventChannel.CloseAsync();
    }

    private static string GetEventRoute<T>(T _)
        => typeof(T).FullName
            ?? throw new System.Diagnostics.UnreachableException();

    private (int Seat, RecvMessageId SonbsId)? GetIdFromDelegatesTab()
    {
        if (tcDelegates.SelectedIndex == 0)
        {
            if (viewSonbs.SelectedItems.Count != 1) { _logger.LogError("selezionare un microfono"); return null; }
            var micId = new RecvMessageId(
                viewSonbs.SelectedItems[0].TabSonbsGetId(),
                viewSonbs.SelectedItems[0].TabSonbsGetConnection());
            var seat = _delegatesMic.Where(_ => _.SonbsId == micId).First();
            return (seat.GaravotId, micId);
        }
        else
        {
            if (viewDelegates.SelectedItems.Count != 1) { _logger.LogError("selezionare un delegato"); return null; }
            var seat = viewDelegates.SelectedItems[0].TabDelegatesGetSeat();
            var micId = _delegatesMic.Where(_ => _.GaravotId == seat).First();
            return (seat, micId.SonbsId);
        }
    }

    //private async Task RabbitEventReceivedAsync(object __, BasicDeliverEventArgs @event){}

    private async Task SendEventAsync<T>(T ev) where T : Planet.EventBus.IPlanetIntegrationEvent
    {
        var body = JsonSerializer.Serialize(ev);
        var bytes = Encoding.UTF8.GetBytes(body);
        await _eventChannel!.BasicPublishAsync(RabbitMqExchange, GetEventRoute(ev), true, bytes);
    }

    private async Task SonbsMicEventReceivedAsync((RecvMessageId MicId, bool IsOpen) eventData)
    {
        bool wasSet = false;
        for (int itemId = 0; itemId < viewSonbs.Items.Count; ++itemId)
        {
            if (viewSonbs.Items[itemId].TabSonbsGetId() == eventData.MicId.Id && viewSonbs.Items[itemId].TabSonbsGetConnection() == eventData.MicId.Target)
            {
                wasSet = true;
                viewSonbs.Items[itemId].TabSonbsSetIsOpen(eventData.IsOpen);
                break;
            }
        }
        if (wasSet == false)
        {
            _logger.LogWarning($"evento sonbs: mic non trovato [id: {eventData.MicId.Id}, wired: {eventData.MicId.Target}]");
            return;
        }
        _logger.LogInformation($"evento sonbs: mic aperto {eventData.IsOpen} [id: {eventData.MicId.Id}, wired: {eventData.MicId.Target}]");

        // invia evento a rabbit
        if (_eventChannel == null) return;

        var ev = new TalkStartedEto
        {
            Account = GaravotAccount,
            SeatNumber = "",
            MicrophoneStatus = eventData.IsOpen ? TalkMicrophoneStatus.Open : TalkMicrophoneStatus.Close,
        };
    }

    private async Task SonbsPowerEmittedAsync(Planet.Devices.Power.PowerStatus status)
    {
        if (status == Planet.Devices.Power.PowerStatus.Stopped) _sonbsClient = null;
        _logger.LogWarning($"evento sonbs: power {status}");
    }

    private void viewOrdini_SelectedIndexChanged(object __, EventArgs _)
    {
        cmdSendTopic.Enabled = viewOrdini.SelectedItems.Count == 1;
    }

    const string GaravotAccount = "ac114";
}

sealed record GovernmentData(
    IEnumerable<DelegateFeDto> Delegates,
    IEnumerable<DelegateGroupFeDto> DelegateGroups,
    IEnumerable<GovernmentBodyDelegateDto> DelegateGroupDelegates);

record struct SonbsToGaravot(int GaravotId, RecvMessageId SonbsId);

file static class FrmSonbsTestExtensions
{
    public static int TabDelegatesGetSeat(this ListViewItem i) => int.Parse(i.SubItems[3].Text);
    public static UnitIdType TabSonbsGetConnection(this ListViewItem i) => i.SubItems[1].Text == "W" ? UnitIdType.Wireless : UnitIdType.Wired;
    public static short TabSonbsGetId(this ListViewItem i) => short.Parse(i.SubItems[0].Text);
    public static void TabSonbsSetIsOpen(this ListViewItem i, bool isOpen) => i.SubItems[3].Text = isOpen.ToString();
}
