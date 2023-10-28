using SmartA4;

var led = new OutputPort { FileName = "/dev/led" };

while (true)
{
    // 灭
    led.Write(false);

    Thread.Sleep(500);

    // 亮
    led.Write(true);

    Thread.Sleep(500);
}
