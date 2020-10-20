using PushNews.Dominio.Entidades;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Data.Entity.ModelConfiguration;
using System;

namespace PushNews.Dominio
{
    [SuppressMessage("Arguments", "JustCode_LiteralArgumentIsNotNamedDiagnostic:The used literal argument is not named", Justification = "Parámetros de métodos estandar y suficientemente autoexplicativos.")]
    public class PushNewsModel : DbContext, IPushNewsUnitOfWork
    {
        public PushNewsModel() : base()
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
        }
        public PushNewsModel(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
        }

        #region ############################################################################################ TABLAS

        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Rol> Roles { get; set; }
        public virtual DbSet<Parametro> Parametros { get; set; }
        public virtual DbSet<Comunicacion> Comunicaciones { get; set; }
        public virtual DbSet<ComunicacionAcceso> Accesos { get; set; }
        public virtual DbSet<Terminal> Terminales { get; set; }
        public virtual DbSet<Aplicacion> Aplicaciones { get; set; }
        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Documento> Documentos { get; set; }
        public virtual DbSet<Telefono> Telefonos { get; set; }
        public virtual DbSet<Localizacion> Localizaciones { get; set; }
        public DbSet<AplicacionCaracteristica> AplicacionesCaracteristicas { get; set; }
        public DbSet<Empresa> Empresas { get; set; }

        public string ConnectionString
        {
            get
            {
                return Database.Connection.ConnectionString;
            }
        }

