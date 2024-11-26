using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using TICO.GAUDI.Commons;

namespace IotedgeV2ApplicationController.Test
{

    public class ApplicationController_Execute : ApplicationController_TesterBase
    {


        public ApplicationController_Execute(ITestOutputHelper output) : base(output)
        {
            //base(output);
        }

        [Fact(DisplayName = "正常系:引数のprocess_nameがprocessesに存在する値→正常終了")]
        public async void ProcessNameInProcesses_Succeeded()
        {
            ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

            string process_name = "test";

            await result.Execute(process_name);
        }


        [Fact(DisplayName = "異常系:引数のprocess_nameがprocessesに存在しない値→例外")]
        public async void NoProcessNameInProcesses_ExceptionThrown()
        {
            ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

            string process_name = "test_notFound";

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });
        }

        [Fact(DisplayName = "異常系:引数のprocess_nameがnull→例外")]
        public async void ProcessNameIsNull_ExceptionThrown()
        {
            ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

            string process_name = null;

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });
        }

        [Fact(DisplayName = "異常系:引数のprocess_nameが空文字→例外")]
        public async void ProcessNameIsEmpty_ExceptionThrown()
        {
            ApplicationController result = new ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

            string process_name = "";

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });
        }

    }
}
