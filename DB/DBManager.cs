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
    class DBManager
    {
        public static MySqlConnection mysql;

        public static JavaScriptSerializer Js = new JavaScriptSerializer();

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

        // 得到角色数据
        public static PlayerInfo SelectPlayerInfo(string id)
        {
            PlayerInfo playerInfo;

            if (!DBManager.IsSafeString(id))
            {
                LogService.Error("[数据库] 用户 ID 不安全");
                return null;
            }

            string sql = string.Format("select data from player where id = '{0}'", id);

            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            if (!dataReader.HasRows)
            {
                dataReader.Close();
                return null;
            }
                
            // 在 Get 前先 Read
            dataReader.Read();
            if (dataReader.IsDBNull(0))
            {
                dataReader.Close();
                return null;
            }

            string data = dataReader.GetString(0);
            dataReader.Close();

            // 如果存储在数据库中的数据格式不对
            try
            {
                playerInfo = Js.Deserialize<PlayerInfo>(data);
            }
            catch
            {
                // Delete 一下
                DeletePlayerInfo(id);
                return null;
            }

            return playerInfo;
        }

        // 存储角色数据
        public static bool InsertPlayerInfo(string id, PlayerInfo playerInfo)
        {
            if (!DBManager.IsSafeString(id))
            {
                LogService.Error("[数据库] 用户 ID 不安全");
                return false;
            }

            string data = Js.Serialize(playerInfo);

            // 更新写入
            string sql = string.Format("insert into player values('{0}', '{1}')", id, data);

            LogService.Info("[数据库] 语句: " + sql);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] 角色信息插入失败: " + e.Message);
                return false;
            }
        }

        // 更新角色数据
        public static bool UpdatePlayerInfo(string id, PlayerInfo playerInfo)
        {
            if (!DBManager.IsSafeString(id))
            {
                LogService.Error("[数据库] 用户 ID 不安全");
                return false;
            }

            string data = Js.Serialize(playerInfo);

            // 更新写入
            string sql = string.Format("update player set data = '{0}' where id = '{1}'", data, id);

            LogService.Info("[数据库] 语句: " + sql);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] 角色信息更新失败: " + e.Message);
                return false;
            }
        }

        // 删除角色数据
        public static bool DeletePlayerInfo(string id)
        {
            if (!DBManager.IsSafeString(id))
            {
                LogService.Error("[数据库] 用户 ID 不安全");
                return false;
            }

            string sql = string.Format("delete from player where id = '{0}'", id);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] 角色信息删除失败: " + e.Message);
                return false;
            }
        }
    }
}
