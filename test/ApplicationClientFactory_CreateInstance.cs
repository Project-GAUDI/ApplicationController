using IotedgeV2ApplicationController;

namespace ApplicationController.Test;

public class ApplicationClientFactory_CreateInstance()
{
    [Fact(DisplayName = "Case095_DP:application.typeにNotSelectedを設定")]
    public void Case095()
    {
        var fact = new ApplicationClientFactory();

        var exception = Assert.Throws<Exception>(() => fact.CreateInstance(MyDesiredProperties.Process.AppSetting.Protocol.NotSelected));

        Assert.Contains("Protocol NotSelected not supported.", exception.Message);
    }

    [Fact(DisplayName = "Case096_DP:application.typeにhttpを設定")]
    public void Case096()
    {
        var fact = new ApplicationClientFactory();

        IApplicationClient result = fact.CreateInstance(MyDesiredProperties.Process.AppSetting.Protocol.http);
        
        Assert.IsAssignableFrom<HttpApplicationClient>(result);
    }
}
