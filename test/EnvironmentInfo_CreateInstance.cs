using IotedgeV2ApplicationController;

namespace ApplicationController.Test;

[Collection(nameof(EnvironmentInfo_CreateInstance))]
[CollectionDefinition(nameof(EnvironmentInfo_CreateInstance), DisableParallelization = true)]
public class EnvironmentInfo_CreateInstance()
{
    [Fact(DisplayName = "Case011_ENV:HttpTimeoutに数値以外（test）を設定")]
    public void Case011()
    {
        Environment.SetEnvironmentVariable("HttpTimeout", "test");
        
        var exception = Assert.Throws<ArgumentException>(() => EnvironmentInfo.CreateInstance());
        
        Assert.Contains("Error retrieving environment variable 'HttpTimeout'", exception.Message);

        Environment.SetEnvironmentVariable("HttpTimeout", null);
    }

    [Fact(DisplayName = "Case012_ENV:HttpTimeoutに制約条件外（0）を設定")]
    public void Case012()
    {
        Environment.SetEnvironmentVariable("HttpTimeout", "0");
        
        var exception = Assert.Throws<ArgumentException>(() => EnvironmentInfo.CreateInstance());
        
        Assert.Contains("Environment variable 'HttpTimeout' did not meet the specified condition.", exception.Message);

        Environment.SetEnvironmentVariable("HttpTimeout", null);
    }

    [Fact(DisplayName = "Case013_ENV:HttpTimeoutに非整数値（1.5）を設定")]
    public void Case013()
    {
        Environment.SetEnvironmentVariable("HttpTimeout", "1.5");
        
        var exception = Assert.Throws<ArgumentException>(() => EnvironmentInfo.CreateInstance());
        
        Assert.Contains("Environment variable 'HttpTimeout' did not meet the specified condition.", exception.Message);

        Environment.SetEnvironmentVariable("HttpTimeout", null);
    }

    [Fact(DisplayName = "Case014_ENV:HttpTimeoutが未設定")]
    public void Case014()
    {
        Environment.SetEnvironmentVariable("HttpTimeout", null);
        EnvironmentInfo result = EnvironmentInfo.CreateInstance();
        Assert.Equal("10", result.HttpTimeout);
        Environment.SetEnvironmentVariable("HttpTimeout", null);
    }

    [Fact(DisplayName = "Case015_ENV:HttpTimeoutに整数値（1）を設定")]
    public void Case015()
    {
        Environment.SetEnvironmentVariable("HttpTimeout", "1");
        EnvironmentInfo result = EnvironmentInfo.CreateInstance();
        Assert.Equal("1", result.HttpTimeout);
        Environment.SetEnvironmentVariable("HttpTimeout", null);
    }
}
