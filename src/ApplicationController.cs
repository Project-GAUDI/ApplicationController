using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TICO.GAUDI.Commons;

namespace ApplicationController
{
    public class ApplicationController
    {
        const string PROPKEY_PROCESS_NAME = "process_name";

        private static Logger MyLogger { get; } = Logger.GetLogger(typeof(ApplicationController));
        private IApplicationClientFactory MyClientFactory { get; } = null;
        private EnvironmentInfo MyEnvInfo { get; } = null;
        private IModuleClient MyClient { get; } = null;
        private Dictionary<string, MyDesiredProperties.Process> ProcessMap { get; } = new Dictionary<string, MyDesiredProperties.Process>();
        private string Body { get; } = null;
        private IDictionary<string, string> Properties { get; } = null;

        public ApplicationController(
            IApplicationClientFactory factory,
            EnvironmentInfo env,
            List<MyDesiredProperties.Process> processes,
            IModuleClient client,
            string body,
            IDictionary<string, string> properties
        )
        {
            if (null == factory)
            {
                throw new Exception("factory is null.");
            }
            MyClientFactory = factory;

            if (null == env)
            {
                throw new Exception("env is null.");
            }
            MyEnvInfo = env;

            if (null == client)
            {
                throw new Exception("client is null.");
            }
            MyClient = client;

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
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }
            
            if (string.IsNullOrEmpty(process_name))
            {
                throw new Exception($"process_name is null or empty.");
            }

            MyDesiredProperties.Process targetProcess = null;
            if (ProcessMap.ContainsKey(process_name))
            {
                targetProcess = ProcessMap[process_name];
                MyLogger.WriteLog(Logger.LogLevel.INFO, $"Target process_name : \"{process_name}\"");
            }
            else
            {
                throw new Exception($"process_name not found.({process_name})");
            }

            string executeProcessResult = await ExecuteProcess(targetProcess);
            await ExecutePostProcess(targetProcess.post_processes, executeProcessResult);

            await Task.CompletedTask;
        }

        private bool IsJsonFormat(string target)
        {
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }
            
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
            MyLogger.WriteLog(Logger.LogLevel.DEBUG, $"ExecuteProcess START. process_name : \"{process.process_name}\"");

            string retString = "";

            IApplicationClient appClient = GetApplicationClient(process.application.type);
            if (null == appClient)
            {
                throw new Exception("ApplicationClient generate failed.");
            }

            bool initStatus = appClient.Initialize(MyEnvInfo, process.application, Body, Properties);
            if (false == initStatus)
            {
                throw new Exception("IApplicationClient Initialize is Failed.");
            }

            bool connectStatus = appClient.Connect();
            if (false == connectStatus)
            {
                throw new Exception("IApplicationClient Connect is Failed.");
            }

            try
            {
                retString = await appClient.SendRequest(Body);
            }
            catch (Exception ex)
            {
                throw new Exception($"IApplicationClient SendRequest is Failed. {ex}");
            }

            try
            {
                appClient.Disconnect();
            }
            catch (Exception ex)
            {
                throw new Exception($"IApplicationClient Disconnect is Failed. {ex}");
            }

