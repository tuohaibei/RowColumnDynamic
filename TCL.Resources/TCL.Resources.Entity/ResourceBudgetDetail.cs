//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
namespace TCL.Resources.Entity
{
    [Serializable]
    public  class ResourceBudgetDetail
    {
        public int ID { get; set; }
        public string CreatrorSAPID { get; set; }
        public string ModifySAPID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> ModifyDateTime { get; set; }
        public bool ISoutsource { get; set; }
        public string OrgLevel4 { get; set; }
        public int BudgetYear { get; set; }
        public int BudgetMonth { get; set; }
        public string ProjectID { get; set; }
        public decimal BudgetResource { get; set; }
    }
}
