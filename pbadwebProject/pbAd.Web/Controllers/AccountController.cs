#region Usings

using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbAd.Core.Utilities;
using pbAd.Data;
using pbAd.Service.CacheManagerServices;
using pbAd.Service.Advertisers;
using pbAd.Web.Infrastructure.Framework;
using pbAd.Web.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using pbAd.Service.Users;
using pbAd.Data.Models;
using pbAd.Data.DomainModels.Accounts;
using pbAd.Web.ViewModels.Advertisers;
using pbAd.Service.EmailSenders;
using Microsoft.Extensions.Configuration;

#endregion

namespace pbAd.Web.Controllers
{
    public class AccountController : Controller
    {
        #region Private Members

        private readonly IAdvertiserService consumerService;
        private readonly IMapper mapper;
        private readonly IWorkContext workContext;
        private readonly ICacheManagerService cacheManagerService;
        private readonly IUserService userService;
        private readonly IEmailSenderService emailSenderService;
        private readonly IConfiguration configuration;
        #endregion

        #region Ctor
        public AccountController(
           IAdvertiserService consumerService,
           IWorkContext workContext,
           ICacheManagerService cacheManagerService,
           IUserService userService,
           IEmailSenderService emailSenderService,
           IConfiguration configuration,
           IMapper mapper
           )
        {
            this.consumerService = consumerService;
            this.mapper = mapper;
            this.workContext = workContext;
            this.cacheManagerService = cacheManagerService;
            this.userService = userService;
            this.emailSenderService = emailSenderService;
            this.configuration = configuration;
        }
        #endregion

