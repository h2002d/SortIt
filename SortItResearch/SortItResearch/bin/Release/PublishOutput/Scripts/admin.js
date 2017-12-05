function saveSubject() {
    var subjectName = $('#subjectName').val();
    var subjectId = $('#subjectId').val();
    if (subjectName== '') {
        $("#validateSubject").append("Անվանումը դատարկ է");
        return;
    }
    $.ajax({
        type: "POST",
        url: "/Admin/SaveSubject",
        data: {
            id: subjectId,
            name: subjectName
        },
        success: function () {
            location.reload();
        },
        error: function (data) {
            alert("Ձեր թեման չի ավելացել");
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
    $("#createModule").show();
    $('#createModule').load("/Admin/CreateModule/?moduleId=" + moduleId+'&subjectId='+subject);
}

function addLessonToDay(day,subject) {
    $("#createModule").show();
    $('#createModule').load("/Admin/AddLessonFromDays?dayId=" + day + "&subjectId=" + subject);
}

function addDay(module,dayId) {
    $("#createModule").show();
    $('#createModule').load("/Admin/CreateModuleDay/?dayId=" + dayId + '&moduleId=' + module);
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