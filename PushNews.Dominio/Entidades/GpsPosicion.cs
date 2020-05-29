using System;

namespace PushNews.Dominio.Entidades
{
    public class GpsPosicion
    {
        /// <summary>
        /// PK
        /// </summary>
        public long GpsPosicionID { get; set; }

        /// <summary>
        /// FK: Identificador del GPS que produce los datos.
        /// </summary>
        public long GpsID { get; set; }

        /// <summary>
        /// Fecha de la última consulta a la api.
        /// </summary>
        public DateTime LecturaFecha { get; set; }

        /// <summary>
        /// Fecha de lectura de la posición por el dispositivo GPS.
        /// </summary>
        public DateTime PosicionFecha { get; set; }

        // Dirección de la posición (campo localidad del mensaje recibido de la API).
        public string Direccion { get; set; }

        /// <summary>
        /// Coordenadas de la posición del gps: latitud
        /// </summary>
        public double Latitud { get; set; }

        /// <summary>
        /// Coordenadas de la posición del gps: longitud
        /// </summary>
        public double Longitud { get; set; }

        /// <summary>
        /// Velocidad medida por el dispositivo GPS.
        /// </summary>
        public double Velocidad { get; set; }

        /// <summary>
        /// Distancia recorrida medida por el dispositivo GPS.
        /// </summary>
        public double Kilometros { get; set; }

        /// <summary>
        /// Indica si el campo ha sido creado o editado manualmente.
        /// </summary>
        public bool Manual { get; set; }

        /// <summary>
        /// Estado de la batería en el momento de registrar la posición
        /// </summary>
        public double? Bateria { get; set; }

        public virtual Gps Gps { get; set; }
    }
}