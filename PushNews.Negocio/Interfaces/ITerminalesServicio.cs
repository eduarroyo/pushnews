using PushNews.Dominio.Entidades;
using System;
using System.Threading.Tasks;

namespace PushNews.Negocio.Interfaces
{
    public interface ITerminalesServicio : IBaseServicio<Terminal>
    {
        /// <summary>
        /// Obtiene un terminal dado su nombre.
        /// </summary>
        Terminal GetByName(string nombre);

        /// <summary>
        /// Obtiene un terminal dado su nombre.
        /// </summary>
        Task<Terminal> GetByNameAsync(string nombre);

        /// <summary>
        /// Acutaliza o crea un terminal con los datos del último acceso.
        /// </summary>
        /// <param name="nombre">Nombre del terminal.</param>
        /// <param name="ip">IP del terminal.</param>
        /// <returns>El terminal acutalizado, sin persistir los cambios.</returns>
        Terminal Acceso(string nombre, string ip);

        /// <summary>
        /// Acutaliza o crea un terminal con los datos del último acceso.
        /// </summary>
        /// <param name="nombre">Nombre del terminal.</param>
        /// <param name="ip">IP del terminal.</param>
        /// <param name="fecha">Instante del acceso.</param>
        /// <returns>El terminal acutalizado, sin persistir los cambios.</returns>
        Terminal Acceso(string nombre, string ip, DateTime fechaAcceso);
    }
}
