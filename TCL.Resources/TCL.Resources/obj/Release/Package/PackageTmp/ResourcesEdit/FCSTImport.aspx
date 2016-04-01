<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FCSTImport.aspx.cs" Inherits="TCL.Resources.Web.ResourcesEdit.FCSTImport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <%-- <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />--%>
    <title>Manpower Plan</title>
    <!--解决IE兼容模式带来的JS问题other options (old and new) include:IE=5, IE=7, IE=8, or IE=edge(edge equals highest mode available)-->
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <script src="../Scripts/jquery/jquery.min.js" type="text/javascript"></script>
   <%--  <script src="../Scripts/jquery/jquery-1.7.2.min.js" type="text/javascript"></script>--%>
    <script src="../Scripts/jqueryPlugs/easyUI/jquery.easyui.min.js"></script>
    <script src="../Scripts/customJS/common.js" type="text/javascript"></script>
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../Scripts/JqueryFileUpload/js/vendor/jquery.ui.widget.js"></script>
    <!-- The Iframe Transport is required for browsers without support for XHR file uploads -->
    <script type="text/javascript" src="../Scripts/JqueryFileUpload/js/jquery.iframe-transport.js"></script>
    <!-- The basic File Upload plugin -->
    <script type="text/javascript" src="../Scripts/JqueryFileUpload/js/jquery.fileupload.js"></script>
    <!-- The main application script -->
<%--    <script type="text/javascript" src="../Scripts/JqueryFileUpload/js/main.js"></script>--%>
    <!-- The XDomainRequest Transport is included for cross-domain file deletion for IE8+ -->
    <!--[if gte IE 8]>
    <script src="../Scripts/JqueryFileUpload/js/cors/jquery.xdr-transport.js"></script>
    <![endif]-->
    <style>
        .bar {
            height: 100%;
            background: url("img/ui-bg_highlight-soft_75_cccccc_1x100.png") repeat-x scroll 50% 50% #CCCCCC;
            border: 1px solid #AAAAAA;
            color: #222222;
            font-weight: bold;
        }

        #progress {
            border-bottom-right-radius: 4px;
            border-bottom-left-radius: 4px;
            border-top-right-radius: 4px;
            border-top-left-radius: 4px;
            background: url("images/ui-bg_flat_75_ffffff_40x100.png") repeat-x scroll 50% 50% #FFFFFF;
            border: 1px solid #AAAAAA;
            color: #222222;
            height: 10;
            overflow: hidden;
        }

        #el02 legend { /* Text and background colour, blue on light gray */
            color: red;
            /*background-color:#ddd;*/
        }
    </style>
    <script type="text/javascript">
        var fileMaxSize = "<%=FileMaxSize %>";
        var allowedFileExtensions = "<%=AllowedFileExtensions %>";

        $(function () {
            var importUrl = "../DataAPI/Ashx/FCSTExport.ashx";
           <%-- $('#fileupload').fileupload({
                url: importUrl,
                formData: {
                    type: "UploadFile",
                    "UploadFilePath": "<%=UploadFilePath%>",
                    "FileMaxSize": fileMaxSize,
                    "AttachType": allowedFileExtensions
                } , //如果需要额外添加参数可以在这里添加  
                dataType: 'json',
                add: function (e, data) {
                    //var goUpload = true;
                    //var uploadFile = data.files[0];
                    //var re = new RegExp("^.*\.(" + allowedFileExtensions + ")$", "gi");
                    //data.context = $("<div/>").html(uploadFile.name + ":");
                    //if (!re.test(uploadFile.name)) {
                    //    //data.context = $("<div/>").html(uploadFile.name + ":You must select an file(" + allowedFileExtensions + ") only.").appendTo("#spanErrorMessage");
                    //    data.context.html(data.context.html() + "You must select an file(" + allowedFileExtensions + ") only.");
                    //    goUpload = false;
                    //}
                    //// 非IE浏览器
                    //if (!$.browser.msie) {
                    //    if (uploadFile.size > parseInt(fileMaxSize, 10) * 1024 * 1024) { // mb
                    //        data.context.html(data.context.html() + 'Please upload a smaller file, max size is ' + fileMaxSize + ' MB');
                    //        goUpload = false;
                    //    }
                    //}
                },
                done: function (e, data) {
                    console.log(data.result);
                    $.each(data.result, function (index, file) {
                        console.log(file);
                    });
                },
                progressall: function (e, data) {
                    $("#progress").show();
                    var progress = parseInt(data.loaded / data.total * 100, 10);
                    $('#progress .bar').css(
                        'width',
                        progress + '%'
                    ).text(progress + '%');
                },
                stop: function (e, data) {
                },
                change: function (e, data) {
                  
                }
            });--%>

        $("#fileupload").fileupload({
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
            });

            var BuildHtml = function (data, column, forzenColumn) {
                var obj = eval(data);
                var columnlength = column.length;
                var title = ""; //列名
                var FBudget_Resource;
                var isValid= 1; //是否有效值
                var tableDetail = "<table width='100%' border='0' id='fcstDetail' cellspacing='0' cellpadding='0' class='table_all'>";
                for (var i = 0; i < obj.length; i++) {
                    //var tablerow = "<tr class='thone'><td>" + obj[i].Level1 + "</td>" + "<td>" + obj[i].Level2 + "</td>" + "<td>" + obj[i].Level3 + "</td>" + "<td>" + obj[i].Level4 + "</td>" + "<td>" + obj[i].Project + "</td>";
                    var tablerow = "<tr class='thone'>";
                    for (var j = 0; j < forzenColumn.length; j++) {
                        title = forzenColumn[j].title;
                        isValid = obj[i]["msg"].length>0?0:1;
                        var input = "<lable isValid='" + isValid + "' title='" + obj[i]["msg"] + "' >" + obj[i]["" + title + ""] + "</lable>";
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
                        var input = "<lable isValid='" + isValid + "' orgid='" + obj[i].zorgid + "'  title='" + obj[i].zorgid + "' ProjectID='" + obj[i].projectid + "' year='" + title.split('-')[0] + "' month='" + title.split('-')[1] + "' isouthouse='" + obj[i].isouthouse + "'>" + FBudget_Resource + "</lable>";//太长了
                        tablerow += "<td>" + input + "</td>";
                    }
                    tablerow += "</tr>";
                    tableDetail += tablerow;
                }
                tableDetail += "</table>";
                $("#inputDetail").html(tableDetail);
            }

            //导入
            $("#divImport").click(function () {
                Import();
            })
        });

        function Import() {
            var json = [];
            $(".datagrid-body lable").each(function (a, b) {
                var item = {};
                item.FIS_outsource = $(b).attr("isouthouse");
                item.FOrg_Level4 = $(b).attr("orgid");
                item.FBudget_Year = $(b).attr("year");
                item.FBudget_Month = $(b).attr("month");
                item.FProject_ID = $(b).attr("ProjectID");
                item.FBudget_Resource = $(b).text();
                json.push(item);

            });
            if (json.length == 0) {
                alert("请先上传数据！！！");
                return;
            }
            if ($(".datagrid-body lable[isValid='0']").length > 0) {
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
            <div class="title">FCST Import Page</div>
            <div class="fcst">
                <div class="fcst_topbar">
                    <div class="fcst_title">FCST Import</div>
                    <div class="fcst_topbar_fl">
                        SELECT FILE :<span class="setpro_title">
                            <input id="fileupload" type="file" multiple>
                        </span>
                    </div>
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
