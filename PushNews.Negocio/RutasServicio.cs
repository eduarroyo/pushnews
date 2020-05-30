using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Excepciones.Rutas;
using PushNews.Negocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;

namespace PushNews.Negocio
{
    public class RutasServicio : BaseServicio<Ruta>, IRutasServicio
    {
        public RutasServicio(IPushNewsUnitOfWork db)
            : base(db)
        {}

        public void CalcularRuta(long rutaId)
        {
            Ruta ruta;
            IEnumerable<Ruta> qRuta = this.Get(r => r.RutaID == rutaId, includeProperties: "GpsCabeza, GpsCola, Hermandad").Take(1);
            if(qRuta == null || !qRuta.Any())
            {
                throw new RutaNoExisteException(rutaId);
            }
            else
            {
                ruta = qRuta.ElementAt(0);
            }

            IGpsPosicionesServicio pSrv = new GpsPosicionesServicio(unitOfWork);
            List<GpsPosicion> posiciones = pSrv.PosicionesCabezaRuta(ruta).ToList();

            // Actualizar la posición de inicio de la ruta. Si no hay posiciones, las coordenadas
            // de inicio se dejan a (0, 0).
            var primeraPosicion = posiciones.FirstOrDefault();
            ruta.InicioLatitud = primeraPosicion?.Latitud ?? 0;
            ruta.InicioLongitud = primeraPosicion?.Longitud ?? 0;

            var ultimaPosicion = posiciones.LastOrDefault();
            ruta.FinLatitud = ultimaPosicion?.Latitud ?? 0;
            ruta.FinLongitud = ultimaPosicion?.Longitud ?? 0;

            // Actualizar valores de distancia, tiempo y velocidad calculados a partir de la lista
            // de posiciones.
            ruta.CalculoDistancia = CalcularDistanciaTotalMetros(posiciones);
            ruta.CalculoTiempo = CalcularTiempoTotal(posiciones);
            ruta.CalculoVelocidad = ruta.CalculoTiempo.TotalHours > 0 
                ? ruta.CalculoDistancia / ruta.CalculoTiempo.TotalHours
                : 0;

            // Actualizar los datos correspondientes a la cabeza de la ruta
            GpsPosicion ultimaPosicionCabezaRuta = pSrv.UltimaPosicionCabezaRuta(ruta);
            ruta.CabezaUltimaPosicionDireccion = ultimaPosicionCabezaRuta?.Direccion ?? "";
            ruta.CabezaUltimaPosicionFecha = ultimaPosicionCabezaRuta?.PosicionFecha;
            ruta.CabezaUltimaPosicionLatitud = ultimaPosicionCabezaRuta?.Latitud;
            ruta.CabezaUltimaPosicionLongitud = ultimaPosicionCabezaRuta?.Longitud;

            // Actualizar la posición de la hermandad si la ruta está activa y tiene datos válidos
            // de coordenadas de la cabeza de ruta.
            DateTime ahora = DateTime.Now;
            if (ruta.InicioFecha <= ahora && (!ruta.FinFecha.HasValue || ruta.FinFecha >= ahora)
                && ruta.CabezaUltimaPosicionLatitud.HasValue && ruta.CabezaUltimaPosicionLongitud.HasValue
                && ruta.CabezaUltimaPosicionLatitud != 0 && ruta.CabezaUltimaPosicionLongitud != 0)
            {
                ruta.Hermandad.IglesiaLatitud = ruta.CabezaUltimaPosicionLatitud;
                ruta.Hermandad.IglesiaLongitud = ruta.CabezaUltimaPosicionLongitud;
                ruta.Hermandad.IglesiaDireccion = ruta.CabezaUltimaPosicionDireccion;
            }

            // Si no hay GPS de cola, limpiar los datos de cola. Si lo hay, actualizar los datos 
            // correspondientes a la cola de la ruta.
            if (ruta.GpsCola == null)
            {
                ruta.ColaUltimaPosicionDireccion = "";
                ruta.ColaUltimaPosicionFecha = null;
                ruta.ColaUltimaPosicionLatitud = null;
                ruta.ColaUltimaPosicionLongitud = null;
            }
            else
            {
                GpsPosicion ultimaPosicionColaRuta = pSrv.UltimaPosicionColaRuta(ruta);
                ruta.ColaUltimaPosicionDireccion = ultimaPosicionColaRuta?.Direccion ?? "";
                ruta.ColaUltimaPosicionFecha = ultimaPosicionColaRuta?.PosicionFecha;
                ruta.ColaUltimaPosicionLatitud = ultimaPosicionColaRuta?.Latitud;
                ruta.ColaUltimaPosicionLongitud = ultimaPosicionColaRuta?.Longitud;
            }

            ApplyChanges();
        }

