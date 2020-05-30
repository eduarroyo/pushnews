using System;
using System.Device.Location;

namespace PushNews.Negocio
{
    public static class Util
    {
        /// <summary>
        /// Calcula los segundos transcurridos desde el día 1/1/1970 hasta <paramref name="fecha"/>.
        /// </summary>
        public static long UnixTimeStamp(DateTime fecha)
        {
            DateTime origen = new DateTime(1970, 1, 1);
            TimeSpan fechaUnix = fecha - origen;
            long unixTimeStamp = (long)(Math.Round(fechaUnix.TotalSeconds, 0));
            return unixTimeStamp;
        }
        
        public static double Distancia(double latitud1, double longitud1, double latitud2, double longitud2)
        {
            GeoCoordinate c1 = new GeoCoordinate(latitud1, longitud1);
            GeoCoordinate c2 = new GeoCoordinate(latitud2, longitud2);
            double distancia = c1.GetDistanceTo(c2);
            return distancia;
        }
    }
}