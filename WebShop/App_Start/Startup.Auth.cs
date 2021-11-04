using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using WebShop.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web;
using System.Net;
using Owin.Security.Providers.LinkedIn;
using Microsoft.Owin.Security.Facebook;

namespace WebShop
{
    public partial class Startup
    {


        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    ClientId: "",
            //    clientSecret: "");

            app.UseTwitterAuthentication(
               consumerKey: "6BMY3zvg2a1JzvBMSmGfUSklL",
               consumerSecret: "lVS9UcT1rUmCQicaKNt7VLHLy7SemGRdbW32iXy5d5ejo4av5v");

            //For Web Publish

            if (app.Properties["host.AppName"].ToString().ToLower().Contains("cyberneticcode"))
            {
                app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                {
                    ClientId = "1081574467436-e9g4ok3k71o28b5rr7rcc93eg1fgtitc.apps.googleusercontent.com",
                    ClientSecret = "710br0dqFdxVl6hHQhGhj7Ge"

                });

                app.UseFacebookAuthentication(new FacebookAuthenticationOptions()
                {
                    AppId = "308358626298826",
                    AppSecret = "f069c1ee42828422e974e05c3a946fe6"
                    //,
                    //BackchannelHttpHandler = new FacebookBackChannelHandler()
                });

                app.UseLinkedInAuthentication(new LinkedInAuthenticationOptions()
                {
                    ClientId = "78ejwftm52cq94",
                    ClientSecret = "v7c08ntVbPEBts8B"

                });

            }
            else
            {

                //For Locall Test
                app.UseFacebookAuthentication(new FacebookAuthenticationOptions()
                {
                    AppId = "116694335660840",
                    AppSecret = "c8430e01d3d455cfc0ecf4b3c770ad04"
                    //,
                    //BackchannelHttpHandler = new FacebookBackChannelHandler()
                });

                app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                {
                    ClientId = "1081574467436-7th3m9npgmos8r2utkm2ohsj8i2rgq06.apps.googleusercontent.com",
                    ClientSecret = "T6R9skGALOuU5RUpjIVEsHhj"

                });

                app.UseLinkedInAuthentication(new LinkedInAuthenticationOptions()
                {
                    ClientId = "7875mxjhv69zai",
                    ClientSecret = "LJpPSuSMOBAcfbqV"
                });

            }

        }
        #region Custom Code
        public class FacebookOauthResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }
        public class FacebookBackChannelHandler : HttpClientHandler
        {
            protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                var result = await base.SendAsync(request, cancellationToken);
                if (!request.RequestUri.AbsolutePath.Contains("access_token"))
                    return result;

                // For the access token we need to now deal with the fact that the response is now in JSON format, not form values. Owin looks for form values.
                var content = await result.Content.ReadAsStringAsync();
                var facebookOauthResponse = JsonConvert.DeserializeObject<FacebookOauthResponse>(content);

                var outgoingQueryString = HttpUtility.ParseQueryString(string.Empty);
                outgoingQueryString.Add(nameof(facebookOauthResponse.access_token), facebookOauthResponse.access_token);
                outgoingQueryString.Add(nameof(facebookOauthResponse.expires_in), facebookOauthResponse.expires_in + string.Empty);
                outgoingQueryString.Add(nameof(facebookOauthResponse.token_type), facebookOauthResponse.token_type);
                var postdata = outgoingQueryString.ToString();

                var modifiedResult = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(postdata)
                };

                return modifiedResult;
            }
        }
        #endregion Custom Code
    }
}