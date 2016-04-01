<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FCSTInput.aspx.cs" Inherits="TCL.Resources.Web.ResourcesEdit.FCSTInput" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manpower Plan</title>
    <!--解决IE兼容模式带来的JS问题other options (old and new) include:IE=5, IE=7, IE=8, or IE=edge(edge equals highest mode available)-->
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link href="../Css/easyUI/easyui.css" rel="stylesheet" />
    <link href="../Css/easyUI/icon.css" rel="stylesheet" />
    <%--<link href="../Css/easyUI/demo.css" rel="stylesheet" />--%>
    <script type="text/javascript" src="../Scripts/DataTable/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="../Scripts/DataTable/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="../Scripts/DataTable/dataTables.fixedColumns.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../Scripts/DataTable/jquery.dataTables.min.css">
    <link rel="stylesheet" type="text/css" href="../Scripts/DataTable/fixedColumns.dataTables.min.css">

    <%--   <script src="../Scripts/jquery/jquery.min.js" type="text/javascript"></script>--%>
    <script src="../Scripts/jqueryPlugs/easyUI/jquery.easyui.min.js"></script>
    <script src="../Scripts/jquery/jquery.blockUI.js"></script>
    <script src="../Scripts/customJS/common.js" type="text/javascript"></script>
    <script src="../Scripts/customJS/inputdetail.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/chosen/chosen.jquery.js"></script>
    <link rel="stylesheet" type="text/css" href="../Scripts/chosen/chosen.css" />
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/customJS/peopleEdit.js"></script>
    <link type="text/css" rel="stylesheet" href="../Scripts/jqueryPlugs/My97DatePicker/skin/WdatePicker.css" />
    <script type="text/javascript" src="../Scripts/jqueryPlugs/My97DatePicker/WdatePicker.js"></script>
    <script src="../Scripts/customJS/TotalProjectTeam.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            var projecturl = "../DataAPI/Ashx/ProjectHandle.ashx";
            var exporturl = "../DataAPI/Ashx/FCSTExport.ashx";
            // 选择部门
            $(".imgSelectUsers").click(function () {
                var selecedUserType = $(this).attr("alt");
                var returnDataType = '';
                var selectUserCallbackFuc = "";
                var selectUserValueClientID = '';
                var selectUserTextClientID = '';
                // 选择部门
                selectUserCallbackFuc = "getDeptShortName";
                //帐号
                selectUserValueClientID = "selDept";
                //名称
                selectUserTextClientID = "hidDept";
                changeRequester = true;

                selectUser(selectUserValueClientID, selectUserTextClientID, "sap", selectUserCallbackFuc, "true", "true");
                isChoiceUser = true;
            });
            // 项目选择组件
            $("#selProject").empty();

            $.getJSON(projecturl, {
                type: "GetProjectList"
            }, function (data) {
                //对请求返回的JSON格式进行分解加载
                var projectOptionHtml = "<option value='0'>select</option>";
                $(data).each(function (i, item) {
                    projectOptionHtml += "<option value='" + item.EProjectID + "'>" + item.ProjectName + "</option>";
                });

                $("#selProject").attr("data-placeholder", "Choose a project...").attr("style", "width:200px;").append(projectOptionHtml).chosen().change(function () {

                });
            });
            //查询
            $("#Query").click(function () {
                QueryData();
            });

            $("#Import").click(function () {
                $("#frmImport").attr("src", "FCSTImport.aspx");
                $dialog = $("#div_cmmbDialog").dialog({
                    title: 'Manpower Plan',
                    width: 950,
                    height: 500,
                    iconCls: 'pag-search',
                    closed: true,
                    cache: false,
                    modal: true,
                    toolbar: '#toolbar',
                    onLoad: function () {

                    },
                    buttons: [{
                        text: '导 入',
                        iconCls: 'ope-save',
                        handler: function () {
                            frmImport.window.Import();
                        }
                    }, {
                        text: '取 消',
                        iconCls: 'ope-close',
                        handler: function () {
                            $dialog.dialog('close');
                        }
                    }]

                });
                $dialog.dialog('open');
            });

            //导出
            $("#Export").click(function () {

                var json = []; //取表头
                var item = {};
                var str = "";
                //var str = "Level1,Level2,Level3,Level4,Project,";
                //var yearMonth = GetArrayYearMonth();
                //for (var i = 0; i < yearMonth.length; i++) {
                //    str += yearMonth[i] + ",";
                //}
                var firstHead = $("#inputDetail #fcstDetail_wrapper .dataTables_scrollHeadInner table tr:first th");
                for (var j = 0; j < firstHead.length; j++) {
                    str += $(firstHead[j]).attr("field") + ",";
                }
                item.str = str.substring(0, str.length - 1);
                json.push(item);

                $("#fcstDetail tbody tr").each(function (i, tritem) {
                    item = {};
                    str = "";
                        $(tritem).find("td").each(function (j, tditem) {
                            if (j <= 4) {
                                str += $(tditem).text() + ",";
                            }
                            else {
                                str += $(tditem).find("input").val()+ ",";
                            }
                        })
                        item.str = str;
                        json.push(item);
                });
              
                //$(".datagrid-view2 table").eq(1).find("tr").each(function (i, tritem) {
                //    str = "";
                //    $(tritem).find("td").each(function (j, tditem) {
                //        str += $(tditem).find("input").val() + ",";
                //    })
                //    json[i + 1].str += str.substring(0, str.length - 1);
                //});

                $.ajax({
                    url: exporturl,
                    type: "Post",
                    dataType: "json",
                    data: {
                        type: "export",
                        ResourcePlanJson: JSON.stringify(json)
                    },
                    success: function (data) {
                        location.href = data;
                    },
                    error: function () {
                        alert("调用失败");
                    }
                })
            });
            //保存
        });

        function CloseDialog() {
            $dialog.dialog('close');
        }


        function getDeptShortName() {
            var deptid = $("#hidDept").val();
            $.ajax({
                url: "../DataAPI/Ashx/OUTreeViewHandler.ashx",
                type: "Post",
                data: {
                    "zorgid": deptid
                },
                success: function (data) {
                    $("#selDept").val(data.ZZ_ORG_SHORT);
                },
                error: function () {

                }
            })
        }
    </script>

