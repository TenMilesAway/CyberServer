using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System;
using System.Collections.Generic;

namespace CyberServer
{
    public partial class DBManager
    {
        public static PlayerInventoryEntity SelectPlayerInventory(string id)
        {
            PlayerInventoryEntity entity = null;

            string sql = string.Format("SELECT * from player_inventory WHERE player_id = '{0}'", id);

            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                entity = new PlayerInventoryEntity
                {
                    player_id = reader["player_id"].ToString(),
                    items = Js.Deserialize<List<ItemInfo>>(reader["items"].ToString()),
                    now_equips = Js.Deserialize<List<ItemInfo>>(reader["now_equips"].ToString()),
                    inventory_num = Convert.ToInt32(reader["inventory_num"]),
                    safebox_num = Convert.ToInt32(reader["safebox_num"]),
                };
            }
            reader.Close();

            return entity;
        }

        public static bool InsertPlayerInventory(PlayerInventoryEntity entity)
        {
            string sql = string.Format("INSERT INTO player_inventory " +
                                       "(player_id, items, now_equips, inventory_num, safebox_num) " +
                                       "VALUES " +
                                       "('{0}', '{1}', '{2}', '{3}', '{4}')",
                                       entity.player_id, Js.Serialize(entity.items), Js.Serialize(entity.now_equips), entity.inventory_num, entity.safebox_num);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] player_inventory 插入失败: " + e.Message);
                return false;
            }
        }

        public static bool UpdatePlayerInventory(PlayerInventoryEntity entity)
        {
            string sql = string.Format("UPDATE player_inventory " +
                                       "SET items = '{0}', " +
                                       "now_equips = '{1}', " +
                                       "inventory_num = '{2}', " +
                                       "safebox_num = '{3}' " +
                                       "WHERE player_id = '{4}'",
                                       Js.Serialize(entity.items), Js.Serialize(entity.now_equips), entity.inventory_num, entity.safebox_num, entity.player_id);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] player_inventory 更新失败: " + e.Message);
                return false;
            }
        }

        public static bool DeletePlayerInventory(string id)
        {
            string sql = string.Format("DELETE FROM player_inventory WHERE player_id = '{0}'", id);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                LogService.Error("[数据库] player_inventory 删除失败: " + e.Message);
                return false;
            }
        }
    }
}
