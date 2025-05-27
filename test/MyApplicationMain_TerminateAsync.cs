namespace IotedgeV2ApplicationController.Test;

public class MyApplicationMain_TerminateAsync
{
    private readonly MyApplicationMain _app = new ();

    [Fact(DisplayName = "No.6 正常終了する")]
    public async Task Case006()
    {
        var ret = await _app.TerminateAsync();
        Assert.True(ret);
    }
}
