using System.ComponentModel.DataAnnotations;
using Txt = PushNews.WebService.App_LocalResources;

namespace PushNews.WebService.Models.ApiClientesExternos
{
    public class EliminarAsociadoModel : SolicitudAsociadosModel
    {
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long AsociadoID { get; set; }
    }
}