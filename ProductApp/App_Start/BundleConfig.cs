using System.Web;
using System.Web.Optimization;

namespace ProductApp
{
    /// <summary>
    /// 它用来将js和css进行压缩（多个文件可以打包成一个文件）
    /// 在Web.config中，当compilation编译的debug属性设为true时，表示项目处于调试模式，这时Bundle是不会将文件进行打包压缩的
    /// 最终部署运行时，将debug设为false就可以看到js和css被打包和压缩了。
    /// </summary>
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));


            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
