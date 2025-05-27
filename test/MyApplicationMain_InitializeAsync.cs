namespace IotedgeV2ApplicationController.Test;

[Collection(nameof(CollectionAttribute))]
public class MyApplicationMain_InitializeAsync(ITestOutputHelper output)
{
    private readonly MyApplicationMain _app = new ();

    private readonly ITestOutputHelper _output = output;

    [Fact(DisplayName = "No.1 CreateInstanceが正常終了する")]
    public async Task Case001()
    {
        var ret = await _app.InitializeAsync();

        Assert.True(ret);
    }

    [Fact(DisplayName = "No.3 環境変数: \"HttpTimeout\"が存在しない")]
    public async void Case003()
    {
        Environment.SetEnvironmentVariable("HttpTimeout", null);

        await _app.InitializeAsync();

        var type = _app.GetType();
        var pi = type.GetProperty("MyEnvInfo", BindingFlags.Static | BindingFlags.NonPublic);

        var envInfo = (EnvironmentInfo)pi!.GetValue(null)!;
        Assert.Null(envInfo!.HttpTimeout);
    }

    [Fact(DisplayName = "No.4 環境変数: \"HttpTimeout\"が設定されている")]
    public async void Case004()
    {
        var random = new Random();
        var testHttpTimeout = random.Next(1, 101).ToString();
        _output.WriteLine($"HttpTimeoutに設定するテスト値: {testHttpTimeout}");

        Environment.SetEnvironmentVariable("HttpTimeout", testHttpTimeout);

        await _app.InitializeAsync();

        var type = _app.GetType();
        var pi = type.GetProperty("MyEnvInfo", BindingFlags.Static | BindingFlags.NonPublic);

        var envInfo = (EnvironmentInfo)pi!.GetValue(null)!;
        Assert.Equal(testHttpTimeout, envInfo!.HttpTimeout);
    }
}
