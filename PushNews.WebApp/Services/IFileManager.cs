using System;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using PushNews.Dominio.Entidades;

namespace PushNews.WebApp.Services
{
    public interface IFileManager : IDisposable
    {
        Task ObtenerDocumento(long documentoID, Stream stream);
        FileStream ObtenerDocumento(Documento documento);
        FileStream ObtenerDocumento(long documentoID);

        Task ObtenerDocumento(Documento documento, Stream stream);

        Task ObtenerMiniatura(Documento documento, Stream stream);
        FileStream ObtenerMiniatura(Documento documento);
        FileStream ObtenerMiniatura(long documentoID);

        Task EliminarDocumento(long documentoID, bool persistir = true);

        Task<Documento> NuevoDocumento(HttpPostedFileBase archivo, string nombre = "");

        Task<Documento> NuevaImagen(HttpPostedFileBase archivo, string nombre = "");
    }
}