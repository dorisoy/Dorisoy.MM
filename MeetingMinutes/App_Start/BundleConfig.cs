using System.Web;
using System.Web.Optimization;

namespace MeetingMinutes
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/Global/Code").Include(
                      "~/Scripts/jquery-3.4.1.js",
                      "~/Scripts/angular.js",
                      //"~/Scripts/angular-route.js",
                      //"~/Scripts/Project/Global/sweetalert.js",
                      "~/Scripts/Projects/Global/sweetalert.min.js",
                      "~/Scripts/Projects/Global/app.js",
                      "~/Scripts/Projects/Global/Global.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                      "~/Content/assets/vendor/datatables/js/jquery.dataTables.js",
                      "~/Content/assets/vendor/datatables/js/dataTables.bootstrap4.js",
                      "~/Content/assets/vendor/typeahead/typeahead.jquery.min.js",
                      "~/Content/assets/vendor/typeahead/bloodhound.min.js"));
        }
    }
}
