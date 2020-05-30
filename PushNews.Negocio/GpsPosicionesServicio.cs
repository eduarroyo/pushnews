using System;
using System.Collections.Generic;
using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using PushNews.Negocio.Excepciones.Rutas;
using System.Linq;

namespace PushNews.Negocio
{
    public class GpsPosicionesServicio : BaseServicio<GpsPosicion>, IGpsPosicionesServicio
    {
        public GpsPosicionesServicio(IPushNewsUnitOfWork db)
            : base(db)
        {}

        public override void Insert(GpsPosicion entity)
        {
            base.Insert(entity);
        }

        /// <summary>
        /// Proporciona la lista de posiciones enmarcada en una ruta identificada por 
        /// <paramref name="rutaId"/>.
        /// </summary>
        /// <param name="rutaId">Identificador de la ruta.</param>
        /// <returns>Lista de posiciones de la ruta ordenadas por fecha.</returns>
        public IEnumerable<GpsPosicion> PosicionesCabezaRuta(long rutaId, int maximoPosiciones = 0, int distanciaCorteRuta = 0)
        {
            IRutasServicio rSrv = new RutasServicio(unitOfWork);
            Ruta ruta = rSrv.GetSingle(r => r.RutaID == rutaId);
            if (ruta == null)
            {
                throw new RutaNoExisteException(rutaId);
            }

            return PosicionesCabezaRuta(ruta, maximoPosiciones, distanciaCorteRuta);
        }

        /// <summary>
        /// Proporciona la lista de posiciones enmarcada en una <paramref name="ruta"/>.
        /// </summary>
        /// <param name="ruta">Ruta para obtener posiciones.</param>
        /// <returns>Lista de posiciones de la ruta ordenadas por fecha.</returns>
        public IEnumerable<GpsPosicion> PosicionesCabezaRuta(Ruta ruta, int maximoPosiciones = 0, int distanciaCorteRuta = 0)
        {
            var posiciones = PosicionesGps(ruta.GpsCabezaID, ruta.InicioFecha, ruta.FinFecha);
            if (maximoPosiciones > 0)
            {
                posiciones = posiciones.Skip(posiciones.Count() - maximoPosiciones);
            }
            if (ruta.ColaUltimaPosicionLatitud.HasValue && ruta.ColaUltimaPosicionLongitud.HasValue)
            {
                // Todas las posiciones de la ruta ordenadas por fecha ascendente.
                List<GpsPosicion> posLista = posiciones.ToList();
                if(posLista.Count > 2)
                {
                    GpsPosicion posicionActual = posLista.Last();

                    // Distancia entre la posición más reciente de la cabeza y la de la cola.
                    double distanciaActual = Math.Abs(Util.Distancia(posicionActual.Latitud,
                        posicionActual.Longitud, ruta.ColaUltimaPosicionLatitud.Value,
                        ruta.ColaUltimaPosicionLongitud.Value));
                    //double distanciaAnterior = double.MaxValue;
                    //bool signoDiferenciaLatitudActual = (posicionActual.Latitud - ruta.ColaUltimaPosicionLatitud.Value) > 0;
                    //bool signoDiferenciaLongitudActual = (posicionActual.Longitud - ruta.ColaUltimaPosicionLongitud.Value) > 0;
                    //bool signoDiferenciaLatitudAnterior = signoDiferenciaLatitudActual;
                    //bool signoDiferenciaLongitudAnterior = signoDiferenciaLongitudActual;
                    int i = posLista.Count - 2;
                    while (i >= 0 && (distanciaCorteRuta <= 0 || distanciaActual > distanciaCorteRuta)
                        /*&& signoDiferenciaLatitudActual == signoDiferenciaLatitudAnterior
                        && signoDiferenciaLongitudActual == signoDiferenciaLongitudAnterior*/
                        /*Math.Abs(distanciaActual) < distanciaAnterior*/
                        )
                    {
                        posicionActual = posLista[i];
                        //signoDiferenciaLatitudAnterior = signoDiferenciaLatitudActual;
                        //signoDiferenciaLongitudAnterior = signoDiferenciaLongitudActual;
                        //signoDiferenciaLatitudActual = (posicionActual.Latitud - ruta.ColaUltimaPosicionLatitud.Value) > 0;
                        //signoDiferenciaLongitudActual = (posicionActual.Longitud - ruta.ColaUltimaPosicionLongitud.Value) > 0;
                        //distanciaAnterior = Math.Abs(distanciaActual);
                        distanciaActual = Math.Abs(Util.Distancia(posicionActual.Latitud,
                            posicionActual.Longitud, ruta.ColaUltimaPosicionLatitud.Value,
                            ruta.ColaUltimaPosicionLongitud.Value));
                        i--;
                    }

                    posLista = posLista.Skip(1+i).ToList();
                }

                return posLista;
            }
            else
            {
                if(maximoPosiciones > 0)
                {
                    return posiciones.Take((int) maximoPosiciones);
                }
                else
                {
                    return posiciones;
                }

            }
        }

