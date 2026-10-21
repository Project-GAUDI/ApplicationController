namespace ApplicationController.Test;

public class ApplicationController_Execute(ITestOutputHelper output) : ApplicationController_TesterBase(output)
{
    [Fact(DisplayName = "Case122_process_nameがnullまたは空白")]
    public async Task Case122()
    {
        string? process_name = null;

        var result = new IotedgeV2ApplicationController.ApplicationController(factory, sender, env, myDesiredProperties.processes, body, properties);
        var ex = await Assert.ThrowsAnyAsync<Exception>(() => result.Execute(process_name));
        Assert.Contains("process_name is null or empty.", ex.Message);
    }

    [Fact(DisplayName = "Case123_ProcessMapにprocess_nameが存在しない")]
    public async Task Case123()
    {
        string process_name = "test_notFound";

        var result = new IotedgeV2ApplicationController.ApplicationController(factory, sender, env, myDesiredProperties.processes, body, properties);
        var ex = await Assert.ThrowsAnyAsync<Exception>(() => result.Execute(process_name));
        Assert.Contains("process_name not found.", ex.Message);
    }

    [Fact(DisplayName = "Case124_ProcessMapにprocess_nameが存在する")]
    public async Task Case124()
    {
        string process_name = "test";
        
        var result = new IotedgeV2ApplicationController.ApplicationController(factory, sender, env, myDesiredProperties.processes, body, properties);
        await result.Execute(process_name);
    }
}