        /// <summary>
        /// Calcula la distancia total acumulada en metros entre una lista de posiciones.
        /// Si posiciones es null o tiene menos de dos posiciones, la distancia será siemrpe 0.
        /// La distancia se calcula en el orden en el que vienen los puntos.
        /// </summary>
        /// <param name="coordenadas">Lista de coordenadas ordenadas.</param>
        private double CalcularDistanciaTotalMetros(List<GpsPosicion> posiciones)
        {
            double distancia = 0;
            int numPos = posiciones != null ? posiciones.Count() : 0;
            
            // Debe haber al menos dos puntos para poder calcular algo.
            // En caso de que haya menos de dos puntos, la distancia siempre será 0.
            if(numPos > 1)
            {
                // Generar la lista de coordenadas a partir de la lista de posiciones.
                List<GeoCoordinate> coordenadas = posiciones
                   .Select(p => new GeoCoordinate(p.Latitud, p.Longitud))
                   .ToList();

                // Empezamos la iteración a partir del segundo punto.
                for (var i = 1; i < numPos; i++)
                {
                    // Coordenadas del punto de destino
                    GeoCoordinate c0 = coordenadas.ElementAt(i-1);
                    GeoCoordinate c1 = coordenadas.ElementAt(i);

                    // Calcular la distancia y acumular.
                    double distanciaPasoMetros = c1.GetDistanceTo(c0);
                    distancia += distanciaPasoMetros;
                }
            }

            return distancia;
        }

        /// <summary>
        /// Calcula el tiempo transcurrido entre la posición más antigua y la más nueva de la lista
        /// de la lista de <paramref name="posiciones"/>.
        /// </summary>
        /// <param name="posiciones">Lista de posiciones.</param>
        private TimeSpan CalcularTiempoTotal(List<GpsPosicion> posiciones)
        {
            TimeSpan resultado;

            // Si posiciones es nulo o contiene menos de dos elementos, el tiempo es 0.
            if(posiciones == null || posiciones.Count() < 2)
            {
                resultado = new TimeSpan(0);
            }
            else
            {
                // El tiempo total es la diferencia entre los valores máximo y mínimo de 
                // PosiciónFecha de entre los elementos de la lista de posiciones.
                DateTime horaMinima = posiciones.Min(p => p.PosicionFecha);
                DateTime horaMaxima = posiciones.Max(p => p.PosicionFecha);
                resultado = horaMaxima - horaMinima;
            }

            return resultado;
        }

        public override void Insert(Ruta entity)
        {
            base.Insert(entity);
            this.ApplyChanges();
            this.CalcularRuta(entity.RutaID);

        }

        public override void Update(Ruta entityToUpdate)
        {
            base.Update(entityToUpdate);
            this.ApplyChanges();
            this.CalcularRuta(entityToUpdate.RutaID);
        }

        /// <summary>
        /// Proporciona las rutas activas de una aplicación en una fecha determinada.
        /// </summary>
        /// <param name="aplicacionId">Aplicación cuyas rutas activas se quieren conocer.</param>
        /// <param name="fecha">Fecha para la que se quiere conocer las rutas activas. Si no se, 
        /// especifica ninguna, se considera la fecha actual.</param>
        public IEnumerable<Ruta> RutasActivas(long aplicacionId, DateTime? fecha = null)
        {
            DateTime fechaFiltro = fecha ?? DateTime.Now;
            return Get(r => r.GpsCabeza.AplicacionID == aplicacionId
                         && r.Hermandad.Activo
                         && r.InicioFecha <= fechaFiltro
                         && (!r.FinFecha.HasValue || r.FinFecha.Value >= fechaFiltro),
                       includeProperties: "GpsCabeza,GpsCola");
        }
    }
}