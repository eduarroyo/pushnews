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
        public virtual DbSet<Perfil> Perfiles { get; set; }
        public virtual DbSet<Rol> Roles { get; set; }
        public virtual DbSet<Claim> Claims { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
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
        public DbSet<Asociado> Asociados { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Hermandad> Hermandades { get; set; }
        public DbSet<Gps> Gpss { get; set; }
        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<GpsPosicion> RutasPosiciones { get; set; }

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
            MapPerfiles(modelBuilder);
            MapLogins(modelBuilder);
            MapClaims(modelBuilder);
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
            MapAsociados(modelBuilder);
            MapEmpresas(modelBuilder);
            MapHermandades(modelBuilder);
            MapRutas(modelBuilder);
            MapGpss(modelBuilder);
            MapRutasPosiciones(modelBuilder);
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
            tabla.Property(a => a.Tipo).HasMaxLength(100);
            tabla.Property(a => a.CloudKey).HasMaxLength(100);
            tabla.Property(a => a.Usuario).HasMaxLength(100);
            tabla.Property(a => a.Clave).HasMaxLength(100);
            tabla.Property(a => a.ClaveSuscripcion).HasMaxLength(100);

            tabla.Property(a => a.ApiKey).HasMaxLength(500);
            tabla.Property(a => a.ApiKeyExternos).HasMaxLength(500);
            tabla.Property(a => a.PlayStoreUrl).HasMaxLength(1000);
            tabla.Property(a => a.ITunesUrl).HasMaxLength(1000);
            tabla.Property(a => a.MicrosoftStoreUrl).HasMaxLength(1000);

            tabla.HasOptional(a => a.Logotipo)
                    .WithMany()
                    .HasForeignKey(a => a.LogotipoID);

            tabla.HasMany(a => a.AplicacionesAmigas)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("AplicacionID");
                    m.MapRightKey("AplicacionAmigaID");
                    m.ToTable("AplicacionesAplicacionesAmigas");
                });
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

            modelBuilder.Entity<Rol>().Property(l => l.Modulo)
                .HasMaxLength(50);

            modelBuilder.Entity<Rol>()
                .Ignore(t => t.Id)
                .Ignore(t => t.Name);
        }

        private void MapPerfiles(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Perfil>()
                .ToTable("Perfiles")
                .HasKey(l => l.PerfilID);

            modelBuilder.Entity<Perfil>()
                .Property(l => l.PerfilID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Perfil>()
                .Property(l => l.Nombre)
                .HasMaxLength(50);

            modelBuilder.Entity<Perfil>()
                .HasMany(c => c.Roles)
                .WithMany(r => r.Perfiles)
                .Map(c =>
                {
                    c.ToTable("PerfilesRoles");
                    c.MapLeftKey("PerfilID");
                    c.MapRightKey("RolID");
                });
        }

        private void MapClaims(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Claim>()
                .ToTable("Claims")
                .HasKey(l => l.ClaimID);

            modelBuilder.Entity<Claim>()
                .Property(l => l.ClaimID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Claim>()
                .HasRequired(l => l.Usuario)
                .WithMany(e => e.Claims)
                .HasForeignKey(c => c.UsuarioID);
        }

        private void MapLogins(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>()
                .ToTable("Logins")
                .HasKey(l => new { l.ProveedorLogin, l.ProveedorClave, l.UsuarioID });

            modelBuilder.Entity<Login>()
                .Property(l => l.ProveedorLogin)
                .HasColumnOrder(1)
                .HasMaxLength(128);

            modelBuilder.Entity<Login>()
                .Property(l => l.ProveedorClave)
                .HasColumnOrder(2)
                .HasMaxLength(128);

            modelBuilder.Entity<Login>()
                .Property(l => l.UsuarioID)
                .HasColumnOrder(3);

            modelBuilder.Entity<Login>()
                .HasRequired(l => l.Usuario)
                .WithMany(e => e.Logins)
                .HasForeignKey(l => l.UsuarioID);
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

            tabla
                .HasMany(c => c.Perfiles)
                .WithMany(r => r.Usuarios)
                .Map(c =>
                {
                    c.ToTable("UsuariosPerfiles");
                    c.MapLeftKey("UsuarioID");
                    c.MapRightKey("PerfilID");
                });

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

        private void MapAsociados(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<Asociado> tabla = modelBuilder.Entity<Asociado>()
                .ToTable("Asociados")
                .HasKey(a => a.AsociadoID);

            tabla.Property(a => a.AsociadoID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tabla.Property(a => a.Codigo).HasMaxLength(100);
            tabla.Property(a => a.Nombre).HasMaxLength(100);
            tabla.Property(a => a.Apellidos).HasMaxLength(100);
            tabla.Property(a => a.Direccion).HasMaxLength(500);
            tabla.Property(a => a.CodigoPostal).HasMaxLength(10);
            tabla.Property(a => a.Localidad).HasMaxLength(100);
            tabla.Property(a => a.Provincia).HasMaxLength(100);
            tabla.Property(a => a.Observaciones).HasMaxLength(500);
            tabla.Property(a => a.Telefono).HasMaxLength(20);
            tabla.Property(a => a.Email).HasMaxLength(250);

            tabla
                .Ignore(a => a.Id)
                .Ignore(a => a.UserName);

            tabla.HasRequired(asoc => asoc.Aplicacion)
                .WithMany(apl => apl.Asociados)
                .HasForeignKey(apl => apl.AplicacionID);

            tabla.HasMany(asoc => asoc.ComunicacionesAccesos)
                .WithOptional(ca => ca.Asociado)
                .HasForeignKey(ca => ca.AsociadoID);
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

        private void MapHermandades(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<Hermandad> tabla = modelBuilder.Entity<Hermandad>()
                .ToTable("Hermandades")
                .HasKey(h => h.HermandadID);

            tabla.Property(h => h.HermandadID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tabla.Property(h => h.Nombre).HasMaxLength(200).IsRequired();
            tabla.Property(h => h.IglesiaNombre).HasMaxLength(200).IsRequired();
            tabla.Property(h => h.IglesiaDireccion).HasMaxLength(500).IsRequired();
            tabla.Property(e => e.Tags).HasMaxLength(500).IsRequired();

            tabla.HasRequired(h => h.Aplicacion)
                .WithMany(apl => apl.Hermandades)
                .HasForeignKey(h => h.AplicacionID);

            tabla.HasOptional(h => h.Logotipo)
                .WithMany()
                .HasForeignKey(h => h.LogotipoDocumentoID);
        }

        private void MapRutas(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<Ruta> tabla = modelBuilder.Entity<Ruta>()
                .ToTable("Rutas")
                .HasKey(r => r.RutaID);

            tabla.Property(r => r.RutaID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tabla.Property(r => r.Descripcion).HasMaxLength(500).IsRequired();
            tabla.Property(r => r.InicioDescripcion).HasMaxLength(200).IsRequired();
            tabla.Property(r => r.EntradaEnCarreraOficial).HasMaxLength(200).IsRequired();
            tabla.Property(r => r.FinDescripcion).HasMaxLength(200).IsRequired();
            tabla.Property(r => r.CabezaUltimaPosicionDireccion).HasMaxLength(200).IsRequired();
            tabla.Property(r => r.ColaUltimaPosicionDireccion).HasMaxLength(200).IsRequired();
            tabla.Ignore(r => r.CalculoTiempo);

            tabla.HasRequired(r => r.Hermandad)
                .WithMany(h => h.Rutas)
                .HasForeignKey(r => r.HermandadID);

            tabla.HasRequired(r => r.GpsCabeza)
                .WithMany()
                .HasForeignKey(r => r.GpsCabezaID);

            tabla.HasOptional(r => r.GpsCola)
                .WithMany()
                .HasForeignKey(r => r.GpsColaID);
        }

        private void MapGpss(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<Gps> tabla = modelBuilder.Entity<Gps>()
                .ToTable("Gpss")
                .HasKey(g => g.GpsID);

            tabla.Property(g => g.GpsID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tabla.Property(g => g.Api).HasMaxLength(100).IsRequired();
            tabla.Property(g => g.Estado).HasMaxLength(200).IsRequired();
            tabla.Property(g => g.Sensores).HasMaxLength(200).IsRequired();
            tabla.Property(g => g.Matricula).HasMaxLength(200).IsRequired();
            tabla.Property(g => g.UltimaPosicionDireccion).HasMaxLength(200).IsRequired();

            tabla.HasRequired(g => g.Aplicacion)
                .WithMany(a => a.Gpss)
                .HasForeignKey(g => g.AplicacionID);
        }

        private void MapRutasPosiciones(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<GpsPosicion> tabla = modelBuilder.Entity<GpsPosicion>()
                .ToTable("GpsPosiciones")
                .HasKey(rp => rp.GpsPosicionID);

            tabla.Property(rp => rp.GpsPosicionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            tabla.Property(rp => rp.Direccion).IsRequired().HasMaxLength(200);

            tabla.HasRequired(rp => rp.Gps)
                .WithMany(g => g.Posiciones)
                .HasForeignKey(rp => rp.GpsID);
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