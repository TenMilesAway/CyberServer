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
public class ItemInfo
{
    public int id;
    public int num;
}
