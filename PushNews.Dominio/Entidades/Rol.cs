using Microsoft.AspNet.Identity;

namespace PushNews.Dominio.Entidades
{
    public class Rol : IRole<long>
    {
        public Rol()
        {}

        public long RolID { get; set; }
        public string Nombre { get; set; }

        /// <summary>
        /// Propiedad para obtener el id del rol, obligada por IRole.
        /// Es sólo un envoltorio de la propiedad RolID.
        /// </summary>
        public long Id
        {
            get { return RolID; }
            set { RolID = value; }
        }

        /// <summary>
        /// Propiedad para obtener el nombre del rol, obligada por IRole.
        /// Es sólo un envoltorio de la propiedad Rol1.
        /// </summary>
        public string Name
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
    }
}