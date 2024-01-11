using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TICO.GAUDI.Commons;
[assembly: InternalsVisibleToAttribute("ApplicationController.Test")]

namespace ApplicationController
{
    class HttpApplicationClient : IApplicationClient
    {
        private static Logger MyLogger { get; } = Logger.GetLogger(typeof(HttpApplicationClient));

        protected string _url = null;
        protected int _timeout = 10;

        private static IHttpClientFactory MyHttpClientFactory { get; set; } = null;
        private HttpClient MyHttpClient { get; set; } = null;

        public virtual bool Initialize(EnvironmentInfo env, MyDesiredProperties.Process.AppSetting appSetting, string body, IDictionary<string, string> properties)
        {
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

            return retStatus;
        }

        private string GetReplaceString(MyDesiredProperties.Process.AppSetting.ReplaceParam param, string body, IDictionary<string, string> properties)
        {
            string retString = "";

            switch (param.source_data_type)
            {
                case MyDesiredProperties.MessageData.Body:
                    JToken jroot = JsonConvert.DeserializeObject(body) as JToken;
                    IEnumerable<JToken> tokens = jroot.SelectTokens(param.source_data_key);
                    int cnt = tokens.Count();

                    if (2 <= cnt)
                    {
                        throw new Exception($"Multiple tokens matched.");
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

            return retString;
        }

        public bool IsConnected()
        {
            return null != MyHttpClient;
        }

        public virtual void SetParam(string key, string value)
        {
            switch (key)
            {
                case "url":
                    if (string.IsNullOrEmpty(value))
                    {
                        throw new Exception($"url: Must be input.(value={value})");
                    }
                    _url = value;
                    break;
                case "timeout":
                    int parsedValue;
                    bool isParse = int.TryParse(value, out parsedValue);
                    if (false == isParse)
                    {
                        throw new Exception($"timeout: Illegal data type.(value={value})");
                    }
                    if (parsedValue <= 0)
                    {
                        throw new Exception($"timeout: Out of range.(value={value})");
                    }

                    _timeout = parsedValue;
                    break;
                default:
                    throw new Exception($"key({key}) not supported.");
            }
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
            if (string.IsNullOrEmpty(_url))
            {
                MyLogger.WriteLog(Logger.LogLevel.ERROR, $"URL is not set.");
                return false;
            }

            if (IsConnected())
            {
                MyLogger.WriteLog(Logger.LogLevel.ERROR, $"Already connected.");
                return false;
            }

            try
            {
                IHttpClientFactory fact = GetFactory();
                MyHttpClient = fact.CreateClient();
                if (null == MyHttpClient)
                {
                    return false;
                }

                MyHttpClient.Timeout = TimeSpan.FromSeconds(_timeout);
                MyHttpClient.DefaultRequestHeaders.Add("Accept", @"application/x-ww-form-");
            }
            catch (Exception ex)
            {
                MyLogger.WriteLog(Logger.LogLevel.ERROR, $"Http connect error. timeout:{_timeout}\n detail:{ex.Message}");
                return false;
            }

            return true;
        }

        public virtual async Task<string> SendRequest(string inputJson)
        {
            MyLogger.WriteLog(Logger.LogLevel.DEBUG, $"SendRequest() url={_url}");
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"SendRequest() inputjson={inputJson}");
            }

            var content = new StringContent(inputJson, Encoding.UTF8, "application/json");
            var resResult = await MyHttpClient.PostAsync(_url, content);

            MyLogger.WriteLog(Logger.LogLevel.DEBUG, $"SendRequest() Statuscode={(int)resResult.StatusCode}, ReasonPhrase={resResult.ReasonPhrase}");
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"SendRequest() response={resResult}");
            }

            // エラー判定
            if (System.Net.HttpStatusCode.OK != resResult.StatusCode)
            {
                throw new Exception($"HttpApplication request error.  Url={_url}, StatusCode={(int)resResult.StatusCode}, ReasonPhrase={resResult.ReasonPhrase}");
            }

            var bodystr = await resResult.Content.ReadAsStringAsync();

            // bodystrを1行に圧縮
            JObject jobj = (JObject)JsonConvert.DeserializeObject(bodystr);
            bodystr = JsonConvert.SerializeObject(jobj, Formatting.None);

            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"SendRequest() response_body={bodystr}");
            }

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
