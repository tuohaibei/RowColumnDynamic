//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TCL.Resources.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    
    public partial class ResourceAuthority
    {
        public int id { get; set; }
        public string AuthoritySAPID { get; set; }
        public string OrgName { get; set; }
        public string OrgID { get; set; }
        public string CreatorSAPID { get; set; }
        public string ModifySAPID { get; set; }
        public string EMail { get; set; }
        [Browsable(true), Description("域账号名称,格式[域名\\姓名]")] 
        public string ZAD { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> ModifyDateTime { get; set; }
    }
}
