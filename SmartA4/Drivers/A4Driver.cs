using System.ComponentModel;
using System.Reflection;
using NewLife;
using NewLife.IoT.Drivers;
using NewLife.IoT.ThingModels;
using NewLife.IoT.ThingSpecification;
using NewLife.Reflection;
using NewLife.Serialization;

namespace SmartA4.Drivers;

/// <summary>
/// A4工业计算机驱动
/// </summary>
/// <remarks>
/// IoT驱动，符合IoT标准库的PC驱动，采集CPU、内存、网络等数据，提供语音播报和重启等服务。
/// </remarks>
[Driver("SmartA4")]
[DisplayName("A4工业计算机")]
public class A4Driver : DriverBase<Node, A4Parameter>
{
    #region 属性
    /// <summary>是否启用重启。默认false</summary>
    public static Boolean EnableReboot { get; set; }

    private A4 _a4 = new();
    #endregion

    #region 方法
    /// <summary>读取数据</summary>
    /// <param name="node">节点对象，可存储站号等信息，仅驱动自己识别</param>
    /// <param name="points">点位集合，Address属性地址示例：D100、C100、W100、H100</param>
    /// <returns></returns>
    public override IDictionary<String, Object> Read(INode node, IPoint[] points)
    {
        var dic = new Dictionary<String, Object>();

        if (points == null || points.Length == 0) return dic;

        var mi = MachineInfo.GetCurrent();
        mi.Refresh();

        foreach (var pi in mi.GetType().GetProperties())
        {
            var point = points.FirstOrDefault(e => e.Name.EqualIgnoreCase(pi.Name));
            if (point != null)
                dic[point.Name] = mi.GetValue(pi);
        }

        foreach (var pi in _a4.GetType().GetProperties())
        {
            var point = points.FirstOrDefault(e => e.Name.EqualIgnoreCase(pi.Name));
            if (point != null)
            {
                var val = _a4.GetValue(pi);
                if (val is InputPort inputPort)
                    dic[point.Name] = inputPort.Read();
                else if (val is OutputPort outputPort)
                    dic[point.Name] = outputPort.Read();
            }
        }

        return dic;
    }

    /// <summary>设备控制</summary>
    /// <param name="node"></param>
    /// <param name="parameters"></param>
    public override Object Control(INode node, IDictionary<String, Object> parameters)
    {
        var service = JsonHelper.Convert<ServiceModel>(parameters);
        if (service == null || service.Name.IsNullOrEmpty()) throw new NotImplementedException();

        switch (service.Name)
        {
            //case nameof(Speak):
            //    Speak(service.InputData);
            //    break;
            case nameof(Reboot):
                if (!EnableReboot) throw new NotSupportedException("未启用重启功能");
                return Reboot(service.InputData.ToInt()) + "";
            case nameof(SetHostName):
                SetHostName(service.InputData);
                break;
            default:
                throw new NotImplementedException();
        }

        return "OK";
    }

    ///// <summary>语音播报</summary>
    ///// <param name="text"></param>
    //[DisplayName("语音播报")]
    //public void Speak(String text) => text.SpeakAsync();

    /// <summary>重启计算机</summary>
    /// <param name="timeout"></param>
    [DisplayName("重启计算机")]
    public Int32 Reboot(Int32 timeout)
    {
        //if (Runtime.Windows)
        //{
        //    var p = "shutdown".ShellExecute($"-r -t {timeout}");
        //    return p?.Id ?? 0;
        //}
        //else if (Runtime.Linux)
        //{
        var p = "reboot".ShellExecute();
        return p?.Id ?? 0;
        //}

        //return -1;
    }

    /// <summary>设置主机名</summary>
    /// <param name="hostName">主机名</param>
    [DisplayName("设置主机名")]
    public void SetHostName(String hostName) => _a4.SetHostName(hostName);

    /// <summary>发现本地节点</summary>
    /// <returns></returns>
    public override ThingSpec GetSpecification()
    {
        var type = GetType();
        var spec = new ThingSpec
        {
            Profile = new Profile
            {
                Version = type.Assembly.GetName().Version + "",
                ProductKey = type.GetCustomAttribute<DriverAttribute>().Name
            }
        };

        var points = new List<PropertySpec>();
        var services = new List<ServiceSpec>();
        var extends = new List<PropertyExtend>();

        var pis = typeof(MachineInfo).GetProperties();

        points.Add(PropertySpec.Create(pis.FirstOrDefault(e => e.Name == "CpuRate")));
        points.Add(PropertySpec.Create(pis.FirstOrDefault(e => e.Name == "Memory")));
        points.Add(PropertySpec.Create(pis.FirstOrDefault(e => e.Name == "AvailableMemory")));
        points.Add(PropertySpec.Create(pis.FirstOrDefault(e => e.Name == "UplinkSpeed")));
        points.Add(PropertySpec.Create(pis.FirstOrDefault(e => e.Name == "DownlinkSpeed")));
        points.Add(PropertySpec.Create(pis.FirstOrDefault(e => e.Name == "Temperature")));
        points.Add(PropertySpec.Create(pis.FirstOrDefault(e => e.Name == "Battery")));

        // 只读
        foreach (var item in points)
            item.AccessMode = "r";

        //services.Add(ServiceSpec.Create(Speak));
        services.Add(ServiceSpec.Create(Reboot));
        services.Add(ServiceSpec.Create(SetHostName));

        // A2特有
        foreach (var pi in _a4.GetType().GetProperties())
            if (pi.PropertyType == typeof(InputPort))
            {
                var pt = PropertySpec.Create(pi);
                pt.DataType.Type = "bool";
                pt.AccessMode = "r";
                points.Add(pt);
            }
            else if (pi.PropertyType == typeof(OutputPort))
            {
                var pt = PropertySpec.Create(pi);
                pt.DataType.Type = "bool";
                pt.AccessMode = "rw";
                points.Add(pt);
            }
        services.Add(ServiceSpec.Create(SetHostName));

        spec.Properties = points.Where(e => e != null).ToArray();
        spec.Services = services.Where(e => e != null).ToArray();

        return spec;
    }
    #endregion
}