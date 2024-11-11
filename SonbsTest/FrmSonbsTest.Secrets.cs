namespace SonbsTest;

public partial class FrmSonbsTest
{
    private string RabbitMqHost => _config["RabbitMQ:Host"] ?? throw new Exception();
    private string RabbitMqUser => _config["RabbitMQ:User"] ?? throw new Exception();
    private string RabbitMqPass => _config["RabbitMQ:Pass"] ?? throw new Exception();
    private string GaravotApiUri => _config["Garavot:ApiUri"] ?? throw new Exception();
    private string SonbsHost => _config["Sonbs:Host"] ?? throw new Exception();
}
