﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PushNews.WebApp.App_LocalResources {
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
    public class PlantillasEmail {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PlantillasEmail() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PushNews.WebApp.App_LocalResources.PlantillasEmail", typeof(PlantillasEmail).Assembly);
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
        ///   Busca una cadena traducida similar a Hola.
        /// </summary>
        public static string Hola {
            get {
                return ResourceManager.GetString("Hola", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Restablecer tu clave.
        /// </summary>
        public static string RestablecerClaveBoton {
            get {
                return ResourceManager.GetString("RestablecerClaveBoton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Te enviamos este mensaje porque hemos recibido tu solicitud de restablecimiento de contraseña. Haz click en este botón para restablecerla:.
        /// </summary>
        public static string RestablecerClaveParrafo1 {
            get {
                return ResourceManager.GetString("RestablecerClaveParrafo1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Si no has solicitado el restablecimiento de tu contraseña, por favor, ignora este mensaje o háznoslo saber reenviándonoslo. Este restablecimiento de clave sólo será válido durante los próximos 30 minutos..
        /// </summary>
        public static string RestablecerClaveParrafo2 {
            get {
                return ResourceManager.GetString("RestablecerClaveParrafo2", resourceCulture);
            }
        }
    }
}
