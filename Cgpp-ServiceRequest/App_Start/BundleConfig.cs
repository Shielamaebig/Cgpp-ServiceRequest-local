using System.Web;
using System.Web.Optimization;

namespace Cgpp_ServiceRequest
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/Scripts/extra-lib/DataTables/DataTables-1.10.16/js/jquery.dataTables.min.j",
                        "~/Content/assets/vendor/bootstrap/js/bootstrap.bundle.min.js",
                        "~/Content/assets/vendor/quill/quill.min.js",
                        "~/Scripts/extra-libs/DataTables/datatables.min.js",
                        "~/Scripts/extra-libs/DataTables/datatables-select.min.js",
                        "~/Scripts/extra-libs/DataTables/datatables.js",
                        "~/Scripts/jquery.datetimepicker.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/toastr.js",
                        "~/Content/assets/js/main.js"
                     ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                                "~/Scripts/custom.js"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/assets/vendor/fontawesome-free/css/all.min.css",
                      "~/Content/assets/vendor/bootstrap/css/bootstrap.min.css",
                      "~/Content/assets/vendor/bootstrap-icons/bootstrap-icons.css",
                      "~/Content/assets/vendor/boxicons/css/boxicons.min.css",
                      "~/Content/assets/vendor/quill/quill.snow.css",
                      "~/Content/assets/vendor/simple-datatables/style.css",
                      "~/Content/assets/vendor/datatables.net-bs4/css/dataTables.bootstrap4.css",
                      "~/Content/summernote-lite.css",
                      "~/Content/toastr.css",
                      "~/Content/jquery.datetimepicker.css",
                      "~/Content/custom.css",
                      "~/Content/assets/css/style.css"
                      ));
        }
    }
}
