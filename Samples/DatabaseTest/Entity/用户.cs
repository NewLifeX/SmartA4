using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using NewLife;
using NewLife.Data;
using XCode;
using XCode.Cache;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace DatabaseTest.Data;

/// <summary>用户。用户帐号信息，以身份验证为中心，拥有多种角色，可加入多个租户</summary>
[Serializable]
[DataObject]
[Description("用户。用户帐号信息，以身份验证为中心，拥有多种角色，可加入多个租户")]
[BindIndex("IU_Member_Name", true, "Name")]
[BindIndex("IX_Member_Mail", false, "Mail")]
[BindIndex("IX_Member_Mobile", false, "Mobile")]
[BindIndex("IX_Member_UpdateTime", false, "UpdateTime")]
[BindTable("Member", Description = "用户。用户帐号信息，以身份验证为中心，拥有多种角色，可加入多个租户", ConnName = "Test", DbType = DatabaseType.None)]
public partial class Member
{
    #region 属性
    private Int32 _Id;
    /// <summary>编号</summary>
    [DisplayName("编号")]
    [Description("编号")]
    [DataObjectField(true, true, false, 0)]
    [BindColumn("Id", "编号", "")]
    public Int32 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

    private String _Name = null!;
    /// <summary>名称。登录用户名</summary>
    [DisplayName("名称")]
    [Description("名称。登录用户名")]
    [DataObjectField(false, false, false, 50)]
    [BindColumn("Name", "名称。登录用户名", "", Master = true)]
    public String Name { get => _Name; set { if (OnPropertyChanging("Name", value)) { _Name = value; OnPropertyChanged("Name"); } } }

    private String? _Password;
    /// <summary>密码</summary>
    [DisplayName("密码")]
    [Description("密码")]
    [DataObjectField(false, false, true, 200)]
    [BindColumn("Password", "密码", "")]
    public String? Password { get => _Password; set { if (OnPropertyChanging("Password", value)) { _Password = value; OnPropertyChanged("Password"); } } }

    private String? _DisplayName;
    /// <summary>昵称</summary>
    [DisplayName("昵称")]
    [Description("昵称")]
    [DataObjectField(false, false, true, 50)]
    [BindColumn("DisplayName", "昵称", "")]
    public String? DisplayName { get => _DisplayName; set { if (OnPropertyChanging("DisplayName", value)) { _DisplayName = value; OnPropertyChanged("DisplayName"); } } }

