using TICO.GAUDI.Commons.Test;
using IotedgeV2ApplicationController;

namespace ApplicationController.Test;

public class ApplicationController_TesterBase(ITestOutputHelper output)
{
    protected readonly ITestOutputHelper _output = output;
    private string _status = "";
    protected TestApplicationClientFactory factory = new();
    protected StubMessageSender sender = new();
    protected EnvironmentInfo env = EnvironmentInfo.CreateInstance();
    protected StubModuleClient client = new();
    protected string body = "{\"A\":\"ABC\"}";
    protected string RequiredBody
    {
        get
        {
            return $"{{\"status\":\"{_status}\",\"url\":\"http://aaa.bbb/ccc\",\"timeout\":10,\"A\":\"ABC\"}}";
        }
        private set { }
    }

    protected IDictionary<string, string> properties = new Dictionary<string, string>();
    protected MyDesiredProperties myDesiredProperties = MyDesiredPropertiesCreater.Create();

    public void SetRequiredStatus(string status)
    {
        _status = status;
    }
}