using PushNews.Negocio.Interfaces;
using PushNews.WebService.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace PushNews.WebService.Controllers
{
    public class LocalizacionesController : BaseController
    {
        [Route("localizaciones")]
        [HttpPost]
        public async Task<IHttpActionResult> Localizaciones(SolicitudModel model)
        {
            if (!ComprobarClaves(model))
            {
                return Unauthorized();
            }

            ILocalizacionesServicio srv = Servicios.LocalizacionesServicio();
            return Ok((await srv.GetAsync(c => c.Activo))
                          .Select(LocalizacionModel.FromEntity)
                          .AsQueryable());
        }
    }
}