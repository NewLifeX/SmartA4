namespace SmartA4;

/// <summary>输出口</summary>
public class OutputPort
{
    /// <summary>文件路径</summary>
    public String FileName { get; set; }

    /// <summary>读取开关值</summary>
    /// <returns></returns>
    public Boolean Read() => File.ReadAllText(FileName)?.Trim() == "1";

    /// <summary>写入开关值</summary>
    /// <param name="value"></param>
    public void Write(Boolean value) => File.WriteAllText(FileName, value ? "1" : "0");
}