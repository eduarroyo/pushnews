using PushNews.WebApp.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Entity = PushNews.Dominio.Entidades.Ruta;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Models.Rutas
{
    public class RutaModel
    {
        public static Func<Entity, RutaModel> FromEntity =
            c => new RutaModel
            {
                // Datos de identificación de la ruta
                RutaID = c.RutaID,
                HermandadID = c.HermandadID,
                Descripcion = c.Descripcion,

                // Datos de inicio de la ruta
                InicioFecha = c.InicioFecha,
                InicioDescripcion = c.InicioDescripcion,
                InicioLatitud = c.InicioLatitud,
                InicioLongitud = c.InicioLongitud,

                // Datos de entrada en carrera oficial
                EntradaEnCarreraOficial = c.EntradaEnCarreraOficial,
                EntradaEnCarreraOficialLatitud = c.EntradaEnCarreraOficialLatitud,
                EntradaEnCarreraOficialLongitud = c.EntradaEnCarreraOficialLongitud,

                // Datos de fin de la ruta
                FinFecha = c.FinFecha,
                FinDescripcion = c.FinDescripcion,
                FinLatitud = c.FinLatitud,
                FinLongitud = c.FinLongitud,

                // Datos de la cabeza de la ruta
                GpsCabezaID = c.GpsCabezaID,
                CabezaUltimaPosicionFecha = c.CabezaUltimaPosicionFecha,
                CabezaUltimaPosicionDireccion = c.CabezaUltimaPosicionDireccion,
                CabezaUltimaPosicionLatitud = c.CabezaUltimaPosicionLatitud,
                CabezaUltimaPosicionLongitud = c.CabezaUltimaPosicionLongitud,

                // Datos de la cola de la ruta
                GpsColaID = c.GpsColaID,
                ColaUltimaPosicionFecha = c.ColaUltimaPosicionFecha,
                ColaUltimaPosicionDireccion = c.ColaUltimaPosicionDireccion,
                ColaUltimaPosicionLatitud = c.ColaUltimaPosicionLatitud,
                ColaUltimaPosicionLongitud = c.ColaUltimaPosicionLongitud,

                // Datos calculados de la ruta
                CalculoDistancia = c.CalculoDistancia,
                CalculoTiempo = c.CalculoTiempo,
                CalculoVelocidad = c.CalculoVelocidad,

                // Datos obtenidos de propiedades de navegación
                Hermandad = c.Hermandad.Nombre,
                GpsCabezaMatricula = c.GpsCabeza.Matricula,
                GpsCabezaApiID = c.GpsCabeza.GpsApiID,
                                GpsColaMatricula = c.GpsCola?.Matricula ?? "",
                GpsColaApiID = c.GpsCola?.GpsApiID,
            };

        public void ActualizarEntidad(Entity modificar)
        {
            modificar.HermandadID = HermandadID;
            modificar.Descripcion = Util.AsegurarNulos(Descripcion);

            // Datos de inicio de la ruta
            modificar.InicioFecha = TimeZoneInfo.ConvertTime(InicioFecha,
                    TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"),
                    TimeZoneInfo.Local);
            modificar.InicioDescripcion = Util.AsegurarNulos(InicioDescripcion);

            // Datos de entrada en carrera oficial
            modificar.EntradaEnCarreraOficial = Util.AsegurarNulos(EntradaEnCarreraOficial);
            modificar.EntradaEnCarreraOficialLatitud = EntradaEnCarreraOficialLatitud;
            modificar.EntradaEnCarreraOficialLongitud = EntradaEnCarreraOficialLongitud;

            // Datos de fin de la ruta
            modificar.FinFecha = FinFecha.HasValue
                ? TimeZoneInfo.ConvertTime(FinFecha.Value,
                    TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"),
                    TimeZoneInfo.Local)
                : (DateTime?) null;
            modificar.FinDescripcion = Util.AsegurarNulos(FinDescripcion);

            // Seleccion de GPS de cabeza y cola.
            modificar.GpsCabezaID = GpsCabezaID;
            modificar.GpsColaID = GpsColaID;
        }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long RutaID { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "Hermandad")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long HermandadID { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "Hermandad")]
        public string Hermandad { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "Descripcion")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Descripcion { get; set; }



        [Display(ResourceType = typeof(Txt.Rutas), Name = "InicioFecha")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        public DateTime InicioFecha { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "InicioDescripcion")]
        [StringLength(200, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string InicioDescripcion { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "InicioLatitud")]
        public double InicioLatitud { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "InicioLongitud")]
        public double InicioLongitud { get; set; }



        [Display(ResourceType = typeof(Txt.Rutas), Name = "EntradaEnCarreraOficial")]
        [StringLength(200, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string EntradaEnCarreraOficial { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "EntradaEnCarreraOficialLatitud")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(-90, 90, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Intervalo")]
        public double EntradaEnCarreraOficialLatitud { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "EntradaEnCarreraOficialLongitud")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(-180, 180, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Intervalo")]
        public double EntradaEnCarreraOficialLongitud { get; set; }


        [Display(ResourceType = typeof(Txt.Rutas), Name = "FinFecha")]
        public DateTime? FinFecha { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "FinDescripcion")]
        [StringLength(200, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string FinDescripcion { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "FinLatitud")]
        public double FinLatitud { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "FinLongitud")]
        public double FinLongitud { get; set; }


        [Display(ResourceType = typeof(Txt.Rutas), Name = "GpsCabeza")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long GpsCabezaID { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "GpsCabezaMatricula")]
        public string GpsCabezaMatricula { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "GpsCabezaApiID")]
        public long GpsCabezaApiID { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "CabezaUltimaPosicionFecha")]
        public DateTime? CabezaUltimaPosicionFecha { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "CabezaUltimaPosicionDireccion")]
        public string CabezaUltimaPosicionDireccion { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "CabezaUltimaPosicionLatitud")]
        public double? CabezaUltimaPosicionLatitud { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "CabezaUltimaPosicionLongitud")]
        public double? CabezaUltimaPosicionLongitud { get; set; }


        [Display(ResourceType = typeof(Txt.Rutas), Name = "GpsCola")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long? GpsColaID { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "GpsColaMatricula")]
        public string GpsColaMatricula { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "GpsColaApiID")]
        public long? GpsColaApiID { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "ColaUltimaPosicionFecha")]
        public DateTime? ColaUltimaPosicionFecha { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "ColaUltimaPosicionDireccion")]
        public string ColaUltimaPosicionDireccion { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "ColaUltimaPosicionLatitud")]
        public double? ColaUltimaPosicionLatitud { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "ColaUltimaPosicionLongitud")]
        public double? ColaUltimaPosicionLongitud { get; set; }


        [Display(ResourceType = typeof(Txt.Rutas), Name = "CalculoDistancia")]
        public double CalculoDistancia { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "CalculoTiempo")]
        public TimeSpan? CalculoTiempo { get; set; }

        [Display(ResourceType = typeof(Txt.Rutas), Name = "CalculoVelocidad")]
        public double CalculoVelocidad { get; set; }

    }
}