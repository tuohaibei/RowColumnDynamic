<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptProjectSetPage.aspx.cs" Inherits="TCL.Resources.Web.ResourcesEdit.DeptProjectSetPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Project Setting</title>
    <script src="../Scripts/jquery/jquery.min.js" type="text/javascript"></script>
    <script src="../Scripts/customJS/levelproject.js" type="text/javascript"></script>
     <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var selectedLeft;
        var selectedRight;
        //左移右移逻辑
        function left(isAll) {
            var rightArray = new Array();
            rightArray = $("#rightproject").find("li");
            for (i = 0; i < rightArray.length; i++) {
                if (isAll) {
                    //var o = new oOption(os[i].text, os[i].value);
                    var li = "<li id=" + rightArray[i].id + ">" + $(rightArray[i]).text() + "</li>";
                    $("#leftproject").append(li);
                    $("#rightproject").find("li").remove();
                    // == $("#right_select").empty();
                } else {
                    if ($(rightArray[i]).attr("id") == selectedRight) {
                        //var o = new Option(os[i].text, os[i].value);
                        var li = "<li id=" + rightArray[i].id + ">" + $(rightArray[i]).text() + "</li>";
                        $("#leftproject").append(li);
                        $("#rightproject li[id=" + selectedRight + "]").remove();
                    }
                }
            }
        }
        function right(isAll) {
            var leftArray = new Array();
            leftArray = $("#leftproject").find("li");
            for (i = 0; i < leftArray.length; i++) {
                if (isAll) {
                    //var o = new Option(os[i].text, os[i].value);
                    var li = "<li id=" + leftArray[i].id + ">" + $(leftArray[i]).text() + "</li>";
                    $("#rightproject").append(li);
                    $("#leftproject").find("li").remove();
                    // == $("#left_select").empty();
                } else {
                    if ($(leftArray[i]).attr("id") == selectedLeft) {
                        var li = "<li id=" + leftArray[i].id + ">" + $(leftArray[i]).text() + "</li>";
                        $("#rightproject").append(li);
                        $("#leftproject  li[id=" + selectedLeft + "]").remove();
                    }
                }
            }
        }
    </script>
</head>
<body>
    <div class="top">
        <h1><a href="#">Resource Plan </a></h1>
        <div class="topbar">
            <div class="topbar_messages">
               <%-- <img src="../Image/message.png" />
                <div class="messages_no">1</div>--%>
            </div>
            <div class="topbar_line">|</div>
            <div class="topbar_user">Welcome,<%=UserName %></div>
            <div class="topbar_line">|</div>
            <div id="path" class="path"><a href="../index.aspx">Index</a>> Project Setting </div>
            <div class="topbar_line">|</div>
        </div>
    </div>
    <div class="clear"></div>
    <div class="main">
        <div class="title">Project Setting</div>
        <div class="set">
            <div class="set_inner">
                <div class="set_mian">
                    <div class="set_mian_pro">
                        <div class="setpro_title">Project Pool:</div>
                        <div class="setpro_title">
                            L1 Dept.
            <select class="select_common" id="branchone">
            </select>L2 Dept.
            <select class="select_common" id="branchtwo">
            </select>
                        </div>
                        <div class="choosedep">
                            <ul id="leftproject">
                            </ul>
                        </div>
                    </div>
                    <div class="setbtn_move">
                        <div class="btn_setmove" onclick="right(true)">&gt;&gt;</div>
                        <div class="btn_setmove" onclick="right()">&gt;</div>
                        <div class="btn_setmove" onclick="left()">&lt;</div>
                        <div class="btn_setmove" onclick="left(true)">&lt;&lt;</div>
                    </div>
                    <div class="set_mian_pro">
                        <div class="setpro_title">My Project:</div>
                        <div class="setpro_title">
                            L3 Dept.
                            <%--<div class="select_common">
                                <input type="hidden" id="OrgValue"/>
                                <input type="text" id="OrgText" readonly="true" style="width: 110px;" />
                                <input type="button" value="组织" onclick="window.open('../SelectOrg/SelectUsers.aspx?txtID=OrgValue&valueID=OrgText', null, 'width=700,toolbar=no,menubar=no,scrollbars=no,resizable=yes,location=no,status=no')" style=" height:24px; width: 34px;" />--%>
                            <select class="select_common" id="levelThree" style="width:180px">
                                <%-- <option>WMD</option>
                                <option selected="selected">Global Cost Control</option>--%>
                            </select>
                        </div>
                        <div class="choosedep_l">
                            <ul id="rightproject">
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="setbtn_bar">
                <div class="btn_common" id="save" onclick="SaveData()">Save</div>
                <div class="btn_common" style="visibility: hidden">Cancel</div>
            </div>
            </div>
        </div>
    </div>
</body>
</html>
