namespace IotedgeV2ApplicationController.Test;

[Collection(nameof(CollectionAttribute))]
public class HttpApplicationClient_Connect
{
    private readonly HttpApplicationClient _applicationClient = new ();

    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new ();

    [Fact(DisplayName = "No.105 フィールド\"_url\"がnull")]
    public void Case105()
    {
        var type = typeof(HttpApplicationClient);
        var fi = type.GetField("_url", BindingFlags.Instance | BindingFlags.NonPublic);
        fi!.SetValue(_applicationClient, null);

        var result = _applicationClient.Connect();
        Assert.False(result);
    }

    [Fact(DisplayName = "No.106 フィールド\"_url\"が空文字")]
    public void Case106()
    {
        var type = typeof(HttpApplicationClient);
        var fi = type.GetField("_url", BindingFlags.Instance | BindingFlags.NonPublic);
        fi!.SetValue(_applicationClient, "");

        var result = _applicationClient.Connect();
        Assert.False(result);
    }

    [Fact(DisplayName = "No.107 フィールド\"_url\"がnull/空文字以外")]
    public void Case107()
    {
        var type = typeof(HttpApplicationClient);
        var fi = type.GetField("_url", BindingFlags.Instance | BindingFlags.NonPublic);
        fi!.SetValue(_applicationClient, "https://sample.xyz/");

        var pi = type.GetProperty("MyHttpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        pi!.SetValue(_applicationClient, null);

        Mock<HttpClient> httpClientMock = new ();
        _httpClientFactoryMock.Setup(m => m.CreateClient(It.IsAny<string>())).Returns(httpClientMock.Object);

        pi = type.GetProperty("MyHttpClientFactory", BindingFlags.Static | BindingFlags.NonPublic);
        pi!.SetValue(_applicationClient, _httpClientFactoryMock.Object);

        var result = _applicationClient.Connect();
        Assert.True(result);

        // staticプロパティをスタブ化するテストのため、後実施の他ケースへ影響が出ないように後処理でスタブを破棄する
        pi!.SetValue(_applicationClient, null);
    }

    [Fact(DisplayName = "No.109 プロパティ\"MyHttpClient\"がnull以外")]
    public void Case109()
    {
        var type = typeof(HttpApplicationClient);
        var fi = type.GetField("_url", BindingFlags.Instance | BindingFlags.NonPublic);
        fi!.SetValue(_applicationClient, "https://sample.xyz/");

        Mock<HttpClient> httpClientMock = new ();
        var pi = type.GetProperty("MyHttpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        pi!.SetValue(_applicationClient, httpClientMock.Object);

        _httpClientFactoryMock.Setup(m => m.CreateClient(It.IsAny<string>())).Returns(httpClientMock.Object);

        pi = type.GetProperty("MyHttpClientFactory", BindingFlags.Static | BindingFlags.NonPublic);
        pi!.SetValue(_applicationClient, _httpClientFactoryMock.Object);

        var result = _applicationClient.Connect();
        Assert.False(result);

        // staticプロパティをスタブ化するテストのため、後実施の他ケースへ影響が出ないように後処理でスタブを破棄する
        pi!.SetValue(_applicationClient, null);
    }

    [Fact(DisplayName = "No.110 HttpClientの生成の結果がnull")]
    public void Case110()
    {
        var type = typeof(HttpApplicationClient);
        var fi = type.GetField("_url", BindingFlags.Instance | BindingFlags.NonPublic);
        fi!.SetValue(_applicationClient, "https://sample.xyz/");

        var pi = type.GetProperty("MyHttpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        pi!.SetValue(_applicationClient, null);

        #pragma warning disable CS8604
        _httpClientFactoryMock.Setup(m => m.CreateClient(It.IsAny<string>())).Returns(null as HttpClient);
        #pragma warning restore CS8604

        pi = type.GetProperty("MyHttpClientFactory", BindingFlags.Static | BindingFlags.NonPublic);
        pi!.SetValue(_applicationClient, _httpClientFactoryMock.Object);

        var result = _applicationClient.Connect();
        Assert.False(result);

        // staticプロパティをスタブ化するテストのため、後実施の他ケースへ影響が出ないように後処理でスタブを破棄する
        pi!.SetValue(_applicationClient, null);
    }

    #region 単体テスト仕様書外の既存テスト
    [Fact(DisplayName = "正常系:urlとtimeoutのSetParam実行済み→戻り値がtrue")]
    public void UrlAndTimeoutSetParamed_TrueReturned()
    {
        HttpApplicationClient appClient = new HttpApplicationClient();
        appClient.SetParam("url","A");
        appClient.SetParam("timeout","5");
        bool result = appClient.Connect();
        Assert.True(result);
    }

    [Fact(DisplayName = "正常系:urlのみSetParam実行済み→戻り値がtrue")]
    public void OnlyUrlSetParamed_TrueReturned()
    {
        HttpApplicationClient appClient = new HttpApplicationClient();
        appClient.SetParam("url","A");
        // appClient.SetParam("timeout","5");
        bool result = appClient.Connect();
        Assert.True(result);
    }

    [Fact(DisplayName = "異常系:urlのSetParam未実行→戻り値がfalse")]
    public void UrlNotSetParamed_FalseReturned()
    {
        HttpApplicationClient appClient = new HttpApplicationClient();
        // appClient.SetParam("url","A");
        appClient.SetParam("timeout","5");
        bool result = appClient.Connect();
        Assert.False(result);
    }

    [Fact(DisplayName = "正常系:Connect未実行→戻り値がtrue")]
    public void NotConnected_TrueReturned()
    {
        HttpApplicationClient appClient = new HttpApplicationClient();
        appClient.SetParam("url","A");
        appClient.SetParam("timeout","5");
        bool result = appClient.Connect();
        Assert.True(result);
    }

    [Fact(DisplayName = "異常系:Connect実行済み→戻り値がfalse")]
    public void AlreadyConnected_FalseReturned()
    {
        HttpApplicationClient appClient = new HttpApplicationClient();
        appClient.SetParam("url","A");
        appClient.SetParam("timeout","5");
        bool result = appClient.Connect();
        result = appClient.Connect();
        Assert.False(result);
    }
    #endregion
}
