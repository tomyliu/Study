using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMall.Controllers
{
    // 用戶篩檢程式
    public class UsersController : Controller
    {

        // 驗證碼部分, 收到請求的時候刷新驗證碼
        [HttpGet]
        public ActionResult Captcha()
        {
            byte[] arr;
            string code;
            // 獲取驗證碼和對應的的圖片
            TMall.util.Captcha.GetCaptcha(out arr, out code);
            // 將新的驗證碼添加到Session裡, (全變成小寫)
            Session.Add("Captcha", code.ToLower());
            // 返回驗證碼圖片
            return File(arr, "image/jpeg");
        }

        // 註冊頁面, 返回註冊頁面
        [HttpGet]
        public ActionResult Register(string redirectURL)
        {
            ViewData["redirectURL"] = redirectURL; // 將重定向的url傳遞到表單隱藏欄位, 實現POST請求之後能重定向回去
            return View();
        }

        // 註冊提交頁面
        [HttpPost]
        public ActionResult Register(TMall.Models.UserProfileModel profileModel, string redirectURL)
        {
            ViewData["redirectURL"] = redirectURL;  // 重定向的url傳遞到表單隱藏欄位, 以免下次提交的時候丟失
            if (ModelState.IsValid)
            { // 表單有效
                if (!profileModel.Captcha.ToLower().Equals((string)Session["Captcha"]))
                {// 如果驗證碼不相等
                    ModelState.AddModelError("Captcha", "請填寫正確的驗證碼");
                    return View();
                }
                if (TMall.Respository.Users.hasRegistered(profileModel.Username))
                {
                    ModelState.AddModelError("Username", "這個用戶名已經被註冊過了, 請換一個試試");
                    return View();
                }
                profileModel.RegisterTime = DateTime.Now;
                profileModel.Level = TMall.Respository.Users.NormalUser;//註冊用戶默認不是管理員許可權
                if (TMall.Respository.Users.register(profileModel))
                {// 註冊成功
                    Session.Add("username", profileModel.Username);
                    if (string.IsNullOrEmpty(redirectURL)) return RedirectToAction("Index", "Home");
                    else return Redirect(redirectURL);
                }
            }
            return View();
        }

        // 登錄頁面
        [HttpGet]
        public ActionResult Login(string redirectURL)
        {
            if (!string.IsNullOrEmpty((string)Session["username"]))
                if (string.IsNullOrEmpty(redirectURL)) return RedirectToAction("Index", "Home");
                else return Redirect(redirectURL);// 如果處於登錄狀態, 直接重定向回去(當然, 這個正常操作是不會發生的)

            HttpCookie cookie = Request.Cookies["authorizeUser"];// 先查cookie
            if (cookie != null)
            {
                string username = cookie["username"];
                string passwd = cookie["passwd"];
                if (TMall.Respository.Users.login(username, passwd))
                {// 如果cookie保存了有效值, 就直接登錄
                    Session.Add("username", username);
                    if (string.IsNullOrEmpty(redirectURL)) return RedirectToAction("Index", "Home");
                    else return Redirect(redirectURL);   // 直接登錄後重定向回去
                }
            }

            // 前面嘗試都不可以之後, 就傳回表單
            if (!string.IsNullOrEmpty(redirectURL))
                ViewData["redirectURL"] = redirectURL; // 將重定向的url傳遞到表單隱藏欄位, 實現POST請求之後能重定向回去
            return View();
        }

        // 登錄提交頁面
        [HttpPost]
        public ActionResult Login(TMall.Models.UserLoginModel loginModel, string remember, string redirectURL)
        {
            if (!string.IsNullOrEmpty(redirectURL))
                ViewData["redirectURL"] = redirectURL;// 將重定向的url傳遞到視圖裡
            if (ModelState.IsValid)
            { //看每個域是否都有效
                if (!loginModel.Captcha.ToLower().Equals((string)Session["Captcha"]))
                {// 如果驗證碼不相等
                    ModelState.AddModelError("Captcha", "請填寫正確的驗證碼");
                    return View();
                }
                if (TMall.Respository.Users.login(loginModel.Username, loginModel.Passwd))
                {// 登錄成功
                 // 將登錄信息寫到Session中
                    Session.Add("username", loginModel.Username);

                    bool re = false;
                    if (!string.IsNullOrEmpty(remember) && remember.Equals("on")) re = true;
                    if (re)
                    {// 如果勾選了記住密碼, 就保存7天cookie
                        HttpCookie cookie = new HttpCookie("authorizeUser");
                        cookie.Expires = DateTime.Now.Add(new TimeSpan(7, 0, 0, 0));// 設置過期時間為7天后
                        cookie.Values.Add("username", loginModel.Username);
                        cookie.Values.Add("passwd", loginModel.Passwd);
                        Response.AppendCookie(cookie);// 將cookie增加到response中
                    }

                    if (string.IsNullOrEmpty(redirectURL)) return RedirectToAction("Index", "Home");// 如果重定向的字串不合法,就回主頁
                    return Redirect(redirectURL);// 重定向到那個字串
                }
                ModelState.AddModelError("Passwd", "用戶名密碼錯誤");
                return View();//將已經填寫的錯誤資訊一併返回
            }
            return View();
        }

        // 登出
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("username");    // 清除登錄狀態
            HttpCookie cookie = new HttpCookie("authorizeUser");
            cookie.Expires = DateTime.Now.Add(new TimeSpan(-10));
            Response.AppendCookie(cookie); // 用過期的cookie覆蓋掉原來的
            return RedirectToAction("Index", "Home");// 重定向到主頁
        }


        // 獲取個人中心的頁面
        [HttpGet]
        [MyUserFilter]
        public ActionResult Profiles()
        {
            string username = (string)Session["username"];
            Models.UserProfileModel user = Respository.Users.GetUsers(username);
            return View(user);
        }

        // 更新使用者資訊
        [HttpPost]
        [MyUserFilter]
        public ActionResult Profiles(TMall.Models.UserProfileModel profileModel)
        {
            if (ModelState.IsValid)
            { //表單格式通過驗證
                if (!profileModel.Captcha.ToLower().Equals((string)Session["Captcha"]))
                {//驗證碼不相等
                    return Json(new { result = "fail", data = "驗證碼錯誤,請填寫正確的驗證碼!" });
                }
                if (Respository.Users.UpdateUsers(profileModel))
                {
                    return Json(new { result = "success" });
                }
                else return Json(new { result = "fail", data = "資料庫操作錯誤!" });
            }
            else
            {
                // 獲取表中的錯誤資訊, 返回
                List<string> errorList = new List<string>();
                List<string> keys = ModelState.Keys.ToList();
                foreach (var key in keys)
                {
                    var errors = ModelState[key].Errors.ToList();
                    foreach (var error in errors)
                    {
                        errorList.Add(error.ErrorMessage);
                    }
                }
                // 更新失敗將資訊返回
                return Json(new { result = "fail", data = errorList });
            }
        }

        // 用戶收藏的功能
        [HttpGet]
        [MyUserFilter]
        public ActionResult Collect(int? item_id, string op)
        {
            string username = (string)Session["username"];
            if (string.IsNullOrEmpty(op) || !item_id.HasValue)
            {//如果參數不全,就是返回列表
                return Json(Respository.Users.GetCollects(username), JsonRequestBehavior.AllowGet);
            }
            if (op.Equals("add"))
            {
                return Json(Respository.Users.AddCollect(username, item_id.Value), JsonRequestBehavior.AllowGet);
            }
            else if (op.Equals("del"))
            {
                return Json(Respository.Users.DelCollect(username, item_id.Value), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(Respository.Users.JudgeCollect(username, item_id.Value), JsonRequestBehavior.AllowGet);
            }
        }

        // 購物車的功能
        [HttpGet]
        [MyUserFilter]
        public ActionResult Cart(string op, int? item_id, int? num)
        {
            string username = (string)Session["username"];
            int id = item_id ?? -1;
            int number = num ?? 1;
            if (!string.IsNullOrEmpty(op))
            {
                if (op.Equals("list"))
                { // list
                    return Json(Respository.Users.GetCarts(username), JsonRequestBehavior.AllowGet);
                }
                else
                {// del, set,add
                    if (id == -1) return Json("商品編號參數錯誤!", JsonRequestBehavior.AllowGet);
                    if (op.Equals("del"))
                    {
                        return Json(Respository.Users.DelCart(username, id), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {//set or add
                        if (number <= 0) return Json("商品個數不合法!", JsonRequestBehavior.AllowGet);
                        if (Respository.Users.JudgeCart(username, id))
                        { // 資料庫已經存在了
                            if (op.Equals("add"))
                            {
                                return Json(Respository.Users.UpdateCartAdd(username, id, number), JsonRequestBehavior.AllowGet);
                            }
                            else
                            { //set
                                return Json(Respository.Users.UpdateCartSet(username, id, number), JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        { // 需要在資料庫新建
                            return Json(Respository.Users.AddCart(username, id, number), JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return Json("操作指令不合法!!", JsonRequestBehavior.AllowGet);
        }

        // 訂單的功能
        [HttpGet]
        [MyUserFilter]
        public ActionResult Orders(string op, int? item_id, int? order_id)
        {
            string username = (string)Session["username"];
            if (!string.IsNullOrEmpty(op))
            {
                if (op.Equals("list"))
                {//獲取訂單清單
                    return Json(Respository.Users.GetOrders(username), JsonRequestBehavior.AllowGet);
                }
                else if (op.Equals("add"))
                { // 新建訂單
                    if (!item_id.HasValue) return Json("參數錯誤!", JsonRequestBehavior.AllowGet);
                    // 獲取商品在購物車裡的數量
                    int number = Respository.Users.GetNumberInCart(username, item_id.Value);
                    // 將這個商品在用戶購物車刪除
                    Respository.Users.DelCart(username, item_id.Value);
                    // 獲取商品的價格
                    double price = Respository.Users.GetItemPrice(item_id.Value);
                    // 新建訂單, 並且將這個訂單返回
                    return Json(Respository.Users.AddOrders(username, item_id.Value, number, price), JsonRequestBehavior.AllowGet);
                }
                else if (op.Equals("pay"))
                { // 支付
                    if (!order_id.HasValue) return Json("參數錯誤!", JsonRequestBehavior.AllowGet);
                    return Json(Respository.Users.ChangeOrderStatus(order_id.Value, "已支付"), JsonRequestBehavior.AllowGet);
                }
            }
            return Json("操作指令不合法!!", JsonRequestBehavior.AllowGet);
        }
    }
}
