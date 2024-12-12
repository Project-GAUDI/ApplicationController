using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IotedgeV2ApplicationController
{
    public interface IApplicationClient
    {
        public bool Initialize(EnvironmentInfo env, MyDesiredProperties.Process.AppSetting appSetting, string body, IDictionary<string, string> properties);
        public bool IsConnected();
        public void SetParam(string key, string value);
        public bool Connect();
        public Task<string> SendRequest(string inputJson);
        public void Disconnect();
    }
}
