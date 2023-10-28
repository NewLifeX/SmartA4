using System.IO.Ports;
using NewLife;
using NewLife.Log;

internal class Program
{
    private static void Main(string[] args)
    {
        XTrace.UseConsole();

        // 硬件串口对照表
        // COM1：/dev/ttyS1
        // COM2：/dev/ttyS2
        // COM3：/dev/ttyS3
        // COM4：/dev/ttyS4

        // 列出所有串口
        var ports = SerialPort.GetPortNames();
        XTrace.WriteLine("串口列表[{0}]：", ports.Length);
        foreach (var port in ports)
        {
            XTrace.WriteLine(port);
        }

        // 配置并打开串口COM1
        var serial = new SerialPort
        {
            PortName = "/dev/ttyS1",
            BaudRate = 9600,
        };
        // 注册数据接收事件
        serial.DataReceived += OnReceive;
        serial.Open();

        // 发送数据
        for (var i = 0; i < 10; i++)
        {
            XTrace.WriteLine("请输入要发送的数据：");
            var input = Console.ReadLine();

            serial.Write(input);
        }

        // 关闭串口
        serial.Close();
    }

    static void OnReceive(Object sender, SerialDataReceivedEventArgs e)
    {
        // 等一会儿，等待数据接收完毕
        Thread.Sleep(10);

        var sp = sender as SerialPort;
        var buf = new Byte[sp.BytesToRead];
        sp.Read(buf, 0, buf.Length);

        XTrace.WriteLine("收到数据：{0}", buf.ToStr());
        //XTrace.WriteLine("收到数据：{0}", buf.ToHex());
    }
}