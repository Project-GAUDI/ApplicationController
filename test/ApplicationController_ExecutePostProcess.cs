namespace ApplicationController.Test;

[Collection(nameof(CollectionAttribute))]
public class ApplicationController_ExecutePostProcess(ITestOutputHelper output) : ApplicationController_TesterBase(output)
{
    [Fact(DisplayName = "Case152_CheckConditionでfalseが返される")]
    public async Task Case152()
    {
        myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
        myDesiredProperties.processes[0].post_processes[0].condition_operator = null;
        factory.SetReturnStatus("200");

        var result = new IotedgeV2ApplicationController.ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);
        await result.Execute("test");
        Assert.Null(sender._message);
    }

    [Fact(DisplayName = "Case153_CheckConditionでtrueが返される、かつ、SendMessageAsyncでエラーが発生する")]
    public async Task Case153()
    {
        myDesiredProperties.processes[0].post_processes[0].tasks[0].output_name = "";
        var result = new IotedgeV2ApplicationController.ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);

        var ex = await Assert.ThrowsAsync<Exception>(async () =>
        {
            await result.Execute("test");
        });
        Assert.Contains("Message sent failed.", ex.Message);
    }

    [Fact(DisplayName = "Case154_CheckConditionでtrueが返される、かつ、SendMessageAsyncが成功する")]
    public async Task Case154()
    {
        var result = new IotedgeV2ApplicationController.ApplicationController(factory,sender, env, myDesiredProperties.processes, body, properties);
        await result.Execute("test");
        Assert.Equal(RequiredBody, sender._message?.GetBodyString());
    }
}