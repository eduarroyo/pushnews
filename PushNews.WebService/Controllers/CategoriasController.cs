using PushNews.Negocio.Interfaces;
using PushNews.WebService.Models;
using System.Linq;
using System.Web.Http;

namespace PushNews.WebService.Controllers
{
    public class CategoriasController : BaseController
    {
        [Route("categorias/lista")]
        [HttpPost]
        public IHttpActionResult Categorias(SolicitudModel model)
        {
            log.Debug("Solicitud de lista de categorías." + Newtonsoft.Json.JsonConvert.SerializeObject(model));
            if (!ComprobarClaves(model))
            {
                log.Debug("Acceso no autorizado");
                return Unauthorized();
            }

            ICategoriasServicio srv = Servicios.CategoriasServicio();
            return Ok(srv.ListaCategorias()
                    .Select(CategoriaModel.FromEntity)
                    .AsQueryable());
        }
    }
}