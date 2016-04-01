<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="TCL.Resources.Web.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Resource Plan</title>
    <link href="Css/style.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="Scripts/customJS/common.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $.ajax(
                {
                    type: "Post",
                    //方法所在页面和方法名
                    url: "DataAPI/Ashx/UserHandle.ashx",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        //返回的数据用data.d获取内容
                        data = JSON.parse(data);
                        $("#banner").text("Welcome," + data.username);
                        $(".menu_right_title").text("Welcome," + data.username);
                    },
                    error: function (err) {
                        alert(err);
                    }

                });
        })
        function Note()
        {
            $("#prompt .introduction").text("正在建设中,敬请期待！");
        }
    </script>
</head>
<body style="background:url(Image/menu_bg.png) #F0F3F7 repeat-y left">
    <div class="top">
        <h1><a href="index.html">Resource Plan</a></h1>
        <div class="topbar">
            <div class="topbar_messages">
              <%--  <img src="Image/message.png" />
                <div class="messages_no">1</div>--%>
            </div>
            <div class="topbar_line">|</div>
            <div class="topbar_user" id="banner"></div>
            <div class="topbar_line">|</div>
        </div>
    </div>
    <div class="clear"></div>
    <div class="main">
        <div class="menu_left">
            <ul>
                <li>
                    <div class="menu_icon"><img src="Image/icon_05.png" /></div>
                    <div class="menu_infor" style="cursor:pointer" id="dept_project_set"><a onclick="GetAuthority()">Project Setting</a></div>
                </li>
                 <li>
                    <div class="menu_icon"><img src="Image/icon_03.png" /></div>
                    <div class="menu_infor" style="cursor:pointer"><a href="ResourcesEdit/FCSTInput.aspx" target="_blank">Manpower Plan</a></div>
                </li>
                <li>
                    <div class="menu_icon"><img src="Image/icon_01.png" /></div>
                    <div class="menu_infor" style="cursor:pointer"><a href="#" onclick="Note()">HC Import</a></div>
                </li>
                <li>
                    <div class="menu_icon"><img src="Image/icon_02.png" /></div>
                    <div class="menu_infor" style="cursor:pointer"><a href="#" onclick="Note()">Project Import</a></div>
                </li>
                <li>
                    <div class="menu_icon"><img src="Image/icon_04.png" /></div>
                    <div class="menu_infor" style="cursor:pointer"><a href="#" onclick="Note()">FCST Query</a></div>
                </li>
                <li>
                    <div class="menu_icon"><img src="Image/icon_06.png" /></div>
                    <div class="menu_infor" style="cursor:pointer"><a onclick="Note()"> Editor Set</a> </div>
                </li>
                <li>
                    <div class="menu_icon"><img src="Image/icon_07.png" /></div>
                    <div class="menu_infor" style="cursor:pointer"><a onclick="Note()">Outhouse Mapping</a></div>
                </li>
            </ul>
        </div>
        <div class="menu_right" id="prompt">
            <div class="menu_right_title"></div><div class="title_next">Resource Plan System Introduction</div><div class="introduction">
                <div class="introduction_title">Introduction</div>
                The Human Resources Management (HRM) function includes a variety of activities, and key among them is deciding what staffing needs you have and whether to use independent contractors or hire employees to fill these needs, recruiting and training the best employees, ensuring they are high performers, dealing with performance issues, and ensuring your personnel and management practices conform to various regulations.
Activities also include managing your approach to employee benefits and compensation, employee records and personnel policies. Usually small businesses (for-profit or nonprofit) have to carry out these activities themselves because they can't yet afford part- or full-time help. However, they should always ensure that employees have -- and are aware of -- personnel policies which conform to current regulations. These policies are often in the form of employee manuals, which all employees have.
                There is a long-standing argument about where HR-related functions should be organized into large organizations, e.g.. "should HR be in the Organization Development department or the other way around?"
The HRM function and HRD profession have undergone tremendous change over the past 20-30 years. Many years ago, large organizations looked to the "Personnel Department," mostly to manage the paperwork around hiring and paying people. More recently, organizations consider the "HR Department" as playing a major role in staffing, training and helping to manage people so that people and the organization are performing at maximum capability in a highly fulfilling manner. 
            </div>
        </div>
    </div>
</body>
</html>
