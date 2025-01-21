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

#pragma warning disable CA1848
#pragma warning disable CA2254

public sealed partial class FrmSonbsTest : Form
{
    const string GaravotAccount = "ac114";

    private AnagraficheData? _anagrafiche;
    private readonly IConfigurationRoot _config;
    private IChannel? _eventChannel;
    private IGaravotApi? _garavotApi;
    private readonly StatusStripLogger _logger;
    private IConnection? _rabbitmqConnection;
    private Sc6200mhTcpClient? _sonbsClient;
    private ScanResult? _sonbsDevs;

    public FrmSonbsTest()
    {
        _config = new ConfigurationBuilder().AddUserSecrets<FrmSonbsTest>().Build();
        InitializeComponent();
        _logger = new StatusStripLogger(tslLog);
        cmbVotazioneTipi.SelectedIndex = 0;
        _logger.LogInformation("Ready");
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
        // ma poi cosa me ne faccio dei risultati, li tengo internamente? li mostro?
        // devo dis/attivare votazione? potrei precaricare view risultati
        var x = await _sonbsClient.WaitSignInAsync(_sonbsDevs.T31.Count, cancellation: default);
        // TODO QUESTO CORRISPONDE AD UNO STATO? devo bloccare operazioni finché non finisce
    }

    private async void btnVotazioneEnd_Click(object __, EventArgs _)
    {
        // TODO non posso stoppare una votazione se non è cominciata
        if (_sonbsClient == null) { _logger.LogError("votazione: sonbs non inizializzato"); return; }
        if (_anagrafiche == null) { _logger.LogError("votazione: garavot non inizializzato"); return; }

        var voti = await _sonbsClient.StopVotingAsync();
        _logger.LogInformation($"votazione terminata: {voti} voti");
        viewVoteResult.Items.Clear();
        foreach (var voto in voti)
        {
            var delegateMicWhere = _anagrafiche.DelegatesMic.Where(_ => _.SonbsId == voto.MicId);
            if (delegateMicWhere.Any() == false)
            {
                _logger.LogWarning($"voto non associato a delegate: {voto.MicId}");
                continue;
            }
            var delegateMic = delegateMicWhere.First();
            var dlgt = _anagrafiche.Delegates.Where(_ => _.DelegateId == delegateMic.GaravotId).First();
            viewVoteResult.Items.Add(new ListViewItem([$"{dlgt.FirstName} {dlgt.LastName}", ((int)voto.Vote).ToString()]));
        }
    }

    // TODO dovrei attivare votazione solo se è selezionato un ordine del giorno
    private async void btnVotazioneStart_Click(object __, EventArgs _)
    {
        if (cmbVotazioneTipi.SelectedItem == null) { _logger.LogError("inizia votazione: nessuna votazione selezionata"); return; }
        if (_sonbsClient == null) { _logger.LogError("inizia votazione: sonbs non inizializzato"); return; }
        if (_anagrafiche == null) { _logger.LogError("inizia votazione: garavot non inizializzato"); return; }

        var mode = Enum.Parse<VotingMode>((string)cmbVotazioneTipi.SelectedItem);
        await _sonbsClient.StartVoting2Async(mode, cancellation: default);
        _logger.LogInformation($"votazione {mode} avviata");
        // TODO stato: in votazione
    }

    private async void btnCloseAllMics_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { _logger.LogError("chiudi tutti mic: sonbs non inizializzato"); return; }
        if (_eventChannel == null) { _logger.LogError("chiudi tutti mic: rabbit non inizializzato"); return; }

        var ev = new AllMicrophonesOffEto { Account = GaravotAccount };
        var eventTask = SendEventAsync(ev);

        async Task TurnOffAsync()
        {
            foreach (var sonbsMic in _sonbsDevs.T31)
                await _sonbsClient.EnableMicVoiceAsync(
                    micId: new UnitMessageTarget(
                        sonbsMic.Id.Target == UnitIdType.Wireless
                            ? MessageTarget.SingleWireless
                            : MessageTarget.SingleWired,
                        sonbsMic.Id.Id),
                    on: false,
                    cancellation: default);
        }
        var turnoffTask = TurnOffAsync();

