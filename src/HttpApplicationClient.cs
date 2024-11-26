using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TICO.GAUDI.Commons;

namespace IotedgeV2ApplicationController
{
    class HttpApplicationClient : IApplicationClient
    {
        private static ILogger MyLogger { get; } = LoggerFactory.GetLogger(typeof(HttpApplicationClient));

        protected string _url = null;
        protected int _timeout = 10;

        private static IHttpClientFactory MyHttpClientFactory { get; set; } = null;
        private HttpClient MyHttpClient { get; set; } = null;

        public virtual bool Initialize(EnvironmentInfo env, MyDesiredProperties.Process.AppSetting appSetting, string body, IDictionary<string, string> properties)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: Initialize");
            bool retStatus = true;

            string timeout = env.HttpTimeout;
            if (false == string.IsNullOrEmpty(timeout))
            {
                SetParam("timeout", timeout);
            }

            string targetURL = appSetting.url;
            foreach (var param in appSetting.replace_params)
            {
                string replaceString = GetReplaceString(param, body, properties);
                targetURL = targetURL.Replace(param.base_name, replaceString);
            }
            SetParam("url", targetURL);
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: Initialize");
            return retStatus;
        }

        private string GetReplaceString(MyDesiredProperties.Process.AppSetting.ReplaceParam param, string body, IDictionary<string, string> properties)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: GetReplaceString");
            string retString = "";

            switch (param.source_data_type)
            {
                case MyDesiredProperties.MessageData.Body:
                    JToken jroot = JsonConvert.DeserializeObject(body) as JToken;
                    IEnumerable<JToken> tokens = jroot.SelectTokens(param.source_data_key);
                    int cnt = tokens.Count();

                    if (2 <= cnt)
                    {
                        var errmsg = $"Multiple tokens matched.";
                        MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: GetReplaceString caused by {errmsg}");
                        throw new Exception(errmsg);
                    }
                    else if (1 == cnt)
                    {
                        retString = tokens.First().ToString();
                    }
                    else if (0 == cnt)
                    {
                        retString = "";
                    }

                    break;

                case MyDesiredProperties.MessageData.Properties:
                    if (properties.ContainsKey(param.source_data_key))
                    {
                        retString = properties[param.source_data_key];
                    }
                    break;

                case MyDesiredProperties.MessageData.NotSelected:
                default:
                    break;
            }
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: GetReplaceString");
            return retString;
        }

        public bool IsConnected()
        {
            return null != MyHttpClient;
        }

        public virtual void SetParam(string key, string value)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: SetParam");
            var errmsg = "";
            switch (key)
            {
                case "url":
                    if (string.IsNullOrEmpty(value))
                    {
                        errmsg = $"url: Must be input.(value={value}).";
                        MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: SetParam caused by {errmsg}");
                        throw new Exception(errmsg);
                    }
                    _url = value;
                    break;
                case "timeout":
                    int parsedValue;
                    bool isParse = int.TryParse(value, out parsedValue);
                    if (false == isParse)
                    {
                        errmsg = $"timeout: Parse error.(value={value})";
                        MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: SetParam caused by {errmsg}");
                        throw new Exception(errmsg);
                    }
                    if (parsedValue <= 0)
                    {
                        errmsg = $"timeout: Out of range.(value={value})";
                        MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: SetParam caused by {errmsg}");
                        throw new Exception(errmsg);
                    }

                    _timeout = parsedValue;
                    break;
                default:
                    errmsg = $"key({key}) not supported.";
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: SetParam caused by {errmsg}");
                    throw new Exception(errmsg);
            }
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: SetParam");
        }

        protected static IHttpClientFactory GetFactory()
        {
            if (null == MyHttpClientFactory)
            {
                MyHttpClientFactory = new ServiceCollection().AddHttpClient().BuildServiceProvider().GetService<IHttpClientFactory>();
            }
            return MyHttpClientFactory;
        }

        public virtual bool Connect()
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: Connect");
            if (string.IsNullOrEmpty(_url))
            {
                var errmsg = $"URL is not set.";
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{errmsg}", true);
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: Connect caused by {errmsg}");
                return false;
            }

            if (IsConnected())
            {
                var errmsg = $"Already connected.";
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{errmsg}", true);
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: Connect caused by {errmsg}");
                return false;
            }

            try
            {
                IHttpClientFactory fact = GetFactory();
                MyHttpClient = fact.CreateClient();
                if (null == MyHttpClient)
                {
                    var errmsg = $"MyHttpClient is null.";
                    MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{errmsg}", true);
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: Connect caused by {errmsg}");
                    return false;
                }

                MyHttpClient.Timeout = TimeSpan.FromSeconds(_timeout);
                MyHttpClient.DefaultRequestHeaders.Add("Accept", @"application/x-ww-form-");
            }
            catch (Exception ex)
            {
                var errmsg = $"Http connect error. timeout:{_timeout}\n detail:{ex.Message}";
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{errmsg}", true);
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: Connect caused by {errmsg}");
                return false;
            }
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: Connect");
            return true;
        }

        public virtual async Task<string> SendRequest(string inputJson)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: SendRequest");

            MyLogger.WriteLog(ILogger.LogLevel.DEBUG, $"SendRequest() url={_url}");
            if (MyLogger.IsLogLevelToOutput(ILogger.LogLevel.TRACE))
            {
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"SendRequest() inputjson={inputJson}");
            }

            var content = new StringContent(inputJson, Encoding.UTF8, "application/json");
            var resResult = await MyHttpClient.PostAsync(_url, content);

            MyLogger.WriteLog(ILogger.LogLevel.DEBUG, $"SendRequest() Statuscode={(int)resResult.StatusCode}, ReasonPhrase={resResult.ReasonPhrase}");
            if (MyLogger.IsLogLevelToOutput(ILogger.LogLevel.TRACE))
            {
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"SendRequest() response={resResult}");
            }

            // エラー判定
            if (System.Net.HttpStatusCode.OK != resResult.StatusCode)
            {
                var errmsg = $"HttpApplication request error.  Url={_url}, StatusCode={(int)resResult.StatusCode}, ReasonPhrase={resResult.ReasonPhrase}.";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: SendRequest caused by {errmsg}");
                throw new Exception(errmsg);
            }

            var bodystr = await resResult.Content.ReadAsStringAsync();

            // bodystrを1行に圧縮
            JObject jobj = (JObject)JsonConvert.DeserializeObject(bodystr);
            bodystr = JsonConvert.SerializeObject(jobj, Formatting.None);

            if (MyLogger.IsLogLevelToOutput(ILogger.LogLevel.TRACE))
            {
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"SendRequest() response_body={bodystr}");
            }

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: SendRequest");
            return bodystr;
        }

        public virtual void Disconnect()
        {
            if (IsConnected())
            {
                MyHttpClient = null;
            }
        }
    }
}
