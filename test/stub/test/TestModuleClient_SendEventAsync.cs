using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using TICO.GAUDI.Commons;

namespace ApplicationController.Test
{

    public class TestModuleClient_SendEventAsync
    {
        private readonly ITestOutputHelper _output;

        public TestModuleClient_SendEventAsync(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "正常系:outputNameが任意の文字列→_messageにIoTMessageが書き込まれる")]
        public async void OutputNameIsDefault_messageSet()
        {
            TestModuleClient client = new TestModuleClient();
            IotMessage message = new IotMessage("body");
            await client.SendEventAsync("test", message);
            Assert.Equal(client._message, message);
        }

        [Fact(DisplayName = "異常系:outputNameが\"\"→例外")]
        public async void OutputNameIsEmpty_ExceptionThrown()
        {
            TestModuleClient client = new TestModuleClient();
            IotMessage message = new IotMessage("body");
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await client.SendEventAsync("", message);
            });
        }

        [Fact(DisplayName = "正常系:TransportTopicがIothub→_messageにIoTMessageが書き込まれる")]
        public async void TransportTopicIsIothub_messageSet()
        {
            TestModuleClient client = new TestModuleClient();
            IotMessage message = new IotMessage("body");
            await client.SendEventAsync("test", message, TransportTopic.Iothub);
            Assert.Equal(client._message, message);
        }

        [Fact(DisplayName = "正常系:TransportTopicがMqtt→_messageにIoTMessageが書き込まれる")]
        public async void TransportTopicIsMqtt_messageSet()
        {
            TestModuleClient client = new TestModuleClient();
            IotMessage message = new IotMessage("body");
            await client.SendEventAsync("test", message, TransportTopic.Mqtt);
            Assert.Equal(client._message, message);
        }

    }

}