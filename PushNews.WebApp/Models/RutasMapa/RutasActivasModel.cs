using PushNews.WebApp.Models.Empresas;
using PushNews.WebApp.Models.Hermandades;
using PushNews.WebApp.Models.Rutas;
using System.Collections.Generic;

namespace PushNews.WebApp.Models.RutasMapa
{
    public class RutasActivasModel
    {
        public IEnumerable<RutaModel> Rutas { get; set; }
        public IEnumerable<HermandadModel> Hermandades { get; set; }
        public IEnumerable<EmpresaModel> Patrocinadores { get; set; }
    }
}