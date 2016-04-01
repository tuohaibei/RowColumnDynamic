function showAlert() {
    alert('该项不是用户和组不能选择');
}
$(function () {
    var pickSelectedText = decodeURI(RequestQueryString("selectText"));
    var pickSelectedValue = decodeURI(RequestQueryString("selectValue"));
    if (pickSelectedText != "" && pickSelectedValue != "") {
        var pText = pickSelectedText.split(";");
        var pValue = pickSelectedValue.split(";");
        var selectedNode = "";
        $.each(pValue, function (index, value) {
            if (value != "") {
                selectedNode += "<span title='双击删除' style='font-size:10pt;cursor:pointer;' ondblclick='removeObj(this)' value='" + value + "'>" + pText[index] + ";</span>";
            }
        });
        $("#txtSelectedUsers").html(selectedNode);
    }
});
$(function () {
    document.onkeydown = function (e) {
        if (window.location.href.indexOf('SelectUsers.aspx') != -1) {
            var ev = document.all ? window.event : e;
            if (ev.keyCode == 13) {
                queryFunction();
                return false;
            }
        }
    }
}); 

function getUsersOfGroup(node) {
    $.ajax({
        url: "../DataAPI/Ashx/OUTreeViewHandler.ashx",

        datatype: "json",
        data: "parentId=" + encodeURIComponent(node.id),
        success: function (data) {
            $("#divPersonInfo").empty();
            ReturnJsonHTML(data, "divPersonInfo");
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(node.id);
        }
    });
}
function queryFunction() {
    var queryKey = $("#txtQueryKey").val();
    if ($.trim(queryKey) == "") {
        alert("请输入查询关键字！");
        return;
    }
    var requestType = RequestQueryString("type");
    $.ajax({
        url: "../DataAPI/Ashx/OUTreeViewHandler.ashx",
        datatype: "json",
        data: "queryKey=" + encodeURIComponent(queryKey) + "&requestType=" + requestType,
        success: function (data) {
            $("#td_query_result").empty();
            ReturnQueryJsonHTML(data, "td_query_result");
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(node.id);
        }
    });
}
function ReturnJsonHTML(jsonString,containerID) {
    var strHtml;
    if (jsonString.length > 0) {
        strHtml = "<table cellpadding='0' id='tb_userinfo' cellspacing='0' style='width:100%;'>";
        strHtml += "<tr style='background-color:#F3F3F3;'><td class='title'>组织缩写</td><td class='title' style='height:25px;'>组织单位名称</td><td class='title'>上级组织单位名称</td></tr>";
        $.each(jsonString, function (index, value) {
            strHtml += "<tr text='" + value.OrgShortName + "'adNumber='" + value.OUID + "' sapNumber='" + value.SapNumber + "' class='trClass'><td style='height:20px;' class='personInfo'>" + value.OUName + "</td><td class='personInfo'>" + value.OrgShortName + "</td><td class='personInfo'>" + value.ParentOrgName + "</td></tr>";
        });
        strHtml += "</table>";
        $("#" + containerID).html(strHtml);
        AddEventForObj(containerID);
        AddMouseoverEvent(containerID);
    }
    else {
        $("#" + containerID).empty();
    }
}

function ReturnQueryJsonHTML(jsonString, containerID) {
    var strHtml;
    if (jsonString.length > 0) {
        strHtml = "<table cellpadding='0' id='tb_userinfo' cellspacing='0' style='width:100%;'>";
        strHtml += "<tr style='background-color:#F3F3F3;'><td class='title'>名称</td><td class='title'>帐号</td><td class='title' style='height:25px;'>职务</td><td class='title'>电话</td></tr>";
        $.each(jsonString, function (index, value) {
            strHtml += "<tr text='" + value.Name + "'adNumber='" + value.AdAccount + "' sapNumber='" + value.SapNumber + "' class='trClass'><td style='height:20px;' class='personInfo'>" + value.Name + "</td><td class='personInfo'>" + value.AdAccount + "</td><td class='personInfo'>" + value.Post + "</td><td class='personInfo'>" + value.Telephone + "</td></tr>";
        });
        strHtml += "</table>";
        $("#" + containerID).html(strHtml);
        AddEventForObj(containerID);
        AddMouseoverEvent(containerID);
    }
    else {
        $("#" + containerID).empty();
    }
}

function AddEventForObj(id) {
    $("#" + id).find("tr").click(function () {
        $(this).addClass("selectedTR");
        $(this).siblings().removeClass("selectedTR");
    }).dblclick(function () {
        var returnKey = RequestQueryString("key");
        if (returnKey.toLowerCase()=="sap") {
            chooseUser($(this).attr("text"), $(this).attr("sapNumber"));
        } else {
            chooseUser($(this).attr("text"), $(this).attr("adNumber"));
        }
    });
}

function AddMouseoverEvent(id) {
    $("#" + id).find("tr").mouseover(function () {
        if (!$(this).hasClass("selectedTR")) {
            $(this).addClass("overClass");
        }
    }).mouseout(function () {
        $(this).removeClass("overClass");
    });
}

