using Newtonsoft.Json;
using TICO.GAUDI.Commons;

namespace IotedgeV2ApplicationController.Test;

public class ApplicationController_CreateSendMessage : ApplicationController_TesterBase
{
    private readonly ApplicationController _controller;

    private readonly Mock<IApplicationClientFactory> _appClientFactoryMock = new ();

    private readonly Mock<IMessageSender> _messageSenderMock = new ();

    private readonly EnvironmentInfo _environmentInfo = EnvironmentInfo.CreateInstance();

    private readonly List<MyDesiredProperties.Process> _processes;

    private readonly Dictionary<string, string> _properties;

    public ApplicationController_CreateSendMessage(ITestOutputHelper output) : base(output)
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

        _controller = new ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, _processes, body, _properties);
    }

    [Fact(DisplayName = "No.144 引数\"apiResponse\"がnull")]
    public void Case144()
    {
        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.ThrowsAny<Exception>(() => {
            try {
                mi!.Invoke(_controller, [task, null]);
            } catch (TargetInvocationException tie) {
                throw tie.InnerException!;
            }
        });
    }

    [Fact(DisplayName = "No.145 引数\"task.set_values\"の要素数が0")]
    public void Case145()
    {
        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        task.set_values = [];

        var apiResponse = @"
        {
            ""responseProp1"": ""responseValue1"",
            ""responseProp2"": ""responseValue2""
        }
        ";

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;
        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));

        var collapsed = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(apiResponse));
        Assert.Equal(collapsed, message.GetBodyString());
    }

    [Fact(DisplayName = "No.146 \"set_value.type\"が\"Body\"かつ、\"apiResponse\"に\"set_value.key\"が存在する")]
    public void Case146()
    {
        const string MODIFY_PROP_KEY = "responseProp1";
        const string MODIFIED_VALUE = "modifiedValue1";

        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        task.set_values = [
            MyDesiredPropertiesCreater.CreateValidSetValue($"$.{MODIFY_PROP_KEY}", MODIFIED_VALUE)
        ];

        var responseMap = new Dictionary<string, string> {
            { MODIFY_PROP_KEY, "responseValue1" },
            { "responseProp2", "responseValue2" }
        };

        var apiResponse = JsonConvert.SerializeObject(responseMap);

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;
        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));

        responseMap[MODIFY_PROP_KEY] = MODIFIED_VALUE;
        var expected = JsonConvert.SerializeObject(responseMap);
        Assert.Equal(expected, message.GetBodyString());
    }

    [Fact(DisplayName = "No.147 \"set_value.type\"が\"Body\"かつ、\"apiResponse\"に\"set_value.key\"が存在しない")]
    public void Case147()
    {
        const string ADD_PROP_KEY = "responseProp3";
        const string ADDED_VALUE = "addedValue1";

        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        task.set_values = [
            MyDesiredPropertiesCreater.CreateValidSetValue($"$.{ADD_PROP_KEY}", ADDED_VALUE)
        ];

        var responseMap = new Dictionary<string, string> {
            { "responseProp1", "responseValue1" },
            { "responseProp2", "responseValue2" }
        };

        var apiResponse = JsonConvert.SerializeObject(responseMap);

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;
        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));

        responseMap[ADD_PROP_KEY] = ADDED_VALUE;
        var expected = JsonConvert.SerializeObject(responseMap);
        Assert.Equal(expected, message.GetBodyString());
    }

    [Fact(DisplayName = "No.148 \"set_value.type\"が\"Body\"かつ、\"apiResponse\"内に\"set_value.key\"が複数該当する")]
    public void Case148()
    {
        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        task.set_values = [
            MyDesiredPropertiesCreater.CreateValidSetValue($"$.samples[*].sampleKey", "sample-value")
        ];

        var apiResponse = @"
        {
            ""samples"": [
                {
                    ""sampleKey"": ""1""
                },
                {
                    ""sampleKey"": ""2""
                }
            ]
        }
        ";

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.ThrowsAny<Exception>(() => {
            try {
                mi!.Invoke(_controller, [task, apiResponse]);
            } catch (TargetInvocationException tie) {
                throw tie.InnerException!;
            }
        });
    }

    [Fact(DisplayName = "No.149 \"set_value.type\"が\"Properties\"、かつ、フィールド\"Properties\"に\"set_value.key\"が存在し、\"set_value.value\"がnull")]
    public void Case149()
    {
        const string REMOVE_PROP_KEY = "prop-key1";

        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue(REMOVE_PROP_KEY, null);
        setValue.type = MyDesiredProperties.MessageData.Properties;
        task.set_values = [ setValue ];

        var responseMap = new Dictionary<string, string> {
            { "responseProp1", "responseValue1" },
            { "responseProp2", "responseValue2" }
        };

        var apiResponse = JsonConvert.SerializeObject(responseMap);

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;
        _properties.Remove(REMOVE_PROP_KEY);
        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));
        Assert.Equal(JsonConvert.SerializeObject(responseMap), message.GetBodyString());
    }

    [Fact(DisplayName = "No.150 \"set_value.type\"が\"Properties\"、かつ、フィールド\"Properties\"に\"set_value.key\"が存在し、\"set_value.value\"がnull以外")]
    public void Case150()
    {
        const string MODIFY_PROP_KEY = "prop-key1";
        const string MODIFIED_PROP_VALUE = "modified-prop-value1";

        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue(MODIFY_PROP_KEY, MODIFIED_PROP_VALUE);
        setValue.type = MyDesiredProperties.MessageData.Properties;
        task.set_values = [ setValue ];

        var responseMap = new Dictionary<string, string> {
            { "responseProp1", "responseValue1" },
            { "responseProp2", "responseValue2" }
        };

        var apiResponse = JsonConvert.SerializeObject(responseMap);

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;
        _properties[MODIFY_PROP_KEY] = MODIFIED_PROP_VALUE;
        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));
        Assert.Equal(JsonConvert.SerializeObject(responseMap), message.GetBodyString());
    }

    [Fact(DisplayName = "No.151 \"set_value.type\"が\"Properties\"、かつ、フィールド\"Properties\"に\"set_value.key\"が存在せず、\"set_value.value\"がnull")]
    public void Case151()
    {
        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue("not-contains-prop", null);
        setValue.type = MyDesiredProperties.MessageData.Properties;
        task.set_values = [ setValue ];

        var responseMap = new Dictionary<string, string> {
            { "responseProp1", "responseValue1" },
            { "responseProp2", "responseValue2" }
        };

        var apiResponse = JsonConvert.SerializeObject(responseMap);

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;
        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));
        Assert.Equal(JsonConvert.SerializeObject(responseMap), message.GetBodyString());
    }

    [Fact(DisplayName = "No.152 \"set_value.type\"が\"Properties\"、かつ、フィールド\"Properties\"に\"set_value.key\"が存在せず、\"set_value.value\"がnull以外")]
    public void Case152()
    {
        const string ADD_PROP_KEY = "not-contains-prop-key1";
        const string ADDED_PROP_VALUE = "added-prop-value1";

        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue(ADD_PROP_KEY, ADDED_PROP_VALUE);
        setValue.type = MyDesiredProperties.MessageData.Properties;
        task.set_values = [ setValue ];

        var responseMap = new Dictionary<string, string> {
            { "responseProp1", "responseValue1" },
            { "responseProp2", "responseValue2" }
        };

        var apiResponse = JsonConvert.SerializeObject(responseMap);

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;

        _properties.Add(ADD_PROP_KEY, ADDED_PROP_VALUE);
        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));
        Assert.Equal(JsonConvert.SerializeObject(responseMap), message.GetBodyString());
    }

    [Fact(DisplayName = "No.153 \"set_value.type\"が\"NotSelected\"")]
    public void Case153()
    {
        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue();
        setValue.type = MyDesiredProperties.MessageData.NotSelected;
        task.set_values = [ setValue ];

        var responseMap = new Dictionary<string, string> {
            { "responseProp1", "responseValue1" },
            { "responseProp2", "responseValue2" }
        };

        var apiResponse = JsonConvert.SerializeObject(responseMap);

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;

        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));
        Assert.Equal(JsonConvert.SerializeObject(responseMap), message.GetBodyString());
    }

    [Fact(DisplayName = "No.154 フィールド\"Properties\"に\"process_name\"が存在し、引数\"task.next_process\"がnull")]
    public void Case154()
    {
        const string PROCESS_NAME_KEY = "process_name";

        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        task.set_values = [];
        task.next_process = null;

        _properties.Add(PROCESS_NAME_KEY, "proc");
        var controller = new ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, _processes, body, _properties);

        var responseMap = new Dictionary<string, string> {
            { "responseProp1", "responseValue1" },
            { "responseProp2", "responseValue2" }
        };

        var apiResponse = JsonConvert.SerializeObject(responseMap);

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(controller, [task, apiResponse])!;

        _properties.Remove(PROCESS_NAME_KEY);
        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));
        Assert.Equal(JsonConvert.SerializeObject(responseMap), message.GetBodyString());
    }

    [Fact(DisplayName = "No.155 フィールド\"Properties\"に\"process_name\"が存在し、引数\"task.next_process\"がnull以外")]
    public void Case155()
    {
        const string PROCESS_NAME_KEY = "process_name";

        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        task.set_values = [];
        task.next_process = "sample-next-proc";

        _properties.Add(PROCESS_NAME_KEY, "proc");
        var controller = new ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, _processes, body, _properties);

        var responseMap = new Dictionary<string, string> {
            { "responseProp1", "responseValue1" },
            { "responseProp2", "responseValue2" }
        };

        var apiResponse = JsonConvert.SerializeObject(responseMap);

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(controller, [task, apiResponse])!;

        _properties[PROCESS_NAME_KEY] = task.next_process;
        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));
        Assert.Equal(JsonConvert.SerializeObject(responseMap), message.GetBodyString());
    }

    [Fact(DisplayName = "No.156 フィールド\"Properties\"に\"process_name\"が存在せず、引数\"task.next_process\"がnull")]
    public void Case156()
    {
        const string PROCESS_NAME_KEY = "process_name";
        Assert.False(_properties.ContainsKey(PROCESS_NAME_KEY), $"Propertiesに{PROCESS_NAME_KEY}が含まれないテストの前提条件が満たされていません");

        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        task.set_values = [];
        task.next_process = null;

        var responseMap = new Dictionary<string, string> {
            { "responseProp1", "responseValue1" },
            { "responseProp2", "responseValue2" }
        };

        var apiResponse = JsonConvert.SerializeObject(responseMap);

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;

        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));
        Assert.Equal(JsonConvert.SerializeObject(responseMap), message.GetBodyString());
    }

    [Fact(DisplayName = "No.157 フィールド\"Properties\"に\"process_name\"が存在せず、引数\"task.next_process\"がnull以外")]
    public void Case157()
    {
        const string PROCESS_NAME_KEY = "process_name";

        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        task.set_values = [];
        task.next_process = "sample-next-proc";

        var responseMap = new Dictionary<string, string> {
            { "responseProp1", "responseValue1" },
            { "responseProp2", "responseValue2" }
        };

        var apiResponse = JsonConvert.SerializeObject(responseMap);

        var type = typeof(ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;

        _properties[PROCESS_NAME_KEY] = task.next_process;
        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));
        Assert.Equal(JsonConvert.SerializeObject(responseMap), message.GetBodyString());
    }
}
