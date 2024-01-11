using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ApplicationController.Test
{
    public class MyDesiredProperties_Dump
    {
        private readonly ITestOutputHelper _output;

        public MyDesiredProperties_Dump(ITestOutputHelper output)
        {
            _output = output;
        }

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
    }

}