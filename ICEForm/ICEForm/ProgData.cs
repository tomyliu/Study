using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICEForm
{
    public class ProgData
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
}
