using PushNews.WebApp.Areas.Backend.Models.Perfiles;
using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models.Opciones;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PushNews.WebApp.Areas.Backend.Controllers
{
    [Authorize]
    public class PerfilesController : BaseController
    {
        public ActionResult Perfil()
        {
            ViewBag.TipoAplicacion = Aplicacion.Tipo;

            var usuario = Usuario;
            ViewBag.NombreActual = usuario.Nombre;
            ViewBag.ApellidosActuales = usuario.Apellidos;

            return PartialView("Perfil");
        }

        public ActionResult Opciones()
        {
            ViewBag.TipoAplicacion = Aplicacion.Tipo;
            return PartialView("Opciones");
        }

        [HttpPost]
        public async Task<ActionResult> GuardarPerfil(PerfilUsuarioModel model)
        {
            if (ModelState.IsValid)
            {
                IUsuariosServicio uSrv = Servicios.UsuariosServicio();
                long usuarioID = CurrentUserID();
                Usuario modificar = await uSrv.GetSingleAsync(u => u.UsuarioID == usuarioID);

                modificar.Nombre = model.Nombre.Trim();
                modificar.Apellidos = model.Apellidos.Trim();
                await uSrv.ApplyChangesAsync();
                return Json(true);
            }
            else
            {
                return Json(Util.SerializarErroresModelo(ModelState));
            }
        }

        [HttpPost]
        public ActionResult GuardarOpciones(OpcionesModel model)
        {
            return Json(null);
        }

        /// <summary>
        /// Proporciona un modelo para lista de selección de motivos de baja.
        /// </summary>
        [Authorize(Roles = "LeerPerfiles")]
        public ActionResult ListaPerfiles()
        {
            IEnumerable<Perfil> registros = RoleManager.Perfiles();
            // Creamos el modelo de lista de selección a partir del resultado obtenido de la 
            // consulta. Si se ha especificado un país, los grupos se establecen por 
            // Pais/Provincia. Si no, sólo por Provincia.
            var lista = new SelectList(registros, dataValueField: "PerfilID", dataTextField: "Nombre");
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}