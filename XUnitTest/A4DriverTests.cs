using NewLife;
using NewLife.IoT.Drivers;
using NewLife.IoT.ThingModels;
using NewLife.IoT.ThingSpecification;
using NewLife.Serialization;
using SmartA4.Drivers;

namespace XUnitTest;

[TestCaseOrderer("NewLife.UnitTest.DefaultOrderer", "NewLife.UnitTest")]
public class A4DriverTests
{
    A4Driver _driver;
    INode _node;
    ThingSpec _spec;
    public A4DriverTests()
    {
        _driver = new A4Driver();
    }

    [Fact]
    public void GetSpecificationTest()
    {
        _spec = _driver.GetSpecification();
        Assert.NotNull(_spec);

        Assert.NotNull(_spec.Profile);
        Assert.Equal("SmartA4", _spec.Profile.ProductKey);
        Assert.NotEmpty(_spec.Profile.Version);

        var ps = _spec.Properties;
        Assert.NotNull(ps);
        Assert.Equal(13, ps.Length);

        Assert.NotNull(_spec.Services);
        Assert.Null(_spec.Events);
        Assert.Null(_spec.ExtendedProperties);
    }

    [Fact]
    public void OpenTest()
    {
        _node = _driver.Open(null, null);
        Assert.NotNull(_node);
    }

    [Fact]
    public void ReadTest()
    {
        var spec = _driver.GetSpecification();
        var points = spec.Properties.Select(e => new PointModel { Name = e.Id, Type = e.DataType.Type }).ToArray();
        points = points.Where(e => e.Type != "bool").ToArray();
        var rs = _driver.Read(_node, points);

        Assert.NotNull(rs);
        Assert.Equal(points.Length, rs.Count);

        var mi = JsonHelper.Convert<MachineInfo>(rs);
        Assert.True(mi.CpuRate > 0);
        Assert.True(mi.Memory > 0);
        Assert.True(mi.AvailableMemory > 0);
    }

    //[Fact]
    //public void ControlTest()
    //{
    //    var model = new ServiceModel { Name = "Speak", InputData = "好好学习" };
    //    var rs = _driver.Control(_node, model.ToDictionary());
    //    Assert.NotNull(rs);

    //    Thread.Sleep(1000);
    //}

    [Fact]
    public void ControlTest2()
    {
        var model = new ServiceModel { Name = "Reboot", InputData = "1213" };
        Assert.Throws<NotSupportedException>(() => _driver.Control(_node, model.ToDictionary()));

        A4Driver.EnableReboot = true;
    }

    [Fact]
    public void ControlTest3()
    {
        var model3 = new ServiceModel { Name = "abcd", InputData = "1213" };
        Assert.Throws<NotImplementedException>(() => _driver.Control(_node, model3.ToDictionary()));
    }

    //[Fact]
    //public void SpeakTest()
    //{
    //    _driver.Speak("学无先后达者为师");
    //    Thread.Sleep(3000);
    //}

    [Fact(Skip = "跳过")]
    public void RebootTest()
    {
        var rs = _driver.Reboot(60);
        Assert.True(rs > 0);

        Thread.Sleep(1000);

        "shutdown".ShellExecute("-a");
    }
}