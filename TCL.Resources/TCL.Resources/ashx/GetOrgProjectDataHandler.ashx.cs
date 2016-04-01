using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Data;
using TCL.Resources.Entity;

namespace TCL.Resources.Web.ashx
{
    /// <summary>
    /// GetOrgProjectDataHandler 的摘要说明
    /// </summary>
    public class GetOrgProjectDataHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string serviceType = context.Request.Params.GetValues("ServiceType")[0];
            string orgID = context.Request.Params.GetValues("orgID")[0];
            TCL.Resources.BLL.OrgProjectBLL orgProjectBLL = new BLL.OrgProjectBLL();
            //1:获取左边组织项目信息
            if (serviceType == "1")
            {
                string containOutsource = context.Request.Params.GetValues("containOutsource")[0];
                bool isContainOutsource = false;
                if (!string.IsNullOrEmpty(containOutsource))
                {
                    isContainOutsource = containOutsource == "1" ? true : false;
                }
                int startYear = ParseData(context.Request.Params.GetValues("startYear")[0]);
                int startMonth = ParseData(context.Request.Params.GetValues("startMonth")[0]);
                int endYear = ParseData(context.Request.Params.GetValues("endYear")[0]);
                int endMonth = ParseData(context.Request.Params.GetValues("endMonth")[0]);
                List<TCL.Resources.Entity.OrgProject> data = orgProjectBLL.GetOrgProjectList(orgID, startYear, startMonth, endYear, endMonth, isContainOutsource);
                string result = JsonConvert.SerializeObject(data);
                context.Response.ContentType = "json/plain";
                context.Response.Write(result);
                context.Response.End();
            }
            //获取月份对应的项目信息
            else if (serviceType == "2")
            {
                string containOutsource = context.Request.Params.GetValues("containOutsource")[0];
                bool isContainOutsource = false;
                if (!string.IsNullOrEmpty(containOutsource))
                {
                    isContainOutsource = containOutsource == "1" ? true : false;
                }
                int startYear = ParseData(context.Request.Params.GetValues("startYear")[0]);
                int startMonth = ParseData(context.Request.Params.GetValues("startMonth")[0]);
                int endYear = ParseData(context.Request.Params.GetValues("endYear")[0]);
                int endMonth = ParseData(context.Request.Params.GetValues("endMonth")[0]);
                List<TCL.Resources.Entity.BudgetDetail> data = orgProjectBLL.GetResourceBudgetDetail(orgID, startYear, startMonth, endYear, endMonth, isContainOutsource);
                string result = JsonConvert.SerializeObject(data);
                context.Response.ContentType = "json/plain";
                context.Response.Write(result);
                context.Response.End();
            }
            //保存对应的项目信息
            else if (serviceType == "3")
            {
                string userAD = context.User.Identity.Name;
                string jsonData = context.Request.Params.GetValues("ResourcePlanJson")[0];
                List<TCL.Resources.Entity.BudgetDetailTable> detailList = JsonConvert.DeserializeObject<List<TCL.Resources.Entity.BudgetDetailTable>>(jsonData);
                string saveResult = orgProjectBLL.SaveResourceBudgetDetail(userAD, detailList);
                context.Response.ContentType = "text/plain";
                context.Response.Write(saveResult);
                context.Response.End();
            }
            //获取可供查询的年份信息
            else if (serviceType == "4")
            {
                string startDate = (context.Request.Params.GetValues("startDate")[0]);
                string endDate = (context.Request.Params.GetValues("endDate")[0]);
                string projectID = (context.Request.Params.GetValues("projectID")[0]);
                //DealValue(ref startDate,ref endDate,ref projectID,ref orgID);
                //List<TCL.Resources.Entity.BudgetDetail> data = orgProjectBLL.GetResourceBudgetYearMonth(orgID, startDate, endDate, projectID);
                DataSet ds = orgProjectBLL.GetResourceBudgetYearMonthForQuery(orgID, startDate, endDate, projectID);
                ResourceResult data = new ResourceResult();
                DataTable dt;
                List<string> dateColumn = new List<string>();
                for(int i=0;i<ds.Tables.Count;i++)
                {
                    //取时间数组
                    if (i == 0)
                    {
                       dt =  ds.Tables[i];  
                       foreach (DataRow dr in dt.Rows)
                       {
                           dateColumn.Add(dr[0].ToString());
                       }
                       data.ColumnName = dateColumn;
                    }
                    //取组织项目
                    else if (i == 1)
                    {
                        dt = ds.Tables[i];
                        data.project = TCL.Resources.Common.ConvertHelper<OrgProject>.ConvertToList(dt);
                    }
                    //取值
                    else if (i == 2)
                    {
                        dt = ds.Tables[i];
                        data.details = TCL.Resources.Common.ConvertHelper<BudgetDetail>.ConvertToList(dt);
                    }
                }
                string result = JsonConvert.SerializeObject(data);
                context.Response.ContentType = "json/plain";
                context.Response.Write(result);
                context.Response.End();
            }
        }

        private int ParseData(string sourceString)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(sourceString))
            {
                if (int.TryParse(sourceString, out result))
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        private void DealValue(ref string startDate,ref string endDate,ref string projectID,ref string orgID)
        {

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class ResourceResult
    {
        public List<string> ColumnName;
        public List<BudgetDetail> details;
        public List<OrgProject> project;
    }
}