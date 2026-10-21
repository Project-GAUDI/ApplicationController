using TICO.GAUDI.Commons;
using TICO.GAUDI.Commons.Test;
using IotedgeV2ApplicationController;

namespace ApplicationController.Test;
public class StubMessageSender : IMessageSender
{
    public  IotMessage? _message  = null;
    public  Dictionary<string, IotMessage> _allMessage  = [];
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
