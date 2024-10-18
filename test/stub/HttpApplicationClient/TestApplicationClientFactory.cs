using System;

namespace IotedgeV2ApplicationController.Test
{
    public class TestApplicationClientFactory : IApplicationClientFactory
    {
        private string _status = null;
        private string _method = null;
        private string _response = null;

        public IApplicationClient CreateInstance(MyDesiredProperties.Process.AppSetting.Protocol protocol)
        {
            TestHttpApplicationClient client = null;
            switch (protocol)
            {
                case MyDesiredProperties.Process.AppSetting.Protocol.http:
                    client = new TestHttpApplicationClient();
                    break;

                case MyDesiredProperties.Process.AppSetting.Protocol.NotSelected:
                default:
                    break;
            }

            if (null != client && null != _status)
            {
                client.SetParam("status", _status);
            }

            if (null != client && null != _method)
            {
                client.SetParam("method", _method);
            }

            if (null != client && null != _response)
            {
                client.SetParam("response", _response);
            }

            return client;
        }

        public void SetReturnStatus(string status)
        {
            _status = status;
        }

        public void SetErrorMethod(string method)
        {
            _method = method;
        }

        public void SetResponse(string response)
        {
            _response = response;
        }
    }
}