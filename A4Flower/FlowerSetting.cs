using System.ComponentModel;

namespace A4Flower;

internal class FlowerSetting
{
    /// <summary>
    /// 定时周期。默认3600秒
    /// </summary>
    [Description("定时周期。默认3600秒")]
    public Int32 Period { get; set; } = 3600;

    /// <summary>定时表达式。支持多个表达式（分号隔开），默认0 30 8-18 * * ? *，每天8到18点的30分执行</summary>
    [Description("定时表达式。支持多个表达式（分号隔开），默认0 30 8-18 * * ? *，每天8到18点的30分执行")]
    public String Cron { get; set; } = "0 30 8-18 * * ? *";

    /// <summary>
    /// 蜂鸣器时间。默认200ms
    /// </summary>
    [Description("蜂鸣器时间。默认200ms")]
    public Int32 BuzzerTime { get; set; } = 200;

    /// <summary>
    /// USB电源时间。默认3000ms
    /// </summary>
    [Description("USB电源时间。默认3000ms")]
    public Int32 UsbTime { get; set; } = 3000;
}
