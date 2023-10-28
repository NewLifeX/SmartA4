using NewLife;
using NewLife.Log;
using NewLife.Net;

// 网络客户端，一般跑在工控机上，充当硬件设备到服务器之间的桥梁

XTrace.UseConsole();

// 支持tcp/udp地址
XTrace.WriteLine("请输入要连接的服务器：");
var server = Console.ReadLine();
if (server.IsNullOrEmpty()) server = "tcp://10.0.2.6:777";

var uri = new NetUri(server);
var client = uri.CreateRemote();
client.Log = XTrace.Log;
client.LogSend = true;
client.LogReceive = true;

// 在事件中接收数据
client.Received += (s, e) =>
{
    XTrace.WriteLine("收到数据：{0}", e.Packet.ToStr());
};
client.Open();

// 发送数据
for (var i = 0; i < 10; i++)
{
    XTrace.WriteLine("请输入要发送的数据：");
    var input = Console.ReadLine();

    client.Send(input);
}
