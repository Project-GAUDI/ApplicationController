using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using TICO.GAUDI.Commons.Test;

namespace IotedgeV2ApplicationController.Test
{

    public class ApplicationController_TesterBase
    {
        protected readonly ITestOutputHelper _output;
        private string _status = "";

        protected TestApplicationClientFactory factory = new TestApplicationClientFactory();
        protected StubMessageSender sender = new StubMessageSender();
        protected EnvironmentInfo env = EnvironmentInfo.CreateInstance();
        protected StubModuleClient client = new StubModuleClient();
        protected string body = "{\"A\":\"ABC\"}";
        protected string RequiredBody
        {
            get
            {
                return $"{{\"status\":\"{_status}\",\"url\":\"http://aaa.bbb/ccc\",\"timeout\":10,\"A\":\"ABC\"}}";
            }
            private set { }
        }

        protected IDictionary<string, string> properties = new Dictionary<string, string>();
        protected MyDesiredProperties myDesiredProperties = MyDesiredPropertiesCreater.Create();

        public ApplicationController_TesterBase(ITestOutputHelper output)
        {
            _output = output;
        }

        public void SetRequiredStatus(string status)
        {
            _status = status;
        }


    }
}
