using TICO.GAUDI.Commons;

namespace IotedgeV2ApplicationController.Test;

public class MyApplicationMain_OnMessageReceivedAsync
{
    private readonly MyApplicationMain _app = new ();

    [Fact(DisplayName = "No.70 受信メッセージ:\"Message Properties\"に\"process_name\"が存在する",
        Skip = "関数内で利用されるMyClientFactoryのスタブ化ができず、接続先となるエンドポイントの用意が必要であることから自動テストから除外")]
    public void Case070()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "No.71 受信メッセージ:\"Message Properties\"に\"process_name\"が存在しない")]
    public async Task Case071()
    {
        var inputName = "input";
        var message = new IotMessage();
        object? userContext = null;
        var ret = await _app.OnMessageReceivedAsync(inputName, message, userContext);
        Assert.False(ret);
    }

    [Fact(DisplayName = "No.72 例外が発生する")]
    public async Task Case072()
    {
        var inputName = "input";
        IotMessage? message = null;
        object? userContext = null;
        var ret = await _app.OnMessageReceivedAsync(inputName, message, userContext);
        Assert.False(ret);
    }
}
