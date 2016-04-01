using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using Newtonsoft.Json;
using ICSharpCode.SharpZipLib;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Data;
using System.Collections;
using System.Text;
using TCL.Resources.Entity;
using TCL.Resources.Common;
using TCL.Resources.BLL;

namespace TCL.Resources.Web.DataAPI.Ashx
{
    /// <summary>
    /// FCSTExport 的摘要说明
    /// </summary>
    public class FCSTExport : IHttpHandler
    {
        string loginName="";
        public void ProcessRequest(HttpContext context)
        {
            loginName = context.User.Identity.Name;
            string type = context.Request.Params.GetValues("Type")[0];
            context.Response.ContentType = "text/plain";
            if (type.Equals("export"))
            {
                string jsonData = context.Request.Params.GetValues("ResourcePlanJson")[0];
                List<Plan> detailList = JsonConvert.DeserializeObject<List<Plan>>(jsonData);
                string outfilename = "";
                ExportExcel(context, detailList, "resourceplan", out outfilename);
                string result = JsonConvert.SerializeObject(outfilename);
                context.Response.Write(result);
              
                context.Response.End();
            }
            else if(type.Equals("UploadFile"))
            {
                UploadFile(context);
            }
            else if (type.Equals("import"))
            {
                ImportFCST(context);
            }
        }

