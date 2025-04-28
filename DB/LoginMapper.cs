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
        private static bool IsSafeString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 检测用户名密码是否对应
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
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

        public static bool CheckRegister(string id, string pw)
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

            // 查询是否有重复用户
            string sql = string.Format("select * from account where id='{0}';", id);

            bool hasRows;

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                hasRows = dataReader.HasRows;
                dataReader.Close();
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] 注册发生异常错误: " + e.Message);
                return false;
            }

            if (hasRows)
            {
                LogService.Warning("[数据库] 该用户名已存在");
                return false;
            }

            sql = string.Format("insert into account values('{0}', '{1}')", id, pw);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                cmd.ExecuteNonQuery();
                LogService.Info("[数据库] 用户注册成功");
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] 注册发生异常错误: " + e.Message);
                return false;
            }

            return true;
        }
    }
}
