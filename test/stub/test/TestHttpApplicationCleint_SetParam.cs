using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ApplicationController.Test
{

    public class TestHttpApplicationCleint_SetParam
    {
        private readonly ITestOutputHelper _output;

        public TestHttpApplicationCleint_SetParam(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "正常系:keyが\"url\"、valueが\"A\"→urlは\"A\"")]
        public async void KeyIsUrlAndValueIsString_UrlIsA()
        {
            TestHttpApplicationClient appClient = new TestHttpApplicationClient();
            appClient.SetParam("url", "A");
            bool connectResult = appClient.Connect();
            string result = await appClient.SendRequest("{\"ABC\":\"123\"}");
            Assert.Contains("\"url\":\"A\"", result);
        }

        [Fact(DisplayName = "正常系:keyが\"timeout\"、valueが\"100\"→timeoutは100")]
        public async void KeyIsTimeoutAndValueIsStringNumber_TimeoutIs100()
        {
            TestHttpApplicationClient appClient = new TestHttpApplicationClient();
            appClient.SetParam("url", "A");
            appClient.SetParam("timeout", "100");
            bool connectResult = appClient.Connect();
            string result = await appClient.SendRequest("{\"ABC\":\"123\"}");
            Assert.Contains("\"timeout\":100", result);
        }

        [Fact(DisplayName = "正常系:keyが\"status\"、valueが\"5\"→statusは\"5\"")]
        public async void KeyIsStatusAndValueIsString_StatusIs5()
        {
            TestHttpApplicationClient appClient = new TestHttpApplicationClient();
            appClient.SetParam("url", "A");
            appClient.SetParam("status", "5");
            bool connectResult = appClient.Connect();
            string result = await appClient.SendRequest("{\"ABC\":\"123\"}");
            Assert.Contains("\"status\":\"5\"", result);
        }

        [Fact(DisplayName = "異常系:keyが\"timeout\"、valueが3.14→例外")]
        public void KeyIsTimeoutAndValueIsFloat_ExceptionThrown()
        {
            TestHttpApplicationClient appClient = new TestHttpApplicationClient();
            Assert.Throws<Exception>(() => { appClient.SetParam("timeout", "3.14"); });
        }

        [Fact(DisplayName = "異常系:keyが\"timeout\"、valueが0→例外")]
        public void KeyIsTimeoutAndValueIs0_ExceptionThrown()
        {
            TestHttpApplicationClient appClient = new TestHttpApplicationClient();
            Assert.Throws<Exception>(() => { appClient.SetParam("timeout", "0"); });
        }

        [Fact(DisplayName = "異常系:keyが\"timeout\"、valueが-1→例外")]
        public void KeyIsTimeoutAndValueIsNegative_ExceptionThrown()
        {
            TestHttpApplicationClient appClient = new TestHttpApplicationClient();
            Assert.Throws<Exception>(() => { appClient.SetParam("timeout", "-1"); });
        }

        [Fact(DisplayName = "異常系:keyが\"timeout\"・\"url\"・\"status\"以外→例外")]
        public void KeyIsIllegal_ExceptionThrown()
        {
            TestHttpApplicationClient appClient = new TestHttpApplicationClient();
            Assert.Throws<Exception>(() => { appClient.SetParam("AAA", "100"); });
        }

        [Fact(DisplayName = "異常系:keyが\"url\", valueがnull→例外")]
        public void KeyIsUrlAndValueIsNull_ExceptionThrown()
        {
            TestHttpApplicationClient appClient = new TestHttpApplicationClient();
            Assert.Throws<Exception>(() => { appClient.SetParam("url", null); });
        }

        [Fact(DisplayName = "異常系:keyが\"url\", valueが\"\"→例外")]
        public void KeyIsUrlAndValueIsEmpty_ExceptionThrown()
        {
            TestHttpApplicationClient appClient = new TestHttpApplicationClient();
            Assert.Throws<Exception>(() => { appClient.SetParam("url", ""); });
        }
    }
}
