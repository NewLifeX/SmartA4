using NewLife.IoT.Controllers;
using SmartA4;

namespace NewLife.Model;

/// <summary>A4硬件工厂扩展</summary>
public static class A4Extensions
{
    /// <summary>在A4工业计算机中注册IBoard服务</summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static Boolean AddA2(this IObjectContainer services)
    {
        // 检测本机如果是A4，则注入服务
        var flag = false;
        var machineName = Environment.MachineName;
        if (!flag && (machineName == "A4" || machineName.StartsWithIgnoreCase("A4-"))) flag = true;
        if (!flag)
        {
            var mi = MachineInfo.GetCurrent();
            if (!flag && (mi.Product == "A4" || mi.Product.StartsWithIgnoreCase("A4-"))) flag = true;
            if (!flag && mi.Board.StartsWithIgnoreCase("A4-")) flag = true;
        }
        if (!flag) return false;

        services.AddSingleton<IBoard, A4>();

        return true;
    }
}
