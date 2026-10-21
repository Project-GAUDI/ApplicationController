using Newtonsoft.Json.Linq;
using IotedgeV2ApplicationController;

namespace ApplicationController.Test;

public class MyDesiredProperties_GetDesiredProperties
{
    private readonly string input_name = "input";
    private readonly string process1 = "process1";
    private readonly string process2 = "process2";
    private readonly string url = "url_base1_base2";
    private readonly string base_name1 = "base1";
    private readonly string base_name2 = "base2";
    private readonly string source_data_key = "key";
    private readonly string condition_path = "path";
    private readonly string condition_operator = "EQ(\\\"9\\\")";
    private readonly string output_name1 = "output1";
    private readonly string output_name2 = "output2";
    private readonly string next_process = "process";
    private readonly string set_values_key1 = "key1";
    private readonly string set_values_key2 = "key2";
    private readonly string set_values_value = "value";
    private readonly string string_test = "test";

    [Fact(DisplayName = "Case016_DP:input_nameが存在しない")]
    public void Case016()
    {
        var props = JObject.Parse(
            @"{
                ""processes"": [
                ]
            }"
        );

        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'input_name' is not specified.", exception.Message);
    }
    
    [Fact(DisplayName = "Case017_DP:input_nameがnull")]
    public void Case017()
    {
        var props = JObject.Parse(
            @"{
                ""input_name"": null
            }"
        );

        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'input_name' cannot be null or empty.", exception.Message);
    }
    
    [Fact(DisplayName = "Case018_DP:input_nameが空文字")]
    public void Case018()
    {
        var props = JObject.Parse(
            @"{
                ""input_name"": """"
            }"
        );

        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'input_name' cannot be null or empty.", exception.Message);
    }
    
    [Fact(DisplayName = "Case019_DP:input_nameが存在する")]
    public void Case019()
    {        
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );

        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(input_name, ret.input_name);
    }
    
    [Fact(DisplayName = "Case020_DP:processesが存在しない")]
    public void Case020()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}""
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'processes' is not specified.", exception.Message);
    }
    
    [Fact(DisplayName = "Case021_DP:processesがArray型でない")]
    public void Case021()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": ""{string_test}""
            }}"
        );

        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Error retrieving property 'processes'", exception.Message);
    }
    
    [Fact(DisplayName = "Case022_DP:processesが存在する（1項目）")]
    public void Case022()
    {        
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );

        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(process1, ret.processes[0].process_name);
    }
    
    [Fact(DisplayName = "Case023_DP:processesが存在する（2項目）")]
    public void Case023()
    {        
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }}
                    }},
                    {{
                        ""process_name"": ""{process2}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );

        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(process1, ret.processes[0].process_name);
        Assert.Equal(process2, ret.processes[1].process_name);
    }

    [Fact(DisplayName = "Case024_DP:process_nameが存在しない")]
    public void Case024()
    {        
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""application"": {{
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );

        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'process_name' is not specified.", exception.Message);
    }

    [Fact(DisplayName = "Case025_DP:process_nameがnull")]
    public void Case025()
    {        
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": null,
                        ""application"": {{
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );

        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'process_name' cannot be null or empty.", exception.Message);
    }

    [Fact(DisplayName = "Case026_DP:process_nameが空文字")]
    public void Case026()
    {        
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": """",
                        ""application"": {{
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );

        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'process_name' cannot be null or empty.", exception.Message);
    }

    [Fact(DisplayName = "Case027_DP:process_nameが存在する")]
    public void Case027()
    {        
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );

        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(process1, ret.processes[0].process_name);
    }

    [Fact(DisplayName = "Case028_DP:applicationが存在しない")]
    public void Case028()
    {        
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}""
                    }}
                ]
            }}"
        );

        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'application' is not specified.", exception.Message);
    }

    [Fact(DisplayName = "Case029_DP:applicationがobject型でない")]
    public void Case029()
    {        
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": ""{string_test}""
                    }}
                ]
            }}"
        );

        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Error retrieving property 'application'", exception.Message);
    }

    [Fact(DisplayName = "Case030_DP:applicationが存在する")]
    public void Case030()
    {        
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );

        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(url, ret.processes[0].application.url);
    }

    [Fact(DisplayName = "Case031_DP:application.typeがhttp以外")]
    public void Case031()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""type"": ""{string_test}"",
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Error retrieving property 'type'", exception.Message);
    }
    
    [Fact(DisplayName = "Case032_DP:application.typeが存在しない")]
    public void Case032()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(MyDesiredProperties.Process.AppSetting.Protocol.http, ret.processes[0].application.type);
    }

    [Fact(DisplayName = "Case033_DP:application.typeにhttpを設定")]
    public void Case033()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""type"": ""http"",
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(MyDesiredProperties.Process.AppSetting.Protocol.http, ret.processes[0].application.type);
    }

    [Fact(DisplayName = "Case034_DP:application.typeにHTTPを設定")]
    public void Case034()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""type"": ""HTTP"",
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(MyDesiredProperties.Process.AppSetting.Protocol.http, ret.processes[0].application.type);
    }
    
    [Fact(DisplayName = "Case035_DP:application.urlが存在しない")]
    public void Case035()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                        }}
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'url' is not specified.", exception.Message);
    }

    [Fact(DisplayName = "Case036_DP:application.urlがnull")]
    public void Case036()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": null
                        }}
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'url' cannot be null or empty.", exception.Message);
    }

    [Fact(DisplayName = "Case037_DP:application.urlが空文字")]
    public void Case037()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": """"
                        }}
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'url' cannot be null or empty.", exception.Message);
    }

    [Fact(DisplayName = "Case038_DP:application.urlが存在する")]
    public void Case038()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(url, ret.processes[0].application.url);
    }

    [Fact(DisplayName = "Case039_DP:replace_paramsが存在しない")]
    public void Case039()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Empty(ret.processes[0].application.replace_params);
    }

    [Fact(DisplayName = "Case040_DP:replace_paramsがArray型でない")]
    public void Case040()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": ""{string_test}""
                        }}
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Error retrieving property 'replace_params'", exception.Message);
    }

    [Fact(DisplayName = "Case041_DP:replace_paramsが存在する（1項目）")]
    public void Case041()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": ""{base_name1}"",
                                    ""source_data_type"": ""{MyDesiredProperties.MessageData.Body}"",
                                    ""source_data_key"": ""{source_data_key}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(base_name1, ret.processes[0].application.replace_params[0].base_name);
    }

    [Fact(DisplayName = "Case042_DP:replace_paramsが存在する（2項目）")]
    public void Case042()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": ""{base_name1}"",
                                    ""source_data_type"": ""{MyDesiredProperties.MessageData.Body}"",
                                    ""source_data_key"": ""{source_data_key}""
                                }},
                                {{
                                    ""base_name"": ""{base_name2}"",
                                    ""source_data_type"": ""{MyDesiredProperties.MessageData.Properties}"",
                                    ""source_data_key"": ""{source_data_key}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(base_name1, ret.processes[0].application.replace_params[0].base_name);
        Assert.Equal(base_name2, ret.processes[0].application.replace_params[1].base_name);
    }

    [Fact(DisplayName = "Case043_DP:base_nameが存在しない")]
    public void Case043()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""source_data_type"": ""{MyDesiredProperties.MessageData.Body}"",
                                    ""source_data_key"": ""{source_data_key}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'base_name' is not specified.", exception.Message);
    }

    [Fact(DisplayName = "Case044_DP:base_nameがnull")]
    public void Case044()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": null,
                                    ""source_data_type"": ""{MyDesiredProperties.MessageData.Body}"",
                                    ""source_data_key"": ""{source_data_key}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'base_name' cannot be null or empty.", exception.Message);
    }

    [Fact(DisplayName = "Case045_DP:base_nameが空文字")]
    public void Case045()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": """",
                                    ""source_data_type"": ""{MyDesiredProperties.MessageData.Body}"",
                                    ""source_data_key"": ""{source_data_key}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'base_name' cannot be null or empty.", exception.Message);
    }

    [Fact(DisplayName = "Case046_DP:base_nameに、DP:application.urlに存在しない文字列を設定")]
    public void Case046()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": ""{string_test}"",
                                    ""source_data_type"": ""{MyDesiredProperties.MessageData.Body}"",
                                    ""source_data_key"": ""{source_data_key}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Property 'base_name' did not meet the specified condition.", exception.Message);
    }

    [Fact(DisplayName = "Case047_DP:base_nameに、DP:application.urlに存在する文字列を設定")]
    public void Case047()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": ""{base_name1}"",
                                    ""source_data_type"": ""{MyDesiredProperties.MessageData.Body}"",
                                    ""source_data_key"": ""{source_data_key}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(base_name1, ret.processes[0].application.replace_params[0].base_name);
    }

    [Fact(DisplayName = "Case048_DP:source_data_typeが存在しない")]
    public void Case048()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": ""{base_name1}"",
                                    ""source_data_key"": ""{source_data_key}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'source_data_type' is not specified.", exception.Message);
    }

    [Fact(DisplayName = "Case049_DP:source_data_typeにBody,Properties以外の文字列(test)を設定")]
    public void Case049()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": ""{base_name1}"",
                                    ""source_data_type"": ""{string_test}"",
                                    ""source_data_key"": ""{source_data_key}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Error retrieving property 'source_data_type'", exception.Message);
    }

    [Fact(DisplayName = "Case050_DP:DP:source_data_typeにbodyを設定")]
    public void Case050()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": ""{base_name1}"",
                                    ""source_data_type"": ""body"",
                                    ""source_data_key"": ""{source_data_key}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(MyDesiredProperties.MessageData.Body, ret.processes[0].application.replace_params[0].source_data_type);
    }

    [Fact(DisplayName = "Case051_DP:DP:source_data_typeにBODYを設定")]
    public void Case051()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": ""{base_name1}"",
                                    ""source_data_type"": ""BODY"",
                                    ""source_data_key"": ""{source_data_key}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(MyDesiredProperties.MessageData.Body, ret.processes[0].application.replace_params[0].source_data_type);
    }

    [Fact(DisplayName = "Case052_DP:DP:source_data_typeにpropertiesを設定")]
    public void Case052()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": ""{base_name1}"",
                                    ""source_data_type"": ""properties"",
                                    ""source_data_key"": ""{source_data_key}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(MyDesiredProperties.MessageData.Properties, ret.processes[0].application.replace_params[0].source_data_type);
    }

    [Fact(DisplayName = "Case053_DP:DP:source_data_typeにPROPERTIESを設定")]
    public void Case053()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": ""{base_name1}"",
                                    ""source_data_type"": ""PROPERTIES"",
                                    ""source_data_key"": ""{source_data_key}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(MyDesiredProperties.MessageData.Properties, ret.processes[0].application.replace_params[0].source_data_type);
    }

    [Fact(DisplayName = "Case054_DP:source_data_keyが存在しない")]
    public void Case054()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": ""{base_name1}"",
                                    ""source_data_type"": ""{MyDesiredProperties.MessageData.Body}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'source_data_key' is not specified.", exception.Message);
    }

    [Fact(DisplayName = "Case055_DP:source_data_keyがnull")]
    public void Case055()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": ""{base_name1}"",
                                    ""source_data_type"": ""{MyDesiredProperties.MessageData.Body}"",
                                    ""source_data_key"": null
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'source_data_key' cannot be null or empty.", exception.Message);
    }

    [Fact(DisplayName = "Case056_DP:source_data_keyが空文字")]
    public void Case056()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": ""{base_name1}"",
                                    ""source_data_type"": ""{MyDesiredProperties.MessageData.Body}"",
                                    ""source_data_key"": """"
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'source_data_key' cannot be null or empty.", exception.Message);
    }

    [Fact(DisplayName = "Case057_DP:source_data_keyが存在する")]
    public void Case057()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}"",
                            ""replace_params"": [
                                {{
                                    ""base_name"": ""{base_name1}"",
                                    ""source_data_type"": ""{MyDesiredProperties.MessageData.Body}"",
                                    ""source_data_key"": ""{source_data_key}""
                                }}
                            ]
                        }}
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(source_data_key, ret.processes[0].application.replace_params[0].source_data_key);
    }

    [Fact(DisplayName = "Case058_DP:post_processesが存在しない")]
    public void Case058()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }}
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Empty(ret.processes[0].post_processes);
    }

    [Fact(DisplayName = "Case059_DP:post_processesがArray型でない")]
    public void Case059()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": ""{string_test}""
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Error retrieving property 'post_processes'", exception.Message);
    }

    [Fact(DisplayName = "Case060_DP:post_processesが存在する（1項目）")]
    public void Case060()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}""
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(output_name1, ret.processes[0].post_processes[0].tasks[0].output_name);        
    }

    [Fact(DisplayName = "Case061_DP:post_processesが存在する（2項目）")]
    public void Case061()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}""
                                    }}
                                ]
                            }},
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name2}""
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(output_name1, ret.processes[0].post_processes[0].tasks[0].output_name); 
        Assert.Equal(output_name2, ret.processes[0].post_processes[1].tasks[0].output_name);
    }
    
    [Fact(DisplayName = "Case062_DP:condition_path、DP:condition_operatorが存在しない")]
    public void Case062()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}""
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Null(ret.processes[0].post_processes[0].condition_path);
        Assert.Null(ret.processes[0].post_processes[0].condition_operator);
    }

    [Fact(DisplayName = "Case063_DP:condition_pathが存在し、DP:condition_operatorが存在しない")]
    public void Case063()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""condition_path"": ""{condition_path}"",
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}""
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<Exception>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Both 'condition_path' and 'condition_operator' must be specified together or both omitted.", exception.Message);
    }

    [Fact(DisplayName = "Case064_DP:condition_pathが存在せず、DP:condition_operatorが存在する")]
    public void Case064()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""condition_operator"": ""{condition_operator}"",
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}""
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<Exception>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Both 'condition_path' and 'condition_operator' must be specified together or both omitted.", exception.Message);
    }

    [Fact(DisplayName = "Case065_DP:condition_path、DP:condition_operatorが存在する")]
    public void Case065()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""condition_path"": ""{condition_path}"",
                                ""condition_operator"": ""{condition_operator}"",
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}""
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(condition_path, ret.processes[0].post_processes[0].condition_path);
        Assert.Equal("EQ(\"9\")", ret.processes[0].post_processes[0].condition_operator);
    }

    [Fact(DisplayName = "Case066_DP:condition_operatorに、\"EQ(\"で始まり\")\"で終わる文字列以外を設定")]
    public void Case066()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""condition_path"": ""{condition_path}"",
                                ""condition_operator"": ""{string_test}"",
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}""
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Property 'condition_operator' did not meet the specified condition.", exception.Message);
    }

    [Fact(DisplayName = "Case067_DP:tasksが存在しない")]
    public void Case067()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'tasks' is not specified.", exception.Message);
    }

    [Fact(DisplayName = "Case068_DP:tasksがArray型でない")]
    public void Case068()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": ""{string_test}""
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Error retrieving property 'tasks'", exception.Message);
    }

    [Fact(DisplayName = "Case069_DP:tasksが存在する（1項目）")]
    public void Case069()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}""
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(output_name1, ret.processes[0].post_processes[0].tasks[0].output_name);
    }

    [Fact(DisplayName = "Case070_DP:tasksが存在する（2項目）")]
    public void Case070()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}""
                                    }},
                                    {{
                                        ""output_name"": ""{output_name2}""
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(output_name1, ret.processes[0].post_processes[0].tasks[0].output_name);
        Assert.Equal(output_name2, ret.processes[0].post_processes[0].tasks[1].output_name);
    }

    [Fact(DisplayName = "Case071_DP:output_nameが存在しない")]
    public void Case071()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'output_name' is not specified.", exception.Message);
    }

    [Fact(DisplayName = "Case072_DP:output_nameがnull")]
    public void Case072()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": null
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'output_name' cannot be null or empty.", exception.Message);
    }

    [Fact(DisplayName = "Case073_DP:output_nameが空文字")]
    public void Case073()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": """"
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'output_name' cannot be null or empty.", exception.Message);
    }

    [Fact(DisplayName = "Case074_DP:output_nameが存在する")]
    public void Case074()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}""
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(output_name1, ret.processes[0].post_processes[0].tasks[0].output_name);
    }

    [Fact(DisplayName = "Case075_DP:next_processが存在しない")]
    public void Case075()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}""
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Null(ret.processes[0].post_processes[0].tasks[0].next_process);
    }

    [Fact(DisplayName = "Case076_DP:next_processが存在する")]
    public void Case076()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""next_process"": ""{next_process}""
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(next_process, ret.processes[0].post_processes[0].tasks[0].next_process);
    }

    [Fact(DisplayName = "Case077_DP:set_valuesが存在しない")]
    public void Case077()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}""
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Empty(ret.processes[0].post_processes[0].tasks[0].set_values);
    }

    [Fact(DisplayName = "Case078_DP:set_valuesがArray型でない")]
    public void Case078()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": ""{string_test}""
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Error retrieving property 'set_values'", exception.Message);
    }

    [Fact(DisplayName = "Case079_DP:set_valuesが存在する（1項目）")]
    public void Case079()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""{MyDesiredProperties.MessageData.Body}"",
                                                ""key"": ""{set_values_key1}"",
                                                ""value"": ""{set_values_value}""
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(set_values_key1, ret.processes[0].post_processes[0].tasks[0].set_values[0].key);
    }

    [Fact(DisplayName = "Case080_DP:set_valuesが存在する（2項目）")]
    public void Case080()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""{MyDesiredProperties.MessageData.Body}"",
                                                ""key"": ""{set_values_key1}"",
                                                ""value"": ""{set_values_value}""
                                            }},
                                            {{
                                                ""type"": ""{MyDesiredProperties.MessageData.Properties}"",
                                                ""key"": ""{set_values_key2}"",
                                                ""value"": ""{set_values_value}""
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(set_values_key1, ret.processes[0].post_processes[0].tasks[0].set_values[0].key);
        Assert.Equal(set_values_key2, ret.processes[0].post_processes[0].tasks[0].set_values[1].key);
    }

    [Fact(DisplayName = "Case081_DP:set_values.typeが存在しない")]
    public void Case081()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""key"": ""{set_values_key1}"",
                                                ""value"": ""{set_values_value}""
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'type' is not specified.", exception.Message);
    }

    [Fact(DisplayName = "Case082_DP:set_values.typeにBody,Properties以外の文字列(test)を設定")]
    public void Case082()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""{string_test}"",
                                                ""key"": ""{set_values_key1}"",
                                                ""value"": ""{set_values_value}""
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Error retrieving property 'type'", exception.Message);
    }

    [Fact(DisplayName = "Case083_DP:set_values.typeにbodyを設定")]
    public void Case083()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""body"",
                                                ""key"": ""{set_values_key1}"",
                                                ""value"": ""{set_values_value}""
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(MyDesiredProperties.MessageData.Body, ret.processes[0].post_processes[0].tasks[0].set_values[0].type);
    }

    [Fact(DisplayName = "Case084_DP:set_values.typeにBODYを設定")]
    public void Case084()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""BODY"",
                                                ""key"": ""{set_values_key1}"",
                                                ""value"": ""{set_values_value}""
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(MyDesiredProperties.MessageData.Body, ret.processes[0].post_processes[0].tasks[0].set_values[0].type);
    }

    [Fact(DisplayName = "Case085_DP:set_values.typeにpropertiesを設定")]
    public void Case085()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""properties"",
                                                ""key"": ""{set_values_key1}"",
                                                ""value"": ""{set_values_value}""
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(MyDesiredProperties.MessageData.Properties, ret.processes[0].post_processes[0].tasks[0].set_values[0].type);
    }

    [Fact(DisplayName = "Case086_DP:set_values.typeにPROPERTIESを設定")]
    public void Case086()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""PROPERTIES"",
                                                ""key"": ""{set_values_key1}"",
                                                ""value"": ""{set_values_value}""
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(MyDesiredProperties.MessageData.Properties, ret.processes[0].post_processes[0].tasks[0].set_values[0].type);
    }

    [Fact(DisplayName = "Case087_DP:set_values.keyが存在しない")]
    public void Case087()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""{MyDesiredProperties.MessageData.Body}"",
                                                ""value"": ""{set_values_value}""
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'key' is not specified.", exception.Message);
    }

    [Fact(DisplayName = "Case088_DP:set_values.keyがnull")]
    public void Case088()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""{MyDesiredProperties.MessageData.Body}"",
                                                ""key"": null,
                                                ""value"": ""{set_values_value}""
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'key' cannot be null or empty.", exception.Message);
    }

    [Fact(DisplayName = "Case089_DP:set_values.keyが空文字")]
    public void Case089()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""{MyDesiredProperties.MessageData.Body}"",
                                                ""key"": """",
                                                ""value"": ""{set_values_value}""
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'key' cannot be null or empty.", exception.Message);
    }

    [Fact(DisplayName = "Case090_DP:set_values.keyが存在する")]
    public void Case090()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""{MyDesiredProperties.MessageData.Body}"",
                                                ""key"": ""{set_values_key1}"",
                                                ""value"": ""{set_values_value}""
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(set_values_key1, ret.processes[0].post_processes[0].tasks[0].set_values[0].key);
    }

    [Fact(DisplayName = "Case091_DP:set_values.valueが存在しない")]
    public void Case091()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""{MyDesiredProperties.MessageData.Body}"",
                                                ""key"": ""{set_values_key1}""
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'value' is not specified.", exception.Message);
    }

    [Fact(DisplayName = "Case092_DP:set_values.valueがnull")]
    public void Case092()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""{MyDesiredProperties.MessageData.Body}"",
                                                ""key"": ""{set_values_key1}"",
                                                ""value"": null
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'value' cannot be null or empty.", exception.Message);
    }
    
    [Fact(DisplayName = "Case093_DP:set_values.valueが空文字")]
    public void Case093()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""{MyDesiredProperties.MessageData.Body}"",
                                                ""key"": ""{set_values_key1}"",
                                                ""value"": """"
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var exception = Assert.Throws<ArgumentException>(() => MyDesiredProperties.GetDesiredProperties(props));
        
        Assert.Contains("Required property 'value' cannot be null or empty.", exception.Message);
    }
    
    [Fact(DisplayName = "Case094_DP:set_values.valueが存在する")]
    public void Case094()
    {
        var props = JObject.Parse(
            $@"{{ 
                ""input_name"": ""{input_name}"",
                ""processes"": [
                    {{
                        ""process_name"": ""{process1}"",
                        ""application"": {{
                            ""url"": ""{url}""
                        }},
                        ""post_processes"": [
                            {{
                                ""tasks"": [
                                    {{
                                        ""output_name"": ""{output_name1}"",
                                        ""set_values"": [
                                            {{
                                                ""type"": ""{MyDesiredProperties.MessageData.Body}"",
                                                ""key"": ""{set_values_key1}"",
                                                ""value"": ""{set_values_value}""
                                            }}
                                        ]
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}"
        );
        
        var ret = MyDesiredProperties.GetDesiredProperties(props);

        Assert.Equal(set_values_value, ret.processes[0].post_processes[0].tasks[0].set_values[0].value);
    }
}
