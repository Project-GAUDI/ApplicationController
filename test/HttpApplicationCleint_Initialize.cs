using Newtonsoft.Json;

namespace IotedgeV2ApplicationController.Test;

[Collection(nameof(CollectionAttribute))]
public class HttpApplicationCleint_Initialize
{
    private readonly HttpApplicationClient _applicationClient = new ();

    private readonly MyDesiredProperties.Process.AppSetting _appSetting = MyDesiredPropertiesCreater.CreateValidAppSetting();

    private readonly Dictionary<string, string> _bodyMap = [];

    private readonly string _body;

    private readonly Dictionary<string, string> _properties = [];

    public HttpApplicationCleint_Initialize()
    {
        _bodyMap.Add("dummyParam1", "value1");
        _bodyMap.Add("dummyParam2", "value2");
        _bodyMap.Add("dummyParam3", "value3");
        _body = JsonConvert.SerializeObject(_bodyMap);
    }

    [Fact(DisplayName = "No.95 引数\"env.Timeout\"がnull")]
    public void Case095()
    {
        var eiType = typeof(EnvironmentInfo);
        var ctor = eiType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, []);
        var env = ctor!.Invoke([]) as EnvironmentInfo;

        var result = _applicationClient.Initialize(env, _appSetting, _body, _properties);
        Assert.True(result);

        var hacType = typeof(HttpApplicationClient);
        var fi = hacType.GetField("_timeout", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(10, fi!.GetValue(_applicationClient));
    }

    [Fact(DisplayName = "No.96 引数\"env.Timeout\"が空文字")]
    public void Case096()
    {
        var eiType = typeof(EnvironmentInfo);
        var ctor = eiType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, []);
        var env = ctor!.Invoke([]) as EnvironmentInfo;
        var pi = eiType.GetProperty("HttpTimeout", BindingFlags.Instance | BindingFlags.Public);
        pi!.SetValue(env, "");

        var result = _applicationClient.Initialize(env, _appSetting, _body, _properties);
        Assert.True(result);

        var hacType = typeof(HttpApplicationClient);
        var fi = hacType.GetField("_timeout", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(10, fi!.GetValue(_applicationClient));
    }

    [Fact(DisplayName = "No.97 引数\"env.Timeout\"がnull/空文字/数値以外")]
    public void Case097()
    {
        var eiType = typeof(EnvironmentInfo);
        var ctor = eiType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, []);
        var env = ctor!.Invoke([]) as EnvironmentInfo;
        var pi = eiType.GetProperty("HttpTimeout", BindingFlags.Instance | BindingFlags.Public);
        pi!.SetValue(env, "invalid-timeout-value");

