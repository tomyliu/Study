using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using TMall.util;

namespace TMall.Respository
{
    public class Item
    {
        private static List<Models.ItemCategoryModel> ItemCategoriesCache = null; // 因為商品類型運行期間一般不會變化, 並且不多(<1000), 所以直接緩存起來

        // 查詢商品列表
        public static List<Models.ItemModel> GetItems(int category, int nowCount, int size, out int totalCount)
        {
            SqlParameter[] sqlParameter1 = new SqlParameter[] {
                new SqlParameter("@C",category),
            };
            string sql1 = "select count(*) from item where item_category_id = @C";
            totalCount = (int?)SqlHelper.ExecuteScalar(sql1, sqlParameter1) ?? -1;
            SqlParameter[] sqlParameters2 = new SqlParameter[] {
                new SqlParameter("@C",category),
                new SqlParameter("@N",nowCount),
                new SqlParameter("@S",size),
            };
            string sql = "select * from item where item_category_id = @C order by item_id offset @N rows fetch next @S rows only";
            SqlDataReader sqlData = SqlHelper.ExecuteTable(sql, sqlParameters2);
            List<Models.ItemModel> items = new List<Models.ItemModel>();
            if (sqlData == null) return items;
            while (sqlData.Read())
            {
                Models.ItemModel item = new Models.ItemModel
                {
                    ItemId = sqlData.GetInt32(0),
                    ItemCategoryId = sqlData.GetInt32(1),
                    ItemName = sqlData.GetString(2),
                    ItemPicture = sqlData.GetString(3),
                    ItemText = sqlData.GetString(4),
                    ItemPrice = sqlData.GetDouble(5),
                    ItemSales = sqlData.GetInt32(6),
                    ItemNumber = sqlData.GetInt32(7),
                    ItemKeyword = sqlData.GetString(8),
                    LastUpdateTime = sqlData.GetDateTime(9)
                };
                items.Add(item);
            }
            sqlData.Close();
            return items;
        }

        // 查詢具體商品資訊
        public static TMall.Models.ItemModel GetItem(int item_id)
        {
            string sql = $"select * from item where item_id = {item_id}";
            SqlDataReader sqlData = SqlHelper.ExecuteTable(sql, null);
            if (sqlData != null && sqlData.Read())
            {
                Models.ItemModel item = new Models.ItemModel
                {
                    ItemId = sqlData.GetInt32(0),
                    ItemCategoryId = sqlData.GetInt32(1),
                    ItemName = sqlData.GetString(2),
                    ItemPicture = sqlData.GetString(3),
                    ItemText = sqlData.GetString(4),
                    ItemPrice = sqlData.GetDouble(5),
                    ItemSales = sqlData.GetInt32(6),
                    ItemNumber = sqlData.GetInt32(7),
                    ItemKeyword = sqlData.GetString(8),
                    LastUpdateTime = sqlData.GetDateTime(9)
                };
                sqlData.Close();
                return item;
            }
            return null;
        }

        //刪除某個商品
        public static bool DeleteItem(int item_id)
        {
            string sql = $"delete from item where item_id = {item_id}";
            return 1 == SqlHelper.ExecuteNoQuery(sql, null);
        }

