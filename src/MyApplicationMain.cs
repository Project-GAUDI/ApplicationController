using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TICO.GAUDI.Commons;

namespace IotedgeV2ApplicationController
{
    /// <summary>
    /// Application Main class
    /// </summary>
    internal class MyApplicationMain : IApplicationMain
    {
        static ILogger MyLogger { get; } = LoggerFactory.GetLogger(typeof(MyApplicationMain));
        private static IApplicationClientFactory MyClientFactory { get; } = new ApplicationClientFactory();
        private static IMessageSender MySender { get; } = new MessageSender();
        private static EnvironmentInfo MyEnvInfo { get; set; } = null;
        private static MyDesiredProperties myDesiredProperties { get; set; } = null;
        const string PROPKEY_PROCESSNAME = "process_name";

        public void Dispose()
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: Dispose");

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: Dispose");
        }

        /// <summary>
        /// アプリケーション初期化					
        /// システム初期化前に呼び出される
        /// </summary>
        /// <returns></returns>
        public async Task<bool> InitializeAsync()
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: InitializeAsync");

            // ここでApplicationMainの初期化処理を行う。
            // 通信は未接続、DesiredPropertiesなども未取得の状態
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここから＝＝＝＝＝＝＝＝＝＝＝＝＝
            bool retStatus = true;
            try
            {
                MyEnvInfo = EnvironmentInfo.CreateInstance();
                MyLogger.WriteLog(ILogger.LogLevel.INFO, $"Custom Environment:\n{MyEnvInfo.ToString()}");
                retStatus = true;
            }
            catch (Exception ex)
            {
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"Custum Environment Load Error.  {ex}", true);
                retStatus = false;
            }

            await Task.CompletedTask;
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここまで＝＝＝＝＝＝＝＝＝＝＝＝＝

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: InitializeAsync");
            return retStatus;
        }

        /// <summary>
        /// アプリケーション起動処理					
        /// システム初期化完了後に呼び出される
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<bool> StartAsync()
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: StartAsync");

            // ここでApplicationMainの起動処理を行う。
            // 通信は接続済み、DesiredProperties取得済みの状態
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここから＝＝＝＝＝＝＝＝＝＝＝＝＝
            bool retStatus = true;

            IApplicationEngine appEngine = ApplicationEngineFactory.GetEngine();

            // メッセージ受信時のコールバック定義
            await appEngine.AddMessageInputHandlerAsync(myDesiredProperties.input_name, OnMessageReceivedAsync, null).ConfigureAwait(false);

            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここまで＝＝＝＝＝＝＝＝＝＝＝＝＝

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: StartAsync");
            return retStatus;
        }

        /// <summary>
        /// アプリケーション解放。					
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TerminateAsync()
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: TerminateAsync");

            // ここでApplicationMainの終了処理を行う。
            // アプリケーション終了時や、
            // DesiredPropertiesの更新通知受信後、
            // 通信切断時の回復処理時などに呼ばれる。
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここから＝＝＝＝＝＝＝＝＝＝＝＝＝
            bool retStatus = true;

            await Task.CompletedTask;
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここまで＝＝＝＝＝＝＝＝＝＝＝＝＝

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: TerminateAsync");
            return retStatus;
        }


        /// <summary>
        /// DesiredPropertis更新コールバック。					
        /// </summary>
        /// <param name="desiredProperties">DesiredPropertiesデータ。JSONのルートオブジェクトに相当。</param>
        /// <returns></returns>
        public async Task<bool> OnDesiredPropertiesReceivedAsync(JObject desiredProperties)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: OnDesiredPropertiesReceivedAsync");

            // DesiredProperties更新時の反映処理を行う。
            // 必要に応じて、メンバ変数への格納等を実施。
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここから＝＝＝＝＝＝＝＝＝＝＝＝＝
            bool retStatus = true;
            try
            {
                string jsonDesiredProperties = desiredProperties.ToString();
                MyLogger.WriteLog(ILogger.LogLevel.DEBUG, $"DesiredProperties is {jsonDesiredProperties}.");

                myDesiredProperties = MyDesiredProperties.Deserialize(jsonDesiredProperties);
                myDesiredProperties.Check();

                List<string> dumps = myDesiredProperties.Dump();
                MyLogger.WriteLog(ILogger.LogLevel.INFO, $"DesiredProperty dump.");
                foreach (string dump in dumps)
                {
                    MyLogger.WriteLog(ILogger.LogLevel.INFO, $"  {dump}");
                }
            }
            catch (Exception ex)
            {
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"OnDesiredPropertiesReceivedAsync failed. {ex}", true);
                retStatus = false;
            }
            await Task.CompletedTask;
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここまで＝＝＝＝＝＝＝＝＝＝＝＝＝

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: OnDesiredPropertiesReceivedAsync");

            return retStatus;
        }

        /// <summary>
        /// メッセージ受信コールバック。					
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="message"></param>
        /// <param name="userContext"></param>
        /// <returns>
        /// 受信処理成否
        ///     true : 処理成功。
        ///     false ： 処理失敗。edgeHubから再送を受ける。
        /// </returns>
        public async Task<bool> OnMessageReceivedAsync(string inputName,IotMessage message,object userContext)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: OnMessageReceivedAsync");

            // メッセージ受信時のコールバック処理を行う。
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここから＝＝＝＝＝＝＝＝＝＝＝＝＝
            bool retStatus = true;

            try
            {
                string body = message.GetBodyString();
                IDictionary<string, string> properties = message.GetProperties();
                string propertiesString = string.Join(",", properties.Select(kvp => kvp.ToString()));

                MyLogger.WriteLog(ILogger.LogLevel.INFO, "1 message received.");

                if (MyLogger.IsLogLevelToOutput(ILogger.LogLevel.TRACE))
                {
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Received Message. Body: [{body}]");
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Received Message. Properties: [{propertiesString}]");
                }

                if (false == properties.ContainsKey(PROPKEY_PROCESSNAME))
                {
                    throw new Exception("process_name is not in Properties.");
                }

                string processName = properties[PROPKEY_PROCESSNAME];

                // アプリ呼び出し処理起動
                ApplicationController obj = new ApplicationController(MyClientFactory,MySender,MyEnvInfo, myDesiredProperties.processes, body, properties);
                await obj.Execute(processName);
            }
            catch (Exception ex)
            {
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"OnMessageReceivedAsync failed. {ex}", true);
                retStatus = false;
            }
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここまで＝＝＝＝＝＝＝＝＝＝＝＝＝

            MyLogger.WriteLog(ILogger.LogLevel.DEBUG, $"Return status : {retStatus}");
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: OnMessageReceivedAsync");
            return retStatus;
        }
    }
}