    private XCode.Membership.SexKinds _Sex;
    /// <summary>性别。未知、男、女</summary>
    [DisplayName("性别")]
    [Description("性别。未知、男、女")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Sex", "性别。未知、男、女", "")]
    public XCode.Membership.SexKinds Sex { get => _Sex; set { if (OnPropertyChanging("Sex", value)) { _Sex = value; OnPropertyChanged("Sex"); } } }

    private String? _Mail;
    /// <summary>邮件。支持登录</summary>
    [DisplayName("邮件")]
    [Description("邮件。支持登录")]
    [DataObjectField(false, false, true, 50)]
    [BindColumn("Mail", "邮件。支持登录", "", ItemType = "mail")]
    public String? Mail { get => _Mail; set { if (OnPropertyChanging("Mail", value)) { _Mail = value; OnPropertyChanged("Mail"); } } }

    private String? _Mobile;
    /// <summary>手机。支持登录</summary>
    [DisplayName("手机")]
    [Description("手机。支持登录")]
    [DataObjectField(false, false, true, 50)]
    [BindColumn("Mobile", "手机。支持登录", "", ItemType = "mobile")]
    public String? Mobile { get => _Mobile; set { if (OnPropertyChanging("Mobile", value)) { _Mobile = value; OnPropertyChanged("Mobile"); } } }

    private Boolean _Enable;
    /// <summary>启用</summary>
    [Category("登录信息")]
    [DisplayName("启用")]
    [Description("启用")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Enable", "启用", "")]
    public Boolean Enable { get => _Enable; set { if (OnPropertyChanging("Enable", value)) { _Enable = value; OnPropertyChanged("Enable"); } } }

    private String _UpdateUser = null!;
    /// <summary>更新者</summary>
    [Category("扩展")]
    [DisplayName("更新者")]
    [Description("更新者")]
    [DataObjectField(false, false, false, 50)]
    [BindColumn("UpdateUser", "更新者", "", DefaultValue = "''")]
    public String UpdateUser { get => _UpdateUser; set { if (OnPropertyChanging("UpdateUser", value)) { _UpdateUser = value; OnPropertyChanged("UpdateUser"); } } }

    private Int32 _UpdateUserID;
    /// <summary>更新用户</summary>
    [Category("扩展")]
    [DisplayName("更新用户")]
    [Description("更新用户")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("UpdateUserID", "更新用户", "")]
    public Int32 UpdateUserID { get => _UpdateUserID; set { if (OnPropertyChanging("UpdateUserID", value)) { _UpdateUserID = value; OnPropertyChanged("UpdateUserID"); } } }

    private String? _UpdateIP;
    /// <summary>更新地址</summary>
    [Category("扩展")]
    [DisplayName("更新地址")]
    [Description("更新地址")]
    [DataObjectField(false, false, true, 50)]
    [BindColumn("UpdateIP", "更新地址", "")]
    public String? UpdateIP { get => _UpdateIP; set { if (OnPropertyChanging("UpdateIP", value)) { _UpdateIP = value; OnPropertyChanged("UpdateIP"); } } }

    private DateTime _UpdateTime;
    /// <summary>更新时间</summary>
    [Category("扩展")]
    [DisplayName("更新时间")]
    [Description("更新时间")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("UpdateTime", "更新时间", "")]
    public DateTime UpdateTime { get => _UpdateTime; set { if (OnPropertyChanging("UpdateTime", value)) { _UpdateTime = value; OnPropertyChanged("UpdateTime"); } } }

    private String? _Remark;
    /// <summary>备注</summary>
    [Category("扩展")]
    [DisplayName("备注")]
    [Description("备注")]
    [DataObjectField(false, false, true, 500)]
    [BindColumn("Remark", "备注", "")]
    public String? Remark { get => _Remark; set { if (OnPropertyChanging("Remark", value)) { _Remark = value; OnPropertyChanged("Remark"); } } }
    #endregion

    #region 获取/设置 字段值
    /// <summary>获取/设置 字段值</summary>
    /// <param name="name">字段名</param>
    /// <returns></returns>
    public override Object? this[String name]
    {
        get => name switch
        {
            "Id" => _Id,
            "Name" => _Name,
            "Password" => _Password,
            "DisplayName" => _DisplayName,
            "Sex" => _Sex,
            "Mail" => _Mail,
            "Mobile" => _Mobile,
            "Enable" => _Enable,
            "UpdateUser" => _UpdateUser,
            "UpdateUserID" => _UpdateUserID,
            "UpdateIP" => _UpdateIP,
            "UpdateTime" => _UpdateTime,
            "Remark" => _Remark,
            _ => base[name]
        };
        set
        {
            switch (name)
            {
                case "Id": _Id = value.ToInt(); break;
                case "Name": _Name = Convert.ToString(value); break;
                case "Password": _Password = Convert.ToString(value); break;
                case "DisplayName": _DisplayName = Convert.ToString(value); break;
                case "Sex": _Sex = (XCode.Membership.SexKinds)value.ToInt(); break;
                case "Mail": _Mail = Convert.ToString(value); break;
                case "Mobile": _Mobile = Convert.ToString(value); break;
                case "Enable": _Enable = value.ToBoolean(); break;
                case "UpdateUser": _UpdateUser = Convert.ToString(value); break;
                case "UpdateUserID": _UpdateUserID = value.ToInt(); break;
                case "UpdateIP": _UpdateIP = Convert.ToString(value); break;
                case "UpdateTime": _UpdateTime = value.ToDateTime(); break;
                case "Remark": _Remark = Convert.ToString(value); break;
                default: base[name] = value; break;
            }
        }
    }
    #endregion

    #region 关联映射
    #endregion

    #region 字段名
    /// <summary>取得用户字段信息的快捷方式</summary>
    public partial class _
    {
        /// <summary>编号</summary>
        public static readonly Field Id = FindByName("Id");

        /// <summary>名称。登录用户名</summary>
        public static readonly Field Name = FindByName("Name");

        /// <summary>密码</summary>
        public static readonly Field Password = FindByName("Password");

        /// <summary>昵称</summary>
        public static readonly Field DisplayName = FindByName("DisplayName");

        /// <summary>性别。未知、男、女</summary>
        public static readonly Field Sex = FindByName("Sex");

        /// <summary>邮件。支持登录</summary>
        public static readonly Field Mail = FindByName("Mail");

        /// <summary>手机。支持登录</summary>
        public static readonly Field Mobile = FindByName("Mobile");

        /// <summary>启用</summary>
        public static readonly Field Enable = FindByName("Enable");

        /// <summary>更新者</summary>
        public static readonly Field UpdateUser = FindByName("UpdateUser");

        /// <summary>更新用户</summary>
        public static readonly Field UpdateUserID = FindByName("UpdateUserID");

        /// <summary>更新地址</summary>
        public static readonly Field UpdateIP = FindByName("UpdateIP");

        /// <summary>更新时间</summary>
        public static readonly Field UpdateTime = FindByName("UpdateTime");

        /// <summary>备注</summary>
        public static readonly Field Remark = FindByName("Remark");

        static Field FindByName(String name) => Meta.Table.FindByName(name);
    }

    /// <summary>取得用户字段名称的快捷方式</summary>
    public partial class __
    {
        /// <summary>编号</summary>
        public const String Id = "Id";

        /// <summary>名称。登录用户名</summary>
        public const String Name = "Name";

        /// <summary>密码</summary>
        public const String Password = "Password";

        /// <summary>昵称</summary>
        public const String DisplayName = "DisplayName";

        /// <summary>性别。未知、男、女</summary>
        public const String Sex = "Sex";

        /// <summary>邮件。支持登录</summary>
        public const String Mail = "Mail";

        /// <summary>手机。支持登录</summary>
        public const String Mobile = "Mobile";

        /// <summary>启用</summary>
        public const String Enable = "Enable";

        /// <summary>更新者</summary>
        public const String UpdateUser = "UpdateUser";

        /// <summary>更新用户</summary>
        public const String UpdateUserID = "UpdateUserID";

        /// <summary>更新地址</summary>
        public const String UpdateIP = "UpdateIP";

        /// <summary>更新时间</summary>
        public const String UpdateTime = "UpdateTime";

        /// <summary>备注</summary>
        public const String Remark = "Remark";
    }
    #endregion
}
