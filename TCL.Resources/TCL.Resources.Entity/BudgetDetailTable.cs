using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCL.Resources.Entity
{
    public class BudgetDetailTable
    {
        /*
         [ID] [int] NOT NULL,
	    [FIS_outsource] [bit] NOT NULL,
	    [FOrg_Level4] [nvarchar](10) NOT NULL,
	    [FBudget_Year] [int] NOT NULL,
	    [FBudget_Month] [int] NOT NULL,
	    [FProject_ID] [int] NOT NULL,
	    [FBudget_Resource] [decimal](18, 2) NOT NULL
         */

        public int ID { get; set; }
        public bool FIS_outsource { get; set; }
        public string FOrg_Level4 { get; set; }
        public int FBudget_Year { get; set; }
        public int FBudget_Month { get; set; }
        public int FProject_ID { get; set; }
        public decimal FBudget_Resource { get; set; }



    }
}