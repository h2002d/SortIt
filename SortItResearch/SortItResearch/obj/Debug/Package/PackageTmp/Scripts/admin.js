function saveSubject() {
    var subjectName = $('#subjectName').val();
    var subjectDescription = $('#subjectDescription').val();

    var subjectId = $('#subjectId').val();
    if (subjectName== '') {
        $("#validateSubject").append("Name is empty");
        return;
    } if (subjectDescription == '') {
        $("#validateSubject").append("Description is empty");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/Admin/SaveSubject",
        data: {
            id: subjectId,
            name: subjectName,
            description: subjectDescription
        },
        success: function () {
            location.reload();
        },
        error: function (data) {
            alert("Error! topic not added.");
        }
    });
}
function showProfileCertificates() {
    $('#profileCertificates').load("/Manage/Certificates/");
}
function changeFinalStatus(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/SetFinalStatus",
        data: {
            id: id,
        },
        success: function (data) {
            alert(data);
            location.reload();
        },
        error: function (data) {
            alert(data);
        }
    });
}
function deleteSubject(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteSubject",
        data: {
            id: id,
        },
        success: function () {
            location.reload();
        },
        error: function (data) {
            alert("Թեման չի ջնջվել");
        }
    });
}

function editSubject(id, name) {
    $('#subjectName').val(name);
   $('#subjectId').val(id);
}

function goToDetails(id) {
    window.location.href = "/Admin/SubjectDetails/" + id;
}
function openLesson(id) {
    window.location.href = "/Admin/Lesson/" + id;
}
function addModule(subject,moduleId) {
    $(".createModule").show();
    $('#createModule').load("/Admin/CreateModule/?moduleId=" + moduleId+'&subjectId='+subject);
}

function addLessonToDay(day,subject) {
    $(".createModule").show();
    $('#createModule').load("/Admin/AddLessonFromDays?dayId=" + day + "&subjectId=" + subject);
}

function addDay(module,dayId) {
    $(".createModule").show();
    $('#createModule').load("/Admin/CreateModuleDay/?dayId=" + dayId + '&moduleId=' + module);
}
function closePopup() {
    $('.createModule').hide();
}
function addLessonPartial(id) {
    $(".createModule").show();
    $('#createModule').load("/Admin/EditLessonPartial?DayId="+id);
}
function addQuestionPartial(id)
{
    $(".createModule").show();
    $('#createModule').load("/Admin/EditQuestionPartial?LessonId=" + id);
}
function deleteModule(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteModule",
        data: {
            id: id,
        },
        success: function () {
            location.reload();
        },
        error: function (data) {
            alert("Մոդուլը չի ջնջվել");
        }
    });
}
function deleteModuleDay(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteModuleDay",
        data: {
            id: id,
        },
        success: function () {
            location.reload();
        },
        error: function (data) {
            alert("Օրը չի ջնջվել");
        }
    });
}

function lessonFilter() {
    window.location.href = "/Admin/Lessons/" + $('#SubjectId').val();
}
function testFilter() {
    window.location.href = "/Admin/Tests/" + $('#LessonId').val();
}
function deleteLesson(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteLesson",
        data: {
            id: id,
        },
        success: function () {
            location.reload();
        },
        error: function (data) {
            alert("Դասընթացը չի ջնջվել");
        }
    });
}
function deleteLessonFromDay(dayId,lessonId) {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteLessonFromDay",
        data: {
            DayId: dayId,
            LessonId: lessonId
        },
        success: function () {
            location.reload();
        },
        error: function (data) {
            alert("Դասընթացը չի ջնջվել");
        }
    });
}

function addLesson(id) {
    window.location.href = "/Admin/editLesson/" + id;
}
function editQuestion(id) {
    window.location.href = "/Admin/editquestion/" + id;
}
function addLessonForDay(dayId, lessonId)
{
    $.ajax({
        type: "POST",
        url: "/Admin/SaveLessonFromDay",
        data: {
            DayId: dayId,
            LessonId: $("#LessonId").val()
        },
        success: function () {
            location.reload();
        },
        error: function (data) {
            alert("Դասընթացը չի ավելացվել");
        }
    });
}
function checkedRadioButton(obj) {
    var type = $('input[name=Type]:checked').val();
    if (type == 1)
        return;
    var id = obj.name;
    var el = obj.form.elements;
    console.log(obj);
    for (var i = 0; i < el.length; i++) {
        if (el[i].name != id) {
            if (el[i].name != 'Type') {
                console.log(el[i]);
                el[i].checked = false;
            }
        }
    }

    obj.checked = true;
}
 // 1 because we already have 0 loaded
