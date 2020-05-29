using System.Collections.Generic;

namespace PushNews.Dominio.Entidades
{
    public class Aplicacion
    {
        public Aplicacion()
        {
            Terminales = new List<Terminal>(0);
            Categorias = new List<Categoria>(0);
            Usuarios = new List<Usuario>(0);
            AplicacionesAmigas = new List<Aplicacion>(0);
            Parametros = new List<Parametro>(0);
            Telefonos = new List<Telefono>(0);
            Localizaciones = new List<Localizacion>(0);
            Caracteristicas = new List<AplicacionCaracteristica>(0);
            Asociados = new List<Asociado>(0);
            Empresas = new List<Empresa>(0);
            Hermandades = new List<Hermandad>(0);
            Gpss = new List<Gps>(0);
        }

        public long AplicacionID { get; set; }
        public long? LogotipoID { get; set; }
        public string Nombre { get; set; }
        public string Version { get; set; }
        public string Tipo { get; set; }
        public bool Activo { get; set; }
        public string CloudKey { get; set; }
        public string SubDominio { get; set; }
        public string ClaveSuscripcion { get; set; }
        public bool RequerirClaveSuscripcion { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }

        /// <summary>
        /// Clave de acceso para autorizar las apps
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Clave para autorizar a aplicaciones externas el acceos a la parte de asociados de la API.
        /// </summary>
        public string ApiKeyExternos { get; set; }
        public bool PermitirAccesoApiExternos { get; set; }

        /// <summary>
        /// Url de la aplicación en PlayStore de Google.
        /// </summary>
        public string PlayStoreUrl { get; set; }

        /// <summary>
        /// Url de la aplicación en ITunes
        /// </summary>
        public string ITunesUrl { get; set; }

        /// <summary>
        /// Url de la aplicación en Microsoft Store
        /// </summary>
        public string MicrosoftStoreUrl { get; set; }

        public virtual Documento Logotipo { get; set; }

        public virtual ICollection<Terminal> Terminales { get; set; }
        public virtual ICollection<Categoria> Categorias { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<Parametro> Parametros { get; set; }
        public virtual ICollection<Telefono> Telefonos { get; set; }
        public virtual ICollection<Localizacion> Localizaciones { get; set; }
        public virtual ICollection<Asociado> Asociados { get; set; }

        /// <summary>
        /// Aplicaciones amigas de la aplicación actual.
        /// </summary>
        public virtual ICollection<Aplicacion> AplicacionesAmigas { get; set; }

        public virtual ICollection<AplicacionCaracteristica> Caracteristicas { get; set; }

        public virtual ICollection<Empresa> Empresas { get; set; }
        public virtual ICollection<Hermandad> Hermandades { get; set; }
        public virtual ICollection<Gps> Gpss { get; set; }
    }
}
