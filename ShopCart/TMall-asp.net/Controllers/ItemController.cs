using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;


namespace TMall.Controllers
{
    /*
	 * 
 /Item
  |-- /List      get:返回商品列表, 兩個參數: category_id,page: 商品類型,頁數
  |-- /Detail    get:返回商品的詳細頁面, 一個參數: item_id
  |-- /Edit      get:有參數時是修改物品, 無參數時是增加新物品(需要管理員許可權)
                 post: 保存修改/增加的物品 (需要管理員許可權)
  |
  |(以下功能都是在Detail頁面中進行的js非同步交互, 沒有單獨的頁面)
  |-- /Category  get: 獲取商品分類的列表
  |-- /Delete    get: 刪除某個物品 (需要管理員許可權)
  |-- /Recommend get: 獲取推薦的物品列表(返回json)
  |-- /Comment   get: 獲取評論列表(返回json), 一個參數: item_id
                 post: 增加評論, 兩個參數: item_id, comment
	 */
    public class ItemController : Controller
    {
        public const int PageSize = 16; // 設置每頁展示的大小

        // 獲取商品列表
        [HttpGet]
        public ActionResult List(int? category_id, int? page)
        {
            int ca_id = category_id ?? 1;
            int pa_id = page ?? 1;
            int now = (pa_id - 1) * PageSize; // 當前頁面的起始範圍
            int totalCount;// 總個數
            PagedList<TMall.Models.ItemModel> pagedList = TMall.Respository.Item.
                GetItems(ca_id, now, PageSize, out totalCount).ToPagedList(now, PageSize);
            pagedList.TotalItemCount = totalCount;
            pagedList.CurrentPageIndex = pa_id;
            ViewData["totalCount"] = totalCount;
            return View(pagedList);
        }

        // 查看商品詳細資訊, 需要登錄許可權
        [HttpGet]
        [MyUserFilter]
        public ActionResult Detail(int? item_id)
        {
            if (!item_id.HasValue) return new HttpStatusCodeResult(404);//參數不合法,跳轉到404頁面
            TMall.Models.ItemModel item = TMall.Respository.Item.GetItem(item_id.Value);
            if (item == null) return new HttpStatusCodeResult(404);
            return View(item);
        }

        // 編輯商品資訊(或者增加新商品), 獲取表單頁面 , 需要管理員許可權
        [HttpGet]
        [MyAdminFilter]
        public ActionResult Edit(int? item_id)
        {
            if (item_id.HasValue)
            {
                TMall.Models.ItemModel item = TMall.Respository.Item.GetItem(item_id.Value);
                return View(item);
            }
            return View();
        }

        // 編輯商品資訊(或者增加商品), 提交頁面, 需要管理員許可權
        [HttpPost]
        [MyAdminFilter]
        public ActionResult Edit(TMall.Models.ItemModel itemModel)
        {
            if (ModelState.IsValid)
            {//查看表單是不是有效
                itemModel.LastUpdateTime = DateTime.Now;
                if (itemModel.ItemId < 0)
                {//新增
                    int id = TMall.Respository.Item.AddItem(itemModel);
                    if (id > 0) return Redirect($"/Item/Detail?item_id={id}");//成功後轉移到詳情頁
                }
                else
                {// update
                    if (TMall.Respository.Item.UpdateItem(itemModel))
                        return Redirect($"/Item/Detail?item_id={itemModel.ItemId}");//成功後轉移到詳情頁
                }
            }
            return View();
        }

        // 刪除商品, 需要管理員許可權
        [HttpGet]
        [MyAdminFilter]
        public ActionResult Delete(int? item_id)
        {
            if (item_id.HasValue)
            {
                int id = item_id.Value;
                if (TMall.Respository.Item.DeleteItem(id))
                {
                    return Json("Delete success!", JsonRequestBehavior.AllowGet);
                }
                else return Json("Delete fail!", JsonRequestBehavior.AllowGet);
            }
            return Json("404 Not Found!", JsonRequestBehavior.AllowGet);
        }

        // 獲取目錄
        [HttpGet]
        public ActionResult Category()
        {
            return Json(TMall.Respository.Item.GetItemCategories(), JsonRequestBehavior.AllowGet);
        }

        // 獲取某個商品的所有評論
        [HttpGet]
        [MyUserFilter]
        public ActionResult Comment(int? item_id)
        {
            if (item_id.HasValue)
            {
                return Json(TMall.Respository.Item.GetItemComments(item_id.Value), JsonRequestBehavior.AllowGet);
            }
            // 如果item_id不合法
            return Json("404 Item Not Found", JsonRequestBehavior.AllowGet);
        }


        // 評論某個商品
        [HttpPost]
        [MyUserFilter]
        public ActionResult Comment(TMall.Models.ItemCommentModel itemCommentModel)
        {
            // 這個是使用json交互的, 所以返回一個固定格式的資訊
            if (ModelState.IsValid)
            {
                itemCommentModel.Username = (string)Session["username"];
                if (TMall.Respository.Item.AddItemComment(itemCommentModel))
                {
                    return Json("success");
                }
            }
            return Json("fail");// 返回失敗的json資訊
        }


        // 返回推薦的商品列表
        [HttpGet]
        [MyUserFilter]
        public ActionResult Recommend()
        {
            string username = (string)Session["username"];
            return Json(Respository.Recommend.GetRecommend(username), JsonRequestBehavior.AllowGet);
        }

    }
}
