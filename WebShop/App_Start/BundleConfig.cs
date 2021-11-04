using System.Web.Optimization;

namespace WebShop
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Resources/Scripts/jquery").Include(
                        "~/Resources/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/Resources/Scripts/jqueryval").Include(
                        "~/Resources/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/Resources/Scripts/modernizr").Include(
                        "~/Resources/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Resources/Scripts/All-JS").Include(
                "~/Resources/Scripts/popper.min.js",
                "~/Resources/Scripts/bootstrap.min.js",
                "~/Resources/Scripts/idangerous.swiper.min.js",
                "~/Resources/Scripts/global.js",

                //custom scrollbar start
                "~/Resources/Scripts/jquery.mousewheel.js",
                "~/Resources/Scripts/jquery.jscrollpane.min.js",
                //custom scrollbar end

                "~/Resources/Scripts/cyberneticcode.js"

            ));

            bundles.Add(new ScriptBundle("~/Resources/Scripts/ControlsJs").Include(

                  "~/Resources/Controls/fullscreen-Loading-Indicator/js/HoldOn.min.js",
                  "~/Resources/Controls/bootstrap-select-1.13.14/dist/js/bootstrap-select.min.js",
                  "~/Resources/Controls/js/chosen.jquery.min.js",
                  "~/Resources/Controls/datepicker/bootstrap-datepicker.js",
                  "~/Resources/Controls/datepicker/locales/bootstrap-datepicker.*",
                  "~/Resources/Controls/jquery-confirm-v3.3.4/js/jquery-confirm.js",
                  "~/Resources/Scripts/jquery.maskedinput.min.js"

            ));

            bundles.Add(new ScriptBundle("~/Resources/Scripts/jquery-UI").Include(
                   "~/Resources/Scripts/jquery-ui.min.js"
               ));

            bundles.Add(new ScriptBundle("~/Resources/Scripts/jquery-UI").Include(
                 "~/Resources/Scripts/jquery-ui.min.js"
             ));

            bundles.Add(new StyleBundle("~/Resources/Controls/Grid.MVC/Css").Include(
            "~/Resources/Controls/Grid.MVC/Css/Gridmvc.css"));

            bundles.Add(new StyleBundle("~/Resources/CSS/jquery-ui").Include(
            "~/Resources/CSS/jquery-ui.min.css"));

            bundles.Add(new StyleBundle("~/Resources/CSS/AllCss").Include(
                "~/Resources/CSS/idangerous.swiper.css",
                "~/Resources/CSS/font-awesome.min.css",
                "~/Resources/CSS/main-style.css",
                "~/Resources/CSS/fonts.css",
                "~/Resources/CSS/persian-fonts.css"

            ));

            bundles.Add(new StyleBundle("~/Resources/CSS/ControlsCss").Include(

               "~/Resources/Controls/fullscreen-Loading-Indicator/css/HoldOn.min.css",
               "~/Resources/Controls/css/chosen.min.css",
               "~/Resources/Controls/jquery-confirm-v3.3.4/dist/jquery-confirm.min.css",
               "~/Resources/Controls/bootstrap-select-1.13.14/dist/css/bootstrap-select.min.css"

           ));

            #region masonry
           // bundles.Add(new ScriptBundle("~/Resources/Controls/MDBFree4192/js").Include(


           //       "~/Resources/Controls/MDBFree4192/js/mdb.js",
           //       "~/Resources/Controls/MDBFree4192/js/addons/masonry.pkgd.min.js",
           //       "~/Resources/Controls/MDBFree4192/js/addons/imagesloaded.pkgd.min.js"


           // ));

           // bundles.Add(new StyleBundle("~/Resources/Controls/MDBFree4192/css").Include(

           //    "~/Resources/Controls/MDBFree4192/css/mdb.min.css"

           //));
            #endregion masonry

        }
    }
}

