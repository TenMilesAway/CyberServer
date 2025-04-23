using System;
using System.Net.Sockets;
using CyberServer;

public partial class MsgHandler
{
	public static void MsgGetPlayerList(ClientState c, MsgBase msgBase)
	{
		MsgGetPlayerList msg = (MsgGetPlayerList)msgBase;

		foreach (ClientState state in NetManager.clients.Values)
		{
			msg.list.Add(state.player);
		}

		// 分发给请求的客户端
		NetManager.Send(c, msg);
	}

	public static void MsgUpdatePlayerEntities(ClientState c, MsgBase msgBase)
    {
		MsgUpdatePlayerEntities msg = (MsgUpdatePlayerEntities)msgBase;

		foreach (ClientState state in NetManager.clients.Values)
		{
			msg.list.Add(state.player);
		}

		// 这里分发给所有客户端，同步更新角色
		foreach (ClientState state in NetManager.clients.Values)
		{
			NetManager.Send(state, msg);
			LogService.Info("[服务器] 将 MsgUpdatePlayerEntities 分发给了 " + state.socket.RemoteEndPoint);
		}
	}

	public static void MsgUploadPlayerTempInfo(ClientState c, MsgBase msgBase)
    {
		MsgUploadPlayerTempInfo msg = (MsgUploadPlayerTempInfo)msgBase;

		// 更新对应 socket 的 tempInfo
		c.tempInfo = msg.tempInfo;
    }

	public static void MsgUpdatePlayerTempInfo(ClientState c, MsgBase msgBase)
    {
		MsgUpdatePlayerTempInfo msg = (MsgUpdatePlayerTempInfo)msgBase;

		// 后面需要加一下每秒限制发送次数

		foreach (ClientState state in NetManager.clients.Values)
        {
			msg.tempInfos.Add(state.tempInfo);
        }

		NetManager.Send(c, msg);
	}
}