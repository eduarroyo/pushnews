using PushNews.WebApp.Controllers;
using PushNews.WebApp.Models.Aplicaciones;
using PushNews.Dominio.Entidades;
using System.Linq;
using System.Web.Mvc;

namespace PushNews.WebApp.Areas.Backend.Controllers
{
    [Authorize]
    public class BackendHomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.NombreUsuario = $"{Usuario.Nombre} {Usuario.Apellidos}";
            ViewBag.Aplicaciones = base.Usuario.Aplicaciones.Select(AplicacionGrid.FromEntity);
            return View();
        }
    }
}