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
        public ActionResult Subject(int id,string sId)
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
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public JsonResult ChangeWorkStatus(int id)
        {
            try
            {
                Progress.SetProgressStatus(id);
                return Json("Աշխատանքը հաստատված է:",JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Փորձեք կրկին:", JsonRequestBehavior.AllowGet);
            }
        }

    }
}