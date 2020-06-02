using PushNews.WebApp.Areas.Backend.Models.Account;
using PushNews.WebApp.Areas.Backend.Models.PlantillasEmail;
using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models.Account;
using PushNews.Dominio.Entidades;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
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

        // POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new Usuario() { UserName = model.Usuario, Email = model.Email };
        //        IdentityResult result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            await SignInAsync(user, isPersistent: false);

        //            // Para obtener más información sobre cómo habilitar la confirmación de la cuenta y el restablecimiento de la contraseña, visite http://go.microsoft.com/fwlink/?LinkID=320771
        //            // Enviar un correo electrónico con este vínculo
        //            //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //            //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //            //await UserManager.SendEmailAsync(user.Id, "Confirmar la cuenta", "Para confirmar su cuenta, haga clic <a href=\"" + callbackUrl + "\">aquí</a>");

        //            return RedirectToAction(actionName: "Index", controllerName: "ParametrosHome");
        //        }
        //        else
        //        {
        //            AddErrors(result);
        //        }
        //    }

        //    // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
        //    return View(model);
        //}

        // GET: /Account/ConfirmEmail
        //[AllowAnonymous]
        //public async Task<ActionResult> ConfirmEmail(long userId, string code)
        //{
        //    if (userId <= 0 || code == null)
        //    {
        //        return View("Error");
        //    }

        //    IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
        //    if (result.Succeeded)
        //    {
        //        return View("ConfirmEmail");
        //    }
        //    else
        //    {
        //        AddErrors(result);
        //        return View();
        //    }
        //}

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //// POST: /Account/ForgotPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Usuario user = await UserManager.FindByNameAsync(model.Email);
        //        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
        //        {
        //            ModelState.AddModelError(key: "", errorMessage: "El usuario no existe o no se ha confirmado.");
        //            return View();
        //        }

        //        // Para obtener más información sobre cómo habilitar la confirmación de la cuenta y
        //        // el restablecimiento de la contraseña, visite 
        //        // http://go.microsoft.com/fwlink/?LinkID=320771
        //        // Enviar un correo electrónico con este vínculo
        //        string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //        string callbackUrl = Url.Action(actionName: "ResetPassword", controllerName: "Account", routeValues: new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //        await UserManager.SendEmailAsync(user.Id, Txt.Account.RestablecerClaveEmailAsunto,
        //            string.Format(Txt.Account.RestablecerClaveEmailCuerpo, callbackUrl));
        //        return RedirectToAction(actionName: "ForgotPasswordConfirmation", controllerName: "Account");
        //    }

        //    // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
        //    return View(model);
        //}

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

        //// POST: /Account/Disassociate
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        //{
        //    ManageMessageId? message = null;
        //    IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
        //    if (result.Succeeded)
        //    {
        //        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //        await SignInAsync(user, isPersistent: false);
        //        message = ManageMessageId.RemoveLoginSuccess;
        //    }
        //    else
        //    {
        //        message = ManageMessageId.Error;
        //    }
        //    return RedirectToAction("Manage", new { Message = message });
        //}
        /**/

        // GET: /Account/Manage
        //public ActionResult Manage(ManageMessageId? message)
        //{
        //    ViewBag.StatusMessage =
        //        message == ManageMessageId.ChangePasswordSuccess ? "La contraseña se ha cambiado."
        //        : message == ManageMessageId.SetPasswordSuccess ? "La contraseña se ha establecido."
        //        : message == ManageMessageId.RemoveLoginSuccess ? "El inicio de sesión externo se ha quitado."
        //        : message == ManageMessageId.Error ? "Se produjo un error."
        //        : "";
        //    ViewBag.HasLocalPassword = HasPassword();
        //    ViewBag.ReturnUrl = Url.Action(actionName: "Manage");
        //    return View();
        //}

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

        // POST: /Account/Manage
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Manage(ManageUserViewModel model)
        //{
        //    bool hasPassword = HasPassword();
        //    ViewBag.HasLocalPassword = hasPassword;
        //    ViewBag.ReturnUrl = Url.Action(actionName: "Manage");
        //    if (hasPassword)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            //IdentityResult result = await UserManager.ChangePasswordAsync(long.Parse(User.Identity.GetUserId()), model.OldPassword, model.NewPassword);
        //            //if (result.Succeeded)
        //            //{
        //            //    var user = await UserManager.FindByIdAsync(long.Parse(User.Identity.GetUserId()));
        //            //    await SignInAsync(user, isPersistent: false);
        //            //    return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
        //            //}
        //            //else
        //            //{
        //            //    AddErrors(result);
        //            //}
        //        }
        //    }
        //    else
        //    {
        //        // El usuario no tiene contraseña. Quite los errores de validación producidos porque falta un campo OldPassword
        //        ModelState state = ModelState["OldPassword"];
        //        if (state != null)
        //        {
        //            state.Errors.Clear();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            IdentityResult result = await UserManager.AddPasswordAsync(long.Parse(User.Identity.GetUserId()), model.NewPassword);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction(actionName: "Manage", routeValues: new { Message = ManageMessageId.SetPasswordSuccess });
        //            }
        //            else
        //            {
        //                AddErrors(result);
        //            }
        //        }
        //    }

        //    // Si llegamos a este punto, es que se ha producido un error , volver a mostrar el formulario
        //    return View(model);
        //}

        //// POST: /Account/ExternalLogin
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    // Solicitar redireccionamiento al proveedor de inicio de sesión externo
        //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}
        /**/

        //// GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    // Si el usuario ya tiene un inicio de sesión, iniciar sesión del usuario con este proveedor de inicio de sesión externo
        //    var user = await UserManager.FindAsync(loginInfo.Login);
        //    if (user != null)
        //    {
        //        await SignInAsync(user, isPersistent: false);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    else
        //    {
        //        // Si el usuario no tiene ninguna cuenta, solicitar que cree una
        //        ViewBag.ReturnUrl = returnUrl;
        //        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //    }
        //}
        /**/

        //// POST: /Account/LinkLogin
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LinkLogin(string provider)
        //{
        //    // Solicitar redirección a proveedor de inicio de sesión externo para vincular un inicio de sesión para el usuario actual
        //    return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        //}
        /**/

        //// GET: /Account/LinkLoginCallback
        //public async Task<ActionResult> LinkLoginCallback()
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        //    }
        //    IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("Manage");
        //    }
        //    return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        //}
        /**/

        // POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Obtener datos del usuario del proveedor de inicio de sesión externo
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new ApplicationUser() { UserName = model.Email, Email = model.Email, Hometown = model.Hometown };
        //        IdentityResult result = await UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInAsync(user, isPersistent: false);

        //                // Para obtener más información sobre cómo habilitar la confirmación de la cuenta y el restablecimiento de la contraseña, visite http://go.microsoft.com/fwlink/?LinkID=320771
        //                // Enviar un correo electrónico con este vínculo
        //                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //                // SendEmail(user.Email, callbackUrl, "Confirmar cuenta", "Haga clic en este vínculo para confirmar la cuenta");

        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}
        /**/

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Remove(name: "Aplicacion");
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "BackendHome");
        }

        //// GET: /Account/ExternalLoginFailure
        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}
        /**/

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

        //private void SendEmail(string email, string callbackUrl, string subject, string message)
        //{
        //    // Para obtener información para enviar correo, visite http://go.microsoft.com/fwlink/?LinkID=320771
        //}

        //public enum ManageMessageId
        //{
        //    ChangePasswordSuccess,
        //    SetPasswordSuccess,
        //    RemoveLoginSuccess,
        //    Error
        //}

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

        //private class ChallengeResult : HttpUnauthorizedResult
        //{
        //    public ChallengeResult(string provider, string redirectUri)
        //        : this(provider, redirectUri, null)
        //    {
        //    }

        //    public ChallengeResult(string provider, string redirectUri, string userId)
        //    {
        //        LoginProvider = provider;
        //        RedirectUri = redirectUri;
        //        UserId = userId;
        //    }

        //    public string LoginProvider { get; set; }
        //    public string RedirectUri { get; set; }
        //    public string UserId { get; set; }

        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
        //        if (UserId != null)
        //        {
        //            properties.Dictionary[XsrfKey] = UserId;
        //        }
        //        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        //    }
        //}

        #endregion

    }
}