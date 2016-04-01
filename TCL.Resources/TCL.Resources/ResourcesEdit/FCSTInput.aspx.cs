using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCL.Resources.BLL;

namespace TCL.Resources.Web.ResourcesEdit
{
    public partial class FCSTInput : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string currentUsername = Page.User.Identity.Name;
                UserName = DataHelper.GetUserName(currentUsername);
                //DataHelper.ge
            }
        }
        public string UserName
        {
            get;
            set;
        }
    }
}