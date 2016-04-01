using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using TCL.Resources.Entity;
using TCL.Resources.Common;
using Newtonsoft.Json;
using TCL.Resources.BLL;
using System.Web.UI;
namespace TCL.Resources.Web.DataAPI.Ashx
{
    /// <summary>
    /// LevelHandle 的摘要说明
    /// </summary>
    public class LevelHandle : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string levelName = context.Request.QueryString["levelName"];
            string level = context.Request.QueryString["level"];
            string result = string.Empty;
            string userDomainName = ResourceHttpContext.DomainFullUserName;
            if (null == level)
            {
                result = GetBranchOneJSON();
            }
            else if (level.Equals(LevelType.LevelOne.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                result = GetBranchTwoJSON(levelName);
            }
            else if (level.Equals(LevelType.LevelThree.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                result = GetBrachThreeJsonForProjectSet(userDomainName);
            }
            else if (level.Equals(LevelType.LevelThreeQuery.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                result = GetBrachThreeJson(userDomainName);
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(result);
        }
        /// <summary>
        /// 获取一级节点信息
        /// </summary>
        /// <returns></returns>
        private string GetBranchOneJSON()
        {
            string sql = "SELECT [BusinessDevise] as OrgLevel1Name"
           + " FROM [TCL_RESOURCE].[dbo].[EP_Project]"
           + " where LEN([BusinessDevise])>0"
           + " Group by BusinessDevise";
            List<ResourceProject> reSources = DataHelper.GetResourceData<ResourceProject>(sql);
            DataTable dt = new DataTable();
            DataColumn dcName = new DataColumn("OrgLevel1Name");
            dt.Columns.Add(dcName);
            for (int i = 0, j = reSources.Count(); i < j; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = reSources[i].OrgLevel1Name;
                dt.Rows.Add(dr);
            }
            return JsonConvert.SerializeObject(dt);
        }
        /// <summary>
        /// 根据一级获取二级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetBranchTwoJSON(string levelTwoName)
        {
            string sql="SELECT [Site] as OrgLevel2Name"
            + " FROM [TCL_RESOURCE].[dbo].[EP_Project]"
            + " where LEN([BusinessDevise])>0 and BusinessDevise='" + levelTwoName + "'"
            + " Group by [Site]";
            List<ResourceProject> reSources = DataHelper.GetResourceData<ResourceProject>(sql);
            DataTable dt = new DataTable();
            DataColumn dcName = new DataColumn("OrgLevel2Name");
            dt.Columns.Add(dcName);
            for (int i = 0, j = reSources.Count(); i < j; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = reSources[i].OrgLevel2Name;
                dt.Rows.Add(dr);
            }
            return JsonConvert.SerializeObject(dt);

        }
        /// <summary>
        /// 获取当前用户可以设置的部门
        /// </summary>
        /// <param name="levelThreeName"></param>
        /// <returns></returns>
        private string GetBrachThreeJson(string userDomainName )
        {
            string sql = ";with cte as(select ZZ_ORG_ID,ZZ_UPORG_ID,OrgLevel = 1,FullOrgName=cast(ZZ_ORG_SHORT as nvarchar(max)) from ZHR_ORG where ZZ_UPORG_ID = '00000010' and ZZ_Enabled = 1 union all select A.ZZ_ORG_ID,A.ZZ_UPORG_ID,OrgLevel = B.OrgLevel + 1,FullOrgName=B.FullOrgName+'/'+a.ZZ_ORG_SHORT from ZHR_ORG A,cte B  where A.ZZ_UPORG_ID = B.ZZ_ORG_ID and A.ZZ_Enabled = 1)select ZZ_ORG_ID as orgID,FullOrgName as orgName  from cte  where ZZ_ORG_ID in(SELECT FOrg_ID FROM T_Resource_Authority WHERE FZAD= @UserDomainName and FOrg_ID in(select distinct FOrg_ID from [T_Resource_OrgProject]))";
            //string sql = "SELECT FOrg_ID as orgID,FOrg_Name as orgName FROM T_Resource_Authority WHERE FZAD=@UserDomainName"+
            //    " and FOrg_ID in(select distinct FOrg_ID from [T_Resource_OrgProject])";
            SqlParameter[] sqlParas={new SqlParameter("@UserDomainName",SqlDbType.NVarChar,20)};
            sqlParas[0].Value = userDomainName;
            //sqlParas[0].Value = @"tct-hq\yifeng.hu";
            DataSet ds = DataHelper.GetResourceData(sql, sqlParas);
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

        /// <summary>
        /// 获取当前用户可以设置的部门
        /// </summary>
        /// <param name="levelThreeName"></param>
        /// <returns></returns>
        private string GetBrachThreeJsonForProjectSet(string userDomainName)
        {
            string sql = ";with cte as(select ZZ_ORG_ID,ZZ_UPORG_ID,OrgLevel = 1,FullOrgName=cast(ZZ_ORG_SHORT as nvarchar(max)) from ZHR_ORG where ZZ_UPORG_ID = '00000010' and ZZ_Enabled = 1 union all select A.ZZ_ORG_ID,A.ZZ_UPORG_ID,OrgLevel = B.OrgLevel + 1,FullOrgName=B.FullOrgName+'/'+a.ZZ_ORG_SHORT from ZHR_ORG A,cte B  where A.ZZ_UPORG_ID = B.ZZ_ORG_ID and A.ZZ_Enabled = 1)select ZZ_ORG_ID as orgID,FullOrgName as orgName  from cte  where ZZ_ORG_ID in(SELECT FOrg_ID FROM T_Resource_Authority WHERE FZAD= @UserDomainName )";
            //string sql = "SELECT FOrg_ID as orgID,FOrg_Name as orgName FROM T_Resource_Authority WHERE FZAD=@UserDomainName"+
            //    " and FOrg_ID in(select distinct FOrg_ID from [T_Resource_OrgProject])";
            SqlParameter[] sqlParas = { new SqlParameter("@UserDomainName", SqlDbType.NVarChar, 20) };
            sqlParas[0].Value = userDomainName;
            //sqlParas[0].Value = @"tct-hq\yifeng.hu";
            DataSet ds = DataHelper.GetResourceData(sql, sqlParas);
            return JsonConvert.SerializeObject(ds.Tables[0]);
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