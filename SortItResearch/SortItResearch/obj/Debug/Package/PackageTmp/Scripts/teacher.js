﻿function sendRequestApproval(token, accepted)
{
    $.ajax({
        type: "POST",
        url: "/Manage/Requests",
        data: { t: token,accepted: accepted},
        success: function (data) {
            alert("Դիմումը ուղարկված է")
        },
        failure: function () {
            alert("Failed!");
        }
    });
}
function changeHomeworkStatus(id)
{
    $.ajax({
        type: "POST",
        url: "/Teacher/ChangeWorkStatus/"+id,
        
        success: function (data) {
            alert(data)
            window.location.reload();
        },
        failure: function (ex) {
            alert(ex);
        }
    });
}
