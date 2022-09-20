using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Apache.NMS.Util;
using FEIBAgentService.DataModel;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Configuration;       
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FEIBAgentService
{
    public partial class AgentService : ServiceBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string RunType = ConfigurationManager.AppSettings.Get("AgentType").ToUpper();
        static bool RunServiceFlag = false;

        public AgentService()
        {
            InitializeComponent();      
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Logger.InfoFormat("Service Start, Running Type is {0}", RunType);
                RunServiceFlag = true;
                Uri connecturl = new Uri(string.Format("activemq:tcp://{0}:{1}", ConfigurationManager.AppSettings.Get("ActiveMQIP"), ConfigurationManager.AppSettings.Get("MQPort")));

                IConnectionFactory factory = new ConnectionFactory(connecturl);

                switch (RunType)
                {
                    case "QUEUE":

                        // 取得Request端進來的Queue名稱
                        string RquestQueueName = string.Format("queue://{0}", ConfigurationManager.AppSettings.Get("RequestQueueName"));
                        Task.Run(() =>
                        {
                            using (IConnection Queueconnection = factory.CreateConnection())
                            {
                                using (ISession Queuesession = Queueconnection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                                {
                                    IDestination destination = SessionUtil.GetDestination(Queuesession, RquestQueueName);
                                    using (IMessageConsumer consumer = Queuesession.CreateConsumer(destination))
                                    {

                                        // 開始進入監聽模式
                                        consumer.Listener += Consumer_Listener;
                                        Queueconnection.Start();
                                        Logger.InfoFormat("The Queue '{0} is Listener!", ConfigurationManager.AppSettings.Get("RequestQueueName"));

                                        while (RunServiceFlag)
                                        {
                                            Thread.Sleep(100);
                                        }

                                    }
                                }
                            }
                        });
                        break;

                    default:
                        // 取得Request端進來的Topic名稱
                        Task.Run(() =>
                        {
                            string TopicName = string.Format("topic://{0}", ConfigurationManager.AppSettings.Get("TopicName"));
                            IConnectionFactory topicfactory = new ConnectionFactory(connecturl);
                            using (IConnection topicconnection = topicfactory.CreateConnection())
                            {
                                using (ISession session = topicconnection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                                {
                                    IMessageConsumer topicconsumer = session.CreateDurableConsumer(
                                      new ActiveMQTopic(ConfigurationManager.AppSettings.Get("TopicName")),
                                      ConfigurationManager.AppSettings.Get("ListenerUser"), null, false);
                                    // 開始進入監聽模式
                                    topicconsumer.Listener += Consumer_Listener;
                                    topicconnection.Start();
                                    Logger.InfoFormat("The Topic '{0} is Listener!", ConfigurationManager.AppSettings.Get("TopicName"));

                                    while (RunServiceFlag)
                                    {
                                        Thread.Sleep(100);

                                    }

                                }
                            }
                        });
                        break;
                }

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex.Message.ToString());
            }

            
        }

        protected override void OnStop()
        {
            RunServiceFlag = false;
            Logger.Info("Service Stop");
        }

        /// <summary>
        /// 監聽元件
        /// </summary>
        /// <param name="message"></param>
        private static void Consumer_Listener(IMessage message)
        {
            Logger.Info("Listener Consumer Start!! ");
            argAgentCommand argContents;
            //argSyncFile FileContents = new argSyncFile();
            argRunCommand CmdContents = new argRunCommand();


            FileInfo fi = null;
            string DecompressStr = null;
            string ResponseQueueName = null;
            bool fileExists = false;
            try
            {
                switch (RunType)
                {
                    case "QUEUE":
                        ResponseQueueName = ConfigurationManager.AppSettings.Get("ResponseQueueName");
                        break;
                    default:
                        ResponseQueueName = ConfigurationManager.AppSettings.Get("TopicResponseQueueName");
                        break;
                }

                ITextMessage receivedMsg = message as ITextMessage;
                if (message != null)
                {
                    // 取得解壓縮字串
                    DecompressStr = DecodeStringZip(receivedMsg.Text);
                    // 透過Json元件套用Class
                    argContents = JsonConvert.DeserializeObject<argAgentCommand>(DecompressStr);
                    //檔案路徑
                    argContents.FileInfo.FilePath = ConfigurationManager.AppSettings["TOPICFilePath"];

                    Logger.InfoFormat("使用者(AD):{0}", argContents.User);
                    Logger.InfoFormat("接收檔案:{0}", argContents.FileInfo.FilePath + @"\\" + argContents.FileInfo.FileName);
                    Logger.InfoFormat("接收時間:{0}", DateTime.Now);

                    switch (argContents.CommandType)
                    {
                        case CommandRunType.File:
                            try
                            {
                                #region 檔案強制覆蓋為否時檢查檔案是否存在
                                if (!argContents.FileInfo.cbIsCover)
                                {
                                    // 驗證檔案是否存在
                                    if (File.Exists(argContents.FileInfo.FilePath + @"\\" + argContents.FileInfo.FileName))
                                    {
                                        fileExists = true;
                                    }
                                }
                                if(!fileExists)
                                {

                                    // 將檔案寫入資料夾中
                                    WriteFileToBase64(argContents.FileInfo.FilePath, argContents.FileInfo.FileName, argContents.FileInfo.FileContent);
                                    Logger.Info(argContents.FileInfo.FilePath + @"\\" + argContents.FileInfo.FileName + " Finish!!");
                                    //WriteFileToBase64(argContents.FileInfo.FilePath, argContents.FileInfo.FileName, argContents.FileInfo.FileContent);
                                    //Logger.Info(argContents.FileInfo.FilePath + @"\\" + argContents.FileInfo.FileName + " Finish!!");
                                }
                                #endregion

                                argContents.ReceiveTime = DateTime.Now;
                                argContents.FileInfo.TransferStatus = fileExists ? false : true;
                                argContents.FileInfo.TopicListener = ConfigurationManager.AppSettings.Get("ListenerUser");
                                argContents.Message = fileExists ?  "File Already Exists" : "File Write OK";

                                Logger.InfoFormat("回覆時間:{0}", argContents.ReceiveTime.ToLongTimeString());
                                Logger.InfoFormat("回覆結果:{0}[{1}]", argContents.FileInfo.TransferStatus, argContents.Message);

                                SendDataToResponseQueue(argContents, ResponseQueueName);
                            }
                            catch (Exception Ex)
                            {

                                argContents.ReceiveTime = DateTime.Now;
                                argContents.FileInfo.TransferStatus = false;
                                argContents.Message = Ex.Message.ToString();
                                argContents.FileInfo.TopicListener = ConfigurationManager.AppSettings.Get("ListenerUser");

                                SendDataToResponseQueue(argContents, ResponseQueueName);
                            }
                            break;
                        case CommandRunType.GetFile:
                            try
                            {
                                //從資料夾讀取檔案
                                //WriteFileToBase64(argContents.FileInfo.FilePath, argContents.FileInfo.FileName, argContents.FileInfo.FileContent);
                                //Logger.Info(argContents.FileInfo.FilePath + @"\\" + argContents.FileInfo.FileName + " Finish!!");

                                argContents.FileInfo.FileName = argContents.FileInfo.FileName;
                                argContents.FileInfo.RepFileContent = ReadFileToBase64(argContents.FileInfo.FilePath + @"\\" + argContents.FileInfo.FileName);
                                argContents.SendTime = DateTime.Now;

                                argContents.ReceiveTime = DateTime.Now;
                                argContents.FileInfo.TransferStatus = true;
                                argContents.FileInfo.TopicListener = ConfigurationManager.AppSettings.Get("ListenerUser");
                                argContents.Message = "Get File  OK";

                                SendDataToResponseQueue(argContents, ResponseQueueName);
                            }
                            catch (Exception Ex)
                            {
 

                                argContents.ReceiveTime = DateTime.Now;
                                argContents.FileInfo.TransferStatus = false;
                                argContents.Message = Ex.Message.ToString();
                                argContents.FileInfo.TopicListener = ConfigurationManager.AppSettings.Get("ListenerUser");

 
                                SendDataToResponseQueue(argContents, ResponseQueueName);
                            }
                            break;
                        case CommandRunType.Command:
                            RunCommand(argContents);
                            break;
                        case CommandRunType.SummitCommand:
                            RunSummitCommand(argContents);
                            break;

                        default:

                            break;
                    }

                }
            }
            catch (Exception Ex)
            {
                Logger.ErrorFormat("Listener Error {0}", Ex.Message.ToString());
            }
        }
        /// <summary>
        /// Run Command
        /// </summary>
        /// <param name="argContents"></param>
        private static void RunCommand(argAgentCommand argContents)
        {
            string ResponseQueueName;

            switch (RunType)
            {
                case "QUEUE":
                    ResponseQueueName = ConfigurationManager.AppSettings.Get("ResponseQueueName");
                    break;
                default:
                    ResponseQueueName = ConfigurationManager.AppSettings.Get("TopicResponseQueueName");
                    break;
            }

            Task.Run(() =>
            {
                Process p = new Process();

                p.StartInfo.FileName = argContents.CmdInfo.Cmd;
                p.StartInfo.Arguments = argContents.CmdInfo.Args;
                p.StartInfo.WorkingDirectory = argContents.CmdInfo.RunPath;
                // 不需啟動新視窗執行
                p.StartInfo.CreateNoWindow = true;
                // 不需使用Shell的方式執行
                p.StartInfo.UseShellExecute = false;
                // 將所有訊息值接導出
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardInput = true;

                // 當程式結束時，會執行Exited Event
                p.EnableRaisingEvents = true;
                // 定義批次結束時，該執行的動作
                p.Exited += new EventHandler(delegate (object sendingProcess, EventArgs outLine2)
                {
                    Logger.InfoFormat("End Run {0}", argContents.CmdInfo.Cmd);
                    argContents.CmdInfo.ReturnCode = (((Process)sendingProcess).ExitCode).ToString();
                });


                //  設定標準輸出處理
                p.OutputDataReceived += new DataReceivedEventHandler(delegate (object sendingProcess, DataReceivedEventArgs outLine)
                {
                    if (!String.IsNullOrEmpty(outLine.Data))
                    {

                        argContents.CmdInfo.StarndardOutput += outLine.Data;
                        Logger.Info(outLine.Data);
                    }
                });
                //  設定錯誤輸出處理
                p.ErrorDataReceived += new DataReceivedEventHandler(delegate (object sendingProcess, DataReceivedEventArgs outLine)
                {
                    if (!String.IsNullOrEmpty(outLine.Data))
                    {
                        argContents.CmdInfo.StarndardError += outLine.Data;
                        Logger.Info(outLine.Data);
                    }
                });

                try
                {
                    p.Start();
                    Logger.InfoFormat("Starting Run {0}", argContents.CmdInfo.Cmd);
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    p.WaitForExit();
                }
                catch (Exception ex)
                {
                    argContents.CmdInfo.StarndardError = ex.Message.ToString();
                }
                argContents.ReceiveTime = DateTime.Now;

                SendDataToResponseQueue(argContents, ResponseQueueName);
            });
        }
        /// <summary>
        /// Run Summit Command
        /// </summary>
        /// <param name="argContents"></param>
        private static void RunSummitCommand(argAgentCommand argContents)
        {
            string ResponseQueueName;

            switch (RunType)
            {
                case "QUEUE":
                    ResponseQueueName = ConfigurationManager.AppSettings.Get("ResponseQueueName");
                    break;
                default:
                    ResponseQueueName = ConfigurationManager.AppSettings.Get("TopicResponseQueueName");
                    break;
            }

            Task.Run(() =>
            {
                #region Set Summit Environment
                Hashtable EnvList = new Hashtable();
                string[] CheckStr;
                string NewStrline;
                foreach (string strline in System.IO.File.ReadLines(ConfigurationManager.AppSettings.Get("SummitEnvFile")))
                {
                    NewStrline = strline;
                    CheckStr = strline.Split('=');
                    int ChkPosition = strline.Split('%').Count();

                    if (CheckStr.Count() >= 2)  //Must has three argument
                    {
                        if (!CheckStr[1].Trim().Equals(""))
                        {
                            // Replace %
                            if (strline.Contains('%'))
                            {
                                foreach (DictionaryEntry EnvItem in EnvList)
                                {
                                    NewStrline = NewStrline.Replace("%" + EnvItem.Key.ToString() + "%", EnvItem.Value.ToString());
                                }
                            }

                            if (NewStrline.Contains('='))
                            {
                                CheckStr = NewStrline.Split('=');

                                EnvList.Add(CheckStr[0].Remove(0, 3).Trim(), CheckStr[1]);

                            }
                        }
                    }
                }

                foreach (DictionaryEntry d in EnvList)
                {
                    if (d.Key.ToString().Trim().ToUpper().Equals("PATH"))
                        Environment.SetEnvironmentVariable(d.Key.ToString(), d.Value.ToString().Replace("%PATH%", "") + Environment.GetEnvironmentVariable("PATH"));
                    else
                        Environment.SetEnvironmentVariable(d.Key.ToString(), d.Value.ToString());
                }
                #endregion
                Process p = new Process();

                p.StartInfo.FileName = argContents.CmdInfo.Cmd;
                p.StartInfo.Arguments = argContents.CmdInfo.Args;
                p.StartInfo.WorkingDirectory = argContents.CmdInfo.RunPath;
                // 不需啟動新視窗執行
                p.StartInfo.CreateNoWindow = true;
                // 不需使用Shell的方式執行
                p.StartInfo.UseShellExecute = false;
                // 將所有訊息值接導出
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardInput = true;

                // 當程式結束時，會執行Exited Event
                p.EnableRaisingEvents = true;
                // 定義批次結束時，該執行的動作
                p.Exited += new EventHandler(delegate (object sendingProcess, EventArgs outLine2)
                {
                    Logger.InfoFormat("End Run {0}", argContents.CmdInfo.Cmd);
                    argContents.CmdInfo.ReturnCode = (((Process)sendingProcess).ExitCode).ToString();
                });


                //  設定標準輸出處理
                p.OutputDataReceived += new DataReceivedEventHandler(delegate (object sendingProcess, DataReceivedEventArgs outLine)
                {
                    if (!String.IsNullOrEmpty(outLine.Data))
                    {

                        argContents.CmdInfo.StarndardOutput += outLine.Data;
                        Logger.Info(outLine.Data);
                    }
                });
                //  設定錯誤輸出處理
                p.ErrorDataReceived += new DataReceivedEventHandler(delegate (object sendingProcess, DataReceivedEventArgs outLine)
                {
                    if (!String.IsNullOrEmpty(outLine.Data))
                    {
                        argContents.CmdInfo.StarndardError += outLine.Data;
                        Logger.Info(outLine.Data);
                    }
                });

                try
                {
                    p.Start();
                    Logger.InfoFormat("Starting Run {0}", argContents.CmdInfo.Cmd);
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    p.WaitForExit();
                }
                catch (Exception ex)
                {
                    argContents.CmdInfo.StarndardError = ex.Message.ToString();
                }
                argContents.ReceiveTime = DateTime.Now;

                SendDataToResponseQueue(argContents, ResponseQueueName);
            });
        }

        static void SendDataToResponseQueue(argAgentCommand message, string QueueName)
        {
            string ResponseQueueName = null;

            string strCompress = null;
            Uri connecturl = new Uri(string.Format("activemq:tcp://{0}:{1}", ConfigurationManager.AppSettings.Get("ActiveMQIP"),
                ConfigurationManager.AppSettings.Get("MQPort")));

            ResponseQueueName = string.Format("topic://{0}", QueueName);
            //ResponseQueueName = string.Format("queue://{0}", QueueName);

            IConnectionFactory factory = new ConnectionFactory(connecturl);
            using (IConnection connection = factory.CreateConnection())
            {
                using (ISession session = connection.CreateSession())
                {
                    IDestination destination = SessionUtil.GetDestination(session, ResponseQueueName);
                    using (IMessageProducer producer = session.CreateProducer(destination))
                    {
                        // Compress String
                        strCompress = EncodeStringZip(JsonConvert.SerializeObject(message));
                        // Create Active MQ Message
                        ITextMessage request = session.CreateTextMessage(strCompress);

                        // Send To MQ
                        producer.Send(request);

                    }
                }
            }
        }
        #region File Function
        /// <summary>
        /// Read File To Base64 String
        /// </summary>
        /// <param name="FullName"></param>
        /// <returns></returns>
        private static string ReadFileToBase64(string FullName)
        {
            string RtVal = null;
            if (File.Exists(FullName))
            {
                Byte[] bytes = File.ReadAllBytes(FullName);
                RtVal = Convert.ToBase64String(bytes);
            }
            else
            {
                throw new Exception(string.Format("{0} not Exist!!", FullName));
            }
            return RtVal;
        }
        /// <summary>
        /// Write File From Base64
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="filename"></param>
        /// <param name="Base64Str"></param>
        private static void WriteFileToBase64(string filepath, string filename, string Base64Str)
        {
            if (Base64Str != null || !Base64Str.Trim().Equals(""))
            {
                // 將字串轉成Byte
                Byte[] bytes = Convert.FromBase64String(Base64Str);

                try
                {
                    // 驗證目錄是否存在
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    // 寫入檔案
                    File.WriteAllBytes(filepath + @"\\" + filename, bytes);

                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message);
                }

            }
        }
        /// <summary>
        /// Compress String To Base64
        /// </summary>
        /// <param name="strEncode"></param>
        /// <returns></returns>
        private static string EncodeStringZip(string strEncode)
        {
            try
            {
                if (string.IsNullOrEmpty(strEncode)) { return strEncode; }
                byte[] bytEncode = Encoding.UTF8.GetBytes(strEncode);
                // Compress
                using (var outStream = new MemoryStream())
                {
                    using (var zip = new GZipStream(outStream, CompressionMode.Compress))
                    {
                        zip.Write(bytEncode, 0, bytEncode.Length);
                        zip.Close();
                        return Convert.ToBase64String(outStream.ToArray());
                    }
                }

            }
            catch (Exception)
            {
                throw new Exception("UnKnow String!!");
            }
        }
        /// <summary>
        /// DeCompress From Base64
        /// </summary>
        /// <param name="compressed"></param>
        /// <returns></returns>
        private static string DecodeStringZip(string compressed)
        {
            try
            {
                if (string.IsNullOrEmpty(compressed)) { return compressed; }
                byte[] buffer = Convert.FromBase64String(compressed);
                using (var inStream = new MemoryStream(buffer))
                {
                    using (var outStream = new MemoryStream())
                    {
                        using (var zip = new GZipStream(inStream, CompressionMode.Decompress))
                        {
                            zip.CopyTo(outStream);
                            zip.Close();
                            string text = Encoding.UTF8.GetString(outStream.ToArray());
                            return text;
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw new Exception("UnKnow String!!");
            }
        }
        #endregion
    }
}
