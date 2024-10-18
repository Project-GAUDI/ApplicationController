using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using TICO.GAUDI.Commons;

namespace IotedgeV2ApplicationController.Test
{

    public class ApplicationController_GetApplicationClient : ApplicationController_TesterBase
    {

        public ApplicationController_GetApplicationClient(ITestOutputHelper output) : base(output)
        {

        }

        [Fact(DisplayName = "正常系:実行→ApplicationClientインスタンス生成")]
        public async void GetApplicationClient_InstanceCreated()
        {
            ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

            string process_name = "test";

            await result.Execute(process_name);
        }

    }
}
