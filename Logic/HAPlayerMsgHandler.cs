using System;
using CyberServer;

public partial class MsgHandler
{
    public static void HAMsgPlayerInfoUpload(ClientState c, MsgBase msgBase)
    {
        HAMsgPlayerInfoUpload msg = (HAMsgPlayerInfoUpload)msgBase;

        // 玩家信息为空
        if (DBManager.SelectPlayerInfo(msg.playerInfo._id) == null)
        {
            // 插入玩家数据
            if (DBManager.InsertPlayerInfo(msg.playerInfo._id, msg.playerInfo))
            {
                // 成功
                msg.result = 0;
                c.player = DBManager.SelectPlayerInfo(msg.playerInfo._id);
                LogService.Info("[服务器] 用户信息插入成功, " + c.socket.RemoteEndPoint);
            }
            else
            {
                // 失败
                msg.result = 1;
                LogService.Error("[服务器] 用户信息插入失败, " + c.socket.RemoteEndPoint);
            }

            NetManager.Send(c, msg);
            return;
        }

        // 玩家信息不为空，更新信息
        if (!DBManager.UpdatePlayerInfo(msg.playerInfo._id, msg.playerInfo))
        {
            msg.result = 1;
            NetManager.Send(c, msg);
            LogService.Error("[服务器] 用户信息更新失败, " + c.socket.RemoteEndPoint);
            return;
        }

        msg.result = 0;
        NetManager.Send(c, msg);
        LogService.Info("[服务器] 用户信息更新成功, " + c.socket.RemoteEndPoint);
    }

    public static void HAMsgPlayerInfoLoad(ClientState c, MsgBase msgBase)
    {
        HAMsgPlayerInfoLoad msg = (HAMsgPlayerInfoLoad)msgBase;

        msg.playerInfo = DBManager.SelectPlayerInfo(msg.playerInfo._id);

        if (msg.playerInfo == null)
        {
            msg.result = 1;
            NetManager.Send(c, msg);
            LogService.Error("[服务器] 用户信息获取失败, " + c.socket.RemoteEndPoint);
            return;
        }

        c.player = DBManager.SelectPlayerInfo(msg.playerInfo._id);
        msg.result = 0;
        NetManager.Send(c, msg);
        LogService.Info("[服务器] 用户信息获取成功, " + c.socket.RemoteEndPoint);
    }
}