using IotedgeV2ApplicationController;

namespace ApplicationController.Test;

[Collection(nameof(CollectionAttribute))]
public class HttpApplicationClient_Initialize
{
    [Fact(DisplayName = "Case097_env.HttpTimeoutがint型に変換できない値")]
    public void Case097()
    {
        var httpTimeout = "invalid-timeout-value";

        var eiType = typeof(EnvironmentInfo);
        var ctor = eiType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, []);
        var envInfo = ctor!.Invoke([]) as EnvironmentInfo;
        var pi = eiType.GetProperty("HttpTimeout", BindingFlags.Instance | BindingFlags.Public);
        pi!.SetValue(envInfo, httpTimeout);
        var appSetting = new MyDesiredProperties.Process.AppSetting
        {
            type = MyDesiredProperties.Process.AppSetting.Protocol.http,
            url = "http://aaa.bbb/ccc"
        };
        var body = "{\"ABC\":\"123\"}";
        var properties = new Dictionary<string, string>();
        var appClient = new TestHttpApplicationClient();

        var ex = Assert.ThrowsAny<Exception>(() => appClient.Initialize(envInfo!, appSetting, body, properties));
        Assert.Contains($"timeout: Parse error.(value={httpTimeout})", ex.Message);
    }

    [Fact(DisplayName = "Case098_env.HttpTimeoutが0以下の数値")]
    public void Case098()
    {
        var httpTimeout = "0";

        var eiType = typeof(EnvironmentInfo);
        var ctor = eiType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, []);
        var envInfo = ctor!.Invoke([]) as EnvironmentInfo;
        var pi = eiType.GetProperty("HttpTimeout", BindingFlags.Instance | BindingFlags.Public);
        pi!.SetValue(envInfo, httpTimeout);
        var appSetting = new MyDesiredProperties.Process.AppSetting
        {
            type = MyDesiredProperties.Process.AppSetting.Protocol.http,
            url = "http://aaa.bbb/ccc"
        };
        var body = "{\"ABC\":\"123\"}";
        var properties = new Dictionary<string, string>();
        var appClient = new TestHttpApplicationClient();

        var ex = Assert.ThrowsAny<Exception>(() => appClient.Initialize(envInfo!, appSetting, body, properties));
        Assert.Contains($"timeout: Out of range.(value={httpTimeout})", ex.Message);
    }

    [Fact(DisplayName = "Case099_env.HttpTimeoutが1以上の数値")]
    public void Case099()
    {
        var httpTimeout = "1";

        var eiType = typeof(EnvironmentInfo);
        var ctor = eiType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, []);
        var envInfo = ctor!.Invoke([]) as EnvironmentInfo;
        var pi = eiType.GetProperty("HttpTimeout", BindingFlags.Instance | BindingFlags.Public);
        pi!.SetValue(envInfo, httpTimeout);
        var appSetting = new MyDesiredProperties.Process.AppSetting
        {
            type = MyDesiredProperties.Process.AppSetting.Protocol.http,
            url = "http://aaa.bbb/ccc"
        };
        var body = "{\"ABC\":\"123\"}";
        var properties = new Dictionary<string, string>();
        var appClient = new TestHttpApplicationClient();

        var result = appClient.Initialize(envInfo!, appSetting, body, properties);
        Assert.True(result);

        var hacType = typeof(HttpApplicationClient);
        var fi = hacType.GetField("_timeout", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(int.Parse(httpTimeout), fi!.GetValue(appClient));
    }

    [Fact(DisplayName = "Case100_env.HttpTimeoutがnullまたは空白")]
    public void Case100()
    {
        var defaultTimeout = 10;

        var eiType = typeof(EnvironmentInfo);
        var ctor = eiType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, []);
        var envInfo = ctor!.Invoke([]) as EnvironmentInfo;
        var appSetting = new MyDesiredProperties.Process.AppSetting
        {
            type = MyDesiredProperties.Process.AppSetting.Protocol.http,
            url = "http://aaa.bbb/ccc"
        };
        var body = "{\"ABC\":\"123\"}";
        var properties = new Dictionary<string, string>();
        var appClient = new TestHttpApplicationClient();

        var result = appClient.Initialize(envInfo!, appSetting, body, properties);
        Assert.True(result);

        var hacType = typeof(HttpApplicationClient);
        var fi = hacType.GetField("_timeout", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(defaultTimeout, fi!.GetValue(appClient));
    }
    
    [Fact(DisplayName = "Case101_param.source_data_type=Bodyで、param.source_data_keyと一致するキーがbodyに2つ以上含まれる")]
    public void Case101()
    {
        var envInfo = EnvironmentInfo.CreateInstance();
        var param = new MyDesiredProperties.Process.AppSetting.ReplaceParam
        {
            base_name = "aaa",
            source_data_type = MyDesiredProperties.MessageData.Body,
            source_data_key = "$.ABC[*]"
        };
        var appSetting = new MyDesiredProperties.Process.AppSetting
        {
            type = MyDesiredProperties.Process.AppSetting.Protocol.http,
            url = "http://aaa.bbb/ccc"
        };
        appSetting.replace_params.Add(param);
        var body = "{\"ABC\":[\"123\", \"456\"]}";
        var properties = new Dictionary<string, string>();
        var appClient = new TestHttpApplicationClient();

        var exception = Assert.Throws<Exception>(() => appClient.Initialize(envInfo, appSetting, body, properties));
        Assert.Contains("Multiple tokens matched.", exception.Message);
    }

    [Fact(DisplayName = "Case102_param.source_data_type=Bodyで、param.source_data_keyと一致するキーがbodyに1つ含まれる")]
    public async Task Case102()
    {
        var envInfo = EnvironmentInfo.CreateInstance();
        var param = new MyDesiredProperties.Process.AppSetting.ReplaceParam
        {
            base_name = "aaa",
            source_data_type = MyDesiredProperties.MessageData.Body,
            source_data_key = "$.ABC"
        };
        var appSetting = new MyDesiredProperties.Process.AppSetting
        {
            type = MyDesiredProperties.Process.AppSetting.Protocol.http,
            url = "http://aaa.bbb/ccc"
        };
        appSetting.replace_params.Add(param);
        var body = "{\"ABC\":\"123\"}";
        var properties = new Dictionary<string, string>();
        var appClient = new TestHttpApplicationClient();

        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        Assert.True(initStatus);

        appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"url\":\"http://123.bbb/ccc\"", result);
    }

    [Fact(DisplayName = "Case103_param.source_data_type=Bodyで、param.source_data_keyと一致するキーがbodyに含まれない")]
    public async Task Case103()
    {
        var envInfo = EnvironmentInfo.CreateInstance();
        var param = new MyDesiredProperties.Process.AppSetting.ReplaceParam
        {
            base_name = "aaa",
            source_data_type = MyDesiredProperties.MessageData.Body,
            source_data_key = "$.DEF"
        };
        var appSetting = new MyDesiredProperties.Process.AppSetting
        {
            type = MyDesiredProperties.Process.AppSetting.Protocol.http,
            url = "http://aaa.bbb/ccc"
        };
        appSetting.replace_params.Add(param);
        var body = "{\"ABC\":\"123\"}";        
        var properties = new Dictionary<string, string>();
        var appClient = new TestHttpApplicationClient();

        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        Assert.True(initStatus);

        appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"url\":\"http://.bbb/ccc\"", result);
    }

    [Fact(DisplayName = "Case104_param.source_data_type=Propertiesで、param.source_data_keyと一致するキーがpropertiesに含まれる")]
    public async Task Case104()
    {
        var envInfo = EnvironmentInfo.CreateInstance();
        var param = new MyDesiredProperties.Process.AppSetting.ReplaceParam
        {
            base_name = "aaa",
            source_data_type = MyDesiredProperties.MessageData.Properties,
            source_data_key = "key1"
        };
        var appSetting = new MyDesiredProperties.Process.AppSetting
        {
            type = MyDesiredProperties.Process.AppSetting.Protocol.http,
            url = "http://aaa.bbb/ccc"
        };
        appSetting.replace_params.Add(param);
        var body = "";
        var properties = new Dictionary<string, string>
        {
            { "key1", "123" }
        };
        var appClient = new TestHttpApplicationClient();

        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        Assert.True(initStatus);

        appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"url\":\"http://123.bbb/ccc\"", result);
    }

    [Fact(DisplayName = "Case105_param.source_data_type=Propertiesで、param.source_data_keyと一致するキーがpropertiesに含まれない")]
    public async Task Case105()
    {
        var envInfo = EnvironmentInfo.CreateInstance();
        var param = new MyDesiredProperties.Process.AppSetting.ReplaceParam
        {
            base_name = "aaa",
            source_data_type = MyDesiredProperties.MessageData.Properties,
            source_data_key = "key1"
        };
        var appSetting = new MyDesiredProperties.Process.AppSetting
        {
            type = MyDesiredProperties.Process.AppSetting.Protocol.http,
            url = "http://aaa.bbb/ccc"
        };
        appSetting.replace_params.Add(param);
        var body = "";
        var properties = new Dictionary<string, string>
        {
            { "keyA", "123" }
        };
        var appClient = new TestHttpApplicationClient();

        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        Assert.True(initStatus);
        
        appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"url\":\"http://.bbb/ccc\"", result);
    }

    [Fact(DisplayName = "Case106_param.source_data_type=NotSelected")]
    public async Task Case106()
    {
        var envInfo = EnvironmentInfo.CreateInstance();
        var param = new MyDesiredProperties.Process.AppSetting.ReplaceParam
        {
            base_name = "aaa",
            source_data_type = MyDesiredProperties.MessageData.NotSelected,
            source_data_key = "key1"
        };
        var appSetting = new MyDesiredProperties.Process.AppSetting
        {
            type = MyDesiredProperties.Process.AppSetting.Protocol.http,
            url = "http://aaa.bbb/ccc"
        };
        appSetting.replace_params.Add(param);
        var body = "";
        var properties = new Dictionary<string, string>
        {
            { "keyA", "123" }
        };
        var appClient = new TestHttpApplicationClient();

        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        Assert.True(initStatus);
        
        appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"url\":\"http://.bbb/ccc\"", result);
    }

    [Fact(DisplayName = "Case107_paramがnull")]
    public async Task Case107()
    {
        var envInfo = EnvironmentInfo.CreateInstance();
        var appSetting = new MyDesiredProperties.Process.AppSetting
        {
            type = MyDesiredProperties.Process.AppSetting.Protocol.http,
            url = "http://aaa.bbb/ccc"
        };
        var body = "{\"ABC\":\"123\"}";
        var properties = new Dictionary<string, string>();
        var appClient = new TestHttpApplicationClient();

        bool initStatus = appClient.Initialize(envInfo, appSetting, body, properties);
        Assert.True(initStatus);

        appClient.Connect();
        string result = await appClient.SendRequest(body);
        Assert.Contains("\"url\":\"http://aaa.bbb/ccc\"", result);
    }

    [Fact(DisplayName = "Case108_置換処理された結果、targetURLが空白")]
    public void Case108()
    {
        var envInfo = EnvironmentInfo.CreateInstance();
        var param = new MyDesiredProperties.Process.AppSetting.ReplaceParam
        {
            base_name = "aaa",
            source_data_type = MyDesiredProperties.MessageData.Body,
            source_data_key = "$.ABC"
        };
        var appSetting = new MyDesiredProperties.Process.AppSetting
        {
            type = MyDesiredProperties.Process.AppSetting.Protocol.http,
            url = "aaa"
        };
        appSetting.replace_params.Add(param);
        var body = "{\"ABC\":\"\"}";
        var properties = new Dictionary<string, string>();
        var appClient = new TestHttpApplicationClient();

        var ex = Assert.ThrowsAny<Exception>(() => appClient.Initialize(envInfo, appSetting, body, properties));
        Assert.Contains($"url: Must be input.(value=).", ex.Message);
    }
}
