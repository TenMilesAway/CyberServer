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
        /// <summary>
        /// 获取指定 id 的 PlayerInfo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 存储指定 id 的 PlayerInfo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="playerInfo"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 更新指定 id 的 PlayerInfo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="playerInfo"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 删除指定 PlayerInfo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
