using System;

namespace ApplicationController
{
    public interface IApplicationClientFactory
    {
        public IApplicationClient CreateInstance(MyDesiredProperties.Process.AppSetting.Protocol protocol);
    }
}