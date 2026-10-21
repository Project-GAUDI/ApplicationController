using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TICO.GAUDI.Commons;

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
                }

                public class ReplaceParam
                {
                    public string base_name = null;
                    public MessageData source_data_type = MessageData.NotSelected;
                    public string source_data_key = null;
                }

                public Protocol type = Protocol.NotSelected;
                public string url = null;
                public List<ReplaceParam> replace_params = new List<ReplaceParam>();

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

                    }
                    public string output_name = null;
                    public string next_process = null;
                    public List<SetValue> set_values = new List<SetValue>();

                }

                public string condition_path = null;
                public string condition_operator = null;
                public List<Task> tasks = new List<Task>();

            }

            public string process_name = null;
            public AppSetting application = null;
            public List<PostProcess> post_processes = new List<PostProcess>();

        }

        public string input_name = null;
        public List<Process> processes = new List<Process>();

        /// <summary>
        /// JObjectからMyDesiredPropertiesを生成する（Util.GetDesiredProperty利用版）
        /// </summary>
        /// <param name="jobj">DesiredPropertiesのJObject</param>
        /// <returns>MyDesiredPropertiesインスタンス</returns>
        public static MyDesiredProperties GetDesiredProperties(JObject jobj)
        {
            var props = new MyDesiredProperties();

            props.input_name = Util.GetDesiredProperty<string>(jobj, "input_name", true);
            var processesArray = Util.GetDesiredProperty<JArray>(jobj, "processes", true);
            if (processesArray.Count == 0)
            {
                throw new Exception("'processes' array must not be empty.");
            }
            foreach (var procToken in processesArray)
            {
                var procObj = procToken as JObject;
                var proc = new Process();
                proc.process_name = Util.GetDesiredProperty<string>(procObj, "process_name", true);
                var appObj = Util.GetDesiredProperty<JObject>(procObj, "application", true, null);
                proc.application = new Process.AppSetting();
                proc.application.type = Util.GetDesiredProperty(appObj, "type", false, Process.AppSetting.Protocol.http);
                proc.application.url = Util.GetDesiredProperty<string>(appObj, "url", true);
                var repArr = Util.GetDesiredProperty<JArray>(appObj, "replace_params", false, null);
                if (repArr != null)
                {
                    if (repArr.Count == 0)
                    {
                        throw new Exception("'replace_params' array must not be empty.");
                    }
                    foreach (var repToken in repArr)
                    {
                        var repObj = repToken as JObject;
                        var rep = new Process.AppSetting.ReplaceParam();
                        rep.base_name = Util.GetDesiredProperty<string>(repObj, "base_name", true, condition: (value) => proc.application.url.Contains(value));
                        rep.source_data_type = Util.GetDesiredProperty<MessageData>(repObj, "source_data_type", true);
                        rep.source_data_key = Util.GetDesiredProperty<string>(repObj, "source_data_key", true);
                        proc.application.replace_params.Add(rep);
                    }
                }
                var postArr = Util.GetDesiredProperty<JArray>(procObj, "post_processes", false, null);
                if (postArr != null)
                {
                    if (postArr.Count == 0)
                    {
                        throw new Exception("'post_processes' array must not be empty.");
                    }
                    foreach (var postToken in postArr)
                    {
                        var postObj = postToken as JObject;
                        var post = new Process.PostProcess();
                        post.condition_path = Util.GetDesiredProperty<string>(postObj, "condition_path", false, null);
                        post.condition_operator = Util.GetDesiredProperty<string>(postObj, "condition_operator", false, null, condition: (value) => value == null || (value.StartsWith("EQ(") && value.EndsWith(")")));
                        if (string.IsNullOrEmpty(post.condition_path) != string.IsNullOrEmpty(post.condition_operator))
                        {
                            throw new Exception("Both 'condition_path' and 'condition_operator' must be specified together or both omitted.");
                        }
                        var taskArr = Util.GetDesiredProperty<JArray>(postObj, "tasks", true);
                        if (taskArr.Count == 0)
                        {
                            throw new Exception("'tasks' array must not be empty.");
                        }
                        foreach (var taskToken in taskArr)
                        {
                            var taskObj = taskToken as JObject;
                            var task = new Process.PostProcess.Task();
                            task.output_name = Util.GetDesiredProperty<string>(taskObj, "output_name", true);
                            task.next_process = Util.GetDesiredProperty<string>(taskObj, "next_process", false, null);
                            var setArr = Util.GetDesiredProperty<JArray>(taskObj, "set_values", false, null);
                            if (setArr != null)
                            {
                                if (setArr.Count == 0)
                                {
                                    throw new Exception("'set_values' array must not be empty.");
                                }
                                foreach (var setToken in setArr)
                                {
                                    var setObj = setToken as JObject;
                                    var set = new Process.PostProcess.Task.SetValue();
                                    set.type = Util.GetDesiredProperty<MessageData>(setObj, "type", true);
                                    set.key = Util.GetDesiredProperty<string>(setObj, "key", true);
                                    set.value = Util.GetDesiredProperty<string>(setObj, "value", true);
                                    task.set_values.Add(set);
                                }
                            }
                            post.tasks.Add(task);
                        }
                        proc.post_processes.Add(post);
                    }
                }
                props.processes.Add(proc);
            }
            return props;
        }
    }
}