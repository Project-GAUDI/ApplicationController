using System;

namespace IotedgeV2ApplicationController
{
    public interface IApplicationClientFactory
    {
        public IApplicationClient CreateInstance(MyDesiredProperties.Process.AppSetting.Protocol protocol);
    }
}