        Assert.ThrowsAny<Exception>(() => _applicationClient.Initialize(env, _appSetting, _body, _properties));
    }

    [Fact(DisplayName = "No.98 引数\"env.Timeout\"が0")]
    public void Case098()
    {
        var eiType = typeof(EnvironmentInfo);
        var ctor = eiType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, []);
        var env = ctor!.Invoke([]) as EnvironmentInfo;
        var pi = eiType.GetProperty("HttpTimeout", BindingFlags.Instance | BindingFlags.Public);
        pi!.SetValue(env, "0");

        Assert.ThrowsAny<Exception>(() => _applicationClient.Initialize(env, _appSetting, _body, _properties));
    }

    [Fact(DisplayName = "No.99 引数\"env.Timeout\"が1")]
    public void Case099()
    {
        var eiType = typeof(EnvironmentInfo);
        var ctor = eiType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, []);
        var env = ctor!.Invoke([]) as EnvironmentInfo;
        var pi = eiType.GetProperty("HttpTimeout", BindingFlags.Instance | BindingFlags.Public);
        pi!.SetValue(env, "1");

        var result = _applicationClient.Initialize(env, _appSetting, _body, _properties);
        Assert.True(result);

        var hacType = typeof(HttpApplicationClient);
        var fi = hacType.GetField("_timeout", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(1, fi!.GetValue(_applicationClient));
    }

    [Fact(DisplayName = "No.100 引数\"appSetting.url\"がnullかつ、引数\"appSetting.replace_params\"が空")]
    public void Case100()
    {
        var env = EnvironmentInfo.CreateInstance();

        _appSetting.url = null;
        _appSetting.replace_params.Clear();

        Assert.ThrowsAny<Exception>(() => _applicationClient.Initialize(env, _appSetting, _body, _properties));
    }

    [Fact(DisplayName = "No.101 引数\"appSetting.url\"が空文字、かつ、引数\"appSetting.replace_params\"が空")]
    public void Case101()
    {
        var env = EnvironmentInfo.CreateInstance();

        _appSetting.url = "";
        _appSetting.replace_params.Clear();

        Assert.ThrowsAny<Exception>(() => _applicationClient.Initialize(env, _appSetting, _body, _properties));
    }

    [Fact(DisplayName = "No.102 引数\"appSetting.url\"がnull/空文字以外、かつ、引数\"appSetting.replace_params\"の要素数が0")]
    public void Case102()
    {
        var env = EnvironmentInfo.CreateInstance();

        _appSetting.replace_params.Clear();

        var result = _applicationClient.Initialize(env, _appSetting, _body, _properties);
        Assert.True(result);
    }

    [Fact(DisplayName = "No.103 引数\"appSetting.url\"がnull/空文字以外、かつ、引数\"appSetting.replace_params\"の要素数が2")]
    public void Case103()
    {
        var env = EnvironmentInfo.CreateInstance();

        _appSetting.replace_params.Clear();

        var param1 = MyDesiredPropertiesCreater.CreateValidReplaceParam();
        param1.base_name = "dummy-host";
        param1.source_data_key = "dummyParam1";
        _appSetting.replace_params.Add(param1);
        var param2 = MyDesiredPropertiesCreater.CreateValidReplaceParam();
        param2.base_name = "value1";
        param2.source_data_key = "dummyParam2";
        _appSetting.replace_params.Add(param2);

        var result = _applicationClient.Initialize(env, _appSetting, _body, _properties);
        Assert.True(result);

        var hacType = typeof(HttpApplicationClient);
        var fi = hacType.GetField("_url", BindingFlags.Instance | BindingFlags.NonPublic);

        var expected = _appSetting.url;
        expected = expected.Replace(param1.base_name, _bodyMap[param1.source_data_key]);
        expected = expected.Replace(param2.base_name, _bodyMap[param2.source_data_key]);
        Assert.Equal(expected, fi!.GetValue(_applicationClient));
    }

    #region 単体テスト仕様書外の既存テスト
    [Fact(DisplayName = "正常系:urlの置換なし→レスポンスのurlが入力と一致")]
    public async void UrlIsNotReplaced_UrlIsInput()
    {
        MyDesiredProperties.Process.AppSetting appSetting = new MyDesiredProperties.Process.AppSetting();
        appSetting.type = MyDesiredProperties.Process.AppSetting.Protocol.http;
        appSetting.url = "http://aaa.bbb/ccc";
        EnvironmentInfo envInfo = EnvironmentInfo.CreateInstance();

        string body = "{\"ABC\":\"123\"}";
        Dictionary<string, string> properties = new Dictionary<string, string>();

        TestHttpApplicationClient appClient = new TestHttpApplicationClient();
        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        bool connectResult = appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"url\":\"http://aaa.bbb/ccc\"", result);
    }

    [Fact(DisplayName = "正常系:環境変数HttpTimeoutの設定あり→レスポンスのtimeoutが入力と一致")]
    public async void HttpTimeoutIsSet_TimeoutIsInput()
    {
        Environment.SetEnvironmentVariable("HttpTimeout", "100");

        EnvironmentInfo envInfo = EnvironmentInfo.CreateInstance();
        Assert.Equal("100", envInfo.HttpTimeout);


        MyDesiredProperties.Process.AppSetting appSetting = new MyDesiredProperties.Process.AppSetting();
        appSetting.type = MyDesiredProperties.Process.AppSetting.Protocol.http;
        appSetting.url = "http://aaa.bbb/ccc";

        string body = "{\"ABC\":\"123\"}";
        Dictionary<string, string> properties = new Dictionary<string, string>();

        TestHttpApplicationClient appClient = new TestHttpApplicationClient();
        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        bool connectResult = appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"timeout\":100", result);

        Environment.SetEnvironmentVariable("HttpTimeout", null);
    }

    [Fact(DisplayName = "正常系:環境変数HttpTimeoutの設定なし→レスポンスのtimeoutが10")]
    public async void NoHttpTimeout_TimeoutIs10()
    {

        EnvironmentInfo envInfo = EnvironmentInfo.CreateInstance();
        Assert.Null(envInfo.HttpTimeout);


        MyDesiredProperties.Process.AppSetting appSetting = new MyDesiredProperties.Process.AppSetting();
        appSetting.type = MyDesiredProperties.Process.AppSetting.Protocol.http;
        appSetting.url = "http://aaa.bbb/ccc";

        string body = "{\"ABC\":\"123\"}";
        Dictionary<string, string> properties = new Dictionary<string, string>();

        TestHttpApplicationClient appClient = new TestHttpApplicationClient();
        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        bool connectResult = appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"timeout\":10", result);
    }

    [Fact(DisplayName = "正常系:指定のprocess_nameのreplace_paramsのbase_nameがurlに含まれる→レスポンスのURLが指定の文字列で置換されている")]
    public async void BaseNameContained_UrlReplaced()
    {
        EnvironmentInfo envInfo = EnvironmentInfo.CreateInstance();

        MyDesiredProperties.Process.AppSetting appSetting = new MyDesiredProperties.Process.AppSetting();
        appSetting.type = MyDesiredProperties.Process.AppSetting.Protocol.http;
        appSetting.url = "http://aaa.bbb/ccc";
        var replaceParam = new MyDesiredProperties.Process.AppSetting.ReplaceParam();
        replaceParam.base_name = "aaa";
        replaceParam.source_data_type = MyDesiredProperties.MessageData.Body;
        replaceParam.source_data_key = "$.ABC";
        appSetting.replace_params.Add(replaceParam);

        string body = "{\"ABC\":\"123\"}";
        Dictionary<string, string> properties = new Dictionary<string, string>();

        TestHttpApplicationClient appClient = new TestHttpApplicationClient();
        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        bool connectResult = appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"url\":\"http://123.bbb/ccc\"", result);
    }

    [Fact(DisplayName = "正常系:指定のprocess_nameのreplace_paramsのbase_nameがurlに含まれていない→Urlが変換されない")]
    public async void BaseNameNotContained_UrlNotReplaced()
    {
        EnvironmentInfo envInfo = EnvironmentInfo.CreateInstance();

        MyDesiredProperties.Process.AppSetting appSetting = new MyDesiredProperties.Process.AppSetting();
        appSetting.type = MyDesiredProperties.Process.AppSetting.Protocol.http;
        appSetting.url = "http://aaa.bbb/ccc";
        var replaceParam = new MyDesiredProperties.Process.AppSetting.ReplaceParam();
        replaceParam.base_name = "ddd";
        replaceParam.source_data_type = MyDesiredProperties.MessageData.Body;
        replaceParam.source_data_key = "$.ABC";
        appSetting.replace_params.Add(replaceParam);

        string body = "{\"ABC\":\"123\"}";
        Dictionary<string, string> properties = new Dictionary<string, string>();

        TestHttpApplicationClient appClient = new TestHttpApplicationClient();
        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        bool connectResult = appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"url\":\"http://aaa.bbb/ccc\"", result);
    }

    [Fact(DisplayName = "正常系:指定のprocess_nameのreplace_paramsのsource_data_typeがBodyで、souce_data_keyの場所にデータが存在する(inputJson)→レスポンスのURLが指定の文字列で置換されている")]
    public async void BodyContainsSourceDataKey_UrlReplaced()
    {
        EnvironmentInfo envInfo = EnvironmentInfo.CreateInstance();

        MyDesiredProperties.Process.AppSetting appSetting = new MyDesiredProperties.Process.AppSetting();
        appSetting.type = MyDesiredProperties.Process.AppSetting.Protocol.http;
        appSetting.url = "http://aaa.bbb/ccc";
        var replaceParam = new MyDesiredProperties.Process.AppSetting.ReplaceParam();
        replaceParam.base_name = "aaa";
        replaceParam.source_data_type = MyDesiredProperties.MessageData.Body;
        replaceParam.source_data_key = "$.ABC";
        appSetting.replace_params.Add(replaceParam);

        string body = "{\"ABC\":\"123\"}";
        Dictionary<string, string> properties = new Dictionary<string, string>();

        TestHttpApplicationClient appClient = new TestHttpApplicationClient();
        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        bool connectResult = appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"url\":\"http://123.bbb/ccc\"", result);
    }

    [Fact(DisplayName = "異常系:指定のprocess_nameのreplace_paramsのsource_data_typeがBodyで、souce_data_keyの場所にデータが存在しない(inputJson)→空文字に置き換えられる")]
    public async void BodyNotContainsSourceDataKey_UrlReplacedEmpty()
    {
        EnvironmentInfo envInfo = EnvironmentInfo.CreateInstance();

        MyDesiredProperties.Process.AppSetting appSetting = new MyDesiredProperties.Process.AppSetting();
        appSetting.type = MyDesiredProperties.Process.AppSetting.Protocol.http;
        appSetting.url = "http://aaa.bbb/ccc";
        var replaceParam = new MyDesiredProperties.Process.AppSetting.ReplaceParam();
        replaceParam.base_name = "aaa";
        replaceParam.source_data_type = MyDesiredProperties.MessageData.Body;
        replaceParam.source_data_key = "$.DEF";
        appSetting.replace_params.Add(replaceParam);

        string body = "{\"ABC\":\"123\"}";
        Dictionary<string, string> properties = new Dictionary<string, string>();

        TestHttpApplicationClient appClient = new TestHttpApplicationClient();
        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        bool connectResult = appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"url\":\"http://.bbb/ccc\"", result);
    }

    [Fact(DisplayName = "正常系:指定のprocess_nameのreplace_paramsのsource_data_typeがPropertiesで、souce_data_keyのデータが存在する(inputJson)→レスポンスのURLが指定の文字列で置換されている")]
    public async void PropertyContainsSourceDataKey_UrlReplaced()
    {
        EnvironmentInfo envInfo = EnvironmentInfo.CreateInstance();

        MyDesiredProperties.Process.AppSetting appSetting = new MyDesiredProperties.Process.AppSetting();
        appSetting.type = MyDesiredProperties.Process.AppSetting.Protocol.http;
        appSetting.url = "http://aaa.bbb/ccc";
        var replaceParam = new MyDesiredProperties.Process.AppSetting.ReplaceParam();
        replaceParam.base_name = "aaa";
        replaceParam.source_data_type = MyDesiredProperties.MessageData.Properties;
        replaceParam.source_data_key = "key1";
        appSetting.replace_params.Add(replaceParam);

        string body = "";
        Dictionary<string, string> properties = new Dictionary<string, string>();
        properties.Add("key1", "123");

        TestHttpApplicationClient appClient = new TestHttpApplicationClient();
        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        bool connectResult = appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"url\":\"http://123.bbb/ccc\"", result);
    }

    [Fact(DisplayName = "異常系:指定のprocess_nameのreplace_paramsのsource_data_typeがPropertiesで、souce_data_keyのデータが存在しない(inputJson)→空文字に置き換えられる")]
    public async void PropertyNotContainsSourceDataKey_UrlReplacedEmpty()
    {
        EnvironmentInfo envInfo = EnvironmentInfo.CreateInstance();

        MyDesiredProperties.Process.AppSetting appSetting = new MyDesiredProperties.Process.AppSetting();
        appSetting.type = MyDesiredProperties.Process.AppSetting.Protocol.http;
        appSetting.url = "http://aaa.bbb/ccc";
        var replaceParam = new MyDesiredProperties.Process.AppSetting.ReplaceParam();
        replaceParam.base_name = "aaa";
        replaceParam.source_data_type = MyDesiredProperties.MessageData.Properties;
        replaceParam.source_data_key = "key1";
        appSetting.replace_params.Add(replaceParam);

        string body = "";
        Dictionary<string, string> properties = new Dictionary<string, string>();
        properties.Add("keyA", "123");

        TestHttpApplicationClient appClient = new TestHttpApplicationClient();
        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        bool connectResult = appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"url\":\"http://.bbb/ccc\"", result);
    }

    [Fact(DisplayName = "異常系:source_data_keyに該当する場所が2個→例外")]
    public void SourceDataKeyMatchMultiple_ExceptionThrown()
    {
        EnvironmentInfo envInfo = EnvironmentInfo.CreateInstance();

        MyDesiredProperties.Process.AppSetting appSetting = new MyDesiredProperties.Process.AppSetting();
        appSetting.type = MyDesiredProperties.Process.AppSetting.Protocol.http;
        appSetting.url = "http://aaa.bbb/ccc";
        var replaceParam = new MyDesiredProperties.Process.AppSetting.ReplaceParam();
        replaceParam.base_name = "aaa";
        replaceParam.source_data_type = MyDesiredProperties.MessageData.Body;
        replaceParam.source_data_key = "$.ABC[*]";
        appSetting.replace_params.Add(replaceParam);

        string body = "{\"ABC\":[\"123\", \"456\"]}";
        Dictionary<string, string> properties = new Dictionary<string, string>();

        TestHttpApplicationClient appClient = new TestHttpApplicationClient();
        Assert.Throws<Exception>(() =>
        {
            bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        });
    }
    #endregion
}
