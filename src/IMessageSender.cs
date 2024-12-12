using System;
using System.Threading.Tasks;
using TICO.GAUDI.Commons;

namespace IotedgeV2ApplicationController
{
    public interface IMessageSender
    {
        public Task SendMessageAsync(string outputName,IotMessage message);
    }
}