using System.Collections.Generic;

namespace PushNews.WebApp.Models.UI
{
    public class HerramientasInlineModel
    {
        public IEnumerable<BotonGridModel> BotonesExtra { get; set; }
        public bool Editar {get ;set;}
        public bool Eliminar {get; set;}
    
        public HerramientasInlineModel(IEnumerable<BotonGridModel> botonesExtra = null)
            : this(editar: true, eliminar: true, botonesExtra: botonesExtra)
        {}
    
        public HerramientasInlineModel(bool editar, bool eliminar, IEnumerable<BotonGridModel> botonesExtra = null)
        {
            Editar = editar;
            Eliminar = eliminar;
            BotonesExtra = botonesExtra == null ? new BotonGridModel[0] : botonesExtra;        
        }
    }
}