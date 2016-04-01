/// <reference path="common.js" />

var GetAuthority = function () {
    $.ajax({
        url: "DataAPI/Ashx/AuthorityHandle.ashx",
        dataType: "text",
        success: function (data) {
            if (data.toLowerCase()!='false') {
                window.open("ResourcesEdit/DeptProjectSetPage.aspx");
            }
            else
            {
                $("#prompt .introduction").text("您没有开启该页的权限");
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown.error);
        }
    })
}

// block UI 通用
$(function () {
    $("body").append('<div id="divBoxBlockUI" style="display:none;"><div id="boxBlockUI" class="loading_all"><div class="loading_next"><div class="loading_infor_all"><img src="../Image/loading.gif" /></div></div></div></div>');
});
function customerBlockUI() {
    $.blockUI({ message: $("#divBoxBlockUI").html() });
}
