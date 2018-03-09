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
        public ActionResult MySubjects()
        {
            var sId = User.Identity.GetUserId();
            var subject = Models.MySubjectViewModel.GetSubjectByStudentId(sId);
            return View(subject);
        }

        [Authorize(Roles = "Student")]
        public ActionResult MySubject(int id)
        {
            var sId = User.Identity.GetUserId();
            ViewBag.Passed = Progress.IsPassed(id, sId);

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
                            lesson.Passed = Models.Progress.GetProgressByLessonId(User.Identity.GetUserId(), Convert.ToInt32(lesson.Id)).Passed;

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
        public JsonResult FileUpload(int id)
        {
            try
            {
                HttpPostedFile file = null;
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    file = System.Web.HttpContext.Current.Request.Files["HttpPostedFileBase"];
                }
                string stamp = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                string filename = file.FileName.Split('.')[0] + stamp + "." + file.FileName.Split('.')[1];
                string pic = System.IO.Path.GetFileName(filename);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Files/homeworks"), pic);
                // file is uploaded
                file.SaveAs(path);
                Progress homework = new Progress();
                homework.Attachement = pic;
                homework.LessonId = id;
                homework.Passed = false;
                homework.StudentId = User.Identity.GetUserId();
                homework.Save();
                SendMail(homework);
                // after successfully uploading redirect the user
                return Json("Աշխատանքը վերբեռնված է", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("ՁՍԽՈՂՈՒՄ:Աշխատանքը վերբեռնված չէ");
            }
        }

        [HttpPost]
        public JsonResult EndLesson(int id)
        {
            try
            {
              
                Progress homework = new Progress();
                homework.Attachement = "";
                homework.LessonId = id;
                homework.Passed = false;
                homework.StudentId = User.Identity.GetUserId();
                homework.Save();
                SendMail(homework);
                // after successfully uploading redirect the user
                return Json("Թեման ավարտված է", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("ՁՍԽՈՂՈՒՄ:");
            }
        }

        public void SendMail(Progress work)
        {
            var Teacher = UserProfile.GetTeacherByStudentLessonId(work.StudentId, work.LessonId);
            string link = string.Format("<div class='row' style='border:1px solid #808080' align='center'><p style='color:red'>Դուք ունեք նոր դիմում</p>" +
                "<a href='http://{0}/Files/homeworks/{1}'>{1}</a>" +
                "<input type='button' value='Հաստատել' onclick='javascript::window.location.href='http://{0}/Teacher/Works''></div>", Request.Url.Authority, work.Attachement);

            SendMailModel.SendMail(Teacher.Email, link, "SortIt. Նոր աշխատանք");
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public JsonResult FinalUpload(Dissertation newDissertation)
        {
            try
            {
                HttpPostedFile file = null;
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    file = System.Web.HttpContext.Current.Request.Files["HttpPostedFileBase"];
                }
                string stamp = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                string filename = file.FileName.Split('.')[0] + stamp + "." + file.FileName.Split('.')[1];
                string pic = System.IO.Path.GetFileName(filename);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Files/dissertations"), pic);
                // file is uploaded
                file.SaveAs(path);

                newDissertation.Attachement = pic;
                newDissertation.StudentId = User.Identity.GetUserId();
                newDissertation.Save();
                // after successfully uploading redirect the user
                return Json("Ավարտական աշխատանքը վերբեռնված է", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("ՁՍԽՈՂՈՒՄ:Ավարտական աշխատանքը վերբեռնված չէ");
            }
        }

        [Authorize(Roles = "Student")]
        public ActionResult Subjects()
        {
            var subjects = Models.Subject.GetSubject(null);
            ViewBag.Areas = TopicArea.GetTopicArea(null);
            return View(subjects);
        }

        [Authorize(Roles = "Student")]
        public ActionResult Attachement(int lessonId)
        {
            return PartialView(lessonId);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public ActionResult AddSubject(int subject,List<int> categoryId,string topicName,string description)
        {
            InviteModel newInvite = new InviteModel();
            newInvite.TeacherId = null;
            newInvite.StudentId = User.Identity.GetUserId();
            newInvite.Type = 1;
            newInvite.SubjectId = Convert.ToInt32(subject);
           
            int token = newInvite.Save();
            if (token == -55)
            {
                throw new HttpException(404, "Some description");
            }
            else
            {
                ResearchTopics topic = new ResearchTopics();
                topic.Topic = topicName;
                topic.SubjectId = subject;
                topic.ShortDescription = description;
                topic.UserId = User.Identity.GetUserId();
                topic.ResearchIds = categoryId;
                topic.Save();
                foreach (int id in categoryId)
                {
                    MailSender(id,subject);
                }
            }
            return null;
        }

        public void MailSender(int categoryId,int subject)
        {
            var users = UserProfile.GetUserByInterest(categoryId);
            var currentUser = new UserProfile(User.Identity.GetUserId());
            var callbackUrl = Url.Action("SendRequestTeacher", "Manage", new { tId = User.Identity.GetUserId(), subId = subject }, protocol: Request.Url.Scheme);
            foreach (var user in users)
            {
                SendMailModel.SendMail(user.Email, string.Format("Dear {0} {1}, <br> We have new student matching your account. Student name {2} {3}", currentUser.Name, currentUser.SurName, user.Name, user.SurName), string.Format("SortIt.Interest matching for {0} {1}", currentUser.Name, currentUser.SurName));
            }
        }

        [Authorize(Roles = "Student")]
        public ActionResult Lesson(int id)
        {
            var Lesson = Models.Lesson.GetLesson(id, null).First();
            ViewBag.IsPassed = Models.Progress.GetProgressByLessonId(User.Identity.GetUserId(), id).Passed;
            return PartialView(Lesson);
        }

        [Authorize(Roles = "Student")]
        public ActionResult Tests(int id)
        {
            Progress prog = Models.Progress.GetProgressByLessonId(User.Identity.GetUserId(), id);
            ViewBag.IsPassed = prog.Passed;
            if (prog.Passed)
                return View();
            var question = Question.GetQuestionByLessonId(id);
            List<Question> tests = new List<Question>();
            List<int> randomList = new List<int>();
            Random rnd = new Random();
            while (true)
            {

                int r = rnd.Next(question.Count());
                if (!randomList.Contains(r))
                    randomList.Add(r);
                if (randomList.Count >= 3)
                    break;
            }
            foreach (int item in randomList)
            {
                tests.Add(question.ElementAt(item));
            }
            return View(tests);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public JsonResult AcceptRequest(int id,int status)
        {
            try
            {
                InviteModel.SaveStatusStudent(id, Convert.ToBoolean(status));
                return Json(string.Format("Your request has been {0}", Convert.ToBoolean(status) ? "accepted" : "declined"));
            }
            catch(Exception ex)
            {
                return Json(string.Format("An error occured please refresh the page"));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public JsonResult Tests(Dictionary<string, List<int>> answerDictionary)
        {
            int points = 0;
            foreach (var ans in answerDictionary)
            {
                int questionId = Convert.ToInt32(ans.Key);
                List<int> answers = ans.Value;
                Models.Question question = Question.GetQuestions(questionId).First();
                List<Models.Answer> rightAnswer = new List<Answer>();
                foreach (var item in question.Answers)
                {
                    if (item.IsRight)
                        rightAnswer.Add(item);
                }
                if (rightAnswer.Count == answers.Count)
                {
                    int selectedCount = 0;
                    foreach (int answer in answers)
                    {
                        int index = rightAnswer.FindIndex(item => item.Id == answer);
                        if (index >= 0)
                        {
                            selectedCount++;
                        }
                    }
                    if (selectedCount == rightAnswer.Count)
                    {
                        points++;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
            if (points >= 2)
            {
                Models.Progress progress = new Progress();
                progress.LessonId = Question.GetQuestions(Convert.ToInt32(answerDictionary.First().Key)).First().LessonId;
                progress.StudentId = User.Identity.GetUserId();
                progress.Passed = true;
                progress.Attachement = "";
                progress.Save();
                return Json("/Student/Passed", JsonRequestBehavior.AllowGet);
            }
            else

            {
                int lessonId = Question.GetQuestions(Convert.ToInt32(answerDictionary.First().Key)).First().LessonId;
                return Json("/Student/Failed/" + lessonId.ToString(), JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "Student")]
        public ActionResult Passed()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public ActionResult Failed(int id)
        {
            return View(id);
        }

        [Authorize(Roles = "Student")]
        public ActionResult Final(int lessonId)
        {
            return PartialView(lessonId);
        }
    }
}