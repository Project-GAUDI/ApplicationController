using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using TICO.GAUDI.Commons;


namespace ApplicationController.Test
{

    public class ApplicationController_ExecuteProcess : ApplicationController_TesterBase
    {

        public ApplicationController_ExecuteProcess(ITestOutputHelper output) : base(output)
        {

        }


        [Fact(DisplayName = "正常系:Excuteの引数のprocess_nameがprocessesに存在する値→正常終了")]
        public async void ProcessNameInProcesses_Succeeded()
        {
            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await result.Execute(process_name);
        }


        [Fact(DisplayName = "異常系:Factoryで生成したApplicatioClient がnull→例外")]
        public async void ApplicatioClientFactoryReturnsNull_ExceptionThrown()
        {
            myDesiredProperties.processes[0].application.type = MyDesiredProperties.Process.AppSetting.Protocol.NotSelected;

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });
        }

        [Fact(DisplayName = "正常系:ApplicationClientのメソッドでエラーが起きない設定で実行→正常終了")]
        public async void AllParamsIsDefault_Succeeded()
        {
            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await result.Execute(process_name);
        }

        [Fact(DisplayName = "異常系:Initializeでエラー→例外")]
        public async void AppClientInitializeIsError_ExceptionThrown()
        {
            factory.SetErrorMethod("Initialize");
            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });
        }

        [Fact(DisplayName = "異常系:Connectでエラー→例外")]
        public async void AppClientConnectIsError_ExceptionThrown()
        {
            factory.SetErrorMethod("Connect");
            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });
        }

        [Fact(DisplayName = "異常系:SendRequestでエラー→例外")]
        public async void AppClientSendRequestIsError_ExceptionThrown()
        {
            factory.SetErrorMethod("SendRequest");
            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });
        }

        [Fact(DisplayName = "異常系:Disconnectでエラー→例外")]
        public async void AppClientDisconnectIsError_ExceptionThrown()
        {
            factory.SetErrorMethod("Disconnect");
            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });
        }

        [Fact(DisplayName = "異常系:レスポンスがjson形式でない場合→例外")]
        public async void ResponseIsNotJson_ExceptionThrown()
        {
            factory.SetErrorMethod("SendRequest:ReturnNonJson");
            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            var ex = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });
            Assert.Contains("Response is not JSON format.", ex.Message);
        }
    }
}
