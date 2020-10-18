using PushNews.AppceleratorPush;
using PushNews.WebApp.Controllers;
using PushNews.WebApp.Filters;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models;
using PushNews.WebApp.Models.Comunicaciones;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Areas.Backend.Controllers
{
    [Authorize]
    public class ComunicacionesController : BaseController
    {
        public object CategoriasGrid { get; private set; }

        public ComunicacionesController(): base()
        { }

        public ActionResult PruebaExcepcionesGlobales()
        {
            // Para probar captura de excepciones globales.
            throw new Exception("Esta excepción es para probar el filtro global de errores");
        }

        [Authorize(Roles = "LeerComunicaciones")]
        public ActionResult Comunicacion(long comunicacionID)
        {
            ViewBag.ComunicacionID = comunicacionID;
            return PartialView("~/Areas/Backend/Views/Comunicaciones/ComunicacionDetalle.cshtml");
        }

        [Authorize(Roles="LeerComunicaciones")]
        public ActionResult Index()
        {
            ICategoriasServicio srv = Servicios.CategoriasServicio();
            ViewBag.Categorias = srv.Get().Select(CategoriaModel.FromEntity);
            return PartialView("Comunicaciones");
        }

        [Authorize(Roles = "LeerComunicaciones")]
        public async Task<ActionResult> Leer([DataSourceRequest] DataSourceRequest request)
        {
            IComunicacionesServicio srv = Servicios.ComunicacionesServicio();            
            List<ComunicacionGrid> registros = new List<ComunicacionGrid>();
            var query = (await srv.GetAsync(c => !c.Borrado, includeProperties: "Categoria, Adjunto, Accesos"));
            if (User.IsInRole("LeerInfoPush"))
            {
                foreach (var c in query)
                {
                    registros.Add(ComunicacionGrid.FromEntityParaAdmin(c, CategoriasPermitidasIds, PeriodoEnvioPushHoras));
                }
            }
            else
            {
                foreach (var c in query)
                {
                    registros.Add(ComunicacionGrid.FromEntity(c, CategoriasPermitidasIds, PeriodoEnvioPushHoras));
                }
            }

            return Json(registros.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "LeerComunicaciones")]
        public async Task<ActionResult> LeerUno(long comunicacionID)
        {
            IComunicacionesServicio srv = Servicios.ComunicacionesServicio();
            Comunicacion com = await srv.GetSingleAsync(c => c.ComunicacionID == comunicacionID);
            if (com != null)
            {
                return Json(ComunicacionGrid.FromEntity(com, CategoriasPermitidasIds, PeriodoEnvioPushHoras),
                            JsonRequestBehavior.AllowGet);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [Authorize(Roles = "LeerComunicaciones")]
        public async Task<ActionResult> LeerUnoDetalle(long comunicacionID)
        {
            IComunicacionesServicio srv = Servicios.ComunicacionesServicio();
            Comunicacion com = await srv.GetSingleAsync(c => c.ComunicacionID == comunicacionID);
            if (com != null)
            {
                return Json(ComunicacionDetalleModel.FromEntity(com), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Devuelve la lista de publicaciones de la aplicación actual para la web, aplicando posibles
        /// restricciones de filtrado, ordenación y paginado.
        /// </summary>
        /// <returns>DataSourceResult para un componente telerik tipo Grid, ListView o similar.</returns>
        [Authorize(Roles = "LeerComunicaciones")]
        public ActionResult ComunicacionesPublicadas([DataSourceRequest] DataSourceRequest request)
        {
            IComunicacionesServicio srv = Servicios.ComunicacionesServicio();
            IEnumerable<Comunicacion> comunicacionesPublicadas1 = srv.Publicadas().ToList();

            List<ComunicacionGrid> comunicacionesPublicadas = new List<ComunicacionGrid>();
            foreach(var c in comunicacionesPublicadas1)
            {
                comunicacionesPublicadas.Add(ComunicacionGrid.FromEntity(c, CategoriasPermitidasIds, PeriodoEnvioPushHoras));
            }

            return Json(comunicacionesPublicadas.ToDataSourceResult(request),
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "ExportarComunicaciones")]
        public ActionResult ExcelExportSave(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        [Authorize(Roles = "EliminarComunicaciones")]
        public async Task<ActionResult> Eliminar([DataSourceRequest] DataSourceRequest request, ComunicacionGrid model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            try
            {
                var srv = Servicios.ComunicacionesServicio();
                var eliminar = srv.GetSingle(p => p.ComunicacionID == model.ComunicacionID);
                if (eliminar != null)
                {
                    long? documentoEliminar = null, imagenEliminar = null;
                    if (model.AdjuntoDocumentoID != null)
                    {
                        documentoEliminar = eliminar.AdjuntoDocumentoID;
                    }
                    if (model.ImagenDocumentoID != null)
                    {
                        imagenEliminar = eliminar.ImagenDocumentoID;
                    }
                    srv.Delete(eliminar);
                    await srv.ApplyChangesAsync();

                    // La eliminación de documentos (registro de la tabla Documentos y fichero de  disco) se
                    // realiza después de haber persistido la eliminación de la comunicación para garantizar
                    // que la comunicación se elimina aunque falle la eliminación de ficheros en disco. Si
                    // hubiera fallos de disco, quedaría el registro de documento sin ninguna comunicación 
                    // que lo enlace, así como el archivo en disco.
                    
                    if (documentoEliminar.HasValue)
                    {
                        try
                        {
                            await FileManager.EliminarDocumento(documentoEliminar.Value);
                        }
                        catch(Exception e)
                        {
                            log.Error($"Error al eliminar documento {documentoEliminar.Value} durante la eliminación de su comunicación asociada.", e);
                        }
                    }
                    if (imagenEliminar.HasValue)
                    {
                        try
                        {
                            await FileManager.EliminarDocumento(imagenEliminar.Value);
                        }
                        catch (Exception e)
                        {
                            log.Error($"Error al eliminar imagen {imagenEliminar.Value} durante la eliminación de su comunicación asociada.", e);
                        }
                        
                    }
                }
                else
                {
                    log.Debug("Eliminar parámetro: el parámetro con id=" + model.ComunicacionID + " no existe.");
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Comunicaciones.ArtEntidad).Frase() };
                }
            }
            catch (Exception e)
            {
                log.Error("Error al eliminar el parámetro con id=" + model.ComunicacionID, e);
                result.Errors = new[] { string.Format(Txt.ErroresComunes.Eliminar, Txt.Comunicaciones.ArtEntidad).Frase() };
            }
            return Json(result);
        }

        [Authorize(Roles = "ModificarComunicaciones")]
        public ActionResult Editar(long? comunicacionID = null)
        {
            ViewBag.ComunicacionID = comunicacionID ?? 0;
            return PartialView("~/Areas/Backend/Views/Comunicaciones/Editar.cshtml");
        }

        [HttpPost]
        [Authorize(Roles = "ModificarComunicaciones")]
        public async Task<ActionResult> Modificar([DataSourceRequest] DataSourceRequest request, ComunicacionGrid model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);

            if (ModelState.IsValid)
            {
                // Validar la categoría escogida por el usuario
                if (CategoriaPermitida(model.CategoriaID))
                {
                    try
                    {
                        IAplicacionesServicio appSrv = Servicios.AplicacionesServicio();
                        Aplicacion app = appSrv.GetSingle(c => c.AplicacionID == Aplicacion.AplicacionID);
                        bool mantenerUbicacion = app.Caracteristicas.Any(c => c.Nombre == "GeoPosicionamiento"); 
                        var srv = Servicios.ComunicacionesServicio();
                        var modificar = srv.GetSingle(p => p.ComunicacionID == model.ComunicacionID);
                        if (modificar != null)
                        {
                            // Si el usuario que edita la comunicación no tiene limitadas las categorías o 
                            // bien tiene asignada la que tenía la comunicación originalmente y es una 
                            // categoría activa, se permite la modificación.
                            if (CategoriaPermitida(modificar.CategoriaID))
                            {
                                // Guardar en variables aparte los ids de adjunto e imagen, por si hay que 
                                // eliminarlos.
                                long? documentoEliminar = null, imagenEliminar = null;
                                if (model.AdjuntoDocumentoID == null)
                                {
                                    documentoEliminar = modificar.AdjuntoDocumentoID;
                                }
                                if (model.ImagenDocumentoID == null)
                                {
                                    imagenEliminar = modificar.ImagenDocumentoID;
                                }
                                model.ActualizarEntidad(modificar, mantenerUbicacion);
                                await srv.ApplyChangesAsync();
                                result = new[]
                                {
                                    ComunicacionGrid.FromEntity(modificar, CategoriasPermitidasIds, PeriodoEnvioPushHoras)
                                }.ToDataSourceResult(request, ModelState);

                                // La eliminación de documentos (registro de la tabla Documentos y fichero de 
                                // disco) se realiza después de haber actualizado la comunicación (poner FK a 
                                // null) para garantizar que la comunicación se actualiza aunque falle la 
                                // la eliminación en disco. Si hubiera fallos, quedaría el registro de documento
                                // sin ninguna comunicación que lo enlace, así como el archivo en disco.
                                if (documentoEliminar.HasValue)
                                {
                                    try
                                    {
                                        await FileManager.EliminarDocumento(documentoEliminar.Value);
                                    }
                                    catch (Exception e)
                                    {
                                        log.Error($"Error al eliminar documento {documentoEliminar.Value} durante la modificación de su comunicación asociada.", e);
                                    }
                                }
                                if (imagenEliminar.HasValue)
                                {
                                    try
                                    {
                                        await FileManager.EliminarDocumento(imagenEliminar.Value);
                                    }
                                    catch (Exception e)
                                    {
                                        log.Error($"Error al eliminar imagen {imagenEliminar.Value} durante la modificación de su comunicación asociada.", e);
                                    }
                                }
                            }
                            else
                            {
                                result.Errors = new[] { string.Format(Txt.Comunicaciones.ModificacionDenegadaPorCategoria, modificar.Categoria.Nombre) };
                            }
                        }
                        else
                        {
                            result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Comunicaciones.ArtEntidad).Frase() };
                        }
                    }
                    catch (Exception e)
                    {
                        log.Error("Error al modificar la comunicación con id=" + model.ComunicacionID, e);
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar, Txt.Comunicaciones.ArtEntidad).Frase() };
                    }
                }
                else
                {
                    log.Warn($"El usuario {Usuario.Nombre} ({Usuario.UsuarioID}) ha intentado modificar la comunicación {model.ComunicacionID} con la categoría {model.CategoriaID} que no tiene asignada.");
                    result.Errors = new[] { Txt.Comunicaciones.CategoriaNoPermitida };
                }
            }

            return Json(result);
        }
        
        [HttpPost]
        [Authorize(Roles = "ModificarComunicaciones")]
        public async Task<ActionResult> ActivarDesactivar(long comunicacionId)
        {
            try
            {
                var srv = Servicios.ComunicacionesServicio();
                var modificar = await srv.GetSingleAsync(p => p.ComunicacionID == comunicacionId);
                if (modificar != null)
                {
                    // Validar la categoría escogida por el usuario
                    if (CategoriaPermitida(modificar.CategoriaID))
                    {
                        modificar.Activo = !modificar.Activo;
                        await srv.ApplyChangesAsync();
                        return Json(true);
                    }
                    else
                    {
                        log.Warn($"Solicitada activación/desactivación de una comunicación de una categoría no autorizada al usuario. Usuario: {Usuario.ApellidosNombre}, Comunicación: {comunicacionId}, Categoría: {modificar.Categoria.Nombre}.");
                        return Json(new { Error = Txt.Comunicaciones.CategoriaNoPermitida });
                    }
                }
                else
                {
                    log.Warn("Solicitada activación/desactivación de una comunicación inexistente o que no pertenece a la aplicación de trabajo.");
                    return Json(new { Error = Util.Frase(string.Format(Txt.ErroresComunes.NoExiste, Txt.Comunicaciones.ArtEntidad)) });
                }
            }
            catch(Exception e)
            {
                log.Error($"Error al activar/desactivar la comunicación {comunicacionId}.", e);
                return Json(new { Error = string.Format(Txt.ErroresComunes.ActivarDesactivar, Txt.Comunicaciones.ArtEntidad) });
            }
        }

        [HttpPost]
        [Authorize(Roles = "CrearComunicaciones")]
        public async Task<ActionResult> Nuevo([DataSourceRequest] DataSourceRequest request, ComunicacionGrid model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                // Validar la categoría escogida por el usuario
                if (CategoriaPermitida(model.CategoriaID))
                {
                    try
                    {
                        IAplicacionesServicio appSrv = Servicios.AplicacionesServicio();
                        Aplicacion app = appSrv.GetSingle(c => c.AplicacionID == Aplicacion.AplicacionID);
                        bool mantenerUbicacion = app.Caracteristicas.Any(c => c.Nombre == "GeoPosicionamiento");
                        var srv = Servicios.ComunicacionesServicio();
                        var nuevo = srv.Create();
                        model.ActualizarEntidad(nuevo, mantenerUbicacion);
                        nuevo.UsuarioID = CurrentUserID();
                        nuevo.Instantanea = model.FechaPublicacion < DateTime.Now;
                        srv.Insert(nuevo);
                        await srv.ApplyChangesAsync();
                        if (nuevo.Instantanea)
                        {
                            if (EnviarPush(nuevo))
                            {
                                nuevo.PushEnviada = true;
                                nuevo.PushFecha = DateTime.Now;
                                await srv.ApplyChangesAsync();
                            }
                            else
                            {
                                nuevo.Instantanea = false;
                                await srv.ApplyChangesAsync();
                            }
                        }

                        result = new[] 
                        {
                            ComunicacionGrid.FromEntity(nuevo, CategoriasPermitidasIds, PeriodoEnvioPushHoras)
                        }.ToDataSourceResult(request, ModelState);
                    }
                    catch (Exception e)
                    {
                        log.Error("Error al añadir la comunicación.", e);
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.Aniadir, Txt.Comunicaciones.ArtEntidad).Frase() };
                    }
                }
                else
                {
                    log.Warn($"El usuario {Usuario.Nombre} ({Usuario.UsuarioID}) ha intentado crear una comunicación con la categoría {model.CategoriaID} que no tiene asignada.");
                    result.Errors = new[] { Txt.Comunicaciones.CategoriaNoPermitida };
                }
            }

            return Json(result);
        }
        
        [HttpPost]
        [Authorize(Roles = "ModificarComunicaciones")]
        public async Task<ActionResult> CrearModificar(ComunicacionGrid model)
        {
            object result;
            try
            {
                if (ModelState.IsValid)
                {
                    // Validar la categoría escogida por el usuario
                    if (CategoriaPermitida(model.CategoriaID))
                    {
                        IAplicacionesServicio appSrv = Servicios.AplicacionesServicio();
                        Aplicacion app = appSrv.GetSingle(c => c.AplicacionID == Aplicacion.AplicacionID);
                        bool mantenerUbicacion = app.Caracteristicas.Any(c => c.Nombre == "GeoPosicionamiento"); 
                        var srv = Servicios.ComunicacionesServicio();
                        if (model.ComunicacionID != 0)
                        {
                            var modificar = srv.GetSingle(p => p.ComunicacionID == model.ComunicacionID);
                            if (modificar != null)
                            {
                                // Verificar que el usuario está autorizado a gestionar la comunicación,
                                // por la categoría original a la que pertenece.
                                if (CategoriaPermitida(modificar.CategoriaID))
                                {
                                    // Guardar en variables aparte los ids de adjunto e imagen, por si hay que 
                                    // eliminarlos.
                                    long? documentoEliminar = null, imagenEliminar = null;
                                    if (model.AdjuntoDocumentoID == null)
                                    {
                                        documentoEliminar = modificar.AdjuntoDocumentoID;
                                    }
                                    if (model.ImagenDocumentoID == null)
                                    {
                                        imagenEliminar = modificar.ImagenDocumentoID;
                                    }
                                    model.ActualizarEntidad(modificar, mantenerUbicacion);
                                    await srv.ApplyChangesAsync();
                                    result = ComunicacionGrid.FromEntity(modificar, CategoriasPermitidasIds, PeriodoEnvioPushHoras);

                                    // La eliminación de documentos (registro de la tabla Documentos y fichero de 
                                    // disco) se realiza después de haber actualizado la comunicación (poner FK a 
                                    // null) para garantizar que la comunicación se actualiza aunque falle la 
                                    // la eliminación en disco. Si hubiera fallos, quedaría el registro de documento
                                    // sin ninguna comunicación que lo enlace, así como el archivo en disco.
                                    if (documentoEliminar.HasValue)
                                    {
                                        try
                                        {
                                            await FileManager.EliminarDocumento(documentoEliminar.Value);
                                        }
                                        catch (Exception e)
                                        {
                                            log.Error($"Error al eliminar documento {documentoEliminar.Value} durante la modificación de su comunicación asociada.", e);
                                        }
                                    }
                                    if (imagenEliminar.HasValue)
                                    {
                                        try
                                        {
                                            await FileManager.EliminarDocumento(imagenEliminar.Value);
                                        }
                                        catch (Exception e)
                                        {
                                            log.Error($"Error al eliminar imagen {imagenEliminar.Value} durante la modificación de su comunicación asociada.", e);
                                        }
                                    }
                                }
                                else
                                {
                                    result = new
                                    {
                                        Errors = new[]
                                        {
                                            string.Format(Txt.Comunicaciones.ModificacionDenegadaPorCategoria, modificar.Categoria.Nombre)
                                        }
                                    };
                                }
                            }
                            else
                            {
                                result = new
                                {
                                    Errors = new[]
                                    {
                                        string.Format(Txt.ErroresComunes.NoExiste, Txt.Comunicaciones.ArtEntidad).Frase()
                                    }
                                };
                            }
                        }
                        else
                        {
                            var nuevo = srv.Create();
                            model.ActualizarEntidad(nuevo, mantenerUbicacion);
                            nuevo.UsuarioID = CurrentUserID();
                            nuevo.Instantanea = model.FechaPublicacion < DateTime.Now;
                            srv.Insert(nuevo);
                            await srv.ApplyChangesAsync();
                            result = ComunicacionGrid.FromEntity(nuevo, CategoriasPermitidasIds, PeriodoEnvioPushHoras);
                            if (nuevo.Instantanea)
                            {
                                if (EnviarPush(nuevo))
                                {
                                    nuevo.PushEnviada = true;
                                    nuevo.PushFecha = DateTime.Now;
                                    await srv.ApplyChangesAsync();
                                }
                                else
                                {
                                    nuevo.Instantanea = false;
                                    await srv.ApplyChangesAsync();
                                }
                            }
                        }
                    }
                    else
                    {
                        log.Warn($"El usuario {Usuario.Nombre} ({Usuario.UsuarioID}) ha intentado crear o modificar una comunicación estableciendo la categoría {model.CategoriaID} que no tiene asignada.");
                        result = new { Errors = new[] { Txt.Comunicaciones.CategoriaNoPermitida } };
                    }
                }
                else
                {
                    // TODO: Serializar a result.Errors los errores del modelo.
                    result = new { Errors = new[] { "Los datos de la comunicación no son válidos. " } };
                }
            }
            catch (Exception e)
            {
                string accion = model.ComunicacionID != 0 ? "modificar" : "añadir";
                log.Error($"Error al {accion} la comunicación.", e);
                result = new { Errors = new[] { string.Format(Txt.ErroresComunes.Aniadir, Txt.Comunicaciones.ArtEntidad).Frase() } };
            }

            return Json(result);
        }

        /// <summary>
        /// Proporciona un modelo para lista de selección de motivos de baja.
        /// </summary>
        [Authorize(Roles = "LeerComunicaciones")]
        public ActionResult ListaComunicaciones()
        {
            var srv = Servicios.ComunicacionesServicio();
            var consulta = srv.Get();
            var registros = consulta.Select(l => ComunicacionGrid.FromEntity(l, CategoriasPermitidasIds, PeriodoEnvioPushHoras));

            // Creamos el modelo de lista de selección a partir del resultado obtenido de la 
            // consulta. Si se ha especificado un país, los grupos se establecen por 
            // Pais/Provincia. Si no, sólo por Provincia.
            var lista = new SelectList(registros, "ComunicacionID", "Nombre");
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        private bool EnviarPush(Comunicacion comunicacion)
        {
            Aplicacion aplicacion = comunicacion.Categoria.Aplicacion;

            // No se envía push si no
            if(string.IsNullOrEmpty(Aplicacion.CloudKey)
                || string.IsNullOrEmpty(Aplicacion.Usuario)
                || string.IsNullOrEmpty(Aplicacion.Clave))
            {
                log.Info($"No se enviará push de la comunicación {comunicacion.ComunicacionID} porque la aplicación {aplicacion.Nombre} no tiene los datos de configuración del servicio push (CloudKey, Usuario, Clave).");
                return false;
            }

            try
            {
                // Obtener el canal, según si la comunicación pertenece a una categoría pública o privada.
                IParametrosServicio pSrv = Servicios.ParametrosServicio();
                string claveCanal = "CanalPush";
                Parametro canal = pSrv.GetByName(claveCanal);
                if(canal == null || string.IsNullOrWhiteSpace(canal.Valor))
                {
                    log.Info($"No se enviará push de la comunicación {comunicacion.ComunicacionID} porque el parámetro {claveCanal} no existe o no tiene valor.");
                    return false;
                }

                string descripcion = comunicacion.Descripcion.Length > 50
                    ? comunicacion.Descripcion.Substring(0, 50) + "..."
                    : comunicacion.Descripcion;
                using (CloudPush push = new CloudPush(Aplicacion.CloudKey, Aplicacion.Usuario, Aplicacion.Clave))
                {
                    Respuesta respuesta = push.EnviarMensaje(comunicacion.Titulo, descripcion, true, canal.Valor,
                        comunicacion.ComunicacionID.ToString());
                    if (respuesta.Meta.Code != 200)
                    {
                        log.Info($"Comunicacion: {comunicacion.ComunicacionID} - El servidor devolvió un error al solicitar push: Código: {respuesta.Meta.Code} Estado: {respuesta.Meta.Status} Mensaje: {respuesta.Meta.Message} Método: {respuesta.Meta.Method_Name}.");
                        return false;
                    }
                    else
                    {
                        log.Info($"Comunicacion: {comunicacion.ComunicacionID} - Push OK");
                        log.Info($"CloudKey: {Aplicacion.CloudKey}");
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("Error en push", e);
                return false;
            }
        }

        // Categorías activas de la apliación actual asociadas al usuario.
        private List<Categoria> _categoriasUsuario = null;

        /// <summary>
        /// Indica si una categoría está permitida para un usuario.
        /// </summary>
        /// <param name="categoriaId">Identificador de la categoría a verificar.</param>
        /// <returns></returns>
        private bool CategoriaPermitida(long categoriaId)
        {
            return SinRestricciones || CategoriasPermitidas.Any(c => c.CategoriaID == categoriaId);
        }

        /// <summary>
        /// Proporciona la lista de categorías permitidas para el usuario. Devuelve null si todas las 
        /// categorías están permitidas.
        /// </summary>
        private IEnumerable<Categoria> CategoriasPermitidas
        {
            get
            {
                if (_categoriasUsuario == null)
                {
                    if (SinRestricciones)
                    {
                        _categoriasUsuario = null;
                    }
                    else
                    {
                        // Categorías asociadas al usuario, que estén activas y que pertenezcan al a aplicación actual.
                        _categoriasUsuario = Usuario.Categorias
                                .Where(c => c.AplicacionID == Aplicacion.AplicacionID && c.Activo)
                                .ToList();
                    }
                }

                return _categoriasUsuario;
            }
        }

        private IEnumerable<long> CategoriasPermitidasIds
        {
            get
            {
                return CategoriasPermitidas == null ? null : CategoriasPermitidas.Select(c => c.CategoriaID).ToArray();
            }
        }

        private bool SinRestricciones
        {
            get
            {
                // Es administrador o no tiene categorías de la aplicación (ni activas ni inactivas).
                return !Usuario.Categorias.Any(c => c.AplicacionID == Aplicacion.AplicacionID)
                        || Usuario.Perfiles.Any(p => p.Nombre == "Administrador");
            }
        }
    }
}