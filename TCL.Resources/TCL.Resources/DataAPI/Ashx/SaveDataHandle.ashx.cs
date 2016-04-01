using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TCL.Resources.BLL;
using System.Data.SqlClient;

namespace TCL.Resources.Web.DataAPI.Ashx
{
    /// <summary>
    /// SaveDataHandle 的摘要说明
    /// </summary>
    public class SaveDataHandle : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string orgProjects = context.Request.Params["orgProject"];
            //string message =
            SaveOrgProject(orgProjects);
            context.Response.ContentType = "text/plain";
            context.Response.Write("");
        }
        private void SaveOrgProject(string orgProjects)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(orgProjects);
            string orgID = jo["orgID"].ToString();
            JToken projectItems = jo["projectItems"];
            DataTable dt = prePareDT();
            foreach (var item in projectItems)
            {
                DataRow dr = dt.NewRow();
                dr[0] = 0;
                dr[1] = orgID;
                dr[2]=item["projectID"];
                dr[3] = ResourceHttpContext.DomainFullUserName;
                dr[4] = System.DateTime.Now;
                dr[5] = ResourceHttpContext.DomainFullUserName;
                dr[6] = System.DateTime.Now;
                dr[7] = item["Level1Name"];
                dr[8] = item["Level2Name"];
                dr[9] = 1;
                dt.Rows.Add(dr);
            }
            SqlParameter[] paras={new SqlParameter("@Resource_OrgProjectTable",dt),
                                     new SqlParameter("@Message",SqlDbType.NVarChar,-1),
                                     new SqlParameter("@isSuccess",SqlDbType.Int)};
            paras[1].Direction=ParameterDirection.Output;
            paras[2].Direction=ParameterDirection.Output;
            DataHelper.SavaBulkData("proc_SaveResourceProject", dt, paras);
            string a = paras[2].Value.ToString();
            string b = paras[1].Value.ToString();
        }
        private DataTable prePareDT()
        {  
            DataTable dt = new DataTable("dt");
            DataColumn[] dtc = new DataColumn[10];
            dtc[0] = new DataColumn("ID", typeof(Int32));
            dtc[1] = new DataColumn("FOrg_ID", typeof(string));
            dtc[2] = new DataColumn("FProject_ID", typeof(string));
            dtc[3] = new DataColumn("FCreator_ZAD", typeof(string));
            dtc[4] = new DataColumn("FCreate_DateTime", typeof(DateTime));
            dtc[5] = new DataColumn("FModify_FZAD", typeof(string));
            dtc[6] = new DataColumn("FModofy_DateTime", typeof(DateTime));
            dtc[7] = new DataColumn("FLevel1_Name", typeof(string));
            dtc[8] = new DataColumn("FLevel2_Name", typeof(string));
            dtc[9] = new DataColumn("FIsEnable", typeof(Int32));
            dt.Columns.AddRange(dtc);
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