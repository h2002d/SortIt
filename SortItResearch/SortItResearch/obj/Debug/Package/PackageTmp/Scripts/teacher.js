function sendRequestApproval(token, accepted) {
    $.ajax({
        type: "POST",
        url: "/Manage/Requests",
        data: { t: token, accepted: accepted },
        success: function (data) {
            alert("Դիմումը ուղարկված է")
        },
        failure: function () {
            alert("Failed!");
        }
    });
}
function changeHomeworkStatus(id) {
    $.ajax({
        type: "POST",
        url: "/Teacher/ChangeWorkStatus/" + id,

        success: function (data) {
            alert(data)
            window.location.reload();
        },
        failure: function (ex) {
            alert(ex);
        }
    });
}
function openSubject(id,sId) {
    window.location.href = "/Teacher/Subject?id=" + id+"&sId="+sId;
};


function showProfileCertificates() {
    $("#profileInfo").css("display", "none");
    $("#profileCertificates").css("display", "block");
    $('#profileCertificates').load("/Manage/Certificates/");

    $(".nav-button-profileInfo").find("a").removeClass("active");

    $(".nav-button-profileCertificates").find("a").addClass("active");
}

function showProfileInfo() {
    $("#profileInfo").css("display", "block");
    $("#profileCertificates").css("display", "none");

    $(".nav-button-profileCertificates").find("a").removeClass("active");

    $(".nav-button-profileInfo").find("a").addClass("active");
}