$(function () {
    //切换显示和隐藏
    $("#toggleDetail").click(function () {
        $("#inputDetail").slideToggle();
    });
    var levelurl = "../DataAPI/Ashx/LevelHandle.ashx";
    $("#levelThree").empty();
    $.getJSON(levelurl,
        {
            level: 'LevelThreeQuery'
        },
        function (data) {
            //对请求返回的JSON格式进行分解加载
            $(data).each(function () {
                $("#levelThree").append($("<option/>").text(this.orgName).attr("value", this.orgID));
            });
        });
    //判断该用户的权限,然后对div createform和save做隐藏
    $.ajax({
        url: "../DataAPI/Ashx/AuthorityHandle.ashx",
        dataType: "text",
        success: function (data) {
            if (data.toLowerCase() == 'false') {
                $("#topbarfirst").hide();
                $("#Import").hide();

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown.error);
        }
    })
    //页面初始化时标题隐藏
    $("#topbarsecond").hide();
    $("#topbarthird").hide();
})
var visibile = 0;
var changeImage = function () {
    if (visibile == 0) {
        $("#toggleDetail").attr("src", "../Image/FCSTInput/icon_shrink.png");
        visibile = 1;
    }
    else {
        $("#toggleDetail").attr("src", "../Image/FCSTInput/icon_stretch.png");
        visibile = 0;
    }

}
//如果是查询状态
var chageImageStaus = function (isQuery) {
    if (isQuery) {
        $("#editImageStatus").addClass("status_image_backgroud")
        $("#queryImageStatus").removeClass("status_image_backgroud");
        ;
    }
    else {
        $("#queryImageStatus").addClass("status_image_backgroud");
        $("#editImageStatus").removeClass("status_image_backgroud")
    }
}
//构造年月数组--为了生成表头
var GetArrayYearMonth = function () {
    var dateTime;
    var year;
    var month;
    var arrayYearMonth = new Array();
    dateTime = new Date();
    year = dateTime.getFullYear();
    month = dateTime.getMonth() == 0 ? 12 : dateTime.getMonth() + 1;
    var i = 0;
    while (i < 3) {
        arrayYearMonth.push(year + "-" + month);
        month = ++month;
        if (month == 13) {
            month = 1;
            year = year + 1
            i++;
        }
    }
    return arrayYearMonth;
}


//生成预算明细--层层调用呢
var createFListHtml = function () {
    var date = new Date();
    var ArrayYearMonth = GetArrayYearMonth();
    $.ajax({
        url: "../ashx/GetOrgProjectDataHandler.ashx",
        type: "Post",
        dataType: "json",
        data: {
            "ServiceType": 1,
            "orgID": $("#levelThree").val(),
            "startYear": date.getFullYear(),
            "startMonth": date.getMonth() == 0 ? 12 : date.getMonth() + 1,
            "endYear": date.getFullYear() + 2,
            "endMonth": 12,
            "containOutsource": $("input:radio:checked").val()
        },
        beforeSend: function () {
            $.blockUI({ message: $("#divBoxBlockUI").html() });
        },
        complete: function () {
           
        },
        success: function (data) {
            $("#topbarsecond").show();
            $("#topbarthird").show();
            BuildHtml(data, ArrayYearMonth);
            FrozenColumns();//固定表头
            QueryBudgetDetail();
            totalByProject();
            totalByL4();
        },
        error: function () {
            alert("调用失败");
        }
    })
}
//查询
var QueryData = function () {
  
    var orgID = $("#hidDept").val();
    if (orgID == "" || orgID == "00000010") {
        return;
    }
    $.blockUI({ message: $("#divBoxBlockUI").html() });
    $.ajax({
        url: "../ashx/GetOrgProjectDataHandler.ashx",
        type: "Post",
        dataType: "json",
        data: {
            "ServiceType": 4,
            "orgID": orgID,
            "startDate": $("#startdate").val(),
            "endDate": $("#enddate").val(),
            "ProjectID": $("#selProject").val()
        },
        beforeSend: function () {
          
        },
        complete: function () {
           
        },
        success: function (data) {
            $("#topbarsecond").show();
            $("#topbarthird").show();
            $("#titlefirst").show();
            if (data != null) {
                var json = eval(data);
                BuildHtml(json.project, json.ColumnName);
                insertValue(json.details);
                var column = [];
                $.each(json.ColumnName, function (i, data) {
                    var temp = {};
                    temp.field = data;
                    temp.title = data;
                    temp.width = 60;
                    column.push(temp);
                })
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
                totalByProject(column);
                totalByL4(column);
                $("#inputDetail input[type='text'][data]").each(function (a, b) {
                    $(this).attr("readonly", "readonly");
                });
                $.unblockUI();
            }
        },
        error: function () {
            alert("调用失败");
        }
    })  
}

