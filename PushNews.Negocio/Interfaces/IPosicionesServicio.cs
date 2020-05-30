using PushNews.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace PushNews.Negocio.Interfaces
{
    public interface IGpsPosicionesServicio : IBaseServicio<GpsPosicion>
    {
        /// <summary>
        /// Proporciona la lista de posiciones enmarcada en una ruta identificada por 
        /// <paramref name="rutaId"/>.
        /// </summary>
        /// <param name="rutaId">Identificador de la ruta.</param>
        /// <param name="maximoPosiciones">Si se establece un valor distinto de cero, limita el 
        /// número de posiciones que se cargan a la cantidad fijada.</param>
        /// <returns>Lista de posiciones de la ruta ordenadas por fecha.</returns>
        IEnumerable<GpsPosicion> PosicionesCabezaRuta(Ruta ruta, int maximoPosiciones = 0, int distanciaCorteRuta = 0);

        /// <summary>
        /// Proporciona la lista de posiciones enmarcada en una <paramref name="ruta"/>.
        /// </summary>
        /// <param name="ruta">Ruta para obtener posiciones.</param>
        /// <returns>Lista de posiciones de la ruta ordenadas por fecha.</returns>
        IEnumerable<GpsPosicion> PosicionesCabezaRuta(long rutaId, int maximoPosiciones = 0, int distanciaCorteRuta = 0);

        /// <summary>
        /// Proporciona la lista de posiciones de un gps identificado por <paramref name="gpsId"/> 
        /// dentro de un intervalo de tiempo.
        /// </summary>
        /// <param name="gpsId">Identificador del gps que generó las posiciones.</param>
        /// <param name="inicio">Inicio del intervalo para obtener las posiciones.</param>
        /// <param name="fin">Fin del intervalo para obtener las posiciones. Si no se especifica,
        /// se utiliza la fecha actual.</param>
        /// <returns>Lista de posiciones de la ruta ordenadas por fecha.</returns>
        IEnumerable<GpsPosicion> PosicionesGps(long gpsId, DateTime inicio, DateTime? fin = null);

        /// <summary>
        /// Obtiene la posición más reciente del gps de cola de una ruta, o lo que es lo mismo, la
        /// última posición del gps de cola que sea anterior a la fecha de fin, si la hubiera.
        /// Si la ruta no tiene fecha de fin, se devolverá la posición más reciente del gps de 
        /// cola.
        /// Si la ruta no tiene gps de cola devolverá <code>null</code>.
        /// </summary>
        /// <param name="ruta">Ruta, la posición de cuya cola queremos averiguar.</param>
        GpsPosicion UltimaPosicionColaRuta(Ruta ruta);

        /// <summary>
        /// Obtiene la posición más reciente del gps de cabeza de una ruta, o lo que es lo mismo,
        /// la última posición del gps de cabeza que sea anterior a la fecha de fin, si la hubiera.
        /// Si la ruta no tiene fecha de fin, se devolverá la posición más reciente del gps de 
        /// cabeza.
        /// </summary>
        /// <param name="ruta">Ruta, la posición de cuya cabeza queremos averiguar.</param>
        GpsPosicion UltimaPosicionCabezaRuta(Ruta ruta);

        /// <summary>
        /// Busca paradas en una ruta. Se considera una parada un conjunto de puntos consecutivos
        /// que se encuentran a una distancia inferior a <paramref name="distanciaMaximaParada"/> 
        /// metros, durante un tiempo superior a <paramref name="tiempoMinimoParada"/>.
        /// </summary>
        /// <param name="rutaSeleccionadaId">Identificador de la ruta</param>
        /// <param name="distanciaMaximaParada">Distancia máxima entre dos puntos para que se considere una parada.</param>
        /// <param name="tiempoMinimoParada">Tiempo mínimo que debe transcurrir para que se considere una parada.</param>
        /// <returns>Lista de tuplas de paradas encontradas: latitud, longitud, inicio, fin.</returns>
        IEnumerable<Tuple<double, double, DateTime, DateTime>> CalcularParadas(
            long rutaSeleccionadaId, double distanciaMaximaParada, TimeSpan tiempoMinimoParada);
        IEnumerable<Tuple<double, double, DateTime, DateTime>> CalcularParadas(
            List<GpsPosicion> puntosRuta, double distanciaMaximaParada, TimeSpan tiempoMinimoParada);
    }
}
