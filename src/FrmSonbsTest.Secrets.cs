namespace SonbsTest;

public partial class FrmSonbsTest
{
    private string RabbitMqExchange => _config["RabbitMQ:Exchange"] ?? throw new Exception();
    private string RabbitMqHost => _config["RabbitMQ:Host"] ?? throw new Exception();
    private string RabbitMqPass => _config["RabbitMQ:Pass"] ?? throw new Exception();
    private string RabbitMqUser => _config["RabbitMQ:User"] ?? throw new Exception();
    private string GaravotApiUri => _config["Garavot:ApiUri"] ?? throw new Exception();
    private string GaravotClientId => _config["Garavot:ClientId"] ?? throw new Exception();
    private string GaravotClientSecret => _config["Garavot:ClientSecret"] ?? throw new Exception();
    private string GaravotOpenIdUrl => _config["Garavot:OpenIdUrl"] ?? throw new Exception();
    private string SonbsHost => _config["Sonbs:Host"] ?? throw new Exception();
}
