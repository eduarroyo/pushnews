﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PushNewsEntities : DbContext
    {
        public PushNewsEntities()
            : base("name=PushNewsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Aplicacion> Aplicaciones { get; set; }
        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Comunicacion> Comunicaciones { get; set; }
        public virtual DbSet<Parametro> Parametros { get; set; }
    }
}
