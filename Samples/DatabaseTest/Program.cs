using DatabaseTest.Data;
using NewLife.Log;
using NewLife.Serialization;

XTrace.UseConsole();

// 新增用户插入数据库
var user = new Member
{
    Name = "Stone",
    DisplayName = "大石头",
    Password = "123456",
    Enable = true
};
user.Insert();

// 查询用户
var user2 = Member.FindByName("Stone");
XTrace.WriteLine("FindByName: {0}", user2.ToJson(true));

// 更新用户
user2.DisplayName = "智能大石头";
user2.Update();

var user3 = Member.FindById(user2.Id);
XTrace.WriteLine("FindByID: {0}", user3.DisplayName);

// 删除用户
user3.Delete();