using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runmont.Net.Components.jQuery;
using Runmont.Net.Components.Utilities;
using System.Data;
using System.Data.SqlClient;
using CDXR.EIP.DataCenter.DataAccess;
using TCL.Resources.Common;
using TCL.Resources.Entity;
using Newtonsoft.Json;
namespace TCL.Resources.Web.DataAPI.Ashx
{
    /// <summary>
    /// OUTreeViewHandler 的摘要说明
    /// </summary>
    public class OUTreeViewHandler : IHttpHandler
    {
        /// <summary>
        /// 您将需要在您网站的 web.config 文件中配置此处理程序，
        /// 并向 IIS 注册此处理程序，然后才能进行使用。有关详细信息，
        /// 请参见下面的链接: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // 如果无法为其他请求重用托管处理程序，则返回 false。
            // 如果按请求保留某些状态信息，则通常这将为 false。
            get { return true; }
        }


        public void ProcessRequest(HttpContext context)
        {
            jQueryTreeNode rootNode = new jQueryTreeNode();
            DataAccess bll = new DataAccess();
            string result = "";
            if (!String.IsNullOrEmpty(context.Request["id"]))
            {
                rootNode.id = context.Request["id"];
                BuildTree(rootNode, bll);
                result = rootNode.ConvertChildrenToJsonString();
            }
            else if (!String.IsNullOrEmpty(context.Request["parentId"]))
            {
                string parentId = context.Request["parentId"];
                result = GetOrgListJson(parentId, bll);
            }
            else if (!string.IsNullOrEmpty(context.Request["queryKey"]) && string.IsNullOrEmpty(context.Request["requestType"]))
            {
                string queryKey = context.Request["queryKey"];
                result = GetQueryResultJson(queryKey, bll);
            }
            else if (!string.IsNullOrEmpty(context.Request["queryKey"]) && !string.IsNullOrEmpty(context.Request["requestType"]))
            {
                string queryKey = context.Request["queryKey"];
                result = GetQueryOUJson(queryKey, bll);
            }
            else if (!String.IsNullOrEmpty(context.Request["zorgid"]))
            {
                string orgid= context.Request["zorgid"];
                var org= GetZhrOrg(orgid);
                result = JsonConvert.SerializeObject(org);
            }
            else
            {
                InitTree(bll, rootNode);
                result = rootNode.ConvertChildrenToJsonString();
            }
            context.Response.ContentType = "application/json";
            context.Response.Charset = "utf-8";
            context.Response.Write(result);
            context.Response.End();
        }

        protected SAPOrganizationInformation GetZhrOrg(string orgID)
        {
            string sql = string.Format("select * from dbo.ZHR_ORG where ZZ_ORG_ID='{0}'",orgID);
            return EntityHelper.FillList<SAPOrganizationInformation>(sql)[0];
        }

        protected void InitTree(DataAccess bll, jQueryTreeNode root)
        {
            jQueryTreeNode rootNode = new jQueryTreeNode();
            NodeEntity ouRootNode = bll.GetRootNode();
            rootNode.text = ouRootNode.OUName;
            rootNode.id = ouRootNode.OUID;
            rootNode.attributes.Add("ADAccount", ouRootNode.ADAccount);
            // root.children.Add(rootNode);
            foreach (var item in bll.GetChildrenNode(ouRootNode.OUID))
            {
                jQueryTreeNode node = new jQueryTreeNode();
                ConvertNodeToJqueryTreeNode(item, node);
                rootNode.children.Add(node);
                rootNode.status = jQueryTreeNodeState.open;
            }
            root.children.Add(rootNode);
        }

        protected void BuildTree(jQueryTreeNode node, DataAccess bll)
        {
            string parentid = node.id;
            foreach (NodeEntity item in bll.GetChildrenNode(parentid))
            {
                jQueryTreeNode treeNode = new jQueryTreeNode();
                ConvertNodeToJqueryTreeNode(item, treeNode);
                node.children.Add(treeNode);
            }
        }

        protected string GetUserListJson(string parentId, DataAccess bll)
        {
            string result = "";
            List<UserEntity> list = bll.GetUserList(parentId);
            result = JsonUtility.Serialize(typeof(UserEntity), list);
            return result;
        }
        protected string GetOrgListJson(string parentID,DataAccess bll)
        {
            string result = string.Empty;
            List<NodeEntity> orgList = bll.GetChildrenNode(parentID);
            result = JsonUtility.Serialize(typeof(NodeEntity), orgList);
            return result;

        }

        protected void ConvertNodeToJqueryTreeNode(NodeEntity node, jQueryTreeNode jNode)
        {
            jNode.id = node.OUID;
            jNode.text = node.OUName;
            jNode.attributes.Add("ADAccount", node.ADAccount);
        }

