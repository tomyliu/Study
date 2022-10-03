using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMall.Models
{
    public class UserProfileModel
    {
        [Display(Name = "用戶名")]
        [Required]
        [MaxLength(120, ErrorMessage = "{0}長度不能超過120")]
        public string Username { get; set; }

        [Display(Name = "密碼")]
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(120, ErrorMessage = "{0}長度不能超過120")]
        public string Passwd { get; set; }

        [Display(Name = "郵箱")]
        [Required]
        [DataType(DataType.EmailAddress)]//郵箱驗證方式
        public string Email { get; set; }

        [Display(Name = "使用者等級")]
        public int Level { get; set; }// 使用者等級

        [Display(Name = "出生日期")]
        [DataType(DataType.Time)]
        public DateTime Birthday { get; set; }

        [Display(Name = "電話")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "註冊時間")]
        public DateTime RegisterTime { get; set; }

        [Display(Name = "QQ號碼")]
        public int QQNumber { get; set; }

        [Display(Name = "收貨地址")]
        [Required]
        public string Address { get; set; }

        [Display(Name = "驗證碼")]
        [Required]
        public string Captcha { get; set; }
    }
}
