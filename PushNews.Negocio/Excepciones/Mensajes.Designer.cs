﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PushNews.Negocio.Excepciones {
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
    internal class Mensajes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Mensajes() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PushNews.Negocio.Excepciones.Mensajes", typeof(Mensajes).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Ya existe una característica con el nombre {0}.
        /// </summary>
        internal static string AplicacionCaracteristicaExisteException {
            get {
                return ResourceManager.GetString("AplicacionCaracteristicaExisteException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Ya existe una aplicación con el nombre {0}..
        /// </summary>
        internal static string AplicacionExisteException {
            get {
                return ResourceManager.GetString("AplicacionExisteException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Ya existe una categoría con el nombre {0} en la aplicación {1}..
        /// </summary>
        internal static string CategoriaExisteException {
            get {
                return ResourceManager.GetString("CategoriaExisteException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No existe la comunicación con id={2} o no pertenece a la aplicación {0} (id={1})..
        /// </summary>
        internal static string ComunicacionNoEncontradaException {
            get {
                return ResourceManager.GetString("ComunicacionNoEncontradaException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Ya existe un parámetro con el nombre {0} en la aplicación {1}..
        /// </summary>
        internal static string ParametroAplicacionExisteException {
            get {
                return ResourceManager.GetString("ParametroAplicacionExisteException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Ya existe un parámetro con el nombre {0}..
        /// </summary>
        internal static string ParametroExisteException {
            get {
                return ResourceManager.GetString("ParametroExisteException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No existe la ruta con id={0}..
        /// </summary>
        internal static string RutaNoExisteException {
            get {
                return ResourceManager.GetString("RutaNoExisteException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Ya existe una aplicación que utiliza el subdominio {0}..
        /// </summary>
        internal static string SubdominioExisteException {
            get {
                return ResourceManager.GetString("SubdominioExisteException", resourceCulture);
            }
        }
    }
}
