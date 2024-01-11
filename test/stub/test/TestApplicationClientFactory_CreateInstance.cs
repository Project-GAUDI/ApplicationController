using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ApplicationController.Test
{

    public class TestApplicationClientFactory_CreateInstance
    {
        private readonly ITestOutputHelper _output;

        public TestApplicationClientFactory_CreateInstance(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "正常系:protocolが\"http\"→TestHttpApplicationClientインスタンス生成")]
        public void ProtocolIsHttp_InstanceCreated()
        {
            IApplicationClientFactory fact = new TestApplicationClientFactory();
            IApplicationClient result = fact.CreateInstance(MyDesiredProperties.Process.AppSetting.Protocol.http);
            Assert.IsAssignableFrom<TestHttpApplicationClient>(result);
        }

        [Fact(DisplayName = "正常系:SetReturnStatusのstatusが\"1\"で実行済み→SendRequestのstatusが\"1\"")]
        public async void SetReturnStatusArgIsString_SendrequestResponsesStatusIs1()
        {
            TestApplicationClientFactory fact = new TestApplicationClientFactory();
            fact.SetReturnStatus("1");
            IApplicationClient appClient = fact.CreateInstance(MyDesiredProperties.Process.AppSetting.Protocol.http);
            appClient.SetParam("url", "A");
            bool connectResult = appClient.Connect();
            string result = await appClient.SendRequest("{\"ABC\":\"123\"}");
            Assert.Contains("\"status\":\"1\"", result);
        }

    }
}
