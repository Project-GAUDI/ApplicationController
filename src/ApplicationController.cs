using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TICO.GAUDI.Commons;

namespace IotedgeV2ApplicationController
{
    public class ApplicationController
    {
        const string PROPKEY_PROCESS_NAME = "process_name";

        private static ILogger MyLogger { get; } = LoggerFactory.GetLogger(typeof(ApplicationController));
        private IApplicationClientFactory MyClientFactory { get; } = null;
        private IMessageSender MySender { get; } = null;
        private EnvironmentInfo MyEnvInfo { get; } = null;
        private Dictionary<string, MyDesiredProperties.Process> ProcessMap { get; } = new Dictionary<string, MyDesiredProperties.Process>();
        private string Body { get; } = null;
        private IDictionary<string, string> Properties { get; } = null;

        public ApplicationController(
            IApplicationClientFactory factory,
            IMessageSender sender,
            EnvironmentInfo env,
            List<MyDesiredProperties.Process> processes,
            string body,
            IDictionary<string, string> properties
        )
        {
            if (null == factory)
            {
                throw new Exception("factory is null.");
            }
            MyClientFactory = factory;
            if (null == sender)
            {
                throw new Exception("sender is null.");
            }
            MySender = sender;

            if (null == env)
            {
                throw new Exception("env is null.");
            }
            MyEnvInfo = env;

            if (null == body)
            {
                throw new Exception("body is null.");
            }
            if (false == IsJsonFormat(body))
            {
                throw new Exception("body is not JSON format.");
            }
            Body = body;

            if (null == properties)
            {
                throw new Exception("properties is null.");
            }
            Properties = properties;

            processes.ForEach((p) => ProcessMap.Add(p.process_name, p));
        }

        public async Task Execute(string process_name)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: Execute");

            if (string.IsNullOrEmpty(process_name))
            {
                var errmsg = $"process_name is null or empty.";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: Execute caused by {errmsg}");
                throw new Exception(errmsg);
            }

            MyDesiredProperties.Process targetProcess = null;
            if (ProcessMap.ContainsKey(process_name))
            {
                targetProcess = ProcessMap[process_name];
                MyLogger.WriteLog(ILogger.LogLevel.INFO, $"Target process_name : \"{process_name}\"");
            }
            else
            {
                var errmsg = $"process_name not found.({process_name}).";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: Execute caused by {errmsg}");
                throw new Exception(errmsg);
            }

            string executeProcessResult = await ExecuteProcess(targetProcess);
            await ExecutePostProcess(targetProcess.post_processes, executeProcessResult);

            await Task.CompletedTask;

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: Execute");
        }

        private bool IsJsonFormat(string target)
        {
            bool retResult = false;

            try
            {
                var jobj = JsonConvert.DeserializeObject(target);
                if (null != jobj)
                {
                    retResult = true;
                }
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                retResult = false;
            }

            return retResult;
        }

