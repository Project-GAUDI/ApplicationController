namespace IotedgeV2ApplicationController.Test;

public class ApplicationController_Execute : ApplicationController_TesterBase
{
    private readonly ApplicationController _controller;

    private readonly Mock<IApplicationClientFactory> _appClientFactoryMock = new ();

    private readonly Mock<IMessageSender> _messageSenderMock = new ();

    private readonly EnvironmentInfo _environmentInfo = EnvironmentInfo.CreateInstance();

    public ApplicationController_Execute(ITestOutputHelper output) : base(output)
    {
        List<MyDesiredProperties.Process> processes = [];
        processes.Add(MyDesiredPropertiesCreater.CreateValidProcess("execute-dummy-process-id"));

        var body = "{}";
        Dictionary<string, string> properties = [];

        _controller = new ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, processes, body, properties);
    }

    [Fact(DisplayName = "No.80 引数\"process_name\"がnull")]
    public async Task Case080()
    {
        await Assert.ThrowsAnyAsync<Exception>(() => _controller.Execute(null));
    }

    [Fact(DisplayName = "No.81 引数\"process_name\"が空文字")]
    public async Task Case081()
    {
        await Assert.ThrowsAnyAsync<Exception>(() => _controller.Execute(""));
    }

    [Fact(DisplayName = "No.83 引数\"process_name\"がプロパティ\"ProcessMap\"に含まれない")]
    public async Task Case083()
    {
        await Assert.ThrowsAnyAsync<Exception>(() => _controller.Execute("not-contains-execute-dummy-process-id"));
    }

    [Fact(DisplayName = "No.84 引数\"process_name\"がプロパティ\"ProcessMap\"に含まれる")]
    public async Task Case084()
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
            .Setup(m => m.Disconnect());

        await _controller.Execute("execute-dummy-process-id");
    }

    #region 単体テスト仕様書外の既存テスト
    [Fact(DisplayName = "正常系:引数のprocess_nameがprocessesに存在する値→正常終了")]
    public async void ProcessNameInProcesses_Succeeded()
    {
        ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

        string process_name = "test";

        await result.Execute(process_name);
    }


    [Fact(DisplayName = "異常系:引数のprocess_nameがprocessesに存在しない値→例外")]
    public async void NoProcessNameInProcesses_ExceptionThrown()
    {
        ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

        string process_name = "test_notFound";

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await result.Execute(process_name);
        });
    }

    [Fact(DisplayName = "異常系:引数のprocess_nameがnull→例外")]
    public async void ProcessNameIsNull_ExceptionThrown()
    {
        ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

        string process_name = null;

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await result.Execute(process_name);
        });
    }

    [Fact(DisplayName = "異常系:引数のprocess_nameが空文字→例外")]
    public async void ProcessNameIsEmpty_ExceptionThrown()
    {
        ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

        string process_name = "";

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await result.Execute(process_name);
        });
    }
    #endregion
}
