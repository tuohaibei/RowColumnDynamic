using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCL.Resources.Entity
{
    public class BudgetDetail
    {
        public bool FIS_outsource { get; set; }
        public string FOrg_Level4 { get; set; }
        public int FBudget_Year { get; set; }
        public int FBudget_Month { get; set; }
        public int FProject_ID { get; set; }
        public decimal FBudget_Resource { get; set; }
        public int ID { get; set; }
    }
}