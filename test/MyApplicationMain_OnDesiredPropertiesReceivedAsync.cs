using Newtonsoft.Json.Linq;

namespace IotedgeV2ApplicationController.Test;

public class MyApplicationMain_OnDesiredPropertiesReceivedAsync
{
    private readonly MyApplicationMain _app = new ();

    [Fact(DisplayName = "No.7 正常終了する")]
    public async Task Case007()
    {
        var props = JObject.Parse(
            @"
            {
                ""input_name"": ""input"",
                ""processes"": [
                    {
                        ""process_name"": ""pre_process"",
                        ""application"": {
                        ""type"": ""http"",
                        ""url"": ""http://Preprocess-<machinenumber>-<dienumber>:5001/score"",
                        ""replace_params"": [
                                {
                                ""base_name"": ""<machinenumber>"",
                                ""source_data_type"": ""Body"",
                                ""source_data_key"": ""$.target.machinenumber""
                                }
                            ]
                        },
                        ""post_processes"": [
                            {
                                ""condition_path"": ""info_data/result"",
                                ""condition_operator"": ""EQ(\""9\"")"",
                                ""tasks"": [
                                    {
                                        ""output_name"": ""out_view1"",
                                        ""next_process"": ""monitoring"",
                                        ""set_values"": [
                                            {
                                                ""type"": ""Body"",
                                                ""key"": ""$.RESULT.status"",
                                                ""value"": ""10""
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
            "
        );

        var ret = await _app.OnDesiredPropertiesReceivedAsync(props);
        Assert.True(ret);
    }

    [Fact(DisplayName = "No.8 例外が発生する")]
    public async Task Case008()
    {
        var props = new JObject();
        var ret = await _app.OnDesiredPropertiesReceivedAsync(props);
        Assert.False(ret);
    }
}
