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
    public class AplicacionesCaracteristicas {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AplicacionesCaracteristicas() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PushNews.WebApp.App_LocalResources.AplicacionesCaracteristicas", typeof(AplicacionesCaracteristicas).Assembly);
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
        ///   Busca una cadena traducida similar a la característica.
        /// </summary>
        public static string ArtEntidad {
            get {
                return ResourceManager.GetString("ArtEntidad", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Buscar características.
        /// </summary>
        public static string Buscar {
            get {
                return ResourceManager.GetString("Buscar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Se va a eliminar la característica.
        /// </summary>
        public static string ConfirmacionEliminar {
            get {
                return ResourceManager.GetString("ConfirmacionEliminar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No se puede eliminar la característica porque está siendo usada por aplicaciones..
        /// </summary>
        public static string ErrorEliminarCaracteristicaEnUso {
            get {
                return ResourceManager.GetString("ErrorEliminarCaracteristicaEnUso", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Nombre.
        /// </summary>
        public static string Nombre {
            get {
                return ResourceManager.GetString("Nombre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Gestión de características.
        /// </summary>
        public static string Subtitulo {
            get {
                return ResourceManager.GetString("Subtitulo", resourceCulture);
            }
        }
    }
}