        /// <summary>
        /// Proporciona la lista de posiciones de un gps identificado por <paramref name="gpsId"/> 
        /// dentro de un intervalo de tiempo.
        /// </summary>
        /// <param name="gpsId">Identificador del gps que generó las posiciones.</param>
        /// <param name="inicio">Inicio del intervalo para obtener las posiciones.</param>
        /// <param name="fin">Fin del intervalo para obtener las posiciones. Si no se especifica,
        /// se utiliza la fecha actual.</param>
        /// <returns>Lista de posiciones de la ruta ordenadas por fecha.</returns>
        public IEnumerable<GpsPosicion> PosicionesGps(long gpsId, DateTime inicio, DateTime? fin = default(DateTime?))
        {
            IEnumerable<GpsPosicion> posiciones;
            if (fin.HasValue)
            {
                posiciones = Get(p => p.GpsID == gpsId
                              && p.PosicionFecha >= inicio
                              && p.PosicionFecha <= fin.Value,
                              pp => pp.OrderBy(p => p.PosicionFecha));
            }
            else
            {
                posiciones = Get(p => p.GpsID == gpsId
                              && p.PosicionFecha >= inicio,
                              pp => pp.OrderBy(p => p.PosicionFecha));
            }
            return posiciones;
        }

        /// <summary>
        /// Obtiene la posición más reciente del gps de cola de una ruta, o lo que es lo mismo, la
        /// última posición del gps de cola que sea anterior a la fecha de fin, si la hubiera.
        /// Si la ruta no tiene fecha de fin, se devolverá la posición más reciente del gps de 
        /// cola.
        /// Si la ruta no tiene gps de cola devolverá <code>null</code>.
        /// </summary>
        /// <param name="ruta">Ruta, la posición de cuya cola queremos averiguar.</param>
        public GpsPosicion UltimaPosicionColaRuta(Ruta ruta)
        {
            GpsPosicion ultimaPosicionCola = null;

            if (ruta.GpsColaID.HasValue)
            {
                ultimaPosicionCola = Get(
                    p => p.GpsID == ruta.GpsColaID.Value
                      && p.PosicionFecha >= ruta.InicioFecha
                      && (!ruta.FinFecha.HasValue || p.PosicionFecha <= ruta.FinFecha.Value),
                    pp => pp.OrderByDescending(p => p.PosicionFecha))
                    .FirstOrDefault();
            }

            return ultimaPosicionCola;

        }

        /// <summary>
        /// Obtiene la posición más reciente del gps de cabeza de una ruta, o lo que es lo mismo,
        /// la última posición del gps de cabeza que sea anterior a la fecha de fin, si la hubiera.
        /// Si la ruta no tiene fecha de fin, se devolverá la posición más reciente del gps de 
        /// cabeza.
        /// </summary>
        /// <param name="ruta">Ruta, la posición de cuya cabeza queremos averiguar.</param>
        public GpsPosicion UltimaPosicionCabezaRuta(Ruta ruta)
        {
            GpsPosicion ultimaPosicionCabeza = null;

            ultimaPosicionCabeza = Get(
                p => p.GpsID == ruta.GpsCabezaID
                    && p.PosicionFecha >= ruta.InicioFecha
                    && (!ruta.FinFecha.HasValue || p.PosicionFecha <= ruta.FinFecha.Value),
                pp => pp.OrderByDescending(p => p.PosicionFecha))
                .FirstOrDefault();

            return ultimaPosicionCabeza;
        }

