using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ApplicationController.Test
{

    public class HttpApplicationClient_Connect
    {
        private readonly ITestOutputHelper _output;

        public HttpApplicationClient_Connect(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "正常系:urlとtimeoutのSetParam実行済み→戻り値がtrue")]
        public void UrlAndTimeoutSetParamed_TrueReturned()
        {
            HttpApplicationClient appClient = new HttpApplicationClient();
            appClient.SetParam("url","A");
            appClient.SetParam("timeout","5");
            bool result = appClient.Connect();
            Assert.True(result);
        }

        [Fact(DisplayName = "正常系:urlのみSetParam実行済み→戻り値がtrue")]
        public void OnlyUrlSetParamed_TrueReturned()
        {
            HttpApplicationClient appClient = new HttpApplicationClient();
            appClient.SetParam("url","A");
            // appClient.SetParam("timeout","5");
            bool result = appClient.Connect();
            Assert.True(result);
        }

        [Fact(DisplayName = "異常系:urlのSetParam未実行→戻り値がfalse")]
        public void UrlNotSetParamed_FalseReturned()
        {
            HttpApplicationClient appClient = new HttpApplicationClient();
            // appClient.SetParam("url","A");
            appClient.SetParam("timeout","5");
            bool result = appClient.Connect();
            Assert.False(result);
        }

        [Fact(DisplayName = "正常系:Connect未実行→戻り値がtrue")]
        public void NotConnected_TrueReturned()
        {
            HttpApplicationClient appClient = new HttpApplicationClient();
            appClient.SetParam("url","A");
            appClient.SetParam("timeout","5");
            bool result = appClient.Connect();
            Assert.True(result);
        }

        [Fact(DisplayName = "異常系:Connect実行済み→戻り値がfalse")]
        public void AlreadyConnected_FalseReturned()
        {
            HttpApplicationClient appClient = new HttpApplicationClient();
            appClient.SetParam("url","A");
            appClient.SetParam("timeout","5");
            bool result = appClient.Connect();
            result = appClient.Connect();
            Assert.False(result);
        }
    }
}