function chooseUser(text, value) {
    if (value == '') {
        alert('ID为空');
        return;
    }
    var selectedUserControlHtml = $("#txtSelectedUsers").html();
    var singleSelect = RequestQueryString("singleSelect");
    if (singleSelect != '' && $("#txtSelectedUsers").find("span").length == 1) {
        alert('不能多选！');
        return;
    }
    if (selectedUserControlHtml.indexOf(value) != -1) {
        alert('已选择！');
        return;
    }
    var selectedNode = "<span title='双击删除' style='font-size:10pt;cursor:pointer;' ondblclick='removeObj(this)' value='" + value + "'>" + text + ";</span>";
    $("#txtSelectedUsers").html(selectedUserControlHtml + selectedNode);
}
function PostUserInfo() {
    var userControlHtml = $("#txtSelectedUsers").html();
    var selectedType = RequestQueryString("type");
    var alertMassage = "未选择信息,是否关闭窗口？";
    if (selectedType == "department") {
        alertMassage = "未选择组织";
    }
    if (userControlHtml.length == 0) {
        if (!confirm(alertMassage)) {
            return;
        }
    }

    var textResult = "";
    var valueResult = "";
    $("#txtSelectedUsers span").each(function () {
        valueResult += $(this).attr("value") + ";";
        textResult += $(this).text();
    });
    valueResult = valueResult.substring(0, valueResult.length - 1);
    textResult = textResult.substring(0, textResult.length - 1);
    var txtControlId = RequestQueryString("txtID");
    var valueControlId = RequestQueryString("valueID");
    window.opener.BackOffAssignment(txtControlId, valueControlId, textResult, valueResult);
    var callBackFuntion = RequestQueryString("functionName");
    if (callBackFuntion != "") {
        callBackFuntion = window.opener[callBackFuntion];
        callBackFuntion(valueResult, txtControlId, valueControlId);
    }
    window.close();
}
function RemoveUsers() {
    $("#txtSelectedUsers").empty();
}
function removeObj(obj) {
    $(obj).remove();
}

function RequestQueryString(paras) {
    var url = location.href;
    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
    var paraObj = {};
    var i;
    var j;
    for (i = 0; j = paraString[i]; i++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof(returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}

function OpenSelectUserDialog(txtid, selectType, returnDataType) {
    
}

function BackOffAssignment(txtControlId, valueControlId, textResult, valueResult) {
    if ($("#" + txtControlId).is("span")) {
        $("#" + txtControlId).text(textResult);
    }
    else {
        $("#" + txtControlId).val(valueResult);
    }
    valueControlId = valueControlId.replace("#", "");
    $("#" + valueControlId).val(textResult);
}

// add by guo.liang
/*
selectUserValueClientID: 保存Key值控件的ID
selectUserTextClientID： 保存Text值控件的ID
selectType ：选择类型：默认为选人、值为department为选部门
returnDataType：返回数据类型（Key值），默认为AD帐号,值为sap为SAP ID号为Key值
singleSelect:true为单选
isSelectOrg:true为可以选部门，也可以选人
*/
function selectUser(selectUserValueClientID, selectUserTextClientID, returnDataType, selectUserCallbackFuc, singleSelect,isSelectOrg) {
    var dialogWidth = 706;
    var dialogHeight = 540;
    var dialogTop = (window.screen.height - dialogHeight) / 2;
    var dialogLeft = (window.screen.width - dialogWidth) / 2;
    var selectType = "";
    if ($("#" + selectUserTextClientID).is("span")) {
        selectText = $("#" + selectUserTextClientID).text();
    }
    else {
        selectText = $("#" + selectUserTextClientID).val();
    }
    selectValue = $("#" + selectUserValueClientID).val();
    var url = '../PeopleEdit/SelectUsers.aspx?functionName=' + selectUserCallbackFuc + '&selectText=' + selectText + '&selectValue=' + selectValue + '&isdlg=1&txtID=' + selectUserTextClientID + '&type=' + selectType + '&key=' + returnDataType + '&valueID=' + selectUserValueClientID + '';
    if (singleSelect === "true") {
        url += "&singleSelect=true";
    }
    if(isSelectOrg ==="true") {
        url += "&IsSelectOrg=1";
    }
    window.open(url,'', 'height=' + dialogHeight + ',width=' + dialogWidth + ',top=' + dialogTop + ',left=' + dialogLeft + ',toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no');
}

function selectDept(selectUserValueClientID, selectUserTextClientID, returnDataType, selectUserCallbackFuc, singleSelect) {
    var dialogWidth = 706;
    var dialogHeight = 540;
    var dialogTop = (window.screen.height - dialogHeight) / 2;
    var dialogLeft = (window.screen.width - dialogWidth) / 2;
    var selectType = "department";
    if ($("#" + selectUserTextClientID).is("span")) {
        selectText = $("#" + selectUserTextClientID).text();
    }
    else {
        selectText = $("#" + selectUserTextClientID).val();
    }
    selectValue = $("#" + selectUserValueClientID).val();
    var url = '../PeopleEdit/SelectUsers.aspx?IsSelectOrg=1&functionName=' + selectUserCallbackFuc + '&selectText=' + selectText + '&selectValue=' + selectValue + '&isdlg=1&txtID=' + selectUserTextClientID + '&type=' + selectType + '&key=' + returnDataType + '&valueID=' + selectUserValueClientID + '';
    if (singleSelect === "true") {
        url += "&singleSelect=true";
    }
    window.open(url, '', 'height=' + dialogHeight + ',width=' + dialogWidth + ',top=' + dialogTop + ',left=' + dialogLeft + ',toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no');
}