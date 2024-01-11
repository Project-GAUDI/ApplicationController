using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using TICO.GAUDI.Commons;
using YamlDotNet.Serialization;

namespace ApplicationController
{
    public class MyDesiredProperties
    {
        public enum MessageData
        {
            NotSelected,
            Body,
            Properties
        }

        public class Process
        {
            public class AppSetting
            {
                public enum Protocol
                {
                    NotSelected,
                    http,
                    // gRPC
                }



                public class ReplaceParam
                {
                    public string base_name = null;
                    public MessageData source_data_type = MessageData.NotSelected;
                    public string source_data_key = null;

                    public void Check()
                    {
                        if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
                        {
                            MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
                        }

                        if (string.IsNullOrEmpty(base_name))
                        {
                            throw new Exception("base_name is null or empty.");
                        }

                        if (MessageData.NotSelected == source_data_type)
                        {
                            throw new Exception("source_data_type is not set.");
                        }

                        if (string.IsNullOrEmpty(source_data_key))
                        {
                            throw new Exception("source_data_key is null or empty.");
                        }
                    }
                }

                public Protocol type = Protocol.NotSelected;
                public string url = null;
                public List<ReplaceParam> replace_params = new List<ReplaceParam>();

                public void Check()
                {
                    if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
                    {
                        MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
                    }

                    if (Protocol.NotSelected == type)
                    {
                        throw new Exception("application type is not set.");
                    }

                    if (string.IsNullOrEmpty(url))
                    {
                        throw new Exception("url is null or empty.");
                    }

                    foreach (var param in replace_params)
                    {
                        param.Check();
                        if (false == url.Contains(param.base_name))
                        {
                            throw new Exception($"url is not contain base_name.(url={url},base_name={param.base_name})");
                        }
                    }


                }
            }

            public class PostProcess
            {
                public class Task
                {
                    public class SetValue
                    {
                        public MessageData type = MessageData.NotSelected;
                        public string key = null;
                        public string value = null;

                        public void Check()
                        {
                            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
                            {
                                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
                            }

                            if (MessageData.NotSelected == type)
                            {
                                throw new Exception("set_values type is not set.");
                            }

                            if (string.IsNullOrEmpty(key))
                            {
                                throw new Exception("set_values key is null or empty.");
                            }

                            if (string.IsNullOrEmpty(value))
                            {
                                throw new Exception("set_values value is null or empty.");
                            }
                        }

                    }
                    public string output_name = null;
                    public string next_process = null;
                    public List<SetValue> set_values = new List<SetValue>();

                    public void Check()
                    {
                        if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
                        {
                            MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
                        }

                        if (string.IsNullOrEmpty(output_name))
                        {
                            throw new Exception("output_name is null or empty.");
                        }

                        foreach (var setValue in set_values)
                        {
                            setValue.Check();
                        }
                    }

                }

                public string condition_path = null;
                public string condition_operator = null;
                public List<Task> tasks = new List<Task>();

                public void Check()
                {
                    if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
                    {
                        MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
                    }

                    if (string.IsNullOrEmpty(condition_path) && false == string.IsNullOrEmpty(condition_operator))
                    {
                        throw new Exception("condition_path is null or empty, but condition_operator is not null or empty.");
                    }
                    else if (false == string.IsNullOrEmpty(condition_path) && string.IsNullOrEmpty(condition_operator))
                    {
                        throw new Exception("condition_path is not null or empty, but condition_operator is null or empty.");
                    }

                    if (false == string.IsNullOrEmpty(condition_path))
                    {
                        if (false == condition_operator.StartsWith("EQ(") ||
                            false == condition_operator.EndsWith(")"))
                        {
                            throw new Exception($"\"{condition_operator}\" not supported operator.");
                        }
                    }

                    if (0 == tasks.Count)
                    {
                        throw new Exception("tasks is empty.");
                    }

                    foreach (var task in tasks)
                    {
                        task.Check();
                    }

                }
            }

            public string process_name = null;
            public AppSetting application = null;
            public List<PostProcess> post_processes = new List<PostProcess>();

            public void Check()
            {
                if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
                {
                    MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
                }

                if (string.IsNullOrEmpty(process_name))
                {
                    throw new Exception("process_name is null or empty.");
                }

                if (null == application)
                {
                    throw new Exception("application is null.");
                }

                foreach (var post_process in post_processes)
                {
                    post_process.Check();
                }

                application.Check();
            }

        }

        public string input_name = null;
        public List<Process> processes = new List<Process>();

        static Logger MyLogger { get; } = Logger.GetLogger(typeof(MyDesiredProperties));

        public static MyDesiredProperties Deserialize(string desiredProperties)
        {
            MyDesiredProperties retDeserialized = null;

            try
            {
                retDeserialized = JsonConvert.DeserializeObject<MyDesiredProperties>(desiredProperties);

            }
            catch
            {
                throw new Exception("Unexpected data type");
            }

            return retDeserialized;
        }

        public List<string> Dump()
        {
            List<string> retList = new List<string>();
            try
            {
                string serialized = new Serializer().Serialize(this);
                retList = serialized.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            }
            catch
            {
                throw new Exception("Failed to serialization.");
            }

            return retList;
        }

        public void Check()
        {

            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }

            if (string.IsNullOrEmpty(input_name))
            {
                throw new Exception("input_name is null or empty.");
            }

            if (0 == processes.Count)
            {
                throw new Exception("processes is empty.");
            }

            foreach (var process in processes)
            {
                process.Check();
            }

        }
    }
}