        await eventTask;
        await turnoffTask;

        _logger.LogInformation("inviato evento All Mics Off");
    }

    private async void btnConnectSonbs_Click(object __, EventArgs _)
    {
        await ConnectSonbsAsync();

        _sonbsDevs = await _sonbsClient!.ScanDevicesAsync(default);
        viewSonbs.Items.Clear();
        foreach (var dev in _sonbsDevs.T31)
            viewSonbs.Items.Add(new ListViewItem([dev.Id.Id.ToString(), dev.Id.Target.ToString(), dev.IsChairman.ToString(), "-"]));
    }

    private async void btnSendTalkOff_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { _logger.LogError("chiudi parola: sonbs non inizializzato"); return; }
        if (_eventChannel == null) { _logger.LogError("chiudi parola: rabbit non inizializzato"); return; }
        if (_anagrafiche == null) { _logger.LogError("chiudi parola: garavot non inizializzato"); return; }

        var selected = GetIdFromDelegatesTab();
        if (selected == null) return;

        var seat = _anagrafiche.Seats.Where(_ => _.CurrentSpeakerId == selected.Value.GaravotId).First();
        var ev = new TalkStartedEto
        {
            Account = GaravotAccount,
            SeatNumber = seat.Number,
            MicrophoneStatus = TalkMicrophoneStatus.Close,
        };
        var garavotTask = SendEventAsync(ev);

        var sonbsId = new UnitMessageTarget(
            selected.Value.SonbsId.Target == UnitIdType.Wired ? MessageTarget.SingleWired : MessageTarget.SingleWireless,
            selected.Value.SonbsId.Id);
        var sonbsTask = _sonbsClient.EnableMicVoiceAsync(sonbsId, on: false, cancellation: default);
        await garavotTask;
        await sonbsTask;
    }

    private async void btnSendTalkOn_Click(object __, EventArgs _)
    {
        if (_sonbsClient == null) { _logger.LogError("apri parola: sonbs non inizializzato"); return; }
        if (_eventChannel == null) { _logger.LogError("apri parola: rabbit non inizializzato"); return; }

        var selected = GetIdFromDelegatesTab();
        if (selected == null) return;

        var seat = _anagrafiche.Seats.First(_ => _.CurrentSpeakerId == selected.Value.GaravotId);
        var ev = new TalkStartedEto
        {
            Account = GaravotAccount,
            SeatNumber = seat.Number,
            MicrophoneStatus = TalkMicrophoneStatus.Open,
        };
        var garavotTask = SendEventAsync(ev);

        var sonbsId = new UnitMessageTarget(
            selected.Value.SonbsId.Target == UnitIdType.Wired ? MessageTarget.SingleWired : MessageTarget.SingleWireless,
            selected.Value.SonbsId.Id);
        var sonbsTask = _sonbsClient.EnableMicVoiceAsync(sonbsId, on: true, cancellation: default);

        await garavotTask;
        await sonbsTask;
    }

    private async void btnSendTopic_Click(object __, EventArgs _)
    {
        if (_eventChannel == null) { _logger.LogError("invia topic: rabbit non inizializzato"); return; }
        if (viewOrdini.SelectedItems.Count != 1) { _logger.LogError("topic: selezionare un ordine"); return; }

        var odg = viewOrdini.SelectedItems[0].SubItems[0].Text;
        var ev = new TopicChangedEto { Account = GaravotAccount, Title = odg };
        await SendEventAsync(ev);
    }

    private async void btnSittingStart_Click(object __, EventArgs _)
    {
        if (_eventChannel == null) { _logger.LogError("inizia seduta: rabbit non inizializzato"); return; }

        var ev = new EventStartedEto
        {
            Account = GaravotAccount,
            Legislature = "1",
            Number = "1",
            Title = "test",
        };
        await SendEventAsync(ev);
    }

    private async void btnSittingStop_Click(object __, EventArgs _)
    {
        if (_eventChannel == null) { _logger.LogError("fine seduta: rabbit non inizializzato"); return; }
        var ev = new EventEndedEto
        {
            Account = GaravotAccount,
            Legislature = "1",
            Number = "1"
        };
        await SendEventAsync(ev);
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

            // TEMP
            var delegatesMic = new[]
            {
                new SonbsToGaravot(17, new RecvMessageId(1, UnitIdType.Wireless)), // pieropan
                new SonbsToGaravot(23, new RecvMessageId(2, UnitIdType.Wireless)), // turrini
                new SonbsToGaravot(137, new RecvMessageId(1, UnitIdType.Wired)), // de santis, presidente
                new SonbsToGaravot(29, new RecvMessageId(2, UnitIdType.Wired)), // castellini
                new SonbsToGaravot(35, new RecvMessageId(3, UnitIdType.Wired)), // costantino
            };

            _garavotApi = GaravotApiFactory.Create(ghc);
            var delegateResponse = await _garavotApi.GetAllDelegatesAsync();
            var delegateGroupsResponse = await _garavotApi.GetAllDelegateGroupsAsync();
            var delegateGroupDelegatesResponse = await _garavotApi.SearchDelegateGroupDelegatesAsync(new DelegateGroupDelegateSearchRequest
            {
                Page = 1,
                PageSize = short.MaxValue,
                SortBy = [new SortCriteria<DelegateGroupDelegateSortField> {
                    Direction = SortDirection.Ascending,
                    SortField = DelegateGroupDelegateSortField.Start }]
            });
            var seats = await _garavotApi.SearchSeatsAsync(new()
            {
                Page = 1,
                PageSize = short.MaxValue,
                SortBy = [new SortCriteria<SeatSortField> {
                    Direction = SortDirection.Ascending,
                    SortField = SeatSortField.Number }]
            });

            var viewToGaravot = new List<(long, int)>();
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
                var delegateGroup = delegateGroupQuery.First().Name;

                var seat = seats.Content.Data.Items.FirstOrDefault(_ => _.CurrentSpeakerId == deleg.DelegateId)?.Number
                    ?? "-";

                var added = viewDelegates.Items.Add(new ListViewItem([deleg.FirstName, deleg.LastName, delegateGroup, seat, "-"]));
                viewToGaravot.Add((deleg.DelegateId, added.IndentCount));
            }

            _anagrafiche = new AnagraficheData(delegateResponse.Content.Data.Items, delegateGroupsResponse.Content.Data.Items,
                delegateGroupDelegatesResponse.Content.Data.Items, delegatesMic, seats.Content.Data.Items, viewToGaravot);
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

        _rabbitmqConnection = await rabbitmqConnectionFactory.CreateConnectionAsync();
        _eventChannel = await _rabbitmqConnection.CreateChannelAsync();

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

    private async void FrmSonbsTest_FormClosingAsync(object sender, FormClosingEventArgs _)
    {
        if (_sonbsClient != null) await _sonbsClient.StopAsync();
        if (_eventChannel != null) await _eventChannel.CloseAsync();
        if (_rabbitmqConnection != null) await _rabbitmqConnection.DisposeAsync();
    }

    private static string GetEventRoute<T>(T _)
        => typeof(T).FullName
            ?? throw new System.Diagnostics.UnreachableException();

    private (long GaravotId, RecvMessageId SonbsId)? GetIdFromDelegatesTab()
    {
        const int SonbsTabId = 0;
        const int GaravotTabId = 1;

        switch (tcDelegates.SelectedIndex)
        {
            case SonbsTabId:
                if (viewSonbs.SelectedItems.Count != 1) { _logger.LogError("selezionare un microfono"); return null; }
                var micId = new RecvMessageId(
                    viewSonbs.SelectedItems[0].TabSonbsGetId(),
                    viewSonbs.SelectedItems[0].TabSonbsGetConnection());
                var delegateMic = _anagrafiche.DelegatesMic.Where(_ => _.SonbsId == micId).First();
                return (delegateMic.GaravotId, micId);

            case GaravotTabId:
                if (viewDelegates.SelectedItems.Count != 1) { _logger.LogError("selezionare un delegato"); return null; }
                var viewToGaravot = _anagrafiche!.ViewGaravotToGaravotId.First(_ => _.Index == viewDelegates.SelectedIndices[0]);
                var micId2 = _anagrafiche.DelegatesMic.Where(_ => _.GaravotId == viewToGaravot.GaravotId).First();
                return (viewToGaravot.GaravotId, micId2.SonbsId);

            default:
                throw new System.Diagnostics.UnreachableException();
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
            if (viewSonbs.Items[itemId].TabSonbsGetId() == eventData.MicId.Id
                && viewSonbs.Items[itemId].TabSonbsGetConnection() == eventData.MicId.Target)
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
        if (_anagrafiche == null) return;

        var foundDelegate = _anagrafiche.DelegatesMic.FirstOrDefault(_ => _.SonbsId == eventData.MicId);
        var seat = _anagrafiche.Seats.FirstOrDefault(_ => _.CurrentSpeakerId == foundDelegate.GaravotId);
        var ev = new TalkStartedEto
        {
            Account = GaravotAccount,
            SeatNumber = foundDelegate.GaravotId.ToString(),
            MicrophoneStatus = eventData.IsOpen ? TalkMicrophoneStatus.Open : TalkMicrophoneStatus.Close,
        };
        await SendEventAsync(ev);
    }

    private async Task SonbsPowerEmittedAsync(Planet.Devices.Power.PowerStatus status)
    {
        if (status == Planet.Devices.Power.PowerStatus.Stopped) _sonbsClient = null;
        _logger.LogWarning($"evento sonbs: power {status}");
    }

    private void viewOrdini_SelectedIndexChanged(object __, EventArgs _)
    {
        btnSendTopic.Enabled = viewOrdini.SelectedItems.Count == 1;
    }

    private void viewOrdini_DoubleClick(object _, EventArgs __)
    {
        var odg = viewOrdini.SelectedItems[0].SubItems[0].Text;
        var editedOdg = Microsoft.VisualBasic.Interaction.InputBox("Modifica ordine del giorno", "edita ordine del giorno", odg);
        if (string.IsNullOrEmpty(editedOdg)) return;
        viewOrdini.SelectedItems[0].SubItems[0].Text = editedOdg;
    }

    private void cmsMenuAddOdg_Click(object _, EventArgs __)
    {
        var newOdg = Microsoft.VisualBasic.Interaction.InputBox("Nuovo ordine del giorno", "inserisci ordine del giorno");
        viewOrdini.Items.Add(newOdg);
    }
}

