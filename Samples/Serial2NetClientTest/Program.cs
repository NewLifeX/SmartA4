using System.IO.Ports;
using NewLife;
using NewLife.Data;
using NewLife.Log;
using NewLife.Net;
using SmartA4;

// 客户端，本地连接串口，远程连接服务端

internal class Program
{
    static SerialPort _serial;
    static ISocketRemote _client;

    private static void Main(string[] args)
    {
        XTrace.UseConsole();

        var host = new A4();

        // 配置并打开串口COM1
        var serial = host.CreateSerial(Coms.COM1, 9600);
        serial.DataReceived += OnReceiveSerial;
        serial.Open();

        // 服务器地址，可保存在配置文件中，支持tcp/udp地址
        var server = "tcp://10.0.2.6:888";
        var uri = new NetUri(server);
        var client = uri.CreateRemote();
        client.Log = XTrace.Log;
        client.Received += OnReceiveSocket;
        client.Open();

        _serial = serial;
        _client = client;

        // 等待退出
        Console.ReadLine();
    }

    static void OnReceiveSerial(Object sender, SerialDataReceivedEventArgs e)
    {
        // 等一会儿，等待数据接收完毕
        Thread.Sleep(10);

        var sp = sender as SerialPort;
        var buf = new Byte[sp.BytesToRead];
        var count = sp.Read(buf, 0, buf.Length);
        if (count <= 0) return;

        // 发送串口数据到服务器
        var pk = new Packet(buf, 0, count);
        _client.Send(pk);
    }

    static void OnReceiveSocket(Object sender, ReceivedEventArgs e)
    {
        // 接收到服务器数据，转发到串口
        var pk = e.Packet;
        _serial.Write(pk.Data, pk.Offset, pk.Count);
    }
}