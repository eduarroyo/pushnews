using System;

namespace PushNews.Dominio.Entidades
{
    public class Ruta
    {
        public Ruta()
        {
            Descripcion = "";
            InicioDescripcion = "";
            EntradaEnCarreraOficial = "";
            FinDescripcion = "";
            CabezaUltimaPosicionDireccion = "";
            ColaUltimaPosicionDireccion = "";
        }

        public long RutaID { get; set; }
        public long HermandadID { get; set; }

        public string Descripcion { get; set; }

        public DateTime InicioFecha { get; set; }
        public string InicioDescripcion { get; set; }
        public double InicioLatitud { get; set; }
        public double InicioLongitud { get; set; }

        public string EntradaEnCarreraOficial { get; set; }
        public double EntradaEnCarreraOficialLatitud { get; set; }
        public double EntradaEnCarreraOficialLongitud { get; set; }

        public DateTime? FinFecha { get; set; }
        public string FinDescripcion { get; set; }
        public double FinLatitud { get; set; }
        public double FinLongitud { get; set; }

        public long GpsCabezaID { get; set; }
        public DateTime? CabezaUltimaPosicionFecha { get; set; }
        public string CabezaUltimaPosicionDireccion { get; set; }
        public double? CabezaUltimaPosicionLongitud { get; set; }
        public double? CabezaUltimaPosicionLatitud { get; set; }

        public long? GpsColaID { get; set; }

        public DateTime? ColaUltimaPosicionFecha { get; set; }
        public string ColaUltimaPosicionDireccion { get; set; }
        public double? ColaUltimaPosicionLongitud { get; set; }
        public double? ColaUltimaPosicionLatitud { get; set; }

        /// <summary>
        /// Velocidad media calculada mediante la división de la distancia recorrida 
        /// (CalculoDistancia) entre el tiempo transcurrido entre la primera y la última posición
        /// de la ruta (CalculoTiempo).
        /// </summary>
        public double CalculoVelocidad { get; set; }

        /// <summary>
        /// Suma acumulada de la distancia entre las coordenadas de una posición de la ruta y las
        /// de la posición anterior.
        /// </summary>
        public double CalculoDistancia { get; set; }

        /// <summary>
        /// Diferencia de tiempo entre el primer y el último registro de posiciones, que se utiliza
        /// para el cálculo de la velocidad.
        /// </summary>
        public TimeSpan CalculoTiempo
        {
            get {
                return TimeSpan.FromHours(CalculoTiempoHoras);
            }
            set { CalculoTiempoHoras = value.TotalHours; }
        }

        /// <summary>
        /// Diferencia de tiempo en horas totales entre el primer y el último registro de
        /// posiciones, que se utiliza para el cálculo de la velocidad.
        /// </summary>
        public double CalculoTiempoHoras { get; set; }


        public virtual Hermandad Hermandad { get; set; }
        public virtual Gps GpsCabeza { get; set; }
        public virtual Gps GpsCola { get; set; }
    }
}