        #region Login

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = "")
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated) return RedirectToAction("/");

            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "Invalid User Name or Password.", type = ActionResultTypeConstants.Message });

            try
            {
                if (model.Username.Length == 4)
                {
                    var user = await userService.AuthenticateUserB(model.Username, model.Password);

                    if (user == null)
                        return Json(new { status = false, message = "Invalid User Name or Password.", type = ActionResultTypeConstants.Message });

                    //let's sign in
                    await SignInAsync(user, model.RememberMe);

                    model.ReturnUrl = "/dashboard";
                    return Json(new { status = true, message = model.ReturnUrl, type = ActionResultTypeConstants.Url });
                }
                else
                {
                    var user = await userService.AuthenticateUser(model.Username, model.Password);

                    if (user == null)
                        return Json(new { status = false, message = "Invalid User Name or Password.", type = ActionResultTypeConstants.Message });

                    //let's sign in
                    await SignInAsync(user, model.RememberMe);

                    model.ReturnUrl = "/dashboard";
                    return Json(new { status = true, message = model.ReturnUrl, type = ActionResultTypeConstants.Url });

                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = model.ReturnUrl, type = ActionResultTypeConstants.Message });
            }
        }

        #endregion

        #region Logout

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            //let's logout
            LogoutOfThisUser();

            return Redirect("/");
        }

        #endregion

        #region Signup

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Signup(string returnUrl)
        {
            var model = new AccountRegisterViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(AccountRegisterViewModel model)
        {
            /*
            if (!model.AcceptTerms)
                return Json(new { status = false, message = "Warning!, You must accept terms and privacy", type = ActionResultTypeConstants.Message });
            */
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "Warning!, You must fill all required fields.", type = ActionResultTypeConstants.Message });

            if (string.IsNullOrWhiteSpace(model.UserName))
                return Json(new { status = false, message = "Username is Required", type = ActionResultTypeConstants.Message });

            var user = new User
            {
                UserName = model.UserName,
                FullName = model.FullName,
                Email = model.Email,
                CreatedDate = DateTime.Now,
                UserPassword = model.Password,
            };

            var getExistingUser = await userService.FindByUsername(model.UserName);
            if (getExistingUser != null)
            {
                string errorMessage = "User already exist, please try a different Username";
                return Json(new { status = false, message = errorMessage, type = ActionResultTypeConstants.Message });
            }

            var newUser = await userService.Add(user);

            if (newUser == null)
            {
                string errorMessage = "Error registering your account. Please try again.";
                return Json(new { status = false, message = errorMessage, type = ActionResultTypeConstants.Message });
            }

            bool rememberMe = true;
            //let's sign in
            await SignInAsync(newUser, rememberMe);

            //send short message to new user phone
            //send an email notification to a new user email            

            if (string.IsNullOrEmpty(model.ReturnUrl))
                return Json(new { status = true, message = "/", type = ActionResultTypeConstants.Url });

            if (Url.IsLocalUrl(model.ReturnUrl))
                return Json(new { status = true, message = model.ReturnUrl, type = ActionResultTypeConstants.Url });

            return Json(new { status = true, message = "/", type = ActionResultTypeConstants.Url });
            //return Json(new { status = true, message = "/account/myprofile", type = ActionResultTypeConstants.Url });
        }

        #endregion

        #region Otp

        [Route("/otprequest")]
        public ActionResult OTPRequest()
        {
            var model = new OTPRequestModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/otprequest")]
        public async Task<ActionResult> OTPRequest(OTPRequestModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "Warning!, You must fill all required fields.", type = ActionResultTypeConstants.Message });

            //find by username and email
            var user = await userService.FindByUsernameAndEmail(model.Username, model.Email);

            if (user == null)
                return Json(new { status = false, message = "Warning! Invalid Username or email. Please try again!", type = ActionResultTypeConstants.Message });

            var otpSendTime = DateTime.Now;

            var otpChangeRequest = new OTPChangeRequest
            {
                UserId = user.UserId,
                OTP = CoreHelper.RandomCodeGenerateNumber(6),
                OTPSentTime = otpSendTime,
                OTPExpireTime = otpSendTime.AddHours(4)
            };

            //let's update User's otp
            var response = await userService.UpdateOTP(otpChangeRequest);

            if (!response.IsChanged)
                return Json(new { status = false, message = response.Confirmation, type = ActionResultTypeConstants.Message });

            var emailSenderModel = new EmailSenderModel
            {
                ToEmail = model.Email,
                OTP = otpChangeRequest.OTP,
                Subject = "পাসওয়ার্ড পরিবর্তনের কোড",
                Body = $@"আপনার কোড টি হলো : {otpChangeRequest.OTP} {System.Environment.NewLine} {System.Environment.NewLine} {System.Environment.NewLine} N.B: This Email is a system generated mail. Do not reply.",
                SMTPMailServer = configuration["EmailSettings:SMTPMailServer"],
                SMTPMailServerPort = Convert.ToInt32(configuration["EmailSettings:SMTPMailServerPort"]),
                FromEmail = configuration["EmailSettings:FromEmail"],
                FromEmailName = configuration["EmailSettings:FromEmailName"],
                MailNetworkUsername = configuration["EmailSettings:MailNetworkUsername"],
                MailNetworkPassword = configuration["EmailSettings:MailNetworkPassword"],
            };

            //let's send otp to this user by email            
            await emailSenderService.SendEmail(emailSenderModel);

            return Json(new { status = true, message = $"/account/username/{model.Username}/ChangePassword/{model.Email}", type = ActionResultTypeConstants.Url });
        }

        #endregion

        #region Change Password

        [Route("/account/username/{username}/changepassword/{email}")]
        public async Task<ActionResult> ChangePassword(string username, string email)
        {            
            var model = new ChangePasswordModel { Username = username, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/account/username/{username}/changepassword/{email}")]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "Warning!, You must fill all required fields.", type = ActionResultTypeConstants.Message });

            //find by username and email
            var user = await userService.FindByUsernameAndEmail(model.Username, model.Email , model.OTP);

            if (user == null)
                return Json(new { status = false, message = "Warning! Invalid OTP. Please try again!", type = ActionResultTypeConstants.Message });

            double otpExpireTime = ((DateTime)(user.OTPExpireTime)- DateTime.Now).TotalHours;

            if (otpExpireTime>4)
                return Json(new { status = false, message = "Warning! OTP Expired. Please try again!", type = ActionResultTypeConstants.Message });

            var passChangeRequest = new PasswordChangeRequest
            {
                UserId = user.UserId,
                NewPassword = model.NewPassword,                
                ValidateOldPassword = false
            };

            //let's Update User Password
            var response = await userService.UpdateUserPassword(passChangeRequest);

            if (!response.IsChanged)
                return Json(new { status = false, message = response.Confirmation, type = ActionResultTypeConstants.Message });

            //let's logout this user if logged in            
            LogoutOfThisUser();

            //let's  go to login page
            return Json(new { status = true, message = "/", type = ActionResultTypeConstants.Url });
        }

        #endregion 

        #region User Access Denied
        [HttpGet]
        [AllowAnonymous]
        public IActionResult UserAccessDenied()
        {
            return View();
        }

        #endregion
                
        #region Error
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }

        #endregion

        #region Ajax Calls



        #endregion

        #region Private Methods
        private void LogoutOfThisUser()
        {
            try
            {
                HttpContext.SignOutAsync();

                //let's remove consumer logged in user cache info
                RemoveCurrentLoggedInUserCacheInfo();
            }
            catch
            {

            }
        }
        private void RemoveCurrentLoggedInUserCacheInfo()
        {
            //get consumer id
            var consumerId = GetAdvertiserId();
            if (consumerId == null || consumerId <= 0) return;

            //let's remove consumer logged in user cache info
            cacheManagerService.RemoveCurrentLoggedInUserCacheInfo(Convert.ToInt32(consumerId));
        }

        private int? GetAdvertiserId()
        {
            int? consumerId = null;

            //get claims from user identity
            var consumerClaims = User.Claims;
            if (consumerClaims == null) return null;

            //get consumer id
            var consumerIdClaim = consumerClaims.FirstOrDefault(c => c.Type == "AdvertiserId");
            if (consumerIdClaim == null) return null;

            consumerId = Convert.ToInt32(consumerIdClaim.Value);

            return consumerId;
        }

        private async Task SignInAsync(User user, bool rememberMe = true)
        {
            //lets signout first
            await HttpContext.SignOutAsync();

            var userClaims = new List<Claim>()
            {
                new Claim("UserId",user.UserId.ToString()),new Claim("RoleId",user.RoleId.ToString()),
                new Claim("MobileNo",string.IsNullOrWhiteSpace(user.MobileNo)?"-":user.MobileNo),
                new Claim("Email",string.IsNullOrWhiteSpace(user.Email)?"-":user.Email),
                new Claim("Designation",string.IsNullOrWhiteSpace(user.Designation)?"-":user.Designation),
                new Claim("EditionId",user.EditionId==null?"0":user.EditionId.ToString()),
                new Claim("DistrictId",user.DistrictId.ToString()),
                new Claim("UpazillaId",user.UpazillaId.ToString()),
                new Claim("GroupId",user.GroupId==null?"0":user.GroupId.ToString()),
                new Claim("DefaultCommission",user.DefaultCommission.ToString()),
                new Claim("UserName",user.UserName),
                new Claim("FullName",user.FullName)
            };

            var userIdentity = new ClaimsIdentity(userClaims, "User Identity");
            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });

            //let's signin
            await HttpContext.SignInAsync(
                userPrincipal,
                new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                    ExpiresUtc = DateTime.UtcNow.AddHours(1)
                });

            //let's set current logged in user
            cacheManagerService.SetCurrentLoggedInUser(user);
        }

        #endregion
    }
}
