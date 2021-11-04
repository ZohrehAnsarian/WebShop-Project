using BLL;
using BLL.SystemTools;
using CyberneticCode.Web.Mvc.Helpers;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.Admin;
using Model.ViewModels.News;
using Model.ViewModels.Person;
using Model.ViewModels.Category;
using Model.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebShop.Filters.ActionFilterAttributes;
using static Model.ApplicationDomainModels.ConstantObjects;
using Model.ViewModels.Product;
using Model.ViewModels.SundryImage;
using Model.ViewModels.FeatureType;
using Model.ViewModels.CategoryField;
using Model.ViewModels.BaseFeatureType;
using System.Configuration;

namespace WebShop.Controllers
{
    [Authorize]
    [RoleBaseAuthorize(SystemRoles.Admin)]
    public class AdminController : BaseController
    {
        private string excelFile;

        // GET: Admin
        public ActionResult Index()
        {
            var UserRoles = TempData["UserRoles"] as IEnumerable<string>;

            return View(new VmAdmin() { CurrentUserRoles = UserRoles });
        }

        [HttpGet]
        [ActionName("lupf")]
        public ActionResult LoadUpdateProfileForm()
        {
            var blPerson = new BLPerson();
            var vmPerson = blPerson.GetPersonByUserId(CurrentUserId);

            vmPerson.OnActionSuccess = "loadAdminPanel";

            return View("UpdateProfile", vmPerson);
        }

        #region Manage categorys and Products
        [HttpGet]
        public ActionResult TestGrid()
        {
            return View("TestGrid", new VmProduct()
            {
                CategoryId = 48

            });
        }

        [HttpGet]
        [ActionName("cpm")]
        public ActionResult CategoryProductDefine(string cIds = "", int mcId = 0)
        {
            return View("CategoryProductManagement", new VmCategoryProductManagement()
            {
                ParentId = -1,
                LoadedCategoryIds = cIds,
                CategoryId = mcId

            });
        }

        [HttpGet]
        [ActionName("hsim")]
        public ActionResult HmeSundryImageManagement()
        {
            return View("HomeSundryImageManagement", new VmSundryImage());
        }

        [HttpGet]
        [ActionName("psim")]
        public ActionResult PackageSundryImageManagement()
        {
            return View("PackageSundryImageManagement", new VmSundryImage());
        }

        [HttpGet]
        [ActionName("afm")]
        public ActionResult AssignFeatureManagement()
        {
            return View("AssignFeatureTypeToCategory", new VmAssignFeatureTypeManagement());
        }

        [HttpGet]
        [ActionName("bfm")]
        public ActionResult BaseFeatureManagement()
        {
            return View("BaseFeatureManagement", new VmBaseFeatureType());
        }
        [HttpGet]
        [ActionName("cfm")]
        public ActionResult CategoryFieldManagement()
        {
            return View("CategoryFieldManagement", new VmCategoryFieldManagement());
        }

        #endregion Manage categorys and Products

        #region Manage News
        [HttpGet]
        [ActionName("mn")]
        public ActionResult NewsDefine()
        {
            return View("NewsManagement", new VmNewsManagement());
        }
        #endregion Manage News

        #region Languages

        [ActionName("delf")]
        public ActionResult DownloadWrittenReports()
        {
            var fileName = Server.MapPath(@"~/Documents/Dictionary/dictionary.xls");

            byte[] finalResult = System.IO.File.ReadAllBytes(fileName);

            return File(finalResult, "application/zip", "dictionary.xls");

        }

        [ActionName("lulf")]
        public ActionResult UploadLanguages()
        {
            return View("UploadLanguages", new VmAdmin());
        }

        [HttpPost]
        [ActionName("ul")]
        public ActionResult UpdateLanguage(VmAdmin model)
        {
            var result = false;
            var message = "Operation has been succeeded";
            try
            {
                if (ModelState.IsValid)
                {
                    model.UploadedFileUrl = UIHelper.UploadFile(model.UploadedDocument, "/Documents/Dictionary/");

                    if (!string.IsNullOrEmpty(model.UploadedFileUrl))
                    {
                        AppSettingsReader appSettingsReader = new AppSettingsReader();

                        if (appSettingsReader.GetValue("IsServer", typeof(string)).ToString() == "true")
                        {
                            excelFile = "dictionary.xls";
                        }
                        else
                        {
                            excelFile = "dictionary.xlsx";

                        }
                        System.IO.File.Copy(Server.MapPath(model.UploadedFileUrl.Split(new char[] { '?' })[0]), Server.MapPath(@"~/Documents/Dictionary/" + excelFile), true);

                        BLDBTools.ImportDataFromExcel(Server.MapPath(@"~/Documents/Dictionary/" + excelFile));

                        System.IO.File.Delete(Server.MapPath(model.UploadedFileUrl.Split(new char[] { '?' })[0]));

                        result = true;
                        LanguageDictionaryList = null;
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\n" + ex.InnerException.Message;

                    if (ex.InnerException.InnerException != null)
                    {
                        message += "\n" + ex.InnerException.InnerException.Message;
                    }

                }
            }
            var jsonData = new
            {
                success = result,
                uploadedFileUrl = model.UploadedFileUrl,
                message,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        // GET: Journal
        [ActionName("galddl")]
        public ActionResult GetActiveLanguagesDropDownList()
        {

            var activeLanguageList = (from al in ActiveLanguageList
                                      select new VmSelectListItem
                                      {
                                          Value = al.Id.ToString(),
                                          Text = al.Name,
                                      });

            return Json(activeLanguageList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpGet]
        [ActionName("ruem")]
        public ActionResult RoleBaseUserEmailManagement()
        {
            return View("RoleBaseUserEmailManagement", new VmRoleBaseUserEmailManagement());
        }

        [HttpPost]
        [ActionName("se")]

        public ActionResult SendEmail(VmEmail email)
        {
            var result = true;
            var message = "Operation succeeded";

            if (email.AdditionalEmails != null)
            {
                if (email.AdditionalEmails.Length == 1 && email.AdditionalEmails[0] == "")
                {
                    email.AdditionalEmails = null;
                }
            }

            List<string> allEmails = new List<string>();
            if (email.UserIds != null && email.UserIds.Length > 0)
            {
                BLPerson blPerson = new BLPerson();
                var emails = blPerson.GetEmailsByUserIds(email.UserIds);
                allEmails.AddRange(emails);
            }

            if (email.AdditionalEmails != null)
            {
                allEmails.AddRange(email.AdditionalEmails);
            }

            if (allEmails.Count > 0)
            {

                emailHelper = new EmailHelper
                {
                    Subject = email.EmailSubject,
                    Body = email.EmailBody,
                    IsBodyHtml = true,
                    EmailList = allEmails.ToArray()
                };

                result = emailHelper.Send();
            }
            else
            {
                result = false;
                message = "Users not selected";
            }
            var jsonResult = new
            {
                result,
                success = result,
                message,
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
    }
}