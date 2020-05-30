using PushNews.Dominio.Entidades;
using PushNews.WebService.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace PushNews.WebService.Controllers
{
    public class PerfilesAsociadosController : BaseController
    {
        [Authorize, HttpPost, Route("perfil/leer")]
        public async Task<IHttpActionResult> PerfilLeer()
        {
            Asociado asociado = await UserManager.FindByIdAsync(CurrentUserID());
            if (asociado != null)
            {
                return Ok(ModificarAsociadoModel.FromEntity(asociado));
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize, HttpPost, Route("perfil/modificar")]
        public async Task<IHttpActionResult> PerfilModificar(ModificarAsociadoModel model)
        {
            if (ModelState.IsValid)
            {
                Asociado modificar = await UserManager.FindByIdAsync(CurrentUserID());
                model.ActualizarEntidad(modificar);
                var result = await UserManager.UpdateAsync(modificar);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize, HttpPost, Route("perfil/cambiarclave")]
        public async Task<IHttpActionResult> CambiarClaveAsociado(CambiarClaveAsociadoModel model)
        {
            log.Info($"Solicitud modificar clave del asociado {CurrentUserID()}");
            if (!ModelState.IsValid)
            {
                log.Info($"Datos no válidos: {JsonConvert.SerializeObject(ModelState.Values)}");
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(CurrentUserID(), model.OldPassword,
                model.NewPassword);

            if (result.Succeeded)
            {
                log.Info($"Clave cambiada {CurrentUserID()}");
                return Ok();
            }
            else
            {
                log.Info($"Error al cambiar la clave {string.Join("; ", result.Errors.ToArray())}");
                return Json(string.Join("; ", result.Errors.ToArray()));
            }
        }

    }
}
