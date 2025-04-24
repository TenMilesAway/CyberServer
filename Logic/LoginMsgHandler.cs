using System;
using CyberServer;

public partial class MsgHandler
{
	public static void MsgLogin(ClientState c, MsgBase msgBase)
	{
		MsgLogin msg = (MsgLogin) msgBase;
		
		// 密码校验
		if (!DBManager.CheckPassword(msg.id, msg.pw))
        {
			msg.result = 1;
			NetManager.Send(c, msg);
			LogService.Error("[服务器] 用户登录失败, " + c.socket.RemoteEndPoint);
			return;
        }

		// 将 PlayerInfo 信息存储在 ClientState 中，便于管理
		c.player = DBManager.SelectPlayerInfo(msg.id);
		msg.result = 0;
		NetManager.Send(c, msg);
		LogService.Info("[服务器] 用户登录成功, " + c.socket.RemoteEndPoint);

		// 分发数据给客户端
	}
}