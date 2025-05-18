using System.ComponentModel;
using NewLife;
using NewLife.IoT.Controllers;
using NewLife.Serial.Protocols;

namespace SmartA4;

/// <summary>串口</summary>
public enum Coms
{
    /// <summary>COM1</summary>
    COM1,

    /// <summary>COM2</summary>
    COM2,

    /// <summary>COM3</summary>
    COM3,

    /// <summary>COM4</summary>
    COM4,
}

/// <summary>A2硬件工厂</summary>
public class A4 : Board
{
    #region 属性
    /// <summary>SYS LED 指示灯</summary>
    [DisplayName("指示灯")]
    public IOutputPort Led { get; } = new FileOutputPort("/dev/led");

    /// <summary>SYS LED 指示灯</summary>
    [DisplayName("指示灯")]
    public IOutputPort Led2 { get; } = new FileOutputPort("/dev/led2");

    /// <summary>SYS LED 指示灯</summary>
    [DisplayName("指示灯")]
    public IOutputPort Led3 { get; } = new FileOutputPort("/dev/led3");

    /// <summary>蜂鸣器</summary>
    [DisplayName("蜂鸣器")]
    public IOutputPort Buzzer { get; } = new FileOutputPort("/dev/buzzer");

    /// <summary>FUN按键</summary>
    [DisplayName("FUN按键")]
    public IInputPort Key { get; } = new FileInputPort("/dev/key");

    /// <summary>USB电源</summary>
    [DisplayName("USB电源")]
    public IOutputPort UsbPower { get; } = new FileOutputPort("/dev/usbpwr");

    /// <summary>看门狗</summary>
    public Watchdog Watchdog { get; } = new Watchdog { FileName = "/dev/watchdog" };

    /// <summary>串口名</summary>
    public String[] ComNames = ["/dev/ttyS1", "/dev/ttyS2", "/dev/ttyS3", "/dev/ttyS4"];
    #endregion

    #region 端口
    /// <summary>创建输出口</summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public override IOutputPort CreateOutput(String name)
    {
        if (name.EndsWithIgnoreCase("led")) return Led;
        if (name.EndsWithIgnoreCase("buzzer")) return Buzzer;
        if (name.EndsWithIgnoreCase("usbpwr")) return UsbPower;

        return base.CreateOutput(name);
    }

    /// <summary>创建输入口</summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public override IInputPort CreateInput(String name)
    {
        if (name.EndsWithIgnoreCase("key")) return Key;

        return base.CreateInput(name);
    }
    #endregion

    #region 串口
    /// <summary>创建串口</summary>
    /// <param name="portName"></param>
    /// <param name="baudrate"></param>
    /// <returns></returns>
    public override ISerialPort CreateSerial(String portName, Int32 baudrate = 9600) => new DefaultSerialPort { PortName = portName, Baudrate = baudrate };

    /// <summary>创建串口</summary>
    /// <param name="com">串口COM1/COM2/COM3/COM4，全部支持RS485，其中COM3/COM4复用RS232</param>
    /// <param name="baudrate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public ISerialPort CreateSerial(Coms com, Int32 baudrate = 9600)
    {
        if (com < 0 || com > Coms.COM4) throw new ArgumentOutOfRangeException(nameof(com), $"无效串口{com}，支持COM1/COM2/COM3/COM4");

        //return new SerialPort(ComNames[(Int32)com], baudrate);
        return new DefaultSerialPort { PortName = ComNames[(Int32)com], Baudrate = baudrate };
    }

    /// <summary>创建Modbus</summary>
    /// <param name="portName"></param>
    /// <param name="baudrate"></param>
    /// <returns></returns>
    public override IModbus CreateModbus(String portName, Int32 baudrate = 9600) => new ModbusRtu { PortName = portName, Baudrate = baudrate };

    /// <summary>创建Modbus</summary>
    /// <param name="com"></param>
    /// <param name="baudrate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public IModbus CreateModbus(Coms com, Int32 baudrate = 9600)
    {
        if (com < 0 || com > Coms.COM4) throw new ArgumentOutOfRangeException(nameof(com), $"无效串口{com}，支持COM1/COM2/COM3/COM4");

        return new ModbusRtu { PortName = ComNames[(Int32)com], Baudrate = baudrate };
    }
    #endregion

    #region 方法
    /// <summary>
    /// 设置主机名
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void SetHostName(String name)
    {
        name = name?.Trim();
        if (name.IsNullOrEmpty()) throw new ArgumentNullException(nameof(name));

        // 修改主机名
        var file = "/etc/hostname";
        File.WriteAllText(file, name + Environment.NewLine);

        // 在hosts文件中添加主机名解析
        file = "/etc/hosts";
        if (File.Exists(file))
        {
            var lines = File.ReadAllLines(file);
            var flag = false;
            foreach (var line in lines)
            {
                var ss = line.Split(' ', '\t');
                if (ss[0] == "127.0.0.1" && ss[^1] == name)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag) File.AppendAllText(file, $"\r\n127.0.0.1\t{name}\r\n");
        }
    }
    #endregion
}
