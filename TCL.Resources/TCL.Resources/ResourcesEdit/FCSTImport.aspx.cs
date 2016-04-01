using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TCL.Resources.Web.ResourcesEdit
{
    public partial class FCSTImport : System.Web.UI.Page
    {
        #region//变量
        /// <summary>
        /// 最大文件大小(MB)
        /// </summary>
        public string FileMaxSize = "10";
        /// <summary>
        /// 扩展的二进制名称
        /// </summary>
        public string AllowedFileExtensions = string.Empty;
        /// <summary>
        /// 扩展的二进制文件
        /// </summary>
        public string AllowedFileCodeList = string.Empty;
        public string UploadFilePath = System.Configuration.ConfigurationManager.AppSettings["UploadFile"];
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            GetFileExtension();
        }

        #region//获取扩展名和扩展的二进制文件
        /// <summary>
        /// 获取扩展名和扩展的二进制文件
        /// </summary>
        /// <param name="AllowedFileExtensions">扩展名</param>
        /// <param name="AllowedFileCode">扩展的二进制文件</param>
        public void GetFileExtension()
        {
            //扩展二进制文件
            StringBuilder sbFileCode = new StringBuilder();
            //
            StringBuilder sbFileExt = new StringBuilder();
            try
            {
                //
                string ExtensionList = ConfigurationManager.AppSettings["AllowedFileExtensions"] == null ?
                    "*.xls|208207,*.xlsx|8075" :
                    ConfigurationManager.AppSettings["AllowedFileExtensions"].ToString();
                //如果没有
                if (!string.IsNullOrEmpty(ExtensionList))
                {
                    //分离,
                    string[] FileExtensionsList = ExtensionList.Split(new char[] { ',' });
                    //
                    //
                    //分离|
                    foreach (string ItemList in FileExtensionsList)
                    {
                        //分离|
                        string[] ItemArrayList = ItemList.Split(new char[] { '|' });
                        //如果有值
                        if (ItemArrayList.Length > 0)
                        {
                            //扩展名
                            sbFileExt.Append(ItemArrayList[0] + ";");
                            //扩展文件的二进制
                            sbFileCode.Append(ItemArrayList[1] + ",");

                        }
                    }
                }
            }
            catch
            {
            }
            //赋值
            AllowedFileExtensions = sbFileExt.ToString().Trim(';');
            //
            AllowedFileCodeList = sbFileCode.ToString().Trim(',');
        }
        #endregion
    }
}