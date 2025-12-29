using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System;

namespace CyberServer
{
    public partial class DBManager
    {
        public static PlayerBaseEntity SelectPlayerBase(string id)
        {
            PlayerBaseEntity entity = null;

            string sql = string.Format("SELECT id, name, head, level, common_currency, rare_currency " +
                                       "FROM player_base WHERE id = '{0}'", id);

            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                entity = new PlayerBaseEntity
                {
                    id = reader["id"].ToString(),
                    name = reader["name"].ToString(),
                    head = reader["head"].ToString(),
                    level = Convert.ToInt32(reader["level"]),
                    common_currency = Convert.ToInt32(reader["common_currency"]),
                    rare_currency = Convert.ToInt32(reader["rare_currency"]),
                };
            }
            reader.Close();

            return entity;
        }

        public static bool InsertPlayerBase(PlayerBaseEntity entity)
        {
            string sql = string.Format("INSERT INTO player_base " +
                                       "(id, name, head, level, common_currency, rare_currency) " +
                                       "VALUES " +
                                       "('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", 
                                       entity.id, entity.name, entity.head, entity.level, entity.common_currency, entity.rare_currency);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] player_base 插入失败: " + e.Message);
                return false;
            }
        }

        public static bool UpdatePlayerBase(PlayerBaseEntity entity)
        {
            string sql = string.Format("UPDATE player_base " +
                                       "SET name = '{0}', " +
                                       "head = '{1}', " +
                                       "level = '{2}', " +
                                       "common_currency = '{3}', " +
                                       "rare_currency = '{4}' " +
                                       "WHERE id = '{5}'",
                                       entity.name, entity.head, entity.level, entity.common_currency, entity.rare_currency, entity.id);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] player_base 更新失败: " + e.Message);
                return false;
            }
        }

        public static bool DeletePlayerBase(string id)
        {
            string sql = string.Format("DELETE FROM player_base WHERE id = '{0}'", id);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] player_base 删除失败: " + e.Message);
                return false;
            }
        }
    }
}
