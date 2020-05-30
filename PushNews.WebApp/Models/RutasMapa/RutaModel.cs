using PushNews.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace PushNews.WebApp.Models.RutasMapa
{

    public class RutaModel
    {
        public static Func<Ruta, RutaModel> FromEntity =
            r => new RutaModel()
            {
                Descripcion = r.Descripcion,
                HermandadID = r.HermandadID,
                InicioDescripcion = r.InicioDescripcion,
                InicioFecha = r.InicioFecha,
                InicioLatitud = r.InicioLatitud,
                InicioLongitud = r.InicioLongitud,
                EntradaEnCarreraOficial = r.EntradaEnCarreraOficial,
                CarreraOficialLatitud = r.EntradaEnCarreraOficialLatitud,
                CarreraOficialLongitud = r.EntradaEnCarreraOficialLongitud,
                FinDescripcion = r.FinDescripcion,
                FinFecha = r.FinFecha,
                FinLatitud = r.FinLatitud,
                FinLongitud = r.FinLongitud,

                CabezaFecha = r.CabezaUltimaPosicionFecha,
                CabezaDireccion = r.CabezaUltimaPosicionDireccion,
                CabezaLatitud = r.CabezaUltimaPosicionLatitud,
                CabezaLongitud = r.CabezaUltimaPosicionLongitud,

                ColaFecha = r.ColaUltimaPosicionFecha,
                ColaDireccion = r.ColaUltimaPosicionDireccion,
                ColaLatitud = r.ColaUltimaPosicionLatitud,
                ColaLongitud = r.ColaUltimaPosicionLongitud,

                Velocidad = r.CalculoVelocidad,
                Distancia = r.CalculoDistancia
            };

        public string Descripcion { get; set; }
        public long HermandadID { get; set; }
        public string InicioDescripcion { get; set; }
        public DateTime InicioFecha { get; set; }
        public double InicioLatitud { get; set; }
        public double InicioLongitud { get; set; }
        public string EntradaEnCarreraOficial { get; set; }
        public double CarreraOficialLatitud { get; set; }
        public double CarreraOficialLongitud { get; set; }
        public string FinDescripcion { get; set; }
        public DateTime? FinFecha { get; set; }
        public double FinLatitud { get; set; }
        public double FinLongitud { get; set; }
        public double Velocidad { get; set; }
        public double Distancia { get; set; }

        public DateTime? CabezaFecha { get; set; }
        public string CabezaDireccion { get; set; }
        public double? CabezaLatitud { get; set; }
        public double? CabezaLongitud { get; set; }

        public DateTime? ColaFecha { get; set; }
        public string ColaDireccion { get; set; }
        public double? ColaLatitud { get; set; }
        public double? ColaLongitud { get; set; }

        public IEnumerable<PosicionModel> Posiciones { get; set; }
    }

    public class PosicionModel
    {
        public static Func<GpsPosicion, PosicionModel> FromEntity =
            p => new PosicionModel()
            {
                Fecha = p.PosicionFecha,
                Latitud = p.Latitud,
                Longitud = p.Longitud,
                Direccion = p.Direccion
            };

        public DateTime Fecha { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Direccion { get; set; }
    }
}