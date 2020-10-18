using System.Diagnostics.CodeAnalysis;
using System.Web.Optimization;

namespace PushNews.WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        [SuppressMessage("Arguments",
            "JustCode_LiteralArgumentIsNotNamedDiagnostic:The used literal argument is not named",
            Justification = "Los parámetros pasados son suficientemente autoexplicativos y además se trata de una herramienta bien conocida del framework.")]
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/font-awesome.css",
                "~/assets/fonts/style.css",
                "~/assets/css/main.css",
                "~/assets/css/main-responsive.css",
                "~/assets/plugins/perfect-scrollbar/src/perfect-scrollbar.css",
                "~/assets/plugins/swipebox/css/swipebox.css",
                "~/assets/plugins/iCheck/orange.css",
                "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/cssportada").Include(
                "~/Content/bootstrap.css",
                "~/Content/font-awesome.css",
                "~/assets/fonts/style.css",
                "~/assets/css/main.css",
                "~/assets/css/main-responsive.css",
                "~/assets/plugins/swipebox/css/swipebox.css",
                "~/Content/site.css",
                "~/Content/styles-fix.css",
                "~/Content/portada.css"));

            bundles.Add(new LessBundle("~/Content/less").Include(
                "~/assets/less/styles.less"));

            bundles.Add(new StyleBundle("~/Content/kendoStyles").Include(
                "~/Content/kendo/2015.3.1111/kendo.common.min.css",
                "~/Content/kendo/2015.3.1111/kendo.mobile.all.min.css",
                "~/Content/kendo/2015.3.1111/kendo.dataviz.min.css",
                "~/Content/kendo/2015.3.1111/kendo.common-bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/kendoPushNews").Include(
                 "~/Content/kendo.pushnews.css",
                 "~/Content/styles-fix.css"));

            bundles.Add(new ScriptBundle("~/bundles/jQuery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/sammy-{version}.js",
                "~/Scripts/jquery.timeago.js",
                "~/assets/plugins/blockUI/jquery.blockUI.js",
                "~/assets/plugins/perfect-scrollbar/src/perfect-scrollbar.js",
                "~/assets/plugins/jquery-cookie/jquery.cookie.js",
                "~/assets/plugins/swipebox/js/jquery.swipebox.js",
                "~/assets/plugins/iCheck/icheck.js",
                "~/Scripts/coordenadas.js",
                "~/Scripts/pushnews/coordenadas2.js",
                "~/assets/js/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                "~/Scripts/bootstrap.js",
                "~/assets/plugins/jquery-cookie/jquery.cookie.js",
                "~/assets/plugins/perfect-scrollbar/src/perfect-scrollbar.js",
                "~/assets/js/login.js"
            ));

            #region KendoUI
            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                "~/Scripts/kendo/2015.3.1111/jszip.min.js",
                "~/Scripts/kendo/2015.3.1111/kendo.all.min.js",
                "~/Scripts/kendo/2015.3.1111/timezones.min.js",
                "~/Scripts/kendo/2015.3.1111/kendo.aspnetmvc.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo/internationalization/es").Include(
                "~/Scripts/kendo-global/cultures/kendo.culture.es-ES.js",
                "~/Scripts/kendo-global/messages/kendo.messages.es-ES.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo/internationalization/es-ES").Include(
                "~/Scripts/kendo-global/cultures/kendo.culture.es-ES.js",
                "~/Scripts/kendo-global/messages/kendo.messages.es-ES.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo/internationalization/en-US").Include(
                "~/Scripts/kendo-global/cultures/kendo.culture.en-US.js",
                "~/Scripts/kendo-global/messages/kendo.messages.en-US.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo/internationalization/en").Include(
                "~/Scripts/kendo-global/cultures/kendo.culture.en-US.js",
                "~/Scripts/kendo-global/messages/kendo.messages.en-US.js"));
            #endregion

           
            #region PushNews app
            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                "~/Scripts/pushnews/util.js",
                "~/Scripts/pushnews/home.js"));

            bundles.Add(new ScriptBundle("~/bundles/pushnews").Include(

                // Comunes
                "~/Scripts/pushnews/pushnews.js",
                "~/Scripts/pushnews/customvalidationrules.js",
                "~/Scripts/pushnews/util.js",
                "~/Scripts/pushnews/modulos/base/grilla-simple.js",
                "~/Scripts/pushnews/modulos/base/formulario.js",

                // Parámetros
                "~/Scripts/pushnews/modulos/parametros.js",

                // Escritorio
                "~/Scripts/pushnews/modulos/escritorio.js",

                // Aplicaciones
                "~/Scripts/pushnews/modulos/aplicaciones.js",

                // Características de aplicaciones
                "~/Scripts/pushnews/modulos/aplicacionescaracteristicas.js",

                // Usuarios
                "~/Scripts/pushnews/modulos/usuarios.js",

                // Categorias
                "~/Scripts/pushnews/modulos/categorias.js",

                // Teléfonos
                "~/Scripts/pushnews/modulos/telefonos.js",

                // Localizaciones
                "~/Scripts/pushnews/modulos/localizaciones.js",

                // Empresas
                "~/Scripts/pushnews/modulos/empresas.js",

                // Comunicaciones
                "~/Scripts/pushnews/modulos/formcomunicaciones.js",
                "~/Scripts/pushnews/modulos/comunicaciones.js",
                "~/Scripts/pushnews/modulos/comunicacion.js",
                "~/Scripts/pushnews/modulos/comunicacionesedicion.js",
                "~/Scripts/pushnews/modulos/editar.js",

                // Opciones y perfil
                "~/Scripts/pushnews/modulos/opciones.js",
                "~/Scripts/pushnews/modulos/perfil.js"
            ));
            #endregion

            // Poner a true para forzar las optimizaciones en debug.
            // es importante probar con los scripts optimizados antes de publicar.
            //BundleTable.EnableOptimizations = true;
        }
    }
}
