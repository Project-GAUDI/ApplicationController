using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IotedgeV2ApplicationController.Test
{
    public class Program_CheckDesiredProperties
    {
        private readonly ITestOutputHelper _output;

        public Program_CheckDesiredProperties(ITestOutputHelper output)
        {
            _output = output;
        }

        // desiredプロパティ文字列作る用
        // var des = MyDesiredPropertiesCreater.Create();
        // string str = Newtonsoft.Json.JsonConvert.SerializeObject(des);
        // _output.WriteLine("str:\n"+ str);
        // Assert.Equal("", str);

        [Fact(DisplayName = "正常系:input_nameがnullでも空文字でもない→正常終了")]
        public void InputNameIsNotNull_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:input_nameがnull→例外")]
        public void InputNameIsNotNull_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "異常系:input_nameが空文字→例外")]
        public void InputNameIsEmpty_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:processesの要素数が1→正常終了")]

        public void ProcessesIsNotNull_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:processesの要素数が0→例外")]
        public void ProcessesCountIs0_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:process_nameがnullでも空文字でもない→正常終了")]
        public void ProcessNameIsNotNullorEmpty_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:process_nameがnull→例外")]
        public void ProcessNameIsNull_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "異常系:process_nameが空文字→例外")]
        public void ProcessNameIsEmpty_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:applicationがnullではない→正常終了")]
        public void ApplicationIsNotNull_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:applicationがnull→例外")]
        public void ApplicationIsNull_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:typeがhttp→正常終了")]
        public void TypeIsHttp_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:typeがNotSelected→例外")]
        public void TypeIsNotSelected_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"url\":\"http://base.bbb/ccc\",\"replace_params\":[]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:typeが\"http\"で、urlがnullでも空文字でもない→正常終了")]
        public void TypeIsHttpAndUrlIsNotNullOrEmpty_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:typeが\"http\"で、urlがnull→例外")]
        public void TypeIsHttpAndUrlIsNull_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"replace_params\":[]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "異常系:typeが\"http\"で、urlが空文字→例外")]
        public void TypeIsHttpAndUrlIsEmpty_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"\",\"replace_params\":[]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:typeが\"http\"で、replace_paramsの要素数が0→正常終了")]
        public void TypeIsHttpAndReplaceParamsIsEmpty_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "正常系:typeが\"http\"で、replace_paramsの要素数が1以上、base_name・source_data_type・source_data_keyがnullでも空文字でもない→正常終了")]
        public void TypeIsHttpAndReplaceParamsIsDefault_Succeeded()
        {

            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Body\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:typeが\"http\"で、replace_paramsの要素数が1以上、base_nameがnull→例外")]
        public void TypeIsHttpAndReplaceParamsBaseNameIsNull_ExceptionThrown()
        {

            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"source_data_type\":\"Body\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "異常系:typeが\"http\"で、replace_paramsの要素数が1以上、base_nameが空文字→例外")]
        public void TypeIsHttpAndReplaceParamsBaseNameIsEmpty_ExceptionThrown()
        {

            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"\",\"source_data_type\":\"Body\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:typeが\"http\"で、replace_paramsの要素数が1以上、source_data_typeがBody→正常終了")]
        public void TypeIsHttpAndReplaceParamsSourceDataTypeIsBody_Succeeded()
        {

            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Body\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "正常系:typeが\"http\"で、replace_paramsの要素数が1以上、source_data_typeがProperties→正常終了")]
        public void TypeIsHttpAndReplaceParamsSourceDataTypeIsProperties_Succeeded()
        {

            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:typeが\"http\"で、replace_paramsの要素数が1以上、source_data_typeがNotSelected→例外")]
        public void TypeIsHttpAndReplaceParamsSourceDataTypeIsNotSelected_ExceptionThrown()
        {

            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "異常系:typeが\"http\"で、replace_paramsの要素数が1以上、source_data_keyがnull→例外")]
        public void TypeIsHttpAndReplaceParamsSourceDataKeyIsNull_ExceptionThrown()
        {

            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\"}]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "異常系:typeが\"http\"で、replace_paramsの要素数が1以上、source_data_keyが空文字→例外")]
        public void TypeIsHttpAndReplaceParamsSourceDataKeyIsEmpty_ExceptionThrown()
        {

            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"\"}]},\"post_processes\":[{\"condition_path\":null,\"condition_operator\":null,\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:post_processesの要素数が0→正常終了")]
        public void PostProcessesIsEmpty_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[]},\"post_processes\":[]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "正常系:condition_pathとcondition_operatorがnullでも空文字でもない→正常終了")]
        public void ConditionPathAndConditionValueIsNotNullOrEmpty_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "正常系:condition_pathとcondition_operatorがnull→正常終了")]
        public void ConditionPathAndConditionValueIsNull_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:condition_pathがnull、valueはnullではない→例外")]
        public void ConditionPathIsNullAndConditionValueIsNotNull_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "異常系:condition_pathがnullではない、condition_operatorがnull→例外")]
        public void ConditionPathIsNotNullAndConditionValueIsNull_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "異常系:condition_pathが空文字、valueは空文字ではない→例外")]
        public void ConditionPathIsEmptyAndConditionValueIsNotEmpty_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "異常系:condition_pathが空文字ではない、condition_operatorが空文字→例外")]
        public void ConditionPathIsNotEmptyAndConditionValueIsEmpty_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:condition_operatorが\"EQ(A)\"→正常終了")]
        public void ConditionOperatorIsEQA_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"A\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "正常系:condition_operatorが\"EQ(A,B)\"→正常終了")]
        public void ConditionOperatorIsEQAB_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"A\\\",\\\"B\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:condition_operatorが\"EQ(A)\"以外の文字列(\"EQU(A)\"など)(Aは可変)→例外")]
        public void ConditionOperatorIsNotEQA_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQU(\\\"A\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:tasksがnullではない→正常終了")]
        public void TasksIsNotNull_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:tasksの要素数が0→例外")]
        public void TasksIsEmpty_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:output_nameがnullでも空文字でもない→正常終了")]
        public void OutputNameIsNotNull_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:output_nameがnull→例外")]
        public void OutputNameIsNull_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }


        [Fact(DisplayName = "異常系:output_nameが空文字→例外")]
        public void OutputNameIsEmpty_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"\",\"next_process\":null,\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:next_processがnullでも空文字でもない→正常終了")]
        public void NextProcessIsNotNull_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }


        [Fact(DisplayName = "正常系:next_processがnull→正常終了")]
        public void NextProcessIsNull_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "正常系:next_processが空文字→正常終了")]
        public void NextProcessIsEmpty_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"\",\"set_values\":[{\"type\":1,\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "正常系:set_valuesの要素数が0→正常終了")]
        public void SetValuesIsEmpty_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "正常系:typeがBody→正常終了")]
        public void SetValuesTypeIsBody_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "正常系:typeがProperties→正常終了")]
        public void SetValuesTypeIsProperties_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Properties\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:typeがNotSelected→例外")]
        public void SetValuesTypeIsNotSelected_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"NotSelected\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:keyがnullでも空文字でもない→正常終了")]
        public void SetValuesKeyIsNotNull_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:keyがnull→例外")]
        public void SetValuesKeyIsNull_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "異常系:keyが空文字→例外")]
        public void SetValuesKeyIsEmpty_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:valueがnullでも空文字でもない→正常終了")]
        public void SetValuesValueIsNotNull_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:valueがnull→例外")]
        public void SetValuesValueIsNull_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "異常系:valueが空文字→例外")]
        public void SetValuesValueIsEmpty_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }

        [Fact(DisplayName = "正常系:processesの要素数が3で全要素が正しい→正常終了")]
        public void ProcessesLengthIs3_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\": \"input_name_value\",\"processes\": [{\"process_name\": \"test1\",\"application\": {\"type\": \"http\",\"url\": \"http://base.bbb/ccc\",\"replace_params\": [{\"base_name\": \"base\",\"source_data_type\": \"Properties\",\"source_data_key\": \"$.A\"}]},\"post_processes\": [{\"condition_path\": \"$.XXX\",\"condition_operator\": \"EQ(\\\"condition_operator_value\\\")\",\"tasks\": [{\"output_name\": \"output1\",\"next_process\": \"test2\",\"set_values\": [{\"type\": \"Body\",\"key\": \"$.A\",\"value\": \"ABC\"}]}]}]},{\"process_name\": \"test2\",\"application\": {\"type\": \"http\",\"url\": \"http://base.bbb/ccc\",\"replace_params\": [{\"base_name\": \"base\",\"source_data_type\": \"Properties\",\"source_data_key\": \"$.A\"}]},\"post_processes\": [{\"condition_path\": \"$.XXX\",\"condition_operator\": \"EQ(\\\"condition_operator_value\\\")\",\"tasks\": [{\"output_name\": \"output1\",\"next_process\": \"test2\",\"set_values\": [{\"type\": \"Body\",\"key\": \"$.A\",\"value\": \"ABC\"}]}]}]},{\"process_name\": \"test3\",\"application\": {\"type\": \"http\",\"url\": \"http://base.bbb/ccc\",\"replace_params\": [{\"base_name\": \"base\",\"source_data_type\": \"Properties\",\"source_data_key\": \"$.A\"}]},\"post_processes\": [{\"condition_path\": \"$.XXX\",\"condition_operator\": \"EQ(\\\"condition_operator_value\\\")\",\"tasks\": [{\"output_name\": \"output1\",\"next_process\": \"test2\",\"set_values\": [{\"type\": \"Body\",\"key\": \"$.A\",\"value\": \"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:processesの要素数が3で最初の要素のprocess_nameがnull→例外")]
        public void ProcessesLengthIs3AndFirstProcessIsIllegal_ExceptioThrown()
        {
            string desiredPropertyStr = "{\"input_name\": \"input_name_value\",\"processes\": [{\"application\": {\"type\": \"http\",\"url\": \"http://base.bbb/ccc\",\"replace_params\": [{\"base_name\": \"base\",\"source_data_type\": \"Properties\",\"source_data_key\": \"$.A\"}]},\"post_processes\": [{\"condition_path\": \"$.XXX\",\"condition_operator\": \"EQ(\\\"condition_operator_value\\\")\",\"tasks\": [{\"output_name\": \"output1\",\"next_process\": \"test2\",\"set_values\": [{\"type\": \"Body\",\"key\": \"$.A\",\"value\": \"ABC\"}]}]}]},{\"process_name\": \"test2\",\"application\": {\"type\": \"http\",\"url\": \"http://base.bbb/ccc\",\"replace_params\": [{\"base_name\": \"base\",\"source_data_type\": \"Properties\",\"source_data_key\": \"$.A\"}]},\"post_processes\": [{\"condition_path\": \"$.XXX\",\"condition_operator\": \"EQ(\\\"condition_operator_value\\\")\",\"tasks\": [{\"output_name\": \"output1\",\"next_process\": \"test2\",\"set_values\": [{\"type\": \"Body\",\"key\": \"$.A\",\"value\": \"ABC\"}]}]}]},{\"process_name\": \"test3\",\"application\": {\"type\": \"http\",\"url\": \"http://base.bbb/ccc\",\"replace_params\": [{\"base_name\": \"base\",\"source_data_type\": \"Properties\",\"source_data_key\": \"$.A\"}]},\"post_processes\": [{\"condition_path\": \"$.XXX\",\"condition_operator\": \"EQ(\\\"condition_operator_value\\\")\",\"tasks\": [{\"output_name\": \"output1\",\"next_process\": \"test2\",\"set_values\": [{\"type\": \"Body\",\"key\": \"$.A\",\"value\": \"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("process_name is null or empty.", ex.Message);
        }

        [Fact(DisplayName = "異常系:processesの要素数が3で2つめの要素のapplicationがnull→例外")]
        public void ProcessesLengthIs3AndSecondProcessIsIllegal_ExceptioThrown()
        {
            string desiredPropertyStr = "{\"input_name\": \"input_name_value\",\"processes\": [{\"process_name\": \"test1\",\"application\": {\"type\": \"http\",\"url\": \"http://base.bbb/ccc\",\"replace_params\": [{\"base_name\": \"base\",\"source_data_type\": \"Properties\",\"source_data_key\": \"$.A\"}]},\"post_processes\": [{\"condition_path\": \"$.XXX\",\"condition_operator\": \"EQ(\\\"condition_operator_value\\\")\",\"tasks\": [{\"output_name\": \"output1\",\"next_process\": \"test2\",\"set_values\": [{\"type\": \"Body\",\"key\": \"$.A\",\"value\": \"ABC\"}]}]}]},{\"process_name\": \"test2\",\"post_processes\": [{\"condition_path\": \"$.XXX\",\"condition_operator\": \"EQ(\\\"condition_operator_value\\\")\",\"tasks\": [{\"output_name\": \"output1\",\"next_process\": \"test2\",\"set_values\": [{\"type\": \"Body\",\"key\": \"$.A\",\"value\": \"ABC\"}]}]}]},{\"process_name\": \"test3\",\"application\": {\"type\": \"http\",\"url\": \"http://base.bbb/ccc\",\"replace_params\": [{\"base_name\": \"base\",\"source_data_type\": \"Properties\",\"source_data_key\": \"$.A\"}]},\"post_processes\": [{\"condition_path\": \"$.XXX\",\"condition_operator\": \"EQ(\\\"condition_operator_value\\\")\",\"tasks\": [{\"output_name\": \"output1\",\"next_process\": \"test2\",\"set_values\": [{\"type\": \"Body\",\"key\": \"$.A\",\"value\": \"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("application is null.", ex.Message);
        }

        [Fact(DisplayName = "異常系:processesの要素数が3で3つめの要素のtypeがNotSelected→例外")]
        public void ProcessesLengthIs3AndThirdProcessIsIllegal_ExceptioThrown()
        {
            string desiredPropertyStr = "{\"input_name\": \"input_name_value\",\"processes\": [{\"process_name\": \"test1\",\"application\": {\"type\": \"http\",\"url\": \"http://base.bbb/ccc\",\"replace_params\": [{\"base_name\": \"base\",\"source_data_type\": \"Properties\",\"source_data_key\": \"$.A\"}]},\"post_processes\": [{\"condition_path\": \"$.XXX\",\"condition_operator\": \"EQ(\\\"condition_operator_value\\\")\",\"tasks\": [{\"output_name\": \"output1\",\"next_process\": \"test2\",\"set_values\": [{\"type\": \"Body\",\"key\": \"$.A\",\"value\": \"ABC\"}]}]}]},{\"process_name\": \"test2\",\"application\": {\"type\": \"http\",\"url\": \"http://base.bbb/ccc\",\"replace_params\": [{\"base_name\": \"base\",\"source_data_type\": \"Properties\",\"source_data_key\": \"$.A\"}]},\"post_processes\": [{\"condition_path\": \"$.XXX\",\"condition_operator\": \"EQ(\\\"condition_operator_value\\\")\",\"tasks\": [{\"output_name\": \"output1\",\"next_process\": \"test2\",\"set_values\": [{\"type\": \"Body\",\"key\": \"$.A\",\"value\": \"ABC\"}]}]}]},{\"process_name\": \"test3\",\"application\": {\"url\": \"http://base.bbb/ccc\",\"replace_params\": [{\"base_name\": \"base\",\"source_data_type\": \"Properties\",\"source_data_key\": \"$.A\"}]},\"post_processes\": [{\"condition_path\": \"$.XXX\",\"condition_operator\": \"EQ(\\\"condition_operator_value\\\")\",\"tasks\": [{\"output_name\": \"output1\",\"next_process\": \"test2\",\"set_values\": [{\"type\": \"Body\",\"key\": \"$.A\",\"value\": \"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("application type is not set.", ex.Message);
        }

        [Fact(DisplayName = "正常系:replace_paramsの要素数が3で全要素が正しい→正常終了")]
        public void ReplaceParamsLengthIs3_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"},{\"base_name\":\"bbb\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.B\"},{\"base_name\":\"ccc\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.C\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }


        [Fact(DisplayName = "異常系:replace_paramsの要素数が3で最初の要素のbase_nameがnull→例外")]
        public void ReplaceParamsLengthIs3FirstReplaceParamIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"},{\"base_name\":\"bbb\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.B\"},{\"base_name\":\"ccc\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.C\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("base_name is null or empty.", ex.Message);
        }

        [Fact(DisplayName = "異常系:replace_paramsの要素数が3で2つめの要素のsource_data_typeがNotSelected→例外")]
        public void ReplaceParamsLengthIs3SecondReplaceParamIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"},{\"base_name\":\"bbb\",\"source_data_key\":\"$.B\"},{\"base_name\":\"ccc\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.C\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("source_data_type is not set.", ex.Message);
        }

        [Fact(DisplayName = "異常系:replace_paramsの要素数が3で3つめの要素のsource_data_keyがnull→例外")]
        public void ReplaceParamsLengthIs3ThirdReplaceParamIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"},{\"base_name\":\"bbb\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.B\"},{\"base_name\":\"ccc\",\"source_data_type\":\"Properties\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("source_data_key is null or empty.", ex.Message);
        }

        [Fact(DisplayName = "正常系:post_processesの要素数が3で全要素が正しい→正常終了")]
        public void PostProcessesLengthIs3_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"},{\"base_name\":\"bbb\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.B\"},{\"base_name\":\"ccc\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.C\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]},{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]},{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:post_processesの要素数が3で最初の要素のcondition_pathのみがnull→例外")]
        public void PostProcessesLengthIs3FirstConditionPathIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"},{\"base_name\":\"bbb\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.B\"},{\"base_name\":\"ccc\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.C\"}]},\"post_processes\":[{\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]},{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]},{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("condition_path is null or empty, but condition_operator is not null or empty.", ex.Message);
        }

        [Fact(DisplayName = "異常系:post_processesの要素数が3で2つめの要素のcondition_operatorのみがnull→例外")]
        public void PostProcessesLengthIs3SecondConditionPathIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"},{\"base_name\":\"bbb\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.B\"},{\"base_name\":\"ccc\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.C\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]},{\"condition_path\":\"$.XXX\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]},{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("condition_path is not null or empty, but condition_operator is null or empty.", ex.Message);
        }

        [Fact(DisplayName = "異常系:post_processesの要素数が3で3つめの要素のtasksがnull→例外")]
        public void PostProcessesLengthIs3ThidConditionPathIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"},{\"base_name\":\"bbb\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.B\"},{\"base_name\":\"ccc\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.C\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]},{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]},{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\"}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("tasks is empty.", ex.Message);
        }

        [Fact(DisplayName = "正常系:tasksの要素数が3で全要素が正しい→正常終了")]
        public void TasksLengthIs3_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]},{\"output_name\":\"output2\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]},{\"output_name\":\"output3\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:tasksの要素数が3で最初の要素のoutput_nameがnull→例外")]
        public void TasksLengthIs3FirstOutputNameIsNull_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]},{\"output_name\":\"output2\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]},{\"output_name\":\"output3\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("output_name is null or empty.", ex.Message);

        }

        [Fact(DisplayName = "異常系:tasksの要素数が3で2つめの要素のoutput_nameが空文字→例外")]
        public void TasksLengthIs3SecondOutputNameIsNull_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]},{\"output_name\":\"\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]},{\"output_name\":\"output3\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("output_name is null or empty.", ex.Message);

        }

        [Fact(DisplayName = "異常系:tasksの要素数が3で3つめの要素のset_valuesが\"A\"→例外")]
        public void TasksLengthIs3ThirdOutputNameIsNull_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]},{\"output_name\":\"output2\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]},{\"output_name\":\"\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("output_name is null or empty.", ex.Message);
        }

        [Fact(DisplayName = "正常系:set_valuesの要素数が3で全要素が正しい→正常終了")]
        public void SetValuesLengthIs3_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"},{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"},{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:set_valuesの要素数が3で最初の要素のtypeがNotSelected→例外")]
        public void SetValuesLengthIs3FirstTypeIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"key\":\"$.A\",\"value\":\"ABC\"},{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"},{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("set_values type is not set.", ex.Message);
        }

        [Fact(DisplayName = "異常系:set_valuesの要素数が3で2つめの要素のkeyがnull→例外")]
        public void SetValuesLengthIs3SecondTypeIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"value\":\"ABC\"},{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"},{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("set_values key is null or empty.", ex.Message);
        }

        [Fact(DisplayName = "異常系:set_valuesの要素数が3で3つめの要素のvalueがnull→例外")]
        public void SetValuesLengthIs3ThirdTypeIsIllegal_ExceptionThrown()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"},{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"},{\"type\":\"Body\",\"key\":\"$.A\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            var ex = Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
            Assert.Contains("set_values value is null or empty.", ex.Message);
        }

        [Fact(DisplayName = "正常系:base_nameがurlに含まれている→正常終了")]
        public void UrlContainBaseName_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"base\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            result.Check();
        }

        [Fact(DisplayName = "異常系:base_nameがurlに含まれていない→例外")]
        public void UrlNotContainBaseName_Succeeded()
        {
            string desiredPropertyStr = "{\"input_name\":\"input_name_value\",\"processes\":[{\"process_name\":\"test\",\"application\":{\"type\":\"http\",\"url\":\"http://base.bbb/ccc\",\"replace_params\":[{\"base_name\":\"aaa\",\"source_data_type\":\"Properties\",\"source_data_key\":\"$.A\"}]},\"post_processes\":[{\"condition_path\":\"$.XXX\",\"condition_operator\":\"EQ(\\\"condition_operator_value\\\")\",\"tasks\":[{\"output_name\":\"output1\",\"next_process\":\"test2\",\"set_values\":[{\"type\":\"Body\",\"key\":\"$.A\",\"value\":\"ABC\"}]}]}]}]}";
            MyDesiredProperties result = MyDesiredProperties.Deserialize(desiredPropertyStr);
            Assert.Throws<Exception>(() =>
            {
                result.Check();
            });
        }
    }
}