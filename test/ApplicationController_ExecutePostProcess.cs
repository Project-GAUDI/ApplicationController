using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using TICO.GAUDI.Commons;


namespace ApplicationController.Test
{

    public class ApplicationController_ExecutePostProcess : ApplicationController_TesterBase
    {

        public ApplicationController_ExecutePostProcess(ITestOutputHelper output) : base(output)
        {

        }

        private void TestBodyPath(string body, string path, string value)
        {
            JToken jroot = JsonConvert.DeserializeObject(body) as JToken;
            JToken token = jroot.SelectToken(path);

            Assert.Equal(value, token.ToString());
        }

        [Fact(DisplayName = "正常系:SendEventAsyncが成功する→正常終了")]
        public async void SendEventAsyncIsSucceeded_Succeeded()
        {
            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await result.Execute(process_name);

            Assert.Equal(body, client._message.GetBodyString());
        }

        [Fact(DisplayName = "異常系:SendEventAsyncがエラー→例外")]
        public async void SendEventAsyncIsFailed_ExceptionThrown()
        {
            myDesiredProperties.processes[0].post_processes[0].tasks[0].output_name = "";
            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";
            var ex = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });
            Assert.Contains("outputName is null or Empty.", ex.Message);
        }

        [Fact(DisplayName = "正常系:condition_pathの場所にデータが存在しない(出力メッセージ)→taskが実行されない")]
        public async void NotExistsData_DoNotTask()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.NotFound";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await result.Execute(process_name);

            Assert.Null(client._message);
        }

        [Fact(DisplayName = "正常系:condition_pathとcondition_operatorの両方が未指定→判定なしでtaskが実行される")]
        public async void ConditionPathIsNull_DoTask()
        {
            //            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            //            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await result.Execute(process_name);

            Assert.Equal(body, client._message.GetBodyString());
        }

        [Fact(DisplayName = "正常系:condition_pathは指定済み、condition_operatorが未指定→taskが実行されない")]
        public async void ConditionPathIsNotNullAndValueIsNull_DoNotTask()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            //            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await result.Execute(process_name);

            Assert.Null(client._message);
        }

        [Fact(DisplayName = "正常系:condition_pathは未指定、condition_operatorが指定済み→判定なしでtaskが実行される")]
        public async void ConditionPathIsNullAndValueIsNotNull_DoTask()
        {
            //            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await result.Execute(process_name);

            Assert.Equal(body, client._message.GetBodyString());
        }

        [Fact(DisplayName = "正常系:condition_pathの場所にデータが存在し、CheckOperatorの戻り値がfalse→taskが実行されない")]
        public async void ConditionPathIsValueCheckOperatorFalseReturned_DoNotTask()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            factory.SetReturnStatus("201");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await result.Execute(process_name);

            Assert.Null(client._message);
        }

        [Fact(DisplayName = "正常系:condition_pathの場所にデータが存在し、CheckOperatorの戻り値がtrue→taskが実行される")]
        public async void ConditionPathIsValueCheckOperatorTrueReturned_DoNotTask()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await result.Execute(process_name);

            Assert.Equal(body, client._message.GetBodyString());
        }

        [Fact(DisplayName = "正常系:condition_pathの場所にデータが存在し、condition_operatorの引数が0文字→taskが実行される")]
        public async void ConditionPathIsValueAndConditionOperatorParamIsNoParam_DoTask()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ()";

            factory.SetReturnStatus("");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await result.Execute(process_name);

            Assert.Equal(body, client._message.GetBodyString());
        }

        [Fact(DisplayName = "正常系:condition_pathの場所にデータが存在し、condition_operatorの引数が空文字→taskが実行される")]
        public async void ConditionPathIsValueAndConditionOperatorParamIsEmpty_DoTask()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"\")";

            factory.SetReturnStatus("");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await result.Execute(process_name);

            Assert.Equal(body, client._message.GetBodyString());
        }


        [Fact(DisplayName = "正常系:set_valuesのtypeがBodyで、keyがnullではなく、その場所にデータが存在する→メッセージ上書き")]
        public async void SetValuesTypeIsBodyAndKeyDataExists_KeyDataModified()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var setValue = myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0];
            setValue.type = MyDesiredProperties.MessageData.Body;
            setValue.key = "$.A";
            setValue.value = "AAA";

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await result.Execute(process_name);

            TestBodyPath(client._message.GetBodyString(), "$.A", "AAA");
        }

        [Fact(DisplayName = "正常系:set_valuesのtypeがBodyで、keyがnullではなく、その場所にデータが存在しない→メッセージ追加")]
        public async void SetValuesTypeIsBodyAndKeyDataNotExists_KeyDataAdded()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var setValue = myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0];
            setValue.type = MyDesiredProperties.MessageData.Body;
            setValue.key = "$.B";
            setValue.value = "AAA";

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await result.Execute(process_name);

            TestBodyPath(client._message.GetBodyString(), "$.B", "AAA");
        }

        [Fact(DisplayName = "正常系:set_valuesのtypeがPropertiesで、keyがnullではなく、その場所にデータが存在する→メッセージ上書き")]
        public async void SetValuesTypeIsPropertyAndKeyDataExists_KeyDataModified()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var setValue = myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0];
            setValue.type = MyDesiredProperties.MessageData.Properties;
            properties.Add("prop1", "value1");
            setValue.key = "prop1";
            setValue.value = "valueA";

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);
            string process_name = "test";

            await result.Execute(process_name);

            Assert.Equal("valueA", client._message.GetProperty("prop1"));
        }

        [Fact(DisplayName = "正常系:set_valuesのtypeがPropertiesで、keyがnullではなく、その場所にデータが存在しない→メッセージ追加")]
        public async void SetValuesTypeIsPropertyAndKeyDataNotExists_KeyDataAdded()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var setValue = myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0];
            setValue.type = MyDesiredProperties.MessageData.Properties;
            properties.Add("prop1", "value1");
            setValue.key = "prop2";
            setValue.value = "valueA";

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);
            string process_name = "test";

            await result.Execute(process_name);

            Assert.Equal("valueA", client._message.GetProperty("prop2"));

            Assert.Equal("value1", client._message.GetProperty("prop1"));
        }

        [Fact(DisplayName = "正常系:set_valuesの要素数が3つで、condition達成→全てのtaskが実行され、出力にvalueが含まれる")]
        public async void SetValuesCountIs3AndCondtionMatch_DoAllTasksAndMessageModified()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var setValues = myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values;
            setValues.Clear();

            MyDesiredProperties.Process.PostProcess.Task.SetValue setValue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
            setValue.type = MyDesiredProperties.MessageData.Body;
            setValue.key = "$.A";
            setValue.value = "AAA";
            setValues.Add(setValue);

            setValue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
            setValue.type = MyDesiredProperties.MessageData.Body;
            setValue.key = "$.B";
            setValue.value = "BBB";
            setValues.Add(setValue);

            setValue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
            setValue.type = MyDesiredProperties.MessageData.Properties;
            setValue.key = "prop2";
            setValue.value = "valueA";
            setValues.Add(setValue);

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);
            string process_name = "test";
            await result.Execute(process_name);

            TestBodyPath(client._message.GetBodyString(), "$.A", "AAA");
            TestBodyPath(client._message.GetBodyString(), "$.B", "BBB");
            Assert.Equal("valueA", client._message.GetProperty("prop2"));
        }

        [Fact(DisplayName = "正常系:set_valuesの要素数が3つで、condition未達成→全てのtaskが実行されず、出力にvalueが含まれない")]
        public async void SetValuesCountIs3AndCondtionNoMatch_DoNotAllTasksAndMessageNotSent()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var setValues = myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values;
            setValues.Clear();

            MyDesiredProperties.Process.PostProcess.Task.SetValue setValue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
            setValue.type = MyDesiredProperties.MessageData.Body;
            setValue.key = "$.A";
            setValue.value = "AAA";
            setValues.Add(setValue);

            setValue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
            setValue.type = MyDesiredProperties.MessageData.Body;
            setValue.key = "$.B";
            setValue.value = "BBB";
            setValues.Add(setValue);

            setValue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
            setValue.type = MyDesiredProperties.MessageData.Properties;
            setValue.key = "prop2";
            setValue.value = "valueA";
            setValues.Add(setValue);

            factory.SetReturnStatus("10");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);
            string process_name = "test";
            await result.Execute(process_name);

            Assert.Null(client._message);
        }

        [Fact(DisplayName = "正常系:tasksの要素数が3つで、condition達成→全てのtaskが実行される")]
        public async void TaskCountIs3AndCondtionMatch_3MessageSent()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var tasks = myDesiredProperties.processes[0].post_processes[0].tasks;
            tasks.Clear();

            MyDesiredProperties.Process.PostProcess.Task task;
            MyDesiredProperties.Process.PostProcess.Task.SetValue setvalue;

            task = new MyDesiredProperties.Process.PostProcess.Task();
            task.output_name = "output1";
            task.set_values = new List<MyDesiredProperties.Process.PostProcess.Task.SetValue>();
            setvalue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
            setvalue.type = MyDesiredProperties.MessageData.Body;
            setvalue.key = "$.A";
            setvalue.value = "AAA";
            task.set_values.Add(setvalue);
            tasks.Add(task);

            task = new MyDesiredProperties.Process.PostProcess.Task();
            task.output_name = "output2";
            task.set_values = new List<MyDesiredProperties.Process.PostProcess.Task.SetValue>();
            setvalue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
            setvalue.type = MyDesiredProperties.MessageData.Body;
            setvalue.key = "$.B";
            setvalue.value = "BBB";
            task.set_values.Add(setvalue);
            tasks.Add(task);

            task = new MyDesiredProperties.Process.PostProcess.Task();
            task.output_name = "output3";
            task.set_values = new List<MyDesiredProperties.Process.PostProcess.Task.SetValue>();
            setvalue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
            setvalue.type = MyDesiredProperties.MessageData.Properties;
            setvalue.key = "prop2";
            setvalue.value = "valueA";
            task.set_values.Add(setvalue);
            tasks.Add(task);

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);
            string process_name = "test";
            await result.Execute(process_name);

            TestBodyPath(client._allMessage["output1"].GetBodyString(), "$.A", "AAA");
            TestBodyPath(client._allMessage["output2"].GetBodyString(), "$.B", "BBB");
            Assert.Equal("valueA", client._allMessage["output3"].GetProperty("prop2"));
        }

        [Fact(DisplayName = "正常系:tasksの要素数が3つで、condition未達成→全てのtaskが実行されない")]
        public async void TaskCountIs3AndCondtionNoMatch_NoMessageSent()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var tasks = myDesiredProperties.processes[0].post_processes[0].tasks;
            tasks.Clear();

            MyDesiredProperties.Process.PostProcess.Task task;
            MyDesiredProperties.Process.PostProcess.Task.SetValue setvalue;

            task = new MyDesiredProperties.Process.PostProcess.Task();
            task.output_name = "output1";
            task.set_values = new List<MyDesiredProperties.Process.PostProcess.Task.SetValue>();
            setvalue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
            setvalue.type = MyDesiredProperties.MessageData.Body;
            setvalue.key = "$.A";
            setvalue.value = "AAA";
            task.set_values.Add(setvalue);
            tasks.Add(task);

            task = new MyDesiredProperties.Process.PostProcess.Task();
            task.output_name = "output2";
            task.set_values = new List<MyDesiredProperties.Process.PostProcess.Task.SetValue>();
            setvalue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
            setvalue.type = MyDesiredProperties.MessageData.Body;
            setvalue.key = "$.B";
            setvalue.value = "BBB";
            task.set_values.Add(setvalue);
            tasks.Add(task);

            task = new MyDesiredProperties.Process.PostProcess.Task();
            task.output_name = "output3";
            task.set_values = new List<MyDesiredProperties.Process.PostProcess.Task.SetValue>();
            setvalue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
            setvalue.type = MyDesiredProperties.MessageData.Properties;
            setvalue.key = "prop2";
            setvalue.value = "valueA";
            task.set_values.Add(setvalue);
            tasks.Add(task);

            factory.SetReturnStatus("10");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);
            string process_name = "test";
            await result.Execute(process_name);

            Assert.Null(client._message);
        }

        [Fact(DisplayName = "正常系:post_processesの要素数が3つで、全てcondition達成→全てのpost_processのtaskが実行される")]
        public async void PostProcessesCountIs3AndCondtionMatch_3MessageSent()
        {
            myDesiredProperties = MyDesiredPropertiesCreater.CreateThreePostProccesses();

            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            myDesiredProperties.processes[0].post_processes[1].condition_path = "$.url";
            myDesiredProperties.processes[0].post_processes[1].condition_operator = "EQ(\"http://aaa.bbb/ccc\")";

            myDesiredProperties.processes[0].post_processes[2].condition_path = "$.timeout";
            myDesiredProperties.processes[0].post_processes[2].condition_operator = "EQ(10)";


            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Body;
            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].key = "$.A";
            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].value = "AAA";

            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Body;
            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].key = "$.B";
            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].value = "BBB";

            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Properties;
            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].key = "prop2";
            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].value = "valueA";

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);
            string process_name = "test";
            await result.Execute(process_name);

            TestBodyPath(client._allMessage["output1"].GetBodyString(), "$.A", "AAA");
            TestBodyPath(client._allMessage["output2"].GetBodyString(), "$.B", "BBB");
            Assert.Equal("valueA", client._allMessage["output3"].GetProperty("prop2"));
        }

        [Fact(DisplayName = "正常系:post_processesの要素数が3つで、1つめのみcondition達成→post_process[0]のtaskのみ全て実行される")]
        public async void PostProcessesCountIs3AndFirstCondtionMatch_1MessageSent()
        {
            myDesiredProperties = MyDesiredPropertiesCreater.CreateThreePostProccesses();

            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            myDesiredProperties.processes[0].post_processes[1].condition_path = "$.url";
            myDesiredProperties.processes[0].post_processes[1].condition_operator = "";

            myDesiredProperties.processes[0].post_processes[2].condition_path = "$.timeout";
            myDesiredProperties.processes[0].post_processes[2].condition_operator = "";


            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Body;
            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].key = "$.A";
            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].value = "AAA";

            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Body;
            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].key = "$.B";
            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].value = "BBB";

            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Properties;
            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].key = "prop2";
            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].value = "valueA";

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);
            string process_name = "test";
            await result.Execute(process_name);

            TestBodyPath(client._allMessage["output1"].GetBodyString(), "$.A", "AAA");
            Assert.Single(client._allMessage);
        }

        [Fact(DisplayName = "正常系:post_processesの要素数が3つで、2つめのみcondition達成→post_process[1]のtaskのみ全て実行される")]
        public async void PostProcessesCountIs3AndSecondCondtionMatch_1MessageSent()
        {
            myDesiredProperties = MyDesiredPropertiesCreater.CreateThreePostProccesses();

            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            myDesiredProperties.processes[0].post_processes[1].condition_path = "$.url";
            myDesiredProperties.processes[0].post_processes[1].condition_operator = "EQ(\"http://aaa.bbb/ccc\")";

            myDesiredProperties.processes[0].post_processes[2].condition_path = "$.timeout";
            myDesiredProperties.processes[0].post_processes[2].condition_operator = "";


            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Body;
            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].key = "$.A";
            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].value = "AAA";

            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Body;
            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].key = "$.B";
            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].value = "BBB";

            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Properties;
            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].key = "prop2";
            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].value = "valueA";

            factory.SetReturnStatus("10");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);
            string process_name = "test";
            await result.Execute(process_name);

            TestBodyPath(client._allMessage["output2"].GetBodyString(), "$.B", "BBB");
            Assert.Single(client._allMessage);
        }

        [Fact(DisplayName = "正常系:post_processesの要素数が3つで、3つめのみcondition達成→post_process[2]のtaskのみ全て実行される")]
        public async void PostProcessesCountIs3AndThirdCondtionMatch_1MessageSent()
        {
            myDesiredProperties = MyDesiredPropertiesCreater.CreateThreePostProccesses();

            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            myDesiredProperties.processes[0].post_processes[1].condition_path = "$.url";
            myDesiredProperties.processes[0].post_processes[1].condition_operator = "";

            myDesiredProperties.processes[0].post_processes[2].condition_path = "$.timeout";
            myDesiredProperties.processes[0].post_processes[2].condition_operator = "EQ(10)";


            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Body;
            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].key = "$.A";
            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].value = "AAA";

            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Body;
            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].key = "$.B";
            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].value = "BBB";

            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Properties;
            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].key = "prop2";
            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].value = "valueA";

            factory.SetReturnStatus("10");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);
            string process_name = "test";
            await result.Execute(process_name);

            Assert.Equal("valueA", client._allMessage["output3"].GetProperty("prop2"));
            Assert.Single(client._allMessage);
        }

        [Fact(DisplayName = "正常系:post_processesの要素数が3つで、3つとも未達成→全てのpost_processのtaskが実行されない")]
        public async void PostProcessesCountIs3AndCondtionNoMatch_NoMessageSent()
        {
            myDesiredProperties = MyDesiredPropertiesCreater.CreateThreePostProccesses();

            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            myDesiredProperties.processes[0].post_processes[1].condition_path = "$.url";
            myDesiredProperties.processes[0].post_processes[1].condition_operator = "";

            myDesiredProperties.processes[0].post_processes[2].condition_path = "$.timeout";
            myDesiredProperties.processes[0].post_processes[2].condition_operator = "";


            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Body;
            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].key = "$.A";
            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].value = "AAA";

            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Body;
            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].key = "$.B";
            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].value = "BBB";

            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Properties;
            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].key = "prop2";
            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].value = "valueA";

            factory.SetReturnStatus("10");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);
            string process_name = "test";
            await result.Execute(process_name);

            Assert.Null(client._message);
            Assert.Empty(client._allMessage);
        }

        [Fact(DisplayName = "正常系:post_processesの要素数が3つで、conditionが1つめ未指定、2つめ未達成、3つめ達成の場合→post_process[0]、post_process[2]のtaskのみ全て実行される")]
        public async void PostProcessesCountIs3AndCondtionIsNullAndNoMatchAndMatch_2MessageSent()
        {
            myDesiredProperties = MyDesiredPropertiesCreater.CreateThreePostProccesses();

            // myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            // myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            myDesiredProperties.processes[0].post_processes[1].condition_path = "$.url";
            myDesiredProperties.processes[0].post_processes[1].condition_operator = "";

            myDesiredProperties.processes[0].post_processes[2].condition_path = "$.timeout";
            myDesiredProperties.processes[0].post_processes[2].condition_operator = "EQ(10)";


            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Body;
            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].key = "$.A";
            myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0].value = "AAA";

            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Body;
            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].key = "$.B";
            myDesiredProperties.processes[0].post_processes[1].tasks[0].set_values[0].value = "BBB";

            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].type = MyDesiredProperties.MessageData.Properties;
            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].key = "prop2";
            myDesiredProperties.processes[0].post_processes[2].tasks[0].set_values[0].value = "valueA";

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);
            string process_name = "test";
            await result.Execute(process_name);

            TestBodyPath(client._allMessage["output1"].GetBodyString(), "$.A", "AAA");
            // TestBodyPath(client._allMessage["output2"].GetBodyString(), "$.B", "BBB");
            Assert.Equal("valueA", client._allMessage["output3"].GetProperty("prop2"));
            Assert.Equal(2, client._allMessage.Count);
        }

        [Fact(DisplayName = "異常系:set_valuesのtypeがBodyで、keyが\"$\"→例外")]
        public async void SetValuesTypeIsBodyAndKeyIsDollar_ExceptionThrown()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var setValue = myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0];
            setValue.type = MyDesiredProperties.MessageData.Body;
            setValue.key = "$";
            setValue.value = "AAA";

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });
        }

        [Fact(DisplayName = "異常系:set_valuesのtypeがBodyで、keyが\"$..\"→例外")]
        public async void SetValuesTypeIsBodyAndKeyIsDollarDotDot_ExceptionThrown()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var setValue = myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0];
            setValue.type = MyDesiredProperties.MessageData.Body;
            setValue.key = "$..";
            setValue.value = "AAA";

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });
        }

        [Fact(DisplayName = "異常系:set_valuesのtypeがBodyで、keyが\"$[\"→例外")]
        public async void SetValuesTypeIsBodyAndKeyIsDollarHalfBracket_ExceptionThrown()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var setValue = myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0];
            setValue.type = MyDesiredProperties.MessageData.Body;
            setValue.key = "$[";
            setValue.value = "AAA";

            factory.SetReturnStatus("200");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });
        }

        [Fact(DisplayName = "異常系:set_valuesのtypeがBodyで、keyが\"$..key\"(項目名がkeyの全取得)→例外")]
        public async void SetValuesTypeIsBodyAndKeyIsDollarDotDotKey_ExceptionThrown()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var setValue = myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0];
            setValue.type = MyDesiredProperties.MessageData.Body;
            setValue.key = "$..key";
            setValue.value = "AAA";

            factory.SetReturnStatus("200");

            body = "{\"A\":{\"key\":\"AAA\"}, \"B\":{\"B2\":{\"key\":\"BBB\"}}}";

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            var ex = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });

            Assert.Contains("Exception in GetJsonPathObject.", ex.Message);
            Assert.Contains("Matched multiple tokens.", ex.Message);
        }

        [Fact(DisplayName = "異常系:set_valuesのtypeがBodyで、keyに\"$.配列名[*]\"(配列の全要素を取得)→例外")]
        public async void SetValuesTypeIsBodyAndKeyContainsWildCard_ExceptionThrown()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var setValue = myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0];
            setValue.type = MyDesiredProperties.MessageData.Body;
            setValue.key = "$.Array[*]";
            setValue.value = "AAA";

            factory.SetReturnStatus("200");

            body = "{\"Array\":[\"AAA\",\"BBB\"]}";

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            var ex = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await result.Execute(process_name);
            });

            Assert.Contains("Exception in GetJsonPathObject.", ex.Message);
            Assert.Contains("Matched multiple tokens.", ex.Message);
        }

        [Fact(DisplayName = "正常系:next_processが未設定(null)→送信メッセージのpropertiesに\"process_name\"が設定されていない(設定済みの場合削除される)")]
        public async void NextProcessNotExists_ProcessNameNotExistsInOutputMessage()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var setValue = myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0];
            setValue.type = MyDesiredProperties.MessageData.Body;
            setValue.key = "$.A";
            setValue.value = "AAA";

            factory.SetReturnStatus("200");

            properties["process_name"] = "test";

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            await result.Execute(properties["process_name"]);

            Assert.Null(client._message.GetProperty("process_name"));
            Assert.Equal("test", properties["process_name"]);
        }

        [Fact(DisplayName = "正常系:next_processが設定済み→送信メッセージpropertiesの\"process_name\"にnext_processの値が設定されている")]
        public async void NextProcessExists_ProcessNameIsNextProcessValueInOutputMessage()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.status";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            var setValue = myDesiredProperties.processes[0].post_processes[0].tasks[0].set_values[0];
            setValue.type = MyDesiredProperties.MessageData.Body;
            setValue.key = "$.A";
            setValue.value = "AAA";

            myDesiredProperties.processes[0].post_processes[0].tasks[0].next_process = "next_process_value";

            factory.SetReturnStatus("200");

            properties["process_name"] = "test";

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            await result.Execute(properties["process_name"]);

            Assert.Equal(myDesiredProperties.processes[0].post_processes[0].tasks[0].next_process, client._message.GetProperty("process_name"));
            Assert.Equal("test", properties["process_name"]);
        }

        [Fact(DisplayName = "正常系:condition_pathに値が複数存在する→taskが実行されない")]
        public async void ConditionPathGetSomeValue_ReturnFalse()
        {
            myDesiredProperties.processes[0].post_processes[0].condition_path = "$.*";
            myDesiredProperties.processes[0].post_processes[0].condition_operator = "EQ(\"200\")";

            factory.SetReturnStatus("100");

            ApplicationController result = new ApplicationController(factory, env, myDesiredProperties.processes, client, body, properties);

            string process_name = "test";

            await result.Execute(process_name);
            Assert.Null(client._message);

        }
    }
}