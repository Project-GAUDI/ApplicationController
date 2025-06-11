namespace IotedgeV2ApplicationController.Test;
public static class MyDesiredPropertiesCreater
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

    public static MyDesiredProperties.Process CreateValidProcess(string? dummyProcessId)
    {
        MyDesiredProperties.Process proc = new()
        {
            process_name = dummyProcessId,
            application = new()
            {
                type = MyDesiredProperties.Process.AppSetting.Protocol.http,
                url = "http://aaa.bbb/ccc"
            },
            post_processes = [ CreateValidPostProcess() ]
        };

        return proc;
    }

    public static MyDesiredProperties.Process.PostProcess CreateValidPostProcess()
    {
        MyDesiredProperties.Process.PostProcess postProcess = new()
        {
            condition_path = null,
            condition_operator = null,
            tasks = [ CreateValidPostProcessTask("dummy-output001") ]
        };

        return postProcess;
    }

    public static MyDesiredProperties.Process.PostProcess.Task CreateValidPostProcessTask(string? dummyOutputName)
    {
        MyDesiredProperties.Process.PostProcess.Task task = new()
        {
            output_name = dummyOutputName,
            set_values = [ CreateValidSetValue() ]
        };

        return task;
    }

    public static MyDesiredProperties.Process.PostProcess.Task.SetValue CreateValidSetValue(string? dummyKey = "$.A", string? dummyValue = "ABC")
    {
        MyDesiredProperties.Process.PostProcess.Task.SetValue definition = new()
        {
            type = MyDesiredProperties.MessageData.Body,
            key = dummyKey,
            value = dummyValue
        };

        return definition;
    }

    public static MyDesiredProperties.Process.AppSetting CreateValidAppSetting()
    {
        MyDesiredProperties.Process.AppSetting setting = new ()
        {
            type = MyDesiredProperties.Process.AppSetting.Protocol.http,
            url = "https://valid-schema-dummy-host.org",
            replace_params = [ CreateValidReplaceParam() ]
        };

        return setting;
    }

    public static MyDesiredProperties.Process.AppSetting.ReplaceParam CreateValidReplaceParam()
    {
        MyDesiredProperties.Process.AppSetting.ReplaceParam param = new ()
        {
            base_name = "dummy-host",
            source_data_key = "source-data-key",
            source_data_type = MyDesiredProperties.MessageData.Body
        };

        return param;
    }
}
