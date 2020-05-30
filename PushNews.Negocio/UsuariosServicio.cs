using PushNews.Dominio;
using System;
using System.Linq;
using PushNews.Negocio.Interfaces;
using PushNews.Dominio.Entidades;

namespace PushNews.Negocio
{
    /// <summary>
    /// Contiene las implementaciones comunes de los métodos de la interfaz IBaseServicio.
    /// Todos los métodos deben poder ser sobrescritos.
    /// </summary>
    /// <typeparam name="T">Clase del tipo de entidad del dominio de PushNews.</typeparam>
    public class UsuariosServicio : BaseServicio<Usuario>, IUsuariosServicio
    {
        public UsuariosServicio(IPushNewsUnitOfWork db)
            : base(db)
        {}

        public override void Insert(Usuario usuario)
        {
            usuario.Clave = "";
            usuario.Activo = false;
            usuario.Creado = DateTime.UtcNow;
            usuario.Externo = false;
            base.Insert(usuario);
        }
    }
}