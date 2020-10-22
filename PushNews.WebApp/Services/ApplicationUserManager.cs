using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Identity;
using System;
using System.Threading.Tasks;

namespace PushNews.WebApp.Services
{
    // Configure el administrador de usuarios de aplicación que se usa en esta aplicación. 
    // UserManager se define en ASP.NET Identity y se usa en la aplicación.
    // Configure el administrador de inicios de sesión que se usa en esta aplicación.
    public class ApplicationUserManager : UserManager<Usuario, long>
    {
        private UserStore PushNewsUserStore
        {
            get
            {
                return (UserStore)Store;
            }
        }

        public ApplicationUserManager(UserStore store) : base(store)
        {
        }

        public async Task<IdentityResult> ChangePasswordAsync(long userId, string newPassword)
        {
            IdentityResult ir = await RemovePasswordAsync(userId);
            if (ir.Succeeded)
            {
                return await AddPasswordAsync(userId, newPassword);
            }
            else
            {
                return ir;
            }
        }

        public async Task EstablecerRolAsync(Usuario emp, long rolId, bool guardarCambios = true)
        {
            emp.RolID = rolId;
            if (guardarCambios)
            {
                await PushNewsUserStore.SaveChangesAsync();
            }
        }

        public override async Task<bool> IsInRoleAsync(long userId, string role)
        {
            Usuario u = await PushNewsUserStore.FindByIdAsync(userId);
            return String.Compare(u.Rol.Nombre, role, ignoreCase: true) == 0;
        }

        public override async Task<IdentityResult> AddToRoleAsync(long userId, string role)
        {
            Usuario u = await PushNewsUserStore.FindByIdAsync(userId);
            if(u == null)
            {
                return IdentityResult.Failed("Usuario no encontrado");
            }
            return await PushNewsUserStore.AsignarRol(u, role);
        }

        public override async Task<IdentityResult> AddToRolesAsync(long userId, params string[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                return await AddToRoleAsync(userId, roles[0]);
            }
            else
            {
                return IdentityResult.Failed("No se especificó el rol.");
            }
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            IPushNewsUnitOfWork pushNewsModel = context.Get<IPushNewsUnitOfWork>();
            var manager = new ApplicationUserManager(new UserStore(pushNewsModel));

            // Configure la lógica de validación de nombres de usuario
            manager.UserValidator = new UserValidator<Usuario, long>(manager)
            {

                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false,
            };

            // Configure la lógica de validación de contraseñas
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                //RequireNonLetterOrDigit = true,
                //RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };

            // Configurar valores predeterminados para bloqueo de usuario
            manager.UserLockoutEnabledByDefault = true;
            //manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Registre proveedores de autenticación en dos fases. Esta aplicación usa los pasos Teléfono y Correo electrónico para recibir un código para comprobar el usuario
            // Puede escribir su propio proveedor y conectarlo aquí.
            manager.RegisterTwoFactorProvider(
                twoFactorProvider: "Código telefónico",
                provider: new PhoneNumberTokenProvider<Usuario, long>
                {
                    MessageFormat = "Su código de seguridad es {0}"
                });
            manager.RegisterTwoFactorProvider(
                twoFactorProvider: "Código de correo electrónico",
                provider: new EmailTokenProvider<Usuario, long>
                {
                    Subject = "Código de seguridad",
                    BodyFormat = "Su código de seguridad es {0}"
                });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();

            IDataProtectionProvider dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<Usuario, long>(
                    dataProtectionProvider.Create(purposes: new string[] { "ASP.NET Identity" }));
            }
            return manager;
        }

        internal async Task EstablecerRol(Usuario modificar, long rolId)
        {
            await PushNewsUserStore.AsignarRol(modificar, rolId);
            await PushNewsUserStore.SaveChangesAsync();
        }
    }
}