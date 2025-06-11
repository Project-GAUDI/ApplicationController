using YamlDotNet.Serialization;

namespace IotedgeV2ApplicationController.Test;
public class MyDesiredProperties_Dump
{
    [Fact(DisplayName = "No.69 MyDesiredPropertyがシリアライズできる")]
    public void Case069()
    {
        var instance = MyDesiredPropertiesCreater.Create();
        var dumped = instance.Dump();

        var expectedYaml = new Serializer().Serialize(instance).Trim();
        Assert.Equal(expectedYaml, string.Join(Environment.NewLine, dumped));
    }

    #region 単体テスト仕様書外の既存テスト
    [Fact(DisplayName = "正常系：すべてデフォルト")]
    public void SetValuesIsEmpty_SetValuesIsEmpty()
    {
        string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{}]},\"post_processes\":[{\"tasks\":[{\"set_values\":[{}]}]}]}]}";
        MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
        List<string> expected = new List<string>()
        {
            "input_name: ",
            "processes:",
            "- process_name: ",
            "  application:",
            "    type: NotSelected",
            "    url: ",
            "    replace_params:",
            "    - base_name: ",
            "      source_data_type: NotSelected",
            "      source_data_key: ",
            "  post_processes:",
            "  - condition_path: ",
            "    condition_operator: ",
            "    tasks:",
            "    - output_name: ",
            "      next_process: ",
            "      set_values:",
            "      - type: NotSelected",
            "        key: ",
            "        value: "
        };

        Assert.Equal(expected.Count, result.Dump().Count);
        Assert.Equal(string.Join(',', expected), string.Join(',', result.Dump()));
    }
    #endregion
}
