using PushNews.Dominio;
using PushNews.Negocio.Interfaces;
using PushNews.Dominio.Entidades;
using System.Linq;
using PushNews.Negocio.Excepciones.Aplicaciones;

namespace PushNews.Negocio
{
    public class AplicacionesServicio : BaseServicio<Aplicacion>, IAplicacionesServicio
    {
        public AplicacionesServicio(IPushNewsUnitOfWork db)
            : base(db)
        {
            // Campos que no pueden repetirse entre diferentes reigstros.
            CamposEvitarDuplicados = new[] { "Nombre", "SubDominio" };
        }

        public Aplicacion GetBySubdomain(string subdomain)
        {
            return GetSingle(ap => ap.SubDominio == subdomain);
        }

        public Aplicacion GetByName(string name)
        {
            return GetSingle(ap => ap.Nombre == name);
        }

        /// <summary>
        /// Lanza una excepción si existe una aplicación diferente (con distinto valor de
        /// <paramref name="aplicacionID"/> cuyo nombre o subdominio coincidan con los que se reciben como
        /// parámetros.
        /// </summary>
        /// <exception cref="AplicacionExisteException">Se dispara si hay una aplicación previa con el mismo nombre.</exception>
        /// <exception cref="SubdominioExisteException">Se dispara si hay una aplicación previa con el mismo subdominio.</exception>
        protected override void ComprobarDuplicados(Aplicacion aplicacion)
        {
            Aplicacion otra = Get(
                a => (a.SubDominio == aplicacion.SubDominio || a.Nombre == aplicacion.Nombre)
                && a.AplicacionID != aplicacion.AplicacionID)
                .FirstOrDefault();
            if (otra != null)
            {
                if (otra.Nombre == aplicacion.Nombre)
                {
                    throw new AplicacionExisteException(aplicacion.Nombre);
                }
                else
                {
                    throw new SubdominioExisteException(aplicacion.SubDominio);
                }
            }
        }
    }
}