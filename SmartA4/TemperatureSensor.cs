using NewLife;
using NewLife.IoT.Controllers;

namespace SmartA4;

/// <summary>温湿度传感器</summary>
public class TemperatureSensor
{
    /// <summary>Modbus对象</summary>
    public IModbus Modbus { get; set; }

    /// <summary>主机地址</summary>
    public Byte Host { get; set; } = 1;

    /// <summary>读取数值</summary>
    /// <returns></returns>
    public (Double temp, Double humi) ReadValues()
    {
        var vs = Modbus.ReadRegister(Host, 0x0000, 2);
        var humi = vs[0] / 1000.0;
        var temp = vs[1] / 10.0 - 40;

        return (temp, humi);
    }

    /// <summary>读取从机地址，FF广播</summary>
    /// <returns></returns>
    public UInt16 ReadAddress() => Modbus.ReadRegister(0xFF, 0x0100, 1)[0];

    /// <summary>写从机地址，FF广播</summary>
    /// <param name="value"></param>
    public void WriteAddress(UInt16 value) => Modbus.WriteRegister(0xFF, 0x0100, value);

    /// <summary>写从机波特率，FF广播</summary>
    /// <param name="value"></param>
    public void WriteBaudrate(UInt16 value) => Modbus.WriteRegister(0xFF, 0x0101, value);

    /// <summary>读取唯一标识</summary>
    /// <returns></returns>
    public Byte[] ReadID()
    {
        var vs = Modbus.ReadRegister(Host, 0x0200, 6);
        var buf = new Byte[12];
        // vs中每个元素转为两个字节
        for (var i = 0; i < vs.Length; i++)
        {
            buf[i * 2] = (Byte)(vs[i] >> 8);
            buf[i * 2 + 1] = (Byte)(vs[i] & 0xFF);
        }
        return buf;
    }

    /// <summary>读取硬件和软件版本</summary>
    /// <returns></returns>
    public (String hardware, String software) ReadVersion()
    {
        var vs = Modbus.ReadRegister(Host, 0x0206, 4);
        var buf = new Byte[8];
        // vs中每个元素转为两个字节
        for (var i = 0; i < vs.Length; i++)
        {
            buf[i * 2] = (Byte)(vs[i] >> 8);
            buf[i * 2 + 1] = (Byte)(vs[i] & 0xFF);
        }

        var hardware = $"{buf.ToHex(2, 2)}{buf.ToHex(0, 2)}";
        var software = $"{buf.ToHex(6, 2)}{buf.ToHex(4, 2)}";

        return (hardware, software);
    }
}