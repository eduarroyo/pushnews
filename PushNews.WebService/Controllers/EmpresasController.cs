using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using PushNews.WebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace PushNews.WebService.Controllers
{
    public class EmpresasController : BaseController
    {
        [Route("empresas")]
        [HttpPost]
        public async Task<IHttpActionResult> Empresas(SolicitudModel model)
        {
            if (!ComprobarClaves(model))
            {
                return Unauthorized();
            }


            var ap = Aplicacion(model.Subdominio);
            if(ap.Caracteristicas.All(c => c.Nombre != "Empresas"))
            {
                return Unauthorized();
            }

            IParametrosServicio pSrv = Servicios.ParametrosServicio();
            Parametro pSubdominioBannersAleatorios = pSrv.GetByName("EmpresaBannerAleatorio");
            string subdominioBannersAleatorios = pSubdominioBannersAleatorios?.Valor;

            IEmpresasServicio srv = Servicios.EmpresasServicio();
            List<EmpresaModel> empresas = (await srv.GetAsync(e => e.Activo))
                .Select(EmpresaModel.FromEntity)
                .ToList();

            // Si el subdominio coincide con el parámetro de banners aleatorios, desordenar la
            // lista de empresas para que los banner se muestren en un orden aleatorio.
            if(model.Subdominio == subdominioBannersAleatorios)
            {
                Random rnd = new Random();
                empresas = empresas.OrderBy(i => rnd.Next()).ToList();
            }

            log.Debug("Sirviendo empresas en el siguiente orden: " + string.Join(", ", empresas.Select(e => e.EmpresaID)));
            return Ok(empresas.AsQueryable());
        }
    }
}