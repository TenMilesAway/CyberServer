using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// [12/4] 修改 PlayerInfo 以适配最新版本
/// </summary>
[System.Serializable]
public class PlayerInfo
{
    public string _id;                // 玩家 ID
    public string _name;              // 玩家名称
    public string _head;              // 玩家头像 (路径)

    public int _level;                // 玩家等级
    public int _commonCurrency;       // 玩家普通货币数
    public int _rareCurrency;         // 玩家稀有货币数
    public int _maxEXP;               // 玩家最大经验值
    public int _currentEXP;           // 玩家当前经验值
    public int _maxHP;                // 玩家最大血量
    public int _currentHP;            // 玩家当前血量
    public int _maxMP;                // 玩家最大灵力
    public int _currentMP;            // 玩家当前灵力
    public int _pAttack;              // 玩家攻击力
    public int _pArmorPenetration;    // 玩家破甲值
    public int _pDefense;             // 玩家防御值
    public int _pDamageAvoidance;     // 玩家免伤值

    public List<ItemInfo> _items;     // 玩家拥有物品
    public List<ItemInfo> _equips;    // 玩家拥有装备
    public List<ItemInfo> _potions;   // 玩家拥有药水
    public List<ItemInfo> _nowEquips; // 玩家当前已装备
    public int _inventoryItemNum;     // 玩家仓库：物品格子数
    public int _inventoryEquipNum;    // 玩家仓库：装备格子数
    public int _inventoryPotionNum;   // 玩家仓库：药水格子数
    public int _safeboxNum;           // 安全行囊格子数
}

[System.Serializable]
public class PlayerTempInfo
{
    // 用于查找
    public string id;

    // 临时信息 - 坐标
    public float x;
    public float y;
    public float z;
    // 临时信息 - 旋转值
    public float rx;
    public float ry;
    public float rz;
    // 临时信息 - 玩家有限状态
    public string state;
}
