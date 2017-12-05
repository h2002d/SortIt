using SortItResearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SortItResearch.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Subject()
        {
            var subjects = Models.Subject.GetSubject(null);

            return View(subjects);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult SaveSubject(Subject subject)
        {
            subject.Save();
            return RedirectToAction("Subject");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteSubject(int id)
        {
            Models.Subject.Delete(id);
            return RedirectToAction("Subject");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult SubjectDetails(int id)
        {
            var subject = Models.Subject.GetSubject(id).First();
            ViewBag.Modules = Models.Module.GetModulesBySubjectId(id);
            return View(subject);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult CreateModule(int? moduleId, int? subjectId)
        {
            if (moduleId != null)
            {
                var module = Module.GetModules(moduleId).First();
                return View(module);
            }
            else
            {
                Module module = new Module();
                module.SubjectId = Convert.ToInt32(subjectId);
                return View(module);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult SaveModule(Module module)
        {
            module.Save();
            return RedirectToAction("SubjectDetails", new { id = module.SubjectId });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteModule(int id)
        {
            Models.Module.Delete(id);
            return RedirectToAction("Subject");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult CreateModuleDay(int? dayId, int? moduleId)
        {
            ModuleDay moduleDay = new ModuleDay();
            if (dayId == null)
                moduleDay.ModuleId = Convert.ToInt32(moduleId);
            else
                moduleDay = Models.ModuleDay.GetDay(dayId).First();
            return PartialView(moduleDay);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateModuleDay(ModuleDay moduleDay)
        {
            moduleDay.Save();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteModuleDay(int id)
        {
            Models.ModuleDay.Delete(id);
            return null;
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Lessons(int? id)
        {
            ViewBag.Subjects = Models.Subject.GetSubject(null);
            if (id == null)
            {
                var lessons = Models.Lesson.GetLesson(null, null);
                return View(lessons);
            }
            else
            {
                var lessons = Models.Lesson.GetLessonBySubjectId(Convert.ToInt32(id));
                return View(lessons);
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Lesson(int? id)
        {
            var lesson = Models.Lesson.GetLesson(id,0).First();
            return View(lesson);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult EditLesson(int? id)
        {
            ViewBag.Subjects = Models.Subject.GetSubject(null);
            if (id != null)
            {
                var lesson = Models.Lesson.GetLesson(id,0).First();
                return View(lesson);

            }
            else
            {
                return View();
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult EditLessonPartial(int DayId)
        {
            Lesson lesson = new Models.Lesson();
            lesson.DayId = DayId;
            ViewBag.Subjects = Models.Subject.GetSubject(null);
            return PartialView(lesson);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditLessonPartial(Lesson lesson)
        {
            int lessonId = lesson.Save();
            Models.Lesson.SaveLessonFromDay(Convert.ToInt32(lesson.DayId), lessonId);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditLesson(Lesson lesson)
        {
            lesson.Save();
            return RedirectToAction("Lessons");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteLesson(int id)
        {
            Models.Lesson.Delete(id);
            return null;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteLessonFromDay(int DayId, int LessonId)
        {
            Models.Lesson.DeleteLessonFromDay(DayId, LessonId);
            return null;
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult AddLessonFromDays(int DayId, int subjectId)
        {
            var lessons = Models.Lesson.GetLessonBySubjectId(subjectId);
            ViewBag.DayId = DayId;
            return PartialView(lessons);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult SaveLessonFromDay(int DayId, int LessonId)
        {
            Models.Lesson.SaveLessonFromDay(DayId, LessonId);
            return null;
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Tests(int? id)
        {
            ViewBag.Lessons = Models.Lesson.GetLesson(null,0);//temanery menak
            if (id != null)
            {
                var model = Question.GetQuestionByLessonId(Convert.ToInt32(id));
                return View(model);
            }
            else
            {
                var model = Question.GetQuestions(null);
                return View(model);
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult EditQuestion(int? id)
        {
            ViewBag.Lessons = Models.Lesson.GetLesson(null,0);//temanery menak
            if (id == null)
            {
                Question question = new Question();

                return View(question);
            }
            else
            {
                var model = Models.Question.GetQuestions(id).First();
                return View(model);
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult EditQuestionPartial(int LessonId)
        {
            Question model = new Question();
            model.LessonId = LessonId;
            return PartialView(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditQuestion(Question question)
        {
            int id = question.Save();
            return RedirectToAction("Tests");
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditQuestionPartial(Question question)
        {
            int id = question.Save();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [Authorize(Roles = "Administrator")]
        public PartialViewResult AnswersPartial(int index, int type)
        {
            Dictionary<int, int> checkInfo = new Dictionary<int, int>();
            checkInfo.Add(index, type);
            return PartialView(checkInfo);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteQuestion(int id)
        {
            Models.Question.Delete(id);
            return null;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteAnswer(int id)
        {
            Models.Answer.Delete(id);
            return null;
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Students()
        {
            return View();
        }

       

       
    }
}