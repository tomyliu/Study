using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TMall.Models
{
    public class ItemCommentModel
    {
        [Display(Name = "評論編號")]
        public int ItemCommentId { get; set; }

        [Display(Name = "商品編號")]
        public int ItemId { get; set; }

        [Display(Name = "評價者")]
        public string Username { get; set; }

        [Display(Name = "分數")]
        public int ItemCommentScore { get; set; }

        [Display(Name = "評價內容")]
        public string ItemCommentText { get; set; }

        [Display(Name = "評價時間")]
        public DateTime ItemCommentTime { get; set; }
    }
}
