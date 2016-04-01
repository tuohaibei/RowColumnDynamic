using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TCL.Resources.Common;
using TCL.Resources.Entity;
using TCL.Resources.DAL;

namespace TCL.Resources.BLL
{
    public class OrgProjectBLL
    {
        public List<OrgProject> GetOrgProjectList(string orgID, int startYear, int startMonth, int endYear, int endMonth, bool isContainOutsource)
        {
            string spName = "Proc_GetOrgProjectList";
            SqlParameter[] sqlParameters = { 
                                               new SqlParameter("@OrgID",orgID),
                                               new SqlParameter("@StartYear",startYear),
                                               new SqlParameter("@startMonth",startMonth),
                                               new SqlParameter("@endYear",endYear),
                                               new SqlParameter("@endMonth",endMonth),
                                               new SqlParameter("@ContainsOutsource",isContainOutsource)
                                           };

            //DataSet dt = SqlHelper.ExecuteDataset(SqlHelper.GetResouceConnectionString(), CommandType.StoredProcedure, spName, sqlParameters);
            List<OrgProject> result = EntityHelper.FillListByProc<OrgProject>(spName, sqlParameters);
            //JsonConvert.SerializeObject(result);
            //context.Response.Write(result);
            //context.Response.End();
            return result;
        }

        public List<BudgetDetail> GetResourceBudgetDetail(string orgID, int startYear, int startMonth, int endYear, int endMonth, bool isContainsOutsource)
        {
            string spName = "Proc_GetResourceBudgetDetail";
            SqlParameter[] sqlParameters = { 
                                               new SqlParameter("@OrgID",orgID),
                                               new SqlParameter("@StartYear",startYear),
                                               new SqlParameter("@startMonth",startMonth),
                                               new SqlParameter("@endYear",endYear),
                                               new SqlParameter("@endMonth",endMonth),
                                               new SqlParameter("@ContainsOutsource",isContainsOutsource)
                                           };

            List<BudgetDetail> result = EntityHelper.FillListByProc<BudgetDetail>(spName, sqlParameters);
            return result;
        }

        public string SaveResourceBudgetDetail(string userAD, List<TCL.Resources.Entity.BudgetDetailTable> budgetDetailList)
        {
            string spName = "Proc_SaveResourceBudgetDetail";
            SqlParameter[] sqlParameters = { 
                                               new SqlParameter("@UserAD",userAD),
                                               new SqlParameter("@T_Resource_BudgetDetail_Table",budgetDetailList.ToDataTableFromIEnumerable()),
                                               new SqlParameter("@IsSuccess",SqlDbType.Int),
                                               new SqlParameter("@Message",SqlDbType.NVarChar,-1)
                                           };
            sqlParameters[2].Direction = ParameterDirection.Output;
            sqlParameters[3].Direction = ParameterDirection.Output;
            try
            {
                TCL.Resources.DAL.SqlHelper.ExecuteNonQuery(SqlHelper.GetResouceConnectionString(), CommandType.StoredProcedure, spName, sqlParameters);
            }
            catch (Exception e)
            {

            }
            string ret = string.Empty;
            if (sqlParameters[3].Value != null)
            {
                ret = sqlParameters[3].Value.ToString();
            }
            return ret;
        }
        public List<BudgetDetail> GetResourceBudgetYearMonth(string orgID, string startDate, string endDate, string projectID)
        {
            string spName = "Proc_GetResourceBudgetYearMonth";
            SqlParameter[] sqlParameters = { 
                                               new SqlParameter("@OrgID",orgID),
                                               new SqlParameter("@StartDate",startDate),
                                               new SqlParameter("@EndDate",endDate),
                                               new SqlParameter("@ProjectID",projectID)
                                           };

            List<BudgetDetail> result = EntityHelper.FillListByProc<BudgetDetail>(spName, sqlParameters);
            return result;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="orgID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataSet GetResourceBudgetYearMonthForQuery(string orgID, string startDate, string endDate, string projectID)
        {
            string spName = "Proc_GetResourceBudgetDetailByQuery";
            SqlParameter[] sqlParameters = { 
                                               new SqlParameter("@OrgID",orgID),
                                               new SqlParameter("@StartDate",startDate),
                                               new SqlParameter("@EndDate",endDate),
                                               new SqlParameter("@ProjectID",projectID)
                                           };
           DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetResouceConnectionString(),CommandType.StoredProcedure,spName, sqlParameters);
           return ds;
        }

        /// <summary>
        /// 批量导入数据
        /// </summary>
        /// <param name="userAD"></param>
        /// <param name="budgetDetailList"></param>
        /// <returns></returns>
        public string SaveResourceBudgetDetailForImport(string userAD, List<TCL.Resources.Entity.BudgetDetailTable> budgetDetailList)
        {
            string spName = "Proc_SaveResourceBudgetDetailForImport";
            SqlParameter[] sqlParameters = { 
                                               new SqlParameter("@UserAD",userAD),
                                               new SqlParameter("@T_Resource_BudgetDetail_Table",budgetDetailList.ToDataTableFromIEnumerable()),
                                               new SqlParameter("@IsSuccess",SqlDbType.Int),
                                               new SqlParameter("@Message",SqlDbType.NVarChar,-1)
                                           };
            sqlParameters[2].Direction = ParameterDirection.Output;
            sqlParameters[3].Direction = ParameterDirection.Output;
            try
            {
                TCL.Resources.DAL.SqlHelper.ExecuteNonQuery(SqlHelper.GetResouceConnectionString(), CommandType.StoredProcedure, spName, sqlParameters);
            }
            catch (Exception e)
            {

            }
            string ret = string.Empty;
            if (sqlParameters[3].Value != null)
            {
                ret = sqlParameters[3].Value.ToString();
            }
            return ret;
        }
    }
}