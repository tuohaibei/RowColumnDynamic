//通过project做统计
var previousValue;
var detailListInput;
var CreateTotalProject = function (obj, columns) {
    for (var i = 0; i <= obj.length; i++) {
        //取出第一个表有多少列就行了
        var firstHead = $(".datagrid-view2 .datagrid-header-row td");
        var tableDetail = "<table width='100%' border='0' id='fcstByProject' cellspacing='0' cellpadding='0' class='table_all'>";
        for (var i = 0; i < obj.length; i++) {
            var tablerow;
            //每行的左半部分
            if (i % 2 == 0) {
                tablerow = "<tr class='thone'><td>" + obj[i].orgidL1Name + "</td>" + "<td>" + obj[i].orgidL2Name + "</td>" + "<td>" + obj[i].orgidL3Name+ "</td>" + "<td>" + "</td>" + "<td>" + obj[i].ProjectName + "</td>";
            }
            else {
                tablerow = "<tr class='thsecond'><td >" + obj[i].orgidL1Name + "</td>" + "<td>" + obj[i].orgidL2Name + "</td>" + "<td>" + obj[i].orgidL3Name + "</td>" + "<td>" + "</td>" + "<td>" + obj[i].ProjectName + "</td>";
            }
            //数组有多长，输入列就要有多少.
            for (var j = 0; j < firstHead.length; j++) {
                // var id = obj[i].OrgLvl4 + "_" + obj[i].FProject_ID + "_" + $(firstHead[j] + ' span').first().text().split('-')[0] + "_" + $(firstHead[j] + ' span').first().text().split('-')[1] + "_" + IS_outsource;//组织唯一主键
                var input = "<input sumByProject='1' class='input_width'  data='0' type='text'" +
                "orgidL3='" + obj[i].orgidL3 + "'ProjectID='" + obj[i].ProjectID + "' year='" + $(firstHead[j]).attr("field").split('-')[0] + "'" +
                "month='" + $(firstHead[j]).attr("field").split('-')[1] + "' value=0></input>";//太长了
                tablerow += "<td>" + input + "</td>";
            }
            tablerow += "</tr>";
            tableDetail += tablerow;
        }

        tableDetail += "</table>";
        $("#totalByProject").html(tableDetail);
        //创建完成之后赋值
        InitByProjectInPutValue(columns);
    }
}
var totalByProject = function (columns) {
    var totalByProject = new Array();
    var k=0;
    detailListInput = $("#inputDetail .datagrid-body input[type='text']");
    for (var i = 0; i < detailListInput.length; i++)
    {
        var orgidL3 = $(detailListInput[i]).attr("orgidL3");
        var ProjectID = $(detailListInput[i]).attr("ProjectID");
        var isexist = false;
        for (var j = 0; j < totalByProject.length;j++)
        {
            if (totalByProject[j].orgidL3 == orgidL3 && totalByProject[j].ProjectID == ProjectID)
            {
                isexist = true;
                break;
            }
        }
        if (!isexist) {
            var item = {};
            item.orgidL3 = $(detailListInput[i]).attr("orgidL3");
            item.ProjectID = $(detailListInput[i]).attr("ProjectID");
            //item.year = $(input[i]).attr("year");
            //item.month = $(input[i]).attr("month");
            item.orgidL1Name = $(detailListInput[i]).attr("orgidL1Name");
            item.orgidL2Name = $(detailListInput[i]).attr("orgidL2Name");
            item.orgidL3Name = $(detailListInput[i]).attr("orgidL3Name");
            item.orgidL4Name = $(detailListInput[i]).attr("orgidL4Name");
            item.ProjectName = $(detailListInput[i]).attr("ProjectName");
            totalByProject.push(item);
        }
    }
    CreateTotalProject(totalByProject, columns);

}

