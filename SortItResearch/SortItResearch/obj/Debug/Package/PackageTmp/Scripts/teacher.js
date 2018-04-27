function sendRequestApproval(token, accepted) {
    $.ajax({
        type: "POST",
        url: "/Manage/Requests",
        data: { t: token, accepted: accepted },
        success: function (data) {
            alert("Դիմումը ուղարկված է");
            window.location.href = '';
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

function filterStudents() {
    var keyword = $('#keyword').val();
    var interestId = $('#Interests').val();
    $('#studentContainer').empty();
    $.ajax({
        type: "POST",
        url: "/Teacher/FindStudentPartial/",
        data: {
            keyword: keyword,
            interestId:interestId
        },
        success: function (data) {
            $('#studentContainer').append(data);
        },
        failure: function (ex) {
            alert(ex);
        }
    });
}

function requestStudent(userId,subjectId) {
    $.ajax({
        type: "POST",
        url: "/Manage/SendRequestTeacher/",
        data: {
            tId: userId,
            subId: subjectId
        },
        success: function (data) {
            alert("Request Sent");
        },
        failure: function (ex) {
            alert(ex);
        }
    });
}