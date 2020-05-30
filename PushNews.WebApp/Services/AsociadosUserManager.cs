using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using PushNews.Dominio;
using PushNews.Seguridad;
using PushNews.Dominio.Entidades;

namespace PushNews.WebApp.Services
{
    // Configure el administrador de usuarios de aplicación que se usa en esta aplicación. 
    // UserManager se define en ASP.NET Identity y se usa en la aplicación.
    // Configure el administrador de inicios de sesión que se usa en esta aplicación.
    public class AsociadosUserManager : UserManager<Asociado, long>
    {
        private IPushNewsAsociadoStore<Asociado, long> AsociadoStore
        {
            get
            {
                return (IPushNewsAsociadoStore<Asociado, long>) Store;
            }
        }

        public AsociadosUserManager(IPushNewsAsociadoStore<Asociado, long> store) : base(store)
        {
        }

        public async Task<IdentityResult> ChangePasswordAsync(long userId, string newPassword)
        {
            IdentityResult ir = await RemovePasswordAsync(userId);
            if(ir.Succeeded)
            {
                return await AddPasswordAsync(userId, newPassword);
            }
            else
            {
                return ir;
            }
        }

        public Task<Asociado> FindByIdAsync(long userId, long aplicacionID)
        {
            return this.AsociadoStore.FindByIdAsync(userId, aplicacionID);
        }

        public Task<Asociado> FindByNameAsync(string userName, long aplicacionID)
        {
            return this.AsociadoStore.FindByNameAsync(userName, aplicacionID);
        }
        
        public static AsociadosUserManager Create(IdentityFactoryOptions<AsociadosUserManager> options, IOwinContext context)
        {
            IPushNewsUnitOfWork pushNewsModel = context.Get<IPushNewsUnitOfWork>();
            var manager = new AsociadosUserManager(new AsociadoStore(pushNewsModel));
            
            // Configure la lógica de validación de nombres de usuario
            manager.UserValidator = new CustomUserValidator(manager)
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
                provider: new PhoneNumberTokenProvider<Asociado, long>
                            {
                                MessageFormat = "Su código de seguridad es {0}"
                            });
            manager.RegisterTwoFactorProvider(
                twoFactorProvider: "Código de correo electrónico",
                provider: new EmailTokenProvider<Asociado, long>
                            {
                                Subject = "Código de seguridad",
                                BodyFormat = "Su código de seguridad es {0}"
                            });

            IDataProtectionProvider dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<Asociado, long>(
                    dataProtectionProvider.Create(purposes: new string[] { "ASP.NET Identity" }));
            }
            return manager;
        }
    }
}