var GlobalIndex=0;
function addAnswers(index) {
    if (GlobalIndex == null)
    {
        GlobalIndex = 0;
    }
    var type = $('input[name=Type]:checked').val();
    $.ajax({
        cache: false,
        type: "GET",
        url: "/Admin/AnswersPartial",
        data: {
            Index: GlobalIndex,
            Type:type
        },
        success: function (data) {
            // data will be the html from the partial view
            $("#AnswerContainer").append(data);
            GlobalIndex++;
        },
        error: function (xhr, ajaxOptions, thrownError) {
            // Handle the error somehow
        }
    }); // end ajax call
}
function setAnswerType() {

    var type = $('input[name=Type]:checked').val();
    console.log(type);
    if (type == 1) {
        $('#AnswerContainer').find("input[type='radio']").prop('checked', false);
        $('#AnswerContainer').find("input[type='radio']").attr('type', 'checkbox');
      
    }
    else if (type == 0)
    {
        $('#AnswerContainer').find("input[type='checkbox']").prop('checked', false);
        $('#AnswerContainer').find("input[type='checkbox']").attr('type', 'radio');
      
    }
}
function changeMandatory() {
    $('input[name=Type]').prop('checked', false);
}

function changeType() {
    $('input[name=IsMandatory]').prop('checked', false);
}
function deleteQuestion(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteQuestion",
        data: {
            id: id,
        },
        success: function () {
            location.reload();
        },
        error: function (data) {
            alert("Հարցը չի ջնջվել");
        }
    });
}

function changeRole(id,roleId) {
    $.ajax({
        type: "POST",
        url: "/Manage/ChangeRole",
        data: {
            id: id,
            roleId:roleId
        },
        success: function () {
            alert("Role changed successfuly")
            location.reload();
        },
        error: function (data) {
            alert("Fail");
        }
    });
}

function findStudent() {

    $('#StudentContent').load("/Manage/FindStudent?studentMail=" + $('#StudentName').val());
}

function findTeacher() {

    $('#TeacherContent').load("/Manage/FindTeacher?studentMail=" + $('#StudentName').val());
}
var removableAnswers = [];

function removeFromAnswers(id,elem)
{
    elem.closest('#answerDiv').remove();
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteAnswer",
        data: {
            id: id,
        },
        success: function () {
            location.reload();
        },
        error: function (data) {
            alert("Հարցը չի ջնջվել");
        }
    });
    removableAnswers.push(id);
    GlobalIndex--;
}
function removeFromAnswer(elem) {
    elem.closest('#answerDiv').remove();
    GlobalIndex--;
}
function removeAnswersOnSubmit()
{
    console.log("this is removed answers:"+removableAnswers);
    if (removableAnswers.length != 0) {
        removableAnswers.forEach(function (item, index) {
           
        });
    }
}
$(document).ready(function () {
    $('#gender').change(function () {
        var value = $(this).val();
        $('#StudentContent').load("/Manage/FindStudentByGender?isMale=" + value);

    });

    GlobalIndex = $('#AnswerCount').val();
});
function upload() {
    alert();
    var data = new FormData();
    var files = $(".uploadEditorImage").get(0).files;
    if (files.length > 0) {
        data.append("HttpPostedFileBase", files[0]);
        $('.image').val('/Files/presentation/' + files[0].name)
    }
    //.val('/images/' + files[0].name);
    $.ajax({
        url: "/Admin/PresentationUpload/",
        type: "POST",
        processData: false,
        contentType: false,
        data: data,
        success: function (response) {
            //code after success
            alert(response)
        },
        error: function (er) {

            alert(er.responseText);
        }


    });
}