using Newtonsoft.Json;

namespace IotedgeV2ApplicationController.Test;

public class ApplicationController_ExecuteProcess : ApplicationController_TesterBase
{
    private readonly ApplicationController _controller;

    private readonly Mock<IApplicationClientFactory> _appClientFactoryMock = new ();

    private readonly Mock<IMessageSender> _messageSenderMock = new ();

    private readonly EnvironmentInfo _environmentInfo = EnvironmentInfo.CreateInstance();

    public ApplicationController_ExecuteProcess(ITestOutputHelper output) : base(output)
    {
        List<MyDesiredProperties.Process> processes = [];
        processes.Add(MyDesiredPropertiesCreater.CreateValidProcess("execute-dummy-process-id"));

        var body = "{}";
        Dictionary<string, string> properties = [];

        _controller = new ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, processes, body, properties);
    }

    [Fact(DisplayName = "No.85 クライアントの取得(GetApplicationClient)の結果がnull")]
    public async Task Case085()
    {
        #pragma warning disable CS8604
        _appClientFactoryMock
            .Setup(m => m.CreateInstance(It.IsAny<MyDesiredProperties.Process.AppSetting.Protocol>()))
            .Returns(null as IApplicationClient);
        #pragma warning restore CS8604

        var proc = MyDesiredPropertiesCreater.CreateValidProcess("dummy-process-id");
        var type = _controller.GetType();
        var mi = type.GetMethod("ExecuteProcess", BindingFlags.Instance | BindingFlags.NonPublic);
        await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            if (mi!.Invoke(_controller, [proc]) is Task<string> executed) await executed;
        });
    }

    [Fact(DisplayName = "No.86 クライアントの初期化(Initialize)の結果がfalse")]
    public async Task Case086()
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
        await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            if (mi!.Invoke(_controller, [proc]) is Task<string> executed) await executed;
        });
    }

    [Fact(DisplayName = "No.87 WebAPPへの接続(Connect)の結果がfalse")]
    public async Task Case087()
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
        await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            if (mi!.Invoke(_controller, [proc]) is Task<string> executed) await executed;
        });
    }

    [Fact(DisplayName = "No.88 リクエスト送信(SendRequest)で例外が発生")]
    public async Task Case088()
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
        await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            if (mi!.Invoke(_controller, [proc]) is Task<string> executed) await executed;
        });
    }

    [Fact(DisplayName = "No.89 WebAPPからの切断(Disconnect)で例外が発生")]
    public async Task Case089()
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
        await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            if (mi!.Invoke(_controller, [proc]) is Task<string> executed) await executed;
        });
    }

    [Fact(DisplayName = "No.90 レスポンスボディがデシリアライズできない")]
    public async Task Case090()
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
        await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            if (mi!.Invoke(_controller, [proc]) is Task<string> executed) await executed;
        });
    }

    [Fact(DisplayName = "No.91 正常終了する")]
    public async Task Case091()
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

    #region 単体テスト仕様書外の既存テスト
    [Fact(DisplayName = "正常系:Excuteの引数のprocess_nameがprocessesに存在する値→正常終了")]
    public async void ProcessNameInProcesses_Succeeded()
    {
        ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

        string process_name = "test";

        await result.Execute(process_name);
    }


    [Fact(DisplayName = "異常系:Factoryで生成したApplicatioClient がnull→例外")]
    public async void ApplicatioClientFactoryReturnsNull_ExceptionThrown()
    {
        myDesiredProperties.processes[0].application.type = MyDesiredProperties.Process.AppSetting.Protocol.NotSelected;

        ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

        string process_name = "test";

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await result.Execute(process_name);
        });
    }

    [Fact(DisplayName = "正常系:ApplicationClientのメソッドでエラーが起きない設定で実行→正常終了")]
    public async void AllParamsIsDefault_Succeeded()
    {
        ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

        string process_name = "test";

        await result.Execute(process_name);
    }

    [Fact(DisplayName = "異常系:Initializeでエラー→例外")]
    public async void AppClientInitializeIsError_ExceptionThrown()
    {
        factory.SetErrorMethod("Initialize");
        ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

        string process_name = "test";

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await result.Execute(process_name);
        });
    }

    [Fact(DisplayName = "異常系:Connectでエラー→例外")]
    public async void AppClientConnectIsError_ExceptionThrown()
    {
        factory.SetErrorMethod("Connect");
        ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

        string process_name = "test";

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await result.Execute(process_name);
        });
    }

    [Fact(DisplayName = "異常系:SendRequestでエラー→例外")]
    public async void AppClientSendRequestIsError_ExceptionThrown()
    {
        factory.SetErrorMethod("SendRequest");
        ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

        string process_name = "test";

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await result.Execute(process_name);
        });
    }

    [Fact(DisplayName = "異常系:Disconnectでエラー→例外")]
    public async void AppClientDisconnectIsError_ExceptionThrown()
    {
        factory.SetErrorMethod("Disconnect");
        ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

        string process_name = "test";

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await result.Execute(process_name);
        });
    }

    [Fact(DisplayName = "異常系:レスポンスがjson形式でない場合→例外")]
    public async void ResponseIsNotJson_ExceptionThrown()
    {
        factory.SetErrorMethod("SendRequest:ReturnNonJson");
        ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

        string process_name = "test";

        var ex = await Assert.ThrowsAsync<Exception>(async () =>
        {
            await result.Execute(process_name);
        });
        Assert.Contains("Response is not JSON format.", ex.Message);
    }
    #endregion
}
