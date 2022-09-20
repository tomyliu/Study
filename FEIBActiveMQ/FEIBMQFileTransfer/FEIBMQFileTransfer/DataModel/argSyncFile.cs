using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIBMQFileTransfer.DataModel
{
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
}
