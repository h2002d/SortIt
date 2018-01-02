function openLesson(lessonId) {
    window.location.href = "/Student/Lesson/" + lessonId;
}

function generateTest(lessonId) {
    window.location.href = "/Student/Tests/" + lessonId;
}


function uploadFile() {
    alert();
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

function enrollSubject(subjectId) {
    $.ajax({       
        type: "POST",
        url: "/Student/AddSubject",
        data: {
            subject: subjectId
        },
        success: function (data) {
            alert("Դասընթացը ավելացված է");
            window.location.href = "/Student/MySubjects";
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("Դասընթացն առկա է");
        }
    })
}

$(document).ready(function () {
    $(".mysubjects").click(function () {
        window.location.href = "/Student/MySubject/" + $(this).prop('id');
    });
});

function showAttachementUpload(lessonId) {
    $("#createModule").show();
    $('#createModule').load("/Student/Attachement/?lessonId=" + lessonId);
}