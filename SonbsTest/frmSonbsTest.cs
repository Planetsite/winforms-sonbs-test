using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Planet.Garavot.HttpApiClient;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sonbs.Sc6200mh.TcpClient;

namespace SonbsTest;

public partial class FrmSonbsTest : Form
{
    private readonly IConfigurationRoot _config;
    private IContentsApi? _contentsApi;
    private IChannel? _eventChannel;
    private ISc6200mhTcpClient? _sc6200mhTcpClient;

    public FrmSonbsTest()
    {
        _config = new ConfigurationBuilder().AddUserSecrets<FrmSonbsTest>().Build();
        InitializeComponent();
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
            accessToken = test.AccessToken;
        }

        try
        {
            using var ghc = new HttpClient();
            ghc.BaseAddress = new(GaravotApiUri);
            ghc.SetBearerToken(accessToken);

            _contentsApi = GaravotHttpApiClientFactory.CreateContentsApi(ghc);
            var bla = await _contentsApi.SearchFrontendAsync(new Planet.Garavot.Contents.ContentSearchRequest
            {
                Page = 1,
                PageSize = 1,
                SortBy = [
                    new Planet.Garavot.Contents.ContentSortCriteria
                    {
                        Direction = Planet.Core.Shared.Request.SortDirection.Ascending,
                        SortField = Planet.Garavot.Contents.ContentSortField.Title
                    }
                ]
            });
        }
        catch (Exception sureErr)
        {
            ;
        }

        var rabbitmqConnectionFactory = new ConnectionFactory()
        {
            HostName = RabbitMqHost,
            UserName = RabbitMqUser,
            Password = RabbitMqPass,
        };

        // TODO rabbitmqConnection e _eventChannel sono ovviamente log-lived e da fare dispose
        var rabbitmqConnection = await rabbitmqConnectionFactory.CreateConnectionAsync();
        _eventChannel = await rabbitmqConnection.CreateChannelAsync();
        await _eventChannel.QueueBindAsync("queue", "exchange", "routing");
        await _eventChannel.BasicPublishAsync("exchange", "routing", true, body: null);

        var consumer = new AsyncEventingBasicConsumer(_eventChannel);
        consumer.ReceivedAsync += RabbitEventReceivedAsync;
        await _eventChannel.BasicConsumeAsync(queue: "task_queue", autoAck: true, consumer);
    }

    private async Task ConnectSonbsAsync()
    {
        _sc6200mhTcpClient = new Sc6200mhTcpClient(null, new Sc6200mhTcpClientSettings(SonbsHost));
        await _sc6200mhTcpClient.StartAsync();
    }

    private async Task RabbitEventReceivedAsync(object sender, BasicDeliverEventArgs @event)
    {
    }

    private async void cmdConnectSonbs_Click(object sender, EventArgs e)
    {
        await _sc6200mhTcpClient.ScanDevicesAsync(default);
    }

    private async void btnConnectGaravot_Click(object sender, EventArgs e)
    {
        await ConnectGaravotAsync();
    }
}