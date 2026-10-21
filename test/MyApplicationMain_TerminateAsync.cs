using IotedgeV2ApplicationController;

namespace ApplicationController.Test;

public class MyApplicationMain_TerminateAsync
{
    private readonly MyApplicationMain _app = new ();

    [Fact(DisplayName = "Case006_TerminateAsyncが呼び出される")]
    public async Task Case006()
    {
        var ret = await _app.TerminateAsync();
        Assert.True(ret);
    }
}
