using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ApplicationController.Test
{

    [Collection(nameof(HttpApplicationCleint_Initialize))]
    [CollectionDefinition(nameof(HttpApplicationCleint_Initialize), DisableParallelization = true)]
    public class HttpApplicationCleint_Initialize
    {
        private readonly ITestOutputHelper _output;

        public HttpApplicationCleint_Initialize(ITestOutputHelper output)
        {
            _output = output;
        }

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
    }
}
