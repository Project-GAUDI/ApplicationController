using IotedgeV2ApplicationController;

namespace ApplicationController.Test;

public class HttpApplicationClient_Disconnect
{
    [Fact(DisplayName = "Case114_MyHttpClientがnullでない")]
    public void Case114()
    {
        var applicationClient = new HttpApplicationClient();
        var type = typeof(HttpApplicationClient);

        var mock = new Mock<HttpClient>();
        var pi = type.GetProperty("MyHttpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        pi!.SetValue(applicationClient, mock.Object);

        applicationClient.Disconnect();
        Assert.Null(pi!.GetValue(applicationClient));
    }
}