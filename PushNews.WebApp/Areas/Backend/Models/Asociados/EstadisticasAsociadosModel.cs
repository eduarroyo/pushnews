using System;
using System.ComponentModel.DataAnnotations;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Models.Asociados
{
    public class EstadisticasAsociadoModel
    {
        public long AsociadoID { get; set; }
        public long ComunicacionID { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "UltimaConsultaFecha")]
        public DateTime UltimaConsultaFecha { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "TotalConsultas")]
        public int TotalConsultas { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "AsociadoNombre")]
        public string AsociadoNombre { get; set; }
    }
}