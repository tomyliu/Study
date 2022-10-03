using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMall.Models
{
    public class UserLoginModel
    {
        [Display(Name = "用戶名")]
        [Required]
        [MaxLength(120, ErrorMessage = "{0}長度不能超過120")]
        public string Username { get; set; }

        [Display(Name = "密碼")]
        [Required]
        [MaxLength(120, ErrorMessage = "{0}長度不能超過120")]
        public string Passwd { get; set; }

        [Display(Name = "驗證碼")]
        [Required]
        public string Captcha { get; set; }
    }
}
