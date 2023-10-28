using NewLife.Log;
using NewLife.Net;

// 网络服务端，一般跑在服务器或上位机上，用于接收工控机内客户端的数据

XTrace.UseConsole();

var server = new NetServer(777)
{
    Log = XTrace.Log,
    SessionLog = XTrace.Log
};

// 新连接会话事件
server.NewSession += (s, e) =>
{
    var uri = e.Session.Remote;
    XTrace.WriteLine("新会话：{0}", uri);

    var session = e.Session;
    session.Send($"欢迎：{uri}");
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

// 等待退出
Console.ReadLine();