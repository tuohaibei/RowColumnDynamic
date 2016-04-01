using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using TCL.Resources.BLL;

namespace TCL.Resources.Web.DataAPI.Ashx
{
    /// <summary>
    /// UserHandle 的摘要说明
    /// </summary>
    public class UserHandle : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            result = GetUserName();
            context.Response.ContentType = "json/plain";
            context.Response.Write(result);
        }
        /// <summary>
        ///获取当前登录用户名称
        /// </summary>
        /// <returns></returns>
        private string GetUserName()
        {
            string domainFullName = ResourceHttpContext.DomainFullUserName;
            string userName = DataHelper.GetUserName(domainFullName);
            string newJson = string.Empty;
            if (null != userName)
            {
                newJson = "{" + "\"domainfullname\":" + "\"" + domainFullName.Replace("\\", "\\\\") +"\"," + "\"username\":" + "\"" + userName + "\"" + "}";//注意要将单引号替换为双引号否则发送到客户端的数据字符串中单引号不被识别
            }
            return JsonConvert.SerializeObject(newJson);
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