        protected string GetQueryResultJson(string queryKey, DataAccess bll)
        {
            string result = "";
            List<UserEntity> list = bll.QueryEntity(queryKey);
            result = JsonUtility.Serialize(typeof(UserEntity), list);
            return result;
        }
        protected string GetQueryOUJson(string queryKey, DataAccess bll)
        {
            string result = "";
            List<UserEntity> list = bll.QueryOUEntity(queryKey);
            result = JsonUtility.Serialize(typeof(UserEntity), list);
            return result;
        }
        #endregion
    }
    #region 实体类
    public class NodeEntity
    {
        public string OUID { get; set; }
        public string OrgShortName { get; set; }
        public string OUName { get; set; }
        public string ParentOUID { get; set; }
        public string ParentOrgName { get; set; }
        public string ADAccount { get; set; }


    }

    public class UserEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string AdAccount { get; set; }
        public string Post { get; set; }
        public string Telephone { get; set; }
        public string SapNumber { get; set; }
    }
    #endregion 实体类

    #region 数据获取
    public class DataAccess
    {
        public NodeEntity GetRootNode()
        {
            NodeEntity root = new NodeEntity();
            DataSet ds = null;
            using (SqlConnection conn = new SqlConnection(Config.Instance.GetDbString()))
            {
                conn.Open();
                ds = SqlData.ExecuteDataset(conn, CommandType.StoredProcedure, "proc_getRootNode");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    root.OUID = dr["ZZ_ORG_ID"] is DBNull ? "" : Convert.ToString(dr["ZZ_ORG_ID"]);
                    root.OUName = dr["ZZ_ORG_TXT"] is DBNull ? "" : Convert.ToString(dr["ZZ_ORG_TXT"]);
                    root.ParentOUID = dr["ZZ_UPORG_ID"] is DBNull ? "" : Convert.ToString(dr["ZZ_UPORG_ID"]);
                    root.ADAccount = dr["ZZ_ADID"] is DBNull ? "" : Convert.ToString(dr["ZZ_ADID"]);
                }
                conn.Close();
            }
            return root;
        }

        public List<NodeEntity> GetChildrenNode(string parentId)
        {
            DataSet ds = null;
            List<NodeEntity> list = new List<NodeEntity>();
            SqlParameter[] sqlParameter =
                {
                    new SqlParameter("@parentId", parentId),
                };
            using (SqlConnection conn = new SqlConnection(Config.Instance.GetDbString()))
            {
                conn.Open();
                ds = SqlData.ExecuteDataset(conn, CommandType.StoredProcedure, "proc_getChildrenNode", sqlParameter);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        NodeEntity node = new NodeEntity();
                        node.OUID = dr["ZZ_ORG_ID"] is DBNull ? "" : Convert.ToString(dr["ZZ_ORG_ID"]);
                        node.OUName = dr["ZZ_ORG_TXT"] is DBNull ? "" : Convert.ToString(dr["ZZ_ORG_TXT"]);
                        node.ParentOUID = dr["ZZ_UPORG_ID"] is DBNull ? "" : Convert.ToString(dr["ZZ_UPORG_ID"]);
                        node.ADAccount = dr["ZZ_ADID"] is DBNull ? "" : Convert.ToString(dr["ZZ_ADID"]);
                        node.OrgShortName = dr["ZZ_ORG_SHORT"] is DBNull ? "" : Convert.ToString(dr["ZZ_ORG_SHORT"]);
                        node.ParentOrgName = dr["ZZ_UPORG_TEXT"] is DBNull ? "" : Convert.ToString(dr["ZZ_UPORG_TEXT"]);
                        list.Add(node);
                    }
                }
                conn.Close();
            }
            return list;
        }

        public List<UserEntity> GetUserList(string parentId)
        {
            DataSet ds = null;
            List<UserEntity> list = new List<UserEntity>();
            SqlParameter[] sqlParameter =
                {
                    new SqlParameter("@parentId", parentId),
                };
            using (SqlConnection conn = new SqlConnection(Config.Instance.GetDbString()))
            {
                conn.Open();
                ds = SqlData.ExecuteDataset(conn, CommandType.StoredProcedure, "proc_getUsers", sqlParameter);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        UserEntity user = new UserEntity();
                        user.AdAccount = dr["ZAD"] is DBNull ? "" : Convert.ToString(dr["ZAD"]);
                        user.Email = dr["ZPMAIL"] is DBNull ? "" : Convert.ToString(dr["ZPMAIL"]);
                        user.Name = BuildUserName(dr["ZNAME"] is DBNull ? "" : Convert.ToString(dr["ZNAME"]), dr["ZFNAME"] is DBNull ? "" : Convert.ToString(dr["ZFNAME"]), dr["ZLNAME"] is DBNull ? "" : Convert.ToString(dr["ZLNAME"]));
                        user.Post = dr["ZPOSTXT"] is DBNull ? "" : Convert.ToString(dr["ZPOSTXT"]);
                        user.Telephone = dr["ZTEL"] is DBNull ? "" : Convert.ToString(dr["ZTEL"]);
                        user.SapNumber = dr["ZSAPNO"] is DBNull ? "" : Convert.ToString(dr["ZSAPNO"]);
                        list.Add(user);
                    }

                }
                conn.Close();
            }
            return list;
        }

        public List<UserEntity> QueryEntity(string key)
        {
            List<UserEntity> list = new List<UserEntity>();
            List<UserEntity> userList = QueryUserEntity(key);
            List<UserEntity> ouList = QueryOUEntity(key);
            list.AddRange(userList);
            list.AddRange(ouList);
            return list;
        }

        public List<UserEntity> QueryOUEntity(string key)
        {
            key = "%" + key + "%";
            DataSet ds = null;
            List<UserEntity> list = new List<UserEntity>();
            SqlParameter[] sqlParameter =
                {
                    new SqlParameter("@querykey", key),
                };
            using (SqlConnection conn = new SqlConnection(Config.Instance.GetDbString()))
            {
                conn.Open();
                ds = SqlData.ExecuteDataset(conn, CommandType.StoredProcedure, "proc_queryOU", sqlParameter);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        UserEntity node = new UserEntity();
                        node.SapNumber = dr["ZZ_ORG_ID"] is DBNull ? "" : Convert.ToString(dr["ZZ_ORG_ID"]);
                        node.Name = dr["ZZ_ORG_TXT"] is DBNull ? "" : Convert.ToString(dr["ZZ_ORG_TXT"]);
                        node.AdAccount = dr["ZZ_ADID"] is DBNull ? "" : Convert.ToString(dr["ZZ_ADID"]);
                        list.Add(node);
                    }
                }
                conn.Close();
            }
            return list;
        }

        public List<UserEntity> QueryUserEntity(string key)
        {
            key = "%" + key + "%";
            DataSet ds = null;
            List<UserEntity> list = new List<UserEntity>();
            SqlParameter[] sqlParameter =
                {
                    new SqlParameter("@querykey", key),
                };
            using (SqlConnection conn = new SqlConnection(Config.Instance.GetDbString()))
            {
                conn.Open();
                ds = SqlData.ExecuteDataset(conn, CommandType.StoredProcedure, "proc_queryUsers", sqlParameter);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        UserEntity user = new UserEntity();
                        user.AdAccount = dr["ZAD"] is DBNull ? "" : Convert.ToString(dr["ZAD"]);
                        user.Email = dr["ZPMAIL"] is DBNull ? "" : Convert.ToString(dr["ZPMAIL"]);
                        user.Name = dr["ZNAME"] is DBNull ? "" : Convert.ToString(dr["ZNAME"]);
                        user.Post = dr["ZPOSTXT"] is DBNull ? "" : Convert.ToString(dr["ZPOSTXT"]);
                        user.Telephone = dr["ZTEL"] is DBNull ? "" : Convert.ToString(dr["ZTEL"]);
                        user.SapNumber = dr["ZSAPNO"] is DBNull ? "" : Convert.ToString(dr["ZSAPNO"]);
                        list.Add(user);
                    }

                }
                conn.Close();
            }
            return list;
        }
        /// <summary>
        /// 组合用户显示名字，格式为 ZNAME(ZFNAME ZLNAME)
        /// </summary>
        /// <param name="ZNAME">中文名</param>
        /// <param name="ZFNAME">first name</param>
        /// <param name="ZLNAME">last name</param>
        /// <returns></returns>
        public string BuildUserName(string ZNAME, string ZFNAME, string ZLNAME)
        {
            if (string.IsNullOrEmpty(ZFNAME) && string.IsNullOrEmpty(ZLNAME))
            {
                return ZNAME;
            }
            else if (!string.IsNullOrEmpty(ZFNAME) && string.IsNullOrEmpty(ZLNAME))
            {
                return string.Format("{0}({1})", ZNAME, ZFNAME);
            }
            else if (string.IsNullOrEmpty(ZFNAME) && !string.IsNullOrEmpty(ZLNAME))
            {
                return string.Format("{0}({1})", ZNAME, ZLNAME);
            }
            else if (!string.IsNullOrEmpty(ZFNAME) && !string.IsNullOrEmpty(ZLNAME))
            {
                return string.Format("{0}({1} {2})", ZNAME, ZFNAME, ZLNAME);
            }
            return ZNAME;
        }
    }
    #endregion 数据获取
}