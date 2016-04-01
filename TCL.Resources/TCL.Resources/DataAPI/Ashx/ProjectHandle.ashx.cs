using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using TCL.Resources.BLL;
using TCL.Resources.Entity;
using TCL.Resources.Common;

namespace TCL.Resources.Web.DataAPI.Ashx
{
    /// <summary>
    /// ProjectHandle 的摘要说明
    /// </summary>
    public class ProjectHandle : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string levelOneName=context.Request.QueryString["levelonename"];
            string levelTwoName = context.Request.QueryString["leveltwoname"];
            string levelThreeID = context.Request.QueryString["levelThreeID"];
            string result = string.Empty;
            string type = context.Request.QueryString["type"];
            //获取项目列表
            if (null!=type&&type.Equals("GetProjectList"))
            {
                result = GetEPProject();
            }
            else
            {
                result = GetLevelProject(levelOneName, levelTwoName, levelThreeID);
            }
            
            context.Response.ContentType = "text/plain";
            context.Response.Write(result);

        }
        private string GetLevelProject(string levelOneName, string levelTwoName,string levelThreeID)
        {
            string sql = string.Empty;
            if (null == levelTwoName && null == levelThreeID)
            {
                sql = "SELECT [id] as EProjectID ,[ProjectName] as ProjectName  FROM [TCL_RESOURCE].[dbo].[EP_Project] where BusinessDevise='" + levelOneName + "'";
            }
            else if (null != levelOneName && null != levelTwoName)
            {
                sql="SELECT [id] as EProjectID ,[ProjectName] as ProjectName  FROM [TCL_RESOURCE].[dbo].[EP_Project] where BusinessDevise='" + levelOneName + "' and Site='" + levelTwoName + "'";
            }
            else if(null!=levelThreeID)
            {
                //sql = "select [id] as EProjectID ,[ProjectName] as ProjectName from [TCL_RESOURCE].[dbo].[EP_Project]"
                //+ " where [id] in (select FProject_ID from  dbo.T_Resource_OrgProject where fcreator_zad='" + "TCT-HQ\\binyu.cui" + "' and FOrg_ID='"+levelThreeID+"')";
                sql = "select a.id as EProjectID ,a.ProjectName as ProjectName,b.FLevel1_Name as OrgLevel1Name,b.FLevel2_Name AS OrgLevel2Name from EP_Project a,T_Resource_OrgProject  b where a.id=b.FProject_ID and b.FOrg_ID='" + levelThreeID + "'";
            }
            List<ResourceProject> reSources = DataHelper.GetResourceData<ResourceProject>(sql);
            DataTable dt = GetDataTable(reSources);
            return JsonConvert.SerializeObject(dt);
        }

        public string GetEPProject()
        {
            string sql = "select [id] as EProjectID ,[ProjectName] as ProjectName from dbo.[EP_Project]";
            List<ResourceProject> list = EntityHelper.FillList<ResourceProject>(sql);
            return JsonConvert.SerializeObject(list);
        }
        private DataTable GetDataTable(List<ResourceProject> reSources)
        {
            DataTable dt = new DataTable();
            DataColumn dcProjectID = new DataColumn("EProjectID");
            DataColumn dcProjectName = new DataColumn("ProjectName");
            DataColumn dcLevel1Name = new DataColumn("OrgLevel1Name");
            DataColumn dcLevel2Name = new DataColumn("OrgLevel2Name");
            dt.Columns.Add(dcProjectID);
            dt.Columns.Add(dcProjectName);
            dt.Columns.Add(dcLevel1Name);
            dt.Columns.Add(dcLevel2Name);
            for (int i = 0, j = reSources.Count(); i < j; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = reSources[i].EProjectID;
                dr[1] = reSources[i].ProjectName;
                dr[2] = reSources[i].OrgLevel1Name;
                dr[3] = reSources[i].OrgLevel2Name;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}