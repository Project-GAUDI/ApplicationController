using IotedgeV2ApplicationController;

namespace ApplicationController.Test;

[Collection(nameof(CollectionAttribute))]
public class MyApplicationMain_InitializeAsync(ITestOutputHelper output) : IDisposable
{
    private readonly MyApplicationMain _app = new ();

    private readonly ITestOutputHelper _output = output;

    public void Dispose(){
        Environment.SetEnvironmentVariable("HttpTimeout", null);
    }

    [Fact(DisplayName = "Case003_ENV:HttpTimeoutに異常値を設定する")]
    public async Task Case003()
    {
        Environment.SetEnvironmentVariable("HttpTimeout", "test");

        var ret = await _app.InitializeAsync();

        Assert.False(ret);

        Environment.SetEnvironmentVariable("HttpTimeout", null);
    }

    [Fact(DisplayName = "Case004_ENV:HttpTimeoutに正常値を設定する")]
    public async Task Case004()
    {
        var random = new Random();
        var testHttpTimeout = random.Next(1, 101).ToString();
        _output.WriteLine($"HttpTimeoutに設定するテスト値: {testHttpTimeout}");

        Environment.SetEnvironmentVariable("HttpTimeout", testHttpTimeout);

        var ret = await _app.InitializeAsync();
        Assert.True(ret);

        var type = _app.GetType();
        var pi = type.GetProperty("MyEnvInfo", BindingFlags.Static | BindingFlags.NonPublic);

        var envInfo = (EnvironmentInfo)pi!.GetValue(null)!;
        Assert.Equal(testHttpTimeout, envInfo!.HttpTimeout);
    }
}
