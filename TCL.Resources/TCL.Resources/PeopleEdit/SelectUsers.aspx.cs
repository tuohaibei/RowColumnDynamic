using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TCL.EP.BPM.ASPX.PeopleEdit
{
    public partial class SelectUsers : System.Web.UI.Page
    {
        protected string SelectUserHtml
        {
            get
            {
                System.Text.StringBuilder oSb = new System.Text.StringBuilder();

                oSb.Append("     <div class=\"easyui-layout\" style=\"width: 100%; height: 400px;\">");
                oSb.Append("         <div id=\"divTreeView\" class=\"divSrollbar\" region=\"west\" split=\"true\" title=\"组织结构\" style=\"width: 250px;&#xA;            padding-left: 5px; padding-top: 5px;\">");
                oSb.Append("              <ul id=\"OUTreeViewContainer\">");
                oSb.Append("              </ul>");
                oSb.Append("         </div>");
                oSb.Append("         <div id=\"divPersonInfo\" class=\"divSrollbar\" region=\"center\" title=\"人员信息\" style=\"background: #fafafa;&#xA;            overflow: auto;\">");
                oSb.Append("         </div>");
                oSb.Append("    </div>");

                return oSb.ToString();
            }
        }

        protected string SelectOuHtml
        {
            get
            {
                System.Text.StringBuilder oSb = new System.Text.StringBuilder();

                oSb.Append("     <div id=\"p\" class=\"easyui-panel\" title=\"组织结构\" style=\"width: 675px; height: 400px;&#xA;        padding: 10px; background: #fafafa;\">");
                oSb.Append("         <ul id=\"OUTreeViewContainer\">");
                oSb.Append("         </ul>");
                oSb.Append("    </div>");
                return oSb.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString().ToLower()=="department")
            {
                this.mainContent.InnerHtml = SelectOuHtml;
            }
            else
            {
                this.mainContent.InnerHtml = SelectUserHtml;
            }
        }
    }
}