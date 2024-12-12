using System;
using TICO.GAUDI.Commons;

namespace IotedgeV2ApplicationController
{
    public class EnvironmentInfo
    {
        private static ILogger MyLogger { get; } = LoggerFactory.GetLogger(typeof(EnvironmentInfo));

        public string HttpTimeout { get; private set; } = null;
        const string ENVNAME_HTTP_TIMEOUT = "HttpTimeout";

        private EnvironmentInfo()
        {
        }

        public static EnvironmentInfo CreateInstance()
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: CreateInstance");

            EnvironmentInfo env = new EnvironmentInfo();

            env.HttpTimeout = Environment.GetEnvironmentVariable(ENVNAME_HTTP_TIMEOUT);

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: CreateInstance");

            return env;
        }
    }
}
