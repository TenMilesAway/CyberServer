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
    public string _id;
    public string _name;
    public string _head;

    public int _level;
    public int _commonCurrency;
    public int _rareCurrency;
    public int _maxHP;
    public int _currentHP;
    public int _maxMP;
    public int _currentMP;
    public int _maxEXP;
    public int _currentEXP;

    public List<ItemInfo> _items;
    public List<ItemInfo> _equips;
    public List<ItemInfo> _potions;
    public List<ItemInfo> _nowEquips;
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
