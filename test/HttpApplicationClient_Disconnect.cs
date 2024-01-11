using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ApplicationController.Test
{

    public class HttpApplicationClient_Disconnect
    {
        private readonly ITestOutputHelper _output;

        public HttpApplicationClient_Disconnect(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "正常系:Connect実行済み→IsConnectedの結果がFalse")]
        public void Connected_FalseReturned()
        {
            bool result;
            HttpApplicationClient appClient = new HttpApplicationClient();
            appClient.SetParam("url", "A");
            appClient.SetParam("timeout", "5");
            appClient.Connect();
            result = appClient.IsConnected();
            Assert.True(result);
            appClient.Disconnect();
            result = appClient.IsConnected();
            Assert.False(result);
        }

        [Fact(DisplayName = "正常系:Connect未実行→IsConnectedの結果がFalse")]
        public void NotConnected_FalseReturned()
        {
            bool result;
            HttpApplicationClient appClient = new HttpApplicationClient();
            appClient.SetParam("url", "A");
            appClient.SetParam("timeout", "5");
            // appClient.Connect();
            result = appClient.IsConnected();
            Assert.False(result);
            appClient.Disconnect();
            result = appClient.IsConnected();
            Assert.False(result);
        }
    }
}
