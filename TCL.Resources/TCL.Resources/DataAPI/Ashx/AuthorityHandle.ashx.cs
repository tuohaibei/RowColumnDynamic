using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using TCL.Resources.BLL;

namespace TCL.Resources.Web.DataAPI.Ashx
{
    /// <summary>
    /// AuthorityHandle 的摘要说明
    /// </summary>
    public class AuthorityHandle : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            bool isExist=false;
            //string domainFullName=context.Request["domainfullname"];
            //domainFullName = HttpUtility.UrlDecode(domainFullName);
            isExist = DataHelper.IsExistAuthority();
            context.Response.ContentType = "text/plain";
            context.Response.Write(isExist.ToString());
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