sealed record AnagraficheData(
    IEnumerable<DelegateFeDto> Delegates,
    IEnumerable<DelegateGroupFeDto> DelegateGroups,
    IEnumerable<GovernmentBodyDelegateDto> DelegateGroupDelegates,
    SonbsToGaravot[] DelegatesMic,
    IEnumerable<SeatDto> Seats,
    ICollection<(long GaravotId, int Index)> ViewGaravotToGaravotId);

record struct SonbsToGaravot(long GaravotId, RecvMessageId SonbsId);

file static class FrmSonbsTestExtensions
{
    public static int TabDelegatesGetSeat(this ListViewItem i) => int.Parse(i.SubItems[3].Text);
    public static UnitIdType TabSonbsGetConnection(this ListViewItem i) => i.SubItems[1].Text == nameof(UnitIdType.Wireless) ? UnitIdType.Wireless : UnitIdType.Wired;
    public static short TabSonbsGetId(this ListViewItem i) => short.Parse(i.SubItems[0].Text);
    public static void TabSonbsSetIsOpen(this ListViewItem i, bool isOpen) => i.SubItems[3].Text = isOpen.ToString();
}

// TODO try-catch

// architettura
// dovremmo quindi avere rabbit che invia eventi metadati sia a garavot che director?
// che significa avere exchange direct con bindate queue garavot + queue director con routing key stessi eventi
// però non penso di poterlo fare tra istanze diverse di rabbit
// quindi dovrei connettere tutti ad uno stesso rabbit? tipo 205 o demo stesso?

// TODO devo fare un client keycloak tipo servizio/confidential per ac114

// TODO pulsanti microfoni dovrebbero essere abilitati solo se app è connessa?
// al momento chiudi tutti è on di default
