using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CyberServer
{
	public static class NetManager
	{
		// 监听 Socket
		public static Socket listenfd;
		// 客户端 Socket 及状态信息
		public static Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();
		// Select 检查列表
		static List<Socket> checkRead = new List<Socket>();
		// ping 间隔
		public static long pingInterval = 30;

		public static void StartLoop(int listenPort)
		{
			LogService.Init("NetManager");
			// Socket
			listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			// Bind
			IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
			IPEndPoint ipEp = new IPEndPoint(ipAdr, listenPort);
			listenfd.Bind(ipEp);

			// Listen
			listenfd.Listen(0);
			LogService.Info("[服务器] 启动成功");

			// 循环
			while (true)
			{
				ResetCheckRead();  // 重置 checkRead
				Socket.Select(checkRead, null, null, 1000);

				// 检查可读对象
				for (int i = checkRead.Count - 1; i >= 0; i--)
				{
					Socket s = checkRead[i];
					if (s == listenfd)
					{
						ReadListenfd(s);
					}
					else
					{
						ReadClientfd(s);
					}
				}
				// 定时器
				Timer();
			}
		}

		// 填充 checkRead 列表
		public static void ResetCheckRead()
		{
			checkRead.Clear();

			checkRead.Add(listenfd);

			foreach (ClientState s in clients.Values)
            {
				checkRead.Add(s.socket);
			}
		}

		// 读取 Listenfd
		public static void ReadListenfd(Socket listenfd)
		{
			try
			{
				Socket clientfd = listenfd.Accept();
				LogService.Info("[服务器] 接收到用户连接: " + clientfd.RemoteEndPoint);

				ClientState state = new ClientState();
				state.socket = clientfd;
				state.lastPingTime = GetTimeStamp();
				clients.Add(clientfd, state);
			}
			catch (SocketException ex)
			{
				LogService.Error("[服务器] 用户连接失败: " + ex.ToString());
			}
		}

		// 关闭连接
		public static void Close(ClientState state)
		{
			// 消息分发
			MethodInfo mei = typeof(EventHandler).GetMethod("OnDisconnect");
			object[] ob = { state };
			mei.Invoke(null, ob);

			// 关闭连接
			state.socket.Close();
			clients.Remove(state.socket);

		}

		// 读取 Clientfd
		public static void ReadClientfd(Socket clientfd)
		{
			ClientState state = clients[clientfd];
			ByteArray readBuff = state.readBuff;
			// 接收
			int count = 0;
			// 缓冲区不够，移动；若依旧不够，返回
			// 缓冲区长度只有 1024，单条协议超过缓冲区长度时会发生错误，根据需要调整长度
			if (readBuff.remain <= 0)
			{
				OnReceiveData(state);
				readBuff.MoveBytes();
			};
			if (readBuff.remain <= 0)
			{
				LogService.Error("[服务器] 消息处理失败，消息的长度大于缓冲区的容量");
				Close(state);
				return;
			}

			try
			{
				count = clientfd.Receive(readBuff.bytes, readBuff.writeIdx, readBuff.remain, 0);
			}
			catch (SocketException ex)
			{
				LogService.Error("[服务器] 消息处理失败: " + ex.ToString());
				Close(state);
				return;
			}
			// 客户端关闭
			if (count <= 0)
			{
				LogService.Warning("[服务器] 用户下线，客户端关闭: " + clientfd.RemoteEndPoint.ToString());
				Close(state);
				return;
			}
			// 消息处理
			readBuff.writeIdx += count;
			// 处理二进制消息
			OnReceiveData(state);
			// 移动缓冲区
			readBuff.CheckAndMoveBytes();
		}

		// 数据处理
		public static void OnReceiveData(ClientState state)
		{
            ByteArray readBuff = state.readBuff;

            // 消息长度
            if (readBuff.length <= 2)
            {
                return;
            }

			// 消息体
			Int16 bodyLength = readBuff.ReadInt16();
            if (readBuff.length < bodyLength)
            {
                return;
            }

            // 解析协议名
            int nameCount = 0;
            string protoName = MsgBase.DecodeName(readBuff.bytes, readBuff.readIdx, out nameCount);

            if (protoName == "")
            {
                LogService.Error("[服务器] 处理数据 MsgBase.DecodeName 失败");
                Close(state);
                return;
            }
            readBuff.readIdx += nameCount;

            // 解析协议体
            int bodyCount = bodyLength - nameCount;
            MsgBase msgBase = MsgBase.Decode(protoName, readBuff.bytes, readBuff.readIdx, bodyCount);
            readBuff.readIdx += bodyCount;
            readBuff.CheckAndMoveBytes();

            // 分发消息
            MethodInfo mi = typeof(MsgHandler).GetMethod(protoName);
            object[] o = { state, msgBase };
            LogService.Info("[服务器] 接收到消息: " + protoName);
            if (mi != null)
            {
                mi.Invoke(null, o);
            }
            else
            {
                Console.WriteLine("OnReceiveData Invoke fail " + protoName);
            }
            //继续读取消息
            if (readBuff.length > 2)
            {
                OnReceiveData(state);
            }
        }

		// 发送
		public static void Send(ClientState cs, MsgBase msg)
		{
			// 状态判断
			if (cs == null)
			{
				return;
			}
			if (!cs.socket.Connected)
			{
				return;
			}
			// 数据编码
			byte[] nameBytes = MsgBase.EncodeName(msg);
			byte[] bodyBytes = MsgBase.Encode(msg);
			int len = nameBytes.Length + bodyBytes.Length;
			byte[] sendBytes = new byte[2 + len];

			// 协议格式：
			// 总长度，占 2 字节
			// 消息名长度，占 2 字节
			// 消息名
			// 消息体

			// 组装总长度
			sendBytes[0] = (byte) (len % 256);
			sendBytes[1] = (byte) (len / 256);
			// 组装消息名字 (消息名长度已封装在 EncodeName 方法内)
			Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
			// 组装消息体
			Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);
			// 为简化代码，不设置回调
			try
			{
				cs.socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, null, null);
			}
			catch (SocketException ex)
			{
				LogService.Error("[服务器] Socket 在 BeginSend 时已关闭: " + ex.ToString());
			}
		}

		//定时器
		static void Timer()
		{
			// 消息分发
			MethodInfo mei = typeof(EventHandler).GetMethod("OnTimer");
			object[] ob = { };
			mei.Invoke(null, ob);
		}

		// 获取时间戳
		public static long GetTimeStamp()
		{
			TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return Convert.ToInt64(ts.TotalSeconds);
		}
	}
}