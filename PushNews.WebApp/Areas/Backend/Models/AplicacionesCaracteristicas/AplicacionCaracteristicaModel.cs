using PushNews.WebApp.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Entity = PushNews.Dominio.Entidades.AplicacionCaracteristica;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Models
{
    public class AplicacionCaracteristicaModel
    {
        public static Func<Entity, AplicacionCaracteristicaModel> FromEntity =
            r => new AplicacionCaracteristicaModel()
            {
                AplicacionCaracteristicaID = (Dominio.Enums.AplicacionCaracteristica) r.AplicacionCaracteristicaID,
                Nombre = Textos.AplicacionCaracteristica((Dominio.Enums.AplicacionCaracteristica) r.AplicacionCaracteristicaID),
                Activo = r.Activo               
            };

        public void ActualizarEntidad(Entity editar)
        {
            editar.Activo = Activo;
        }

        public Dominio.Enums.AplicacionCaracteristica AplicacionCaracteristicaID { get; set; }

        [Display(ResourceType = typeof(Txt.AplicacionesCaracteristicas), Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(Txt.Comun), Name = "Activa")]
        [HiddenInput(DisplayValue = false)]
        public bool Activo { get; set; }
    }
}