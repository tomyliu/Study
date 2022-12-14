using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using TMall.util;

namespace TMall.Respository
{
    public class Users
    {
        public const int NormalUser = 1;
        public const int Admin = 0;// users表的level許可權, 1是表示正常用戶, 0是表示管理員


        // 登錄, 檢查用戶名密碼是不是對應
        public static bool login(string username, string passwd)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username),
                new SqlParameter("@Passwd",passwd)
            };
            string sql = "select count(*) from users where username = @Username and passwd = @Passwd";
            return 1 == (int)SqlHelper.ExecuteScalar(sql, sqlParameters);
        }
        // 註冊, 將新使用者保存到資料庫, 如果有錯誤返回false
        public static bool register(TMall.Models.UserProfileModel profileModel)
        {
            SqlParameter[] sqlParameter = new SqlParameter[] {
                new SqlParameter("@Username",profileModel.Username),
                new SqlParameter("@Passwd",profileModel.Passwd),
                new SqlParameter("@Email",profileModel.Email),
                new SqlParameter("@Level",profileModel.Level),
                new SqlParameter("@Address",profileModel.Address),
                new SqlParameter("@Birthday",profileModel.Birthday),
                new SqlParameter("@PhoneNumber",profileModel.PhoneNumber),
                new SqlParameter("@RegisterTime",profileModel.RegisterTime),
                new SqlParameter("@QQNumber",profileModel.QQNumber),
            };
            string sql = "insert into users values (@Username,@Passwd,@Email,@Level,@Address,@Birthday,@PhoneNumber,@RegisterTime,@QQNumber)";
            return 1 == (int)SqlHelper.ExecuteNoQuery(sql, sqlParameter);
        }
        // 查詢此用戶名是不是已經註冊
        public static bool hasRegistered(string username)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username)
            };
            string sql = "select count(*) from users where username = @Username";
            return 1 == (int)SqlHelper.ExecuteScalar(sql, sqlParameters);
        }
        // 判斷用戶是不是管理員
        public static bool isAdmin(string username)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username)
            };
            string sql = "select level from users where username = @Username";
            return Admin == (int)SqlHelper.ExecuteScalar(sql, sqlParameters);
        }

        // 獲取使用者資訊
        public static Models.UserProfileModel GetUsers(string username)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username)
            };
            string sql = "select * from users where username = @Username";
            SqlDataReader sqlData = SqlHelper.ExecuteTable(sql, sqlParameters);
            if (sqlData != null && sqlData.Read())
            {
                Models.UserProfileModel user = new Models.UserProfileModel
                {
                    Username = sqlData.GetString(0),
                    Passwd = sqlData.GetString(1),
                    Email = sqlData.GetString(2),
                    Level = sqlData.GetInt32(3),
                    Address = sqlData.IsDBNull(4) ? null : sqlData.GetString(4),
                    Birthday = sqlData.IsDBNull(5) ? DateTime.Now : sqlData.GetDateTime(5),
                    PhoneNumber = sqlData.IsDBNull(6) ? null : sqlData.GetString(6),
                    RegisterTime = sqlData.GetDateTime(7),
                    QQNumber = sqlData.IsDBNull(8) ? 0 : sqlData.GetInt32(8)
                };
                sqlData.Close();
                return user;
            }
            return null;
        }

        // 更新使用者資訊
        public static bool UpdateUsers(Models.UserProfileModel profileModel)
        {
            SqlParameter[] sqlParameter = new SqlParameter[] {
                new SqlParameter("@Username",profileModel.Username),
                new SqlParameter("@Passwd",profileModel.Passwd),
                new SqlParameter("@Email",profileModel.Email),
                new SqlParameter("@Level",profileModel.Level),
                new SqlParameter("@Address",profileModel.Address),
                new SqlParameter("@Birthday",profileModel.Birthday),
                new SqlParameter("@PhoneNumber",profileModel.PhoneNumber),
                new SqlParameter("@RegisterTime",profileModel.RegisterTime),
                new SqlParameter("@QQNumber",profileModel.QQNumber),
            };
            string sql = "update users set passwd=@Passwd,email=@Email,level=@Level,address=@Address,birthday=@Birthday,phone_number=@PhoneNumber,register_time=@RegisterTime,qq_number=@QQNumber where username = @Username";
            return 1 == (int)SqlHelper.ExecuteNoQuery(sql, sqlParameter);
        }

        // 獲取用戶收藏列表
        public static List<Models.CollectModel> GetCollects(string username)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username),
            };
            string sql = "select item.item_id,item_name,item_picture,item_price,item_sales,item_number,collect_time from item inner join collection on item.item_id = collection.item_id where collection.username = @Username";
            SqlDataReader sqlData = SqlHelper.ExecuteTable(sql, sqlParameters);
            if (sqlData == null) return null;
            List<Models.CollectModel> result = new List<Models.CollectModel>();
            while (sqlData.Read())
            {
                Models.CollectModel model = new Models.CollectModel();
                model.ItemId = sqlData.GetInt32(0);
                model.ItemName = sqlData.GetString(1);
                model.ItemPicture = sqlData.GetString(2);
                model.ItemPrice = sqlData.GetDouble(3);
                model.ItemSales = sqlData.GetInt32(4);
                model.ItemNumber = sqlData.GetInt32(5);
                model.CollectTime = sqlData.GetDateTime(6);
                result.Add(model);
            }
            sqlData.Close();
            return result;
        }

        // 獲取用戶是否收藏某商品
        public static bool JudgeCollect(string username, int item_id)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username),
                new SqlParameter("@Item_id",item_id),
            };
            string sql = "select count(*) from collection where username = @Username and item_id = @Item_id";
            return 1 == (int)SqlHelper.ExecuteScalar(sql, sqlParameters);
        }

        // 新增收藏
        public static bool AddCollect(string username, int item_id)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username),
                new SqlParameter("@Item_id",item_id),
                new SqlParameter("@Now",DateTime.Now),
            };
            string sql = "insert into collection values(@Username,@Item_id,@Now)";
            return 1 == SqlHelper.ExecuteNoQuery(sql, sqlParameters);
        }

        // 取消收藏
        public static bool DelCollect(string username, int item_id)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username),
                new SqlParameter("@Item_id",item_id),
            };
            string sql = $"delete from collection where username = @Username and item_id = @Item_id";
            return 1 == SqlHelper.ExecuteNoQuery(sql, sqlParameters);
        }

        // 獲取購物車列表
        public static List<Models.CartModel> GetCarts(string username)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username)
            };
            string sql = "select item.item_id,item_name,item_picture,item_price,item_sales,item_number,cart_item_number,cart_intime from item inner join cart on item.item_id = cart.item_id where cart.username = @Username";
            SqlDataReader sqlData = SqlHelper.ExecuteTable(sql, sqlParameters);
            if (sqlData == null) return null;
            List<Models.CartModel> result = new List<Models.CartModel>();
            while (sqlData.Read())
            {
                Models.CartModel model = new Models.CartModel();
                model.ItemId = sqlData.GetInt32(0);
                model.ItemName = sqlData.GetString(1);
                model.ItemPicture = sqlData.GetString(2);
                model.ItemPrice = sqlData.GetDouble(3);
                model.ItemSales = sqlData.GetInt32(4);
                model.ItemNumber = sqlData.GetInt32(5);
                model.CartItemNumber = sqlData.GetInt32(6);
                model.CartInTime = sqlData.GetDateTime(7);
                result.Add(model);
            }
            sqlData.Close();
            return result;
        }

        // 獲取購物車內有沒有這個商品
        public static bool JudgeCart(string username, int item_id)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username),
                new SqlParameter("@Item_id",item_id)
            };
            string sql = "select count(*) from cart where username = @Username and item_id = @Item_id";
            return 1 == (int)SqlHelper.ExecuteScalar(sql, sqlParameters);
        }

        // 新增購物車
        public static bool AddCart(string username, int item_id, int number)
        {
            if (number <= 0) return false;
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username),
                new SqlParameter("@Item_id",item_id),
                new SqlParameter("@Number",number),
                new SqlParameter("@Time",DateTime.Now)
            };
            string sql = "insert into cart values(@Username,@Item_id,@Time,@Number)";
            return 1 == (int)SqlHelper.ExecuteNoQuery(sql, sqlParameters);
        }

        // 從購物車內刪除這個商品
        public static bool DelCart(string username, int item_id)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username),
                new SqlParameter("@Item_id",item_id),
            };
            string sql = $"delete from cart where username = @Username and item_id = @Item_id";
            return 1 == SqlHelper.ExecuteNoQuery(sql, sqlParameters);
        }

        // 更改購物車內的數量, 在原先的基礎上增加addNumber
        public static bool UpdateCartAdd(string username, int item_id, int addNumber)
        {
            if (addNumber <= 0) return false;
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username),
                new SqlParameter("@Item_id",item_id),
                new SqlParameter("@AddNumber",addNumber),
            };
            string sql = "update cart set cart_item_number=cart_item_number+@AddNumber where username=@Username and item_id=@Item_id";
            return 1 == (int)SqlHelper.ExecuteNoQuery(sql, sqlParameters);
        }
        // 更改購物車內某個的數量, 改成number
        public static bool UpdateCartSet(string username, int item_id, int number)
        {
            if (number <= 0) return false;
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username),
                new SqlParameter("@Item_id",item_id),
                new SqlParameter("@Number",number),
            };
            string sql = "update cart set cart_item_number=@Number where username=@Username and item_id=@Item_id";
            return 1 == (int)SqlHelper.ExecuteNoQuery(sql, sqlParameters);
        }


        // 獲取訂單清單
        public static List<Models.OrdersModel> GetOrders(string username)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username",username)
            };
            string sql = "select * from orders where username = @Username";
            SqlDataReader data = SqlHelper.ExecuteTable(sql, sqlParameters);
            if (data == null) return null;
            List<Models.OrdersModel> result = new List<Models.OrdersModel>();
            while (data.Read())
            {
                Models.OrdersModel order = new Models.OrdersModel();
                order.OrderId = data.GetInt32(0);
                order.ItemId = data.GetInt32(1);
                order.Username = data.GetString(2);
                order.OrderNum = data.GetInt32(3);
                order.OrderPrice = data.GetDouble(4);
                order.OrderStatus = data.GetString(5);
                order.OrderCreateTime = data.GetDateTime(6);
                result.Add(order);
            }
            return result;
        }

        // 新建訂單, 並且返回訂單
        public static Models.OrdersModel AddOrders(string username, int item_id, int num, double price)
        {
            Models.OrdersModel orders = new Models.OrdersModel();
            orders.OrderCreateTime = DateTime.Now;
            orders.OrderNum = num;
            orders.OrderPrice = price;
            orders.ItemId = item_id;
            orders.OrderStatus = "未支付";
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Username", username),
                new SqlParameter("@ItemId",item_id),
                new SqlParameter("@Num",num),
                new SqlParameter("@Price",price),
                new SqlParameter("@Status",orders.OrderStatus),
                new SqlParameter("@Time",orders.OrderCreateTime)
            };
            string sql = "insert into orders (item_id,username,order_num,order_price,order_status,order_create_time) values(@ItemId,@Username,@Num,@Price,@Status,@Time)\r\n select SCOPE_IDENTITY()\r\n go";
            orders.OrderId = Convert.ToInt32(SqlHelper.ExecuteScalar(sql, sqlParameters));
            return orders;
        }

        // 改變某個訂單的狀態
        public static bool ChangeOrderStatus(int order_id, string status)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@A",order_id),
                new SqlParameter("@B",status)
            };
            string sql = "update orders set order_status = @B where order_id = @A";
            return 1 == (int)SqlHelper.ExecuteNoQuery(sql, sqlParameters);
        }

        // 獲取某個商品的單價
        public static double GetItemPrice(int item_id)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@A",item_id)
            };
            string sql = "select item_price from item where item_id = @A";
            return Convert.ToDouble(SqlHelper.ExecuteScalar(sql, sqlParameters));
        }

        // 獲取某個物品在購物車的數量
        public static int GetNumberInCart(string username, int item_id)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@A",username),
                new SqlParameter("@B",item_id)
            };
            string sql = "select cart_item_number from cart where username=@A and item_id=@B";
            return Convert.ToInt32(SqlHelper.ExecuteScalar(sql, sqlParameters));
        }

        // 清理未完成超時的訂單
        public static void OrdersClear()
        {
            // 刪除一天前的未支付訂單
            string sql = "delete from orders where order_status='未支付' and order_create_time < getdate()-1";
            SqlHelper.ExecuteNoQuery(sql, null);
        }
    }
}
