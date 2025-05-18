using NewLife;
using NewLife.Configuration;
using NewLife.Log;
using NewLife.Model;
using NewLife.Remoting.Clients;
using NewLife.Threading;
using SmartA4;
using Stardust;
using Stardust.Registry;

namespace A4Flower;

/// <summary>
/// 后台任务。支持构造函数注入服务
/// </summary>
public class Worker : IHostedService
{
    private readonly A4 _a4;
    private readonly AppClient _appClient;
    private readonly IConfigProvider _configProvider;
    private readonly FlowerSetting _setting = new();
    private readonly ITracer _tracer;
    private TimerX _timer;

    public Worker(A4 a4, IRegistry registry, IConfigProvider configProvider, ITracer tracer)
    {
        _a4 = a4;
        _appClient = registry as AppClient;
        _configProvider = configProvider;
        _tracer = tracer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            // 绑定参数到配置中心，支持热更新
            _configProvider.LoadAll();
            if (_configProvider.Keys.Count < 3)
                _configProvider.Save(_setting);
            else
                _configProvider.Bind(_setting, true, null);

            _appClient?.RegisterCommand("test", s => DoWork(s));

            // 关闭
            var usb = _a4.UsbPower;
            usb.Write(false);
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
        }

        SetTimer(_setting.Cron);

        return Task.CompletedTask;
    }

    private String _cron;
    private void SetTimer(String cron)
    {
        if (cron == _cron) return;

        _timer.TryDispose();

        if (!cron.IsNullOrEmpty())
        {
            // 支持多个Cron表达式，分号隔开
            var timer = _timer = new TimerX(DoWork, null, cron) { Async = true };

            var next = timer.NextTime;
            if (next > DateTime.MinValue) XTrace.WriteLine("下一次执行时间：{0}", next.ToFullString());
        }

        _cron = cron;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.TryDispose();

        return Task.CompletedTask;
    }

    void DoWork(Object state)
    {
        XTrace.WriteLine("打开开关");

        using var span = _tracer?.NewSpan("DoWork");
        try
        {
            var buzzer = _a4.Buzzer;
            var usb = _a4.UsbPower;

            var t = _setting.BuzzerTime;
            if (t > 0)
            {
                buzzer.Write(true);
                Thread.Sleep(t);
                buzzer.Write(false);
            }

            t = _setting.UsbTime > 0 ? _setting.UsbTime : 3000;
            usb.Write(true);
            Thread.Sleep(t);
            usb.Write(false);
        }
        catch (Exception ex)
        {
            span?.SetError(ex, null);
            XTrace.WriteException(ex);
        }

        XTrace.WriteLine("关闭电源");

        // 定时时间有改变
        SetTimer(_setting.Cron);
    }
}