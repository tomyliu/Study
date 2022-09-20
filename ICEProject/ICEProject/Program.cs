using CfApiNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ICEProject
{
    class ProgData
    {
        public bool mQuiet;
        public bool mDebug;
        public bool mMultithread;
        public string mRemoteHost;
        public int mRemotePort;
        public string mRemoteHost2;
        public int mRemotePort2;
        public string mFileName;
        public bool mCompressedData;
        public int mMaxUserThreads;
        public int mMaxCspThreads;
        public string mUsername;
        public string mPassword;
        public int mReadTimeout;
        public int mStatsInterval;
        public bool mConflationIndicator;
        public StreamWriter outFile;
        public int mQueueSize;
        public int mConflInterval;
        public string mNatFile;
        public List<string> mPrimaryConn;
        public List<string> mBackupConn;
        public int mConflType;

        public ProgData()
        {
            mQuiet = false;
            mDebug = false;
            mMultithread = false;
            mMaxUserThreads = -1; //invalid value
            mMaxCspThreads = -1;  //invalid value
            mCompressedData = true;
            mReadTimeout = -1;	//invalid value 
            mStatsInterval = -1; //invalid value 
            mConflationIndicator = false;
            mQueueSize = -1;
            mConflInterval = -1;
            mPrimaryConn = new List<string>();
            mBackupConn = new List<string>();
            mConflType = -1;
        }
    }

    internal class Program
    {

        public static StreamWriter LogFile;

        public static ProgData ProgramData;

        private static Dictionary<string, int> CommandsToken;

        public static ConcurrentDictionary<long, string> RequestCommands;

        public static ActionBlock<string> LogWriter;

        public const string TimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        private static void PrintHelp()
        {
            Console.WriteLine("Usage: CfApiTest.exe [-h] [-q] [-D] [-o output file] [-u userid] [-p password] [-M] [-U max_user_threads] [-C max_csp_threads] [-z] [-r read timeout] [-s interval] [-c] [-Q queue_size] [-a conflation_interval] [-n NAT_file] [-t conflation_type] -P remote_host_ip_address remote_port [-B backup_host_ip_address backup_port] input_command_file");
            Console.WriteLine("-h:             Prints this help message");
            Console.WriteLine("-q:             Quiet mode - don't print data");
            Console.WriteLine("-D:             Debug mode - print API debug messages to stderr");
            Console.WriteLine("-o:             Output file - write data here instead of stdout");
            Console.WriteLine("-u:             userid for CSP");
            Console.WriteLine("-p:             password for CSP");
            Console.WriteLine("-M:             Use Multithreading in API");
            Console.WriteLine("-U:             Max user-side threads API may create when in Multithreading mode");
            Console.WriteLine("-C:             Max CSP-side threads API may create when in Multithreading mode");
            Console.WriteLine("-z:             Turn off compression between API and CSP");
            Console.WriteLine("-r:             read timeout in seconds");
            Console.WriteLine("-s:             Get API statistics; specify interval in seconds");
            Console.WriteLine("-c:             Request conflation indicator");
            Console.WriteLine("-Q:             queue size in megabytes [Default 1 MB]");
            Console.WriteLine("-a:             Conflation interval in milliseconds. Default is 1000 (ms)");
            Console.WriteLine("-n:             NAT IP address mapping file (each line should contain <CSP IP addr>,<local IP addr>)");
            Console.WriteLine("-t:             Conflation type. 1=Trade Safe(default); 2=Intervalized");
            Console.WriteLine("-P:             Primary connection  with <IP addr><Port no>\n");
            Console.WriteLine("-B:             Backup connection  with <IP addr><Port no>\n");

        }

        private static int ReadCommandLine(string[] args)
        {
            HashSet<int> primaryPorts = new HashSet<int>();
            HashSet<int> backupPorts = new HashSet<int>();

            if (args.Length < 4)
            {
                PrintHelp();
                return 0;
            }

            var option = 0;
            for (int i = 0; i < args.Length - 1; ++i)
            {
                string arg = args[i];

                if (arg == "-h")
                {

                    PrintHelp();
                    return (0);
                }

                if (arg == "-q")
                {
                    ProgramData.mQuiet = true;
                    option++;
                }

                if (arg == "-D")
                {
                    ProgramData.mDebug = true;
                    option++;
                }

                if (arg == "-o")
                {
                    ProgramData.outFile = new StreamWriter(args[++i]);
                    option += 2;
                }

                if (arg == "-M")
                {
                    ProgramData.mMultithread = true;
                    option++;
                }

                if (arg == "-U")
                {
                    ProgramData.mMaxUserThreads = int.Parse(args[++i]);
                    option += 2;
                }

                if (arg == "-C")
                {
                    ProgramData.mMaxCspThreads = int.Parse(args[++i]);
                    option += 2;
                }

                if (arg == "-z")
                {
                    ProgramData.mCompressedData = false;
                    option++;
                }

                if (arg == "-u")
                {
                    ProgramData.mUsername = args[++i];
                    option += 2;
                }

                if (arg == "-p")
                {
                    ProgramData.mPassword = args[++i];
                    option += 2;
                }

                if (arg == "-r")
                {
                    ProgramData.mReadTimeout = int.Parse(args[++i]);
                    option += 2;
                }

                if (arg == "-s")
                {
                    ProgramData.mStatsInterval = int.Parse(args[++i]);
                    option += 2;
                }

                if (arg == "-c")
                {
                    ProgramData.mConflationIndicator = true;
                    option++;
                }
                if (arg == "-Q")
                {
                    ProgramData.mQueueSize = int.Parse(args[++i]);
                    option += 2;
                }
                if (arg == "-a")
                {
                    ProgramData.mConflInterval = int.Parse(args[++i]);
                    option += 2;
                }
                if (arg == "-n")
                {
                    ProgramData.mNatFile = args[++i];
                    option += 2;
                }
                if (arg == "-t")
                {
                    ProgramData.mConflType = int.Parse(args[++i]);
                    option += 2;
                }
                if (arg == "-P")
                {
                    string ip = args[++i];
                    string port = args[++i];
                    string hoststring = ip + ":" + port;


                    if (primaryPorts.Contains(int.Parse(port)))
                    {
                        Console.WriteLine("ERROR: Primary port {0} is repeated", port);
                        PrintHelp();
                        return 0;
                    }

                    primaryPorts.Add(int.Parse(port));

                    if (!ProgramData.mPrimaryConn.Contains(hoststring))
                    {
                        ProgramData.mPrimaryConn.Add(hoststring);
                        option += 3;
                    }
                    else
                    {
                        PrintHelp();
                        return 0;
                    }
                }
                if (arg == "-B")
                {
                    string ip = args[++i];
                    string port = args[++i];
                    string hoststring = ip + ":" + port;

                    if (backupPorts.Contains(int.Parse(port)))
                    {
                        Console.WriteLine("ERROR: Backup port {0} is repeated", port);
                        PrintHelp();
                        return 0;
                    }

                    backupPorts.Add(int.Parse(port));

                    if (!ProgramData.mBackupConn.Contains(hoststring))
                    {
                        ProgramData.mBackupConn.Add(hoststring);
                        option += 3;
                    }
                    else
                    {
                        PrintHelp();
                        return 0;
                    }
                }
                //TODO default			
            }

            if (ProgramData.outFile == null)
                ProgramData.outFile = new StreamWriter("data.txt");

            if (args.Length - option == 1)
            {
                try
                {
                    ProgramData.mFileName = args[option];

                    option++;
                    return option;
                }
                catch (Exception ex)
                {
                    PrintHelp();
                    return 0;
                }
            }
            else
            {
                Console.WriteLine("ERROR: Invalid argument count");
                PrintHelp();
                return -1;
            }
        }

        private static void ParseNatFile(ConnectionConfig connectionConfig, string filename)
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

        static void Main(string[] args)
        {
            MyUserEventHandler myUserEventHandler = null;
            MySessionEventHandler mySessionEventHandler = null;
            MyMessageEventHandler myMessageEventHandler = null;
            MyStatisticsEventHandler myStatisticsEventHandler = null;
            Session session = null;
            UserInfo userInfo = null;

            Init();

            try
            {
                Console.WriteLine("Start: " + System.DateTime.Now);

                //int rc = ReadCommandLine(args);
                //if (rc <= 0)
                //    return;

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

                LogFile.WriteLine("Start: " + System.DateTime.Now);

                ApiFactory.Instance.Initialize("CfApiTest", "1.0.0", ProgramData.mDebug, "cfapilog", "DotNet");

                myUserEventHandler = new MyUserEventHandler();
                userInfo = ApiFactory.Instance.CreateUserInfo(ProgramData.mUsername, ProgramData.mPassword, myUserEventHandler);

                mySessionEventHandler = new MySessionEventHandler();
                session = ApiFactory.Instance.CreateSession(userInfo, mySessionEventHandler);

                if (ProgramData.mDebug)
                    PrintAuthStatus(userInfo);

                ProgramData.mMultithread = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("mMultithread"));
                Console.WriteLine(ProgramData.mMultithread);

                if (ProgramData.mMultithread) //for Multithread
                {
                    var sessionConfig = session.GetSessionConfig();
                    sessionConfig.Set(SessionConfig.Parameters.MULTITHREADED_API_CONNECTIONS_BOOL, true);
                    if (ProgramData.mMaxUserThreads > 0)
                        sessionConfig.Set(SessionConfig.Parameters.MAX_USER_THREADS_LONG, ProgramData.mMaxUserThreads);
                    if (ProgramData.mMaxCspThreads > 0)
                        sessionConfig.Set(SessionConfig.Parameters.MAX_CSP_THREADS_LONG, ProgramData.mMaxCspThreads);
                }

                ProgramData.mConflationIndicator = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("mConflationIndicator"));
                Console.WriteLine(ProgramData.mConflationIndicator);

                var connectionConfig = session.GetConnectionConfig();
                connectionConfig.Set(HostConfig.Parameters.CONFLATION_INDICATOR_BOOL, ProgramData.mConflationIndicator);
                //connectionConfig.Set(HostConfig.Parameters.CONNECTION_TIMEOUT_LONG, 10);
                //connectionConfig.Set(HostConfig.Parameters.CONNECTION_RETRY_LIMIT_LONG, 1);

                ProgramData.mNatFile = ConfigurationManager.AppSettings.Get("mNatFile");
                Console.WriteLine(ProgramData.mNatFile);

                if (!string.IsNullOrWhiteSpace(ProgramData.mNatFile))
                    ParseNatFile(connectionConfig, ProgramData.mNatFile);

                ProgramData.mConflInterval = Convert.ToInt32(ConfigurationManager.AppSettings.Get("mConflInterval"));
                Console.WriteLine(ProgramData.mConflInterval);
                ProgramData.mConflType = Convert.ToInt32(ConfigurationManager.AppSettings.Get("mConflType"));
                Console.WriteLine(ProgramData.mConflType);
                ProgramData.mReadTimeout = Convert.ToInt32(ConfigurationManager.AppSettings.Get("mReadTimeout"));
                Console.WriteLine(ProgramData.mReadTimeout);

                if (ProgramData.mConflInterval != -1)
                    connectionConfig.Set(HostConfig.Parameters.CONFLATION_INTERVAL_LONG, ProgramData.mConflInterval);
                if (ProgramData.mConflType > -1)
                    connectionConfig.Set(HostConfig.Parameters.CONFLATION_TYPE_LONG, ProgramData.mConflType);
                if (ProgramData.mReadTimeout > -1)
                    connectionConfig.Set(HostConfig.Parameters.READ_TIMEOUT_LONG, ProgramData.mReadTimeout);

                myStatisticsEventHandler = new MyStatisticsEventHandler();

                ProgramData.mStatsInterval = Convert.ToInt32(ConfigurationManager.AppSettings.Get("mStatsInterval"));
                Console.WriteLine(ProgramData.mStatsInterval);
                ProgramData.mQueueSize = Convert.ToInt32(ConfigurationManager.AppSettings.Get("mQueueSize"));
                Console.WriteLine(ProgramData.mQueueSize);

                if (ProgramData.mStatsInterval > 0)
                    session.RegisterStatisticsEventHandler(myStatisticsEventHandler, ProgramData.mStatsInterval);

                if (ProgramData.mQueueSize != -1)
                    connectionConfig.Set(HostConfig.Parameters.QUEUE_SIZE_LONG, ProgramData.mQueueSize);

                ProgramData.mPrimaryConn.Add(ConfigurationManager.AppSettings.Get("mPrimaryConn1"));

                ProgramData.mPrimaryConn.Add(ConfigurationManager.AppSettings.Get("mPrimaryConn2"));
                Console.WriteLine(ProgramData.mPrimaryConn.Count);
                ProgramData.mCompressedData = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("mCompressedData"));
                Console.WriteLine(ProgramData.mCompressedData);

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
                    PrintHelp();
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
                        PrintHelp();
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
                //register to get called when messages received
                session.RegisterMessageEventHandler(myMessageEventHandler);

                var sw = Stopwatch.StartNew();
                string failReason = null;
                var isSessionStart = session.Start(ref failReason);

                sw.Stop();

                if (!isSessionStart)
                {
                    throw new Exception("session could not be established: " + failReason);
                }
                //Console.WriteLine("Got session took: {0} , time {1}", sw.Elapsed, System.DateTime.Now);

                LogFile.WriteLine("Got session took: {0} , time {1}", sw.Elapsed, System.DateTime.Now);

                Console.WriteLine("pressed enter to quit");
                Console.ReadKey();
                Console.WriteLine("Stopping API session");
            }
            catch (Exception ex)
            {
                LogFile.WriteLine(ex);
                Console.WriteLine(ex);
                Console.ReadKey();
            }
            finally
            {
                if (session != null)
                {
                    session.Stop();
                    ApiFactory.Instance.destroySession(session);
                    ApiFactory.Instance.destroyUserInfo(userInfo);
                    ApiFactory.Instance.Uninitialize();
                }

                LogFile.Flush();
                if (ProgramData.outFile != null)
                    ProgramData.outFile.Flush();

                Thread.Sleep(2000);

                LogWriter.Complete();
                if (ProgramData.outFile != null)
                {
                    ProgramData.outFile.Close();
                    ProgramData.outFile.Dispose();
                }
                LogFile.Close();
                LogFile.Dispose();
            }
        }

        private static void Init()
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
            LogWriter = new ActionBlock<string>(item => WriteToLog(item));
            LogFile = new StreamWriter("cfApiNetlog.txt");
            ProgramData = new ProgData();
        }

        public static void PrintAuthStatus(UserInfo user)
        {
            var state = user.GetState();
            switch (state)
            {
                case UserStates.NOT_AUTHENTICATED:
                    Console.WriteLine("Authorization status = NOT_AUTHENTICATED");
                    break;
                case UserStates.AUTHENTICATED:
                    Console.WriteLine("Authorization status = AUTHENTICATED");
                    break;
                case UserStates.PARTIALLY_AUTHENTICATED:
                    Console.WriteLine("Authorization status = PARTIALLY_AUTHENTICATED");
                    break;
            }
        }

        private static void WriteToLog(string item)
        {
            ProgramData.outFile.Write(item);
            ProgramData.outFile.Flush();
        }

        public static void SendCommand(Session session)
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
                    Console.WriteLine("error occurred on sending command, reading from file {0} Exception: " + ex.Message, ProgramData.mFileName);
                    return;
                }

                var request = session.CreateRequest();

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

                    request.ClearRequest();

                    var badMsg = SetCommand(line, request);
                    if (badMsg)
                        Console.WriteLine("Invalid message not sent command: " + line);
                    else//API send request -- if ok
                    {
                        long reqHandle = request.GenerateTag();
                        RequestCommands.TryAdd(reqHandle, line);
                        reqHandle = session.Send(request);
                        if (reqHandle == 0)
                            Console.WriteLine("Error: send queue is full, cannot send command: " + line);
                        else
                            Console.WriteLine("Send command:[{0}]: tag =[{1}]: ", line, reqHandle);
                    }
                }

                session.FreeRequest(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send command Exception: " + ex.Message);
            }
        }

        private static bool SetCommand(string line, Request request)
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
                                Console.WriteLine("Invalid command: " + parmValues[1]);
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
                            Console.WriteLine("Invalid token in the request: " + parmValues[0]);
                        }
                        else
                            request.Add(tokenNumber, parmValues[1]);
                        break;
                    default:
                        Console.WriteLine("Invalid token in the request: " + parmValues[0]);
                        badMsg = true;
                        break;
                }
            }
            return badMsg;
        }

        private static IEnumerable<string> GetParamters(string line, out bool badMsg)
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
    }
    class MyMessageEventHandler : MessageEventHandler
    {
        private const string ETX = "<ETX>\r";

        public override void OnMessageEvent(MessageEvent messageEvent)
        {
            try
            {
                var type = messageEvent.GetType();

                if ((type == MessageEvent.Types.STATUS) || (type == MessageEvent.Types.IMAGE_COMPLETE))
                {
                    var messageEventTag = messageEvent.GetTag();

                    string command;
                    Program.RequestCommands.TryRemove(messageEventTag, out command);

                    Console.WriteLine("Status code = {0} ({1}) for tag {2} ({3})", messageEvent.GetStatusCode(),
                        messageEvent.GetStatusString(), messageEventTag, command);
                }
                else if (Program.ProgramData.mConflationIndicator)
                {
                    string tmpStr;
                    var isConflatable = messageEvent.IsConflatable();
                    switch (isConflatable)
                    {
                        case 0:
                            tmpStr = "not ";
                            break;
                        case 1:
                            tmpStr = "";
                            break;
                        default:
                            tmpStr = "not known if ";
                            break;
                    }
                    Console.WriteLine("This message is {0}conflatable", tmpStr);
                }

                StringBuilder sb = new StringBuilder();

                //Only add if each is present
                int perm = messageEvent.GetPermission();
                if (perm != 0)
                    sb.AppendLine("PERMISSION(3)=" + perm);

                int src = messageEvent.GetSource();
                if (src != 0)
                    sb.AppendLine("ENUM.SRC.ID(4)=" + src);

                var symbol = messageEvent.GetSymbol();
                if (!string.IsNullOrEmpty(symbol))
                    sb.AppendLine("SYMBOL.TICKER(5)=" + symbol);

                for (ulong i = 0; i < messageEvent.GetNumberofAlternateIndexes(); i++)
                {
                    sb.AppendFormat("{0}({1})={2}\n", messageEvent.GetAlternateIndexTokenName(i),
                                messageEvent.GetAlternateIndexTokenNumber(i),
                                messageEvent.GetAlternateIndexValue(i));
                }

                if (type == MessageEvent.Types.REFRESH)
                    sb.AppendLine("REFRESH(24)=1");

                var reader = messageEvent.GetReader();

                int tokenNumber;
                string tokenName;

                while (reader.Next() != MessageReader.END_OF_MESSAGE)
                {
                    tokenNumber = reader.GetTokenNumber();
                    tokenName = reader.GetTokenName();
                    switch (reader.GetValueType())
                    {
                        case ValueTypes.INT64:
                            var integer = reader.GetValueAsInteger();
                            sb.AppendFormat("{0}({1})={2}\n", tokenName, tokenNumber, integer);
                            break;
                        case ValueTypes.STRING:
                            var str = reader.GetValueAsString();
                            sb.AppendFormat("{0}({1})={2}\n", tokenName, tokenNumber, str);
                            break;
                        case ValueTypes.DOUBLE:
                            var doubleValue = reader.GetValueAsDouble();
                            sb.AppendFormat("{0}({1})={2}\n", tokenName, tokenNumber, doubleValue);
                            break;
                        case ValueTypes.DATETIME:
                            var dateTime = reader.GetValueAsDateTime();
                            var date = dateTime.GetDate();
                            var time = dateTime.GetTime();
                            var dateTimeFormat = string.Format("{0}-{1}-{2} {3}:{4}:{5}.{6} UTC",
                                date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second,
                                (time.Millisecond * 1000) + time.Microsecond);
                            doubleValue = reader.GetValueAsDouble();
                            sb.AppendFormat("{0}({1})={2}({3})\n", tokenName, tokenNumber, dateTimeFormat, doubleValue);
                            break;
                        case ValueTypes.UNKNOWN:
                        default:
                            Console.WriteLine("OnMessageEvent, Unknown value type");
                            sb.AppendFormat("{0}({1})={2}\n", tokenName, tokenNumber, "UNKNOWN value type, cannot decode");
                            break;
                    }
                }
                sb.AppendLine(ETX);

                if (!Program.ProgramData.mQuiet)
                    Program.LogWriter.Post(sb.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("OnMessageEvent exception: " + ex);
                Program.LogWriter.Post(ex + Environment.NewLine);
            }
        }
    }

    class MyUserEventHandler : UserEventHandler
    {
        public override void OnUserEvent(UserEvent userEvent)
        {
            try
            {
                var type = userEvent.GetType();
                if (type == UserEventTypes.AUTHORIZATION_FAILURE)
                {
                    Console.WriteLine("login failed: return code = {0}: {1}", userEvent.GetRetCode(),
                        userEvent.GetRetCodeString());
                }

                if (Program.ProgramData.mDebug)
                    Program.PrintAuthStatus(userEvent.GetSession().GetUserInfo());
            }
            catch (Exception ex)
            {
                Program.LogFile.WriteLine(ex);
            }
        }
    }

    class MyStatisticsEventHandler : StatisticsEventHandler
    {
        public override void OnStatisticsEvent(StatisticsEvent statisticsEvent)
        {
            //try
            //{
            //    Console.WriteLine(
            //        "MSGS_IN --> {0}; MSGS_OUT --> {1}; NET_MSGS_OUT --> {2}; DROP --> {3}; CSP_DROP --> {4}; PCT_FULL={5}/{6}; IN_MSGS/SEC(100ms)={7}/{8} IN_MSGS/SEC(1sec)={9}/{10}; OUT_MSGS/SEC(100ms)={11}/{12} OUT_MSGS/SEC(1sec)={13}/{14}; NET_OUT_MSGS/SEC(100ms)={15}/{16} NET_OUT_MSGS/SEC(1sec)={17}/{18}\n",
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.MSGS_IN),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.MSGS_OUT),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.NET_MSGS_OUT),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.DROP),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.CSP_DROP),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PCT_FULL),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_PCT_FULL),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.IN_MSGS_SEC_100MS),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_IN_MSGS_SEC_100MS),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.IN_MSGS_SEC),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_IN_MSGS_SEC),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.OUT_MSGS_SEC_100MS),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_OUT_MSGS_SEC_100MS),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.OUT_MSGS_SEC),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_OUT_MSGS_SEC),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.NET_OUT_MSGS_SEC_100MS),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_NET_OUT_MSGS_SEC_100MS),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.NET_OUT_MSGS_SEC),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_NET_OUT_MSGS_SEC));

            //    Program.LogFile.WriteLine(
            //        "TIME --> {19}; MSGS_IN --> {0}; MSGS_OUT --> {1}; NET_MSGS_OUT --> {2}; DROP --> {3}; CSP_DROP --> {4}; PCT_FULL={5}/{6}; IN_MSGS/SEC(100ms)={7}/{8} IN_MSGS/SEC(1sec)={9}/{10}; OUT_MSGS/SEC(100ms)={11}/{12} OUT_MSGS/SEC(1sec)={13}/{14}; NET_OUT_MSGS/SEC(100ms)={15}/{16} NET_OUT_MSGS/SEC(1sec)={17}/{18}\n",
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.MSGS_IN),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.MSGS_OUT),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.NET_MSGS_OUT),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.DROP),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.CSP_DROP),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PCT_FULL),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_PCT_FULL),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.IN_MSGS_SEC_100MS),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_IN_MSGS_SEC_100MS),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.IN_MSGS_SEC),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_IN_MSGS_SEC),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.OUT_MSGS_SEC_100MS),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_OUT_MSGS_SEC_100MS),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.OUT_MSGS_SEC),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_OUT_MSGS_SEC),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.NET_OUT_MSGS_SEC_100MS),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_NET_OUT_MSGS_SEC_100MS),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.NET_OUT_MSGS_SEC),
            //        statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_NET_OUT_MSGS_SEC), System.DateTime.Now);

            //    Program.LogFile.Flush();
            //}
            //catch (Exception ex)
            //{
            //    Program.LogFile.WriteLine(ex);
            //}
        }
    }

    class MySessionEventHandler : SessionEventHandler
    {
        public override void OnSessionEvent(SessionEvent sessionEvent)
        {
            try
            {
                var type = sessionEvent.GetType();
                if (type == SessionEventTypes.CFAPI_SESSION_ESTABLISHED)
                {
                    Program.LogFile.WriteLine("Session established " + System.DateTime.Now.ToString(Program.TimeFormat));
                    var session = sessionEvent.GetSession();
                    Task.Factory.StartNew(() => Program.SendCommand(session));
                }
                else if (type == SessionEventTypes.CFAPI_SESSION_UNAVAILABLE)
                {
                    Console.WriteLine("session unavailable");
                }
                else if (type == SessionEventTypes.CFAPI_SESSION_RECOVERY)
                {
                    Console.WriteLine("SESSION_RECOVERY");
                }
                else if (type == SessionEventTypes.CFAPI_CDD_LOADED)
                {
                    Console.WriteLine("CFAPI_CDD_LOADED; version={0}", sessionEvent.GetCddVersion());
                }
                else if (type == SessionEventTypes.CFAPI_SESSION_RECOVERY_SOURCES)
                {
                    int sourceID = sessionEvent.GetSourceID();
                    Console.WriteLine("CFAPI_SESSION_RECOVERY_SOURCES  sourceID = {0}", sourceID);
                }
                else if (type == SessionEventTypes.CFAPI_SESSION_AVAILABLE_ALLSOURCES)
                {
                    int sourceID = sessionEvent.GetSourceID();
                    Console.WriteLine("CFAPI_SESSION_AVAILABLE_ALLSOURCES  sourceID ={0}", sourceID);
                }
                else if (type == SessionEventTypes.CFAPI_SESSION_AVAILABLE_SOURCES)
                {
                    int sourceID = sessionEvent.GetSourceID();
                    Console.WriteLine("CFAPI_SESSION_AVAILABLE_SOURCES  sourceID = {0}", sourceID);
                }
                else if (type == SessionEventTypes.CFAPI_SESSION_RECEIVE_QUEUE_ABOVE_THRESHOLD)
                {
                    Console.WriteLine("SESSION_RECEIVE_QUEUE_ABOVE_THRESHOLD ({0})", sessionEvent.GetQueueDepth());
                }
                else if (type == SessionEventTypes.CFAPI_SESSION_RECEIVE_QUEUE_BELOW_THRESHOLD)
                {
                    Console.WriteLine("SESSION_RECEIVE_QUEUE_BELOW_THRESHOLD ({0})", sessionEvent.GetQueueDepth());
                }
                else
                    Console.WriteLine("Unknown session state");
            }
            catch (Exception ex)
            {
                Program.LogFile.WriteLine(ex);
            }
        }
    }
}
