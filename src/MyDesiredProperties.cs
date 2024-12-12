using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using TICO.GAUDI.Commons;
using YamlDotNet.Serialization;

namespace IotedgeV2ApplicationController
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

                        if (string.IsNullOrEmpty(base_name))
                        {
                            var errmsg = $"base_name is null or empty.";
                            throw new Exception(errmsg);
                        }

                        if (MessageData.NotSelected == source_data_type)
                        {
                            var errmsg = $"source_data_type is not set.";
                            throw new Exception(errmsg);
                        }

                        if (string.IsNullOrEmpty(source_data_key))
                        {
                            var errmsg = $"source_data_key is null or empty.";
                            throw new Exception(errmsg);
                        }

                    }
                }

                public Protocol type = Protocol.NotSelected;
                public string url = null;
                public List<ReplaceParam> replace_params = new List<ReplaceParam>();

                public void Check()
                {

                    if (Protocol.NotSelected == type)
                    {
                        var errmsg = $"application type is not set.";
                        throw new Exception(errmsg);
                    }

                    if (string.IsNullOrEmpty(url))
                    {
                        var errmsg = $"url is null or empty.";
                        throw new Exception(errmsg);
                    }

                    foreach (var param in replace_params)
                    {
                        param.Check();
                        if (false == url.Contains(param.base_name))
                        {
                            var errmsg = $"url is not contain base_name.(url={url},base_name={param.base_name})";
                            throw new Exception(errmsg);
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

                            if (MessageData.NotSelected == type)
                            {
                                var errmsg = $"set_values type is not set.";
                                throw new Exception(errmsg);
                            }

                            if (string.IsNullOrEmpty(key))
                            {
                                var errmsg = $"set_values key is null or empty.";
                                throw new Exception(errmsg);
                            }

                            if (string.IsNullOrEmpty(value))
                            {
                                var errmsg = $"set_values value is null or empty.";
                                throw new Exception(errmsg);
                            }
                        }
                    }
                    public string output_name = null;
                    public string next_process = null;
                    public List<SetValue> set_values = new List<SetValue>();

                    public void Check()
                    {

                        if (string.IsNullOrEmpty(output_name))
                        {
                            var errmsg = $"output_name is null or empty.";
                            throw new Exception(errmsg);
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

                    if (string.IsNullOrEmpty(condition_path) && false == string.IsNullOrEmpty(condition_operator))
                    {
                        var errmsg = $"condition_path is null or empty, but condition_operator is not null or empty.";
                        throw new Exception(errmsg);
                    }
                    else if (false == string.IsNullOrEmpty(condition_path) && string.IsNullOrEmpty(condition_operator))
                    {
                        var errmsg = $"condition_path is not null or empty, but condition_operator is null or empty.";
                        throw new Exception(errmsg);
                    }

                    if (false == string.IsNullOrEmpty(condition_path))
                    {
                        if (false == condition_operator.StartsWith("EQ(") ||
                            false == condition_operator.EndsWith(")"))
                        {
                            var errmsg = $"\"{condition_operator}\" not supported operator.";
                            throw new Exception(errmsg);
                        }
                    }

                    if (0 == tasks.Count)
                    {
                        var errmsg = $"tasks is empty.";
                        throw new Exception(errmsg);
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

                if (string.IsNullOrEmpty(process_name))
                {
                    var errmsg = $"process_name is null or empty.";
                    throw new Exception(errmsg);
                }

                if (null == application)
                {
                    var errmsg = $"application is null.";
                    throw new Exception(errmsg);
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
        public static MyDesiredProperties Deserialize(string desiredProperties)
        {
            MyDesiredProperties retDeserialized = null;

            try
            {
                retDeserialized = JsonConvert.DeserializeObject<MyDesiredProperties>(desiredProperties);

            }
            catch
            {
                var errmsg = $"Unexpected data type.";
                throw new Exception(errmsg);
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
                var errmsg = $"Failed to serialization.";
                throw new Exception(errmsg);
            }

            return retList;
        }

        public void Check()
        {
            if (string.IsNullOrEmpty(input_name))
            {
                var errmsg = $"input_name is null or empty.";
                throw new Exception(errmsg);
            }

            if (0 == processes.Count)
            {
                var errmsg = $"processes is empty.";
                throw new Exception(errmsg);
            }

            foreach (var process in processes)
            {
                process.Check();
            }

        }
    }
}
