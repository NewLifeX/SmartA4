using NewLife;

namespace SmartA4;

/// <summary>
/// 看门狗。Open打开后，需要定期Feed喂狗，否则系统重启
/// </summary>
/// <remarks>看门狗的超时时间为 10 秒，如果超过 10 秒没有喂狗，系统将自动重启</remarks>
public class Watchdog(String fileName) : DisposeBase
{
    /// <summary>文件路径</summary>
    public String FileName { get; set; } = fileName;

    private FileStream _fs;

    /// <summary>销毁</summary>
    /// <param name="disposing"></param>
    protected override void Dispose(Boolean disposing)
    {
        base.Dispose(disposing);

        if (_fs != null)
        {
            Close();
            _fs.TryDispose();
        }
    }

    private FileStream GetFile() => _fs ??= new FileStream(FileName.GetFullPath(), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

    /// <summary>关闭看门狗</summary>
    public void Close()
    {
        var fs = GetFile();

        fs.WriteByte((Byte)'V');
        fs.Flush();
    }

    /// <summary>喂狗</summary>
    public void Feed()
    {
        var fs = GetFile();

        // 向看门狗设备写入数据，刷新watchdog计数器
        fs.WriteByte((Byte)'1');
        fs.Flush();
    }
}