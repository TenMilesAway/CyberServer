using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace CyberServer
{
    public partial class DBManager
    {
        public static MySqlConnection mysql;

        public static JavaScriptSerializer Js = new JavaScriptSerializer();

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="db"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        public static bool Connect(string db, string ip, int port, string user, string pw)
        {
            mysql = new MySqlConnection();

            // 这里大小写和空格并不重要，会自动矫正
            string s = string.Format("Database = {0}; DataSource = {1}; port = {2}; Userid = {3}; Password = {4}", db, ip, port, user, pw);

            mysql.ConnectionString = s;

            try
            {
                mysql.Open();
                return true;
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] 连接失败原因, " + e.Message);
                return false;
            }
        }
    }
}
