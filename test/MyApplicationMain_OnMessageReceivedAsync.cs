using TICO.GAUDI.Commons;
using IotedgeV2ApplicationController;

namespace ApplicationController.Test;

public class MyApplicationMain_OnMessageReceivedAsync
{
    private readonly MyApplicationMain _app = new ();

    [Fact(DisplayName = "Case009_非JSON形式のメッセージを受信する")]
    public async Task Case009()
    {
        var inputName = "input";
        string messageStr = "test";
        var message = new IotMessage(messageStr);
        object? userContext = null;
        var ret = await _app.OnMessageReceivedAsync(inputName, message, userContext);
        Assert.True(ret);
    }

    [Fact(DisplayName = "Case010_propertiesにprocess_nameを含まないJSON形式のメッセージを受信する")]
    public async Task Case010()
    {
        var inputName = "input";
        string messageStr = @"{'test':'value'}";
        var message = new IotMessage(messageStr);
        object? userContext = null;
        var ret = await _app.OnMessageReceivedAsync(inputName, message, userContext);
        Assert.True(ret);
    }
}
