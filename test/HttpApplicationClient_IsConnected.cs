using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ApplicationController.Test
{

    public class HttpApplicationClient_IsConnected
    {
        private readonly ITestOutputHelper _output;

        public HttpApplicationClient_IsConnected(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "正常系:Connect未実行→戻り値がfalse")]
        public void NotConnected_FalseReturned()
        {
            HttpApplicationClient appClient = new HttpApplicationClient();
            appClient.SetParam("url","A");
            appClient.SetParam("timeout","5");
            //appClient.Connect();
            bool result = appClient.IsConnected();
            Assert.False(result);
        }

        [Fact(DisplayName = "正常系:Connect実行済み→戻り値がtrue")]
        public void AlreadyConnected_TrueReturned()
        {
            HttpApplicationClient appClient = new HttpApplicationClient();
            appClient.SetParam("url","A");
            appClient.SetParam("timeout","5");
            appClient.Connect();
            bool result = appClient.IsConnected();
            Assert.True(result);
        }
    }
}
