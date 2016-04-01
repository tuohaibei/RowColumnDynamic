var projecturl = "../DataAPI/Ashx/ProjectHandle.ashx";
var levelurl = "../DataAPI/Ashx/LevelHandle.ashx";
$(function () {
    //要请求的一级机构JSON获取页面
    $("#branchone").append($("<option/>").text("--Please Select--").attr("value", "-1"));
    initOrgLevel1(levelurl);//初始化level1;
    //一级下拉联动二级下拉
    $("#branchone").change(function () {
        if ( $("#levelThree").val()=="-1") {
            alert("Please select L3 Dept. first,Otherwise your selected will losed!")
            return;
        }
        initOrgLevel2($("#branchone").val());
    });
    //二级节点下的工程设置
    $("#branchtwo").change(function () {
        InitProjectByOrgLevel2();
    })
    //设置点击之后背景色变化并还原其它的子节点
    $("#leftproject").on("click","li" ,function () {
        $(this).css("background", "#cccccc").siblings().css("background", "#F0F3F7");
        selectedLeft = $(this).attr("id");
    })
    $("#rightproject").on("click", "li", function () {
        $(this).css("background", "#cccccc").siblings().css("background", "#F0F3F7");
        selectedRight = $(this).attr("id");
    })
    //获取第三节点的部门信息
    $("#levelThree").empty();
    $("#levelThree").append($("<option/>").text("--Please Select--").attr("value", "-1"));
    $.getJSON(levelurl,
        {
            level: 'levelThree'
        },
        function (data) {
            //对请求返回的JSON格式进行分解加载
            $(data).each(function () {
                $("#levelThree").append($("<option/>").text(this.orgName).attr("value", this.orgID));
            });
        });
    //三级部门切换时对应的查找已设定的项目
    $("#levelThree").change(function () {
        //$("#path").text($("#branchone").val() + '=>' + $(this).val());
        $("#rightproject").empty();
        $("#leftproject").empty();
        $.getJSON(projecturl, {
            levelThreeID: $(this).val(),
            rad:Math.random()
        }, function (data) {
            //对请求返回的JSON格式进行分解加载
            $(data).each(function () {
                $("#rightproject").append($("<li/>").text(this.ProjectName).attr("id", this.EProjectID));
            });
            //首先判断data的长度
            if (data.length > 0) {
                $.ajaxSettings.async = false;
                $("#branchone").val(data[0].OrgLevel1Name);//设置选中项.
                initOrgLevel2(data[0].OrgLevel1Name);//初始化level2.
                $("#branchtwo").val(data[0].OrgLevel2Name);//设置选中项.
                InitProjectByOrgLevel2();//初始化项目列表.
                $(data).each(function () {
                    $("#leftproject li").remove("li[id ='"+this.EProjectID+"']");
                });
                $.ajaxSettings.async = true;

            }
            else {
                $("#branchone").val("-1");
                $("#branchtwo").empty();
            }
            //否则就让level1和level2置为请选择.
        });
    })
});
//保存数据
var SaveData = function () {
    var projectArrary = $("#rightproject").children().toArray();
    if (projectArrary.length == 0) { alert("Please Select Project!"); return; }
    if ($("#levelThree").val() == "-1") {
        alert("Please Select Dept.!");
        return;
    }
    var jsonT = "{'orgID':'"+$("#levelThree").val()+"','projectItems':[";
    for (var i = 0; i < projectArrary.length; i++) {
        if (i == 0) {
            jsonT += "{'projectID':'" + $(projectArrary[i]).attr("id") +  "','Level1Name':'"+ $("#branchone").val()+"','Level2Name':'"+ $("#branchtwo").val()+"'}"
        } else {
            jsonT += ",{'projectID':'" + $(projectArrary[i]).attr("id") + "','Level1Name':'" + $("#branchone").val() + "','Level2Name':'" + $("#branchtwo").val() + "'}"
        }
    }
    jsonT += "]}";
    $.ajax({
        url:"../DataAPI/Ashx/SaveDataHandle.ashx",
        type:"Post",
        dataType:"text",
        data: {"orgProject":jsonT},
        success:function(data) {
            alert("Save Success");
        },
        error:function()
        {
            alert("Save Failure");
        }
    })
}
var initOrgLevel1 = function (levelurl) {
    $.getJSON(levelurl, function (data) {
        //对请求返回的JSON格式进行分解加载
        $(data).each(function () {
            $("#branchone").append($("<option/>").text(this.OrgLevel1Name)).attr("value", this.OrgLevel1Name);
        });
    });
}
var initOrgLevel2 = function (OrgLevel1Name) {
    $("#branchtwo").empty();
    $("#branchtwo").append($("<option/>").text("--Please Select--").attr("value", "-1"));
    //$("#path").text($(this).attr("value"));
    // 将选中的一级下拉列表项的name传过去
    $.getJSON(levelurl, {
        levelName: OrgLevel1Name,
        level: "levelone"
    }, function (data) {
        //对请求返回的JSON格式进行分解加载
        $(data).each(function () {
            $("#branchtwo").append($("<option/>").text(this.OrgLevel2Name).attr("value", this.OrgLevel2Name));
        });
    });
    //一级节点下的工程设置
    initLeftProject(OrgLevel1Name);
}
var initLeftProject=function(OrgLevel1Name)
{
    $("#leftproject").empty();
    $.getJSON(projecturl, {
        levelonename: OrgLevel1Name
    }, function (data) {
        //对请求返回的JSON格式进行分解加载
        $(data).each(function () {
            $("#leftproject").append($("<li/>").text(this.ProjectName).attr("id", this.EProjectID));
        }
        );
        DeleteProjectExist();
    });
}
var DeleteProjectExist=function(){
    var liArray = $("#rightproject li").toArray();
    $(liArray).each(function (i,item) {
        $("#leftproject li").remove("li[id ='" + $(item).attr("id") + "']");
    }
   );
}
var InitProjectByOrgLevel2 = function () {
    $("#leftproject").empty();
    //选择了具体的项目切换才有意义
    if ($("#branchtwo").val() != "-1") {
        $.getJSON(projecturl, {
            levelonename: $("#branchone").val(),
            leveltwoname: $("#branchtwo").val()
        }, function (data) {
            //对请求返回的JSON格式进行分解加载
            $(data).each(function (i, item) {
                $("#leftproject").append($("<li/>").text(item.ProjectName).attr("id", item.EProjectID));
            });
            DeleteProjectExist();
        });
    }
}
