using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Player
{
    public string id = "";

    // 指向 ClientState，方便做一对一操作
    public ClientState state;

    // 临时数据 (坐标)
    public int x;
    public int y;
    public int z;
}