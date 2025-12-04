using System;

namespace CyberServer
{
	public partial class EventHandler
	{
		public static void OnDisconnect(ClientState c)
		{
			// Player 下线

			// 数据保存

			MsgPlayerDisconnect msg = new MsgPlayerDisconnect();

			if (c.player == null) return;
			msg.id = c.player.id;

			// 广播
			foreach (ClientState state in NetManager.clients.Values)
            {
				// 如果不是当前地图，则跳过信息广播
				if (state.mapInfo != c.mapInfo)
					continue;

				NetManager.Send(state, msg);
            }
		}


		public static void OnTimer()
		{
			CheckPing();
		}

		//Ping检查
		public static void CheckPing()
		{
			// 现在的时间戳
			long timeNow = NetManager.GetTimeStamp();

			// 遍历，删除
			foreach (ClientState s in NetManager.clients.Values)
			{
				if (timeNow - s.lastPingTime > NetManager.pingInterval * 4)
				{
					LogService.Error("[服务器] Ping Close " + s.socket.RemoteEndPoint.ToString());
					NetManager.Close(s);
					return;
				}
			}
		}


	}
}

