using System.Web;
using System.Web.Optimization;

namespace CMSDiamondStay
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Index/css").Include(
                     "~/App_Themes/CMS/vendors/bootstrap/dist/css/bootstrap.min.css",
                     "~/App_Themes/CMS/vendors/font-awesome/css/font-awesome.min.css",
                    "~/App_Themes/CMS/vendors/nprogress/nprogress.css",
                    "~/App_Themes/CMS/vendors/iCheck/skins/flat/green.css"

                     ));
            bundles.Add(new StyleBundle("~/Index2/css").Include(
                   "~/App_Themes/CMS/vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css",
                   "~/App_Themes/CMS/vendors/jqvmap/dist/jqvmap.min.css",
                   "~/App_Themes/CMS/vendors/bootstrap-daterangepicker/daterangepicker.css",
                   "~/App_Themes/CMS/build/css/custom.min.css"

                    ));
            bundles.Add(new ScriptBundle("~/Index/jquery").Include(
                       "~/App_Themes/CMS/vendors/jquery/dist/jquery.min.js",
                       "~/App_Themes/CMS/vendors/bootstrap/dist/js/bootstrap.bundle.min.js",
                       "~/App_Themes/CMS/vendors/fastclick/lib/fastclick.js",
                       "~/App_Themes/CMS/vendors/nprogress/nprogress.js",
                       "~/App_Themes/CMS/vendors/Chart.js/dist/Chart.min.js",
                       "~/App_Themes/CMS/vendors/gauge.js/dist/gauge.min.js",
                       "~/App_Themes/CMS/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js",
                       "~/App_Themes/CMS/vendors/iCheck/icheck.min.js",
                       "~/App_Themes/CMS/vendors/skycons/skycons.js",
                       "~/App_Themes/CMS/vendors/Flot/jquery.flot.js",
                       "~/App_Themes/CMS/vendors/Flot/jquery.flot.pie.js",
                       "~/App_Themes/CMS/vendors/Flot/jquery.flot.time.js",
                        "~/App_Themes/CMS/vendors/Flot/jquery.flot.stack.js",
                        "~/App_Themes/CMS/vendors/Flot/jquery.flot.resize.js",
                        "~/App_Themes/CMS/vendors/flot.orderbars/js/jquery.flot.orderBars.js",
                        "~/App_Themes/CMS/vendors/flot-spline/js/jquery.flot.spline.min.js",
                        "~/App_Themes/CMS/vendors/flot.curvedlines/curvedLines.js",
                        "~/App_Themes/CMS/vendors/DateJS/build/date.js",
                        "~/App_Themes/CMS/vendors/jqvmap/dist/jquery.vmap.js"
                       
                       ));
            bundles.Add(new ScriptBundle("~/Index2/jquery").Include(
                     
                       "~/App_Themes/CMS/vendors/jqvmap/dist/maps/jquery.vmap.world.js",
                       "~/App_Themes/CMS/vendors/jqvmap/examples/js/jquery.vmap.sampledata.js",
                        "~/App_Themes/CMS/vendors/moment/min/moment.min.js",
                         "~/App_Themes/CMS/vendors/bootstrap-daterangepicker/daterangepicker.js",
                         "~/App_Themes/CMS/build/js/custom.min.js"
                      ));

            bundles.Add(new StyleBundle("~/CreateApartment/css").Include(
                    "~/App_Themes/CMS/vendors/bootstrap/dist/css/bootstrap.min.css",
                   "~/App_Themes/CMS/vendors/bootstrap/dist/css/bootstrap.min.css",
                   "~/App_Themes/CMS/vendors/font-awesome/css/font-awesome.min.css",
                   "~/App_Themes/CMS/vendors/nprogress/nprogress.css",
                    "~/App_Themes/CMS/vendors/bootstrap-daterangepicker/daterangepicker.css",
                    "~/App_Themes/CMS/vendors/bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.css",
                    "~/App_Themes/CMS/vendors/normalize-css/normalize.css",
                    "~/App_Themes/CMS/vendors/ion.rangeSlider/css/ion.rangeSlider.css",
                    "~/App_Themes/CMS/vendors/ion.rangeSlider/css/ion.rangeSlider.skinFlat.css",
                    "~/App_Themes/CMS/vendors/mjolnic-bootstrap-colorpicker/dist/css/bootstrap-colorpicker.min.css",
                    "~/App_Themes/CMS/vendors/cropper/dist/cropper.min.css",
                    "~/App_Themes/CMS/build/css/custom.min.css"

                    ));
           

        }
    }


}
