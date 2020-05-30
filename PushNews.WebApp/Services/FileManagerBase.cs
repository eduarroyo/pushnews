using System.IO;
using PushNews.Negocio.Interfaces;
using System;
using System.Web;
using PushNews.WebApp.Helpers;
using log4net;
using System.Threading.Tasks;
using PushNews.Dominio.Entidades;
using PushNews.Dominio.Enums;
using System.Web.Helpers;

namespace PushNews.WebApp.Services
{
    public abstract class FileManagerBase : IFileManager
    {
        public void Dispose()
        {

        }

        protected ILog log;

        protected IServiciosFactoria servicios;

        protected abstract Task<Tuple<string, long>> GuardarArchivoAsync(string archivo, string nombre);

        protected abstract Task<string> GuardarArchivoAsync(HttpPostedFileBase archivo, string nombre);
        protected abstract void GuardarThumbnail(string rutaOriginal, string nombreArchivo);

        protected abstract Task EliminarArchivoAsync(Documento documento);

        protected abstract Task DocumentoAsync(Documento documento, Stream stream);
        protected abstract FileStream Documento(Documento documento);

        protected abstract Task MiniaturaAsync(Documento documento, Stream stream);
        protected abstract FileStream Miniatura(Documento documento);

        public async Task EliminarDocumento(long documentoID, bool persistir = true)
        {
            IDocumentosServicio srv = servicios.DocumentosServicio();
            Documento doc = srv.GetSingle(d => d.DocumentoID == documentoID);
            await EliminarArchivoAsync(doc);
            srv.Delete(documentoID);
            if (persistir)
            {
                await srv.ApplyChangesAsync();
            }
        }

        public async Task ObtenerDocumento(long documentoID, Stream stream)
        {
            IDocumentosServicio srv = servicios.DocumentosServicio();
            Documento doc = srv.GetSingle(d => d.DocumentoID == documentoID);
            await ObtenerDocumento(doc, stream);
        }

        public FileStream ObtenerDocumento(long documentoID)
        {
            IDocumentosServicio srv = servicios.DocumentosServicio();
            Documento doc = srv.GetSingle(d => d.DocumentoID == documentoID);
            return ObtenerDocumento(doc);
        }

        public FileStream ObtenerDocumento(Documento doc)
        {
            return Documento(doc);
        }

        public async Task ObtenerDocumento(Documento documento, Stream stream)
        {
            await DocumentoAsync(documento, stream);
        }
        public async Task ObtenerMiniatura(Documento documento, Stream stream)
        {
            await MiniaturaAsync(documento, stream);
        }

        public FileStream ObtenerMiniatura(long documentoID)
        {
            IDocumentosServicio srv = servicios.DocumentosServicio();
            Documento doc = srv.GetSingle(d => d.DocumentoID == documentoID);
            return ObtenerMiniatura(doc);
        }

        public FileStream ObtenerMiniatura(Documento doc)
        {
            return Miniatura(doc);
        }

        public async Task<Documento> NuevaImagen(HttpPostedFileBase archivo, string nombre = "")
        {
            if (string.IsNullOrEmpty(nombre))
            {
                nombre = archivo.FileName;
            }

            var doc = new Documento()
            {
                Tipo = DocumentoTipo.Imagen,
                Nombre = nombre,
                Mime = archivo.GetMime(),
                Fecha = DateTime.Now,
                Tamano = archivo.ContentLength,
            };
            doc.Ruta = await GuardarArchivoAsync(archivo, nombre);
            GuardarThumbnail(doc.Ruta, doc.Ruta);

            IDocumentosServicio srv = servicios.DocumentosServicio();
            srv.Insert(doc);
            await srv.ApplyChangesAsync();
            return doc;
        }

        public async Task<Documento> NuevoDocumento(HttpPostedFileBase archivo, string nombre = "")
        {
            if (string.IsNullOrEmpty(nombre))
            {
                nombre = archivo.FileName;
            }

            string mime = archivo.GetMime();
            var doc = new Documento()
            {
                Tipo = DocumentoTipo.Adjunto,
                Nombre = nombre,
                Mime = mime,
                Fecha = DateTime.Now,
                Tamano = archivo.ContentLength,
            };

            doc.Ruta = await GuardarArchivoAsync(archivo, nombre);

            IDocumentosServicio srv = servicios.DocumentosServicio();
            srv.Insert(doc);
            await srv.ApplyChangesAsync();
            return doc;
        }

        protected string GenerarNombreNuevo(string nombreArchivo)
        {
            return Path.GetFileNameWithoutExtension(nombreArchivo)
                                + "_" + Util.GenerarCadenaAleatoria(longitud: 5)
                                + Path.GetExtension(nombreArchivo);
        }
    }
}