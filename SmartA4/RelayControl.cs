using NewLife;
using NewLife.IoT.Protocols;

namespace SmartA4;

/// <summary>继电器控制板</summary>
public class RelayControl
{
    /// <summary>Modbus对象</summary>
    public Modbus Modbus { get; set; }

    /// <summary>主机地址</summary>
    public Byte Host { get; set; } = 1;

    /// <summary>控制指定点位</summary>
    /// <param name="address"></param>
    /// <param name="flag"></param>
    public void Write(Int32 address, Boolean flag) => Modbus.WriteCoil(Host, (UInt16)address, (UInt16)(flag ? 0xFF00 : 0x0000));

    /// <summary>翻转指定点位</summary>
    /// <param name="address"></param>
    public void Invert(Int32 address) => Modbus.WriteCoil(Host, (UInt16)address, 0x5500);

    /// <summary>控制指定点位</summary>
    /// <param name="flag"></param>
    public void WriteAll(Boolean flag) => Modbus.WriteCoil(Host, 0x00FF, (UInt16)(flag ? 0xFFFF : 0x0000));

    /// <summary>翻转指定点位</summary>
    public void InvertAll() => Modbus.WriteCoil(Host, 0x00FF, 0x5a00);

    /// <summary>读取指定点位</summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public UInt16 Read(Int32 address) => Modbus.ReadCoil(Host, (UInt16)address, 1).ReadUInt16(false);

    /// <summary>读取所有点位</summary>
    /// <returns></returns>
    public UInt16 ReadAll() => Modbus.ReadCoil(Host, 0x00FF, 0).ReadUInt16(false);
}