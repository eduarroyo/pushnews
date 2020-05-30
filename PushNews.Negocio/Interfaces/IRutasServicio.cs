using PushNews.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace PushNews.Negocio.Interfaces
{
    public interface IRutasServicio : IBaseServicio<Ruta>
    {
        void CalcularRuta(long rutaId);
        IEnumerable<Ruta> RutasActivas(long aplicacionId, DateTime? fecha = null);
    }
}