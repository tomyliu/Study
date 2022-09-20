using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIBAgentService.DataModel
{
    public class argAgentCommand
    {
        /// <summary>
        /// 執行人員
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 命令模式
        /// File:檔案傳輸
        /// Command: 執行命令
        /// SummitCommand: 執行Summit命令
        /// Message: 傳送訊息
        /// </summary>
        public CommandRunType CommandType { get; set; }
        /// <summary>
        /// 傳送時間
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 回覆時間
        /// </summary>
        public DateTime ReceiveTime { get; set; }
        /// <summary>
        /// 回應訊息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 檔案資訊
        /// </summary>
        public argSyncFile FileInfo { get; set; }
        /// <summary>
        /// 命令資訊
        /// </summary>
        public argRunCommand CmdInfo { get; set; }
    }
    public class argRunCommand
    {
        #region Request
        public string Cmd { get; set; }
        public string Args { get; set; }
        public string RunPath { get; set; }
        #endregion
        #region Response
        public string StarndardOutput { get; set; }
        public string StarndardError { get; set; }
        public string ReturnCode { get; set; }
        #endregion
    }
    public class argSyncFile
    {
        #region Request

        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 檔案路徑
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 檔案內容
        /// </summary>
        public string FileContent { get; set; }
        /// <summary>
        /// MD5 編碼
        /// </summary>
        public string MD5Check { get; set; }
        /// <summary>
        /// 強制覆蓋
        /// </summary>
        public bool cbIsCover { get; set; }
        #endregion

        #region Response
        /// <summary>
        /// 傳遞狀態
        /// </summary>
        public bool TransferStatus { get; set; }
        /// <summary>
        /// Topic Listener
        /// </summary>
        public string TopicListener { get; set; }
        /// <summary>
        /// 回傳檔案內容
        /// </summary>
        public string RepFileContent { get; set; }
        #endregion
    }

    public enum CommandRunType
    {
        Message = 0,        // 一般訊息(未開發）
        File = 1,           // 檔案傳輸模式
        Command = 2,        // 一般命令模式
        SummitCommand,      // SUMMIT 命令模式
        GetFile             // 取得檔案(未開發）      
    }
}
