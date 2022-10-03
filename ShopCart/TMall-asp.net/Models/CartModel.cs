using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace TMall.Models
{
    public class CartModel
    {

        [Display(Name = "商品編號")]
        public int ItemId { get; set; }

        [Display(Name = "名稱")]
        public string ItemName { get; set; }

        [Display(Name = "圖片URL")]
        public string ItemPicture { get; set; }

        [Display(Name = "價格")]
        public double ItemPrice { get; set; }

        [Display(Name = "銷量")]
        public int ItemSales { get; set; }

        [Display(Name = "庫存")]
        public int ItemNumber { get; set; }

        [Display(Name = "購物車內個數")]
        public int CartItemNumber { get; set; }

        [Display(Name = "加入時間")]
        public DateTime CartInTime { get; set; }
    }
}
