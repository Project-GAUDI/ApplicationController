using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using TICO.GAUDI.Commons;


namespace ApplicationController.Test
{

    public class ApplicationController_Constructor : ApplicationController_TesterBase
    {

        public ApplicationController_Constructor(ITestOutputHelper output) : base(output)
        {

        }

        [Fact(DisplayName = "正常系:実行→ApplicationControllerインスタンス生成")]
        public void AllParamsIsDefault_InstanceCreated()
        {
            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);
            Assert.IsAssignableFrom<ApplicationController>(result);
        }

        [Fact(DisplayName = "異常系:factoryがnull→例外")]
        public void FactoryIsNull_ExceptionThrown()
        {
            Assert.Throws<Exception>(() =>
            {
                ApplicationController result = new ApplicationController(null, env, myDesiredProperties.processes, client, body, properties);
            });
        }

        [Fact(DisplayName = "異常系:envがnull→例外")]
        public void EnvIsNull_ExceptionThrown()
        {
            Assert.Throws<Exception>(() =>
            {
                ApplicationController result = new ApplicationController(factory, null, myDesiredProperties.processes, client, body, properties);
            });
        }

        [Fact(DisplayName = "異常系:client(IModuleClient)がnull→例外")]
        public void ClientIsNull_ExceptionThrown()
        {
            Assert.Throws<Exception>(() =>
            {
                ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, null, body, properties);
            });
        }

        [Fact(DisplayName = "異常系:bodyがnull→例外")]
        public void BodyIsNull_ExceptionThrown()
        {
            Assert.Throws<Exception>(() =>
            {
                ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, null, properties);
            });
        }

        [Fact(DisplayName = "異常系:bodyがjson形式でない→例外")]
        public void BodyIsNotJsonFormat_ExceptionThrown()
        {
            body = "Bad format.";
            var ex = Assert.Throws<Exception>(() =>
            {
                ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);
            });
            Assert.Equal("body is not JSON format.", ex.Message);
        }


        [Fact(DisplayName = "異常系:propertiesがnull→例外")]
        public void PropertiesIsNull_ExceptionThrown()
        {
            Assert.Throws<Exception>(() =>
            {
                ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, null);
            });
        }

    }
}