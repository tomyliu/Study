using System.Web;
using System.Web.Optimization;

namespace TMall
{
    public class BundleConfig
    {
        // 有關捆綁的詳細資訊，請訪問 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用於開發和學習的 Modernizr 的開發版本。然後，當你做好
            // 生產準備就緒，請使用 https://modernizr.com 上的生成工具僅選擇所需的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/encrypt").Include(
                      "~/Scripts/jquery.md5.js"));
        }
    }
}

