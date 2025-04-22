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
			LogService.Error("[服务器] 用户登录失败");
			return;
        }

		msg.result = 0;
		NetManager.Send(c, msg);
		LogService.Info("[服务器] 用户登录成功");
	}
}