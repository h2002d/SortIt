function sendRequestApproval(token, accepted)
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
