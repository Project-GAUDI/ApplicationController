using IotedgeV2ApplicationController;
using Newtonsoft.Json;

namespace ApplicationController.Test;

[Collection(nameof(CollectionAttribute))]
public class ApplicationController_Constructor(ITestOutputHelper output) : ApplicationController_TesterBase(output)
{
    private readonly Mock<IApplicationClientFactory> _appClientFactoryMock = new ();

    private readonly Mock<IMessageSender> _messageSenderMock = new ();

    private readonly EnvironmentInfo _environmentInfo = EnvironmentInfo.CreateInstance();

    private readonly List<MyDesiredProperties.Process> _processes = [];

    private readonly string _body = "{}";

    private readonly Dictionary<string, string> _properties = [];

    [Fact(DisplayName = "Case115_factoryがnull")]
    public void Case115()
    {
        var ex = Assert.ThrowsAny<Exception>(() => 
            new IotedgeV2ApplicationController.ApplicationController(null,_messageSenderMock.Object, _environmentInfo, _processes, _body, _properties));
        Assert.Contains("factory is null.", ex.Message);
    }

    [Fact(DisplayName = "Case116_senderがnull")]
    public void Case116()
    {
        var ex = Assert.ThrowsAny<Exception>(() => 
            new IotedgeV2ApplicationController.ApplicationController(_appClientFactoryMock.Object, null, _environmentInfo, _processes, _body, _properties));
        Assert.Contains("sender is null.", ex.Message);
    }

    [Fact(DisplayName = "Case117_envがnull")]
    public void Case117()
    {
        var ex = Assert.ThrowsAny<Exception>(() =>
            new IotedgeV2ApplicationController.ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, null, _processes, _body, _properties));
        Assert.Contains("env is null.", ex.Message);
    }

    [Fact(DisplayName = "Case118_bodyがnull")]
    public void Case118()
    {
        var ex = Assert.ThrowsAny<Exception>(() =>
            new IotedgeV2ApplicationController.ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, _processes, null, _properties));
        Assert.Contains("body is null.", ex.Message);
    }

    [Fact(DisplayName = "Case119_bodyが非JSON形式")]
    public void Case119()
    {
        var ex = Assert.ThrowsAny<Exception>(() =>
            new IotedgeV2ApplicationController.ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, _processes, "test", _properties));
        Assert.Contains("body is not JSON format.", ex.Message);
    }

    [Fact(DisplayName = "Case120_propertiesがnull")]
    public void Case120()
    {
        var ex = Assert.ThrowsAny<Exception>(() =>
            new IotedgeV2ApplicationController.ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, _processes, _body, null));
        Assert.Contains("properties is null.", ex.Message);
    }

    [Fact(DisplayName = "Case121_factoryがnullでない、かつ、senderがnullでない、かつ、envがnullでない、かつ、bodyがJSON形式、かつ、propertiesがnullでない")]
    public void Case121()
    {
        _processes.Add(new MyDesiredProperties.Process { process_name = "dummy-process-id" });

        var controller = new IotedgeV2ApplicationController.ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, _processes, _body, _properties);

        var type = controller.GetType();
        object? getPropertyFunc(string prop)
        {
            var pi = type.GetProperty(prop, BindingFlags.Instance | BindingFlags.NonPublic);
            return pi?.GetValue(controller);
        }

        Assert.Equal(_appClientFactoryMock.Object, getPropertyFunc("MyClientFactory"));
        Assert.Equal(_messageSenderMock.Object, getPropertyFunc("MySender"));
        Assert.Equal(_environmentInfo, getPropertyFunc("MyEnvInfo"));
        Assert.Equal(_body, getPropertyFunc("Body"));
        Assert.Equal(_properties, getPropertyFunc("Properties"));

        var casted = _processes.ToDictionary(p => p.process_name, p=> p);
        var expectedProcessMap = JsonConvert.SerializeObject(casted);
        var actualProcessMap = JsonConvert.SerializeObject(getPropertyFunc("ProcessMap"));
        Assert.Equal(expectedProcessMap, actualProcessMap);
    }
}