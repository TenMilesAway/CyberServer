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
        public static PlayerStatsEntity SelectPlayerStats(string id)
        {
            PlayerStatsEntity entity = null;

            string sql = string.Format("SELECT * FROM player_stats WHERE player_id = '{0}'", id);

            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                entity = new PlayerStatsEntity
                {
                    player_id = reader["player_id"].ToString(),
                    max_hp = Convert.ToInt32(reader["max_hp"]),
                    max_mp = Convert.ToInt32(reader["max_mp"]),
                    max_exp = Convert.ToInt32(reader["max_exp"]),
                    current_hp = Convert.ToInt32(reader["current_hp"]),
                    current_mp = Convert.ToInt32(reader["current_mp"]),
                    current_exp = Convert.ToInt32(reader["current_exp"]),
                    attack = Convert.ToInt32(reader["attack"]),
                    armor_penetration = Convert.ToInt32(reader["armor_penetration"]),
                    defense = Convert.ToInt32(reader["defense"]),
                    damage_avoidance = Convert.ToInt32(reader["damage_avoidance"]),
                    critical_probability = Convert.ToInt32(reader["critical_probability"]),
                    critical_multiplier = Convert.ToInt32(reader["critical_multiplier"]),
                    suck_probability = Convert.ToInt32(reader["suck_probability"]),
                    suck_multiplier = Convert.ToInt32(reader["suck_multiplier"])
                };
            }
            reader.Close();

            return entity;
        }

        public static bool InsertPlayerStats(PlayerStatsEntity entity)
        {
            string sql = string.Format("INSERT INTO player_stats " +
                                       "(player_id, max_hp, max_mp, max_exp, current_hp, current_mp, current_exp, " +
                                       "attack, armor_penetration, defense, damage_avoidance, critical_probability, " +
                                       "critical_multiplier, suck_probability, suck_multiplier) " +
                                       "VALUES " +
                                       "('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}')",
                                       entity.player_id, entity.max_hp, entity.max_mp, entity.max_exp, entity.current_hp, entity.current_mp,
                                       entity.current_exp, entity.attack, entity.armor_penetration, entity.defense, entity.damage_avoidance,
                                       entity.critical_probability, entity.critical_multiplier, entity.suck_probability, entity.suck_multiplier);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] player_stats 插入失败: " + e.Message);
                return false;
            }
        }

        public static bool UpdatePlayerStats(PlayerStatsEntity entity)
        {
            string sql = string.Format("UPDATE player_stats " +
                                       "SET max_hp = '{0}', " +
                                       "max_mp = '{1}', " +
                                       "max_exp = '{2}', " +
                                       "current_hp = '{3}', " +
                                       "current_mp = '{4}', " +
                                       "current_exp = '{5}', " +
                                       "attack = '{6}', " +
                                       "armor_penetration = '{7}', " +
                                       "defense = '{8}', " +
                                       "damage_avoidance = '{9}', " +
                                       "critical_probability = '{10}', " +
                                       "critical_multiplier = '{11}', " +
                                       "suck_probability = '{12}', " +
                                       "suck_multiplier = '{13}' " +
                                       "WHERE player_id = '{14}'", 
                                       entity.max_hp, entity.max_mp, entity.max_exp, entity.current_hp, entity.current_mp, entity.current_exp,
                                       entity.attack, entity.armor_penetration, entity.defense, entity.damage_avoidance, entity.critical_probability,
                                       entity.critical_multiplier, entity.suck_probability, entity.suck_multiplier, entity.player_id);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] player_stats 更新失败: " + e.Message);
                return false;
            }
        }

        public static bool DeletePlayerStats(string id)
        {
            string sql = string.Format("DELETE FROM player_stats WHERE player_id = '{0}'", id);

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
