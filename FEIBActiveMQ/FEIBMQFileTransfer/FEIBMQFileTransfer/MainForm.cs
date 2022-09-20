using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;
using FEIBMQFileTransfer.DataModel;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FEIBMQFileTransfer
{
    public partial class MainForm : Form
    {
        public bool ResponseListenerStatus = true;
        public Thread ListenQueueResponseThread, ListenTopicResponseThread;
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private delegate void DelegateShowMessage(string sMessage);
        private void AddMessage(string sMessage)
        {
            if (this.InvokeRequired)
            {
                DelegateShowMessage mi = new DelegateShowMessage(AddMessage);
                this.Invoke(mi, sMessage);
            }
            else
            {
                this.txtStatusMessage.Text += sMessage + Environment.NewLine;
            }
        }
        public MainForm()
        {
            InitializeComponent();
            txtTargetPath.Text = @"c:/temp";

            txtFileName.Text = @"應用三組相關資訊20210924_ARVIN_9.xlsx";
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var dropped = ((string[])e.Data.GetData(DataFormats.FileDrop));
            var files = dropped.ToList();

            if (!files.Any())
                return;

            //foreach(string drop in dropped)
            //{
            //    if (Directory.Exists(drop))
            //    {
            //        files.AddRange(Directory.GetFiles(drop, "*.*", SearchOption.AllDirectories));
            //    }
            //}
            listBoxFileList.Items.Clear();
            FileAttributes fa;
            FileInfo fi = null;
            foreach (string file in files)
            {
                fa = File.GetAttributes(file);
                if (fa != FileAttributes.Directory)
                {
                    fi = new FileInfo(file);
                    txtTargetPath.Text = fi.DirectoryName;
                    listBoxFileList.Items.Add(file);
                }
            }

        }

        private void MainForm_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listBoxFileList_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listBoxFileList_DragDrop(object sender, DragEventArgs e)
        {
            var dropped = ((string[])e.Data.GetData(DataFormats.FileDrop));
            var files = dropped.ToList();

            if (!files.Any())
                return;

            //foreach(string drop in dropped)
            //{
            //    if (Directory.Exists(drop))
            //    {
            //        files.AddRange(Directory.GetFiles(drop, "*.*", SearchOption.AllDirectories));
            //    }
            //}
            listBoxFileList.Items.Clear();
            FileAttributes fa;
            FileInfo fi = null;
            foreach (string file in files)
            {
                fa = File.GetAttributes(file);
                if (fa != FileAttributes.Directory)
                {
                    fi = new FileInfo(file);
                    txtTargetPath.Text = fi.DirectoryName;
                    listBoxFileList.Items.Add(file);
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

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            //Open wait form dialog
            using (WaitForm frm = new WaitForm(SendDataToQueue))
            {
                frm.ShowDialog(this);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ListenQueueResponseThread = new Thread(new ThreadStart(ListenResponseQueue));
            ListenQueueResponseThread.Start();
            ListenTopicResponseThread = new Thread(new ThreadStart(ListenResponseTopic));
            ListenTopicResponseThread.Start();
        }


        void SendDataToQueue()
        {
            argAgentCommand argContents = new argAgentCommand();
            argContents.CommandType = CommandRunType.File;
            argContents.FileInfo = new argSyncFile();

            FileInfo fi = null;
            string strCompress = null;
            Uri connecturl = new Uri(string.Format("activemq:tcp://{0}:{1}", ConfigurationManager.AppSettings.Get("ActiveMQIP"),
                ConfigurationManager.AppSettings.Get("MQPort")));
            string RequestQueueName = string.Format("queue://{0}", ConfigurationManager.AppSettings.Get("RequestQueueName"));
            string ResponseQueueName = string.Format("queue://{0}", ConfigurationManager.AppSettings.Get("ResponseQueueName"));

            IConnectionFactory factory = new ConnectionFactory(connecturl);
            using (IConnection connection = factory.CreateConnection())
            {
                using (ISession session = connection.CreateSession())
                {
                    IDestination destination = SessionUtil.GetDestination(session, RequestQueueName);
                    using (IMessageProducer producer = session.CreateProducer(destination))
                    {
                        // Begin to Send file
                        foreach (string sendfile in listBoxFileList.Items)
                        {
                            fi = new FileInfo(sendfile);
                            argContents.FileInfo.FilePath = txtTargetPath.Text;
                            argContents.FileInfo.FileName = fi.Name;
                            argContents.FileInfo.FileContent = ReadFileToBase64(sendfile);
                            argContents.SendTime = DateTime.Now;
                            // Compress String
                            strCompress = EncodeStringZip(JsonConvert.SerializeObject(argContents));
                            // Create Active MQ Message
                            ITextMessage request = session.CreateTextMessage(strCompress);
                            // Send To MQ
                            producer.Send(request);
                        }

                    }
                }
            }

        }
        #region Listen Response Queue
        void ListenResponseQueue()
        {
            argAgentCommand argContents = new argAgentCommand();


            string ReturnMessage = null;
            string DecompressStr = null;

            Uri connecturl = new Uri(string.Format("activemq:tcp://{0}:{1}", ConfigurationManager.AppSettings.Get("ActiveMQIP"),
                ConfigurationManager.AppSettings.Get("MQPort")));
            string ResponseQueueName = string.Format("queue://{0}", ConfigurationManager.AppSettings.Get("ResponseQueueName"));

            IConnectionFactory factory = new ConnectionFactory(connecturl);
            using (IConnection connection = factory.CreateConnection())
            {
                using (ISession session = connection.CreateSession())
                {
                    IDestination destination = SessionUtil.GetDestination(session, ResponseQueueName);
                    IMessageProducer producer = session.CreateProducer(destination);
                    IMessageConsumer consumer = session.CreateConsumer(destination);
                    connection.Start();

                    while (ResponseListenerStatus)
                    {
                        var message = (ITextMessage)consumer.Receive(TimeSpan.FromMilliseconds(100));
                        if (message != null)
                        {
                            try
                            {
                                // 取得解壓縮字串
                                DecompressStr = DecodeStringZip(message.Text);
                                // 透過Json元件套用Class
                                argContents = JsonConvert.DeserializeObject<argAgentCommand>(DecompressStr);
                                ReturnMessage = string.Format("FileName [{0}\\{1}] Start[{2}] End[{3}] [{4}] Say Status [{5}] ",
                                    argContents.FileInfo.FilePath,
                                    argContents.FileInfo.FileName,
                                    argContents.SendTime.ToLongTimeString(),
                                    argContents.ReceiveTime.ToLongTimeString(),
                                    argContents.FileInfo.TopicListener,
                                    argContents.FileInfo.TransferStatus);
                                AddMessage(ReturnMessage);
                                Logger.Info(ReturnMessage);
                            }
                            catch (Exception Ex)
                            {
                                Logger.Error(Ex.Message);
                            }
                        }
                        Thread.Sleep(100);
                    }
                }
            }
        }
        void ListenResponseTopic()
        {
            argAgentCommand argContents = new argAgentCommand();

            string ReturnMessage = null;
            string DecompressStr = null;
           
            Uri connecturl = new Uri(string.Format("activemq:tcp://{0}:{1}", ConfigurationManager.AppSettings.Get("ActiveMQIP"),
                ConfigurationManager.AppSettings.Get("MQPort")));
            string ResponseQueueName = string.Format("queue://{0}", ConfigurationManager.AppSettings.Get("TopicResponseQueueName"));

            IConnectionFactory factory = new ConnectionFactory(connecturl);
            using (IConnection connection = factory.CreateConnection())
            {
                using (ISession session = connection.CreateSession())
                {
                    IDestination destination = SessionUtil.GetDestination(session, ResponseQueueName);
                    IMessageProducer producer = session.CreateProducer(destination);
                    IMessageConsumer consumer = session.CreateConsumer(destination);
                    connection.Start();

                    while (ResponseListenerStatus)
                    {
                        var message = (ITextMessage)consumer.Receive(TimeSpan.FromMilliseconds(100));
                        if (message != null)
                        {
                            try
                            {
                                // 取得解壓縮字串
                                DecompressStr = DecodeStringZip(message.Text);
                                // 透過Json元件套用Class
                                argContents = JsonConvert.DeserializeObject<argAgentCommand>(DecompressStr);
                                switch (argContents.CommandType )
                                {
                                    case CommandRunType.File:
                                        ReturnMessage = string.Format("FileName [{0}\\{1}] Start[{2}] End[{3}] [{4}] Say Status [{5}] ",
                                    argContents.FileInfo.FilePath,
                                    argContents.FileInfo.FileName,
                                    argContents.SendTime.ToLongTimeString(),
                                    argContents.ReceiveTime.ToLongTimeString(),
                                      argContents.FileInfo.TopicListener,
                                      argContents.FileInfo.TransferStatus);
                                        AddMessage(ReturnMessage);
                                        Logger.Info(ReturnMessage);
                                        break;
                                    case CommandRunType.GetFile:
                                        // 將檔案寫入資料夾中
                                        WriteFileToBase64(argContents.FileInfo.FilePath , argContents.FileInfo.FileName, argContents.FileInfo.RepFileContent);
                                        Logger.Info(argContents.FileInfo.FilePath + @"\\" + argContents.FileInfo.FileName + " Finish!!");

                                        ReturnMessage = string.Format("FileName [{0}\\{1}] Start[{2}] End[{3}] [{4}] Say Status [{5}] ",
                                   argContents.FileInfo.FilePath,
                                   argContents.FileInfo.FileName,
                                   argContents.SendTime.ToLongTimeString(),
                                   argContents.ReceiveTime.ToLongTimeString(),
                                     argContents.FileInfo.TopicListener,
                                     argContents.FileInfo.TransferStatus);
                                        AddMessage(ReturnMessage);
                                        Logger.Info(ReturnMessage);
                                        break;
                                    case CommandRunType.Command:
                                    case CommandRunType.SummitCommand:

                                        AddMessage(DecompressStr);
                                        Logger.Info(DecompressStr);
                                        break;
                                }
                                
                            }
                            catch (Exception Ex)
                            {
                                Logger.Error(Ex.Message);
                            }
                        }
                        Thread.Sleep(100);
                    }
                }
            }
        }
        private void Consumer_Listener(IMessage message)
        {
            argSyncFile FileContents = new argSyncFile();
            FileInfo fi = null;
            string ReturnMessage = null;

            argSyncFile MessageContents = new argSyncFile();

            string DecompressStr = null;
            ITextMessage receivedMsg = message as ITextMessage;
            if (message != null)
            {
                try
                {
                    // 取得解壓縮字串
                    DecompressStr = DecodeStringZip(receivedMsg.Text);
                    // 透過Json元件套用Class
                    FileContents = JsonConvert.DeserializeObject<argSyncFile>(DecompressStr);
                    ReturnMessage = string.Format("{0}\\{1} Status={2}", FileContents.FilePath, FileContents.FileName, FileContents.TransferStatus);
                    AddMessage(ReturnMessage);
                    Logger.Info(ReturnMessage);
                }
                catch (Exception Ex)
                {
                    Logger.Error(Ex.Message);
                }

            }
        }
        #endregion

        void SendDataToQueue1(List<string> FileLists)
        {
            argAgentCommand argContents = new argAgentCommand();
            argContents.FileInfo = new argSyncFile();

            FileInfo fi = null;
            string strCompress = null;
            Uri connecturl = new Uri(string.Format("activemq:tcp://{0}:{1}", ConfigurationManager.AppSettings.Get("ActiveMQIP"),
                ConfigurationManager.AppSettings.Get("MQPort")));
            string RequestQueueName = string.Format("queue://{0}", ConfigurationManager.AppSettings.Get("RequestQueueName"));
            string ResponseQueueName = string.Format("queue://{0}", ConfigurationManager.AppSettings.Get("ResponseQueueName"));

            IConnectionFactory factory = new ConnectionFactory(connecturl);
            using (IConnection connection = factory.CreateConnection())
            {
                using (ISession session = connection.CreateSession())
                {
                    IDestination destination = SessionUtil.GetDestination(session, RequestQueueName);
                    using (IMessageProducer producer = session.CreateProducer(destination))
                    {
                        // Begin to Send file
                        foreach (string sendfile in FileLists)
                        {
                            fi = new FileInfo(sendfile);
                            argContents.FileInfo.FilePath = txtTargetPath.Text;
                            argContents.FileInfo.FileName = fi.Name;
                            argContents.FileInfo.FileContent = ReadFileToBase64(sendfile);
                            // Compress String
                            strCompress = EncodeStringZip(JsonConvert.SerializeObject(argContents));
                            // Create Active MQ Message
                            ITextMessage request = session.CreateTextMessage(strCompress);

                            // Send To MQ
                            producer.Send(request);
                        }
                    }
                }
            }
        }

        private void btnSendFileToTopic_Click(object sender, EventArgs e)
        {
            //Open wait form dialog
            using (WaitForm frm = new WaitForm(SendDataToTopic))
            {
                frm.ShowDialog(this);
            }
        }

        void SendDataToTopic()
        {
            argAgentCommand argContents = new argAgentCommand();
            argContents.CommandType = CommandRunType.File;
            argContents.FileInfo = new argSyncFile();

            FileInfo fi = null;
            string strCompress = null;
            Uri connecturl = new Uri(string.Format("activemq:tcp://{0}:{1}", ConfigurationManager.AppSettings.Get("ActiveMQIP"), ConfigurationManager.AppSettings.Get("MQPort")));
            string QueueName = string.Format("topic://{0}", ConfigurationManager.AppSettings.Get("TopicName"));
            IConnectionFactory factory = new ConnectionFactory(connecturl);
            using (IConnection connection = factory.CreateConnection())
            {
                using (ISession session = connection.CreateSession())
                {
                    IDestination destination = SessionUtil.GetDestination(session, QueueName);
                    using (IMessageProducer producer = session.CreateProducer(destination))
                    {
                        // Begin to Send file
                        foreach (string sendfile in listBoxFileList.Items)
                        {
                            fi = new FileInfo(sendfile);
                            argContents.FileInfo.FilePath = txtTargetPath.Text;
                            argContents.FileInfo.FileName = fi.Name;
                            argContents.FileInfo.FileContent = ReadFileToBase64(sendfile);
                            argContents.SendTime = DateTime.Now;
                            // Compress String
                            strCompress = EncodeStringZip(JsonConvert.SerializeObject(argContents));
                            // Create Active MQ Message
                            ITextMessage request = session.CreateTextMessage(strCompress);
                            // Send To MQ
                            producer.Send(request);
                        }

                    }
                }
            }

        }

        private void btnCommand_Click(object sender, EventArgs e)
        {
            argAgentCommand argContents = new argAgentCommand();
            argContents.CommandType = CommandRunType.Command;
            argContents.CmdInfo = new argRunCommand();

           
            string strCompress = null;
            Uri connecturl = new Uri(string.Format("activemq:tcp://{0}:{1}", ConfigurationManager.AppSettings.Get("ActiveMQIP"), ConfigurationManager.AppSettings.Get("MQPort")));
            string QueueName = string.Format("topic://{0}", ConfigurationManager.AppSettings.Get("TopicName"));
            IConnectionFactory factory = new ConnectionFactory(connecturl);
            using (IConnection connection = factory.CreateConnection())
            {
                using (ISession session = connection.CreateSession())
                {
                    IDestination destination = SessionUtil.GetDestination(session, QueueName);
                    using (IMessageProducer producer = session.CreateProducer(destination))
                    {
                        argContents.CmdInfo.Cmd = txtCommand.Text;
                        argContents.CmdInfo.RunPath = txtRunPath.Text;
                        argContents.CmdInfo.Args = txtArgs.Text;
                        argContents.SendTime = DateTime.Now;

                        // Compress String
                        strCompress = EncodeStringZip(JsonConvert.SerializeObject(argContents));
                        // Create Active MQ Message
                        ITextMessage request = session.CreateTextMessage(strCompress);
                        // Send To MQ
                        producer.Send(request);
                    }
                }
            }
        }

        private void btnSummitCommand_Click(object sender, EventArgs e)
        {
            argAgentCommand argContents = new argAgentCommand();
            argContents.CommandType = CommandRunType.SummitCommand;
            argContents.CmdInfo = new argRunCommand();


            string strCompress = null;
            Uri connecturl = new Uri(string.Format("activemq:tcp://{0}:{1}", ConfigurationManager.AppSettings.Get("ActiveMQIP"), ConfigurationManager.AppSettings.Get("MQPort")));
            string QueueName = string.Format("topic://{0}", ConfigurationManager.AppSettings.Get("TopicName"));
            IConnectionFactory factory = new ConnectionFactory(connecturl);
            using (IConnection connection = factory.CreateConnection())
            {
                using (ISession session = connection.CreateSession())
                {
                    IDestination destination = SessionUtil.GetDestination(session, QueueName);
                    using (IMessageProducer producer = session.CreateProducer(destination))
                    {
                        argContents.CmdInfo.Cmd = txtCommand.Text;
                        argContents.CmdInfo.RunPath = txtRunPath.Text;
                        argContents.CmdInfo.Args = txtArgs.Text;
                        argContents.SendTime = DateTime.Now;

                        // Compress String
                        strCompress = EncodeStringZip(JsonConvert.SerializeObject(argContents));
                        // Create Active MQ Message
                        ITextMessage request = session.CreateTextMessage(strCompress);
                        // Send To MQ
                        producer.Send(request);
                    }
                }
            }
        }

        #region 取得已上傳的檔案
        private void btnGetFileFromTopic_Click(object sender, EventArgs e)
        {
            using (WaitForm frm = new WaitForm(GetDataFromTopic))
            {
                frm.ShowDialog(this);
            }
        }

        void GetDataFromTopic()
        {
            argAgentCommand argContents = new argAgentCommand();
            argContents.CommandType = CommandRunType.GetFile;
            argContents.FileInfo = new argSyncFile();

            FileInfo fi = null;
            string strCompress = null;
            Uri connecturl = new Uri(string.Format("activemq:tcp://{0}:{1}", ConfigurationManager.AppSettings.Get("ActiveMQIP"), ConfigurationManager.AppSettings.Get("MQPort")));
            string QueueName = string.Format("topic://{0}", ConfigurationManager.AppSettings.Get("TopicName"));
            IConnectionFactory factory = new ConnectionFactory(connecturl);
            using (IConnection connection = factory.CreateConnection())
            {
                using (ISession session = connection.CreateSession())
                {
                    IDestination destination = SessionUtil.GetDestination(session, QueueName);
                    using (IMessageProducer producer = session.CreateProducer(destination))
                    {
                        // Begin to GEt file
                            argContents.FileInfo.FilePath = txtTargetPath.Text;
                            argContents.FileInfo.FileName = txtFileName.Text;
                            //argContents.FileInfo.FileContent = ReadFileToBase64(sendfile);
                            argContents.SendTime = DateTime.Now;
                            // Compress String
                            strCompress = EncodeStringZip(JsonConvert.SerializeObject(argContents));
                            // Create Active MQ Message
                            ITextMessage request = session.CreateTextMessage(strCompress);
                            // Send To MQ
                            producer.Send(request);
                    }
                }
            }

        }
        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResponseListenerStatus = false;
            ListenQueueResponseThread.Abort();
            ListenTopicResponseThread.Abort();
        }
    }
}
