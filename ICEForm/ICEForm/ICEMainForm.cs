using CfApiNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ICEForm
{

    public partial class ICEMainForm : Form
    {
        XElement XMLFile = new XElement("ICEMarketData");

        protected log4net.ILog Entrylog = log4net.LogManager.GetLogger("ICELog");
        public const string TimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        public static StreamWriter LogFile;

        public static ProgData ProgramData;

        public static Dictionary<string, int> CommandsToken;

        public static ConcurrentDictionary<long, string> RequestCommands;

        public static SessionEvent CurrentSession;

        MyUserEventHandler myUserEventHandler = null;
        MySessionEventHandler mySessionEventHandler = null;
        MyMessageEventHandler myMessageEventHandler = null;
        MyStatisticsEventHandler myStatisticsEventHandler = null;
        Session Currentsession = null;
        UserInfo userInfo = null;

        //SessionEvent sessionEvent;
        bool SessionStatus;


        public ICEMainForm()
        {
            InitializeComponent();
            Init();
        }

        private void ICEMainForm_Load(object sender, EventArgs e)
        {

            ProgramData.mDebug = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("mDebug"));
            Console.WriteLine(ProgramData.mDebug);
            ProgramData.mUsername = ConfigurationManager.AppSettings.Get("mUsername");
            Console.WriteLine(ProgramData.mUsername);
            ProgramData.mPassword = ConfigurationManager.AppSettings.Get("mPassword");
            Console.WriteLine(ProgramData.mPassword);
            // Request
            ProgramData.mFileName = ConfigurationManager.AppSettings.Get("mFileName");
            Console.WriteLine(ProgramData.mFileName);
            ProgramData.outFile = new StreamWriter(ConfigurationManager.AppSettings.Get("outFile"));
            Console.WriteLine(ProgramData.outFile);


            ProgramData.mMultithread = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("mMultithread"));
            Console.WriteLine(ProgramData.mMultithread);


            ProgramData.mConflationIndicator = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("mConflationIndicator"));
            Console.WriteLine(ProgramData.mConflationIndicator);


            //ProgramData.mNatFile = ConfigurationManager.AppSettings.Get("mNatFile");
            //Console.WriteLine(ProgramData.mNatFile);



            ProgramData.mConflInterval = Convert.ToInt32(ConfigurationManager.AppSettings.Get("mConflInterval"));
            Console.WriteLine(ProgramData.mConflInterval);
            ProgramData.mConflType = Convert.ToInt32(ConfigurationManager.AppSettings.Get("mConflType"));
            Console.WriteLine(ProgramData.mConflType);
            ProgramData.mReadTimeout = Convert.ToInt32(ConfigurationManager.AppSettings.Get("mReadTimeout"));
            Console.WriteLine(ProgramData.mReadTimeout);




            ProgramData.mStatsInterval = Convert.ToInt32(ConfigurationManager.AppSettings.Get("mStatsInterval"));
            Console.WriteLine(ProgramData.mStatsInterval);
            ProgramData.mQueueSize = Convert.ToInt32(ConfigurationManager.AppSettings.Get("mQueueSize"));
            Console.WriteLine(ProgramData.mQueueSize);



            ProgramData.mPrimaryConn.Add(ConfigurationManager.AppSettings.Get("mPrimaryConn1"));

            //ProgramData.mPrimaryConn.Add(ConfigurationManager.AppSettings.Get("mPrimaryConn2"));
            Console.WriteLine(ProgramData.mPrimaryConn.Count);
            ProgramData.mCompressedData = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("mCompressedData"));
            Console.WriteLine(ProgramData.mCompressedData);


        }
        void ParseNatFile(ConnectionConfig connectionConfig, string filename)
        {//Assume format of each line of file is: <CSP IP>,<local IP>
            try
            {
                var lines = File.ReadAllLines(filename);
                foreach (var line in lines)
                {
                    if (String.IsNullOrWhiteSpace(line))
                        continue;
                    var lineData = line.Split(',');
                    var ipCsp = lineData[0];
                    var ipClient = lineData[1];

                    connectionConfig.AddNATPair(ipCsp, ipClient);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: Parsing NatFile", ex);
            }
        }
        void Init()
        {
            CommandsToken = new Dictionary<string, int>
            {
                {"COMMAND", 0},
                {"ENUM_SRC_ID", 1},
                {"SYMBOL_TICKER", 2},
                {"CUSIP", 3},
                {"SEDOL", 4},
                {"ISIN", 5},
                {"ENUM_SRC_UNDERLYING_ID", 6},
                {"SYMBOL_UNDERLYING_TICKER", 7},
                {"CONFLATION", 8},
                {"CTF_TOKEN_NUM", 9},
                {"SYMBOL_BLOOMBERG_TICKER", 10},
                {"PRODUCT_ROOT", 11},
                {"CTF_TOKEN_NAME", 12},
                {"SYMBOL_ESIGNAL_TICKER", 13},
                { "DEPTH_TYPE", 14},
                { "CONFLATION_INTERVAL", 15}
            };
            RequestCommands = new ConcurrentDictionary<long, string>();
           

            ProgramData = new ProgData();
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Start: " + System.DateTime.Now);

            if (string.IsNullOrEmpty(ProgramData.mUsername))
            {
                Console.Write("Enter userid: ");
                var line = Console.ReadLine();
                ProgramData.mUsername = line;
            }

            if (string.IsNullOrEmpty(ProgramData.mPassword))
            {
                Console.Write("Enter password: ");
                var line = Console.ReadLine();
                ProgramData.mPassword = line;
            }

            Entrylog.Info("Start: " + System.DateTime.Now);

            ApiFactory.Instance.Initialize("CfApiTest", "1.0.0", ProgramData.mDebug, "cfapilog", "DotNet");

            myUserEventHandler = new MyUserEventHandler();
            myUserEventHandler.ProgramData = ProgramData;
            myUserEventHandler.FEIBSetUserMessage += MyUserEventHandler_FEIBSetUserMessage;
            userInfo = ApiFactory.Instance.CreateUserInfo(ProgramData.mUsername, ProgramData.mPassword, myUserEventHandler);

            mySessionEventHandler = new MySessionEventHandler();

            mySessionEventHandler.FEIBSetSession += MySessionEventHandler_FEIBSetSession;
            mySessionEventHandler.FEIBSetSessionMessage += MySessionEventHandler_FEIBSetSessionMessage;
            Currentsession = ApiFactory.Instance.CreateSession(userInfo, mySessionEventHandler);

            if (ProgramData.mDebug)
                PrintAuthStatus(userInfo);

            if (ProgramData.mMultithread) //for Multithread
            {
                var sessionConfig = Currentsession.GetSessionConfig();
                sessionConfig.Set(SessionConfig.Parameters.MULTITHREADED_API_CONNECTIONS_BOOL, true);
                if (ProgramData.mMaxUserThreads > 0)
                    sessionConfig.Set(SessionConfig.Parameters.MAX_USER_THREADS_LONG, ProgramData.mMaxUserThreads);
                if (ProgramData.mMaxCspThreads > 0)
                    sessionConfig.Set(SessionConfig.Parameters.MAX_CSP_THREADS_LONG, ProgramData.mMaxCspThreads);
            }



            var connectionConfig = Currentsession.GetConnectionConfig();
            connectionConfig.Set(HostConfig.Parameters.CONFLATION_INDICATOR_BOOL, ProgramData.mConflationIndicator);
            //connectionConfig.Set(HostConfig.Parameters.CONNECTION_TIMEOUT_LONG, 10);
            //connectionConfig.Set(HostConfig.Parameters.CONNECTION_RETRY_LIMIT_LONG, 1);



            //if (!string.IsNullOrWhiteSpace(ProgramData.mNatFile))
            //ParseNatFile(connectionConfig, ProgramData.mNatFile);


            if (ProgramData.mConflInterval != -1)
                connectionConfig.Set(HostConfig.Parameters.CONFLATION_INTERVAL_LONG, ProgramData.mConflInterval);
            if (ProgramData.mConflType > -1)
                connectionConfig.Set(HostConfig.Parameters.CONFLATION_TYPE_LONG, ProgramData.mConflType);
            if (ProgramData.mReadTimeout > -1)
                connectionConfig.Set(HostConfig.Parameters.READ_TIMEOUT_LONG, ProgramData.mReadTimeout);

            myStatisticsEventHandler = new MyStatisticsEventHandler();

            if (ProgramData.mStatsInterval > 0)
                Currentsession.RegisterStatisticsEventHandler(myStatisticsEventHandler, ProgramData.mStatsInterval);

            if (ProgramData.mQueueSize != -1)
                connectionConfig.Set(HostConfig.Parameters.QUEUE_SIZE_LONG, ProgramData.mQueueSize);

            //CEF changes
            if (ProgramData.mPrimaryConn.Count != 0)
            {
                foreach (string address in ProgramData.mPrimaryConn)
                {
                    var hostConfig = connectionConfig.GetHostConfig(address);
                    hostConfig.Set(HostConfig.Parameters.BACKUP_BOOL, false);
                    hostConfig.Set(HostConfig.Parameters.COMPRESSION_BOOL, ProgramData.mCompressedData);
                }
            }
            else // Primary conn not specified
            {
                Console.Write(" Error - Primary connection not specified \n");
                return;
            }


            //var address = string.Format("{0}:{1}", ProgramData.mRemoteHost, ProgramData.mRemotePort);
            //var hostConfig = connectionConfig.GetHostConfig(address);
            //hostConfig.Set(HostConfig.Parameters.BACKUP_BOOL, false);
            //hostConfig.Set(HostConfig.Parameters.COMPRESSION_BOOL, ProgramData.mCompressedData);

            //Setup failover
            if (ProgramData.mBackupConn.Count != 0)
            {
                if (ProgramData.mBackupConn.Count != ProgramData.mPrimaryConn.Count)
                {
                    Console.WriteLine("Error - The no of backup connections should be same as the primary connections\n");
                    return;
                }
                foreach (string address in ProgramData.mBackupConn)
                {
                    var hostConfig = connectionConfig.GetHostConfig(address);
                    hostConfig.Set(HostConfig.Parameters.BACKUP_BOOL, true);
                    hostConfig.Set(HostConfig.Parameters.COMPRESSION_BOOL, ProgramData.mCompressedData);
                }
            }


            myMessageEventHandler = new MyMessageEventHandler();
            myMessageEventHandler.RequestCommands = RequestCommands;
            myMessageEventHandler.ProgramData = ProgramData;
            myMessageEventHandler.FEIBGetMessage += MyMessageEventHandler_FEIBGetMessage;
            myMessageEventHandler.FEIBGetMarketData += MyMessageEventHandler_FEIBGetMarketData;
            //register to get called when messages received
            Currentsession.RegisterMessageEventHandler(myMessageEventHandler);

            // Session Connect Start
            ThreadStart threadDelegate = new ThreadStart(Start);
            Thread newThread = new Thread(threadDelegate);
            newThread.Start();


        }

        private void MyUserEventHandler_FEIBSetUserMessage(string Msg)
        {
            ShowResult(String.Format("UserHandler Msg = {0}", Msg));
        }

        void Start()
        {
            string failReason = null;
            var isSessionStart = Currentsession.Start(ref failReason);
            if (!isSessionStart)
            {
                ShowResult("session could not be established: " + failReason);
            }
            //Console.WriteLine("Got session took: {0} , time {1}", sw.Elapsed, System.DateTime.Now);
        }


        private void MyMessageEventHandler_FEIBGetMarketData(List<MarketDataItem> msg)
        {
            // 取得項目
            string Ticket = msg.Where(P => P.ItemName.Equals("SYMBOL.TICKER")).First().Value;
            string SourceID = msg.Where(P => P.ItemName.Equals("ENUM.SRC.ID")).First().Value;
            // 檢查項目是否存在XML
            var CheckSourceID = XMLFile.Elements().Where(P => P.Attribute("SYMBOL.TICKER").Value.Equals(Ticket) && P.Attribute("ENUM.SRC.ID").Value.Equals(SourceID));

            if (CheckSourceID.Count() == 0)
            {
                XMLFile.Add(new XElement("Item", new XAttribute("SYMBOL.TICKER", Ticket), new XAttribute("ENUM.SRC.ID", SourceID)));
            }

            CheckSourceID = XMLFile.Elements().Where(P => P.Attribute("SYMBOL.TICKER").Value.Equals(Ticket) && P.Attribute("ENUM.SRC.ID").Value.Equals(SourceID));

            // 處理資料
            foreach(MarketDataItem ItemData in msg.Where(Q => Q.ItemType.Equals(1)))
            {
                var CheckItemID = CheckSourceID.Elements().Where(Q => Q.Name.LocalName.Equals(ItemData.ItemName));

                if (CheckItemID.Count() > 0)
                {
                    CheckItemID.First().Value = ItemData.Value;
                }
                else
                {
                    CheckSourceID.First().Add(new XElement(ItemData.ItemName, ItemData.Value, new XAttribute("Code", ItemData.Code)));
                }
            }
           
            
            XMLFile.Save("C:\\temp\\ICEMarket.xml");
        }

       
        private void MySessionEventHandler_FEIBSetSessionMessage(string Msg)
        {
            ShowResult(String.Format("SessionHandler Msg = {0}", Msg));
        }

        private void MyMessageEventHandler_FEIBGetMessage(string msg)
        {
            ShowResult(String.Format("EventHandler Msg = {0}", msg));
        }

        private delegate void ShowMessage(string sMessage);
        private void ShowResult(string Message)
        {
            if (this.InvokeRequired) // 若非同執行緒
            {
                //利用委派執行
                ShowMessage show = new ShowMessage(ShowResult);
                this.Invoke(show, Message);
            }
            else // 同執行緒
            {
                this.txtResult.Text = System.DateTime.Now.ToString(TimeFormat) + ":" + Message + Environment.NewLine + this.txtResult.Text;
            }
        }

        private void MySessionEventHandler_FEIBSetSession(bool status, SessionEvent sevent)
        {
            if (status)
            {
                ShowResult(String.Format("Session Status = {0} , Session ID = {1}", status, sevent.GetSourceID()));
                SessionStatus = status;
                Currentsession = sevent.GetSession();
            }
        }

        public void SendCommandStart(Session session)
        {
            try
            {
                if (ProgramData.mDebug)
                    PrintAuthStatus(session.GetUserInfo());

                string[] lines;
                try
                {
                    lines = File.ReadAllLines(ProgramData.mFileName);
                }
                catch (Exception ex)
                {
                    Entrylog.Info(string.Format("error occurred on sending command, reading from file {0} Exception: " + ex.Message, ProgramData.mFileName));
                    return;
                }

                var request = session.CreateRequest();

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

                    request.ClearRequest();

                    var badMsg = SetCommandStart(line, request);
                    if (badMsg)
                        Entrylog.Info("Invalid message not sent command: " + line);
                    else//API send request -- if ok
                    {
                        long reqHandle = request.GenerateTag();
                        RequestCommands.TryAdd(reqHandle, line);
                        reqHandle = session.Send(request);
                        if (reqHandle == 0)
                            Entrylog.Info("Error: send queue is full, cannot send command: " + line);
                        else
                            Entrylog.Info(string.Format("Send command:[{0}]: tag =[{1}]: ", line, reqHandle));
                    }
                }

                session.FreeRequest(request);
            }
            catch (Exception ex)
            {
                Entrylog.Info("Send command Exception: " + ex.Message);
            }
        }

        public bool SetCommandStart(string line, Request request)
        {
            bool badMsg = false;

            IEnumerable<string> paramters = GetParamters(line, out badMsg);

            foreach (var paramter in paramters)
            {
                var parmValues = paramter.Split('=');
                var toknum = CommandsToken.ContainsKey(parmValues[0]) ? CommandsToken[parmValues[0]] : -1;
                switch (toknum)
                {
                    case 0://command
                        switch (parmValues[1].ToUpper())
                        {
                            case "SUBSCRIBE":
                                request.SetCommand(Commands.SUBSCRIBE);
                                break;
                            case "UNSUBSCRIBE":
                                request.SetCommand(Commands.UNSUBSCRIBE);
                                break;
                            case "QUERYDEPTH":
                                request.SetCommand(Commands.QUERYDEPTH);
                                break;
                            case "QUERYSNAP":
                                request.SetCommand(Commands.QUERYSNAP);
                                break;
                            case "QUERYWILDCARD":
                                request.SetCommand(Commands.QUERYWILDCARD);
                                break;
                            case "QUERYDEPTHANDSUBSCRIBE":
                                request.SetCommand(Commands.QUERYDEPTHANDSUBSCRIBE);
                                break;
                            case "QUERYSNAPANDSUBSCRIBE":
                                request.SetCommand(Commands.QUERYSNAPANDSUBSCRIBE);
                                break;
                            case "LISTENUMERATION":
                                request.SetCommand(Commands.LISTENUMERATION);
                                break;
                            case "LISTAVAILABLETOKENS":
                                request.SetCommand(Commands.LISTAVAILABLETOKENS);
                                break;
                            case "LISTADMINISTRATIONINFO":
                                request.SetCommand(Commands.LISTADMINISTRATIONINFO);
                                break;
                            case "LISTEXTENDEDEXCHANGEINFO":
                                request.SetCommand(Commands.LISTEXTENDEDEXCHANGEINFO);
                                break;
                            case "SELECTUSERFILTERTOKENS":
                                request.SetCommand(Commands.SELECTUSERFILTERTOKENS);
                                break;
                            case "LISTSUBSCRIBEDSYMBOLS":
                                request.SetCommand(Commands.LISTSUBSCRIBEDSYMBOLS);
                                break;
                            case "QUERYXREF":
                                request.SetCommand(Commands.QUERYXREF);
                                break;
                            case "LISTUSERPERMISSION":
                                request.SetCommand(Commands.LISTUSERPERMISSION);
                                break;
                            case "SETCONFLATIONINTERVAL":
                                request.SetCommand(Commands.SETCONFLATIONINTERVAL);
                                break;
                            case "SUBSCRIBEWILDCARD":
                                request.SetCommand(Commands.SUBSCRIBEWILDCARD);
                                break;
                            case "QUERYSNAPANDSUBSCRIBEWILDCARD":
                                request.SetCommand(Commands.QUERYSNAPANDSUBSCRIBEWILDCARD);
                                break;
                            case "UNSUBSCRIBEWILDCARD":
                                request.SetCommand(Commands.UNSUBSCRIBEWILDCARD);
                                break;
                            default:
                                Entrylog.Info("Invalid command: " + parmValues[1]);
                                badMsg = true;
                                break;
                        }
                        break;
                    case 1:
                        request.Add(RequestParameters.ENUM_SRC_ID, parmValues[1]);
                        break;
                    case 2:
                        request.Add(RequestParameters.SYMBOL_TICKER, parmValues[1]);
                        break;
                    case 3:
                        request.Add(RequestParameters.CUSIP, parmValues[1]);
                        break;
                    case 4:
                        request.Add(RequestParameters.SEDOL, parmValues[1]);
                        break;
                    case 5:
                        request.Add(RequestParameters.ISIN, parmValues[1]);
                        break;
                    case 6:
                        request.Add(RequestParameters.ENUM_SRC_UNDERLYING_ID, parmValues[1]);
                        break;
                    case 7:
                        request.Add(RequestParameters.SYMBOL_UNDERLYING_TICKER, parmValues[1]);
                        break;
                    case 8:
                        request.Add(RequestParameters.CONFLATION, parmValues[1]);
                        break;
                    case 9:
                        request.Add(RequestParameters.CTF_TOKEN_NUM, parmValues[1]);
                        break;
                    case 10:
                        request.Add(RequestParameters.SYMBOL_BLOOMBERG_TICKER, parmValues[1]);
                        break;
                    case 11:
                        request.Add(RequestParameters.PRODUCT_ROOT, parmValues[1]);
                        break;
                    case 12:
                        request.Add(RequestParameters.CTF_TOKEN_NAME, parmValues[1]);
                        break;
                    case 13:
                        request.Add(RequestParameters.SYMBOL_ESIGNAL_TICKER, parmValues[1]);
                        break;
                    case 14:
                        request.Add(RequestParameters.DEPTH_TYPE, parmValues[1]);
                        break;
                    case 15:
                        request.Add(RequestParameters.CONFLATION_INTERVAL, parmValues[1]);
                        break;
                    case -1:    //Allow using specific token number
                        int tokenNumber;
                        if (!int.TryParse(parmValues[0], out tokenNumber))
                        {
                            badMsg = true;
                            Entrylog.Info("Invalid token in the request: " + parmValues[0]);
                        }
                        else
                            request.Add(tokenNumber, parmValues[1]);
                        break;
                    default:
                        Entrylog.Info("Invalid token in the request: " + parmValues[0]);
                        badMsg = true;
                        break;
                }
            }
            return badMsg;
        }


        public void SendCommandStop(Session session)
        {
            try
            {
                if (ProgramData.mDebug)
                    PrintAuthStatus(session.GetUserInfo());

                string[] lines;
                try
                {
                    lines = File.ReadAllLines(ProgramData.mFileName);
                }
                catch (Exception ex)
                {
                    Entrylog.Info(string.Format("error occurred on sending command, reading from file {0} Exception: " + ex.Message, ProgramData.mFileName));
                    return;
                }

                var request = session.CreateRequest();

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

                    request.ClearRequest();

                    var badMsg = SendCommandStop(line, request);
                    if (badMsg)
                        Entrylog.Info("Invalid message not sent command: " + line);
                    else//API send request -- if ok
                    {
                        long reqHandle = request.GenerateTag();
                        RequestCommands.TryAdd(reqHandle, line);
                        reqHandle = session.Send(request);
                        if (reqHandle == 0)
                            Entrylog.Info("Error: send queue is full, cannot send command: " + line);
                        else
                            Entrylog.Info(string.Format("Send command:[{0}]: tag =[{1}]: ", line, reqHandle));
                    }
                }

                session.FreeRequest(request);
            }
            catch (Exception ex)
            {
                Entrylog.Info("Send command Exception: " + ex.Message);
            }
        }

        public bool SendCommandStop(string line, Request request)
        {
            bool badMsg = false;

            IEnumerable<string> paramters = GetParamters(line, out badMsg);

            foreach (var paramter in paramters)
            {
                var parmValues = paramter.Split('=');
                var toknum = CommandsToken.ContainsKey(parmValues[0]) ? CommandsToken[parmValues[0]] : -1;
                switch (toknum)
                {
                    case 0://command
                        switch (parmValues[1].ToUpper())
                        {
                            case "UNSUBSCRIBE":
                            case "SUBSCRIBE":
                            case "QUERYSNAPANDSUBSCRIBE":
                            case "QUERYDEPTHANDSUBSCRIBE":
                                request.SetCommand(Commands.UNSUBSCRIBE);
                                Console.WriteLine("UNSUBSCRIBE");
                                break;

                            case "QUERYSNAPANDSUBSCRIBEWILDCARD":
                            case "SUBSCRIBEWILDCARD":
                            case "UNSUBSCRIBEWILDCARD":
                                request.SetCommand(Commands.UNSUBSCRIBEWILDCARD);
                                Console.WriteLine("UNSUBSCRIBEWILDCARD");
                                break;

                            default:
                                Entrylog.Info("Invalid command: " + parmValues[1]);
                                badMsg = true;
                                break;
                        }
                        break;
                }
            }
            return badMsg;
        }

        public IEnumerable<string> GetParamters(string line, out bool badMsg)
        {
            badMsg = false;
            var paramters = new List<KeyValuePair<string, string>>();
            var index = line.IndexOf("=", StringComparison.Ordinal);
            if (index == -1)
            {
                Console.WriteLine("Missing '=': " + line);
                badMsg = true;
                return new List<string>();
            }
            var remainLine = line;
            while (index != -1)
            {
                var cmd = remainLine.Substring(0, index);
                remainLine = remainLine.Substring(index + 1);
                int indexValue;
                string qouteValue;
                if (remainLine[0] == '\"')
                {
                    indexValue = remainLine.IndexOf("\"", 1, StringComparison.Ordinal);
                    if (indexValue == -1)
                    {
                        Console.WriteLine("Missing end-quote: " + line);
                        badMsg = true;
                        return new List<string>();
                    }
                    qouteValue = remainLine.Substring(1, indexValue - 1);
                    indexValue += 2;
                }
                else
                {
                    indexValue = remainLine.IndexOfAny(new char[] { ' ', '\t' });

                    if (indexValue == -1)
                    {
                        qouteValue = remainLine.Substring(0);
                        indexValue = qouteValue.Length;
                    }
                    else
                    {
                        qouteValue = remainLine.Substring(0, indexValue);
                        indexValue += 1;
                    }
                }
                paramters.Add(new KeyValuePair<string, string>(cmd, qouteValue));

                if (remainLine.Length < indexValue)
                    break;

                remainLine = remainLine.Substring(indexValue).TrimStart();

                index = remainLine.IndexOf("=", StringComparison.Ordinal);
            }
            var paramtersStr = paramters.Select(o => o.Key + "=" + o.Value);
            return paramtersStr;
        }

        void PrintAuthStatus(UserInfo user)
        {
            var state = user.GetState();
            string Msg = null;
            switch (state)
            {
                case UserStates.NOT_AUTHENTICATED:
                    Msg = "Authorization status = NOT_AUTHENTICATED";
                    break;
                case UserStates.AUTHENTICATED:
                    Msg = "Authorization status = AUTHENTICATED" ;
                    break;
                case UserStates.PARTIALLY_AUTHENTICATED:
                    Msg = "Authorization status = PARTIALLY_AUTHENTICATED" ;
                    break;
            }
            ShowResult(Msg);
            Entrylog.Info(Msg);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (SessionStatus)
            {
                Task.Factory.StartNew(() => SendCommandStart(Currentsession));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

          
            Currentsession.Stop();
            ApiFactory.Instance.destroySession(Currentsession);
            ApiFactory.Instance.destroyUserInfo(userInfo);
            ApiFactory.Instance.Uninitialize();
        }

        private void btnUnSubscribe_Click(object sender, EventArgs e)
        {

            if (SessionStatus)
            {
                Task.Factory.StartNew(() => SendCommandStop(Currentsession));
            }
        }
    }

}

  

 
