namespace IotedgeV2ApplicationController.Test;

public class HttpApplicationClient_Disconnect
{
    [Fact(DisplayName = "No.118 プロパティ\"MyHttpClient\"がnull以外")]
    public void Case118()
    {
        var applicationClient = new HttpApplicationClient();
        var type = typeof(HttpApplicationClient);

        var mock = new Mock<HttpClient>();
        var pi = type.GetProperty("MyHttpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        pi!.SetValue(applicationClient, mock.Object);

        applicationClient.Disconnect();
        Assert.Null(pi!.GetValue(applicationClient));
    }

    [Fact(DisplayName = "No.119 プロパティ\"MyHttpClient\"がnull")]
    public void Case119()
    {
        var applicationClient = new HttpApplicationClient();
        var type = typeof(HttpApplicationClient);

        var pi = type.GetProperty("MyHttpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        pi!.SetValue(applicationClient, null);

        applicationClient.Disconnect();
    }

    #region 単体テスト仕様書外の既存テスト
    [Fact(DisplayName = "正常系:Connect実行済み→IsConnectedの結果がFalse")]
    public void Connected_FalseReturned()
    {
        bool result;
        HttpApplicationClient appClient = new HttpApplicationClient();
        appClient.SetParam("url", "A");
        appClient.SetParam("timeout", "5");
        appClient.Connect();
        result = appClient.IsConnected();
        Assert.True(result);
        appClient.Disconnect();
        result = appClient.IsConnected();
        Assert.False(result);
    }

    [Fact(DisplayName = "正常系:Connect未実行→IsConnectedの結果がFalse")]
    public void NotConnected_FalseReturned()
    {
        bool result;
        HttpApplicationClient appClient = new HttpApplicationClient();
        appClient.SetParam("url", "A");
        appClient.SetParam("timeout", "5");
        // appClient.Connect();
        result = appClient.IsConnected();
        Assert.False(result);
        appClient.Disconnect();
        result = appClient.IsConnected();
        Assert.False(result);
    }
    #endregion
}
