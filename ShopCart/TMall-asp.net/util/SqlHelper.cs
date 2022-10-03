using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace TMall.util
{
    public class SqlHelper
    {
        private SqlHelper() { }

        private static readonly string connStr = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;

        // 返回受影響的行數, 主要用於insert,update,delete等
        public static int ExecuteNoQuery(string sqlString, params SqlParameter[] sqlParameters)
        {
            SqlConnection conn = new SqlConnection(connStr);//資料庫連線物件
            SqlCommand sqlcmd = new SqlCommand(sqlString, conn);//command 對象
            if (sqlParameters != null && sqlParameters.Length > 0)
            {
                sqlcmd.Parameters.AddRange(sqlParameters);//添加執行的參數
            }
            try
            {
                conn.Open();// 建立連接
                return sqlcmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                conn.Close();// 最後關閉連接
            }
        }

        // 返回查詢表格的第一行第一列, 如查詢個數的問題
        public static object ExecuteScalar(string sqlString, params SqlParameter[] sqlParameters)
        {
            SqlConnection conn = new SqlConnection(connStr);//資料庫連線物件
            SqlCommand sqlcmd = new SqlCommand(sqlString, conn);//command 對象
            if (sqlParameters != null && sqlParameters.Length > 0)
            {
                sqlcmd.Parameters.AddRange(sqlParameters);//添加執行的參數
            }
            try
            {
                conn.Open();// 建立連接
                return sqlcmd.ExecuteScalar();//執行
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                conn.Close();// 最後關閉連接
            }
        }

        // 返回查詢的表格
        public static SqlDataReader ExecuteTable(string sqlString, params SqlParameter[] sqlParameters)
        {
            SqlConnection conn = new SqlConnection(connStr);//資料庫連線物件
            SqlCommand sqlcmd = new SqlCommand(sqlString, conn);//command 對象
            if (sqlParameters != null && sqlParameters.Length > 0)
            {
                sqlcmd.Parameters.AddRange(sqlParameters);//添加執行的參數
            }
            try
            {
                conn.Open();// 建立連接
                return sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);//關閉SqlDataReader的時候自動關閉連接
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
