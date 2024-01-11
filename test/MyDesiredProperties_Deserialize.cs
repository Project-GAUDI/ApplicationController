using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ApplicationController.Test
{
    public class MyDesiredProperties_Deserialize
    {
        private readonly ITestOutputHelper _output;

        public MyDesiredProperties_Deserialize(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "正常系:DesiredPropertiesインスタンス生成→DesiredPropertiesインスタンス生成")]
        public void SimpleValue_MyDesiredPropertiesCreated()
        {
            string desiredPropertyStr = "{}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.IsAssignableFrom<MyDesiredProperties>(result);
        }

        [Fact(DisplayName = "正常系:input_nameがない→input_nameはnull")]
        public void NoInputName_InputNameIsNull()
        {
            string desiredPropertyStr = "{}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Null(result.input_name);
        }

        [Fact(DisplayName = "正常系:input_nameが\"A\"→input_nameは\"A\"")]
        public void InputNameIsString_InputNameIsA()
        {
            string desiredPropertyStr = "{\"input_name\":\"A\"}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("A", result.input_name);
        }

        [Fact(DisplayName = "正常系:input_nameが1→input_nameは\"1\"")]
        public void InputNameIsInt_InputNameIs1()
        {
            string desiredPropertyStr = "{\"input_name\":1}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("1", result.input_name);
        }

        [Fact(DisplayName = "異常系:input_nameが[\"A\",\"B\"]→例外")]
        public void InputNameIsArray_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":[\"A\",\"B\"]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:processesがない→processesの要素数は0")]
        public void NoProcesses_ProcessesIsEmpty()
        {
            string desiredPropertyStr = "{}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Empty(result.processes);
        }

        [Fact(DisplayName = "異常系:processesが\"A\"→例外")]
        public void ProcessesIsString_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":\"A\"}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:processesの要素数が0→processesの要素数は0")]
        public void ProcessesIsEmpty_ProcessesIsEmpty()
        {
            string desiredPropertyStr = "{\"processes\":[]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Empty(result.processes);
        }

        [Fact(DisplayName = "正常系:processesがあり、process_nameがない→process_nameはnull")]
        public void NoProcessName_ProcessNameIsNull()
        {
            string desiredPropertyStr = "{\"processes\":[{}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Null(result.processes[0].process_name);
        }


        [Fact(DisplayName = "正常系:processesがあり、process_nameが\"A\"→process_nameは\"A\"")]
        public void ProcessNameIsString_ProcessNameIsA()
        {
            string desiredPropertyStr = "{\"processes\":[{\"process_name\":\"A\"}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("A", result.processes[0].process_name);
        }


        [Fact(DisplayName = "正常系:processesがあり、process_nameが1→process_nameは\"1\"")]
        public void ProcessNameIsInt_ProcessNameIs1()
        {
            string desiredPropertyStr = "{\"processes\":[{\"process_name\":1}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("1", result.processes[0].process_name);
        }

        [Fact(DisplayName = "異常系:processesがあり、process_nameが[\"A\",\"B\"]→例外")]
        public void ProcessNameIsArray_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"process_name\":[\"A\", \"B\"]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }


        [Fact(DisplayName = "正常系:processesがあり、applicationがない→applicationはnull")]
        public void NoApplication_ApplicationIsNull()
        {
            string desiredPropertyStr = "{\"processes\":[{\"process_name\":1}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Null(result.processes[0].application);
        }

        [Fact(DisplayName = "異常系::processesがあり、applicationが\"A\"→例外")]
        public void ApplicationIsString_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":\"A\"}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:applicationがありtypeがない→typeはNotSelected")]
        public void NoApplicationType_TypeIsNotSelected()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal(MyDesiredProperties.Process.AppSetting.Protocol.NotSelected, result.processes[0].application.type);
        }

        [Fact(DisplayName = "正常系:applicationがありtypeが\"http\"→typeはhttp")]
        public void ApplicationTypeIsHttp_TypeIsHttp()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"type\":\"http\"}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal(MyDesiredProperties.Process.AppSetting.Protocol.http, result.processes[0].application.type);
        }

        [Fact(DisplayName = "異常系:applicationがありtypeが\"A\"→例外")]
        public void ApplicationTypeIsA_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"type\":\"A\"}}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:applicationがあり、urlがない→urlはnull")]
        public void NoUrl_UrlIsNull()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Null(result.processes[0].application.url);
        }

        [Fact(DisplayName = "正常系:applicationがあり、urlが\"A\"→urlは\"A\"")]
        public void UrlIsString_UrlIsA()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"url\":\"A\"}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("A", result.processes[0].application.url);
        }

        [Fact(DisplayName = "正常系:applicationがあり、urlが1→urlは\"1\"")]
        public void UrlIsInt_UrlIs1()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"url\":1}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("1", result.processes[0].application.url);
        }

        [Fact(DisplayName = "異常系:applicationがあり、urlが[\"A\",\"B\"]→例外")]
        public void UrlIsArray_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"url\":[\"A\",\"B\"]}}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:applicationがあり、replace_paramsがない→replace_paramsの要素数が0")]
        public void NoReplaceParams_ReplaceParamsIsEmpty()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Empty(result.processes[0].application.replace_params);
        }

        [Fact(DisplayName = "異常系:applicationがあり、replace_paramsが\"A\"→例外")]
        public void ReplaceParamsIsString_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":\"A\"}}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:applicationがあり、replace_paramsの要素数が0→replace_paramsの要素数は0")]
        public void ReplaceParamsIsEmpty_ReplaceParamsIsEmpty()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[]}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Empty(result.processes[0].application.replace_params);
        }

        [Fact(DisplayName = "正常系:replace_paramsがあり、base_nameがない→base_nameはnull")]
        public void NoBaseName_BaseNameIsNull()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{}]}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Null(result.processes[0].application.replace_params[0].base_name);
        }

         [Fact(DisplayName = "正常系:replace_paramsがあり、base_nameが\"A\"→base_nameは\"A\"")]
        public void BaseNameIsString_BaseNameIsA()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{ \"base_name\":\"A\" }]}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("A", result.processes[0].application.replace_params[0].base_name);
        }

         [Fact(DisplayName = "正常系:replace_paramsがあり、base_nameが1→base_nameは\"1\"")]
        public void BaseNameIsInt_BaseNameIs1()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{ \"base_name\":1 }]}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("1", result.processes[0].application.replace_params[0].base_name);
        }

        [Fact(DisplayName = "異常系:replace_paramsがあり、base_nameが[\"A\",\"B\"]→例外")]
        public void BaseNameIsArray_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{ \"base_name\":[\"A\" , \"B\"] }]}}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:replace_paramsがあり、source_data_typeがない→source_data_typeはNotSelected")]
        public void NoSourceDataType_SourceDataTypeIsNotSelected()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{}]}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal(MyDesiredProperties.MessageData.NotSelected, result.processes[0].application.replace_params[0].source_data_type);
        }

        [Fact(DisplayName = "正常系:replace_paramsがあり、source_data_typeが\"Body\"→source_data_typeは\"Body\"")]
        public void SourceDataTypeIsBody_SourceDataTypeIsBody()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{ \"source_data_type\":\"Body\" }]}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal(MyDesiredProperties.MessageData.Body, result.processes[0].application.replace_params[0].source_data_type);
        }

        [Fact(DisplayName = "正常系:replace_paramsがあり、source_data_typeが\"Properties\"→source_data_typeは\"Properties\"")]
        public void SourceDataTypeIsProperties_SourceDataTypeIsProperties()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{ \"source_data_type\":\"Properties\" }]}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal(MyDesiredProperties.MessageData.Properties, result.processes[0].application.replace_params[0].source_data_type);
        }

        [Fact(DisplayName = "異常系:replace_paramsがあり、source_data_typeが\"A\"→例外")]
        public void SourceDataTypeIsA_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{ \"source_data_type\":\"A\" }]}}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:replace_paramsがあり、source_data_keyがない→source_data_keyはnull")]
        public void NoSourceDataValue_SourceDataValueIsNull()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{}]}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Null(result.processes[0].application.replace_params[0].source_data_key);
        }


        [Fact(DisplayName = "正常系:replace_paramsがあり、source_data_keyが\"$.A.B\"→source_data_keyは\"$.A.B\"")]
        public void SourceDataValueIsJsonPath_SourceDataValueIsJsonPath()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{ \"source_data_key\":\"$.A.B\" }]}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("$.A.B", result.processes[0].application.replace_params[0].source_data_key);
        }

        [Fact(DisplayName = "正常系:replace_paramsがあり、source_data_keyが1→source_data_keyは\"1\"")]
        public void SourceDataValueIsInt_SourceDataValueIs1()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{ \"source_data_key\":1 }]}}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("1", result.processes[0].application.replace_params[0].source_data_key);
        }

        [Fact(DisplayName = "異常系:replace_paramsがあり、source_data_keyが[\"A\",\"B\"]→例外")]
        public void SourceDataValueIsArray_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{ \"source_data_key\":[\"A\",\"B\"] }]}}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }




        [Fact(DisplayName = "正常系:processesがあり、post_processesがない→post_processesの要素数が0")]
        public void NoPostProcesses_PostProcessIsNull()
        {
            string desiredPropertyStr = "{\"processes\":[{}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Empty(result.processes[0].post_processes);
        }

        [Fact(DisplayName = "異常系:processesがあり、post_processesが\"A\"→例外")]
        public void PostProcessesIsString_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":\"A\"}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:processesがあり、post_processesの要素数が0→post_processesの要素数は0")]
        public void PostProcessesIsEmpty_PostProcessesIsEmpty()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Empty(result.processes[0].post_processes);
        }

        [Fact(DisplayName = "正常系:post_processesがあり、condition_pathがない→condition_pathはnull")]
        public void NoConditionPath_ConditionPathIsNull()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Null(result.processes[0].post_processes[0].condition_path);
        }

        [Fact(DisplayName = "正常系:post_processesがあり、condition_pathが\"$.A.B\"→condition_pathは\"$.A.B\"")]
        public void ConditionPathIsJsonPath_ConditionPathIsJsonPath()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"condition_path\": \"$.A.B\"}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("$.A.B", result.processes[0].post_processes[0].condition_path);
        }

        [Fact(DisplayName = "正常系:post_processesがあり、condition_pathが1→condition_pathは\"1\"")]
        public void ConditionPathIsInt_ConditionPathIs1()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"condition_path\": 1}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("1", result.processes[0].post_processes[0].condition_path);
        }

        [Fact(DisplayName = "異常系:post_processesがあり、condition_pathが[\"A\",\"B\"]→例外")]
        public void ConditionPathIsArray_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"condition_path\": [\"A\",\"B\"]}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:post_processesがあり、condition_operatorがない→condition_operatorはnull")]
        public void ConditionValueIsEmpty_ConditionValueIsNull()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Null(result.processes[0].post_processes[0].condition_operator);
        }

        [Fact(DisplayName = "正常系:post_processesがあり、condition_operatorが\"A\"→condition_operatorは\"A\"")]
        public void ConditionValueIsString_ConditionValueIsA()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"condition_operator\":\"A\"}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("A",result.processes[0].post_processes[0].condition_operator);
        }

        [Fact(DisplayName = "正常系:post_processesがあり、condition_operatorが1→condition_operatorは\"1\"")]
        public void ConditionValueIsInt_ConditionValueIs1()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"condition_operator\":1}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("1",result.processes[0].post_processes[0].condition_operator);
        }

        [Fact(DisplayName = "異常系:post_processesがあり、condition_operatorが[\"A\",\"B\"]→例外")]
        public void ConditionValueIsArray_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"condition_operator\":[\"A\",\"B\"]}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:post_processesがあり、tasksがない→tasksの要素数は0")]
        public void NoTasks_TasksIsEmpty()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Empty(result.processes[0].post_processes[0].tasks);
        }

        [Fact(DisplayName = "異常系:post_processesがあり、tasksが\"A\"→例外")]
        public void TasksIsString_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":\"A\"}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:post_processesがあり、tasksの要素数が0→tasksの要素数は0")]
        public void TasksIsEmpty_TasksIsEmpty()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Empty(result.processes[0].post_processes[0].tasks);
        }

        [Fact(DisplayName = "正常系:tasksがあり、output_nameがない→output_nameはnull")]
        public void NoOutputName_OutputNameIsNull()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Null(result.processes[0].post_processes[0].tasks[0].output_name);
        }

        [Fact(DisplayName = "正常系:tasksがあり、output_nameが\"A\"→output_nameは\"A\"")]
        public void OutputNameIsString_OutputNameIsA()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"output_name\":\"A\"}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("A",result.processes[0].post_processes[0].tasks[0].output_name);
        }

        [Fact(DisplayName = "正常系:tasksがあり、output_nameが1→output_nameは\"1\"")]
        public void OutputNameIsInt_OutputNameIs1()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"output_name\":1}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("1",result.processes[0].post_processes[0].tasks[0].output_name);
        }

        [Fact(DisplayName = "異常系:tasksがあり、output_nameが[\"A\",\"B\"]→例外")]
        public void OutputNameIsArray_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"output_name\":[\"A\",\"B\"]}]}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:tasksがあり、next_processがない→next_processはnull")]
        public void NoNextProcess_NextProcessIsNull()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Null(result.processes[0].post_processes[0].tasks[0].next_process);
        }

        [Fact(DisplayName = "正常系:tasksがあり、next_processが\"A\"→next_processは\"A\"")]
        public void NextProcessIsString_NextProcessIsA()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"next_process\":\"A\"}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("A",result.processes[0].post_processes[0].tasks[0].next_process);
        }

        [Fact(DisplayName = "正常系:tasksがあり、next_processが1→next_processは\"1\"")]
        public void NextProcessIsInt_NextProcessIs1()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"next_process\":1}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("1",result.processes[0].post_processes[0].tasks[0].next_process);
        }

        [Fact(DisplayName = "異常系:tasksがあり、next_processが[\"A\",\"B\"]→例外")]
        public void NextProcessIsArray_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"next_process\":[\"A\",\"B\"]}]}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:tasksがあり、set_valuesがない→set_valuesの要素数は0")]
        public void NoSetValues_SetValuesIsEmpty()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Empty(result.processes[0].post_processes[0].tasks[0].set_values);
        }

        [Fact(DisplayName = "異常系:tasksがあり、set_valuesが\"A\"→例外")]
        public void SetValuesIsString_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":\"A\"}]}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:tasksがあり、set_valuesの要素数が0→set_valuesの要素数は0")]
        public void SetValuesIsEmpty_SetValuesIsEmpty()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Empty(result.processes[0].post_processes[0].tasks[0].set_values);
        }

        [Fact(DisplayName = "正常系:set_valuesがあり、typeがない→typeはNotSelected")]
        public void NoSetValueType_SetValueTypeIsNotSelected()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal(MyDesiredProperties.MessageData.NotSelected, result.processes[0].post_processes[0].tasks[0].set_values[0].type);
        }

        [Fact(DisplayName = "正常系:set_valuesがあり、typeが\"Body\"→typeは\"Body\"")]
        public void SetValueTypeIsBody_SetValueTypeIsBody()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{\"type\": \"Body\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal(MyDesiredProperties.MessageData.Body, result.processes[0].post_processes[0].tasks[0].set_values[0].type);
        }

        [Fact(DisplayName = "正常系:set_valuesがあり、typeが\"Properties\"→typeは\"Properties\"")]
        public void SetValueTypeIsProperties_SetValueTypeIsProperties()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{\"type\": \"Properties\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal(MyDesiredProperties.MessageData.Properties, result.processes[0].post_processes[0].tasks[0].set_values[0].type);
        }

        [Fact(DisplayName = "異常系:set_valuesがあり、typeが\"A\"→例外")]
        public void SetValueTypeIsA_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{\"type\": \"A\"}]}]}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }



        [Fact(DisplayName = "正常系:set_valuesがあり、keyがない→keyはnull")]
        public void NoSetValuesKey_SetValuesKeyIsNull()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Null(result.processes[0].post_processes[0].tasks[0].set_values[0].key);
        }

        [Fact(DisplayName = "正常系:set_valuesがあり、keyが\"$.A.B\"→keyは\"$.A.B\"")]
        public void SetValuesKeyIsJsonPath_SetValuesKeyIsJsonPath()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{\"key\":\"$.A.B\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("$.A.B", result.processes[0].post_processes[0].tasks[0].set_values[0].key);
        }

        [Fact(DisplayName = "正常系:set_valuesがあり、keyが1→keyは\"1\"")]
        public void SetValuesKeyIsInt_SetValuesKeyIs1()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{\"key\":1}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("1", result.processes[0].post_processes[0].tasks[0].set_values[0].key);
        }

        [Fact(DisplayName = "異常系:set_valuesがあり、keyが[\"A\",\"B\"]→例外")]

        public void SetValuesKeyIsArray_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{\"key\":[\"A\",\"B\"]}]}]}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }



        [Fact(DisplayName = "正常系:set_valuesがあり、valueがない→valueはnull")]
        public void NoSetValuesValue_SetValuesValueIsNull()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Null(result.processes[0].post_processes[0].tasks[0].set_values[0].value);
        }

        [Fact(DisplayName = "正常系:set_valuesがあり、valueが\"$.A.B\"→valueは\"$.A.B\"")]
        public void SetValuesValueIsJsonPath_SetValuesValueIsJsonPath()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{\"value\":\"$.A.B\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("$.A.B", result.processes[0].post_processes[0].tasks[0].set_values[0].value);
        }

        [Fact(DisplayName = "正常系:set_valuesがあり、valueが1→valueは\"1\"")]
        public void SetValuesValueIsInt_SetValuesValueIs1()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{\"value\":1}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Equal("1", result.processes[0].post_processes[0].tasks[0].set_values[0].value);
        }

        [Fact(DisplayName = "異常系:set_valuesがあり、valueが[\"A\",\"B\"]→例外")]
        public void SetValuesValueIsArray_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{\"value\":[\"A\",\"B\"]}]}]}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }




        [Fact(DisplayName = "正常系:processesの要素数が3で全要素が正しい→正常終了")]
        public void ProcessesLengthIs3_Succeeded()
        {
            string desiredPropertyStr = "{\"processes\":[{},{},{}]}";
            MyDesiredProperties.Deserialize(desiredPropertyStr);
        }


        [Fact(DisplayName = "異常系:processesの要素数が3で最初の要素のprocess_nameが[\"A\",\"B\"]→例外")]
        public void ProcessesLengthIs3AndFirstProcessIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"process_name\":[\"A\",\"B\"]},{},{}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "異常系:processesの要素数が3で2つめの要素のapplicationが\"A\"→例外")]
        public void ProcessesLengthIs3AndSecondProcessIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{},{\"application\":\"A\"},{}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "異常系:processesの要素数が3で3つめの要素のtypeが[\"A\",\"B\"]→例外")]
        public void ProcessesLengthIs3AndThirdProcessIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{},{},{\"application\":{\"type\":[\"A\",\"B\"]}}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:replace_paramsの要素数が3で全要素が正しい→正常終了")]
        public void ReplaceParamsLengthIs3_Succeeded()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{},{},{}]}}]}";
            MyDesiredProperties.Deserialize(desiredPropertyStr);
        }

        [Fact(DisplayName = "異常系:replace_paramsの要素数が3で最初の要素のbase_nameが[\"A\",\"B\"]→例外")]
        public void ReplaceParamsLengthIs3AndFirstReplaceParamIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{\"base_name\":[\"A\",\"B\"]},{},{}]}}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "異常系:replace_paramの要素数が3で2つめの要素のsource_data_typeが\"A\"→例外")]
        public void ReplaceParamsLengthIs3AndSecondPReplaceParamIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{},{\"source_data_type\":\"A\"},{}]}}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "異常系:replace_paramの要素数が3で3つめの要素のsource_data_keyが[\"A\",\"B\"]→例外")]
        public void ReplaceParamsLengthIs3AndThirdReplaceParamIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"application\":{\"replace_params\":[{},{},{\"source_data_key\":[\"A\",\"B\"]}]}}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:post_processesの要素数が3で全要素が正しい→正常終了")]
        public void PostProcessesLengthIs3_Succeeded()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{},{},{}]}]}";
            MyDesiredProperties.Deserialize(desiredPropertyStr);
        }

        [Fact(DisplayName = "異常系:post_processesの要素数が3で最初の要素のcondition_pathが[\"A\",\"B\"]→例外")]
        public void PostProcessesLengthIs3AndFirstPostProcessIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"condition_path\":[\"A\",\"B\"]},{},{}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "異常系:post_processesの要素数が3で2つめの要素のcondition_operatorが[\"A\",\"B\"]→例外")]
        public void PostProcessesLengthIs3AndSecondPostProcessIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{},{\"condition_operator\":[\"A\",\"B\"]},{}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "異常系:post_processesの要素数が3で3つめの要素のtasksが\"A\"→例外")]
        public void PostProcessesLengthIs3AndThirdPostProcessIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{},{},{\"tasks\":\"A\"}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "正常系:tasksの要素数が3で全要素が正しい→正常終了")]
        public void TasksLengthIs3_Succeeded()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{},{},{}]}]}]}";
            MyDesiredProperties.Deserialize(desiredPropertyStr);
        }

        [Fact(DisplayName = "異常系:tasksの要素数が3で最初の要素のoutput_nameが[\"A\",\"B\"]→例外")]
        public void TasksLengthIs3AndFirstTaskIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"output_name\":[\"A\",\"B\"]},{},{}]}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "異常系:tasksの要素数が3で2つめの要素のnext_processが[\"A\",\"B\"]→例外")]
        public void TasksLengthIs3AndSecondTaskIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{},{\"next_process\":[\"A\",\"B\"]},{}]}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "異常系:tasksの要素数が3で3つめの要素のset_valuesが\"A\"→例外")]
        public void TasksLengthIs3AndThirdTaskIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{},{},{\"set_values\":\"A\"}]}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }





        [Fact(DisplayName = "正常系:set_valuesの要素数が3で全要素が正しい→正常終了")]
        public void SetValuesLengthIs3_Succeeded()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{},{},{}]}]}]}]}";
            MyDesiredProperties.Deserialize(desiredPropertyStr);
        }

        [Fact(DisplayName = "異常系:set_valuesの要素数が3で最初の要素のtypeが\"A\"→例外")]
        public void SetValuesLengthIs3AndFirstSetValueIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{\"type\":\"A\"},{},{}]}]}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "異常系:set_valuesの要素数が3で2つめの要素のkeyが[\"A\",\"B\"]→例外")]
        public void SetValuesLengthIs3AndSecondSetValueIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{},{\"key\":[\"A\",\"B\"]},{}]}]}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        [Fact(DisplayName = "異常系:set_valuesの要素数が3で3つめの要素のvalueが[\"A\",\"B\"]→例外")]
        public void SetValuesLengthIs3AndThirdSetValueIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"post_processes\":[{\"tasks\":[{\"set_values\":[{},{},{\"value\": [\"A\",\"B\"]}]}]}]}]}";
            Assert.Throws<Exception>(() => { MyDesiredProperties.Deserialize(desiredPropertyStr); });
        }

        
    }
}
