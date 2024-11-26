using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TICO.GAUDI.Commons;
using TICO.GAUDI.Commons.Test;

namespace IotedgeV2ApplicationController.Test
{
    public class StubMessageSender : IMessageSender
    {
        public  IotMessage _message  = null;
        public  Dictionary<string, IotMessage> _allMessage  = new Dictionary<string, IotMessage>();
        public StubMessageSender()
        {
        }
        
        public async Task SendMessageAsync(string outputName,IotMessage message)
        {
            IModuleClient moduleClient = new StubModuleClient();
            await moduleClient.SendEventAsync(outputName, message);
            _message = message;
            _allMessage[outputName] = message;
            return;
        }
    }
}