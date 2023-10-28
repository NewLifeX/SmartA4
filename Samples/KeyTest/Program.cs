using SmartA4;

var key = new InputPort { FileName = "/dev/key" };

var f = false;
for (var i = 0; i < 100; i++)
{
    var rs = key.Read();
    if (rs != f)
    {
        f = rs;
        Console.WriteLine(f ? "按下" : "松开");
    }

    Thread.Sleep(100);
}