using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebShop
{
    /// <summary>
    /// Summary description for ProductAuthenticationWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ProductAuthenticationWebService : System.Web.Services.WebService
    {

        static string path;
        static string authenticatePath;
        static ProductAuthenticationWebService()
        {
            path = HttpContext.Current.Server.MapPath("~/App_Data/URL.txt");
            authenticatePath = HttpContext.Current.Server.MapPath("~/App_Data/authenticate.txt");
        }

        [WebMethod]
        public bool Authenticate(string Url, List<string> ip)
        {
            try
            {
                var streamReader = new StreamReader(authenticatePath);
                var data = streamReader.ReadToEnd().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                //var data = new string[]
                //{
                //    "EEA18F8DBFEBFBFF000206A7",//Mine
                //    "46B57F14BFEBFBFF000306C3",//Z.A
                //    "10519255BFEBFBFF000306C3", //A.N
                //    "localhost",
                //   "a1.sefidshahr.com",
                //   "217.219.157.99",
                //   "aiajournals.com",
                //   "www.aiajournals.com",
                //   "sexycode.com",
                //   "www.sexycode.com",
                //   "aimijournal.com",
                //   "www.aimijournal.com",
                //   "ncmconferences.com",
                //   "www.ncmconferences.com",
                //   "aimiconferences.com",
                //   "emsei.com",
                //   "eurokd.com",
                //   "www.eurokd.com",
                //   "aimi.ir"
                //};

                Url = Url.ToLower();
                var ipExists = data.Any(x => ip.Contains(x));

                if (ipExists == true)
                {
                    streamReader.Close();
                    return true;
                }

                streamReader.Close();

                return false;
                try
                {
                    TextWriter tw = new StreamWriter(path, true);
                    tw.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": ");
                    tw.WriteLine(Url);
                    tw.Close();
                }
                catch (Exception ex1)
                {

                    return true;
                }
            }
            catch (Exception ex)
            {
                return true;

            }
            return true;
        }
    }
}
