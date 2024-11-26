using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IotedgeV2ApplicationController.Test
{
    class TestHttpApplicationClient : HttpApplicationClient
    {
        private string _status = null;
        private string _method = null;
        private string _response = null;

        public override void SetParam(string key, string value)
        {
            switch (key)
            {
                case "status":
                    _status = value;
                    break;
                case "method":
                    _method = value;
                    break;
                case "response":
                    _response = value;
                    break;
                default:
                    base.SetParam(key, value);
                    break;
            }
        }

        public override bool Initialize(EnvironmentInfo env, MyDesiredProperties.Process.AppSetting appSetting, string body, IDictionary<string, string> properties)
        {
            if ("Initialize" == _method)
            {
                return false;
            }

            return base.Initialize(env, appSetting, body, properties);
        }

        public override bool Connect()
        {
            if ("Connect" == _method)
            {
                return false;
            }

            return base.Connect();

        }

        public override async Task<string> SendRequest(string inputJson)
        {
            if ("SendRequest" == _method)
            {
                throw new Exception("SendRequest Test Force Exception.");
            }

            if ("SendRequest:ReturnNonJson" == _method)
            {
                return "Bad format";
            }

            if (false == IsConnected())
            {
                throw new Exception("Not Connected.");
            }

            await Task.CompletedTask;
            string response = $"{{\"status\":\"{_status}\",\"url\":\"{_url}\",\"timeout\":{_timeout}}}";
            if (null != _response)
            {
                response = _response;
            }
            return response;
        }

        public override void Disconnect()
        {
            if ("Disconnect" == _method)
            {
                throw new Exception("Disconnect Test Force Exception.");
            }

            base.Disconnect();
        }

    }
}
