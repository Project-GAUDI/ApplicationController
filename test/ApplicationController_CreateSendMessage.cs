using IotedgeV2ApplicationController;
using Newtonsoft.Json;
using TICO.GAUDI.Commons;

namespace ApplicationController.Test;

public class ApplicationController_CreateSendMessage : ApplicationController_TesterBase
{
    private readonly IotedgeV2ApplicationController.ApplicationController _controller;

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

        _controller = new IotedgeV2ApplicationController.ApplicationController(_appClientFactoryMock.Object, _messageSenderMock.Object, _environmentInfo, _processes, body, _properties);
    }

    [Fact(DisplayName = "Case145_setvalue.type=Bodyで、JSONデータapiResponseにsetvalue.keyのパスと一致するキーが1つ含まれる")]
    public void Case145()
    {
        const string MODIFY_PROP_KEY = "responseProp1";
        const string MODIFIED_VALUE = "modifiedValue1";
        const string NEXT_PROCESS = "next-process-name";

        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue($"$.{MODIFY_PROP_KEY}", MODIFIED_VALUE);
        setValue.type = MyDesiredProperties.MessageData.Body;
        task.next_process = NEXT_PROCESS;
        task.set_values = [ setValue ];

        var responseMap = new Dictionary<string, string> {
            { MODIFY_PROP_KEY, "responseValue1" },
            { "responseProp2", "responseValue2" }
        };

        var apiResponse = JsonConvert.SerializeObject(responseMap);

        var properties = new Dictionary<string, string> {
            { "prop-key1", "prop-value1" },
            { "prop-key2", "prop-value2" },
            { "prop-key3", "prop-value3" }
        };

        var processes = new List<MyDesiredProperties.Process> {
            MyDesiredPropertiesCreater.CreateValidProcess("execute-dummy-process-id")
        };

        var controller = new IotedgeV2ApplicationController.ApplicationController(
            _appClientFactoryMock.Object,
            _messageSenderMock.Object,
            _environmentInfo,
            processes,
            "{}",
            properties
        );

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(controller, [task, apiResponse])!;

        responseMap[MODIFY_PROP_KEY] = MODIFIED_VALUE;
        var expectedBody = JsonConvert.SerializeObject(responseMap);
        Assert.Equal(expectedBody, message.GetBodyString());

        properties["process_name"] = NEXT_PROCESS;
        var expectedProperties = JsonConvert.SerializeObject(properties);
        Assert.Equal(expectedProperties, JsonConvert.SerializeObject(message.GetProperties()));
    }

    [Fact(DisplayName = "Case146_setvalue.type=Bodyで、JSONデータapiResponseにsetvalue.keyのパスと一致するキーが含まれない")]
    public void Case146()
    {
        const string ADD_PROP_KEY = "responseProp3";
        const string ADDED_VALUE = "addedValue1";
        const string NEXT_PROCESS = "next-process-name";

        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("sample-task");
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue($"$.{ADD_PROP_KEY}", ADDED_VALUE);
        setValue.type = MyDesiredProperties.MessageData.Body;
        task.next_process = NEXT_PROCESS;
        task.set_values = [ setValue ];

        var responseMap = new Dictionary<string, string> {
            { "responseProp1", "responseValue1" },
            { "responseProp2", "responseValue2" }
        };

        var apiResponse = JsonConvert.SerializeObject(responseMap);

        var properties = new Dictionary<string, string> {
            { "prop-key1", "prop-value1" },
            { "prop-key2", "prop-value2" },
            { "prop-key3", "prop-value3" }
        };

        var processes = new List<MyDesiredProperties.Process> {
            MyDesiredPropertiesCreater.CreateValidProcess("execute-dummy-process-id")
        };

        var controller = new IotedgeV2ApplicationController.ApplicationController(
            _appClientFactoryMock.Object,
            _messageSenderMock.Object,
            _environmentInfo,
            processes,
            "{}",
            properties
        );

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(controller, [task, apiResponse])!;

        responseMap[ADD_PROP_KEY] = ADDED_VALUE;
        var expectedBody = JsonConvert.SerializeObject(responseMap);
        Assert.Equal(expectedBody, message.GetBodyString());

        properties["process_name"] = NEXT_PROCESS;
        var expectedProperties = JsonConvert.SerializeObject(properties);
        Assert.Equal(expectedProperties, JsonConvert.SerializeObject(message.GetProperties()));
    }

    [Fact(DisplayName = "Case147_setvalue.type=Propertiesで、setvalue.keyがPropertiesに存在し、setvalue.valueがnull")]
    public void Case147()
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

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;
        _properties.Remove(REMOVE_PROP_KEY);
        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));
        Assert.Equal(JsonConvert.SerializeObject(responseMap), message.GetBodyString());
    }

    [Fact(DisplayName = "Case148_setvalue.type=Propertiesで、setvalue.keyがPropertiesに存在し、setvalue.valueがnullでない")]
    public void Case148()
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

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;
        _properties[MODIFY_PROP_KEY] = MODIFIED_PROP_VALUE;
        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));
        Assert.Equal(JsonConvert.SerializeObject(responseMap), message.GetBodyString());
    }

    [Fact(DisplayName = "Case149_setvalue.type=Propertiesで、setvalue.keyがPropertiesに存在せず、setvalue.valueがnull")]
    public void Case149()
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

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;
        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));
        Assert.Equal(JsonConvert.SerializeObject(responseMap), message.GetBodyString());
    }

    [Fact(DisplayName = "Case150_setvalue.type=Propertiesで、setvalue.keyがPropertiesに存在せず、setvalue.valueがnullでない")]
    public void Case150()
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

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;

        _properties.Add(ADD_PROP_KEY, ADDED_PROP_VALUE);
        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));
        Assert.Equal(JsonConvert.SerializeObject(responseMap), message.GetBodyString());
    }

    [Fact(DisplayName = "Case151_setvalue.type=NotSelected")]
    public void Case151()
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

        var type = typeof(IotedgeV2ApplicationController.ApplicationController);
        var mi = type.GetMethod("CreateSendMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var message = (IotMessage)mi!.Invoke(_controller, [task, apiResponse])!;

        Assert.Equal(JsonConvert.SerializeObject(_properties), JsonConvert.SerializeObject(message.GetProperties()));
        Assert.Equal(JsonConvert.SerializeObject(responseMap), message.GetBodyString());
    }
}