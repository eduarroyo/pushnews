using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using System;
using System.Linq;

namespace PushNews.Negocio
{
    public class ComunicacionesAccesosServicio : BaseServicio<ComunicacionAcceso>, IComunicacionesAccesosServicio
    {
        private Aplicacion aplicacion;

        TimeSpan periodoNoConteo;

        public ComunicacionesAccesosServicio(IPushNewsUnitOfWork db, Aplicacion aplicacion)
            : base(db)
        {
            this.aplicacion = aplicacion;
            if (this.aplicacion != null)
            {
                RestrictFilter = ca => ca.Terminal.AplicacionID == aplicacion.AplicacionID;
            }

            IParametrosServicio pSrv = new ParametrosServicio(db);
            Parametro paramPeriodoNoConteo = pSrv.GetByName("PeriodoNoConteoMinutos");
            int minutosNoConteo = int.Parse(paramPeriodoNoConteo?.Valor ?? "1");
            periodoNoConteo = new TimeSpan(0, minutosNoConteo, 0);
        }

        public void AccesoTerminal(Comunicacion comunicacion, string uid, string ip, long? asociadoId = null)
        {
            DateTime ahora = DateTime.Now;
            // Obtener el terminal actualizado con los datos del último acceso. Si no existe, se crea.
            ITerminalesServicio tsrv = new TerminalesServicio(unitOfWork, aplicacion);
            Terminal t = tsrv.Acceso(uid, ip, ahora);

            ComunicacionAcceso ultimaConsulta = Get(ca => ca.TerminalID == t.TerminalID 
                                                       && ca.ComunicacionID == comunicacion.ComunicacionID)
                .OrderByDescending(ca => ca.Fecha)
                .FirstOrDefault();

            TimeSpan periodoDesdeUltimaConsulta = ultimaConsulta != null
                ? DateTime.Now - ultimaConsulta.Fecha
                : TimeSpan.MaxValue;

            if (periodoDesdeUltimaConsulta > periodoNoConteo)
            {
                // Crear el registro de acceso del terminal a la comunicación.
                ComunicacionAcceso nueva = Create();
                nueva.Terminal = t;
                nueva.Comunicacion = comunicacion;
                nueva.Fecha = ahora;
                nueva.AsociadoID = asociadoId;
                Insert(nueva);
            }
        }
    }
}