        //增加商品, 返回商品id
        public static int AddItem(TMall.Models.ItemModel item)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@A",item.ItemCategoryId),
                new SqlParameter("@B",item.ItemName),
                new SqlParameter("@C",item.ItemPicture),
                new SqlParameter("@D",item.ItemText),
                new SqlParameter("@E",item.ItemPrice),
                new SqlParameter("@F",item.ItemSales),
                new SqlParameter("@G",item.ItemNumber),
                new SqlParameter("@H",item.ItemKeyword),
                new SqlParameter("@I",item.LastUpdateTime),
            };
            string sql = "insert into item (item_category_id,item_name,item_picture,item_text,item_price,item_sales,item_number,item_keyword,item_last_update_time) values(@A,@B,@C,@D,@E,@F,@G,@H,@I)\r\n select SCOPE_IDENTITY()\r\n go";
            var x = SqlHelper.ExecuteScalar(sql, sqlParameters);
            // 如果返回的object是decimal的實例, 要轉化為int返回
            return Convert.ToInt32(x);
        }

        // 更新商品
        public static bool UpdateItem(TMall.Models.ItemModel item)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@A",item.ItemCategoryId),
                new SqlParameter("@B",item.ItemName),
                new SqlParameter("@C",item.ItemPicture),
                new SqlParameter("@D",item.ItemText),
                new SqlParameter("@E",item.ItemPrice),
                new SqlParameter("@F",item.ItemSales),
                new SqlParameter("@G",item.ItemNumber),
                new SqlParameter("@H",item.ItemKeyword),
                new SqlParameter("@I",item.LastUpdateTime),
                new SqlParameter("@J",item.ItemId),
            };
            string sql = "update item set item_category_id=@A,item_name=@B,item_picture=@C,item_text=@D,item_price=@E,item_sales=@F,item_number=@G,item_keyword=@H,item_last_update_time=@I where item_id = @J";
            return 1 == (int)SqlHelper.ExecuteNoQuery(sql, sqlParameters);
        }

        // 查詢商品目錄
        public static List<TMall.Models.ItemCategoryModel> GetItemCategories()
        {
            if (ItemCategoriesCache != null) return ItemCategoriesCache; // 如果本地有緩存,直接返回即可
            SqlDataReader data = SqlHelper.ExecuteTable("select * from item_category", null);
            List<TMall.Models.ItemCategoryModel> itemCategories = new List<Models.ItemCategoryModel>();
            if (data == null) return itemCategories;
            while (data.Read())
            {
                Models.ItemCategoryModel itemCategory = new Models.ItemCategoryModel
                {
                    ItemCategoryId = data.GetInt32(0),
                    ItemCategoryName = data.GetString(1)
                };
                itemCategories.Add(itemCategory);
            }
            data.Close();
            ItemCategoriesCache = itemCategories; // 將查詢的資料作為緩存保存起來
            return itemCategories;
        }

        // 查詢商品的所有評論
        public static List<TMall.Models.ItemCommentModel> GetItemComments(int item_id)
        {
            string sql = $"select * from item_comment where item_id = {item_id}";
            SqlDataReader data = SqlHelper.ExecuteTable(sql, null);
            List<Models.ItemCommentModel> comments = new List<Models.ItemCommentModel>();
            if (data == null) return comments;
            while (data.Read())
            {
                Models.ItemCommentModel comment = new Models.ItemCommentModel
                {
                    ItemCommentId = data.GetInt32(0),
                    ItemId = data.GetInt32(1),
                    Username = data.GetString(2),
                    ItemCommentScore = data.GetInt32(3),
                    ItemCommentText = data.GetString(4),
                    ItemCommentTime = data.GetDateTime(5)
                };
                comments.Add(comment);
            }
            data.Close();
            return comments;
        }

        // 新增評論
        public static bool AddItemComment(Models.ItemCommentModel itemComment)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@A",itemComment.ItemId),
                new SqlParameter("@B",itemComment.Username),
                new SqlParameter("@C",itemComment.ItemCommentScore),
                new SqlParameter("@D",itemComment.ItemCommentText),
                new SqlParameter("@E",itemComment.ItemCommentTime)
            };
            string sql = "insert into item_comment (item_id,username,item_comment_score,item_comment_text,item_comment_time) values(@A,@B,@C,@D,@E)";
            return 1 == (int)SqlHelper.ExecuteNoQuery(sql, sqlParameters);
        }

        // 根據商品類型的id獲取商品類型的名稱
        public static string GetItemCategoryName(int category_id)
        {
            if (ItemCategoriesCache == null) GetItemCategories();
            foreach (var entry in ItemCategoriesCache)
            {
                if (entry.ItemCategoryId == category_id) return entry.ItemCategoryName;
            }
            return string.Empty;
        }

        // 返回某個類型最暢銷(熱門)的N個物品
        public static List<Models.ItemModel> GetTopSaleItems(int itemCategoryId, int N)
        {
            string sql = $"select top {N} * from item where item_category_id={itemCategoryId} order by item_sales desc";
            SqlDataReader data = SqlHelper.ExecuteTable(sql, null);
            List<Models.ItemModel> items = new List<Models.ItemModel>();
            if (data == null) return items;
            while (data.Read())
            {
                Models.ItemModel item = new Models.ItemModel
                {
                    ItemId = data.GetInt32(0),
                    ItemCategoryId = data.GetInt32(1),
                    ItemName = data.GetString(2),
                    ItemPicture = data.GetString(3),
                    ItemText = data.GetString(4),
                    ItemPrice = data.GetDouble(5),
                    ItemSales = data.GetInt32(6),
                    ItemNumber = data.GetInt32(7),
                    ItemKeyword = data.GetString(8),
                    LastUpdateTime = data.GetDateTime(9)
                };
                items.Add(item);
            }
            data.Close();
            return items;
        }
    }
}
