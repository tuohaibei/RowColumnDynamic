using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TCL.Resources.Common;
using TCL.Resources.DAL;
using TCL.Resources.Entity;

namespace TCL.Resources.BLL
{
    public class DataHelper
    {
        public static List<T> GetResourceData<T>(string sql)
        {
            using (IDbConnection conn = SqlHelper.CreateConnection())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                List<T> reSources = conn.Query<T>(sql) as List<T>;
                return reSources;
            }

        }
        public static DataSet GetResourceData(string sql, SqlParameter[] sqlParas)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetResouceConnectionString(), CommandType.Text, sql, sqlParas);
            return ds;
        }
        public static string GetUserName(string domainUserName)
        {
            string result = string.Empty;
            string sql = "select [ZNAME] from [ZHRINFO] where ZAD=@DomainUserName";
            SqlParameter[] sqlPara ={
            new SqlParameter("@DomainUserName",SqlDbType.NVarChar,20)
            };
            sqlPara[0].Value = domainUserName;
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetResouceConnectionString(), CommandType.Text, sql, sqlPara);
            if (ds.Tables[0].Rows.Count > 0)
            {
                result = ds.Tables[0].Rows[0]["ZNAME"].ToString();
            }
            return result;
        }
        public static bool IsExistAuthority()
        {
            string result = string.Empty;
            string sql = "select count(*) as num from [T_Resource_Authority] where FZAD=@DomainUserName";
            SqlParameter[] sqlPara ={
            new SqlParameter("@DomainUserName",SqlDbType.NVarChar,50)
            };
            sqlPara[0].Value ="tct-hq\\binyu.cui";
            int count = (int)SqlHelper.ExecuteScalar(SqlHelper.GetResouceConnectionString(), CommandType.Text, sql, sqlPara);
            return count > 0;
        }

        /// <summary>
        /// 获取用户组织权限
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static List<ResourceOrgProject> GetUserAuthority(string userName)
        {
            string result = string.Empty;
            string sql = "  select a.FOrg_ID as OrgID,b.FProject_ID as ProjectID from [T_Resource_Authority] a left join T_Resource_OrgProject b on a.FOrg_ID=b.FOrg_ID where a.FZAD=@DomainUserName";
            SqlParameter[] sqlPara ={
            new SqlParameter("@DomainUserName",SqlDbType.NVarChar,50)
            };
            sqlPara[0].Value =  userName;
            List<ResourceOrgProject> list = EntityHelper.FillList<ResourceOrgProject>(sql,sqlPara);
            return list;
        }
        /// <summary>
        /// 使用存储过程批量插入数据
        /// </summary>
        public static void SavaBulkData(string procedureName, DataTable dt,SqlParameter[] sqlParas)
        {
            using (SqlConnection conn = SqlHelper.CreateConnection())
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = procedureName;
                conn.Open();
                SqlHelper.ExecuteNonQuery(SqlHelper.GetResouceConnectionString(), CommandType.StoredProcedure, procedureName, sqlParas);
            }
        }
    }
}