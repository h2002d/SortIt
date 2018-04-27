using SortItResearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace SortItResearch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Info()
        {
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult FindSkill()
        {
            ViewBag.Subjects = Subject.GetSubject(null);

            return View();
        }
        public ActionResult Lesson(int id)
        {
            var Lesson = Models.Lesson.GetLesson(id, null).First();
           return PartialView(Lesson);
        }

        public ActionResult Course(int id)
        {
            var subject = Models.MySubjectViewModel.GetSubject(id).First();
            var Modules = Models.Module.GetModulesBySubjectId(subject.Id);
           
            ViewBag.Modules = Modules;

            return View(subject);
        }

        [HttpPost]
        [Authorize]
        public JsonResult RefreshModule(int sId)
        {
            return Json(Module.GetModulesBySubjectId(sId), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Student")]
        public ActionResult SkillPartial(int? sId, int? mId, int? rId)
        {

            if (sId != null && mId != null)
            {
                var users = UserProfile.GetSkilledUsers(Convert.ToInt32(mId), Convert.ToInt32(rId));
                return PartialView(users);
            }
            return View();
        }
        [Authorize]
        public ActionResult Requests()
        {
            var invites = InviteModel.GetInviteByStudentId(User.Identity.GetUserId());
            return View(invites);
        }
    }
}