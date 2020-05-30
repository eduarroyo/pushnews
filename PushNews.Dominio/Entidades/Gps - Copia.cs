//using System;
//using System.Collections.Generic;

//namespace PushNews.Dominio.Entidades
//{
//    public class Gps
//    {
//        public Gps()
//        {
//            Posiciones = new List<GpsPosicion>(0);
//            Matricula = "";
//            Estado = "";
//            Sensores = "";
//            UltimaPosicionDireccion = "";
//            Activo = true;
//        }

//        /// <summary>
//        /// Identificador único del GPS en PushNews
//        /// </summary>
//        public long GpsID { get; set; }

//        public string Api { get; set; }

//        /// <summary>
//        /// Identificador del GPS en la API
//        /// </summary>
//        public long GpsApiID { get; set; }

//        /// <summary>
//        /// Identificador de la aplicación a la que está asociado el GPS.
//        /// </summary>
//        public long AplicacionID { get; set; }

//        public bool Activo { get; set; }

//        /// <summary>
//        /// Nombre del GPS. Se empareja con el campo matrícula para identificar los registros que
//        /// se obtienen de la API de GPSs.
//        /// </summary>
//        public string Matricula { get; set; }

//        /// <summary>
//        /// Estado del GPS, copiado del último dato de posición obtenido.
//        /// </summary>
//        public string Estado { get; set; }

//        /// <summary>
//        /// Estado de carga de la batería, en porcentaje (supongo).
//        /// Null si el estado de carga es desconocido.
//        /// </summary>
//        public double? Bateria { get; set; }

//        /// <summary>
//        /// Cadena del GPS con el estado de los sensores.
//        /// Nos puede interesar por el estado de la batería.
//        /// </summary>
//        public string Sensores { get; set; }

//        /// <summary>
//        /// Fecha de la última consulta a la api.
//        /// </summary>
//        public DateTime? UltimaLecturaFecha { get; set; }

//        /// <summary>
//        /// Fecha de lectura de la posición por el dispositivo GPS.
//        /// </summary>
//        public DateTime? UltimaPosicionFecha { get; set; }

//        /// <summary>
//        /// Dirección de la posición GPS.
//        /// </summary>
//        public string UltimaPosicionDireccion { get; set; }

//        /// <summary>
//        /// Coordenadas GPS: latitud.
//        /// </summary>
//        public double? UltimaPosicionLatitud { get; set; }

//        /// <summary>
//        /// Coordenadas GPS: longitud.
//        /// </summary>
//        public double? UltimaPosicionLongitud { get; set; }

//        public virtual Aplicacion Aplicacion { get; set; }
//        public virtual ICollection<GpsPosicion> Posiciones { get; set; }

//    }
//}