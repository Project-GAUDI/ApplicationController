using System;
using System.Threading.Tasks;
using TICO.GAUDI.Commons;

namespace IotedgeV2ApplicationController
{
    public class MessageSender : IMessageSender
    {
        public async Task SendMessageAsync(string outputName,IotMessage message)
        {
            IApplicationEngine appEngine = ApplicationEngineFactory.GetEngine();
            await appEngine.SendMessageAsync(outputName, message);
            return; 
        }
    }
}