using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIBMQFileTransferTOPIC.DataModel
{
    public enum CommandRunType
    {
        Message = 0,        // 一般訊息(未開發）
        File = 1,           // 檔案傳輸模式
        Command = 2,        // 一般命令模式
        SummitCommand,      // SUMMIT 命令模式
        GetFile             // 取得檔案(未開發）      
    }
}
