<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectUsers.aspx.cs" Inherits="TCL.EP.BPM.ASPX.PeopleEdit.SelectUsers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery/jquery.min.js" type="text/javascript"></script>
    <script src="/Scripts/jqueryPlugs/jquery-easyui-1.2.5/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/jqueryPlugs/jquery-easyui-1.2.5/jquery.easyui.min.js" type="text/javascript"></script>
    <link href="/Scripts/jqueryPlugs/jquery-easyui-1.2.5/themes/gray/easyui.css" rel="stylesheet"
        type="text/css" />
    <link href="/Scripts/jqueryPlugs/jquery-easyui-1.2.5/themes/gray/tree.css" rel="stylesheet"
        type="text/css" />
    <link href="/Scripts/jqueryPlugs/jquery-easyui-1.2.5/themes/gray/layout.css" rel="stylesheet"
        type="text/css" />
    <script src="/Scripts/customJS/peopleEdit.js" type="text/javascript"></script>
</head>
<style type="text/css">
    #tb_userinfo td
    {
        padding-left: 5px;
        cursor: pointer;
    }
    .title
    {
        font-weight: bold;
    }
    .selectedTR
    {
        background-color: #0092dc;
    }
    .overClass
    {
        background-color: #f3f3f3;
    }
    .divSrollbar
    {
        scrollbar-3dlight-color:#f7f7f7;
        scrollbar-darkshadow-color:#fafafa;
        scrollbar-face-color:#f7f7f7;
        scrollbar-highlight-color:#cecfce;
        scrollbar-shadow-color:#949494;
        scrollbar-track-color:#fffbff;
    }
    .tree-folder{width:0;}
    .tree-node-selected{background-color:#ccc;}
</style>
<script type="text/javascript">
    $(function () {
        $('#div_peopleEdit').tabs({
            border: true,
            tabPosition: "top"
        });
        $('#OUTreeViewContainer').tree({
            lines: true,
            animate: true,
            url: '/DataAPI/Ashx/OUTreeViewHandler.ashx',
            onLoadSuccess: function (node, data) {

            },
            onDblClick: function (node) {
                var returnKey = RequestQueryString("key");

                //added by robbie 2014/1/20
                var orgflag = RequestQueryString("IsSelectOrg");
                if (orgflag == "" || orgflag != "1") {
                    //alert("不能选择组织，请选择用户！");
                    return;
                }

                if (returnKey.toLowerCase() == "sap") {
                    chooseUser(node.text, node.id);
                } else {
                    chooseUser(node.text, node.attributes.ADAccount);
                }
            },
            onClick: function (node) {
                getUsersOfGroup(node);
            }
        });
    });
    var runCallbackFunction = function () { alert(1); };
</script>
<body>
    <form id="form1" runat="server">
    <div id="div_peopleEdit" class="easyui-tabs" style="width: 690px; height: 450px; text-align: center;">
        <div runat="server" title="组织结构" id="peopleEidtOU" style=" padding:5px;">
            <div id="mainContent" style="width: 100%;" runat="server">
            </div>
        </div>
        <div runat="server" title="信息查询" id="peopleEditSearch" class="divSrollbar" style=" padding:5px;">
                <table cellpadding="0" cellspacing="0" style="width:100%; border-collapse:collapse;">
                    <tr>
                        <td style="border: 1px solid #d3d3d3;BACKGROUND-COLOR: #f3f3f3; height:30px; padding-left:10px; font-weight:bold;">
                            条件：<input type="text" id="txtQueryKey" style="border: 1px solid #d3d3d3; width:480px; padding-right:10px; height:20px;" />
                            <a href="#" class="easyui-linkbutton" iconcls="icon-search" onclick="queryFunction();return false;">查询</a>
                        </td>
                    </tr>
                    <tr>
                        <td  id="td_query_result" valign="top" style="background-color:#fafafa;border: 1px solid #d3d3d3;width: 675px; height: 370px;">
                        </td>
                    </tr>
                  </table>
        </div>
    </div>
    <div style="width: 100%;">
        <table cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td style="width: 60px;">
                    <input type="button" value="成员>>" style="padding-top: 6px; padding-bottom: 10px;
                        width: 60px;" disabled="disabled" />
                </td>
                <td style="text-align: left; height: 40px; width: 1000px;">
                    <div id="txtSelectedUsers" style="width: 100%; height: 35px; overflow: auto; float: right;
                        border-width: 1px; border-collapse: collapse; border-style: solid; border-color: #dbddde;">
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: right; padding-right: 50px; height: 40px;">
                    <input type="button" value="确定" style="width: 150px;" onclick="PostUserInfo()" />
                    <input type="button" value="清除" style="width: 150px;" onclick="RemoveUsers()" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
