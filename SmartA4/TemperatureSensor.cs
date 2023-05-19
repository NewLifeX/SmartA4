using NewLife;
using NewLife.IoT.Protocols;

namespace SmartA4;

/// <summary>温湿度传感器</summary>
public class TemperatureSensor
{
    /// <summary>Modbus对象</summary>
    public Modbus Modbus { get; set; }

    /// <summary>主机地址</summary>
    public Byte Host { get; set; } = 1;

    /// <summary>读取数值</summary>
    /// <returns></returns>
    public (Double temp, Double humi) ReadValues()
    {
        var buf = Modbus.ReadRegister(Host, 0x0000, 2);
        var humi = (Int16)((buf[0] << 8) + buf[1]) / 1000.0;
        var temp = ((buf[2] << 8) + buf[3]) / 10.0 - 40;

        return (temp, humi);
    }

    /// <summary>读取从机地址，FF广播</summary>
    /// <returns></returns>
    public UInt16 ReadAddress() => Modbus.ReadRegister(0xFF, 0x0100, 1).ReadUInt16(false);

    /// <summary>写从机地址，FF广播</summary>
    /// <param name="value"></param>
    public void WriteAddress(UInt16 value) => Modbus.WriteRegister(0xFF, 0x0100, value);

    /// <summary>写从机波特率，FF广播</summary>
    /// <param name="value"></param>
    public void WriteBaudrate(UInt16 value) => Modbus.WriteRegister(0xFF, 0x0101, value);

    /// <summary>读取唯一标识</summary>
    /// <returns></returns>
    public Byte[] ReadID() => Modbus.ReadRegister(Host, 0x0200, 6)?.ReadBytes();

    /// <summary>读取硬件和软件版本</summary>
    /// <returns></returns>
    public (String hardware, String software) ReadVersion()
    {
        var buf = Modbus.ReadRegister(Host, 0x0206, 4).ReadBytes();
        var hardware = $"{buf.ToHex(2, 2)}{buf.ToHex(0, 2)}";
        var software = $"{buf.ToHex(6, 2)}{buf.ToHex(4, 2)}";

        return (hardware, software);
    }
}