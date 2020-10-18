using System;
using PushNews.Negocio.Interfaces;
using PushNews.Dominio;
using PushNews.Dominio.Entidades;

namespace PushNews.Negocio
{
    public class ServiciosFactoria: IServiciosFactoria
    {
        private readonly IPushNewsUnitOfWork db;
        private readonly Aplicacion aplicacion;

        public ServiciosFactoria(IPushNewsUnitOfWork dbContext, Aplicacion aplicacion)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(paramName: "dbContext");
            }

            db = dbContext;
            this.aplicacion = aplicacion;
        }

        public IAplicacionesServicio AplicacionesServicio()
        {
            return new AplicacionesServicio(db);
        }

        public IParametrosServicio ParametrosServicio()
        {
            return new ParametrosServicio(db);
        }

        public IUsuariosServicio UsuariosServicio()
        {
            return new UsuariosServicio(db);
        }

        public ICategoriasServicio CategoriasServicio()
        {
            return new CategoriasServicio(db, aplicacion);
        }

        public IComunicacionesServicio ComunicacionesServicio()
        {
            return new ComunicacionesServicio(db, aplicacion);
        }

        public IComunicacionesAccesosServicio ComunicacionesAccesosServicio()
        {
            return new ComunicacionesAccesosServicio(db, aplicacion);
        }

        public ITerminalesServicio TerminalesServicio()
        {
            return new TerminalesServicio(db, aplicacion);
        }

        public IDocumentosServicio DocumentosServicio()
        {
            return new DocumentosServicio(db, aplicacion);
        }

        public ITelefonosServicio TelefonosServicio()
        {
            return new TelefonosServicio(db, aplicacion);
        }

        public ILocalizacionesServicio LocalizacionesServicio()
        {
            return new LocalizacionesServicio(db, aplicacion);
        }

        public IAplicacionesCaracteristicasServicio AplicacionesCaracteristicasServicio()
        {
            return new AplicacionesCaracteristicasServicio(db);
        }

        public IEmpresasServicio EmpresasServicio()
        {
            return new EmpresasServicio(db, aplicacion);
        }
    }
}