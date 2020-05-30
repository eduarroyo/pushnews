using PushNews.Dominio;
using PushNews.Negocio.Interfaces;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Excepciones.Parametros;
using System.Collections.Generic;

namespace PushNews.Negocio
{
    public class ParametrosServicio : BaseServicio<Parametro>, IParametrosServicio
    {
        public ParametrosServicio(IPushNewsUnitOfWork db/*, Aplicacion aplicacion*/)
            : base(db)
        {
            CamposEvitarDuplicados = new[] { "Nombre" };
        }

        protected override void ComprobarDuplicados(Parametro parametro)
        {
            // Buscar un parámetro con el mismo nombre, la misma aplicación y diferente ID.
            Parametro otro = GetSingle(p => p.Nombre == parametro.Nombre
                                         && p.AplicacionID == parametro.AplicacionID
                                         && p.ParametroID != parametro.ParametroID);
            if (otro != null)
            {
                throw new ParametroExisteException(parametro.Nombre, otro.Aplicacion?.Nombre);
            }
        }

        public Parametro GetByName(string nombreParametro)
        {
            Parametro registro = GetSingle(r => r.Nombre == nombreParametro);
            return registro;
        }
        public IEnumerable<Parametro> ParametrosAplicacion(long aplicacionID)
        {
            return Get(p => p.AplicacionID == aplicacionID);
        }
    }
}