using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using PushNews.WebApp.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PushNews.WebApp.Areas.Backend.Controllers
{
    public class ListasController : BaseController
    {
        // GET: Backend/Listas
        public ActionResult Perfiles()
        {
            IRolesServicio srv = Servicios.RolesServicio();
            List<Rol> roles = srv.Get().ToList();
            return Json(roles, JsonRequestBehavior.AllowGet);
        }
    }
}