using IotedgeV2ApplicationController;

namespace ApplicationController.Test;

public class ApplicationController_CheckCondition : ApplicationController_TesterBase
{
    private readonly IotedgeV2ApplicationController.ApplicationController _controller;

    private readonly Mock<IApplicationClientFactory> _appClientFactoryMock = new ();

    private readonly Mock<IMessageSender> _messageSenderMock = new ();

    private readonly EnvironmentInfo _environmentInfo = EnvironmentInfo.CreateInstance();

    public ApplicationController_CheckCondition(ITestOutputHelper output) : base(output)
    {
        List<MyDesiredProperties.Process> processes = [];
        processes.Add(MyDesiredPropertiesCreater.CreateValidProcess("execute-dummy-process-id"));

        var body = "{}";
        Dictionary<string, string> properties = [];

        _controller = new IotedgeV2ApplicationController.ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, processes, body, properties);
    }

    [Fact(DisplayName = "Case132_postProcess.condition_pathがnullまたは空白")]
    public void Case132()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = null;

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, "{}"])!;
        Assert.True(result);
    }

    [Fact(DisplayName = "Case133_postProcess.condition_pathが空白でない、かつ、" +
                        "outputJsonにpostProcess.condition_pathと一致するキーが2つ以上含まれる")]
    public void Case133()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = "$.samples[*].samplePath";

        var sampleOutput = @"
        {
            samples: [
                {
                    ""samplePath"": ""value1""
                },
                {
                    ""samplePath"": ""value2""
                }
            ]
        }";

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.False(result);
    }

    [Fact(DisplayName = "Case134_postProcess.condition_pathが空白でない、かつ、" +
                        "outputJsonにpostProcess.condition_pathと一致するキーが含まれない")]
    public void Case134()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = "$.samples[*].samplePath";

        var sampleOutput = @"
        {
            samples: []
        }";

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.False(result);
    }

    [Fact(DisplayName = "Case135_postProcess.condition_pathが空白でない、かつ、" +
                        "outputJsonにpostProcess.condition_pathと一致するキーが1つ含まれる、かつ、" +
                        "postProcess.condition_operatorがnull")]
    public void Case135()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = "$.samples[*].samplePath";
        proc.condition_operator = null;

        var sampleOutput = @"
        {
            samples: [
                {
                    ""samplePath"": ""value1""
                }
            ]
        }";

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.False(result);
    }

    [Fact(DisplayName = "Case136_postProcess.condition_pathが空白でない、かつ、" +
                        "outputJsonにpostProcess.condition_pathと一致するキーが1つ含まれる、かつ、" +
                        "postProcess.condition_operatorがnullでなく、EQ(で始まり)で終わる文字列以外")]
    public void Case136()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = "$.samples[*].samplePath";
        proc.condition_operator = "INVALID_WORD(\"value1\")";

        var sampleOutput = @"
        {
            samples: [
                {
                    ""samplePath"": ""value1""
                }
            ]
        }";

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.False(result);
    }

    [Fact(DisplayName = "Case137_postProcess.condition_pathが空白でない、かつ、" +
                        "outputJsonにpostProcess.condition_pathと一致するキーが1つ含まれる、かつ、" +
                        "postProcess.condition_operatorがnullでなく、EQ(で始まり)で終わる文字列となっている、かつ、" +
                        "EQ(と)を除いた部分とpostProcess.condition_pathと一致するキーの値が一致しない")]
    public void Case137()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = "$.samples[*].samplePath";
        proc.condition_operator = "EQ(not-match-value)";

        var sampleOutput = @"
        {
            samples: [
                {
                    ""samplePath"": ""value1""
                }
            ]
        }";

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.False(result);
    }

    [Fact(DisplayName = "Case138_postProcess.condition_pathが空白でない、かつ、" +
                        "outputJsonにpostProcess.condition_pathと一致するキーが1つ含まれる、かつ、" +
                        "postProcess.condition_operatorがnullでなく、EQ(で始まり)で終わる文字列となっている、かつ、" +
                        "EQ(と)を除いた部分とpostProcess.condition_pathと一致するキーの値が一致する")]
    public void Case138()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = "$.samples[*].samplePath";
        proc.condition_operator = "EQ(value1)";

        var sampleOutput = @"
        {
            samples: [
                {
                    ""samplePath"": ""value1""
                }
            ]
        }";

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.True(result);
    }
}