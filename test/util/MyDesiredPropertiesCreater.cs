using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IotedgeV2ApplicationController.Test
{
    public class MyDesiredPropertiesCreater
    {
        public static MyDesiredProperties Create()
        {
            MyDesiredProperties myDesiredProperties = new MyDesiredProperties();

            MyDesiredProperties.Process proc = new MyDesiredProperties.Process();

            proc.process_name = "test";

            MyDesiredProperties.Process.AppSetting app = new MyDesiredProperties.Process.AppSetting();
            app.type = MyDesiredProperties.Process.AppSetting.Protocol.http;
            proc.application = app;

            proc.application.url = "http://aaa.bbb/ccc";

            proc.post_processes = new List<MyDesiredProperties.Process.PostProcess>();
            MyDesiredProperties.Process.PostProcess post_process = new MyDesiredProperties.Process.PostProcess();
            post_process.condition_path = null;
            post_process.condition_operator = null;
            post_process.tasks = new List<MyDesiredProperties.Process.PostProcess.Task>();

            MyDesiredProperties.Process.PostProcess.Task task = new MyDesiredProperties.Process.PostProcess.Task();
            task.output_name = "output1";

            task.set_values = new List<MyDesiredProperties.Process.PostProcess.Task.SetValue>();

            MyDesiredProperties.Process.PostProcess.Task.SetValue setValue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
            setValue.type = MyDesiredProperties.MessageData.Body;
            setValue.key = "$.A";
            setValue.value = "ABC";
            task.set_values.Add(setValue);

            post_process.tasks.Add(task);
            proc.post_processes.Add(post_process);

            myDesiredProperties.processes.Add(proc);
            return myDesiredProperties;
        }

        public static MyDesiredProperties CreateThreePostProccesses()
        {
            MyDesiredProperties myDesiredProperties = new MyDesiredProperties();

            MyDesiredProperties.Process proc = new MyDesiredProperties.Process();

            proc.process_name = "test";

            MyDesiredProperties.Process.AppSetting app = new MyDesiredProperties.Process.AppSetting();
            app.type = MyDesiredProperties.Process.AppSetting.Protocol.http;
            proc.application = app;

            proc.application.url = "http://aaa.bbb/ccc";

            proc.post_processes = new List<MyDesiredProperties.Process.PostProcess>();

            {
                MyDesiredProperties.Process.PostProcess post_process = new MyDesiredProperties.Process.PostProcess();
                post_process.condition_path = null;
                post_process.condition_operator = null;
                post_process.tasks = new List<MyDesiredProperties.Process.PostProcess.Task>();

                MyDesiredProperties.Process.PostProcess.Task task = new MyDesiredProperties.Process.PostProcess.Task();
                task.output_name = "output1";

                task.set_values = new List<MyDesiredProperties.Process.PostProcess.Task.SetValue>();

                MyDesiredProperties.Process.PostProcess.Task.SetValue setValue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
                setValue.type = MyDesiredProperties.MessageData.Body;
                setValue.key = "$.A";
                setValue.value = "ABC";
                task.set_values.Add(setValue);

                post_process.tasks.Add(task);
                proc.post_processes.Add(post_process);
            }


            {
                MyDesiredProperties.Process.PostProcess post_process = new MyDesiredProperties.Process.PostProcess();
                post_process.condition_path = null;
                post_process.condition_operator = null;
                post_process.tasks = new List<MyDesiredProperties.Process.PostProcess.Task>();

                MyDesiredProperties.Process.PostProcess.Task task = new MyDesiredProperties.Process.PostProcess.Task();
                task.output_name = "output2";

                task.set_values = new List<MyDesiredProperties.Process.PostProcess.Task.SetValue>();

                MyDesiredProperties.Process.PostProcess.Task.SetValue setValue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
                setValue.type = MyDesiredProperties.MessageData.Body;
                setValue.key = "$.D";
                setValue.value = "DEF";
                task.set_values.Add(setValue);

                post_process.tasks.Add(task);
                proc.post_processes.Add(post_process);
            }

            {
                MyDesiredProperties.Process.PostProcess post_process = new MyDesiredProperties.Process.PostProcess();
                post_process.condition_path = null;
                post_process.condition_operator = null;
                post_process.tasks = new List<MyDesiredProperties.Process.PostProcess.Task>();

                MyDesiredProperties.Process.PostProcess.Task task = new MyDesiredProperties.Process.PostProcess.Task();
                task.output_name = "output3";

                task.set_values = new List<MyDesiredProperties.Process.PostProcess.Task.SetValue>();

                MyDesiredProperties.Process.PostProcess.Task.SetValue setValue = new MyDesiredProperties.Process.PostProcess.Task.SetValue();
                setValue.type = MyDesiredProperties.MessageData.Body;
                setValue.key = "$.G";
                setValue.value = "GHI";
                task.set_values.Add(setValue);

                post_process.tasks.Add(task);
                proc.post_processes.Add(post_process);
            }


            myDesiredProperties.processes.Add(proc);
            return myDesiredProperties;
        }

    }
}