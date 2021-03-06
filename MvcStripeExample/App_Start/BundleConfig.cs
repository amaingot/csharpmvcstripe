﻿using System.Web;
using System.Web.Optimization;

namespace MvcStripeExample
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

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

            // Scripts and Styles for Admin area

            bundles.Add(new StyleBundle("~/Content/metisMenu").Include(
                "~/Content/sb-admin/bower_components/metisMenu/dist/metisMenu.min.css"));

            bundles.Add(new StyleBundle("~/Content/timeline").Include(
                "~/Content/sb-admin/dist/css/timeline.css"));

            bundles.Add(new StyleBundle("~/Content/admincss").Include(
                "~/Content/sb-admin/dist/css/sb-admin-2.css"));

            bundles.Add(new StyleBundle("~/Content/morris").Include(
                "~/Content/sb-admin/bower_components/morrisjs/morris.css"));

            bundles.Add(new StyleBundle("~/Content/font-awesome").Include(
                "~/Content/sb-admin/bower_components/font-awesome/css/font-awesome.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/sb-admin").Include(
                      "~/Scripts/sb-admin/dist/js/sb-admin-2.js"));

            bundles.Add(new ScriptBundle("~/bundles/metisMenu").Include(
                      "~/Scripts/sb-admin/bower_components/metisMenu/dist/metisMenu.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/morris").Include(
                      "~/Scripts/sb-admin/bower_components/raphael/raphael-min.js",
                      "~/Scripts/sb-admin/bower_components/morrisjs/morris.min.js",
                      "~/Scripts/sb-admin/js/morris-data.js"));
        }
    }
}
