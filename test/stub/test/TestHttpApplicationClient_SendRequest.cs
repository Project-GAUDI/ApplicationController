using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ApplicationController.Test
{

    public class TestHttpApplicationClient_SendRequest
    {
        private readonly ITestOutputHelper _output;

        public TestHttpApplicationClient_SendRequest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "正常系:Connect実行済み→正常終了")]
        public async void Connected_Succeeded()
        {
            TestHttpApplicationClient appClient = new TestHttpApplicationClient();
            appClient.SetParam("url", "A");
            appClient.SetParam("timeout", "5");
            appClient.SetParam("status", "10");
            bool connectResult = appClient.Connect();
            string result = await appClient.SendRequest("{\"ABC\":\"123\"}");
            Assert.Equal("{\"status\":\"10\",\"url\":\"A\",\"timeout\":5}", result);
        }

        [Fact(DisplayName = "異常系:Connect未実行→例外")]
        public async void NotConnected_ExceptionThrown()
        {
            TestHttpApplicationClient appClient = new TestHttpApplicationClient();
            appClient.SetParam("url", "A");
            appClient.SetParam("timeout", "5");
            appClient.SetParam("status", "10");
            //bool connectResult = appClient.Connect();
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await appClient.SendRequest("{\"ABC\":\"123\"}");
            });
        }

    }
}
