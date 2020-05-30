using PushNews.Negocio.Interfaces;
using PushNews.WebService.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace PushNews.WebService.Controllers
{
    public class TelefonosController : BaseController
    {
        [Route("telefonos")]
        [HttpPost]
        public async Task<IHttpActionResult> Telefonos(SolicitudModel model)
        {
            if (!ComprobarClaves(model))
            {
                return Unauthorized();
            }

            ITelefonosServicio srv = Servicios.TelefonosServicio();
            return Ok((await srv.GetAsync(c => c.Activo))
                    .Select(TelefonoModel.FromEntity)
                    .AsQueryable());
        }
    }
}