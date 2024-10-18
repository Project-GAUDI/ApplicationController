using System;
using TICO.GAUDI.Commons;

namespace IotedgeV2ApplicationController
{
    class ApplicationClientFactory : IApplicationClientFactory
    {
        static ILogger _logger { get; } = LoggerFactory.GetLogger(typeof(ApplicationClientFactory));

        public IApplicationClient CreateInstance(MyDesiredProperties.Process.AppSetting.Protocol protocol)
        {
            _logger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: CreateInstance");

            IApplicationClient client = null;

            switch(protocol)
            {
                case MyDesiredProperties.Process.AppSetting.Protocol.http:
                    client = new HttpApplicationClient();
                    break;
                case MyDesiredProperties.Process.AppSetting.Protocol.NotSelected:
                default:
                    var errmsg = $"Protocol {protocol} not supported.";
                    _logger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: CreateInstance caused by {errmsg}");
                    throw new Exception(errmsg);
            }

            _logger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: CreateInstance");

            return client;
        }
    }
}