        public bool AutoDetectChangedEnabled
        {
            get
            {
                return Configuration.AutoDetectChangesEnabled;
            }
        }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Deshabilitar eliminación en cascada por defecto.
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // Configuración de los tipos de entidad y sus relaciones
            MapRoles(modelBuilder);
            MapUsuarios(modelBuilder);
            MapParametros(modelBuilder);
            MapComunicaciones(modelBuilder);
            MapAccesos(modelBuilder);
            MapTerminales(modelBuilder);
            MapAplicaciones(modelBuilder);
            MapCategorias(modelBuilder);
            MapDocumentos(modelBuilder);
            MapTelefonos(modelBuilder);
            MapLocalizaciones(modelBuilder);
            MapAplicacionesCaracteristicas(modelBuilder);
            MapEmpresas(modelBuilder);
        }

        private void MapCategorias(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<Categoria> tabla =
                   modelBuilder.Entity<Categoria>()
                       .ToTable("Categorias")
                       .HasKey(t => t.CategoriaID);
            tabla.Property(t => t.CategoriaID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tabla.Property(t => t.Nombre).HasMaxLength(100);
            tabla.Property(t => t.Icono).HasMaxLength(256);
            tabla.Property(t => t.Orden);

            tabla.HasRequired(c => c.Aplicacion)
                .WithMany(a => a.Categorias)
                .HasForeignKey(c => c.AplicacionID);

            tabla.HasRequired(c => c.Creador)
                .WithMany()
                .HasForeignKey(c => c.UsuarioID);
        }

        private void MapAplicaciones(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<Aplicacion> tabla =
                   modelBuilder.Entity<Aplicacion>()
                       .ToTable("Aplicaciones")
                       .HasKey(a => a.AplicacionID);
            tabla.Property(a => a.AplicacionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tabla.Property(a => a.Nombre).HasMaxLength(100);
            tabla.Property(a => a.Version).HasMaxLength(100);
            tabla.Property(a => a.CloudKey).HasMaxLength(100);
            tabla.Property(a => a.Usuario).HasMaxLength(100);
            tabla.Property(a => a.Clave).HasMaxLength(100);

            tabla.Property(a => a.ApiKey).HasMaxLength(500);
            tabla.Property(a => a.PlayStoreUrl).HasMaxLength(1000);
            tabla.Property(a => a.AppStoreUrl).HasMaxLength(1000);

            tabla.HasOptional(a => a.Logotipo)
                    .WithMany()
                    .HasForeignKey(a => a.LogotipoID);
        }

        private void MapTerminales(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<Terminal> tabla =
                modelBuilder.Entity<Terminal>()
                    .ToTable("Terminales")
                    .HasKey(t => t.TerminalID);
            tabla.Property(t => t.TerminalID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tabla.Property(t => t.Nombre).HasMaxLength(100);
            tabla.Property(t => t.UltimaConexionIP).HasMaxLength(40); // Para que quepa una IPv6

            tabla.HasRequired(t => t.Aplicacion)
                .WithMany(a => a.Terminales)
                .HasForeignKey(t => t.AplicacionID);
        }

        private void MapComunicaciones(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<Comunicacion> tabla = modelBuilder.Entity<Comunicacion>();

            tabla
                .ToTable("Comunicaciones")
                .HasKey(c => c.ComunicacionID);

            tabla.Property(c => c.ComunicacionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tabla.Property(c => c.Titulo).HasMaxLength(100);
            tabla.Property(c => c.Autor).HasMaxLength(100);
            tabla.Property(c => c.ImagenTitulo).HasMaxLength(100);
            tabla.Property(c => c.AdjuntoTitulo).HasMaxLength(100);
            tabla.Property(c => c.Youtube).HasMaxLength(256);
            tabla.Property(c => c.YoutubeTitulo).HasMaxLength(100);
            tabla.Property(c => c.Enlace).HasMaxLength(256);
            tabla.Property(c => c.EnlaceTitulo).HasMaxLength(100);
            tabla.Property(c => c.GeoPosicionTitulo).HasMaxLength(100);
            tabla.Property(c => c.RecordatorioTitulo).HasMaxLength(100);
            tabla.Property(c => c.UltimaEdicionIP).HasMaxLength(40);
            tabla.Property(c => c.GeoPosicionDireccion).HasMaxLength(500);
            tabla.Property(c => c.GeoPosicionLocalidad).HasMaxLength(100);
            tabla.Property(c => c.GeoPosicionProvincia).HasMaxLength(100);
            tabla.Property(c => c.GeoPosicionPais).HasMaxLength(100);

            tabla.HasRequired(c => c.Categoria)
                .WithMany(cat => cat.Comunicaciones)
                .HasForeignKey(c => c.CategoriaID);

            tabla.HasRequired(c => c.Usuario)
                .WithMany(u => u.Comunicaciones)
                .HasForeignKey(c => c.UsuarioID);

            tabla.HasOptional(c => c.Adjunto)
                .WithMany(d => d.ComunicacionesAdjunto)
                .HasForeignKey(c => c.AdjuntoDocumentoID);

            tabla.HasOptional(c => c.Imagen)
                .WithMany(d => d.ComunicacionesImagen)
                .HasForeignKey(c => c.ImagenDocumentoID);
        }

        private void MapAccesos(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<ComunicacionAcceso> tabla =
                modelBuilder.Entity<ComunicacionAcceso>()
                    .ToTable("ComunicacionesAccesos")
                    .HasKey(ca => ca.ComunicacionAccesoID);
            tabla.Property(ca => ca.ComunicacionAccesoID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tabla.HasRequired(a => a.Terminal)
                .WithMany(t => t.Accesos)
                .HasForeignKey(a => a.TerminalID);

            tabla.HasRequired(a => a.Comunicacion)
                .WithMany(c => c.Accesos)
                .HasForeignKey(c => c.ComunicacionID);
        }

        private void MapRoles(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>()
                .ToTable("Roles")
                .HasKey(l => l.RolID);

            modelBuilder.Entity<Rol>()
                .Property(l => l.RolID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Rol>().Property(l => l.Nombre)
                .HasMaxLength(50);

            modelBuilder.Entity<Rol>()
                .Ignore(t => t.Id)
                .Ignore(t => t.Name);
        }

        private void MapUsuarios(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<Usuario> tabla = modelBuilder.Entity<Usuario>();
            tabla
                .ToTable("Usuarios")
                .HasKey(c => c.UsuarioID);

            tabla
                .Property(c => c.UsuarioID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tabla
                .Property(c => c.Nombre)
                .HasMaxLength(50);

            tabla
                .Property(c => c.Apellidos)
                .HasMaxLength(150);

            tabla
                .Property(c => c.Movil)
                .HasMaxLength(50);

            tabla
                .Property(c => c.Email)
                .HasMaxLength(250);

            tabla
                .Property(c => c.Locale)
                .HasMaxLength(20);

            tabla
                .Ignore(e => e.Id)
                .Ignore(e => e.UserName)
                .Ignore(e => e.ApellidosNombre);

            tabla
                .HasMany(e => e.Aplicaciones)
                .WithMany(c => c.Usuarios)
                .Map(c =>
                {
                    c.ToTable("AplicacionesUsuarios");
                    c.MapLeftKey("UsuarioID");
                    c.MapRightKey("AplicacionID");
                });

            tabla.HasRequired(c => c.Rol)
                .WithMany()
                .HasForeignKey(u => u.RolID);

            tabla
                .HasMany(c => c.Categorias)
                .WithMany()
                .Map(c =>
                {
                    c.ToTable("UsuariosCategorias");
                    c.MapLeftKey("UsuarioID");
                    c.MapRightKey("CategoriaID");
                });
        }

        private void MapParametros(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parametro>()
                .ToTable("Parametros")
                .HasKey(p => p.ParametroID);

            modelBuilder.Entity<Parametro>()
                .Property(p => p.ParametroID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Parametro>()
                .Property(p => p.Nombre)
                .HasMaxLength(100);

            modelBuilder.Entity<Parametro>()
                .Property(p => p.Valor)
                .HasMaxLength(500);

            modelBuilder.Entity<Parametro>()
                .Property(p => p.Descripcion)
                .HasMaxLength(200);

            modelBuilder.Entity<Parametro>()
                .HasOptional(p => p.Aplicacion)
                .WithMany(c => c.Parametros)
                .HasForeignKey(p => p.AplicacionID);
        }

        private void MapDocumentos(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Documento>()
                .ToTable("Documentos")
                .HasKey(p => p.DocumentoID);

            modelBuilder.Entity<Documento>()
                .Property(p => p.DocumentoID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Documento>()
                .Property(dt => dt.Nombre)
                .HasMaxLength(50);

            modelBuilder.Entity<Documento>()
                .Property(dt => dt.Ruta)
                .HasMaxLength(256);

            // EAR - 17/04/2017 - Cambio en el esquema para aumentar tamaño del campo MIME. 
            // El valor anterior (50) era el motivo de los errores al subir documentos de excel 
            // (.xlsx) y similares, cuyo tipo MIME es una cadena de más de 50 caracteres.
            modelBuilder.Entity<Documento>()
                .Property(dt => dt.Mime)
                .HasMaxLength(300);

            modelBuilder.Entity<Documento>()
                .HasRequired(d => d.Aplicacion)
                .WithMany()
                .HasForeignKey(d => d.AplicacionID);
        }

        private void MapTelefonos(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<Telefono> tabla = modelBuilder.Entity<Telefono>();

            tabla
                .ToTable("Telefonos")
                .HasKey(t => t.TelefonoID);

            tabla.Property(t => t.TelefonoID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tabla.Property(t => t.Numero).HasMaxLength(100);
            tabla.Property(t => t.Descripcion).HasMaxLength(256);

            tabla.HasRequired(c => c.Aplicacion)
                .WithMany(a => a.Telefonos)
                .HasForeignKey(c => c.AplicacionID);
        }

        private void MapLocalizaciones(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<Localizacion> tabla =
                   modelBuilder.Entity<Localizacion>()
                       .ToTable("Localizaciones")
                       .HasKey(t => t.LocalizacionID);
            tabla.Property(t => t.LocalizacionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tabla.Property(t => t.Descripcion).HasMaxLength(256);

            tabla.HasRequired(c => c.Aplicacion)
                .WithMany(a => a.Localizaciones)
                .HasForeignKey(c => c.AplicacionID);
        }

        private void MapAplicacionesCaracteristicas(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<AplicacionCaracteristica> tabla =
                   modelBuilder.Entity<AplicacionCaracteristica>()
                       .ToTable("AplicacionesCaracteristicas")
                       .HasKey(t => t.AplicacionCaracteristicaID);

            tabla.Property(t => t.AplicacionCaracteristicaID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            tabla.Property(t => t.Nombre).HasMaxLength(100);

            tabla.HasMany(ac => ac.Aplicaciones)
                .WithMany(a => a.Caracteristicas)
                .Map(m =>
                {
                    m.MapRightKey("AplicacionID");
                    m.MapLeftKey("AplicacionCaracteristicaID");
                    m.ToTable("AplicacionesAplicacionesCaracteristicas");
                });
        }

        private void MapEmpresas(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<Empresa> tabla = modelBuilder.Entity<Empresa>()
                .ToTable("Empresas")
                .HasKey(e => e.EmpresaID);

            tabla.Property(e => e.EmpresaID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tabla.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            tabla.Property(e => e.Direccion).HasMaxLength(500).IsRequired();
            tabla.Property(e => e.CodigoPostal).HasMaxLength(10).IsRequired();
            tabla.Property(e => e.Localidad).HasMaxLength(100).IsRequired();
            tabla.Property(e => e.Provincia).HasMaxLength(100).IsRequired();
            tabla.Property(e => e.Descripcion).HasMaxLength(500).IsRequired();
            tabla.Property(e => e.Telefono).HasMaxLength(20).IsRequired();
            tabla.Property(e => e.Email).HasMaxLength(250).IsRequired();
            tabla.Property(e => e.Web).HasMaxLength(500).IsRequired();
            tabla.Property(e => e.Facebook).HasMaxLength(500).IsRequired();
            tabla.Property(e => e.Twitter).HasMaxLength(500).IsRequired();
            tabla.Property(e => e.Tags).HasMaxLength(500).IsRequired();

            tabla.HasRequired(e => e.Aplicacion)
                .WithMany(apl => apl.Empresas)
                .HasForeignKey(e => e.AplicacionID);

            tabla.HasOptional(e => e.Logotipo)
                .WithMany()
                .HasForeignKey(e => e.LogotipoDocumentoID);

            tabla.HasOptional(e => e.Banner)
                .WithMany()
                .HasForeignKey(e => e.BannerDocumentoID);
        }

        #region IEntityFrameworkUnitOfWork
        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            if (base.Entry<TEntity>(entity).State == EntityState.Detached)
            {
                base.Set<TEntity>().Attach(entity);
            }
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return base.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return base.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        public async Task<int> ExecuteCommandAsync(string sqlCommand, params object[] parameters)
        {
            return await base.Database.ExecuteSqlCommandAsync(sqlCommand, parameters);
        }

        public void SetModified<TEntity>(TEntity entity) where TEntity : class
        {
            base.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public void Commit()
        {
            base.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await base.SaveChangesAsync();
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    base.SaveChanges();
                    saveFailed = false;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    ex.Entries
                        .ToList()
                        .ForEach(entry => entry.OriginalValues.SetValues(entry.GetDatabaseValues()));
                }
            } while (saveFailed);
        }

        public void Rollback()
        {
            base.ChangeTracker.Entries()
                .ToList()
                .ForEach(entry => entry.State = EntityState.Unchanged);
        }

        public void LoadProperty<T>(T entity, string propertyName) where T : class
        {
            Entry(entity).Reference(propertyName).Load();
        }

        public void LoadCollection<T>(T entity, string propertyName) where T : class
        {
            Entry(entity).Collection(propertyName).Load();
        }

        public async Task LoadPropertyAsync<T>(T entity, string propertyName) where T : class
        {
            await Entry(entity).Reference(propertyName).LoadAsync();
        }

        public async Task LoadCollectionAsync<T>(T entity, string propertyName) where T : class
        {
            await Entry(entity).Collection(propertyName).LoadAsync();
        }

        public override DbSet<TEntity> Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        #endregion
    }
}