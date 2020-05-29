namespace PushNews.Dominio.Entidades
{
    public class Empresa
    {
        public Empresa()
        {
            Nombre = "";
            Direccion = "";
            CodigoPostal = "";
            Localidad = "";
            Provincia = "";
            Descripcion = "";
            Telefono = "";
            Email = "";
            Web = "";
            Facebook = "";
            Twitter = "";
            Tags = "";
            Activo = true;
        }

        /// <summary>
        /// Identificador único de la empresa (PK).
        /// </summary>
        public long EmpresaID { get; set; }

        /// <summary>
        /// Identificador de la aplicación a la que la empresa está asociada (FK).
        /// </summary>
        public long AplicacionID { get; set; }

        /// <summary>
        /// Nombre de la empresa
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Dirección postal de la empresa
        /// </summary>
        public string Direccion { get; set; }

        /// <summary>
        /// Localidad donde se encuentra la empresa
        /// </summary>
        public string Localidad { get; set; }

        /// <summary>
        /// Código postal de la empresa
        /// </summary>
        public string CodigoPostal { get; set; }

        /// <summary>
        /// Provincia donde se encuentra la empresa
        /// </summary>
        public string Provincia { get; set; }

        /// <summary>
        /// Coordenadas de la empresa: latitud.
        /// </summary>
        public double? Latitud { get; set; }

        /// <summary>
        /// Coordenadas de la empresa: longitud.
        /// </summary>
        public double? Longitud { get; set; }

        /// <summary>
        /// Teléfono de contacto de la empresa.
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Email de contacto de la empresa.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// URL de la web de la empresa.
        /// </summary>
        public string Web { get; set; }

        /// <summary>
        /// URL de la página de facebook de la empresa.
        /// </summary>
        public string Facebook { get; set; }

        /// <summary>
        /// URL de la página de twitter de la empresa.
        /// </summary>
        public string Twitter { get; set; }

        /// <summary>
        /// Identificador del documento del logotipo de la empresa (FK).
        /// </summary>
        public long? LogotipoDocumentoID { get; set; }

        /// <summary>
        /// Identificador del documento del banner de la empresa (FK).
        /// </summary>
        public long? BannerDocumentoID { get; set; }

        /// <summary>
        /// Descripcion de la empresa.
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Contiene tags descriptivos de la empresa separados por comas, para propósitos de 
        /// clasificación y búsqueda.
        /// </summary>
        public string Tags { get; set; }

        public bool Activo { get; set; }

        public virtual Aplicacion Aplicacion { get; set; }
        public virtual Documento Logotipo { get; set; }
        public virtual Documento Banner { get; set; }
    }
}
