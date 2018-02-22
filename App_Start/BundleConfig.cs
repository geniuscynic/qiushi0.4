using System.Web;
using System.Web.Optimization;

namespace qiushi.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery-{version}.js"
                        ));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/js/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                     "~/Content/js/jquery-{version}.js",
                      "~/Content/js/popper.min.js",
                      "~/Content/js/bootstrap.js",
                      "~/Content/js/jquery.validation.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/font/css/open-iconic.css",
                      "~/Content/css/site.css"
                     ));

            BundleTable.EnableOptimizations = false;
        }
    }
}
