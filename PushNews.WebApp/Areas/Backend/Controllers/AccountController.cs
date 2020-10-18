using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using PushNews.Dominio.Entidades;
using PushNews.WebApp.Areas.Backend.Models.Account;
using PushNews.WebApp.Areas.Backend.Models.PlantillasEmail;
using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models.Account;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Claim = System.Security.Claims.Claim;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Areas.Backend.Controllers
{
    public class AccountController : BaseController
    {
        // La acción Authorize es el extremo que se invoca al acceder a cualquier
        // Web API protegida. Si el usuario no ha iniciado sesión, se le redirigirá a 
        // la página Login. Tras iniciar sesión correctamente, podrá invocar una Web API.
        [HttpGet]
        public ActionResult Authorize()
        {
            Claim[] claims = new ClaimsPrincipal(User).Claims.ToArray();
            var identity = new ClaimsIdentity(claims, authenticationType: "Bearer");
            AuthenticationManager.SignIn(identity);
            return new EmptyResult();
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl = "/Backend#/escritorio")
        {
            log.Info("Login solicitado: " + model.Usuario);
            if (ModelState.IsValid)
            {
                Usuario user = await UserManager.FindAsync(model.Usuario, model.Password);
                if (user != null)
                {
                    // El usuario existe pero no está activo: no se hace login y se informa al usuario.
                    if (!user.Activo)
                    {
                        log.Info(message: "Intento de login de usuario desactivado.");
                        AddErrors(new IdentityResult(new[] { "Cuenta de usuario desactivada." }));
                    }

                    // Usuario existe y está activo, pero no pertenece a la aplicación del subdominio.
                    // Se escribe en el log el suceso detallado y al usuario se le indica "usuario o clave no válidos".
                    else if(user.Aplicaciones.All(a => a.AplicacionID != Aplicacion.AplicacionID))
                    {
                        log.Info($"El usuario {user.Email} ({user.UsuarioID}) ha intentado el login contra el subdominio {Aplicacion.SubDominio}, que corresponde a una aplicación que no tiene asignada.");
                        AddErrors(new IdentityResult(new[] { "Nombre de usuario o contraseña no válidos." }));
                    }

                    // Usuario existe, está activo y tiene asignada la aplicación del subdominio.
                    // Login correcto y normal.
                    else
                    {
                        await SignInAsync(user, model.RememberMe);
                        // Fin de proceso de login con éxito.
                        return RedirectToLocal(returnUrl);
                    }
                }

                // Usuario o clave incorrectos.
                else
                {
                    log.Info(message: "Usuario o clave incorrectos.");
                    AddErrors(new IdentityResult(new[] { "Nombre de usuario o contraseña no válidos." }));
                }
            }

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
        }

        //// GET: /Account/Register
        //[AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecuperarClave(RecuperarClaveModel model)
        {
            if (ModelState.IsValid)
            {
                // Obtener el usuario y verificar que existe y pertenece a la aplicación actual.
                Usuario user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || user.Aplicaciones.All(a => a.AplicacionID != Aplicacion.AplicacionID))
                {
                    // Si no existe o no pertenece a la aplicación actual, notificar.
                    return Json(new { resul = false, mensaje = Txt.Account.EmailNoEncontrado });
                }

                // Si el usuario existe y pertenece a la aplicación actual, obtener un token para resetear
                // la clave y enviarlo a la dirección de email.
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                string callbackUrl = Url.Action(actionName: "ResetPassword", controllerName: "Account",
                    routeValues: new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                string logoPath = HttpContext.Server.MapPath("~/Content/Images/PushNews-150.png");
                RestablecerClave modeloEmail = new RestablecerClave(callbackUrl, user.Nombre, logoPath);
                string cuerpo = RenderToString("~/Areas/Backend/Views/PlantillasEmail/RestablecerClave.cshtml", modeloEmail);

                await EmailService.EnviarEmailRecuperarClave(user.Email, cuerpo, modeloEmail);

                return Json(new { resul = true, mensaje = Txt.Account.ClaveReseteada });
            }
            else
            {
                return Json(new { resul = false, mensaje = Util.SerializarErroresModelo(ModelState) });
            }
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (code == null)
            {
                return View(viewName: "Error");
            }
            return View("~/Areas/Backend/Views/Account/ResetPassword.cshtml");
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                Usuario user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(key: "", errorMessage: "No se encontró ningún usuario.");
                    return View("~/Areas/Backend/Views/Account/ResetPassword.cshtml");
                }
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(actionName: "ResetPasswordConfirmation", controllerName: "Account");
                }
                else
                {
                    AddErrors(result);
                    return View("~/Areas/Backend/Views/Account/ResetPassword.cshtml");
                }
            }

            // Si llegamos a este punto, es que se ha producido un error, volver a mostrar el formulario
            return View("~/Areas/Backend/Views/Account/ResetPassword.cshtml", model);
        }

        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View("~/Areas/Backend/Views/Account/ResetPasswordConfirmation.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> CambiarClave(CambiarClaveModel model)
        {
            if (ModelState.IsValid)
            {
                long usuarioID = long.Parse(User.Identity.GetUserId());
                IdentityResult result = 
                    await UserManager.ChangePasswordAsync(usuarioID, model.ClaveActual, model.ClaveNueva);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(long.Parse(User.Identity.GetUserId()));
                    await SignInAsync(user, isPersistent: false);
                    return Json(true);
                }
                else
                {
                    return Json(new { result.Errors });
                }
            }

            return Json(new { Errors = Util.SerializarErroresModelo(ModelState) });
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Remove(name: "Aplicacion");
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "BackendHome");
        }


        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            System.Collections.Generic.IList<UserLoginInfo> linkedAccounts = UserManager.GetLogins(long.Parse(User.Identity.GetUserId()));
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return PartialView(viewName: "_RemoveAccountPartial", model: linkedAccounts);
        }

        #region Aplicaciones auxiliares

        // Se usa para la protección XSRF al agregar inicios de sesión externos
        //private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(Usuario user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(
                new AuthenticationProperties() { IsPersistent = isPersistent }, 
                await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(key: "", errorMessage: error);
            }
        }

        private bool HasPassword()
        {
            Usuario user = UserManager.FindById(long.Parse(User.Identity.GetUserId()));
            if (user != null)
            {
                return user.Clave!= null;
            }
            return false;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            }
        }

        #endregion

    }
}