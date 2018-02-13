using SortItResearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

    }
}