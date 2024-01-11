using System;

namespace ApplicationController
{
    class ApplicationClientFactory : IApplicationClientFactory
    {

        public IApplicationClient CreateInstance(MyDesiredProperties.Process.AppSetting.Protocol protocol){
            IApplicationClient client = null;

            switch(protocol)
            {
                case MyDesiredProperties.Process.AppSetting.Protocol.http:
                    client = new HttpApplicationClient();
                    break;
                case MyDesiredProperties.Process.AppSetting.Protocol.NotSelected:
                default:
                    throw new Exception($"Protocol {protocol} not supported.");
            }

            return client;
        }

    }
}
