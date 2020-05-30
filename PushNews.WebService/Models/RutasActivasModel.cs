using System.Collections.Generic;

namespace PushNews.WebService.Models
{
    public class RutasActivasModel
    {
        public IEnumerable<RutaModel> Rutas { get; set; }
        public IEnumerable<HermandadModel> Hermandades { get; set; }
        public IEnumerable<EmpresaModel> Patrocinadores { get; set; }
    }
}