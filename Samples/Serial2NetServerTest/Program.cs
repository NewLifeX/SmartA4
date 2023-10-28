using NewLife.Log;
using NewLife.Net;

// 网络服务端，接收客户端数据

XTrace.UseConsole();

// 应用层连接字典
var users = new Dictionary<String, Int32>();

var server = new NetServer(888)
{
    Log = XTrace.Log,
    SessionLog = XTrace.Log
};

// 新连接会话事件
server.NewSession += (s, e) =>
{
    var uri = e.Session.Remote;
    XTrace.WriteLine("新会话：{0}", uri);

    // 记录该设备IP，后面通过IP找到对应会话并下发数据
    var session = e.Session;
    users[uri.Address + ""] = session.ID;
};

// 在事件中接收数据
server.Received += (s, e) =>
{
    var msg = e.Packet.ToStr();
    XTrace.WriteLine("收到数据：{0}", msg);

    // 倒序返回
    var session = s as INetSession;
    var cs = msg.Reverse().ToArray();
    session.Send(new String(cs));
};

server.Start();

// 应用层向指定设备下发数据
var ip = "10.0.2.6";
if (users.TryGetValue(ip, out var id))
{
    // 根据ID找到对应会话，如果会话不存在，可能是设备已经断开
    var session = server.GetSession(id);
    if (session != null)
    {
        var msg = "Hello " + ip;
        session.Send(msg);
        XTrace.WriteLine("向[{0}]发送数据[{1}]", ip, msg);
    }
}

// 等待退出
Console.ReadLine();