using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SortItResearch.Models;
using Microsoft.AspNet.Identity.Owin;

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
    }
}