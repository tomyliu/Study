using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TMall.Models
{
    public class ItemModel
    {
        [Display(Name = "編號")]
        public int ItemId { get; set; }

        [Display(Name = "分類編號")]
        public int ItemCategoryId { get; set; }

        [Display(Name = "名稱")]
        public string ItemName { get; set; }

        [Display(Name = "圖片URL")]
        public string ItemPicture { get; set; }

        [Display(Name = "具體參數")]
        public string ItemText { get; set; }

        [Display(Name = "價格")]
        public double ItemPrice { get; set; }

        [Display(Name = "銷量")]
        public int ItemSales { get; set; }

        [Display(Name = "庫存")]
        public int ItemNumber { get; set; }

        [Display(Name = "關鍵字")]
        public string ItemKeyword { get; set; }

        [Display(Name = "最後更新時間")]
        public DateTime LastUpdateTime { get; set; }
    }
}
