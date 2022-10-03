using System.Web;
using System;
using System.Web.Mvc;

namespace TMall
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }


    // 自訂許可權篩檢程式, 用於檢查是否是登錄用戶, 如果不是登錄用戶,則跳轉到登錄頁面
    public class MyUserFilter : AuthorizeAttribute
    {
        // 重載驗證函數, 判斷是否是登錄用戶
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!string.IsNullOrEmpty((string)httpContext.Session["username"])) return true; //已經登錄返回true, 如果session沒有檢查到, 也就是沒有登錄, 就檢查cookie有沒有有效的登錄資訊
            HttpCookie cookie = httpContext.Request.Cookies["authorizeUser"];
            if (cookie != null)
            {
                string username = cookie["username"];
                string passwd = cookie["passwd"];
                if (TMall.Respository.Users.login(username, passwd))
                {// 如果cookie保存了有效值, 就直接登錄 , 並且返回true
                    httpContext.Session.Add("username", username);
                    return true;
                }
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {// 如果是ajax非同步請求, 就返回錯誤資訊, 否則重定向
                filterContext.HttpContext.Response.StatusCode = 401;// 返回未授權狀態碼
            }
            else filterContext.Result = new RedirectResult("/Users/Login?redirectURL=" + filterContext.HttpContext.Request.Url);
        }
    }

    // 自訂許可權篩檢程式, 用於檢查是不是管理員用戶, 如果不是, 跳轉到錯誤頁面
    public class MyAdminFilter : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!string.IsNullOrEmpty((string)httpContext.Session["username"]))
            {
                //已經登錄就判斷是不是管理員, 如果session沒有檢查到, 也就是沒有登錄, 就檢查cookie有沒有有效的登錄資訊
                return TMall.Respository.Users.isAdmin((string)httpContext.Session["username"]);
            }
            HttpCookie cookie = httpContext.Request.Cookies["authorizeUser"];
            if (cookie != null)
            {
                string username = cookie["username"];
                string passwd = cookie["passwd"];
                if (TMall.Respository.Users.login(username, passwd))
                {// 如果cookie保存了有效值, 就直接登錄 , 並且返回true
                    httpContext.Session.Add("username", username);
                    return TMall.Respository.Users.isAdmin(username);
                }
            }
            return false;
        }
        // 如果不是管理員, 就返回到主頁
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {// 如果是ajax非同步請求, 就返回錯誤資訊, 否則重定向到主頁
                filterContext.HttpContext.Response.StatusCode = 401;// 返回未授權狀態碼
            }
            else filterContext.Result = new RedirectResult("/Home/Index");
        }
    }
}

