using PushNews.Dominio.Entidades;
using PushNews.Dominio.Interfaces;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace PushNews.Dominio
{
    public interface IPushNewsUnitOfWork : IEntityFrameworkUnitOfWork, IDisposable
    {
        DbSet<Usuario> Usuarios { get; set; }
        DbSet<Claim> Claims { get; set; }
        DbSet<Parametro> Parametros { get; set; }
        DbSet<Perfil> Perfiles { get; set; }
        DbSet<Rol> Roles { get; set; }
        DbSet<Comunicacion> Comunicaciones { get;set; }
        DbSet<ComunicacionAcceso> Accesos { get; set; }
        DbSet<Terminal> Terminales { get; set; }
        DbSet<Aplicacion> Aplicaciones { get; set; }
        DbSet<Categoria> Categorias { get; set; }
        DbSet<Documento> Documentos { get; set; }
        DbSet<Telefono> Telefonos { get; set; }
        DbSet<Localizacion> Localizaciones { get; set; }
        DbSet<AplicacionCaracteristica> AplicacionesCaracteristicas { get; set; }
        DbSet<Empresa> Empresas { get; set; }

        DbChangeTracker ChangeTracker { get; }

        void LoadProperty<T>(T entity, string propertyName) where T : class;
        void LoadCollection<T>(T entity, string propertyName) where T : class;
        Task LoadPropertyAsync<T>(T entity, string propertyName) where T : class;
        Task LoadCollectionAsync<T>(T entity, string propertyName) where T : class;
        
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}