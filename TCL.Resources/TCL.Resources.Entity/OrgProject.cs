using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCL.Resources.Entity
{
    public class OrgProject
    {

        public int FProject_ID { get; set; }
        public string ProjectName { get; set; }
        public string OrgLvl4 { get; set; }
        public string OrgLvl4Txt { get; set; }
        public string OrgLvl3 { get; set; }
        public string OrgLvl3Txt { get; set; }
        public string OrgLvl2 { get; set; }
        public string OrgLvl2Txt { get; set; }
        public string OrgLvl1 { get; set; }
        public string OrgLvl1Txt { get; set; }
        public bool FIS_outsource { get; set; }
    }
}