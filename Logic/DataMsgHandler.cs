using System;
using CyberServer;

public partial class MsgHandler
{
	public static void MsgPlayerDataSave(ClientState c, MsgBase msgBase)
	{
		MsgPlayerDataSave msg = (MsgPlayerDataSave) msgBase;

		if (!DBManager.UpdatePlayerInfo(msg.playerInfo.id, msg.playerInfo))
        {
			msg.result = 1;
			NetManager.Send(c, msg);
			LogService.Error("[服务器] 用户信息更新失败");
			return;
		}

		msg.result = 0;
		NetManager.Send(c, msg);
		LogService.Info("[服务器] 用户信息更新成功");
	}

	public static void MsgPlayerDataLoad(ClientState c, MsgBase msgBase)
    {
		MsgPlayerDataLoad msg = (MsgPlayerDataLoad) msgBase;

		msg.playerInfo = DBManager.GetPlayerInfo(msg.playerInfo.id);

		if (msg.playerInfo == null)
        {
			msg.result = 1;
			NetManager.Send(c, msg);
			LogService.Error("[服务器] 用户信息获取失败");
			return;
		}

		msg.result = 0;
		NetManager.Send(c, msg);
		LogService.Info("[服务器] 用户信息获取成功");
	}
}