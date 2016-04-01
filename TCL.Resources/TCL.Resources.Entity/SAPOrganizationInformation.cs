using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCL.Resources.Entity
{
    public class SAPOrganizationInformation
    {
        //组织单位代码
        public string ZZ_ORG_ID { get; set; }
        //组织单位名称缩写
        public string ZZ_ORG_SHORT{get;set;}
        //组织单位名称长文本 
        public string ZZ_ORG_TXT{get;set;}
        //组织单位名称长文本_2
        public string ZZ_ORG_TXT_2{get;set;}
        //上级组织单位代码
        public string ZZ_UPORG_ID{get;set;}
        //上级组织单位名称
        public string ZZ_UPORG_TEXT{get;set;}
        //部门类别
        public string ZZ_ORGUNITSORT{get;set;}
        //部门层级
        public string ZZ_ORGLEVEL{get;set;}
        //部门负责人（人员编号） 
        public string ZZ_LEADER{get;set;}
        //分管部门负责人（人员编号） 
        public string ZZ_LEADER2 { get; set; }
        //部门助理（人员编号）
        public string ZZ_ASSID{get;set;}
        //人力资源代表（人员编号）
        public string ZZ_HRID{get;set;}
        //部门AD账号
        public string ZZ_ADID{get;set;}
        // 递归解析的部门层级（弥补SAP里维护的组织层级数据不准确）
        public int OrgLevel { get; set; }
    }
}