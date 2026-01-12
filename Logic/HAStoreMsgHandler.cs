using System;
using CyberServer;

public partial class MsgHandler
{
    public static void HAMsgStoreBuyItem(ClientState c, MsgBase msgBase)
    {
        HAMsgStoreBuyItem msg = (HAMsgStoreBuyItem)msgBase;

        PlayerBaseEntity baseInfo = DBManager.SelectPlayerBase(msg.playerID);

        if (baseInfo == null)
        {
            msg.result = 1;
            NetManager.Send(c, msg);
            LogService.Error("[服务器] 购买物品失败，用户基础信息不存在" + c.socket.RemoteEndPoint);
            return;
        }

        // 检查货币是否足够
        if (msg.currencyType == 0) // 普通货币
        {
            if (baseInfo.common_currency < msg.price)
            {
                msg.result = 1; // 货币不足
                NetManager.Send(c, msg);
                LogService.Info("[服务器] 购买物品失败，普通货币不足" + c.socket.RemoteEndPoint);
                return;
            }
            baseInfo.common_currency -= msg.price;
            DBManager.UpdatePlayerBase(baseInfo);
            msg.result = 0;
            NetManager.Send(c, msg);
        }
        else if (msg.currencyType == 1) // 稀有货币
        {
            if (baseInfo.rare_currency < msg.price)
            {
                msg.result = 1; // 货币不足
                NetManager.Send(c, msg);
                LogService.Info("[服务器] 购买物品失败，稀有货币不足" + c.socket.RemoteEndPoint);
                return;
            }
            baseInfo.rare_currency -= msg.price;
            DBManager.UpdatePlayerBase(baseInfo);
            msg.result = 0;
            NetManager.Send(c, msg);
        }
    }
}