var BuildHtml = function (data, ArrayYearMonth) {
    var obj = eval(data);;
    var tableDetail = "<table width='100%' border='0' id='fcstDetail' cellspacing='0' cellpadding='0' class='table_all'>";
    tableDetail += "<thead>";
    tableDetail += "<tr>";
    tableDetail += "<th field='Level1'>Level1</th>";
    tableDetail += "<th field='Level2'>Level2</th>";
    tableDetail += "<th field='Level3'>Level3</th>";
    tableDetail += "<th field='Level4'>Level4</th>";
    tableDetail += "<th field='Project'>Project</th>";
    for (var j = 0; j < ArrayYearMonth.length; j++) {
        tableDetail += "<th field='" + ArrayYearMonth[j] + "'>" + ArrayYearMonth[j] + "</th>";
    }
    tableDetail += "</tr>";
    tableDetail += "</thead>";

    for (var i = 0; i < obj.length; i++) {
        var tablerow;
        if (i % 2 == 0) {
            tablerow = "<tr class='thone'><td id='123' >" + obj[i].OrgLvl1Txt + "</td>" + "<td>" + obj[i].OrgLvl2Txt + "</td>" + "<td>" + obj[i].OrgLvl3Txt + "</td>" + "<td>" + obj[i].OrgLvl4Txt + "</td>" + "<td>" + obj[i].ProjectName + "</td>";
        }
        else {
            tablerow = "<tr class='thsecond'><td >" + obj[i].OrgLvl1Txt + "</td>" + "<td>" + obj[i].OrgLvl2Txt + "</td>" + "<td>" + obj[i].OrgLvl3Txt + "</td>" + "<td>" + obj[i].OrgLvl4Txt + "</td>" + "<td>" + obj[i].ProjectName + "</td>";
        }
        var IS_outsource = obj[i].FIS_outsource.toString().toLowerCase();
        //数组有多长，输入列就要有多少.
        for (var j = 0; j < ArrayYearMonth.length; j++) {
            var id = obj[i].OrgLvl4 + "_" + obj[i].FProject_ID + "_" + ArrayYearMonth[j].split('-')[0] + "_" + ArrayYearMonth[j].split('-')[1] + "_" + IS_outsource;//组织唯一主键
            var input = "<input id='" + id + "' class='input_width' onchange='totalValue(this)' data='0' type='text' ProjectName='" + obj[i].ProjectName + "' orgidL1Name='" + obj[i].OrgLvl1Txt + "' orgidL2Name='" + obj[i].OrgLvl2Txt + "' orgidL3Name='" + obj[i].OrgLvl3Txt + "' orgidL4Name='" + obj[i].OrgLvl4Txt + "' orgidL4='" + obj[i].OrgLvl4 + "'" +
            " orgidL3='" + obj[i].OrgLvl3 + "'orgidL2='" + obj[i].OrgLvl2 + "' orgidL1='" + obj[i].OrgLvl1 + "' ProjectID='" + obj[i].FProject_ID + "' year='" + ArrayYearMonth[j].split('-')[0] + "'" +
            " month='" + ArrayYearMonth[j].split('-')[1] + "' isouthouse='" + IS_outsource + "' value=0></input>";//太长了
            tablerow += "<td>" + input + "</td>";
        }
        tablerow += "</tr>";
        tableDetail += tablerow;
    }
    tableDetail += "</table>";
    $("#inputDetail").html(tableDetail);
}
//给已经构造好的表结构查询值.
var QueryBudgetDetail = function () {
    var date = new Date();
    $.ajax({
        url: "../ashx/GetOrgProjectDataHandler.ashx",
        type: "Post",
        dataType: "json",
        async: false,
        data: {
            "ServiceType": 2,
            "orgID": $("#levelThree").val(),
            "startYear": date.getFullYear(),
            "startMonth": date.getMonth() == 0 ? 12 : date.getMonth() + 1,
            "endYear": date.getFullYear() + 2,
            "endMonth": 12,
            "containOutsource": $("input:radio:checked").val()
        },
        success: function (data) {
            insertValue(data);
        },
        error: function () {
            alert("调用失败");
        }
    })
}
//填充值
var insertValue = function (data) {

    var obj = eval(data);
    for (var i = 0; i < obj.length; i++) {
        var IS_outsource = obj[i].FIS_outsource.toString().toLowerCase();
        var id = obj[i].FOrg_Level4 + "_" + obj[i].FProject_ID + "_" + obj[i].FBudget_Year + "_" + obj[i].FBudget_Month + "_" + IS_outsource;//组织唯一主键
        $("#" + id).val(obj[i].FBudget_Resource).attr("data", obj[i].ID);
        $("#" + id).attr("value", obj[i].FBudget_Resource);
    }
}
var FrozenColumns = function () {
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
    $.unblockUI();
}
var Save = function (data) {
    var json = [];
    $("#inputDetail  input[type='text'][data]").each(function (a, b) {
        var item = {};
        item.ID = $(b).attr("data");
        item.FIS_outsource = $(b).attr("isouthouse");
        item.FOrg_Level4 = $(b).attr("orgidL4");
        item.FOrg_Level3 = $(b).attr("orgidL3");
        item.FOrg_Level2 = $(b).attr("orgidL2");
        item.FOrg_Level1 = $(b).attr("orgidL1");
        item.FBudget_Year = $(b).attr("year");
        item.FBudget_Month = $(b).attr("month");
        item.FProject_ID = $(b).attr("ProjectID");
        item.FBudget_Resource = $(b).val();
        json.push(item);

    });
    var date = new Date();
    $.ajax({
        url: "../ashx/GetOrgProjectDataHandler.ashx",
        type: "Post",
        dataType: "text",
        data: {
            "ServiceType": 3,
            "orgID": $("#levelThree").val(),
            "startYear": date.getFullYear(),
            "startMonth": date.getMonth() == 0 ? 12 : date.getMonth() + 1,
            "endYear": date.getFullYear() + 2,
            "endMonth": 12,
            "containOutsource": $("input:radio:checked").val(),
            "ResourcePlanJson": JSON.stringify(json)
        },
        success: function (data) {
            alert("Save Success");
        },
        error: function () {
            alert("Save Failure");
        }
    })
}
var CreateData = function () {
    var yearMonth = GetArrayYearMonth();
    var columns = new Array();
    for (var i = 0; i < yearMonth.length; i++) {
        var column = {};
        column["title"] = yearMonth[i];
        column["field"] = yearMonth[i];
        column["align"] = "center";
        column["width"] = 60;
        columns.push(column);
    }
    return columns
}
//不同状态下图片显示不同
var ReplaceImage = function () {
    $(".icon-save").css("background", "url('../Image/FCSTInput/icon_editor.png')");
}
