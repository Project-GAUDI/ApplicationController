using IotedgeV2ApplicationController;
using Newtonsoft.Json;

namespace ApplicationController.Test;

[Collection(nameof(CollectionAttribute))]
public class HttpApplicationClient_SendRequest
{
    private readonly HttpApplicationClient _applicationClient = new ();

    [Fact(DisplayName = "Case112_HTTP POST リクエスト送信に失敗し、ステータスコード=OK以外のレスポンスが返る")]
    public async Task Case112()
    {
        var type = typeof(HttpApplicationClient);
        var testResponse = new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.BadRequest
        };
        var mock = new Mock<HttpClient>();

        // PostAsyncの基底処理であるSendAsyncをスタブ化する
        mock.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>())).ReturnsAsync(testResponse);
        var pi = type.GetProperty("MyHttpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        pi!.SetValue(_applicationClient, mock.Object);

        var fi = type.GetField("_url", BindingFlags.Instance | BindingFlags.NonPublic);
        fi!.SetValue(_applicationClient, "https://sample.xyz/");

        var ex = await Assert.ThrowsAnyAsync<Exception>(async() => {
            await _applicationClient.SendRequest("{}");
        });
        Assert.Contains($"HttpApplication request error.", ex.Message);
    }

    [Fact(DisplayName = "Case113_HTTP POST リクエスト送信に成功し、ステータスコード=OKのレスポンスが返る")]
    public async Task Case113()
    {
        var type = typeof(HttpApplicationClient);
        var responseBodyObj = new Dictionary<string, string>()
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };
        var testResponse = new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK
        };
        using var sc = new StringContent(JsonConvert.SerializeObject(responseBodyObj, Formatting.Indented));
        testResponse.Content = sc;
        var mock = new Mock<HttpClient>();

        // PostAsyncの基底処理であるSendAsyncをスタブ化する
        mock.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>())).ReturnsAsync(testResponse);
        var pi = type.GetProperty("MyHttpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        pi!.SetValue(_applicationClient, mock.Object);

        var fi = type.GetField("_url", BindingFlags.Instance | BindingFlags.NonPublic);
        fi!.SetValue(_applicationClient, "https://sample.xyz/");

        var result = await _applicationClient.SendRequest("{}");
        Assert.Equal(JsonConvert.SerializeObject(responseBodyObj, Formatting.None), result);
    }
}