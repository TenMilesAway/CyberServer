using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public class PlayerInfo
{
    public string id;

    public int level;
    public int gold;
    public int gem;
    public int hp;
    public string head;

    public List<ItemInfo> items;
    public List<ItemInfo> equips;
    public List<ItemInfo> potions;
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
