namespace IotedgeV2ApplicationController.Test;

public class ApplicationController_GetApplicationClient : ApplicationController_TesterBase
{
    private readonly ApplicationController _controller;

    private readonly IApplicationClientFactory _appClientFactory = new ApplicationClientFactory();

    private readonly Mock<IMessageSender> _messageSenderMock = new ();

    private readonly EnvironmentInfo _environmentInfo = EnvironmentInfo.CreateInstance();

    public ApplicationController_GetApplicationClient(ITestOutputHelper output) : base(output)
    {
        List<MyDesiredProperties.Process> processes = [];
        processes.Add(MyDesiredPropertiesCreater.CreateValidProcess("execute-dummy-process-id"));

        var body = "{}";
        Dictionary<string, string> properties = [];

        _controller = new ApplicationController(_appClientFactory, _messageSenderMock.Object, _environmentInfo, processes, body, properties);
    }

    [Fact(DisplayName = "No.92 protocolが\"http\"")]
    public void Case092()
    {
        var protocol = MyDesiredProperties.Process.AppSetting.Protocol.http;

        var type = _controller.GetType();
        var mi = type.GetMethod("GetApplicationClient", BindingFlags.Instance | BindingFlags.NonPublic);

        var client = mi!.Invoke(_controller, [protocol]);
        Assert.NotNull(client);
        Assert.IsType<HttpApplicationClient>(client);
    }

    [Fact(DisplayName = "No.93 protocolがNotSelected")]
    public void Case093()
    {
        var protocol = MyDesiredProperties.Process.AppSetting.Protocol.NotSelected;

        var type = _controller.GetType();
        var mi = type.GetMethod("GetApplicationClient", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.ThrowsAny<Exception>(() => {
            try {
                mi!.Invoke(_controller, [protocol]);
            } catch (TargetInvocationException tie) {
                throw tie.InnerException!;
            }
        });
    }

    [Fact(DisplayName = "No.94 protocolが\"http\"/\"NotSelected\"以外")]
    public void Case094()
    {
        var protocol = (MyDesiredProperties.Process.AppSetting.Protocol)(-1);

        var type = _controller.GetType();
        var mi = type.GetMethod("GetApplicationClient", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.ThrowsAny<Exception>(() => {
            try {
                mi!.Invoke(_controller, [protocol]);
            } catch (TargetInvocationException tie) {
                throw tie.InnerException!;
            }
        });
    }

    #region 単体テスト仕様書外の既存テスト
    [Fact(DisplayName = "正常系:実行→ApplicationClientインスタンス生成")]
    public async void GetApplicationClient_InstanceCreated()
    {
        ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

        string process_name = "test";

        await result.Execute(process_name);
    }
    #endregion
}
