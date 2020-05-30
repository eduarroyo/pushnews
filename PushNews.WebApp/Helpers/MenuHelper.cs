using System.Web.Routing;
using PushNews.WebApp.Models.UI;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PushNews.WebApp.Helpers
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Arguments", "JustCode_LiteralArgumentIsNotNamedDiagnostic:The used literal argument is not named", Justification = "<Pendiente>")]
    public static class MenuHelper
    {
        public static MvcHtmlString MenuPrincipal(this HtmlHelper helper, IEnumerable<ElementoMenu> elementos, object htmlAttributes)
        {
            return MenuPrincipal(helper, elementos, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString MenuPrincipal(this HtmlHelper helper, IEnumerable<ElementoMenu> elementos, IDictionary<string, object> htmlAttributes = null)

        {
            var tag = new TagBuilder("ul");
            if (htmlAttributes != null)
            {
                tag.MergeAttributes(htmlAttributes);
            }
            foreach(var eActual in elementos)
            {
                tag.InnerHtml += ElementoMenu(helper, eActual);
            }
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ElementoMenu(this HtmlHelper helper, ElementoMenu elemento)
        {
            MvcHtmlString submenu, iconoFlecha, icono;
            
            if(elemento.TieneHijos)
            {
                submenu = MenuPrincipal(helper, elemento.Hijos, new { @class = "sub-menu" });
                var flecha = new TagBuilder("i");
                flecha.AddCssClass("icon-arrow");
                iconoFlecha = MvcHtmlString.Create(flecha.ToString(TagRenderMode.Normal));
            }
            else
            {
                submenu = MvcHtmlString.Create("");
                iconoFlecha = MvcHtmlString.Create("");
            }

            var texto = new TagBuilder("span");
            texto.AddCssClass("title");
            texto.SetInnerText(elemento.Texto);

            if(!String.IsNullOrWhiteSpace(elemento.ClaseIcono))
            {
                var ico = new TagBuilder("i");
                ico.AddCssClass(elemento.ClaseIcono);
                icono = MvcHtmlString.Create(ico.ToString(TagRenderMode.Normal));
            }
            else
            {
                icono = MvcHtmlString.Create("");
            }

            var selected = new TagBuilder("span");
            selected.AddCssClass("selected");
            var enlace = new TagBuilder("a");

            string area = string.IsNullOrWhiteSpace(elemento.Area) ? "" : $"{elemento.Area}";
            string modulo = string.IsNullOrWhiteSpace(elemento.Modulo) ? "" : $"#/{elemento.Modulo}";
            string sufijo = $"{area}{modulo}";
            enlace.MergeAttribute("href", string.IsNullOrEmpty(sufijo) ? "javascript:void(0);" : sufijo);

            enlace.InnerHtml += icono;
            enlace.InnerHtml += MvcHtmlString.Create(texto.ToString(TagRenderMode.Normal));
            enlace.InnerHtml += iconoFlecha;
            enlace.InnerHtml += MvcHtmlString.Create(selected.ToString(TagRenderMode.Normal));

            var li = new TagBuilder("li");
            li.InnerHtml += MvcHtmlString.Create(enlace.ToString(TagRenderMode.Normal));
            li.InnerHtml += submenu;

            return MvcHtmlString.Create(li.ToString(TagRenderMode.Normal));
        }
    }
}