using System.Web.Optimization;

namespace DamvayShop.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/jquery").Include(
                        "~/Assets/client/js/jquery-1.11.0.min.js"
                        ));
            bundles.Add(new ScriptBundle("~/js/plugins").Include(
                       "~/Scripts/jquery-ui-1.12.1.min.js",
                       "~/Scripts/jquery.validate.min.js",
                       "~/Scripts/mustache.js",
                       "~/Scripts/numeral/numeral.min.js",
                       "~/Assets/client/js/bootstrap.min.js",
                       "~/Assets/client/js/common.js",
                       "~/Assets/client/js/jquery.cookie.js",
                       "~/Assets/client/js/waypoints.min.js",
                       "~/Assets/client/js/modernizr.js",
                       "~/Assets/client/js/bootstrap-hover-dropdown.js",
                       "~/Assets/client/js/owl.carousel.min.js",
                       "~/Assets/client/js/front.js"

                       ));
            bundles.Add(new ScriptBundle("~/js/shoppingcart").Include(
                       "~/Assets/client/js/shoppingCart.js"
                       ));

            bundles.Add(new StyleBundle("~/css/base")
                .Include("~/Assets/client/css/font-awesome.css",new CssRewriteUrlTransform())
                .Include("~/Assets/client/css/bootstrap.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/css/animate.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/css/owl.carousel.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/css/owl.theme.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/css/style.default.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/css/custom.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/css/customerRe.css", new CssRewriteUrlTransform())
                     );
            BundleTable.EnableOptimizations = true;
        }
    }
}