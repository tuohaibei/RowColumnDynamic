using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace TCL.Resources.Common
{
    public class JsonHelper
    {
        public static string DataTable2JSON(DataTable dt, string tableName)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"" + tableName + "\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0)
                {
                    jsonBuilder.Append(",");
                }
                jsonBuilder.Append("{");
                jsonBuilder.Append("\"no\":\"" + i + "\",");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j > 0)
                    {
                        jsonBuilder.Append(",");
                    }
                    if (dt.Columns[j].DataType.Equals(typeof(DateTime)) && dt.Rows[i][j].ToString() != "")
                    {
                        jsonBuilder.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\": \"" 
                            + Convert.ToDateTime(dt.Rows[i][j].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "\"");
                    }
                    else if (dt.Columns[j].DataType.Equals(typeof(String)))
                    {
                        jsonBuilder.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\": \"" 
                            + dt.Rows[i][j].ToString().Replace("\\", "\\\\").Replace("\'", "\\\'").Replace("\t", " ").Replace("\r", " ").Replace("\n", "<br/>") + "\"");
                    }
                    else
                    {
                        jsonBuilder.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\": \"" + dt.Rows[i][j].ToString() + "\"");
                    }
                }
                jsonBuilder.Append("}");
            }
            jsonBuilder.Append("]}");
            return jsonBuilder.ToString();
        }

        public static string GetFormJSON(string strJson)
        {
            #region 屏蔽
            //DataTable dt = null;
            //Hashtable ht = new Hashtable();

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string[] jsonData = serializer.Deserialize<string[]>(strJson);
            //if (jsonData.Length > 0)
            //{
            //    for (int i = 0; i < jsonData.Length; i++)
            //    {

            //    }
            //}

            //return string.Empty; 
            #endregion

            //string orgID = Request
            return string.Empty;
        }

    }
}