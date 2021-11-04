using BLL;
using WebShop.Models;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Threading.Tasks;

namespace WebShop.Controllers
{
    public class ApiController : System.Web.Http.ApiController
    {

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // ApplicationDbContext context = new ApplicationDbContext();

        #region Register by phone number

        // POST: api/Geography
        [HttpPost]
        [Route("api/SendPhoneNumber")]
        public Task<bool> SendPhoneNumber([FromBody] string phoneNumber)
        {
            return RegisterByPhoneNumber(phoneNumber);
        }

        // POST: api/Geography
        [HttpPost]
        [Route("api/SendConfirmAuthenticationCode")]
        public string SendConfirmAuthenticationCode(AMSMSAuthentication amSMSAuthentication)
        {
            string userId = string.Empty;
            try
            {
                var user = UserManager.Users.SingleOrDefault(u => u.PhoneNumber == amSMSAuthentication.PhoneNumber);

                if (user != null && user.AuthenticationCode == amSMSAuthentication.AuthenticationCode)
                {
                    user.PhoneNumberConfirmed = true;
                    userId = user.Id;

                    // UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());

                    UserManager.RemovePassword(userId);

                    var password = BLHelper.GeneratePassword(6);

                    UserManager.AddPassword(userId, password);

                    var smsResult = BLHelper.SendSMS(
                            amSMSAuthentication.PhoneNumber,
                            "به WebShop خوش آمدید" + "\n" +
                            " جهت ورود به حساب کاربریتان در سایت WebShop.com و استفاده از سرویس های ارایه شده از اطلاعات زیر استفاده نمایید : " + "\n" +
                            "نام کاربری : " + amSMSAuthentication.PhoneNumber + "\n" +
                            "رمز عبور : " + password + "\n" + ""
                        );

                    if (smsResult == 10) //Delivered
                    {
                        //1   در صف ارسال قرار دارد.
                        //2   زمان بندی شده(ارسال در تاریخ معین).
                        //4   ارسال شده به مخابرات.
                        //5   ارسال شده به مخابرات(همانند وضعیت 4)
                        //6   خطا در ارسال پیام که توسط سر شماره پیش می آید و به معنی عدم رسیدن پیامک می باشد(Failed)
                        //10  رسیده به گیرنده(Delivered)
                        //11  نرسیده به گیرنده ،این وضعیت به دلایلی از جمله خاموش یا خارج از دسترس بودن گیرنده اتفاق می افتد(Undelivered)
                        //13  ارسال پیام از سمت کاربر لغو شده یا در ارسال آن مشکلی پیش آمده که هزینه آن به حساب برگشت داده میشود.
                        //14  بلاک شده است،عدم تمایل گیرنده به دریافت پیامک از خطوط تبلیغاتی که هزینه آن به حساب برگشت داده میشود
                        //100 شناسه پیامک نامعتبر است.(به این معنی که شناسه پیام در پایگاه داده کاوه نگار ثبت نشده است یا متعلق به شما نمی باشد)
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return userId;
        }

        [NonAction]
        public async Task<bool> RegisterByPhoneNumber(string phoneNumber)
        {
            try
            {
                var AuthenticationCode = BLHelper.GenerateRandomNumber(1000, 9999).ToString();
                var smsResult = BLHelper.SendSMS(phoneNumber, "کد چهار رقمی احراز هویت :" + "\n" + AuthenticationCode);

                //if (smsResult == 10) //Delivered
                //{
                //    //1   در صف ارسال قرار دارد.
                //    //2   زمان بندی شده(ارسال در تاریخ معین).
                //    //4   ارسال شده به مخابرات.
                //    //5   ارسال شده به مخابرات(همانند وضعیت 4)
                //    //6   خطا در ارسال پیام که توسط سر شماره پیش می آید و به معنی عدم رسیدن پیامک می باشد(Failed)
                //    //10  رسیده به گیرنده(Delivered)
                //    //11  نرسیده به گیرنده ،این وضعیت به دلایلی از جمله خاموش یا خارج از دسترس بودن گیرنده اتفاق می افتد(Undelivered)
                //    //13  ارسال پیام از سمت کاربر لغو شده یا در ارسال آن مشکلی پیش آمده که هزینه آن به حساب برگشت داده میشود.
                //    //14  بلاک شده است،عدم تمایل گیرنده به دریافت پیامک از خطوط تبلیغاتی که هزینه آن به حساب برگشت داده میشود
                //    //100 شناسه پیامک نامعتبر است.(به این معنی که شناسه پیام در پایگاه داده کاوه نگار ثبت نشده است یا متعلق به شما نمی باشد)
                //}

                var user = UserManager.Users.SingleOrDefault(u => u.PhoneNumber == phoneNumber);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = phoneNumber,
                        PhoneNumber = phoneNumber,
                        PhoneNumberConfirmed = false,
                        RegisterDate = DateTime.Now,
                        AuthenticationCode = AuthenticationCode,
                    };

                    var result = await UserManager.CreateAsync(user);

                    if (result.Succeeded)
                    {
                        UserManager.AddToRole(user.Id, "Regular");
                    }
                }
                else
                {
                    user.AuthenticationCode = AuthenticationCode;
                    UserManager.Update(user);
                }

                return true;
            }
            catch  
            {
                return false;
            }
        }


        #endregion  Register by phone number
      
        // GET: api/Geography
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Geography/5
        public string Get(int id)
        {
            return "value";
        }

        // PUT: api/Geography/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Geography/5
        public void Delete(int id)
        {
        }
    }
}
