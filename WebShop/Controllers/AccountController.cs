using BLL;
using CyberneticCode.Web.Mvc.Helpers;
using WebShop.Filters.ActionFilterAttributes;
using WebShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Model.ApplicationDomainModels;
using Model.Base;
using Model.ViewModels.Person;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Model.ApplicationDomainModels.ConstantObjects;
using Newtonsoft.Json;
using Model.ViewModels;
using System.Web.Configuration;
using System.Net;

namespace WebShop.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        ApplicationDbContext context = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(new LoginViewModel()
                {
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("login");
            }

        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl, Guid? tempCartId)
        {
            try
            {
                ViewBag.ReturnUrl = returnUrl;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true

                IEnumerable<string> userRoles = null;
                var UserName = model.UserName;

                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(model.UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                if (user != null)
                {
                    if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                    {
                        ModelState.AddModelError("", "You need to confirm your email.");
                        return View(model);
                    }
                    //if (await UserManager.IsLockedOutAsync(user.Id))
                    //{
                    //    return View("Lockout");
                    //}

                    if (tempCartId != null)
                    {
                        var blInvoice = new BLInvoice(CurrentLanguageId);
                        blInvoice.UpdateUserId(tempCartId.Value, user.Id);
                    }

                    UserName = user.UserName;

                    if (SmUserRolesList.UserRoles == null)
                    {
                        var blUser = new BLUser();
                        SmUserRolesList.UserRoles = blUser.GetAllUserRoles();
                    }

                    userRoles = (from roles in SmUserRolesList.UserRoles where roles.UserName == UserName select roles.RoleName).AsEnumerable<string>();
                    TempData["UserRoles"] = userRoles;
                }

                var result = await SignInManager.PasswordSignInAsync(UserName, model.Password, model.RememberMe, shouldLockout: false);

                switch (result)
                {
                    case SignInStatus.Success:

                        CurrentUserId = user.Id;

                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            if (userRoles.Contains(SystemRoles.Admin.ToString()))
                            {
                                ViewBag.UserRole = "Admin";
                                return RedirectToAction("index", "admin");
                            }

                            ViewBag.UserRole = "Member";
                        }

                        return RedirectToLocal(returnUrl);


                    //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe, SetWelcomMessage = true });

                    case SignInStatus.LockedOut:
                        return View("Lockout");

                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });

                    case SignInStatus.Failure:

                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("login");
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error", new VMHandleErrorInfo());
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string returnUrl = "")
        {
            var UserRoles = TempData["UserRoles"] as IEnumerable<string>;

            return View(new RegisterViewModel() { CurrentUserRoles = UserRoles, ReturnUrl = returnUrl });
        }

        [ActionName("acu")]
        [RoleBaseAuthorize(SystemRoles.Admin)]
        public ActionResult AdminCreateUser(string role = "")
        {
            var roleList = context.Roles.Where(r => r.Id != "652a69dc-d46c-4cbf-ba28-8e7759b37752" && r.Id == "58c326dd-38ea-4d3c-92f9-3935e3763e68").OrderBy(r => r.Name).ToList()
                .Select(r => new SelectListItem { Value = r.Name.ToString(), Text = r.Name }).ToList();

            var roleName = "";
            if (role == "eic")
            {
                roleName = "Editor In Chief";
                roleList.Where(r => r.Value == roleName).First().Selected = true;
            }

            ViewBag.Roles = roleList;

            return View("AdminCreateUser", new RegisterViewModel()
            {
                RoleName = roleName,
                ReturnUrl = HttpContext.Request.RawUrl,
                AllowAcceptReject = true,
            });
        }


        /// <summary>  
        /// Validate Captcha  
        /// </summary>  
        /// <param name="response"></param>  
        /// <returns></returns>  
        public CaptchaResponse ValidateCaptcha(string response)
        {
            string secret = WebConfigurationManager.AppSettings["recaptchaPrivateKey"];
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            return JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult.ToString());
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string byAdmin = "")
        {
            var blSiteInfo = new BLPerson();

            CaptchaResponse response = ValidateCaptcha(model.GoogleRecaptchaResponse);
            if (response.Success == false && byAdmin == "")
            {
                blSiteInfo.CreateSiteInfo("Error From Google ReCaptcha");

                return Content("Reg: Error From Google ReCaptcha : " + response.ErrorMessage[0].ToString());
            }


            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    RegisterDate = DateTime.UtcNow,
                    LastSignIn = DateTime.UtcNow,
                    AllowAcceptReject = model.AllowAcceptReject,

                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link

                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code, returnUrl = model.ReturnUrl }, protocol: Request.Url.Scheme);

                    var domainName = callbackUrl.Split('/')[2];


                    if (string.IsNullOrEmpty(model.RoleName))
                    {
                        model.RoleName = "Member";
                    }

                    UserManager.AddToRole(user.Id, model.RoleName);


                    var blUser = new BLUser();
                    SmUserRolesList.UserRoles = blUser.GetAllUserRoles();

                    var blPerson = new BLPerson();
                    blPerson.CreatePerson(new VmPerson { UserId = user.Id });

                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "<h1>" + domainName +
                       "</h1><br/><h2>Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</h2></a>" +

                       "<br/><br/><span>Or copy link below and paste in the browser: </span><br/>" + callbackUrl +
                       "<br/><br/><span>E-mail as User name: </span>" + user.UserName +
                       "<br/><span>Password: </span>" + model.Password
                       );

                    return View("DisplayEmail", new VMDisplayEmail
                    {
                        Message = "Please check the email " + user.Email + " and confirm email address.",
                        RoleName = model.RoleName
                    });
                }

                AddErrors(result);
            }
            //string userName = HttpContext.User.Identity.Name;

            //if (HttpContext.User.IsInRole(SystemRoles.Admin.ToString()))
            //{
            //    var roleList = context.Roles.Where(r => r.Id != "652a69dc-d46c-4cbf-ba28-8e7759b37752").OrderBy(r => r.Name).ToList().Select(r => new SelectListItem { Value = r.Name.ToString(), Text = r.Name }).ToList();
            //    ViewBag.Roles = roleList;
            //    return View("AdminCreateUser", model);

            //}

            // If we got this far, something failed, redisplay form

            if (!string.IsNullOrEmpty(model.ReturnUrl) && model.RoleName != "Member")
            {
                return RedirectToLocal(model.ReturnUrl);
            }

            TempData["LastModelStateErrors"] = null;

            return View(model);

        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = model.UserName,
        //            Email = model.Email,
        //            RegisterDate = DateTime.UtcNow
        //        };

        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //            var callbackUrl = Url.Action(
        //               "ConfirmEmail", "Account",
        //               new { userId = user.Id, code = code },
        //               protocol: Request.Url.Scheme);

        //            await UserManager.SendEmailAsync(user.Id,
        //               "Confirm your account",
        //               "Please confirm your account by clicking this link: <a href=\""
        //                                               + callbackUrl + "\">link</a>");
        //            // ViewBag.Link = callbackUrl;   // Used only for initial demo.
        //            return View("DisplayEmail");
        //        }
        //        AddErrors(result);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code, string returnUrl = "")
        {

            if (userId == null || code == null)
            {
                return View("Error", new VMHandleErrorInfo("Email Confirmation not valid"));
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {

                var user = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
                await SignInManager.SignInAsync(user, false, false);

                var UserName = user.UserName;

                var blUser = new BLUser();
                SmUserRolesList.UserRoles = blUser.GetAllUserRoles();

                var userRoles = (from roles in SmUserRolesList.UserRoles where roles.UserName == UserName select roles.RoleName).AsEnumerable<string>();
                TempData["UserRoles"] = userRoles;

                if (returnUrl != "")
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    CurrentUserId = user.Id;
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        if (userRoles.Contains(SystemRoles.Admin.ToString()))
                        {
                            return RedirectToAction("index", "admin");
                        }

                        ViewBag.UserRole = "Member";

                        return RedirectToAction("index", "user");

                    }
                }

            }

            if (result.Errors.First().ToLower().Contains("invalid token"))
            {
                code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);

                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userId, code }, protocol: Request.Url.Scheme);

                var domainName = callbackUrl.Split('/')[2];

                await UserManager.SendEmailAsync(userId, "Confirm your account", "<h1>" + domainName +
                    "</h1><br/><h2>Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</h2></a>" +

                    "<br/><br/><span>Or copy link below and paste in the browser: </span><br/>" + callbackUrl
                    );

                return View("Error", new
                    VMHandleErrorInfo("Confirmation email link has been expired for security reasons. \n New Confirmation email has sent to your email."));
            }

            return View("Error", new VMHandleErrorInfo(result.Errors.First()));

        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(model.UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();


                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPassword", new ForgotPasswordViewModel("the E-mail " + model.UserName + " not found in WebShop..."));
                }

                if (!(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    return View("ForgotPassword", new ForgotPasswordViewModel("the email " + user.Email + " not confirmed in WebShop..."));
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new
                {
                    userId = user.Id,
                    code
                }, protocol: Request.Url.Scheme);

                await UserManager.SendEmailAsync(user.Id, "Reset Password", "<h2>Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</h2></a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View(new ForgotPasswordConfirmationViewModel());
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View(new ResetPasswordViewModel());
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //ApplicationUser user = context.Users.Where(u => u.UserName.Equals(model.UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user != null)
            {

                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, false, false);

                    return RedirectToAction("Index", "Home");
                    //  return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                }
            }
            else
            {
                AddErrors(new IdentityResult(new string[] { "User not found...!" }));
            }


            return View(new ResetPasswordViewModel());

        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View(new BaseViewModel());
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider

            try
            {
                Session["Workaround"] = 0;
                return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message + " \n " + (ex.InnerException) ?? "";
                //return View("Error", new VMHandleErrorInfo());
                return RedirectToAction("login");
            }
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                //return View("Error", new VMHandleErrorInfo());
                return RedirectToAction("login");
            }

            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error", new VMHandleErrorInfo());

            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return View("Error", new VMHandleErrorInfo("Problem in Social Signin"));
                //return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home", new { SetWelcomMessage = true });

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email, MostSetWelcomeMessage = true });
            }
        }

        ////
        //// GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{

        //    var result = await AuthenticationManager.AuthenticateAsync(DefaultAuthenticationTypes.ExternalCookie);
        //    if (result == null || result.Identity == null)//here will check if user login done
        //    {
        //        return View("Error", new VMHandleErrorInfo("if (result == null || result.Identity == null)"));
        //        //return RedirectToAction("Login");
        //    }

        //    var idClaim = result.Identity.FindFirst(ClaimTypes.NameIdentifier);
        //    if (idClaim == null)
        //    {
        //        return View("Error", new VMHandleErrorInfo("idClaim == null"));
        //        //return RedirectToAction("Login");
        //    }

        //    var login = new UserLoginInfo(idClaim.Issuer, idClaim.Value);//here getting login info
        //    var name = result.Identity.Name == null ? "" : result.Identity.Name.Replace(" ", "");//here getting E-mail

        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return View("Error", new VMHandleErrorInfo("loginInfo == null"));
        //        //return RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var result1 = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //    switch (result1)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //        case SignInStatus.Failure:
        //        default:
        //            // If the user does not have an account, then prompt the user to create an account
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //    }
        //}

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure", new VMHandleErrorInfo());
                }
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    RegisterDate = DateTime.UtcNow.Date,
                    EmailConfirmed = true
                };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Member");
                    var blPerson = new BLPerson();

                    blPerson.CreatePerson(new VmPerson { UserId = user.Id });

                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpGet]
        [ActionName("lfa")]
        [ValidateAntiForgeryToken]
        public ActionResult LogOffFromAdmin()
        {
            try
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Session["WelcomeMessage"] = null;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("login");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Session["WelcomeMessage"] = null;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("login");
            }
        }
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View(new VMHandleErrorInfo());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
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
            TempData["LastModelStateErrors"] = result.Errors;

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {

                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);

            }
        }
        #endregion

        #region Custom Actoins
        [ChildActionOnly]
        public PartialViewResult Get_ExternalLoginsListPartial(string returnUrl)
        {
            return PartialView("~/Views/Account/_ExternalLoginsListPartial", new ExternalLoginListViewModel() { ReturnUrl = returnUrl });
        }

        [ActionName("gassp")]
        [HttpPost]
        public JsonResult GenerateAndSendSimplePassword(string id)
        {
            ApplicationUser user = context.Users.Find(id);

            var simplePassword = BLHelper.GenerateRandomNumber(102925, 982054).ToString();
            var token = UserManager.GeneratePasswordResetToken(id);
            var result = UserManager.ResetPassword(id, token, simplePassword);
            var email = new EmailHelper
            {
                IsBodyHtml = true,
                EmailList = new string[] { user.Email },
                Subject = "Reset Password",
                Body = "<h2>Your Account Info:</h2><hr/>" +
                "E-mail as User name:" +
                "<br/>" + user.UserName +
                "<br/>Password:" +
                "<br/>" +
                simplePassword
            };

            var jsonObject = new
            {
                success = email.Send(),
            };

            return Json(jsonObject, JsonRequestBehavior.AllowGet);
        }
        #endregion Custom Actoins
    }
}