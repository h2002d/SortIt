using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SortItResearch.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace SortItResearch.Controllers
{

    public class TeacherController : Controller
    {


        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Teacher(string userId)
        {
            UserProfile user = new UserProfile(userId);
            List<TeacherSkillViewModel> skills = new List<TeacherSkillViewModel>();
            List<Subject> subjects = Models.Subject.GetSubject(null);
            foreach (var subject in subjects)
            {
                List<Module> module = Module.GetModulesBySubjectId(subject.Id);
                TeacherSkillViewModel skill = new TeacherSkillViewModel();
                skill.Subject = subject;
                skill.Modules = module;
                skills.Add(skill);
            }
            ViewBag.Skills = skills;
            ViewBag.SelectedSkills = TeacherSkillViewModel.GetSelectedSkills(userId);
            ViewBag.Subjects = subjects;
            return View(user);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult MyStudent()
        {
            var inviteList = InviteModel.GetInviteByTeacherId(User.Identity.GetUserId());
            return View(inviteList);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult Works()
        {
            var homeworks = Progress.GetProgressByTeacher(User.Identity.GetUserId());
            return View(homeworks);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult Subject(int id, string sId)
        {
            var subject = Models.MySubjectViewModel.GetSubjectByStudentIdById(sId, id);
            if (subject.Id == null)
            {
                return RedirectToAction("Subjects");
            }
            else
            {

                var Modules = Models.Module.GetModulesBySubjectId(subject.Id);
                foreach (var item in Modules)
                {
                    foreach (var day in item.ModuleDays)
                    {
                        foreach (var lesson in day.Lessons)
                        {
                            lesson.Passed = Models.Progress.GetProgressByLessonId(sId, Convert.ToInt32(lesson.Id)).Passed;

                        }
                    }
                }
                ViewBag.Modules = Modules;
                var Teacher = new Models.UserProfile(subject.TeacherId);
                ViewBag.Teacher = Teacher;
                return View(subject);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public JsonResult ChangeWorkStatus(int id)
        {
            try
            {
                Progress.SetProgressStatus(id);
                return Json("Work approved", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Fail!", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult MyCertificates()
        {
            var certificates = Certificate.GetCertificateByStudentId(User.Identity.GetUserId());
            return View(certificates);
        }


        public ActionResult Certificates(int certId)
        {
            var certificate = Certificate.GetCertificate(certId).First();
            return View(certificate);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult FindStudent()
        {
            ViewBag.Interests = TopicArea.GetTopicArea(null);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public ActionResult FindStudentPartial(string keyword, int? interestId)
        {
            var researchTopics = ResearchTopics.GetTopicByInterestId(interestId, keyword);
            return PartialView(researchTopics);
        }
    }
}