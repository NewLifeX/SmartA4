using A4Flower;
using NewLife.Log;
using NewLife.Model;
using SmartA4;
using Stardust;

//!!! 轻量级控制台项目模板

// 启用控制台日志，拦截所有异常
XTrace.UseConsole();

// 初始化对象容器，提供注入能力
var services = ObjectContainer.Current;
services.AddStardust();

services.AddSingleton<A4, A4>();

// 注册后台任务 IHostedService
services.AddHostedService<Worker>();

var host = services.BuildHost();

// 异步阻塞，友好退出
await host.RunAsync();
