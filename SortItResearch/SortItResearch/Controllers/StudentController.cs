using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using SortItResearch.Models;

namespace SortItResearch.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public ActionResult MySubject()
        {
            var sId = User.Identity.GetUserId();
            var subject = Models.MySubjectViewModel.GetSubjectByStudentId(sId);
            ViewBag.Modules = Models.Module.GetModulesBySubjectId(subject.Id);
            var Teacher = new Models.UserProfile(subject.TeacherId);
            ViewBag.Teacher = Teacher;
            return View(subject);
        }

        [Authorize(Roles = "Student")]
        public ActionResult Lesson(int id)
        {
            var Lesson = Models.Lesson.GetLesson(id, null).First();
            return View(Lesson);
        }

        [Authorize(Roles="Student")]
        public ActionResult Tests(int id)
        {
            var question = Question.GetQuestionByLessonId(id);
            List<Question> tests = new List<Question>();
            List<int> randomList = new List<int>();
            Random rnd = new Random();
            while (true)
            {

                int r = rnd.Next(question.Count());
                if (!randomList.Contains(r))
                    randomList.Add(r);
                if (randomList.Count >= 1)
                    break;
            }
            foreach (int item in randomList)
            {
                tests.Add(question.ElementAt(item));
            }
            return View(tests);
        }
    }
}