            bool chkResStatus = IsJsonFormat(retString);
            if (false == chkResStatus)
            {
                throw new Exception($"Response is not JSON format. (response={retString})");
            }

            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"return : \"{retString}\"");
            }

            MyLogger.WriteLog(Logger.LogLevel.DEBUG, $"ExecuteProcess END.");
            return retString;
        }

        private async Task ExecutePostProcess(List<MyDesiredProperties.Process.PostProcess> postProcesses, string outputJson)
        {
            MyLogger.WriteLog(Logger.LogLevel.DEBUG, $"ExecutePostProcess START. postProcesses.Count : \"{postProcesses.Count}\"");

            foreach (var process in postProcesses)
            {
                if (CheckCondition(process, outputJson))
                {
                    MyLogger.WriteLog(Logger.LogLevel.DEBUG, $"pass CheckCondition. condition_path : \"{process.condition_path}\",  condition_value : \"{process.condition_operator}\"");

                    foreach (var task in process.tasks)
                    {
                        IotMessage message = CreateSendMessage(task);
                        try
                        {
                            string body = message.GetBodyString();
                            IDictionary<string, string> properties = message.GetProperties();
                            string propertiesString = string.Join(",", properties.Select(kvp => kvp.ToString()));

                            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
                            {
                                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"output_name : \"{task.output_name}\"");
                                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Send Message. Body: [{body}]");
                                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Send Message. Properties: [{propertiesString}]");
                            }

                            await MyClient.SendEventAsync(task.output_name, message);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Message sent failed. Exception : ({ex})");
                        }
                        MyLogger.WriteLog(Logger.LogLevel.DEBUG, "Message sent succeeded.");
                    }
                }
            }

            MyLogger.WriteLog(Logger.LogLevel.DEBUG, $"ExecutePostProcess END.");
        }

        private bool CheckCondition(MyDesiredProperties.Process.PostProcess postProcess, string outputJson)
        {
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }
            
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
                    MyLogger.WriteLog(Logger.LogLevel.WARN, "Multiple tokens matched in CheckCondition method.");
                    targetString = null;
                }
                else if (1 == cnt)
                {
                    if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
                    {
                        MyLogger.WriteLog(Logger.LogLevel.TRACE, "A token matched in CheckCondition method.");
                    }
                    targetString = tokens.First().ToString();
                }
                else if (0 == cnt)
                {
                    if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
                    {
                        MyLogger.WriteLog(Logger.LogLevel.TRACE, "No token matched in CheckCondition method.");
                    }
                    targetString = null;
                }

                if (null != postProcess.condition_operator && null != targetString)
                {
                    retStatus = CheckOperator(postProcess.condition_operator, targetString);
                }
            }

            return retStatus;
        }

        private bool CheckOperator(string condition_operator, string value)
        {
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }
            
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
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }

            JToken retObject = null;

            var targetTokens = root.SelectTokens(string.Join(".", pathArray));
            var targetTokensCount = targetTokens.Count();

            if (2 <= targetTokensCount)
            {
                throw new Exception("Matched multiple tokens.");
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

            return retObject;
        }

        private void ModifyBody(JToken ioRoot, string key, string value)
        {
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }

            var patharray = key.Split(".");
            JToken jt;
            try
            {
                jt = GetJsonPathObject(ioRoot, patharray);
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                throw new Exception($"Illegal JSONPath. (key={key}, body={JsonConvert.SerializeObject(ioRoot)}, execption={ex})");
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception in GetJsonPathObject. (key={key}, body={JsonConvert.SerializeObject(ioRoot)}, execption={ex})");
            }

            if (null == jt)
            {
                throw new Exception($"Error in GetJsonPathObject. (key={key}, body={JsonConvert.SerializeObject(ioRoot)})");
            }
            var jv = jt as JValue;
            if (null == jv)
            {
                throw new Exception($"Bad data type. (key={key}, body={JsonConvert.SerializeObject(ioRoot)})");
            }
            jv.Value = value;
        }

        private void ModifyProperties(IDictionary<string, string> ioProperties, string key, string value)
        {
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }

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
        }

        private IotMessage CreateSendMessage(MyDesiredProperties.Process.PostProcess.Task task)
        {
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }

            var properties = new Dictionary<string, string>(Properties);

            JToken jroot = JsonConvert.DeserializeObject(Body) as JToken;

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

            return retMessage;
        }


        private IApplicationClient GetApplicationClient(MyDesiredProperties.Process.AppSetting.Protocol protocol)
        {
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }
            
            IApplicationClient appClient = MyClientFactory.CreateInstance(protocol);
            return appClient;
        }
    }
}
