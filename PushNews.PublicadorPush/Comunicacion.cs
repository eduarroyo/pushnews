//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PushNews.PublicadorPush
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comunicacion
    {
        public long ComunicacionID { get; set; }
        public long UsuarioID { get; set; }
        public long CategoriaID { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public System.DateTime FechaPublicacion { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Autor { get; set; }
        public string ImagenTitulo { get; set; }
        public string AdjuntoTitulo { get; set; }
        public string EnlaceTitulo { get; set; }
        public string Enlace { get; set; }
        public string YoutubeTitulo { get; set; }
        public string Youtube { get; set; }
        public string GeoPosicionTitulo { get; set; }
        public Nullable<float> GeoPosicionLatitud { get; set; }
        public Nullable<float> GeoPosicionLongitud { get; set; }
        public string UltimaEdicionIP { get; set; }
        public bool Activo { get; set; }
        public bool Borrado { get; set; }
        public Nullable<System.DateTime> FechaBorrado { get; set; }
        public Nullable<long> ImagenDocumentoID { get; set; }
        public Nullable<long> AdjuntoDocumentoID { get; set; }
        public long TimeStamp { get; set; }
        public string GeoPosicionDireccion { get; set; }
        public string GeoPosicionLocalidad { get; set; }
        public string GeoPosicionProvincia { get; set; }
        public string GeoPosicionPais { get; set; }
        public bool PushEnviada { get; set; }
        public Nullable<System.DateTime> PushFecha { get; set; }
        public bool Destacado { get; set; }
        public string RecordatorioTitulo { get; set; }
        public Nullable<System.DateTime> RecordatorioFecha { get; set; }
        public Nullable<System.DateTime> PushRecordatorio { get; set; }
        public bool Instantanea { get; set; }
    
        public virtual Categoria Categoria { get; set; }
    }
}