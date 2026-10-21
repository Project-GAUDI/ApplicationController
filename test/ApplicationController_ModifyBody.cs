using IotedgeV2ApplicationController;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApplicationController.Test;

public class ApplicationController_ModifyBody : ApplicationController_TesterBase
{
    private readonly IotedgeV2ApplicationController.ApplicationController _controller;

    private readonly Mock<IApplicationClientFactory> _appClientFactoryMock = new ();

    private readonly Mock<IMessageSender> _messageSenderMock = new ();

    private readonly EnvironmentInfo _environmentInfo = EnvironmentInfo.CreateInstance();

    private readonly List<MyDesiredProperties.Process> _processes;

    private readonly Dictionary<string, string> _properties;

    public ApplicationController_ModifyBody(ITestOutputHelper output) : base(output)
    {
        _processes = [
            MyDesiredPropertiesCreater.CreateValidProcess("execute-dummy-process-id")
        ];

        var body = "{}";
        _properties = new () {
            { "prop-key1", "prop-value1" },
            { "prop-key2", "prop-value2" },
            { "prop-key3", "prop-value3" }
        };

        _controller = new IotedgeV2ApplicationController.ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, _processes, body, _properties);
    }

    [Fact(DisplayName = "Case139_JSONデータioRootに、keyのパスと一致するキーが2つ以上含まれる")]
    public void Case139()
    {
        var jsonData = @"
        {
            ""A"": { ""key"": ""AAA"" },
            ""B"": { ""B2"": { ""key"": ""BBB"" } }
        }";

        var jroot = JsonConvert.DeserializeObject<JToken>(jsonData);
        var key = "$..key";
        var value = "NEW_VALUE";

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("ModifyBody", BindingFlags.Instance | BindingFlags.NonPublic);

        var ex = Assert.ThrowsAny<Exception>(() =>
        {
            try
            {
                mi!.Invoke(_controller, [jroot, key, value]);
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException!;
            }
        });

        Assert.Contains("Multiple values matched by JSONPath in URL replace.", ex.Message);
    }

    [Fact(DisplayName = "Case140_JSONデータioRootに、keyのパスと一致するキーを検索時、JsonExceptionが発生する")]
    public void Case140()
    {
        var jsonData = @"
        {
            ""A"": { ""key"": ""AAA"" },
            ""B"": { ""B2"": { ""key"": ""BBB"" } }
        }";

        var jroot = JsonConvert.DeserializeObject<JToken>(jsonData);
        var key = "invalid[.path";
        var value = "NEW_VALUE";

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("ModifyBody", BindingFlags.Instance | BindingFlags.NonPublic);

        var ex = Assert.ThrowsAny<Exception>(() =>
        {
            try
            {
                mi!.Invoke(_controller, [jroot, key, value]);
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException!;
            }
        });

        Assert.Contains("Illegal JSONPath", ex.Message);
    }

    [Fact(DisplayName = "Case141_JSONデータioRootに、keyのパスと一致するキーを検索時、Exceptionが発生する")]
    public void Case141()
    {
        var jsonData = @"
        {
            ""A"": { ""key"": ""AAA"" },
            ""B"": { ""B2"": { ""key"": ""BBB"" } }
        }";

        var jroot = JsonConvert.DeserializeObject<JToken>(jsonData);
        var key = "A.key.invalid.path";
        var value = "NEW_VALUE";

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("ModifyBody", BindingFlags.Instance | BindingFlags.NonPublic);

        var ex = Assert.ThrowsAny<Exception>(() =>
        {
            try
            {
                mi!.Invoke(_controller, [jroot, key, value]);
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException!;
            }
        });

        Assert.Contains("Exception in GetJsonPathObject", ex.Message);
    }

    [Fact(DisplayName = "Case142_GetJsonPathObjectより返された値がJValue型ではない")]
    public void Case142()
    {
        var jsonData = @"
        {
            ""A"": { ""B"": { ""key"": ""AAA"" } }
        }";

        var jroot = JsonConvert.DeserializeObject<JToken>(jsonData);
        var key = "A.B";
        var value = "NEW_VALUE";

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("ModifyBody", BindingFlags.Instance | BindingFlags.NonPublic);

        var ex = Assert.ThrowsAny<Exception>(() =>
        {
            try
            {
                mi!.Invoke(_controller, [jroot, key, value]);
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException!;
            }
        });

        Assert.Contains("Bad data type", ex.Message);
    }

    [Fact(DisplayName = "Case143_JSONデータioRootに、keyのパスと一致するキーが1つ含まれる")]
    public void Case143()
    {
        var jsonData = @"
        {
            ""A"": { ""key"": ""AAA"" },
            ""B"": { ""B2"": { ""key"": ""BBB"" } }
        }";

        var jroot = JsonConvert.DeserializeObject<JToken>(jsonData);
        var key = "A.key";
        var value = "NEW_VALUE";

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("ModifyBody", BindingFlags.Instance | BindingFlags.NonPublic);

        mi!.Invoke(_controller, [jroot, key, value]);

        var modifiedValue = jroot!.SelectToken("A.key")!.Value<string>();
        Assert.Equal(value, modifiedValue);
    }

    [Fact(DisplayName = "Case144_JSONデータioRootに、keyのパスと一致するキーが含まれない")]
    public void Case144()
    {
        var jsonData = @"
        {
            ""A"": { ""key"": ""AAA"" },
            ""B"": { ""B2"": { ""key"": ""BBB"" } }
        }";

        var jroot = JsonConvert.DeserializeObject<JToken>(jsonData);
        var key = "C.key";
        var value = "NEW_VALUE";

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("ModifyBody", BindingFlags.Instance | BindingFlags.NonPublic);

        mi!.Invoke(_controller, [jroot, key, value]);

        var addedValue = jroot!.SelectToken("C.key")!.Value<string>();
        Assert.Equal(value, addedValue);
    }
}