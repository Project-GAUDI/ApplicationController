using Newtonsoft.Json.Linq;
using IotedgeV2ApplicationController;

namespace ApplicationController.Test;

public class MyApplicationMain_OnDesiredPropertiesReceivedAsync
{
    private readonly MyApplicationMain _app = new();

    [Fact(DisplayName = "Case007_DPに不正な設定をする")]
    public async Task Case007()
    {
        var props = new JObject();
        var ret = await _app.OnDesiredPropertiesReceivedAsync(props);
        Assert.False(ret);
    }

    [Fact(DisplayName = "Case008_DPに正常な設定をする")]
    public async Task Case008()
    {
        var props = JObject.Parse(
            @"{ 
                ""input_name"": ""input"",
                ""processes"": [
                    {
                        ""process_name"": ""pre_process"",
                        ""application"": {
                            ""url"": ""http://Preprocess-<machinenumber>-<dienumber>:5001/score""
                        }
                    }
                ]
            }"
        );
        var ret = await _app.OnDesiredPropertiesReceivedAsync(props);
        Assert.True(ret);
    }
}
