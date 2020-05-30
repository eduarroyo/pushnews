using System.Threading.Tasks;
using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using System;

namespace PushNews.Negocio
{
    public class TerminalesServicio : BaseServicio<Terminal>, ITerminalesServicio
    {
        private Aplicacion aplicacion;

        public TerminalesServicio(IPushNewsUnitOfWork db, Aplicacion aplicacion)
            : base(db)
        {
            this.aplicacion = aplicacion;
            if (this.aplicacion != null)
            {
                RestrictFilter = t => t.AplicacionID == aplicacion.AplicacionID;
            }
        }

        /// <summary>
        /// Obtiene un terminal dado su nombre.
        /// </summary>
        public Terminal GetByName(string nombre)
        {
            return GetSingle(t => t.Nombre == nombre);
        }

        /// <summary>
        /// Obtiene un terminal dado su nombre.
        /// </summary>
        public async Task<Terminal> GetByNameAsync(string nombre)
        {
            return await GetSingleAsync(t => t.Nombre == nombre);
        }

        /// <summary>
        /// Acutaliza o crea un terminal con los datos del último acceso.
        /// </summary>
        /// <param name="nombre">Nombre del terminal.</param>
        /// <param name="ip">IP del terminal.</param>
        /// <param name="fecha">Instante del acceso.</param>
        /// <returns>El terminal acutalizado, sin persistir los cambios.</returns>
        public Terminal Acceso(string nombre, string ip, DateTime fecha)
        {
            Terminal t = GetByName(nombre);
            if (t == null)
            {
                t = Create();
                t.Nombre = nombre;
                Insert(t);
            }
            t.UltimaConexionIP = ip;
            t.UltimaConexionFecha = fecha;
            return t;
        }

        /// <summary>
        /// Acutaliza o crea un terminal con los datos del último acceso.
        /// </summary>
        /// <param name="nombre">Nombre del terminal.</param>
        /// <param name="ip">IP del terminal.</param>
        /// <returns>El terminal acutalizado, sin persistir los cambios.</returns>
        public Terminal Acceso(string nombre, string ip)
        {
            return Acceso(nombre, ip, DateTime.Now);
        }

        /// <summary>
        /// Registra un nuevo terminal, después de asignarle la aplicación a la que pertenece.
        /// </summary>
        public override void Insert(Terminal terminal)
        {
            terminal.AplicacionID = aplicacion.AplicacionID;
            base.Insert(terminal);
        }
    }
}
