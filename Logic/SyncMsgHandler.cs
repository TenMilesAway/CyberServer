using System;
using System.Net.Sockets;
using CyberServer;
using System.Collections.Generic;

public partial class MsgHandler
{
	/// <summary>
	/// 获取当前的玩家列表
	/// </summary>
	/// <param name="c"></param>
	/// <param name="msgBase"></param>
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

	/// <summary>
	/// 将玩家列表分发给所有 Socket
	/// </summary>
	/// <param name="c"></param>
	/// <param name="msgBase"></param>
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

	/// <summary>
	/// 更新 ClientState 内存储的信息
	/// </summary>
	/// <param name="c"></param>
	/// <param name="msgBase"></param>
	public static void MsgUploadPlayerTempInfo(ClientState c, MsgBase msgBase)
    {
		MsgUploadPlayerTempInfo msg = (MsgUploadPlayerTempInfo)msgBase;

		// 更新对应 socket 的 tempInfo 和 mapInfo
		c.tempInfo = msg.tempInfo;
		c.mapInfo = msg.mapInfo;

		// 更新了以后分发
		MsgUpdatePlayerTempInfo(c, new MsgUpdatePlayerTempInfo(), c.mapInfo);
    }

	/// <summary>
	/// 分发玩家临时信息
	/// </summary>
	/// <param name="c"></param>
	/// <param name="msgBase"></param>
	public static void MsgUpdatePlayerTempInfo(ClientState c, MsgBase msgBase, Maps mapInfo)
    {
		MsgUpdatePlayerTempInfo msg = (MsgUpdatePlayerTempInfo)msgBase;
		msg.tempInfos = new List<PlayerTempInfo>();

		// 遍历所有 ClientState
		foreach (ClientState state in NetManager.clients.Values)
        {
			// 如果不是当前地图，则跳过信息添加
			if (state.mapInfo != mapInfo)
				continue;

			msg.tempInfos.Add(state.tempInfo);
        }

		foreach (ClientState state in NetManager.clients.Values)
		{
			// 如果不是当前地图，则跳过分发
			if (state.mapInfo != mapInfo)
				continue;

			NetManager.Send(c, msg);
		}
	}
}