using System;

namespace ApplicationController
{
    public class EnvironmentInfo
    {
        public string HttpTimeout { get; private set; } = null;
        const string ENVNAME_HTTP_TIMEOUT = "HttpTimeout";

        private EnvironmentInfo()
        {
        }

        public static EnvironmentInfo CreateInstance()
        {
            EnvironmentInfo env = new EnvironmentInfo();

            env.HttpTimeout = Environment.GetEnvironmentVariable(ENVNAME_HTTP_TIMEOUT);

            return env;
        }
    }
}
