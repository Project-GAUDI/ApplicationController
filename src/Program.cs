namespace ApplicationController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Loader;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Shared;
    using TICO.GAUDI.Commons;

    class Program
    {
        static IModuleClient MyModuleClient { get; set; } = null;

        static Logger MyLogger { get; } = Logger.GetLogger(typeof(Program));

        static bool IsReady { get; set; } = false;

        private static IApplicationClientFactory MyClientFactory { get; } = new ApplicationClientFactory();
        private static EnvironmentInfo MyEnvInfo { get; set; } = null;
        private static MyDesiredProperties myDesiredProperties { get; set; } = null;

        const string PROPKEY_PROCESSNAME = "process_name";

        static void Main(string[] args)
        {
            try
            {
                Init().Wait();
            }
            catch (Exception e)
            {
                MyLogger.WriteLog(Logger.LogLevel.ERROR, $"Init failed. {e}", true);
                Environment.Exit(1);
            }

            // Wait until the app unloads or is cancelled
            var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();
            WhenCancelled(cts.Token).Wait();
        }

        /// <summary>
        /// Handles cleanup operations when app is cancelled or unloads
        /// </summary>
        public static Task WhenCancelled(CancellationToken cancellationToken)
        {
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Initializes the ModuleClient and sets up the callback to receive
        /// </summary>
        static async Task Init()
        {
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }
            
            IsReady = false;

            // 取得済みのModuleClientを解放する
            if (MyModuleClient != null)
            {
                await MyModuleClient.CloseAsync();
                MyModuleClient.Dispose();
                MyModuleClient = null;
            }

            // 環境変数から送信トピックを判定
            TransportTopic defaultSendTopic = TransportTopic.Iothub;
            string sendTopicEnv = Environment.GetEnvironmentVariable("DefaultSendTopic");
            if (Enum.TryParse(sendTopicEnv, true, out TransportTopic sendTopic))
            {
                MyLogger.WriteLog(Logger.LogLevel.INFO, $"Evironment Variable \"DefaultSendTopic\" is {sendTopicEnv}.");
                defaultSendTopic = sendTopic;
            }
            else
            {
                MyLogger.WriteLog(Logger.LogLevel.DEBUG, "Evironment Variable \"DefaultSendTopic\" is not set.");
            }

            // 環境変数から受信トピックを判定
            TransportTopic defaultReceiveTopic = TransportTopic.Iothub;
            string receiveTopicEnv = Environment.GetEnvironmentVariable("DefaultReceiveTopic");
            if (Enum.TryParse(receiveTopicEnv, true, out TransportTopic receiveTopic))
            {
                MyLogger.WriteLog(Logger.LogLevel.INFO, $"Evironment Variable \"DefaultReceiveTopic\" is {receiveTopicEnv}.");
                defaultReceiveTopic = receiveTopic;
            }
            else
            {
                MyLogger.WriteLog(Logger.LogLevel.DEBUG, "Evironment Variable \"DefaultReceiveTopic\" is not set.");
            }

            // MqttModuleClientを作成
            if (Boolean.TryParse(Environment.GetEnvironmentVariable("M2MqttFlag"), out bool m2mqttFlag) && m2mqttFlag)
            {
                string sasTokenEnv = Environment.GetEnvironmentVariable("SasToken");
                MyModuleClient = new MqttModuleClient(sasTokenEnv, defaultSendTopic: defaultSendTopic, defaultReceiveTopic: defaultReceiveTopic);
            }
            // IoTHubModuleClientを作成
            else
            {
                ITransportSettings[] settings = null;
                string protocolEnv = Environment.GetEnvironmentVariable("TransportProtocol");
                if (Enum.TryParse(protocolEnv, true, out TransportProtocol transportProtocol))
                {
                    MyLogger.WriteLog(Logger.LogLevel.INFO, $"Evironment Variable \"TransportProtocol\" is {protocolEnv}.");
                    settings = transportProtocol.GetTransportSettings();
                }
                else
                {
                    MyLogger.WriteLog(Logger.LogLevel.DEBUG, "Evironment Variable \"TransportProtocol\" is not set.");
                }

                MyModuleClient = await IotHubModuleClient.CreateAsync(settings, defaultSendTopic, defaultReceiveTopic).ConfigureAwait(false);
            }

            // edgeHubへの接続
            while (true)
            {
                try
                {
                    await MyModuleClient.OpenAsync().ConfigureAwait(false);
                    break;
                }
                catch (Exception e)
                {
                    MyLogger.WriteLog(Logger.LogLevel.WARN, $"Open a connection to the Edge runtime is failed. {e.Message}");
                    await Task.Delay(1000);
                }
            }

            // Loggerへモジュールクライアントを設定
            Logger.SetModuleClient(MyModuleClient);

            // 環境変数からログレベルを設定
            string logEnv = Environment.GetEnvironmentVariable("LogLevel");
            try
            {
                if (logEnv != null) Logger.SetOutputLogLevel(logEnv);
                MyLogger.WriteLog(Logger.LogLevel.INFO, $"Output log level is: {Logger.OutputLogLevel.ToString()}");
            }
            catch (ArgumentException e)
            {
                MyLogger.WriteLog(Logger.LogLevel.WARN, $"Environment LogLevel does not expected string. Exception:{e.Message}");
            }

            // カスタム環境変数の読込
            bool customEnvLoaded = false;
            try
            {
                MyEnvInfo = EnvironmentInfo.CreateInstance();
                MyLogger.WriteLog(Logger.LogLevel.INFO, $"Custom Environment:\n{MyEnvInfo.ToString()}");
                customEnvLoaded = true;
            }
            catch (Exception e)
            {
                MyLogger.WriteLog(Logger.LogLevel.ERROR, $"Custum Environment Load Error.  {e}", true);
                customEnvLoaded = false;
            }

            // desiredプロパティの取得
            var twin = await MyModuleClient.GetTwinAsync().ConfigureAwait(false);
            var collection = twin.Properties.Desired;
            IsReady = customEnvLoaded & SetMyProperties(collection);

            // プロパティ更新時のコールバックを登録
            await MyModuleClient.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertiesUpdate, null).ConfigureAwait(false);

            // メッセージ受信時のコールバックを登録
            if (IsReady)
            {
                await MyModuleClient.SetInputMessageHandlerAsync(myDesiredProperties.input_name, ReceiveMessage, null).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// プロパティ更新時のコールバック処理
        /// </summary>
        static async Task OnDesiredPropertiesUpdate(TwinCollection desiredProperties, object userContext)
        {
            MyLogger.WriteLog(Logger.LogLevel.INFO, "OnDesiredPropertiesUpdate Called.");

            try
            {
                await Init();
            }
            catch (Exception e)
            {
                MyLogger.WriteLog(Logger.LogLevel.ERROR, $"OnDesiredPropertiesUpdate failed. {e}", true);
            }
        }

        /// <summary>
        /// メッセージ受信時のコールバック処理
        /// </summary>
        static async Task<MessageResponse> ReceiveMessage(IotMessage message, object userContext)
        {
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }

            try
            {
                string body = message.GetBodyString();
                IDictionary<string, string> properties = message.GetProperties();
                string propertiesString = string.Join(",", properties.Select(kvp => kvp.ToString()));

                if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
                {
                    MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Received Message. Body: [{body}]");
                    MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Received Message. Properties: [{propertiesString}]");
                }

                if (false == properties.ContainsKey(PROPKEY_PROCESSNAME))
                {
                    throw new Exception("process_name is not in Properties.");
                }

                string processName = properties[PROPKEY_PROCESSNAME];

                // アプリ呼び出し処理起動
                ApplicationController obj = new ApplicationController(MyClientFactory, MyEnvInfo, myDesiredProperties.processes, MyModuleClient, body, properties);
                await obj.Execute(processName);
            }
            catch (Exception e)
            {
                MyLogger.WriteLog(Logger.LogLevel.ERROR, $"ReceiveMessage failed. {e}", true);
            }

            return MessageResponse.Completed;
        }

        /// <summary>
        /// desiredプロパティから自クラスのプロパティをセットする
        /// </summary>
        /// <returns>desiredプロパティに想定しない値があればfalseを返す</returns>
        static bool SetMyProperties(TwinCollection desiredProperties)
        {
            if ((int)Logger.OutputLogLevel <= (int)Logger.LogLevel.TRACE)
            {
                MyLogger.WriteLog(Logger.LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }
            
            bool result = true;
            try
            {
                string jsonDesiredProperties = desiredProperties.ToJson();
                MyLogger.WriteLog(Logger.LogLevel.DEBUG, $"DesiredProperties is {jsonDesiredProperties}.");

                myDesiredProperties = MyDesiredProperties.Deserialize(jsonDesiredProperties);
                myDesiredProperties.Check();

                List<string> dumps = myDesiredProperties.Dump();
                MyLogger.WriteLog(Logger.LogLevel.INFO, $"DesiredProperty dump.");
                foreach (string dump in dumps)
                {
                    MyLogger.WriteLog(Logger.LogLevel.INFO, $"  {dump}");
                }
            }
            catch (Exception e)
            {
                MyLogger.WriteLog(Logger.LogLevel.ERROR, $"SetMyProperties failed. {e}", true);
                result = false;
            }

            return result;
        }
    }
}
