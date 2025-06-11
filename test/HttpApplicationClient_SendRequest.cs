using Newtonsoft.Json;

namespace IotedgeV2ApplicationController.Test;

[Collection(nameof(CollectionAttribute))]
public class HttpApplicationClient_SendRequest
{
    private readonly HttpApplicationClient _applicationClient = new ();

    private readonly Dictionary<string, string> _bodyMap = [];

    [Fact(DisplayName = "No.111 引数\"inputJson\"がnull")]
    public async Task Case111()
    {
        await Assert.ThrowsAnyAsync<Exception>(async() => {
            await _applicationClient.SendRequest(null);
        });
    }

    [Fact(DisplayName = "No.112 フィールド\"_url\"がnull")]
    public async Task Case112()
    {
        var type = typeof(HttpApplicationClient);
        var factory = type.GetMethod("GetFactory", BindingFlags.Static | BindingFlags.NonPublic)!.Invoke(null, []) as IHttpClientFactory;

        var pi = type.GetProperty("MyHttpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        pi!.SetValue(_applicationClient, factory?.CreateClient());

        await Assert.ThrowsAnyAsync<Exception>(async() => {
            await _applicationClient.SendRequest("{}");
        });
    }

    [Fact(DisplayName = "No.113 フィールド\"_url\"が空文字")]
    public async Task Case113()
    {
        var type = typeof(HttpApplicationClient);
        var factory = type.GetMethod("GetFactory", BindingFlags.Static | BindingFlags.NonPublic)!.Invoke(null, []) as IHttpClientFactory;

        var pi = type.GetProperty("MyHttpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        pi!.SetValue(_applicationClient, factory?.CreateClient());

        var fi = type.GetField("_url", BindingFlags.Instance | BindingFlags.NonPublic);
        fi!.SetValue(_applicationClient, "");

        // HttpClient.PostAsyncにて、URLが空文字の場合もNullReferenceExceptionで検証される
        await Assert.ThrowsAnyAsync<Exception>(async() => {
            await _applicationClient.SendRequest("{}");
        });
    }

    [Fact(DisplayName = "No.114 フィールド\"_url\"に接続可能",
        Skip = "接続先となるエンドポイントの用意が必要であることから自動テストから除外")]
    public void Case114()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "No.115 フィールド\"_url\"に接続不可",
        Skip = "接続先となるエンドポイントの用意が必要であることから自動テストから除外")]
    public void Case115()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "No.116 レスポンス:\"Status\"が200以外")]
    public async Task Case116()
    {
        var type = typeof(HttpApplicationClient);

        var testResponse = new HttpResponseMessage();
        testResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
        var mock = new Mock<HttpClient>();

        // PostAsyncの基底処理であるSendAsyncをスタブ化する
        mock.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testResponse);
        var pi = type.GetProperty("MyHttpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        pi!.SetValue(_applicationClient, mock.Object);

        var fi = type.GetField("_url", BindingFlags.Instance | BindingFlags.NonPublic);
        fi!.SetValue(_applicationClient, "https://sample.xyz/");

        await Assert.ThrowsAnyAsync<Exception>(async() => {
            await _applicationClient.SendRequest("{}");
        });
    }

    [Fact(DisplayName = "No.117 レスポンス:\"Status\"が200")]
    public async Task Case117()
    {
        var type = typeof(HttpApplicationClient);

        var responseBodyObj = new Dictionary<string, string>()
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };

        var testResponse = new HttpResponseMessage();
        testResponse.StatusCode = System.Net.HttpStatusCode.OK;
        using var sc = new StringContent(JsonConvert.SerializeObject(responseBodyObj, Formatting.Indented));
        testResponse.Content = sc;
        var mock = new Mock<HttpClient>();

        // PostAsyncの基底処理であるSendAsyncをスタブ化する
        mock.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testResponse);
        var pi = type.GetProperty("MyHttpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        pi!.SetValue(_applicationClient, mock.Object);

        var fi = type.GetField("_url", BindingFlags.Instance | BindingFlags.NonPublic);
        fi!.SetValue(_applicationClient, "https://sample.xyz/");

        var result = await _applicationClient.SendRequest("{}");
        Assert.Equal(JsonConvert.SerializeObject(responseBodyObj, Formatting.None), result);
    }
}
