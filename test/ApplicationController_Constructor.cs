using Newtonsoft.Json;

namespace IotedgeV2ApplicationController.Test;

public class ApplicationController_Constructor(ITestOutputHelper output) : ApplicationController_TesterBase(output)
{
    private readonly Mock<IApplicationClientFactory> _appClientFactoryMock = new ();

    private readonly Mock<IMessageSender> _messageSenderMock = new ();

    private readonly EnvironmentInfo _environmentInfo = EnvironmentInfo.CreateInstance();

    [Fact(DisplayName = "No.73 引数\"factory\"がnull")]
    public void Case073()
    {
        List<MyDesiredProperties.Process> processes = [];
        var body = "{}";
        Dictionary<string, string> properties = [];

        Assert.ThrowsAny<Exception>(() =>
            new ApplicationController(null, _messageSenderMock.Object, _environmentInfo, processes, body, properties));
    }

    [Fact(DisplayName = "No.74 引数\"sender\"がnull")]
    public void Case074()
    {
        List<MyDesiredProperties.Process> processes = [];
        var body = "{}";
        Dictionary<string, string> properties = [];

        Assert.ThrowsAny<Exception>(() =>
            new ApplicationController(_appClientFactoryMock.Object, null, _environmentInfo, processes, body, properties));
    }

    [Fact(DisplayName = "No.75 引数\"env\"がnull")]
    public void Case075()
    {
        List<MyDesiredProperties.Process> processes = [];
        var body = "{}";
        Dictionary<string, string> properties = [];

        Assert.ThrowsAny<Exception>(() =>
            new ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, null, processes, body, properties));
    }

    [Fact(DisplayName = "No.76 引数\"body\"がnull")]
    public void Case076()
    {
        List<MyDesiredProperties.Process> processes = [];
        Dictionary<string, string> properties = [];

        Assert.ThrowsAny<Exception>(() =>
            new ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, processes, null, properties));
    }

    [Fact(DisplayName = "No.77 引数\"body\"がJSON形式ではない")]
    public void Case077()
    {
        List<MyDesiredProperties.Process> processes = [];
        var body = "invalid schema body";
        Dictionary<string, string> properties = [];

        Assert.ThrowsAny<Exception>(() =>
            new ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, processes, body, properties));
    }

    [Fact(DisplayName = "No.78 引数\"properties\"がnull")]
    public void Case078()
    {
        List<MyDesiredProperties.Process> processes = [];
        var body = "{}";

        Assert.ThrowsAny<Exception>(() =>
            new ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, processes, body, null));
    }

    [Fact(DisplayName = "No.79 正常終了する")]
    public void Case079()
    {
        List<MyDesiredProperties.Process> processes = [];
        processes.Add(MyDesiredPropertiesCreater.CreateValidProcess("dummy-process-id"));

        var body = "{}";
        Dictionary<string, string> properties = [];

        var controller = new ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, processes, body, properties);

        var type = controller.GetType();
        object? getPropertyFunc(string prop)
        {
            var pi = type.GetProperty(prop, BindingFlags.Instance | BindingFlags.NonPublic);
            return pi?.GetValue(controller);
        }

        Assert.Equal(_appClientFactoryMock.Object, getPropertyFunc("MyClientFactory"));
        Assert.Equal(_messageSenderMock.Object, getPropertyFunc("MySender"));
        Assert.Equal(_environmentInfo, getPropertyFunc("MyEnvInfo"));
        Assert.Equal(body, getPropertyFunc("Body"));
        Assert.Equal(properties, getPropertyFunc("Properties"));

        var casted = processes.ToDictionary(p => p.process_name, p=> p);
        var expectedProcessMap = JsonConvert.SerializeObject(casted);
        var actualProcessMap = JsonConvert.SerializeObject(getPropertyFunc("ProcessMap"));
        _output.WriteLine($"expected process map: {expectedProcessMap}");
        _output.WriteLine($"actual process map: {actualProcessMap}");
        Assert.Equal(expectedProcessMap, actualProcessMap);
    }

    #region 単体テスト仕様書外の既存テスト
    [Fact(DisplayName = "正常系:実行→ApplicationControllerインスタンス生成")]
    public void AllParamsIsDefault_InstanceCreated()
    {
        ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);
        Assert.IsAssignableFrom<ApplicationController>(result);
    }

    [Fact(DisplayName = "異常系:factoryがnull→例外")]
    public void FactoryIsNull_ExceptionThrown()
    {
        Assert.Throws<Exception>(() =>
        {
            ApplicationController result = new ApplicationController(null,sender, env, myDesiredProperties.processes, body, properties);
        });
    }

    [Fact(DisplayName = "異常系:envがnull→例外")]
    public void EnvIsNull_ExceptionThrown()
    {
        Assert.Throws<Exception>(() =>
        {
            ApplicationController result = new ApplicationController(factory,sender, null, myDesiredProperties.processes, body, properties);
        });
    }

    [Fact(DisplayName = "異常系:bodyがnull→例外")]
    public void BodyIsNull_ExceptionThrown()
    {
        Assert.Throws<Exception>(() =>
        {
            ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, null, properties);
        });
    }

    [Fact(DisplayName = "異常系:bodyがjson形式でない→例外")]
    public void BodyIsNotJsonFormat_ExceptionThrown()
    {
        body = "Bad format.";
        var ex = Assert.Throws<Exception>(() =>
        {
            ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);
        });
        Assert.Equal("body is not JSON format.", ex.Message);
    }


    [Fact(DisplayName = "異常系:propertiesがnull→例外")]
    public void PropertiesIsNull_ExceptionThrown()
    {
        Assert.Throws<Exception>(() =>
        {
            ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, null);
        });
    }
    #endregion
}
