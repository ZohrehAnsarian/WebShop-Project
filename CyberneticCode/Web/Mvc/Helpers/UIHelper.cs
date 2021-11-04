using iTextSharp.text;
using iTextSharp.text.pdf;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace CyberneticCode.Web.Mvc.Helpers
{

    public static class UIHelper
    {
        #region file handeling
        public static string UploadFile(HttpPostedFileBase file, string path)
        {


            var newFilePrefix = "";
            var fileUrl = string.Empty;
            var serverPath = string.Empty;

            var date = DateTime.Now;
            newFilePrefix = date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2") + date.Hour.ToString("D2") + date.Minute.ToString("D2") + date.Second.ToString("D2");
            var fileName = newFilePrefix + file.FileName.Replace("-", "_").Replace("+", "_").Replace("+", "_");
            if (file != null && file.ContentLength > 0)
            {

                serverPath = Path.Combine(HttpContext.Current.Server.MapPath(path), Path.GetFileName(fileName));

                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
                file.SaveAs(serverPath);

                fileUrl = Path.Combine(path, fileName).Replace("\\", "/");

            }
            return fileUrl;
        }
        public static string UploadPictureFile(string base64String, string pictureFileName, string contentType, string path)
        {
            var fileUrl = string.Empty;
            //var date = DateTime.Now;
            //string newFilePrefix = date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2") + date.Hour.ToString("D2") + date.Minute.ToString("D2") + date.Second.ToString("D2");
            var fileName = pictureFileName.Replace("-", "_").Replace("+", "_");
            if (!string.IsNullOrEmpty(base64String))
            {
                System.Drawing.Image image;
                byte[] bytes = Convert.FromBase64String(base64String);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = System.Drawing.Image.FromStream(ms);

                    string serverPath = Path.Combine(HttpContext.Current.Server.MapPath(path), Path.GetFileName(fileName.Split('?')[0]));
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
                    image.Save(serverPath);

                    fileUrl = Path.Combine(path, fileName + "?w=" + image.Size.Width + "&h=" + image.Size.Height).Replace("\\", "/");
                }
            }
            return fileUrl;
        }

        public static void DeleteFile(string path)
        {
            try
            {
                var serverPath = HttpContext.Current.Server.MapPath(path.Split(new char[] { '?' })[0]);

                File.Delete(serverPath);

            }
            catch (Exception ex)
            {

            }
        }
        public static void DeleteFilesByPattern(string path, string pattern)
        {
            try
            {
                var serverPath = HttpContext.Current.Server.MapPath(path);

                DirectoryInfo di = new DirectoryInfo(serverPath);

                foreach (FileInfo file in di.GetFiles(pattern))
                {
                    file.Delete();
                }

            }
            catch (Exception ex)
            {

            }
        }
        public static void DeleteFilesByFolder(string path)
        {
            try
            {
                var serverPath = HttpContext.Current.Server.MapPath(path);

                DirectoryInfo di = new DirectoryInfo(serverPath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

            }
            catch (Exception ex)
            {

            }
        }

        public static System.Drawing.Image ConvertBase64ToImage(string base64String)
        {
            //data:image/gif;base64,
            //this image is a single pixel (black)
            //  byte[] bytes = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw==");
            byte[] bytes = Convert.FromBase64String(base64String);

            System.Drawing.Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = System.Drawing.Image.FromStream(ms);
            }

            return image;
        }
        #endregion file handeling     

        #region PDF file handeling         
        public static void MergePDF(string[] fileArray, string outputPdfPath)
        {

            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;

            sourceDocument = new Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

            //output file Open  
            sourceDocument.Open();


            //files list wise Loop  
            for (int f = 0; f < fileArray.Length; f++)
            {
                int pages = TotalPageCount(fileArray[f]);

                reader = new PdfReader(fileArray[f]);
                //Add pages in new file  
                for (int i = 1; i <= pages; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }

                reader.Close();
            }
            //save the output file  
            sourceDocument.Close();
        }

        private static int TotalPageCount(string file)
        {
            using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }
        #endregion PDF file handeling         

        #region Calculation
        public static double GetAspectRatio(double x, double y)
        {
            if (x > y)
            {
                return x / y;
            }
            else if (y > x)
            {
                return y / x;
            }
            else
                return 1.0;
        }
        #endregion Calculation

        #region Google Services
        public static string TranslateText(string input)
        {
            // Set the language from/to in the url (or pass it into this function)
            string url = String.Format
            ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
             "auto", "en", Uri.EscapeUriString(input));
            HttpClient httpClient = new HttpClient();
            string result = httpClient.GetStringAsync(url).Result;

            // Get all json data
            var jsonData = new JavaScriptSerializer().Deserialize<List<dynamic>>(result);

            // Extract just the first array element (This is the only data we are interested in)
            var translationItems = jsonData[0];

            // Translation Data
            string translation = "";

            // Loop through the collection extracting the translated objects
            foreach (object item in translationItems)
            {
                // Convert the item array to IEnumerable
                IEnumerable translationLineObject = item as IEnumerable;

                // Convert the IEnumerable translationLineObject to a IEnumerator
                IEnumerator translationLineString = translationLineObject.GetEnumerator();

                // Get first object in IEnumerator
                translationLineString.MoveNext();

                // Save its value (translated text)
                translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
            }

            // Remove first blank character
            if (translation.Length > 1)
            { translation = translation.Substring(1); };

            // Return translation
            return translation;
        }

        #endregion Google Services
    }
}