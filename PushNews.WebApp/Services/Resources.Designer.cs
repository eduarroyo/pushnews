﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PushNews.WebApp.Services {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PushNews.WebApp.Services.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a An unknown failure has occured..
        /// </summary>
        public static string DefaultError {
            get {
                return ResourceManager.GetString("DefaultError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Email &apos;{0}&apos; is already taken..
        /// </summary>
        public static string DuplicateEmail {
            get {
                return ResourceManager.GetString("DuplicateEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Name {0} is already taken..
        /// </summary>
        public static string DuplicateName {
            get {
                return ResourceManager.GetString("DuplicateName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a A user with that external login already exists..
        /// </summary>
        public static string ExternalLoginExists {
            get {
                return ResourceManager.GetString("ExternalLoginExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Email &apos;{0}&apos; is invalid..
        /// </summary>
        public static string InvalidEmail {
            get {
                return ResourceManager.GetString("InvalidEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Invalid token..
        /// </summary>
        public static string InvalidToken {
            get {
                return ResourceManager.GetString("InvalidToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a User name {0} is invalid, can only contain letters or digits..
        /// </summary>
        public static string InvalidUserName {
            get {
                return ResourceManager.GetString("InvalidUserName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Lockout is not enabled for this user..
        /// </summary>
        public static string LockoutNotEnabled {
            get {
                return ResourceManager.GetString("LockoutNotEnabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No IUserTokenProvider is registered..
        /// </summary>
        public static string NoTokenProvider {
            get {
                return ResourceManager.GetString("NoTokenProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No IUserTwoFactorProvider for &apos;{0}&apos; is registered..
        /// </summary>
        public static string NoTwoFactorProvider {
            get {
                return ResourceManager.GetString("NoTwoFactorProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Incorrect password..
        /// </summary>
        public static string PasswordMismatch {
            get {
                return ResourceManager.GetString("PasswordMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Passwords must have at least one digit (&apos;0&apos;-&apos;9&apos;)..
        /// </summary>
        public static string PasswordRequireDigit {
            get {
                return ResourceManager.GetString("PasswordRequireDigit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Passwords must have at least one lowercase (&apos;a&apos;-&apos;z&apos;)..
        /// </summary>
        public static string PasswordRequireLower {
            get {
                return ResourceManager.GetString("PasswordRequireLower", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Passwords must have at least one non letter or digit character..
        /// </summary>
        public static string PasswordRequireNonLetterOrDigit {
            get {
                return ResourceManager.GetString("PasswordRequireNonLetterOrDigit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Passwords must have at least one uppercase (&apos;A&apos;-&apos;Z&apos;)..
        /// </summary>
        public static string PasswordRequireUpper {
            get {
                return ResourceManager.GetString("PasswordRequireUpper", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Passwords must be at least {0} characters..
        /// </summary>
        public static string PasswordTooShort {
            get {
                return ResourceManager.GetString("PasswordTooShort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a {0} cannot be null or empty..
        /// </summary>
        public static string PropertyTooShort {
            get {
                return ResourceManager.GetString("PropertyTooShort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Role {0} does not exist..
        /// </summary>
        public static string RoleNotFound {
            get {
                return ResourceManager.GetString("RoleNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Store does not implement IQueryableRoleStore&lt;TRole&gt;..
        /// </summary>
        public static string StoreNotIQueryableRoleStore {
            get {
                return ResourceManager.GetString("StoreNotIQueryableRoleStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Store does not implement IQueryableUserStore&lt;TUser&gt;..
        /// </summary>
        public static string StoreNotIQueryableUserStore {
            get {
                return ResourceManager.GetString("StoreNotIQueryableUserStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Store does not implement IUserClaimStore&lt;TUser&gt;..
        /// </summary>
        public static string StoreNotIUserClaimStore {
            get {
                return ResourceManager.GetString("StoreNotIUserClaimStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Store does not implement IUserConfirmationStore&lt;TUser&gt;..
        /// </summary>
        public static string StoreNotIUserConfirmationStore {
            get {
                return ResourceManager.GetString("StoreNotIUserConfirmationStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Store does not implement IUserEmailStore&lt;TUser&gt;..
        /// </summary>
        public static string StoreNotIUserEmailStore {
            get {
                return ResourceManager.GetString("StoreNotIUserEmailStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Store does not implement IUserLockoutStore&lt;TUser&gt;..
        /// </summary>
        public static string StoreNotIUserLockoutStore {
            get {
                return ResourceManager.GetString("StoreNotIUserLockoutStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Store does not implement IUserLoginStore&lt;TUser&gt;..
        /// </summary>
        public static string StoreNotIUserLoginStore {
            get {
                return ResourceManager.GetString("StoreNotIUserLoginStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Store does not implement IUserPasswordStore&lt;TUser&gt;..
        /// </summary>
        public static string StoreNotIUserPasswordStore {
            get {
                return ResourceManager.GetString("StoreNotIUserPasswordStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Store does not implement IUserPhoneNumberStore&lt;TUser&gt;..
        /// </summary>
        public static string StoreNotIUserPhoneNumberStore {
            get {
                return ResourceManager.GetString("StoreNotIUserPhoneNumberStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Store does not implement IUserRoleStore&lt;TUser&gt;..
        /// </summary>
        public static string StoreNotIUserRoleStore {
            get {
                return ResourceManager.GetString("StoreNotIUserRoleStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Store does not implement IUserSecurityStampStore&lt;TUser&gt;..
        /// </summary>
        public static string StoreNotIUserSecurityStampStore {
            get {
                return ResourceManager.GetString("StoreNotIUserSecurityStampStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Store does not implement IUserTwoFactorStore&lt;TUser&gt;..
        /// </summary>
        public static string StoreNotIUserTwoFactorStore {
            get {
                return ResourceManager.GetString("StoreNotIUserTwoFactorStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a User already has a password set..
        /// </summary>
        public static string UserAlreadyHasPassword {
            get {
                return ResourceManager.GetString("UserAlreadyHasPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a User already in role..
        /// </summary>
        public static string UserAlreadyInRole {
            get {
                return ResourceManager.GetString("UserAlreadyInRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a UserId not found..
        /// </summary>
        public static string UserIdNotFound {
            get {
                return ResourceManager.GetString("UserIdNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a User {0} does not exist..
        /// </summary>
        public static string UserNameNotFound {
            get {
                return ResourceManager.GetString("UserNameNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a User is not in role..
        /// </summary>
        public static string UserNotInRole {
            get {
                return ResourceManager.GetString("UserNotInRole", resourceCulture);
            }
        }
    }
}
