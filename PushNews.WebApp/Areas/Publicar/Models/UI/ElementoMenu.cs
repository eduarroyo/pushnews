using System.Collections.Generic;
using System.Linq;

namespace PushNews.WebApp.Models.UI
{
    public class ElementoMenu
    {
        public ElementoMenu(string texto, string claseIcono, IEnumerable<ElementoMenu> hijos)
        {
            Texto = texto;
            ClaseIcono = claseIcono;
            Hijos = hijos != null ? hijos.ToList() : null;
        }

        public ElementoMenu(string texto, string modulo, string rol, string claseIcono = "",
            IEnumerable<ElementoMenu> hijos = null) : this(texto, "", modulo, rol, claseIcono, hijos)
        {}

        public ElementoMenu(string texto, string area, string modulo, string rol, string claseIcono = "",
            IEnumerable<ElementoMenu> hijos = null) : this(texto, claseIcono, hijos)
        {
            Modulo = modulo;
            Area = area;
            roles = new List<string>() { rol };
        }

        public string Texto { get; set; }

        public string ClaseIcono { get; set; }

        public string Area { get; set; }

        public string Modulo { get; set; }

        public IList<ElementoMenu> Hijos { get; set; }

        private readonly IEnumerable<string> roles;

        /// <summary>
        /// Devuelve los roles del elemento si los tuviera o si no, los roles de los hijos.
        /// </summary>
        public IEnumerable<string> Roles 
        {
            get
            {
                if (roles != null && roles.Any())
                {
                    return roles;
                }
                else
                {
                    return Hijos == null || !Hijos.Any()
                           ? new List<string>(0)
                           : Hijos.Select(h => h.Roles).SelectMany(x => x).Distinct();
                }
            }
        }

        public bool TieneHijos
        {
            get
            {
                return Hijos != null && Hijos.Any();
            }
        }
    }
}