// 登录注册协议
public class MsgLogin : MsgBase
{
    public MsgLogin() { protoName = "MsgLogin"; }

    public string id = "";
    public string pw = "";

    // 回复 (0 - 成功，1 - 失败)
    public int result = 0;
}

