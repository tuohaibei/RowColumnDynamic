using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCL.Resources.Web.DataAPI.Ashx
{
    public class ResourceHttpContext
    {
        public static string DomainFullUserName
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }
    }
}