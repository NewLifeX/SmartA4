namespace SmartA4;

/// <summary>
/// 看门狗。Open打开后，需要定期Feed喂狗，否则系统重启
/// </summary>
/// <remarks>看门狗的超时时间为 10 秒，如果超过 10 秒没有喂狗，系统将自动重启</remarks>
public class Watchdog
{
    /// <summary>
    /// 文件路径
    /// </summary>
    public String FileName { get; set; }

    private FileStream _stream;

    /// <summary>
    /// 打开看门狗
    /// </summary>
    public void Open() => _stream = new FileStream(FileName, FileMode.OpenOrCreate);

    /// <summary>
    /// 关闭看门狗
    /// </summary>
    public void Close()
    {
        if (_stream == null) return;

        var buf = new Byte[1];
        buf[0] = (Byte)'V';

        _stream.Position = 0;
        _stream.Write(buf, 0, buf.Length);

        _stream.Dispose();
        _stream = null;
    }

    /// <summary>
    /// 喂狗
    /// </summary>
    public void Feed()
    {
        //var buf = new Byte[1];
        //buf[0] = (Byte)'1';

        //_stream.Position = 0;
        //_stream.Write(buf, 0, buf.Length);
        // 向看门狗设备写入数据，刷新watchdog计数器
        File.WriteAllText(FileName.GetFullPath(), "1");
    }
}