</head>
<body>
    <div class="top">
        <h1><a href="#">Resource Plan </a></h1>
        <div class="topbar">
            <div class="topbar_messages">
                <%--<img src="../Image/FCSTInput/message.png" />
      <div class="messages_no">1</div>--%>
            </div>
            <div class="topbar_line">|</div>
            <div class="topbar_user">Welcome,<%=UserName %></div>
            <div class="topbar_line">|</div>
            <div class="path"><a href="../index.aspx">Index</a> &gt; Manpower Plan </div>
            <div class="topbar_line">|</div>
        </div>
    </div>
    <div class="clear"></div>
    <div class="main">
        <div class="title">Manpower Plan</div>
        <div class="fcst">
            <div class="fcst_topbar">
                <div class="fcst_title">Query</div>
                <div class="btn_common" id="Import">Import</div>
                <div class="btn_common" id="Export">EXport</div>
                <div class="btn_common" onclick="QueryData()">Query</div>
                <br />
                <div class="fcst_topbar_fl">
                    Dept : 
                    <input type="text" class="input_common" style="width: 180px" name="selDept" id="selDept" /><img id="imgAddressBook" src="/Image/dept.png" alt="requestorId" class="imgSelectUsers" style="width: 20px; height: 20px" /><input type="hidden" value="" id="hidDept" />
                </div>
                <div class="fcst_topbar_fl">
                    Project :
      <select class="chosen-select select_common" id="selProject" style="width: 50px"></select>
                </div>
                <div class="fcst_topbar_fl">Start Date:<input type="text" id="startdate" onchange="" class="input_common" style="width: 180px" onclick="WdatePicker({ skin: 'whyGreen', dateFmt: 'yyyy-MM', lang: 'en' })" /></div>
                <div class="fcst_topbar_fl">End Date:<input type="text" id="enddate" onchange="" class="input_common" style="width: 180px" onclick="WdatePicker({ skin: 'whyGreen', dateFmt: 'yyyy-MM', lang: 'en' })" /></div>
            </div>
        </div>
        <div class="fcst">
            <div class="fcst_topbar" id="topbarfirst" style="">
                <div class="fcst_title" id="titlefirst">Manpower Plan</div>
                <%-- <div class="fcst_topbar_img"><img id="editImageStatus" src="../Image/FCSTInput/icon_editor.png" /></div> 
         <div class="fcst_topbar_img"> <img id="queryImageStatus" src="../Image/FCSTInput/icon_lock.png" /></div>--%>
                <div class="fcst_topbar_fl">
                    Select Dept :<span class="setpro_title">
                        <select name="select" id="levelThree" class="select_common">
                        </select>
                    </span>
                </div>
                <div class="fcst_topbar_fl">
                    Is Out Sourcing :
        <input name="outhouse" type="radio" value="1" />
                    &nbsp;YES&nbsp;&nbsp;&nbsp;
        <input name="outhouse" type="radio" value="0" checked="checked" />
                    &nbsp;NO
                </div>
                <%-- <div class="btn_stretch"><img id="toggleDetail" onclick="changeImage()" src="../Image/FCSTInput/icon_stretch.png" /></div>--%>
                <div class="btn_common" style="width: 85px" onclick="Save()">Save</div>
                <div class="btn_common" style="width: 85px" onclick="createFListHtml()">Create Form</div>
            </div>
            <div class="clear"></div>
            <div id="boxBlockUI" style="display: none">
                <img src="/Image/AjaxLoading.gif" />
            </div>
            <div id="inputDetail">
            </div>
        </div>
        <div class="fcst" id="topbarsecond">
            <div class="fcst_topbar" style="height: 20px">
                <div class="fcst_title">
                    Manpower Plan Total By Project
                </div>
            </div>
            <div class="clear"></div>
            <div class="fcsftable" id="totalByProject">
            </div>
        </div>
        <div class="clear"></div>
        <div class="fcst" id="topbarthird">
            <div class="fcst_topbar" style="height: 20px">
                <div class="fcst_title">
                    Manpower Plan Total  By Team
                </div>
            </div>
            <div class="clear"></div>
            <div class="fcsftable" id="totalByL4Team">
            </div>
        </div>
    </div>
    <div style="display: none">
        <div id="div_cmmbDialog">
            <iframe id="frmImport" name="frmImport" style="width: 900px; height: 600px; border-width: 0px"></iframe>
        </div>
    </div>
</body>
</html>
