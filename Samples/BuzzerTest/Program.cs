using SmartA4;

var buzzer = new OutputPort { FileName = "/dev/buzzer" };

for (var i = 0; i < 5; i++)
{
    // 响
    buzzer.Write(true);

    Thread.Sleep(500);

    // 不响
    buzzer.Write(false);

    Thread.Sleep(500);
}