using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Planet.Core.Shared.Request;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sonbs.Sc6200mh.TcpClient;

namespace SonbsTest;

public partial class FrmSonbsTest : Form
{
    private readonly IConfigurationRoot _config;
    private IChannel? _eventChannel;
    private IGaravotApi? _garavotApi;
    private ISc6200mhTcpClient? _sc6200mhTcpClient;

    public FrmSonbsTest()
    {
        _config = new ConfigurationBuilder().AddUserSecrets<FrmSonbsTest>().Build();
        InitializeComponent();
    }

    private async Task ConnectGaravotAsync()
    {
        await ConnectGaravot();

        await ConnectRabbit();

        async Task ConnectRabbit()
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
        }

        async Task ConnectGaravot()
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
    }

    private async Task ConnectSonbsAsync()
    {
        _sc6200mhTcpClient = new Sc6200mhTcpClient(null, new Sc6200mhTcpClientSettings(SonbsHost));
        await _sc6200mhTcpClient.StartAsync();
    }

    //private async Task RabbitEventReceivedAsync(object sender, BasicDeliverEventArgs @event){}

    private async void cmdConnectSonbs_Click(object sender, EventArgs e)
    {
        await ConnectSonbsAsync();
        var devs = await _sc6200mhTcpClient.ScanDevicesAsync(default);
        viewSonbs.Clear();
        foreach (var dev in devs.T31)
            viewSonbs.Items.Add(new ListViewItem([dev.Id.ToString(), dev.IsChairman.ToString(), dev.MicState.ToString(), string.Empty]));
    }

    private async void btnConnectGaravot_Click(object sender, EventArgs e)
    {
        await ConnectGaravotAsync();
    }
}
