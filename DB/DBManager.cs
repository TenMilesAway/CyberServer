using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace CyberServer
{
    class DBManager
    {
        public static MySqlConnection mysql;

        // 连接数据库
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

        private static bool IsSafeString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        // 检测用户名密码
        public static bool CheckPassword(string id, string pw)
        {
            // 防 SQL 注入
            if (!DBManager.IsSafeString(id))
            {
                LogService.Error("[数据库] 密码验证失败: 用户名不安全");
                return false;
            }
            if (!DBManager.IsSafeString(pw))
            {
                LogService.Error("[数据库] 密码验证失败: 密码不安全");
                return false;
            }
            // 查询
            string sql = string.Format("select * from account where id='{0}' and pw='{1}';", id, pw);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                bool hasRows = dataReader.HasRows;
                dataReader.Close();
                return hasRows;
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] 密码验证失败: " + e.Message);
                return false;
            }
        }
    }
}