//通过L4z做统计
var CreateTotalL4 = function (obj,column) {
    for (var i = 0; i <= obj.length; i++) {
        //取出第一个表有多少列就行了
        var firstHead = $(".datagrid-view2 .datagrid-header-row td");
        var tableDetail = "<table width='100%' border='0' id='fcstByL4' cellspacing='0' cellpadding='0' class='table_all'>";
        for (var i = 0; i < obj.length; i++) {
            var tablerow;
            //每行的左半部分
            if (i % 2 == 0) {
                tablerow = "<tr class='thone'><td>" + obj[i].orgidL1Name + "</td>" + "<td>" + obj[i].orgidL2Name + "</td>" + "<td>" + obj[i].orgidL3Name + "</td>" + "<td>" + obj[i].orgidL4Name+ "</td>" + "<td></td>";
            }
            else {
                tablerow = "<tr class='thsecond'><td >" + obj[i].orgidL1Name + "</td>" + "<td>" + obj[i].orgidL2Name + "</td>" + "<td>" + obj[i].orgidL3Name + "</td>" + "<td>" + obj[i].orgidL4Name+ "</td>" + "<td></td>";
            }
            //数组有多长，输入列就要有多少.
            for (var j = 0; j < firstHead.length; j++) {
                // var id = obj[i].OrgLvl4 + "_" + obj[i].FProject_ID + "_" + $(firstHead[j] + ' span').first().text().split('-')[0] + "_" + $(firstHead[j] + ' span').first().text().split('-')[1] + "_" + IS_outsource;//组织唯一主键
                var input = "<input sumByTeam='1' class='input_width'  data='0' type='text'" +
                "orgidL4='" + obj[i].orgidL4 + "' year='" + $(firstHead[j]).attr("field").split('-')[0] + "'" +
                "month='" + $(firstHead[j]).attr("field").split('-')[1] + "' value=0></input>";//太长了
                tablerow += "<td>" + input + "</td>";
            }
            tablerow += "</tr>";
            tableDetail += tablerow;
        }

        tableDetail += "</table>";
        $("#totalByL4Team").html(tableDetail);
        InitL4InPutValue(column);
    }
}
var totalByL4 = function (column) {
    var totalByL4 = new Array();
    var k = 0;
    for (var i = 0; i < detailListInput.length; i++) {
        var orgidL4 = $(detailListInput[i]).attr("orgidL4");
        var isexist = false;
        for (var j = 0; j < totalByL4.length; j++) {
            if (totalByL4[j].orgidL4 == orgidL4) {
                isexist = true;
                break;
            }
        }
        if (!isexist) {
            var item = {};
            item.orgidL3 = $(detailListInput[i]).attr("orgidL3");
            item.orgidL4 = $(detailListInput[i]).attr("orgidL4");
            item.ProjectID = $(detailListInput[i]).attr("ProjectID");
            //item.year = $(input[i]).attr("year");
            //item.month = $(input[i]).attr("month");
            item.orgidL1Name = $(detailListInput[i]).attr("orgidL1Name");
            item.orgidL2Name = $(detailListInput[i]).attr("orgidL2Name");
            item.orgidL3Name = $(detailListInput[i]).attr("orgidL3Name");
            item.orgidL4Name = $(detailListInput[i]).attr("orgidL4Name");
            item.ProjectName = $(detailListInput[i]).attr("ProjectName");
            totalByL4.push(item);
        }
    }
    CreateTotalL4(totalByL4,column);

}
var InitByProjectInPutValue=function(columns)
{
    FrozenColumnsByProject(columns);
    var inputByProject = $("input[sumByProject='1']");
    for (var i = 0; i < inputByProject.length; i++)
    {
        var value = 0
        var year = $(inputByProject[i]).attr("year");
        var month = $(inputByProject[i]).attr("month");
        var orgidL3 = $(inputByProject[i]).attr("orgidL3");
        var ProjectID = $(inputByProject[i]).attr("ProjectID");
        for(var j=0;j<detailListInput.length;j++ )
        {
            if (year == $(detailListInput[j]).attr('year') && month == $(detailListInput[j]).attr('month') && orgidL3 == $(detailListInput[j]).attr('orgidL3') && ProjectID == $(detailListInput[j]).attr('ProjectID'))
            {
                value +=parseFloat($(detailListInput[j]).val());
            }
        }
        $(inputByProject[i]).val(value);
    }
}
var InitL4InPutValue = function (columns) {
    FrozenColumnsbylevel4(columns);
    var inputByL4 = $("input[sumByTeam='1']");
    for (var i = 0; i < inputByL4.length; i++) {
        var value = 0
        var year = $(inputByL4[i]).attr("year");
        var month = $(inputByL4[i]).attr("month");
        var orgidL4 = $(inputByL4[i]).attr("orgidL4");
        for (var j = 0; j < detailListInput.length; j++) {
            if (year == $(detailListInput[j]).attr('year') && month == $(detailListInput[j]).attr('month') && orgidL4 == $(detailListInput[j]).attr('orgidL4')) {
                value +=parseFloat($(detailListInput[j]).val());
            }
        }
        $(inputByL4[i]).val(value);
    }
}
var FrozenColumnsByProject=function(frozeColumns)
{
    var columns;
    //如果等于空那就用后年的最后一个月
    if (null==frozeColumns)
    {
        columns=CreateData();
    }
    else
    {
        columns = frozeColumns;
    }
    $("#fcstByProject").datagrid({
        title:'',
        iconCls: '',
        width: "100%",
        height: "100%",
        //url: 'data/datagrid_data.json',
        frozenColumns: [[
            { field: '1', title: 'Level1' },
            { field: '2', title: 'Level2' },
            { field: '3', title: 'Level3' },
            { field: '4', title: 'Level4' },
            { field: '5', title: 'Project' }
        ]],
        columns: [
           columns
        ], rownumbers: true
    });
}
var FrozenColumnsbylevel4 = function (frozeColumns)
{
    var columns;
    //如果等于空那就用后年的最后一个月
    if (null == frozeColumns) {
        columns = CreateData();
    }
    else {
        columns = frozeColumns;
    }
    $("#fcstByL4").datagrid({
        iconCls: '',
        title: '',
        width: "100%",
        height: "100%",
        //url: 'data/datagrid_data.json',
        frozenColumns: [[
            { field: '1', title: 'Level1' },
            { field: '2', title: 'Level2' },
            { field: '3', title: 'Level3' },
            { field: '4', title: 'Level4' },
            { field: '5', title: 'Project' }
        ]],
        columns: [
             columns
        ], rownumbers: true
    });
}
var totalValue=function(input)
{
    //var re = /^[0-9]+.?[0-9]*$/;   //判断字符串是否为数字  
    var nubmer = $(input).val();
    if (isNaN(Number(nubmer)) || (nubmer.substr(0, 1)=='0'&&nubmer.length>1)) {
        alert("Please Write Number");
        $(input).val(0);
        return false;
    }
    var valueByProject = 0;
    var valueByL4 = 0;
    var year = $(input).attr('year');
    var month = $(input).attr('month');
    var l3OrgID = $(input).attr('orgidL3');
    var l4OrgID = $(input).attr("orgidL4");
    var projectID = $(input).attr('ProjectID')
    var $inputByProject = $("input[sumByProject='1'][year='" + year + "'][month='" + month + "'][orgidL3='" + l3OrgID + "'][ProjectID='" + projectID + "']");
    for (var j = 0; j < detailListInput.length; j++) {
        if (year == $(detailListInput[j]).attr('year') && month == $(detailListInput[j]).attr('month') && l3OrgID == $(detailListInput[j]).attr('orgidL3') && projectID == $(detailListInput[j]).attr('ProjectID')) {
            valueByProject += parseFloat($(detailListInput[j]).val());
        }
    }
    $inputByProject.val(valueByProject);
    var $inputByL4 = $("input[sumByTeam='1'][year='" + year + "'][month='" + month + "'][orgidL4='" + l4OrgID + "']");
    for (var j = 0; j < detailListInput.length; j++) {
        if (year == $(detailListInput[j]).attr('year') && month == $(detailListInput[j]).attr('month') && l4OrgID == $(detailListInput[j]).attr('orgidL4')) {
            valueByL4 += parseFloat($(detailListInput[j]).val());
        }
    }
    $inputByL4.val(valueByL4);
}

