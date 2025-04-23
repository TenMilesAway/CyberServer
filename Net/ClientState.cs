using System.Net.Sockets;

public class ClientState
{
	public Socket socket; 
	public ByteArray readBuff = new ByteArray(); 

	// Ping
	public long lastPingTime = 0;

	// 玩家信息，需入库
	public PlayerInfo player;
	// 临时玩家信息，不入库
	public PlayerTempInfo tempInfo;
}