using PushNews.WebApp.Controllers;
using PushNews.WebApp.Models.Aplicaciones;
using PushNews.Dominio.Entidades;
using System.Linq;
using System.Web.Mvc;

namespace PushNews.WebApp.Areas.Publicar.Controllers
{
    [Authorize]
    public class PublicarHomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.NombreUsuario = $"{Usuario.Nombre} {Usuario.Apellidos}";
            ViewBag.Aplicaciones = base.Usuario.Aplicaciones.Select(AplicacionGrid.FromEntity);
            return View();
        }
    }
}