        private async Task<string> ExecuteProcess(MyDesiredProperties.Process process)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: ExecuteProcess. process_name : \"{process.process_name}\"");

            string retString = "";

            IApplicationClient appClient = GetApplicationClient(process.application.type);
            if (null == appClient)
            {
                var errmsg = $"ApplicationClient generate failed.";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: ExecuteProcess caused by {errmsg}");
                throw new Exception(errmsg);
            }

            bool initStatus = appClient.Initialize(MyEnvInfo, process.application, Body, Properties);
            if (false == initStatus)
            {
                var errmsg = $"IApplicationClient Initialize is Failed.";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: ExecuteProcess caused by {errmsg}");
                throw new Exception(errmsg);
            }

            bool connectStatus = appClient.Connect();
            if (false == connectStatus)
            {
                var errmsg = $"IApplicationClient Connect is Failed.";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: ExecuteProcess caused by {errmsg}");
                throw new Exception(errmsg);
            }

            try
            {
                retString = await appClient.SendRequest(Body);
            }
            catch (Exception ex)
            {
                var errmsg = $"IApplicationClient SendRequest is Failed.";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: ExecuteProcess caused by {errmsg}");
                throw new Exception($"{errmsg} {ex}");
            }

            try
            {
                appClient.Disconnect();
            }
            catch (Exception ex)
            {
                var errmsg = $"IApplicationClient Disconnect is Failed.";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: ExecuteProcess caused by {errmsg}");
                throw new Exception($"{errmsg} {ex}");
            }

            bool chkResStatus = IsJsonFormat(retString);
            if (false == chkResStatus)
            {
                var errmsg = $"Response is not JSON format. (response={retString}).";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: ExecuteProcess caused by {errmsg}");
                throw new Exception(errmsg);
            }

            if (MyLogger.IsLogLevelToOutput(ILogger.LogLevel.TRACE))
            {
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"return : \"{retString}\"");
            }

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: ExecuteProcess");
            return retString;
        }

        private async Task ExecutePostProcess(List<MyDesiredProperties.Process.PostProcess> postProcesses, string outputJson)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: ExecutePostProcess. postProcesses.Count : \"{postProcesses.Count}\"");

            foreach (var process in postProcesses)
            {
                if (CheckCondition(process, outputJson))
                {
                    MyLogger.WriteLog(ILogger.LogLevel.DEBUG, $"pass CheckCondition. condition_path : \"{process.condition_path}\",  condition_value : \"{process.condition_operator}\"");

                    foreach (var task in process.tasks)
                    {
                        IotMessage message = CreateSendMessage(task, outputJson);
                        try
                        {
                            string body = message.GetBodyString();
                            IDictionary<string, string> properties = message.GetProperties();
                            string propertiesString = string.Join(",", properties.Select(kvp => kvp.ToString()));

                            if (MyLogger.IsLogLevelToOutput(ILogger.LogLevel.TRACE))
                            {
                                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"output_name : \"{task.output_name}\"");
                                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Send Message. Body: [{body}]");
                                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Send Message. Properties: [{propertiesString}]");
                            }

                            await MySender.SendMessageAsync(task.output_name, message);
                        }
                        catch (Exception ex)
                        {
                            var errmsg = $"Message sent failed.";
                            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: ExecutePostProcess caused by {errmsg}");
                            throw new Exception($"{errmsg} Exception : ({ex})");
                        }
                        MyLogger.WriteLog(ILogger.LogLevel.INFO, "1 message sent");
                    }
                }
            }

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: ExecutePostProcess");
        }

        private bool CheckCondition(MyDesiredProperties.Process.PostProcess postProcess, string outputJson)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: CheckCondition");
            bool retStatus = false;

            if (string.IsNullOrEmpty(postProcess.condition_path))
            {
                retStatus = true;
            }
            else
            {
                JToken jroot = JsonConvert.DeserializeObject(outputJson) as JToken;
                var tokens = jroot.SelectTokens(postProcess.condition_path);
                int cnt = tokens.Count();

                string targetString = null;

                if (2 <= cnt)
                {
                    MyLogger.WriteLog(ILogger.LogLevel.WARN, "Multiple tokens matched in CheckCondition method.");
                    targetString = null;
                }
                else if (1 == cnt)
                {
                    if (MyLogger.IsLogLevelToOutput(ILogger.LogLevel.TRACE))
                    {
                        MyLogger.WriteLog(ILogger.LogLevel.TRACE, "A token matched in CheckCondition method.");
                    }
                    targetString = tokens.First().ToString();
                }
                else if (0 == cnt)
                {
                    if (MyLogger.IsLogLevelToOutput(ILogger.LogLevel.TRACE))
                    {
                        MyLogger.WriteLog(ILogger.LogLevel.TRACE, "No token matched in CheckCondition method.");
                    }
                    targetString = null;
                }

                if (null != postProcess.condition_operator && null != targetString)
                {
                    retStatus = CheckOperator(postProcess.condition_operator, targetString);
                }
            }
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: CheckCondition");
            return retStatus;
        }

        private bool CheckOperator(string condition_operator, string value)
        {
            bool retStatus = false;

            if (condition_operator.StartsWith("EQ(") && condition_operator.EndsWith(")"))
            {
                string condition_value = null;

                condition_value = condition_operator["EQ(".Length..^(")".Length)];

                if (condition_value.StartsWith("\"") && condition_value.EndsWith("\""))
                {
                    condition_value = condition_value[1..^1];
                }

                if (condition_value == value)
                {
                    retStatus = true;
                }
            }
            return retStatus;
        }

        static JToken GetJsonPathObject(JToken root, string[] pathArray, string leafObjectType = "JValue")
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: GetJsonPathObject");

            JToken retObject = null;

            var targetTokens = root.SelectTokens(string.Join(".", pathArray));
            var targetTokensCount = targetTokens.Count();

            if (2 <= targetTokensCount)
            {
                var errmsg = $"Matched multiple tokens.";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: GetJsonPathObject caused by {errmsg}");
                throw new Exception(errmsg);
            }
            else if (1 == targetTokensCount)
            {
                retObject = targetTokens.First();
            }
            else if (0 == targetTokensCount)
            {
                var parentToken = GetJsonPathObject(root, pathArray[0..^1], "JObject");
                switch (leafObjectType)
                {
                    case "JValue":
                        retObject = new JValue("");
                        break;
                    case "JObject":
                        retObject = new JObject();
                        break;
                }
                parentToken[pathArray[^1]] = retObject;
            }

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: GetJsonPathObject");

            return retObject;
        }

        private void ModifyBody(JToken ioRoot, string key, string value)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: ModifyBody");

            var patharray = key.Split(".");
            JToken jt;
            try
            {
                jt = GetJsonPathObject(ioRoot, patharray);
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                var errmsg = $"Illegal JSONPath. (key={key}, body={JsonConvert.SerializeObject(ioRoot)}.";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: ModifyBody caused by {errmsg}");
                throw new Exception($"{errmsg} execption={ex})");
            }
            catch (Exception ex)
            {
                var errmsg = $"Exception in GetJsonPathObject. (key={key}, body={JsonConvert.SerializeObject(ioRoot)}.";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: ModifyBody caused by {errmsg}");
                throw new Exception($"{errmsg} execption={ex})");
            }

            if (null == jt)
            {
                var errmsg = $"Error in GetJsonPathObject. (key={key}, body={JsonConvert.SerializeObject(ioRoot)}).";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: ModifyBody caused by {errmsg}");
                throw new Exception(errmsg);
            }
            var jv = jt as JValue;
            if (null == jv)
            {
                var errmsg = $"Bad data type. (key={key}, body={JsonConvert.SerializeObject(ioRoot)}).";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: ModifyBody caused by {errmsg}");
                throw new Exception(errmsg);
            }
            jv.Value = value;

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: ModifyBody");
        }

        private void ModifyProperties(IDictionary<string, string> ioProperties, string key, string value)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: ModifyProperties");

            if (null == value)
            {
                if (ioProperties.ContainsKey(key))
                {
                    ioProperties.Remove(key);
                }
            }
            else
            {
                ioProperties[key] = value;
            }

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: ModifyProperties");
        }

        private IotMessage CreateSendMessage(MyDesiredProperties.Process.PostProcess.Task task, string apiResponse)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: CreateSendMessage");

            var properties = new Dictionary<string, string>(Properties);

            JToken jroot = JsonConvert.DeserializeObject(apiResponse) as JToken;

            foreach (var setvalue in task.set_values)
            {
                switch (setvalue.type)
                {
                    case MyDesiredProperties.MessageData.Body:
                        ModifyBody(jroot, setvalue.key, setvalue.value);
                        break;

                    case MyDesiredProperties.MessageData.Properties:
                        ModifyProperties(properties, setvalue.key, setvalue.value);
                        break;

                    case MyDesiredProperties.MessageData.NotSelected:
                    default:
                        break;
                }

            }

            ModifyProperties(properties, PROPKEY_PROCESS_NAME, task.next_process);

            var body = JsonConvert.SerializeObject(jroot);


            var retMessage = new IotMessage(body);
            retMessage.SetProperties(properties);

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: CreateSendMessage");

            return retMessage;
        }


        private IApplicationClient GetApplicationClient(MyDesiredProperties.Process.AppSetting.Protocol protocol)
        {
            IApplicationClient appClient = MyClientFactory.CreateInstance(protocol);

            return appClient;
        }
    }
}
