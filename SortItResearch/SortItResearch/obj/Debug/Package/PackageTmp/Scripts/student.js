function openLesson(lessonId) {
    window.location.href = "/Student/Lesson/" + lessonId;
}

function generateTest(lessonId) {
    window.location.href = "/Student/Tests/" + lessonId;
}


function uploadFile() {
    var parent = $('.fileUpload').parent();
    var data = new FormData();
    var files = $('.fileUpload').parent().find(".uploadEditorImage").get(0).files;
    if (files.length > 0) {
        data.append("HttpPostedFileBase", files[0]);
        $('.fileUpload').parent().find('.file').val('/Files/homeworks/' + files[0].name)
    }
    var lessonId = $('#lessonId').val();
    //.val('/images/' + files[0].name);
    $.ajax({
        url: "/Student/FileUpload/" + lessonId,
        type: "POST",
        processData: false,
        contentType: false,
        data: data,
        success: function (response) {
            //code after success
            alert(response);
            window.location.reload();

        },
        error: function (er) {

            alert(er.responseText);
        }

    });
}
function chooseTeacher() {
    alert('Ընտրեք ղեկավար');
}
function showtest() {
    $("#lesson-info").css("display", "none");
    $("#lesson-test").css("display", "block");

    $(".nav-button-services").find("a").removeClass("active");
    $(".nav-button-home").find("a").addClass("active");
}
function showinfo() {
    $("#lesson-info").css("display", "block");
    $("#lesson-test").css("display", "none");
    $(".nav-button-home").find("a").removeClass("active");

    $(".nav-button-services").find("a").addClass("active");
}
function lessonPartial(id) {
    $('#lesson-container').empty();
    $('#lesson-container').append("<div class='loader'></div>")
    $('#lesson-container').load("/Student/Lesson/" + id);

}
function showProfileCertificates()
{
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
function showProgress()
{
    $("#progress").css("display", "block");
    $("#learnings").css("display", "none");
    $(".nav-button-learn").find("a").removeClass("active");

    $(".nav-button-progress").find("a").addClass("active");
}

function showLesson() {
    $("#progress").css("display", "none");
    $("#learnings").css("display", "block");
    $(".nav-button-progress").find("a").removeClass("active");

    $(".nav-button-learn").find("a").addClass("active");
}
function endLesson(lessonId) {
    $.ajax({
        url: "/Student/EndLesson/" + lessonId,
        type: "POST",
        processData: false,
        contentType: false,
        data: data,
        success: function (response) {
            //code after success
            alert(response);
            window.location.reload();

        },
        error: function (er) {

            alert(er.responseText);
        }

    });
}
function uploadFinalAttachement() {
    var parent = $('.fileUpload').parent();
    var data = new FormData();
    var files = $('.fileUpload').parent().find(".uploadEditorImage").get(0).files;
    if (files.length > 0) {
        data.append("HttpPostedFileBase", files[0]);
        $('.fileUpload').parent().find('.file').val('/Files/dissertations/' + files[0].name)
    }
    var lessonId = $('#lessonId').val();
    var category = $('#category').val();
    var description = $('#description').val();
    var shortDesc = $('#shortDesc').val();
    var title = $('#title').val();
    var design = $('#design').val();
    var status = $('#status').val();

    //.val('/images/' + files[0].name);
    $.ajax({
        url: "/Student/FinalUpload/",
        type: "POST",
        processData: false,
        contentType: false,
        data: {
            SubjectId: lessonId, Category: category, Description: description,
            ShortDescription: shortDesc,Title:title,Design:design,Status:status
        },
        success: function (response) {
            //code after success
            alert(response);
            window.location.reload();

        },
        error: function (er) {

            alert(er.responseText);
        }

    });
}

function submitAnswers() {
   
    var dict = {}; // create an empty array
    $(".test-div").each(function (index) {
        var rightAnswers = [];
        var id = $(this).prop('id');
        $(this).find('.col-md-12').find("input.test-answer:checked").each(function (i) {
            rightAnswers.push($(this).prop('value'));
            console.log(rightAnswers)
        });
        dict["[" + index + "].Key"] = id;
        dict["[" + index + "].Value"] = rightAnswers;
        console.log(index);
    });
    $.ajax({
        type: "POST",
        url: "/Student/Tests",
        data: dict,
        success: function (data) {
            window.location.href = data;
        },
        error: function (data) {
            alert("Ձեր թեման չի ավելացել");
        }
    });


}

function enrollSubject(subjectId, listIds) {

    var topic = $("#topic").val();
    var desc = $("#shortDesc").val();
    $.ajax({    
        
        type: "POST",
        url: "/Student/AddSubject",
        data: {
            subject: subjectId,
            categoryId: listIds,
            topicName: topic,
            description:desc
        },
        dataType:"json",
        success: function (data) {
            alert("Course selected");
            window.location.href = "/Student/MySubjects";
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("You already have this course");
        }
    })
}

$(document).ready(function () {
 
    $(".mysubjects").click(function () {
        window.location.href = "/Student/MySubject/" + $(this).prop('id');
    });
    $('.list-group-item').click(function () {
        $(this).addClass('active-lesson');
    });
    $('.btn-enroll').click(function () {
        var id = $(this).prop('id');
        event.preventDefault();
        var searchIDs = $(".checkbox-areas input:checkbox:checked").map(function () {
            return $(this).val();
        }).get();
        enrollSubject(id,searchIDs);
    });
});

function showAttachementUpload(lessonId) {
    $("#createModule").show();
    $('#createModule').load("/Student/Attachement/?lessonId=" + lessonId);
}

function showEnroll(id) {

    $(".createModule").show();
  
    $('.btn-enroll').attr('id',id)
}
function showFinalUpload(lessonId) {
    $("#createModule").show();
    $('#createModule').load("/Student/Final/?lessonId=" + lessonId);
}
