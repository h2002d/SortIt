using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
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
        public JsonResult FileUpload()
        {
            try
            {
                HttpPostedFile file = null;
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    file = System.Web.HttpContext.Current.Request.Files["HttpPostedFileBase"];
                }
                string stamp = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                string filename = file.FileName;
                string pic = System.IO.Path.GetFileName(filename);
                string thumbnailpic = System.IO.Path.GetFileName(file.FileName.Split('.')[0] + "_thumb." + file.FileName.Split('.')[1]);

                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/images/subjects"), pic);
                string thumbpath = System.IO.Path.Combine(
                                        Server.MapPath("~/images/subjects"), thumbnailpic);
                // file is uploaded
                file.SaveAs(path);

                ImageResizer.ResizeSettings resizeSetting = new ImageResizer.ResizeSettings
                {
                    Width = 285,
                    Height = 175,
                    Format = file.FileName.Split('.')[1]
                };
                ImageResizer.ImageBuilder.Current.Build(path, thumbpath, resizeSetting);
                // after successfully uploading redirect the user
                return Json("File Uploaded", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Eror");
            }
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
            var lesson = Models.Lesson.GetLesson(id, 0).First();
            return View(lesson);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult EditLesson(int? id)
        {
            ViewBag.Subjects = Models.Subject.GetSubject(null);
            var context = new ApplicationDbContext();
            var users = context.Users
    .Where(x => x.Roles.Select(y => y.RoleId).Contains("59"))
    .ToList();
            List<UserProfile> profiles = new List<UserProfile>();
            foreach (var user in users)
            {
                UserProfile profile = new UserProfile(user.Id);
                profiles.Add(profile);
            }
            ViewBag.Teacher = profiles;
            if (id != null)
            {
                var lesson = Models.Lesson.GetLesson(id, 0).First();
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
            lesson.SubjectId = Models.Module.GetModules(Models.ModuleDay.GetDay(DayId).First().ModuleId).First().SubjectId;
            var context = new ApplicationDbContext();
            var users = context.Users
    .Where(x => x.Roles.Select(y => y.RoleId).Contains("59"))
    .ToList();
            List<UserProfile> profiles = new List<UserProfile>();
            foreach (var user in users)
            {
                UserProfile profile = new UserProfile(user.Id);
                profiles.Add(profile);
            }
            ViewBag.Teacher = profiles;
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
            ViewBag.Lessons = Models.Lesson.GetLesson(null, 0);//temanery menak
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
            ViewBag.Lessons = Models.Lesson.GetLesson(null, 0);//temanery menak
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
            ApplicationDbContext context = new ApplicationDbContext();
            var users = context.Users
   .Where(x => x.Roles.Select(y => y.RoleId).Contains("88"))
   .ToList();
            List<UserProfile> profiles = new List<UserProfile>();
            foreach (var user in users)
            {
                UserProfile profile = new UserProfile(user.Id);
                profile.isTeacher = true;
                profiles.Add(profile);
            }
            return View(profiles);
        }


        [Authorize(Roles = "Administrator")]
        public ActionResult Teachers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var users = context.Users
   .Where(x => x.Roles.Select(y => y.RoleId).Contains("59"))
   .ToList();
            List<UserProfile> profiles = new List<UserProfile>();
            foreach (var user in users)
            {
                UserProfile profile = new UserProfile(user.Id);
                profile.isTeacher = true;
                profiles.Add(profile);
            }
            return View(profiles);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Finals(int? id)
        {
            if (id == null)
            {
                id = Models.Subject.GetSubject(null).First().Id;
            }
            var finals = Dissertation.GetDissertationBySubjectId(Convert.ToInt32(id));
            return View(finals);
        }

        [HttpPost]
        public JsonResult PresentationUpload()
        {
            try
            {
                HttpPostedFile file = null;
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    file = System.Web.HttpContext.Current.Request.Files["HttpPostedFileBase"];
                }
                string stamp = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                string filename = file.FileName;
                string pic = System.IO.Path.GetFileName(filename);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Files/presentations"), pic);
                // file is uploaded
                file.SaveAs(path);

                // after successfully uploading redirect the user
                return Json("File Uploaded", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Eror");
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult SetFinalStatus(int id)
        {
            try
            {
                DissertationViewModel dissertation = Dissertation.GetDissertationById(id);
                dissertation.SaveStatus();
                Certificate cert = new Certificate();
                cert.StudentId = dissertation.StudentId;
                cert.SubjectId = dissertation.SubjectId;
                int certId = cert.Save();
                var user = UserManager.FindById(dissertation.StudentId);
                UserManager.AddToRole(user.Id, "Teacher");
                var callbackUrl = Url.Action("Certificates", "Teacher", new { certId = certId }, protocol: Request.Url.Scheme);

                string email = dissertation.Student.Email;
                string body = string.Format("Հարգելի {0} {1}, Ձեր աշխատությունը հաստատված է ադմինիստրատորի կողմից:</br> Սերտիֆիկատ գեներացնելու և հաստատելու համար անցեք <a href='{2}'>հղումով</a>", dissertation.Student.Name, dissertation.Student.SurName, callbackUrl);
                string subject = "SortIt.Աշխատությունը հաստատված է";
                SendMailModel.SendMail(email, body, subject);

                return Json("Աշխատանքը հաստատվել է");
            }
            catch (Exception ex)
            {
                return Json("ՁԱԽՈՂՈՒՄ:Չի հաստատվել:", JsonRequestBehavior.AllowGet);
            }
        }


    }
}