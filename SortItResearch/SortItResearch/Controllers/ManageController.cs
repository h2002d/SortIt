using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SortItResearch.Models;
using System.Collections.Generic;

namespace SortItResearch.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion

        #region CustomActions
        [HttpPost]
        public ActionResult SaveSkills(List<int> moduleIds)
        {
            TeacherSkillViewModel skill = new TeacherSkillViewModel();
            skill.Save(User.Identity.GetUserId(), moduleIds);
            return null;
        }

        public ActionResult TeacherSkills()
        {
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
            ViewBag.SelectedSkills = TeacherSkillViewModel.GetSelectedSkills(User.Identity.GetUserId());
            return PartialView();
        }

        public ActionResult ProfileInfo(string userId)
        {
            if (String.IsNullOrEmpty(userId))
                userId = User.Identity.GetUserId();
            UserProfile user = new UserProfile();
            user.Id = userId;
            return View(user);
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult ConfirmTeacher(string userId)
        {
            var user = UserManager.FindById(userId);
            UserManager.RemoveFromRole(user.Id, "Student");
            UserManager.AddToRole(user.Id, "Teacher");
            var info = new UserProfile(user.Id);
            return View(info);
        }
        [HttpPost]
        public ActionResult ProfileInfo(UserProfile user)
        {
            if (ModelState.IsValid)
            {
                user.Save();
                if (user.isTeacher)
                {

                    var callbackUrl = Url.Action("ConfirmTeacher", "Manage", new { userId = user.Id}, protocol: Request.Url.Scheme);
                    string email= System.Web.Configuration.WebConfigurationManager.AppSettings["AdminEmail"];
                    SendMailModel.SendMail(email, "User "+user.Name+" "+user.SurName+" wants to register as facilitator click <a href='" + callbackUrl + "'>here</a>"+
                        " to confirm <br/>Dissertation link <a href='"+user.Dissertation+"'>"+user.Dissertation+"</a>", "SortIt. Facilitator request!");

                }
                if (User.IsInRole("Student"))
                    return RedirectToAction("MySubjects", "Student");
                else
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public ActionResult SendRequest(string tId, string subId)
        {
            //Notif part via email
            //collect the link and send it via email
            //save request row in db without approved generate token
            InviteModel newInvite = new InviteModel();
            newInvite.TeacherId = tId;
            newInvite.StudentId = User.Identity.GetUserId();
            newInvite.Type = 1;
            newInvite.SubjectId = Convert.ToInt32(subId);
            int token = newInvite.Save();
            if (token == -55)
            {
                throw new HttpException(404, "Some description");
            }
            string link = string.Format("<p style=\"color:red\">Դուք ունեք նոր դիմում</p><a href=\"{0}/Manage/Requests?t={1}\">{0}/Manage/Requests?t={1}</a>", Request.Url.Authority, token.ToString());
            SendMailModel.SendMail(UserManager.FindByIdAsync(tId).Result.Email, link, "SortIt. Դուք ունեք նոր դիմում");
            return null;
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult Requests(int t)//t=token
        {
            //show view where you need to accept or decline
            var invite = InviteModel.GetInvite(t);
            if (!invite.TeacherId.Equals(User.Identity.GetUserId()) || invite.Accepted)
                return RedirectToAction("Index", "Home");
            ViewBag.Inviter = new UserProfile(invite.StudentId);
            return View(invite);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public ActionResult Requests(int t, bool accepted)//t=token
        {
            InviteModel.SaveStatus(t, accepted, User.Identity.GetUserId());
            return null;
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult ChangeRole(string id, string roleId)
        {
            var user = UserManager.FindById(id);
            UserManager.RemoveFromRole(user.Id, "Student");
            UserManager.AddToRole(user.Id, roleId);
            return null;
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult FindStudent(string studentMail)
        {
            if (String.IsNullOrEmpty(studentMail))
                return null;
            var user = UserManager.FindByEmail(studentMail);
            if (user == null)
                return null;
            if (!user.Roles.SingleOrDefault().RoleId.Equals("88"))
                return null;
            var student = new UserProfile(user.Id);
            if (student == null)
                return null;
            return PartialView(student);
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult FindTeacher(string studentMail)
        {
            if (String.IsNullOrEmpty(studentMail))
                return null;
            var user = UserManager.FindByEmail(studentMail);
            if (user == null)
                return null;
            if (!user.Roles.SingleOrDefault().RoleId.Equals("59"))
                return null;
            var student = new UserProfile(user.Id);
            if (student == null)
                return null;
            return PartialView(student);
        }
        [Authorize]
        public ActionResult Certificates()
        {
            var certificates = Certificate.GetCertificateByStudentId(User.Identity.GetUserId());
            return PartialView(certificates);
        }
        #endregion

        #region Profile edit
        public ActionResult ProfileDetails()
        {
            UserProfile user = new UserProfile(User.Identity.GetUserId());
            return PartialView(user);
        }
        [HttpPost]
        public ActionResult ProfileDetails(UserProfile user)
        {
            user.Save();
            return Json("Your info has been chanhed!",JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}