        /// <summary>
        /// Busca paradas en una ruta. Se considera una parada un conjunto de puntos consecutivos
        /// que se encuentran a una distancia inferior a <paramref name="distanciaMaximaParada"/> 
        /// metros, durante un tiempo superior a <paramref name="tiempoMinimoParada"/>.
        /// </summary>
        /// <param name="rutaId">Identificador de la ruta</param>
        /// <param name="distanciaMaximaParada">Distancia máxima entre dos puntos para que se considere una parada.</param>
        /// <param name="tiempoMinimoParada">Tiempo mínimo que debe transcurrir para que se considere una parada.</param>
        /// <returns>Lista de tuplas de paradas encontradas: latitud, longitud, inicio, fin.</returns>
        public IEnumerable<Tuple<double, double, DateTime, DateTime>> CalcularParadas(
            long rutaId, double distanciaMaximaParada, TimeSpan tiempoMinimoParada)
        {
            List<GpsPosicion> posicionesRuta = this.PosicionesCabezaRuta(rutaId).ToList();
            return CalcularParadas(posicionesRuta, distanciaMaximaParada, tiempoMinimoParada);
        }


        /// <summary>
        /// Busca paradas en una ruta. Se considera una parada un conjunto de puntos consecutivos
        /// que se encuentran a una distancia inferior a <paramref name="distanciaMaximaParada"/> 
        /// metros, durante un tiempo superior a <paramref name="tiempoMinimoParada"/>.
        /// </summary>
        /// <param name="rutaId">Identificador de la ruta</param>
        /// <param name="distanciaMaximaParada">Distancia máxima entre dos puntos para que se considere una parada.</param>
        /// <param name="tiempoMinimoParada">Tiempo mínimo que debe transcurrir para que se considere una parada.</param>
        /// <returns>Lista de tuplas de paradas encontradas: latitud, longitud, inicio, fin.</returns>
        public IEnumerable<Tuple<double, double, DateTime, DateTime>> CalcularParadas(List<GpsPosicion> posicionesRuta,
            double distanciaMaximaParada, TimeSpan tiempoMinimoParada)
        {
            List<Tuple<double, double, DateTime, DateTime>> paradas = new List<Tuple<double, double, DateTime, DateTime>>();
            double paradaLatitud = 0, paradaLongitud = 0;
            DateTime? paradaInicio = null, paradaFin = null;

            foreach(GpsPosicion p in posicionesRuta)
            {
                // Si no hay datos de parada para comparar, establecerlos a partir del punto actual.
                if(!paradaInicio.HasValue)
                {
                    paradaInicio = p.PosicionFecha;
                    paradaFin = p.PosicionFecha;
                    paradaLatitud = p.Latitud;
                    paradaLongitud = p.Longitud;
                }
                else
                {
                    // Si la distancia del punto actual es menor al máximo de detección de parada,
                    // se considera que el punto está englobado dentro de la parada.
                    double distancia = Math.Abs(Util.Distancia(paradaLatitud, paradaLongitud, p.Latitud, p.Longitud));
                    if(distancia < distanciaMaximaParada)
                    {
                        paradaFin = p.PosicionFecha;
                    }

                    // Cuando el punto se aleja de las coordenadas iniciales de la parada más de 
                    // la distancia máxima, se comprueba si el tiempo transcurrido ha superado el
                    // mínimo para considerarse una parada. Si es así, se registra la parada.
                    else
                    {
                        if ((paradaFin - paradaInicio) > tiempoMinimoParada)
                        {
                            paradas.Add(new Tuple<double, double, DateTime, DateTime>(
                                paradaLatitud, paradaLongitud, paradaInicio.Value, paradaFin.Value));
                        }
                        paradaInicio = p.PosicionFecha;
                        paradaFin = p.PosicionFecha;
                        paradaLatitud = p.Latitud;
                        paradaLongitud = p.Longitud;
                    }
                }
            }

            return paradas;
        }
    }
}