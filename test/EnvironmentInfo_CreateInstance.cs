using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading;
using System.Collections.Generic;

namespace IotedgeV2ApplicationController.Test
{

    [Collection(nameof(EnvironmentInfo_CreateInstance))]
    [CollectionDefinition(nameof(EnvironmentInfo_CreateInstance),DisableParallelization = true)]    
    public class EnvironmentInfo_CreateInstance
    {
        private readonly ITestOutputHelper _output;

        public EnvironmentInfo_CreateInstance(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "正常系:HttpTimeoutがない→HttpTimeoutはnull(デフォルト値)")]
        public void NoHttpTimeout_HttpTimeoutIsNull()
        {
            EnvironmentInfo result = EnvironmentInfo.CreateInstance();
            Assert.Null(result.HttpTimeout);
        }

        [Fact(DisplayName = "正常系:HttpTimeoutが\"100\"→HttpTimeoutは\"100\"")]
        public void HttpTimeoutIsStringNumber_HttpTimeoutIs100()
        {
            Environment.SetEnvironmentVariable("HttpTimeout", "100");
            EnvironmentInfo result = EnvironmentInfo.CreateInstance();
            Assert.Equal("100", result.HttpTimeout);
            Environment.SetEnvironmentVariable("HttpTimeout", null);
        }

        [Fact(DisplayName = "正常系:HttpTimeoutが\"A\"→HttpTimeoutは\"A\"")]
        public void HttpTimeoutIsString_HttpTimeoutIsA()
        {
            Environment.SetEnvironmentVariable("HttpTimeout", "A");
            EnvironmentInfo result = EnvironmentInfo.CreateInstance();
            Assert.Equal("A", result.HttpTimeout);
            Environment.SetEnvironmentVariable("HttpTimeout", null);
        }
    }
}
