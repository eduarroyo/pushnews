using PushNews.Dominio.Entidades;
using System.Collections.Generic;

namespace PushNews.Negocio.Interfaces
{
    public interface IParametrosServicio : IBaseServicio<Parametro>
    {
        Parametro GetByName(string nombreParametro);

        IEnumerable<Parametro> ParametrosAplicacion(long aplicacionID);
    }
}