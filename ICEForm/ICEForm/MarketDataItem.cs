using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICEForm
{
    public  class MarketDataItem
    {
        /// <summary>
        /// 項目
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 項目類別 0: 定義 1:資料
        /// </summary>
        public int ItemType { get; set; }

        public string Code { get; set; }
        public string Value { get; set; }
        

    }
}
