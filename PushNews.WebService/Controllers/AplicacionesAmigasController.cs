using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using PushNews.WebService.Models;
using System.Linq;
using System.Web.Http;

namespace PushNews.WebService.Controllers
{
    public class AplicacionesAmigasController : BaseController
    {

        [Route("aplicacionesamigas/lista")]
        [HttpPost]
        public IHttpActionResult AplicacionesAmigas(SolicitudModel model)
        {
            if (!ComprobarClaves(model))
            {
                return Unauthorized();
            }

            IAplicacionesServicio srv = Servicios.AplicacionesServicio();

            Aplicacion ap = srv.GetBySubdomain(model.Subdominio);

            bool aplicacionTieneAsociados = ap.Caracteristicas.Any(c => c.Nombre == "Asociados");
            if (!aplicacionTieneAsociados || User.Identity.IsAuthenticated)
            {
                return Ok(ap.AplicacionesAmigas.Select(AplicacionAmigaModel.FromEntity).AsQueryable());
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}