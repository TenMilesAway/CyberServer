public class HAMsgStoreBuyItem : MsgBase
{
    public HAMsgStoreBuyItem() { protoName = "HAMsgStoreBuyItem"; }

    // 目前未在服务端进行校验
    public string playerID;
    public int price;
    public int currencyType; // 0灵晶币 1灵星石
    public int itemID;
    public int itemNum;

    /// <summary>
    /// 回复: 0成功 1失败
    /// </summary>
    public int result = 0;
}