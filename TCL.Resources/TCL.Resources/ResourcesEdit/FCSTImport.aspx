<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FCSTImport.aspx.cs" Inherits="TCL.Resources.Web.ResourcesEdit.FCSTImport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <%-- <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />--%>
    <title>Manpower Plan</title>
    <!--解决IE兼容模式带来的JS问题other options (old and new) include:IE=5, IE=7, IE=8, or IE=edge(edge equals highest mode available)-->
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link href="../Css/easyUI/easyui.css" rel="stylesheet" />
    <%--<script src="../Scripts/jquery/jquery.min.js" type="text/javascript"></script>--%>
     <script type="text/javascript" src="../Scripts/DataTable/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="../Scripts/DataTable/jquery.dataTables.min.js"></script>
       <script type="text/javascript" src="../Scripts/DataTable/dataTables.fixedColumns.min.js"></script>
        <link rel="stylesheet" type="text/css" href="../Scripts/DataTable/jquery.dataTables.min.css">
    <link rel="stylesheet" type="text/css" href="../Scripts/DataTable/fixedColumns.dataTables.min.css">
    <script src="../Scripts/jquery/ajaxfileupload.js"></script>
   <%--  <script src="../Scripts/jquery/jquery-1.7.2.min.js" type="text/javascript"></script>--%>
     <script src="../Scripts/jqueryPlugs/easyUI/jquery.easyui.min.js"></script>
    <script src="../Scripts/jquery/jquery.blockUI.js"></script>
    <script src="../Scripts/customJS/common.js" type="text/javascript"></script>
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />

   <%-- <script type="text/javascript" src="../Scripts/JqueryFileUpload/js/vendor/jquery.ui.widget.js"></script>
    <!-- The Iframe Transport is required for browsers without support for XHR file uploads -->
    <script type="text/javascript" src="../Scripts/JqueryFileUpload/js/jquery.iframe-transport.js"></script>
    <!-- The basic File Upload plugin -->
    <script type="text/javascript" src="../Scripts/JqueryFileUpload/js/jquery.fileupload.js"></script>--%>
    <!-- The main application script -->
