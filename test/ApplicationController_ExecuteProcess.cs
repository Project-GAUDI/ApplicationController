using Newtonsoft.Json;
using IotedgeV2ApplicationController;

namespace ApplicationController.Test;

[Collection(nameof(CollectionAttribute))]
public class ApplicationController_ExecuteProcess : ApplicationController_TesterBase
{
    private readonly IotedgeV2ApplicationController.ApplicationController _controller;

    private readonly Mock<IApplicationClientFactory> _appClientFactoryMock = new ();

    private readonly Mock<IMessageSender> _messageSenderMock = new ();

    private readonly EnvironmentInfo _environmentInfo = EnvironmentInfo.CreateInstance();

    public ApplicationController_ExecuteProcess(ITestOutputHelper output) : base(output)
    {
        List<MyDesiredProperties.Process> processes = [];
        processes.Add(MyDesiredPropertiesCreater.CreateValidProcess("execute-dummy-process-id"));

        var body = "{}";
        Dictionary<string, string> properties = [];

        _controller = new IotedgeV2ApplicationController.ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, processes, body, properties);
    }

    [Fact(DisplayName = "Case125_GetApplicationClientでIApplicationClientの取得に失敗する（DP:application.typeにNotSelectedを設定）")]
    public async Task Case125()
    {
        #pragma warning disable CS8604
        _appClientFactoryMock
            .Setup(m => m.CreateInstance(MyDesiredProperties.Process.AppSetting.Protocol.NotSelected))
            .Returns(null as IApplicationClient);
        #pragma warning restore CS8604

        var proc = MyDesiredPropertiesCreater.CreateValidProcess("dummy-process-id");
        var type = _controller.GetType();
        var mi = type.GetMethod("ExecuteProcess", BindingFlags.Instance | BindingFlags.NonPublic);
        var ex = await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            if (mi!.Invoke(_controller, [proc]) is Task<string> executed) await executed;
        });
        Assert.Contains("ApplicationClient generate failed.", ex.Message);
    }

    [Fact(DisplayName = "Case126_appClient.Initializeでfalseが返される")]
    public async Task Case126()
    {
        var _appClientMock = new Mock<IApplicationClient>();
        _appClientFactoryMock
            .Setup(m => m.CreateInstance(It.IsAny<MyDesiredProperties.Process.AppSetting.Protocol>()))
            .Returns(_appClientMock.Object);
        _appClientMock
            .Setup(m => m.Initialize(
                It.IsAny<EnvironmentInfo>(),
                It.IsAny<MyDesiredProperties.Process.AppSetting>(),
                It.IsAny<string>(),
                It.IsAny<IDictionary<string, string>>()))
            .Returns(false);

        var proc = MyDesiredPropertiesCreater.CreateValidProcess("dummy-process-id");
        var type = _controller.GetType();
        var mi = type.GetMethod("ExecuteProcess", BindingFlags.Instance | BindingFlags.NonPublic);
        var ex = await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            if (mi!.Invoke(_controller, [proc]) is Task<string> executed) await executed;
        });
        Assert.Contains("IApplicationClient Initialize is Failed.", ex.Message);
    }

    [Fact(DisplayName = "Case127_appClient.Connectでfalseが返される")]
    public async Task Case127()
    {
        var _appClientMock = new Mock<IApplicationClient>();
        _appClientFactoryMock
            .Setup(m => m.CreateInstance(It.IsAny<MyDesiredProperties.Process.AppSetting.Protocol>()))
            .Returns(_appClientMock.Object);

        _appClientMock
            .Setup(m => m.Initialize(
                It.IsAny<EnvironmentInfo>(),
                It.IsAny<MyDesiredProperties.Process.AppSetting>(),
                It.IsAny<string>(),
                It.IsAny<IDictionary<string, string>>()))
            .Returns(true);
        _appClientMock
            .Setup(m => m.Connect())
            .Returns(false);

        var proc = MyDesiredPropertiesCreater.CreateValidProcess("dummy-process-id");
        var type = _controller.GetType();
        var mi = type.GetMethod("ExecuteProcess", BindingFlags.Instance | BindingFlags.NonPublic);
        var ex = await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            if (mi!.Invoke(_controller, [proc]) is Task<string> executed) await executed;
        });
        Assert.Contains("IApplicationClient Connect is Failed.", ex.Message);
    }

    [Fact(DisplayName = "Case128_appClient.SendRequestでExceptionがthrowされる")]
    public async Task Case128()
    {
        var _appClientMock = new Mock<IApplicationClient>();
        _appClientFactoryMock
            .Setup(m => m.CreateInstance(It.IsAny<MyDesiredProperties.Process.AppSetting.Protocol>()))
            .Returns(_appClientMock.Object);
        _appClientMock
            .Setup(m => m.Initialize(
                It.IsAny<EnvironmentInfo>(),
                It.IsAny<MyDesiredProperties.Process.AppSetting>(),
                It.IsAny<string>(),
                It.IsAny<IDictionary<string, string>>()))
            .Returns(true);
        _appClientMock
            .Setup(m => m.Connect())
            .Returns(true);
        _appClientMock
            .Setup(m => m.SendRequest(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Throws Mock Exception from SendRequest."));

        var proc = MyDesiredPropertiesCreater.CreateValidProcess("dummy-process-id");
        var type = _controller.GetType();
        var mi = type.GetMethod("ExecuteProcess", BindingFlags.Instance | BindingFlags.NonPublic);
        var ex = await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            if (mi!.Invoke(_controller, [proc]) is Task<string> executed) await executed;
        });
        Assert.Contains("IApplicationClient SendRequest is Failed.", ex.Message);
    }

    [Fact(DisplayName = "Case129_appClient.DisconnectでExceptionがthrowされる")]
    public async Task Case129()
    {
        var _appClientMock = new Mock<IApplicationClient>();
        _appClientFactoryMock
            .Setup(m => m.CreateInstance(It.IsAny<MyDesiredProperties.Process.AppSetting.Protocol>()))
            .Returns(_appClientMock.Object);
        _appClientMock
            .Setup(m => m.Initialize(
                It.IsAny<EnvironmentInfo>(),
                It.IsAny<MyDesiredProperties.Process.AppSetting>(),
                It.IsAny<string>(),
                It.IsAny<IDictionary<string, string>>()))
            .Returns(true);
        _appClientMock
            .Setup(m => m.Connect())
            .Returns(true);
        _appClientMock
            .Setup(m => m.SendRequest(It.IsAny<string>()))
            .ReturnsAsync("{}");
        _appClientMock
            .Setup(m => m.Disconnect())
            .Throws(new Exception("Throws Mock Exception from Disconnect."));

        var proc = MyDesiredPropertiesCreater.CreateValidProcess("dummy-process-id");
        var type = _controller.GetType();
        var mi = type.GetMethod("ExecuteProcess", BindingFlags.Instance | BindingFlags.NonPublic);
        var ex = await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            if (mi!.Invoke(_controller, [proc]) is Task<string> executed) await executed;
        });
        Assert.Contains("IApplicationClient Disconnect is Failed.", ex.Message);
    }

    [Fact(DisplayName = "Case130_appClient.SendRequestの戻り値が非JSON形式")]
    public async Task Case130()
    {
        var _appClientMock = new Mock<IApplicationClient>();
        _appClientFactoryMock
            .Setup(m => m.CreateInstance(It.IsAny<MyDesiredProperties.Process.AppSetting.Protocol>()))
            .Returns(_appClientMock.Object);
        _appClientMock
            .Setup(m => m.Initialize(
                It.IsAny<EnvironmentInfo>(),
                It.IsAny<MyDesiredProperties.Process.AppSetting>(),
                It.IsAny<string>(),
                It.IsAny<IDictionary<string, string>>()))
            .Returns(true);
        _appClientMock
            .Setup(m => m.Connect())
            .Returns(true);
        _appClientMock
            .Setup(m => m.SendRequest(It.IsAny<string>()))
            .ReturnsAsync("Can not deserialize data");
        _appClientMock
            .Setup(m => m.Disconnect());

        var proc = MyDesiredPropertiesCreater.CreateValidProcess("dummy-process-id");
        var type = _controller.GetType();
        var mi = type.GetMethod("ExecuteProcess", BindingFlags.Instance | BindingFlags.NonPublic);
        var ex = await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            if (mi!.Invoke(_controller, [proc]) is Task<string> executed) await executed;
        });
        Assert.Contains("Response is not JSON.", ex.Message);
    }

    [Fact(DisplayName = "Case131_GetApplicationClient、appClient.Initialize、appClient.Connect、appClient.SendRequest、appClient.Disconnectの処理に成功する")]
    public async Task Case131()
    {
        var _appClientMock = new Mock<IApplicationClient>();
        _appClientFactoryMock
            .Setup(m => m.CreateInstance(It.IsAny<MyDesiredProperties.Process.AppSetting.Protocol>()))
            .Returns(_appClientMock.Object);
        _appClientMock
            .Setup(m => m.Initialize(
                It.IsAny<EnvironmentInfo>(),
                It.IsAny<MyDesiredProperties.Process.AppSetting>(),
                It.IsAny<string>(),
                It.IsAny<IDictionary<string, string>>()))
            .Returns(true);
        _appClientMock
            .Setup(m => m.Connect())
            .Returns(true);
        
        var sampleResponse = JsonConvert.SerializeObject(new {
            TestKey1 = "Valid Response Sample"
        });
        _appClientMock
            .Setup(m => m.SendRequest(It.IsAny<string>()))
            .ReturnsAsync(sampleResponse);
        _appClientMock
            .Setup(m => m.Disconnect());

        var proc = MyDesiredPropertiesCreater.CreateValidProcess("dummy-process-id");
        var type = _controller.GetType();
        var mi = type.GetMethod("ExecuteProcess", BindingFlags.Instance | BindingFlags.NonPublic);
        var result = await (mi!.Invoke(_controller, [proc]) as Task<string>)!;
        Assert.Equal(sampleResponse, result);
    }
}
