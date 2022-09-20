using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIBMQFileTransfer.DataModel
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
}