<%--    <script type="text/javascript" src="../Scripts/JqueryFileUpload/js/main.js"></script>--%>
   
  
    <script type="text/javascript">
        var fileMaxSize = "<%=FileMaxSize %>";
        var allowedFileExtensions = "<%=AllowedFileExtensions %>";
        var importUrl = "../DataAPI/Ashx/FCSTExport.ashx";
        $(function () {
       <%-- $("#fileupload").fileupload({
                url: importUrl,//文件上传地址，当然也可以直接写在input的data-url属性内
                formData: {
                    type: "UploadFile",
                    "UploadFilePath": "<%=UploadFilePath%>",
                    "FileMaxSize": fileMaxSize,
                    "AttachType": allowedFileExtensions
                },
                dataType: 'json',
                done: function (e, data) {
                    $("#divTips").html("");
                    if (data.result != null)
                    {
                        var json = JSON.parse(data.result);
                        //var json = eval(data.result);
                        if (json.ErrMsg != null && json.ErrMsg != "") {
                            alert(json.ErrMsg);
                            return;
                        }
                        if (json.TipMsg != null && json.TipMsg != "") {
                            $("#divTips").html(json.TipMsg);
                        }  
                        BuildHtml(json.ColumnResult, json.Column, json.ForzenColumn);

                        $('#inputDetail').datagrid({
                            title: 'Frozen Columns',
                            width: '98%',
                            height: 400,
                            frozenColumns: [json.ForzenColumn],
                            columns: [
                                json.Column
                            ], rownumbers: true
                        });

                        $(".datagrid-body lable[isValid='0']").each(function (a, b) {
                            $(this).attr("style","color:red");
                        });
                    }
                }
            });--%>

            var BuildHtml = function (data, column, forzenColumn) {
                var obj = eval(data);;
                var tableDetail = "<table width='100%' border='0' id='fcstDetail' cellspacing='0' cellpadding='0' class='table_all'>";
                tableDetail += "<thead>";
                tableDetail += "<tr>";
                tableDetail += "<th field='Level1'>Level1</th>";
                tableDetail += "<th field='Level2'>Level2</th>";
                tableDetail += "<th field='Level3'>Level3</th>";
                tableDetail += "<th field='Level4'>Level4</th>";
                tableDetail += "<th field='Project'>Project</th>";
                for (var j = 0; j < column.length; j++) {
                    tableDetail += "<th field='" + column[j].field + "'>" + column[j].title + "</th>";
                }
                tableDetail += "</tr>";
                tableDetail += "</thead>";
                var columnlength = column.length;
                var title = ""; //列名
                var FBudget_Resource;
                var isValid= 1; //是否有效值
                for (var i = 0; i < obj.length; i++) {
                    //var tablerow = "<tr class='thone'><td>" + obj[i].Level1 + "</td>" + "<td>" + obj[i].Level2 + "</td>" + "<td>" + obj[i].Level3 + "</td>" + "<td>" + obj[i].Level4 + "</td>" + "<td>" + obj[i].Project + "</td>";
                    var tablerow = "<tr class='thone'>";
                    for (var j = 0; j < forzenColumn.length; j++) {
                        title = forzenColumn[j].title;
                        isValid = obj[i]["msg"].length>0?0:1;
                        var input = "<span isValid='" + isValid + "' title='" + obj[i]["msg"] + "' >" + obj[i]["" + title + ""] + "</span>";
                        tablerow += "<td>" + input + "</td>";
                    }
                    var is_outsource = false;
                    for (var j = 0; j < columnlength; j++) {
                        title = column[j].title;
                        isValid = 1;
                        FBudget_Resource = obj[i]["" + title + ""];
                        if (isNaN(FBudget_Resource) || obj[i]["msg"].length > 0) {
                            isValid = 0;
                        }
                        var input = "<span isValid='" + isValid + "' orgid='" + obj[i].zorgid + "'   ProjectID='" + obj[i].projectid + "' year='" + title.split('-')[0] + "' month='" + title.split('-')[1] + "' isouthouse='" + obj[i].isouthouse + "'>" + FBudget_Resource + "</span>";//太长了
                        tablerow += "<td>" + input + "</td>";
                    }
                    tablerow += "</tr>";
                    tableDetail += tablerow;
                }
                tableDetail += "</table>";
                $("#inputDetail").html(tableDetail);
            }

            $("#divUpload").click(function () {
                $.blockUI({ message: $("#divBoxBlockUI").html() });
                $.ajaxFileUpload({
                    url: importUrl, //用于文件上传的服务器端请求地址
                    secureuri: false, //一般设置为false
                    fileElementId: 'fileupload', //文件上传空间的id属性  <input type="file" id="file" name="file" />
                    dataType: 'json', //返回值类型 一般设置为json
                    data: {
                        type: "UploadFile",
                        "UploadFilePath": "<%=UploadFilePath%>",
                        "FileMaxSize": fileMaxSize,
                        "AttachType": allowedFileExtensions
                    },
                    success: function (data, status)  //服务器成功响应处理函数
                    {
                        //console.log(data);
                      
                        $("#divTips").html("");
                        if (data != null)
                        {
                            //var json = JSON.parse(data.result);
                            var json = eval(data);
                            if (json.ErrMsg != null && json.ErrMsg != "") {
                                alert(json.ErrMsg);
                                return;
                            }
                            if (json.TipMsg != null && json.TipMsg != "") {
                                $("#divTips").html(json.TipMsg);
                            }  
                            BuildHtml(json.ColumnResult, json.Column, json.ForzenColumn);

                            var table = $('#fcstDetail').DataTable({
                                bFilter: false,
                                scrollY: "300px",
                                scrollX: true,
                                scrollCollapse: true,
                                paging: false,
                                ordering: false,
                                info: false,
                                fixedColumns: {
                                    leftColumns: 5
                                }
                            });
                            //$('#inputDetail').datagrid({
                            //    title: 'Frozen Columns',
                            //    width: '98%',
                            //    height: 400,
                            //    frozenColumns: [json.ForzenColumn],
                            //    columns: [
                            //        json.Column
                            //    ], rownumbers: true
                            //});

                            $(".datagrid-cell span[isValid=0]").each(function (a, b) {
                                $(this).attr("style","color:red");
                            });
                            $.unblockUI();
                        }
                    },
                    error: function (data, status, e)//服务器响应失败处理函数
                    {
                      
                    }
                }
                )
            });

            //导入
            $("#divImport").click(function () {
                Import();
            })
        });

        function Import() {
           
            var json = [];
            $(".datagrid-body span").each(function (a, b) {
                var item = {};
                item.FIS_outsource = $(b).attr("isouthouse");
                item.FOrg_Level4 = $(b).attr("orgid");
                item.FBudget_Year = $(b).attr("year");
                item.FBudget_Month = $(b).attr("month");
                item.FProject_ID = $(b).attr("ProjectID");
                item.FBudget_Resource = $(b).text();
                if ($(b).attr("ProjectID") != null && $(b).attr("ProjectID")!="")
                   json.push(item);

            });
            if (json.length == 0) {
                alert("请先上传数据！！！");
                return;
            }
            if ($(".datagrid-cell span[isValid=0]").length > 0) {
                alert("数据有异常，无法导入！！！");
                return;
            }
            if (window.confirm('确定要导入数据吗？')) {
                $.ajax({
                    url: importUrl,
                    type: "Post",
                    dataType: "text",
                    data: {
                        "ResourcePlanJson": JSON.stringify(json),
                        type: "import"
                    },
                    success: function (data) {
                        alert("Save Success");
                        parent.CloseDialog();
                    },
                    error: function () {
                        alert("Save Failure");
                    }
                })
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <%--   <div class="top">
            <h1><a href="#">Resource Plan </a></h1>
            <div class="topbar">
                <div class="topbar_messages">
                </div>
                <div class="topbar_line">|</div>
                <div class="topbar_user">Welcome,Koala.Deng</div>
                <div class="topbar_line">|</div>
                <div class="path"><a href="../index.aspx">Index</a> &gt; manpower plan </div>
                <div class="topbar_line">|</div>
            </div>
        </div>
        <div class="clear"></div>--%>
        <div class="main">
            <div class="title">Manpower Plan</div>
            <div class="fcst">
                <div class="fcst_topbar">
                    <div class="fcst_title">Manpower Plan</div>
                    <div class="fcst_topbar_fl">
                        SELECT FILE :<span class="setpro_title">
                            <input id="fileupload" type="file" name="fileupload">
                        </span>
                    </div>
                    <div class="btn_common"  id="divUpload">Upload</div>
                    <div class="btn_common"  id="divImport" style="display:none">Import</div>

                </div>
                <div class="clear"></div>
                <div id="divTips" style="color:red">

                </div>
                <div class="fcsftable" id="inputDetail">
                </div>
            </div>
        </div>

    </form>
</body>
</html>
