using System;
using System.Linq;

namespace PushNews.WebApp.Models.UI
{
    public class GestionArchivosModel
    {
        public enum Modos 
        {
            Seleccionar,
            SeleccionarSubir,
            SeleccionarCapturar,
            SeleccionarSubirCapturar,
            SubirCapturar,
            Capturar,
            Subir
        }

        public class ConfiguracionSubida
        {
            public bool SubidaMultiple { get; set; }

            public long CategoriaArchivos { get; set; }

            public string FiltroArchivos { get; set; }

            public string ControladorSubirArchivos { get; set; }

            public string AccionSubirArchivos { get; set; }

            public object ParametrosAdicionalesUpload { get; set; }
        }

        public class ConfiguracionCaptura
        {
            public bool SubidaMultiple { get; set; }

            public long CategoriaArchivos { get; set; }

            public string UrlSubirArchivos { get; set; }

            public string UrlSubirArchivo { get; set; }

            public object ParametrosAdicionalesUpload { get; set; }

            public string PrefijoNombreFoto { get; set; }
        }

        public class ConfiguracionSeleccion
        {
            public bool SeleccionMultiple { get; set; }
        
            public long TipoArchivos { get; set; }
            
            public string ControladorListaArchivos { get; set; }

            public object ParametrosAdicionales { get; set; }

            public long[] FiltroCategoriasArchivos { get; set; }
        }

        public string Titulo { get; set; }
        
        public Modos Modo { get; set; }

        public string Identificador { get; set; }

        public ConfiguracionSubida ConfigSubida { get; set; }

        public ConfiguracionCaptura ConfigCaptura { get; set; }

        public ConfiguracionSeleccion ConfigSeleccion { get; set; }


        public GestionArchivosModel(string identificador, string titulo)
        {
            Identificador = identificador;
            Titulo = titulo;
        }

        /// <summary>
        /// Constructor para el modo de subida de archivos.
        /// </summary>
        /// <param name="Titulo"></param>
        /// <param name="configSubida"></param>
        public GestionArchivosModel(string identificador, string titulo,
            ConfiguracionSubida configSubida): this(identificador, titulo)
        {
            ConfigSubida = configSubida;
            Modo = Modos.Subir;

            ConfigSeleccion = new ConfiguracionSeleccion();
            ConfigCaptura = new ConfiguracionCaptura();
        }

        /// <summary>
        /// Constructor para el modo de selección de archivos con subida.
        /// </summary>
        /// <param name="Titulo"></param>
        /// <param name="configSubida"></param>
        /// <param name="configSeleccion"></param>
        public GestionArchivosModel(string identificador, string titulo,
            ConfiguracionSubida configSubida, ConfiguracionSeleccion configSeleccion)
            : this(identificador, titulo)
        {
            ConfigSubida = configSubida;
            ConfigSeleccion = configSeleccion;
            Modo = Modos.SeleccionarSubir;
            
            ConfigCaptura = new ConfiguracionCaptura();
        }

        /// <summary>
        /// Constructor para el modo de subida de archivos y captura de fotos.
        /// </summary>
        /// <param name="Titulo"></param>
        /// <param name="configSubida"></param>
        public GestionArchivosModel(string identificador, string titulo,
            ConfiguracionSubida configSubida, ConfiguracionCaptura configCaptura)
            : this(identificador, titulo)
        {
            ConfigSubida = configSubida;
            ConfigCaptura = configCaptura;
            Modo = Modos.SubirCapturar;

            ConfigSeleccion = new ConfiguracionSeleccion();
        }

        /// <summary>
        /// Constructor para el modo de subida de archivos y captura de fotos con selección.
        /// </summary>
        /// <param name="Titulo"></param>
        /// <param name="configSubida"></param>
        public GestionArchivosModel(string identificador, string titulo, 
            ConfiguracionSubida configSubida, ConfiguracionCaptura configCaptura,
            ConfiguracionSeleccion configSeleccion)
            : this(identificador, titulo)
        {
            ConfigSubida = configSubida;
            ConfigCaptura = configCaptura;
            ConfigSeleccion = configSeleccion;
            Modo = Modos.SeleccionarSubirCapturar;
        }

        /// <summary>
        /// Constructor para el modo de selección de archivos.
        /// </summary>
        /// <param name="Titulo"></param>
        /// <param name="configSeleccion"></param>
        public GestionArchivosModel(string identificador, string titulo,
            ConfiguracionSeleccion configSeleccion)
            : this(identificador, titulo)
        {
            ConfigSeleccion = configSeleccion;
            Modo = Modos.Seleccionar;

            ConfigSeleccion = new ConfiguracionSeleccion();
            ConfigCaptura = new ConfiguracionCaptura();
        }

        public bool MostrarSeleccionar 
        {
            get
            {
                return Modo == Modos.Seleccionar
                    || Modo == Modos.SeleccionarCapturar
                    || Modo == Modos.SeleccionarSubir
                    || Modo == Modos.SeleccionarSubirCapturar;
            }
        }

        public bool MostrarCapturar
        {
            get
            {
                return Modo == Modos.Capturar
                    || Modo == Modos.SeleccionarCapturar
                    || Modo == Modos.SeleccionarSubirCapturar
                    || Modo == Modos.SubirCapturar;
            }
        }

        public bool MostrarSubir
        {
            get
            {
                return Modo == Modos.Subir
                    || Modo == Modos.SubirCapturar
                    || Modo == Modos.SeleccionarSubir
                    || Modo == Modos.SeleccionarSubirCapturar;
            }
        }
    }
}