        public void ImportFCST(HttpContext context)
        {
            string jsonData = context.Request.Params.GetValues("ResourcePlanJson")[0];
            List<TCL.Resources.Entity.BudgetDetailTable> detailList = JsonConvert.DeserializeObject<List<TCL.Resources.Entity.BudgetDetailTable>>(jsonData);
            TCL.Resources.BLL.OrgProjectBLL orgProjectBLL = new BLL.OrgProjectBLL();
            string saveResult = orgProjectBLL.SaveResourceBudgetDetailForImport(loginName, detailList);
            context.Response.Write(saveResult);
            context.Response.End();
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="context"></param>
        public void UploadFile(HttpContext context)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值 
            FCSTExportResult result = new FCSTExportResult();
            try
            {
                result.ErrMsg = "";
                //上传路径
                string UploadFilePath = context.Request.Params["UploadFilePath"]; //上传文件的路径
                //fileType  附件类型
                string attachType = context.Request.Params["AttachType"];
                //变成实际上传路径
                string Path = context.Server.MapPath(UploadFilePath);
                //如果路径不存在，则创建
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
                HttpPostedFile uploadFile =null;
                if(context.Request.Files.Count>0)
                {
                     uploadFile = context.Request.Files[0];
                     // 判断文件大小(解决IE不能在客户端JS获取大小的问题)
                     if (FilesStatus.ConvertBytesToMegabytes(uploadFile.ContentLength) > double.Parse(context.Request.Params["FileMaxSize"]))
                     {
                         result.ErrMsg = "Please upload a smaller file, max size is " + context.Request.Params["FileMaxSize"] + " MB";
                     }
                     else
                     {
                         //得到文件名
                         string fileName = uploadFile.FileName;
                         //保存文件
                         if (fileName.LastIndexOf(".") > -1)
                         {
                             string ext = fileName.Substring(fileName.LastIndexOf("."));
                             //保存的文件名
                             string fileId = "resourceplanimport_" + DateTime.Now.ToString("yyyyMMddHH") + ext;
                             //保存的文件名
                             string fileSaveFileName = Path + fileId;
                             //保存到数据的路径和文件，这里只用相对路径
                             string fileSaveDataBaseName = UploadFilePath + fileId;
                             //保存
                             uploadFile.SaveAs(fileSaveFileName);

                             DataTable dt = ExcelToDataTable(fileSaveFileName, true);
                             ArrayList arrayList = new ArrayList();
                             List<ResourceProject> projectList = GetEPProject(); //取所有项目
                             List<OrgProject> orgList = GetZhrOrg(); //取所有组织
                             List<ResourceOrgProject> authlist = DataHelper.GetUserAuthority(loginName); //取用户权限
                             string projectName = "", orgLevelName = "", TipMsg = "";
                             ResourceProject project;
                             OrgProject org; ResourceOrgProject orgProject;
                             Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合  
                             foreach (DataRow dataRow in dt.Rows)
                             {
                                 orgLevelName = "";
                                 dictionary = new Dictionary<string, object>();
                                 foreach (DataColumn dataColumn in dt.Columns)
                                 {
                                     dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName]);
                                 }
                                 if (dataRow["Level1"] != null)
                                 {
                                     orgLevelName += dataRow["Level1"].ToString().Replace("_Outsource", "") + "/";
                                 }
                                 if (dataRow["Level2"] != null)
                                 {
                                     orgLevelName += dataRow["Level2"].ToString().Replace("_Outsource", "") + "/";
                                 }
                                 if (dataRow["Level3"] != null)
                                 {
                                     orgLevelName += dataRow["Level3"].ToString().Replace("_Outsource", "") + "/";
                                 }
                                 if (dataRow["Level4"] != null)
                                 {
                                     orgLevelName += dataRow["Level4"].ToString().Replace("_Outsource", "") + "/";
                                 }
                                 orgLevelName = !string.IsNullOrEmpty(orgLevelName) ? orgLevelName.TrimEnd(new char[] { '/' }) : "";
                                 dictionary.Add("isouthouse", dataRow["Level4"] != null && dataRow["Level4"].ToString().Contains("Outsource") ? true : false); //是否外部项目
                                 projectName = dataRow["Project"] != null ? dataRow["Project"].ToString() : "";
                                 project = projectList.Where(p => p.ProjectName.Equals(projectName)).FirstOrDefault();
                                 dictionary.Add("projectid", project != null ? project.EProjectID : "0"); //添加项目ID
                                 org = orgList.Where(p => p.OrgLvl4Txt.Equals(orgLevelName)).FirstOrDefault();
                                 dictionary.Add("zorgid", org != null ? org.OrgLvl4 : "0"); //添加组织ID
                                 string msg = "";
                                 //验证组织是否存在 且是否有权限
                                 if (org == null)
                                 {
                                     msg += "【" + orgLevelName + "】不存在 ";
                                     if (!TipMsg.Contains("【" + orgLevelName + "】不存在"))
                                     {
                                         TipMsg += msg + "<br/>";
                                     }

                                 }
                                 else
                                 {
                                     orgProject = authlist.Where(p => p.OrgID.Equals(org.OrgLvl3)).FirstOrDefault();
                                     if (orgProject == null)
                                     {
                                         msg += "组织【" + orgLevelName + "】无导入权限 ";
                                         if (!TipMsg.Contains("组织【" + orgLevelName + "】无导入权限"))
                                         {
                                             TipMsg += msg + "<br/>";
                                         }
                                     }
                                     else
                                     {
                                         if (project != null)
                                         {
                                             orgProject = authlist.Where(p => p.OrgID.Equals(org.OrgLvl3) && p.ProjectID.Equals(project.EProjectID)).FirstOrDefault();
                                             if (orgProject == null)
                                             {
                                                 msg += "【" + orgLevelName + "】下该项目【" + dataRow["Project"] + "】未配置";
                                                 if (!TipMsg.Contains("【" + orgLevelName + "】下该项目【" + dataRow["Project"] + "】未配置"))
                                                 {
                                                     TipMsg += msg + "<br/>";
                                                 }
                                             }
                                         }
                                     }
                                 }
                                 if (project == null)
                                 {
                                     msg = "项目【" + dataRow["Project"] + "】不存在";
                                     if (!TipMsg.Contains("项目【" + dataRow["Project"] + "】不存在"))
                                     {
                                         TipMsg += msg + "<br/>";
                                     }
                                 }
                                 dictionary.Add("msg", msg);
                                 arrayList.Add(dictionary); //ArrayList集合中添加键值     
                             }
                             List<GridClass> fozenColumn = new List<GridClass>(); //冻结列
                             List<GridClass> Column = new List<GridClass>();  //非冻结列
                             foreach (DataColumn dataColumn in dt.Columns)
                             {
                                 GridClass field = new GridClass() { field = dataColumn.ColumnName, title = dataColumn.ColumnName,width=80 };
                                 if (dataColumn.ColumnName.IndexOf('-') > 0)
                                 {
                                     Column.Add(field);
                                 }
                                 else
                                 {
                                     fozenColumn.Add(field);
                                 }
                             }

                             if (fozenColumn.Count() == 0 || Column.Count() == 0)
                             {
                                 result.ErrMsg = "上传文件格式有误，请使用导出的模板上传数据！！！";
                             }
                             result.TipMsg = TipMsg;
                             result.ColumnResult = arrayList;
                             result.Column = Column;
                             result.ForzenColumn = fozenColumn;

                             if (File.Exists(fileSaveFileName))
                             {
                                 File.Delete(fileSaveFileName);
                             }
                         }
                         else
                         {
                             result.ErrMsg = "请先上传格式正确的文件！！！";
                         }
                     }
                }
                else
                {
                    result.ErrMsg = "Please upload a  file ";
                }            }
            catch (Exception ex)
            {
                result.ErrMsg = "上传文件出现异常："+ex.ToString();
            }
            var jsonObj = javaScriptSerializer.Serialize(result);
            context.Response.ContentType = "text/html";
            context.Response.Write(jsonObj);
            context.Response.End();
        }

        /// <summary>
        /// 获取所有项目
        /// </summary>
        /// <returns></returns>
        public List<ResourceProject> GetEPProject()
        {
            string sql = "select [id] as EProjectID ,[ProjectName] as ProjectName from dbo.[EP_Project]";
            List<ResourceProject> list = EntityHelper.FillList<ResourceProject>(sql);
            return list;
        }

        /// <summary>
        /// 获取所有组织
        /// </summary>
        /// <returns></returns>
        public List<OrgProject> GetZhrOrg()
        {
            string sql = " ;with cte as(select ZZ_ORG_ID,OrgLevel = 1,ZZ_UPORG_ID,FullOrgName=cast(ZZ_ORG_SHORT as nvarchar(max)) from ZHR_ORG where ZZ_UPORG_ID = '00000010' and ZZ_Enabled = 1 union all select A.ZZ_ORG_ID,OrgLevel = B.OrgLevel + 1,A.ZZ_UPORG_ID,FullOrgName=B.FullOrgName+'/'+a.ZZ_ORG_SHORT from ZHR_ORG A,cte B  where A.ZZ_UPORG_ID = B.ZZ_ORG_ID and A.ZZ_Enabled = 1) select ZZ_UPORG_ID as OrgLvl3, ZZ_ORG_ID as OrgLvl4,FullOrgName as OrgLvl4Txt  from cte where OrgLevel>2";
            List<OrgProject> list = EntityHelper.FillList<OrgProject>(sql);
            return list;
        }

        #region 导入Excel
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string fileName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            FileStream fs = null;
            IWorkbook workbook = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);


                sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            DataColumn column = new DataColumn(firstRow.GetCell(i).StringCellValue);
                            data.Columns.Add(column);
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }
        #endregion

        #region 导出EXCEL
        /// <summary>
        /// 导出Excel方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tempTable">数据源</param>
        /// <param name="modelName">模板名称</param>
        /// <param name="beginRow">开始行</param>
        /// <param name="fileName">导出文件名称</param>
        private void ExportExcel(HttpContext context, List<Plan> tempList, string fileName,out string outfilename)
        {
            outfilename = "";
            if (tempList== null ||tempList.Count()<=1)
                return;
            // 生成Excel 
            string excelFileName = string.Format("{0}_{1}.xls", fileName, DateTime.Now.ToString("yyyyMMddHHmmss"));

            string path = context.Server.MapPath("~\\UploadFiles\\" + excelFileName);
            outfilename = @"/UploadFiles/"+excelFileName;
            HSSFWorkbook wb = new HSSFWorkbook();
            ISheet ws = wb.CreateSheet("sheet1");
            ws.CreateFreezePane(5, 1, 5, 1); //冻结行列
            ws.SetColumnWidth(0, 3000);
            ws.SetColumnWidth(1, 3000);
            ws.SetColumnWidth(2, 3000);
            ws.SetColumnWidth(3,4000);
            ws.SetColumnWidth(4, 3000);
            NPOI.SS.UserModel.ICellStyle _lockedcellstyle = wb.CreateCellStyle();
            _lockedcellstyle.IsLocked = true;//锁定

            NPOI.SS.UserModel.ICellStyle _unlockedcellstyle = wb.CreateCellStyle();
            _unlockedcellstyle.IsLocked = false;//解锁锁定
            string str = "";
            // 生成主体数据
            int index1 = 0;
            foreach (var item in tempList)
            {
                str = item.str;
                string[] strValue= str.Split(new char[] { ','});
                var row = ws.CreateRow(index1);
                for (int index2 = 0; index2 < strValue.Length; index2++)
                {
                    var cell = row.CreateCell(index2);
                    if (index1==0||index2 <= 4)
                    {
                        cell.CellStyle = _lockedcellstyle;
                    }
                    else
                    {
                        cell.CellStyle = _unlockedcellstyle;
                    }
                    WriteExcelValue(cell,strValue[index2]);
                }
                index1++;
            }
            string password = ConfigurationManager.AppSettings["excelPassword"] == "" ? "123456" : ConfigurationManager.AppSettings["excelPassword"];
            ws.ProtectSheet(password);
            using (var fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                wb.Write(fileStream);
                fileStream.Close();
            }
        }

        /// <summary>
        /// 把值写入指定的Excel格子里
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        private void WriteExcelValue(NPOI.SS.UserModel.ICell cell, object value)
        {
            if ((value != null) && (value != DBNull.Value))
            {
                Type[] typeStrings = new Type[] { typeof(string) };
                Type[] typeNumbers = new Type[] { typeof(int), typeof(short), typeof(long), typeof(byte), typeof(float), typeof(double), typeof(decimal) };
                Type[] typeDateTimes = new Type[] { typeof(DateTime) };
                Type[] typeBools = new Type[] { typeof(bool) };

                if (typeStrings.Contains(value.GetType()))
                {
                    cell.SetCellValue(value.ToString());
                }
                else if (typeNumbers.Contains(value.GetType()))
                {
                    cell.SetCellValue(Convert.ToDouble(value));
                }
                else if (typeDateTimes.Contains(value.GetType()))
                {
                    cell.SetCellValue((DateTime)value);
                }
                else if (typeBools.Contains(value.GetType()))
                {
                    cell.SetCellValue((bool)value);
                }
                else
                {
                    cell.SetCellValue(value.ToString());
                }
            }
        }

        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class FCSTExportResult
    {
        public List<GridClass> ForzenColumn;
        public List<GridClass> Column;
        public ArrayList ColumnResult;
        public string ErrMsg;
        public string TipMsg;
    }

    public class GridClass
    {
        public string field;
        public string title;
        public int width;
    }

    public class FilesStatus
    {
        /// <summary>
        /// 文件保存路径
        /// </summary>
        public string filePath { get; set; }
        /// <summary>
        /// 文件名字
        /// </summary>
        public string fileName { get; set; }
        /// <summary>
        /// 文件大小,以M为单位
        /// </summary>
        public double fileSize { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 文件下载地址
        /// </summary>
        public string fileDownloadPath
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileLength"></param>
        /// <param name="fullPath"></param>
        public FilesStatus(string fileName, int fileLength, string fullPath)
        {
            SetValues(fileName, fileLength, fullPath);
        }

        public FilesStatus(string message)
        {
            this.message = message;
        }

        private void SetValues(string fileName, long fileLength, string fullPath )
        {
            this.fileName = fileName;
            this.filePath = fullPath;
            this.fileSize = ConvertBytesToMegabytes(fileLength);
        }
        /// <summary>
        /// 转换为M
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }

    public class Plan
    {
        public int ID { get; set; }
        public string str { get; set; }
    }
}