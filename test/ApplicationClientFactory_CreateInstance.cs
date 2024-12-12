using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IotedgeV2ApplicationController.Test
{

    public class ApplicationClientFactory_CreateInstance
    {
        private readonly ITestOutputHelper _output;

        public ApplicationClientFactory_CreateInstance(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "正常系:protocolが\"http\"→TestHttpApplicationClientインスタンス生成")]
        public void ProtocolIsHttp_InstanceCreated()
        {
            IApplicationClientFactory fact = new ApplicationClientFactory();
            IApplicationClient result = fact.CreateInstance(MyDesiredProperties.Process.AppSetting.Protocol.http);
            Assert.IsAssignableFrom<HttpApplicationClient>(result);
        }

        [Fact(DisplayName = "正常系:CreateInstanceの引数にNotSelectedがセット→例外")]
        public void ProtocolIsNotSelected_ExceptionThrown()
        {
            IApplicationClientFactory fact = new ApplicationClientFactory();
            Assert.Throws<Exception>(()=> {
                IApplicationClient result = fact.CreateInstance(MyDesiredProperties.Process.AppSetting.Protocol.NotSelected);
            });
        }
    }
}
