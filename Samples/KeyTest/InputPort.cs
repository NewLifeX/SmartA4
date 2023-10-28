namespace SmartA4;

/// <summary>输入口</summary>
public class InputPort
{
    /// <summary>文件路径</summary>
    public String FileName { get; set; }

    FileStream _stream;
    Stream GetStream() => _stream ??= File.OpenRead(FileName);

    /// <summary>读取开关值</summary>
    /// <returns></returns>
    public Boolean Read() => GetStream().ReadByte() == '1';
}