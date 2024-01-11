using System;

namespace ApplicationController
{
    public class TestApplicationClientFactory : IApplicationClientFactory
    {
        private string _status = null;
        private string _method = null;

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
    }
}