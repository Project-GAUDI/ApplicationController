using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using TICO.GAUDI.Commons;


namespace ApplicationController.Test
{

    public class ApplicationController_TesterBase
    {
        protected readonly ITestOutputHelper _output;

        protected TestApplicationClientFactory factory = new TestApplicationClientFactory();
        protected EnvironmentInfo env = EnvironmentInfo.CreateInstance();
        protected TestModuleClient client = new TestModuleClient();
        protected string body = "{\"A\":\"ABC\"}";
        protected IDictionary<string, string> properties = new Dictionary<string, string>();
        protected MyDesiredProperties myDesiredProperties = MyDesiredPropertiesCreater.Create();

        public ApplicationController_TesterBase(ITestOutputHelper output)
        {
            _output = output;
        }


    }
}
