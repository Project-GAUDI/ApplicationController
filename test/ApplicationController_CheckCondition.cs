namespace IotedgeV2ApplicationController.Test;

public class ApplicationController_CheckCondition : ApplicationController_TesterBase
{
    private readonly ApplicationController _controller;

    private readonly Mock<IApplicationClientFactory> _appClientFactoryMock = new ();

    private readonly Mock<IMessageSender> _messageSenderMock = new ();

    private readonly EnvironmentInfo _environmentInfo = EnvironmentInfo.CreateInstance();

    public ApplicationController_CheckCondition(ITestOutputHelper output) : base(output)
    {
        List<MyDesiredProperties.Process> processes = [];
        processes.Add(MyDesiredPropertiesCreater.CreateValidProcess("execute-dummy-process-id"));

        var body = "{}";
        Dictionary<string, string> properties = [];

        _controller = new ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, processes, body, properties);
    }

    [Fact(DisplayName = "No.131 引数\"postProcess.condition_path\"がnull")]
    public void Case131()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = null;

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, "{}"])!;
        Assert.True(result);
    }

    [Fact(DisplayName = "No.132 引数\"postProcess.condition_path\"が空文字")]
    public void Case132()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = "";

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, "{}"])!;
        Assert.True(result);
    }

    [Fact(DisplayName = "No.133 引数\"outputJson\"がnull")]
    public void Case133()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = "sample-path";

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.ThrowsAny<Exception>(() => {
            try {
                mi!.Invoke(_controller, [proc, null]);
            } catch (TargetInvocationException tie) {
                throw tie.InnerException!;
            }
        });
    }

    [Fact(DisplayName = "No.134 引数\"outputJson\"が空文字")]
    public void Case134()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = "sample-path";

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.ThrowsAny<Exception>(() => {
            try {
                mi!.Invoke(_controller, [proc, ""]);
            } catch (TargetInvocationException tie) {
                throw tie.InnerException!;
            }
        });
    }

    [Fact(DisplayName = "No.135 引数\"postProcess.condition_path\"に合致する要素数が2")]
    public void Case135()
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
        }
        ";

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.False(result);
    }

    [Fact(DisplayName = "No.136 引数\"postProcess.condition_path\"に合致する要素数が0")]
    public void Case136()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = "$.samples[*].samplePath";

        var sampleOutput = @"
        {
            samples: []
        }
        ";

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.False(result);
    }

    [Fact(DisplayName = "No.137 引数\"postProcess.condition_path\"に合致する要素数が1、かつ、引数\"postProcess.condition_operator\"がnull")]
    public void Case137()
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
        }
        ";

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.False(result);
    }

    [Fact(DisplayName = "No.138 引数\"postProcess.condition_path\"に合致する要素数が1、かつ、\"postProcess.condition_operator\"が\"EQ(\"で始まり、\")\"で終わり、\"EQ(\",\")\"を除いた値が合致要素の値と一致する")]
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
        }
        ";

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.True(result);
    }

    [Fact(DisplayName = "No.139 引数\"postProcess.condition_path\"に合致する要素数が1、かつ、\"postProcess.condition_operator\"が\"EQ(\"で始まり、\")\"で終わり、\"EQ(\",\")\"を除いた値が\"\"で囲まれている場合、\"\"を除いた値と合致要素の値と一致する")]
    public void Case139()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = "$.samples[*].samplePath";
        proc.condition_operator = "EQ(\"value1\")";

        var sampleOutput = @"
        {
            samples: [
                {
                    ""samplePath"": ""value1""
                }
            ]
        }
        ";

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.True(result);
    }

    [Fact(DisplayName = "No.140 引数\"postProcess.condition_path\"に合致する要素数が1、かつ、postProcess.condition_operatorが\"EQ(\"で始まらない")]
    public void Case140()
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
        }
        ";

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.False(result);
    }

    [Fact(DisplayName = "No.141 引数\"postProcess.condition_path\"に合致する要素数が1、かつ、postProcess.condition_operatorが\")\"で終わらない")]
    public void Case141()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = "$.samples[*].samplePath";
        proc.condition_operator = "EQ(\"value1\"INVALID_END";

        var sampleOutput = @"
        {
            samples: [
                {
                    ""samplePath"": ""value1""
                }
            ]
        }
        ";

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.False(result);
    }

    [Fact(DisplayName = "No.142 引数\"postProcess.condition_path\"に合致する要素数が1、かつ、postProcess.condition_operatorが\"EQ(\"で始り、\")\"で終わり、\"EQ(\",\")\"を除いた値が引数\"value\"と一致しない")]
    public void Case142()
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
        }
        ";

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.False(result);
    }

    [Fact(DisplayName = "No.143 引数\"postProcess.condition_path\"に合致する要素数が1、かつ、postProcess.condition_operatorが\"EQ(\"で始り、\")\"で終わり、\"EQ(\",\")\"を除いた値が\"\"で囲まれている場合、\"\"を除いた値と引数\"value\"と一致しない")]
    public void Case143()
    {
        var proc = MyDesiredPropertiesCreater.CreateValidPostProcess();
        proc.condition_path = "$.samples[*].samplePath";
        proc.condition_operator = "EQ(\"not-match-value\")";

        var sampleOutput = @"
        {
            samples: [
                {
                    ""samplePath"": ""value1""
                }
            ]
        }
        ";

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CheckCondition", BindingFlags.Instance | BindingFlags.NonPublic);

        var result = (bool)mi!.Invoke(_controller, [proc, sampleOutput])!;
        Assert.False(result);
    }
}
