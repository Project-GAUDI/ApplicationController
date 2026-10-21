using IotedgeV2ApplicationController;

namespace ApplicationController.Test;

[Collection(nameof(CollectionAttribute))]
public class HttpApplicationClient_Connect
{
    private readonly HttpApplicationClient _applicationClient = new ();

    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new ();

    [Fact(DisplayName = "Case109__urlがnullまたは空白")]
    public void Case109()
    {
        var type = typeof(HttpApplicationClient);
        var fi = type.GetField("_url", BindingFlags.Instance | BindingFlags.NonPublic);
        fi!.SetValue(_applicationClient, null);

        var result = _applicationClient.Connect();
        Assert.False(result);
    }

    [Fact(DisplayName = "Case110_MyHttpClientがnullでない")]
    public void Case110()
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

    [Fact(DisplayName = "Case111__urlがnullまたは空白でない、かつ、MyHttpClientがnull、かつ、protocolがHttp、かつ、_timeoutが数値")]
    public void Case111()
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
}