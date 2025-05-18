namespace SmartA4;

/// <summary>输入口</summary>
public class InputPort
{
    /// <summary>文件路径</summary>
    public String FileName { get; set; }

    /// <summary>读取开关值</summary>
    /// <returns></returns>
    public Boolean Read() => File.ReadAllText(FileName.GetFullPath())?.Trim() == "1";
}