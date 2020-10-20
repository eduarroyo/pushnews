using PushNews.AppceleratorPush;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushNews.PublicadorPush
{
    class PublicadorPush
    {
        static void Main(string[] args)
        {
            DateTime inicio = DateTime.Now;
            log4net.Config.XmlConfigurator.Configure();
            ILog log = LogManager.GetLogger(typeof(PublicadorPush));
            log.Info("#########################");
            log.Info("INICIO");

            int pushPublicaciones = 0, pushRecordatorios = 0, errores = 0;
            PushNewsEntities db = new PushNewsEntities();

            DateTime instantePrepararBd = DateTime.Now;
            log.Debug($"Tiempo preparar db: {(instantePrepararBd - inicio).TotalSeconds}.");

            Dictionary<string, string> parametrosGenerales = db.Parametros
                .Where(p => p.AplicacionID == null)
                .ToDictionary(k => k.Nombre, v => v.Valor);

            DateTime instanteParametrosCargados = DateTime.Now;
            log.Debug($"Tiempo cargar parámetros: {(instanteParametrosCargados - instantePrepararBd).TotalSeconds}");
            // Cargar el parámetro CanalPush. Si no se da un valor para este parámetro
            // en la tabla de parámetros, no se pueden enviar notificaciones push.
            if (!parametrosGenerales.TryGetValue("CanalPush", out string canal) || string.IsNullOrWhiteSpace(canal))
            {
                log.Error("Imposible enviar notificaciones push: Falta el parámetro \"CanalPush\".");
                return;
            }

            // Cargar el parámetro HorasEnvioPush. Este parámetro indica el intervalo en horas
            // hacia atrás en el tiempo en el que se buscan comunicaciones con envíos pendientes
            // (de publicación o de recordatorio). Si no está especificado, se utiliza como valor
            // por defecto 1h.

            double horas = 1;
            if (parametrosGenerales.TryGetValue("HorasEnvioPush", out string horasAtras))
            {
                horas = double.TryParse(horasAtras, out double aux) ? aux : 1;
            }

            DateTime instanteParametrosProcesados = DateTime.Now;
            log.Debug($"Tiempo procesar parámetros: {(instanteParametrosProcesados - instanteParametrosCargados).TotalSeconds} ");

            DateTime ahora = DateTime.Now;
            DateTime horaInicio = ahora.AddHours(-1 * horas);

            log.Info($"Buscar notificaciones de publicación pendientes entre {horaInicio:G} y {ahora:G}.");

            // Comunicaciones pendientes de push ordenadas por aplicación:
            // Las comunicaciones a enviar son aquellas que no han sido ya notificadas por push y no son instantáneas
            // o que tienen recordatorios pendientes, cuya aplicación tiene todos los datos necesarios para utilizar
            // el servicio push (CloudKey, Usuario y Clave), que no han sido borradas, que están activas y cuya fecha
            // de publicación está dentro del intervalo acutal de envío de notificaciones.
            //IComunicacionesServicio srv = factoria.ComunicacionesServicio();
            //IEnumerable<Comunicaciones> comunicacionesPendientes = srv.ComunicacionesPendientes();
            //srv.Get(c =>
            //// Condiciones generales: comunicación no borrada, activa y cuya aplicación tiene cloudkey, usuario y clave.
            //!c.Borrado && c.Activo
            //&& !string.IsNullOrEmpty(c.Categoria.Aplicacion.CloudKey)
            //&& !string.IsNullOrEmpty(c.Categoria.Aplicacion.Usuario)
            //&& !string.IsNullOrEmpty(c.Categoria.Aplicacion.Clave)
            //&& (
            //    // Condiciones para notificaciones: que no sea instantánea, que no esté ya enviada y que esté 
            //    // dentro del intervalo acutal de notificación.
            //    (!c.Instantanea && !c.PushEnviada && c.FechaPublicacion < ahora && c.FechaPublicacion > horaInicio)
            //    ||
            //    // Condiciones para recordatorios: que tenga recordatorio configurado, que no esté ya enviado
            //    // y que la fecha de recordatorio esté dentro del intervalo actual de notificación.
            //    (!c.PushRecordatorio.HasValue && c.RecordatorioFecha.HasValue 
            //        && c.RecordatorioFecha < ahora && c.RecordatorioFecha > horaInicio)
            //),
            //cc => cc.OrderBy(c => c.Categoria.AplicacionID)).ToList();
            //DateTime ahora = DateTime.Now;
            //DateTime horaInicio = ahora.AddHours(-1 * horas);
            // Comunicaciones pendientes de push ordenadas por aplicación:
            // Las comunicaciones a enviar son aquellas que no han sido ya notificadas por push y no son instantáneas
            // o que tienen recordatorios pendientes, cuya aplicación tiene todos los datos necesarios para utilizar
            // el servicio push (CloudKey, Usuario y Clave), que no han sido borradas, que están activas y cuya fecha
            // de publicación está dentro del intervalo acutal de envío de notificaciones.
            List<Comunicacion> comunicacionesPendientes = db.Set<Comunicacion>()
                .Include(c => c.Categoria)
                .Include(c => c.Categoria.Aplicacion)
                .Where(c =>
               // Condiciones generales: comunicación no borrada, activa y cuya aplicación tiene cloudkey, usuario y clave.
               !c.Borrado && c.Activo
               && !string.IsNullOrEmpty(c.Categoria.Aplicacion.CloudKey)
               && !string.IsNullOrEmpty(c.Categoria.Aplicacion.Usuario)
               && !string.IsNullOrEmpty(c.Categoria.Aplicacion.Clave)
               && (
                   // Condiciones para notificaciones: que no sea instantánea, que no esté ya enviada y que esté 
                   // dentro del intervalo acutal de notificación.
                   (!c.Instantanea && !c.PushEnviada && c.FechaPublicacion < ahora && c.FechaPublicacion > horaInicio)
                   ||
                   // Condiciones para recordatorios: que tenga recordatorio configurado, que no esté ya enviado
                   // y que la fecha de recordatorio esté dentro del intervalo actual de notificación.
                   (!c.PushRecordatorio.HasValue && c.RecordatorioFecha.HasValue
                       && c.RecordatorioFecha < ahora && c.RecordatorioFecha > horaInicio)
               ))
                .OrderBy(c => c.Categoria.AplicacionID)
                .ToList();

            log.Info("Mensajes pendientes cargados: " + comunicacionesPendientes.Count());
            DateTime instanteComunicacionesCargadas = DateTime.Now;
            log.Debug($"Tiempo cargar comunicaciones pendientes: {(instanteComunicacionesCargadas - instanteParametrosProcesados).TotalSeconds}");
            
            if (comunicacionesPendientes.Any())
            {
                // Aplicación de la comunicación actual.
                Aplicacion aplicacionActual = null;
                // Cliente rest para enviar mensajes push.
                CloudPush cp = null;
                bool recordatorio = false;
                string textoLog = "Comunicación o recordatorio";

                // Enviar un push por cada comunicación
                foreach(Comunicacion c in comunicacionesPendientes)
                {
                    try
                    {
                        // Actualizar el cliente para push cuando cambia la aplicación
                        if (aplicacionActual == null || aplicacionActual.AplicacionID != c.Categoria.AplicacionID)
                        {
                            aplicacionActual = c.Categoria.Aplicacion;
                            cp = new CloudPush(aplicacionActual.CloudKey, aplicacionActual.Usuario, aplicacionActual.Clave);
                        }

                        if (cp != null)
                        {
                            // Si una comunicación de comunicacionesPendientes tiene ya marcado que se ha enviado el push
                            // de publicación, significa que tiene pendiente el envío de recordatorio.
                            recordatorio = c.PushEnviada;
                            textoLog = recordatorio ? "Recordatorio" : "Comunicación";

                            string mensaje = c.Descripcion.Length > 50
                                ? c.Descripcion.Substring(0, 50) + "..."
                                : c.Descripcion;

                            Respuesta respuesta = cp.EnviarMensaje(
                                recordatorio ? c.RecordatorioTitulo : c.Titulo,
                                mensaje, true, canal, c.ComunicacionID.ToString());

                            if (respuesta.Meta.Code != 200)
                            {
                                // Loguear detalles en caso de error:
                                log.Info($"{textoLog}: {c.ComunicacionID} - El servidor devolvió un error al solicitar push: Código: {respuesta.Meta.Code} Estado: {respuesta.Meta.Status} Mensaje: {respuesta.Meta.Message} Método: {respuesta.Meta.Method_Name}.");
                                errores++;
                            }
                            else
                            {
                                // Actualizar comunicación y contadores en caso de que la notificación se haya enviado correctamente.
                                log.Info($"{textoLog}: {c.ComunicacionID} - Push OK");
                                if(recordatorio)
                                {
                                    c.PushRecordatorio = DateTime.Now;
                                    db.SaveChanges();
                                    pushRecordatorios++;
                                }
                                else
                                {
                                    c.PushEnviada = true;
                                    c.PushFecha = DateTime.Now;
                                    db.SaveChanges();
                                    pushPublicaciones++;
                                }
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        errores++;
                        log.Error($"{textoLog}: {c.ComunicacionID} - Error al solicitar push", e);
                    }
                }
            }

            DateTime instanteComunicacionesEnviadas = DateTime.Now;
            log.Debug($"Tiempo enviar mensajes push: {(instanteComunicacionesEnviadas - instanteComunicacionesCargadas).TotalSeconds}");

            log.Info("Mensajes de publicación enviados: " + pushPublicaciones);
            log.Info("Recordatorios enviados: " + pushRecordatorios);
            log.Info("Errores de publicación: " + errores);
            log.Info($"Tiempo total: {(instanteComunicacionesEnviadas - inicio).TotalSeconds}");
            log.Info("FIN");
        }
    }
}
