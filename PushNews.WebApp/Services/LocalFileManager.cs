using System.IO;
using System.Web;
using System;
using PushNews.WebApp.Helpers;
using log4net;
using Txt = PushNews.WebApp.App_LocalResources;
using System.Threading.Tasks;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using System.Drawing;

namespace PushNews.WebApp.Services
{
    public class LocalFileManager : FileManagerBase, IFileManager
    {
        private readonly string raiz;
        private readonly string sufijoMiniatura = "_thumb";

        public LocalFileManager(IServiciosFactoria servicios)
        {
            base.servicios = servicios;
            log = LogManager.GetLogger(this.GetType());
            Parametro paramRaiz = servicios.ParametrosServicio().GetByName("RaizDocumentos");
            if (paramRaiz == null)
            {
                log.Error(message: "No se pudo obtener el parámetro de configuración \"RaizDocumentos\".");
                throw new Exception(Txt.FileManager.ParamRaizNoEncontrado);
            }
            raiz = paramRaiz.Valor;
        }

        //public void Dispose()
        //{
        //    // TODO: Implement this method
        //    // throw new NotImplementedException();
        //}

        public static IFileManager Create(IServiciosFactoria servicios)
        {
            return new LocalFileManager(servicios);
        }

        protected override async Task<string> GuardarArchivoAsync(HttpPostedFileBase archivo, string nombre)
        {
            return await Task.Run(() => GuardarArchivo(archivo, nombre));
        }

        private string GuardarArchivo(HttpPostedFileBase archivo, string nombreArchivo)
        {
            string nombreNuevo = GenerarNombreNuevo(nombreArchivo);
            if(!Directory.Exists(raiz))
            {
                Directory.CreateDirectory(raiz);
            }
            archivo.SaveAs(Path.Combine(raiz, nombreNuevo));
            return nombreNuevo;
        }

        protected override async Task<Tuple<string, long>> GuardarArchivoAsync(string archivo, string nombreArchivo)
        {
            return await Task.Run(() => GuardarArchivo(archivo, nombreArchivo));
        }

        private Tuple<string, long> GuardarArchivo(string archivo, string nombreArchivo)
        {
            long tamano;
            string nombreNuevo = Path.GetFileNameWithoutExtension(nombreArchivo)
                                + "_" + Util.GenerarCadenaAleatoria(longitud: 5)
                                + Path.GetExtension(nombreArchivo);
            using (FileStream fs = new FileStream(Path.Combine(raiz, nombreNuevo), FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(archivo);
                    bw.Write(data);
                    tamano = fs.Length;
                    bw.Close();
                }
                fs.Close();

            }
            return new Tuple<string, long>(nombreNuevo, tamano);
        }

        protected override async Task EliminarArchivoAsync(Documento doc)
        {
            await Task.Run(() => EliminarArchivo(doc));
        }

        /// <summary>
        /// Elimina el archivo referenciado por el documento, así como la miniatura si existiera.
        /// </summary>
        private void EliminarArchivo(Documento doc)
        {
            string absoluta = Path.Combine(raiz, doc.Ruta);
            string thumb = Path.Combine(Path.GetDirectoryName(absoluta), 
                Path.GetFileNameWithoutExtension(absoluta) + sufijoMiniatura + Path.GetExtension(absoluta));
            if (File.Exists(absoluta))
            {
                File.Delete(absoluta);
            }
            if(File.Exists(thumb))
            {
                File.Delete(thumb);
            }
        }

        protected override void GuardarThumbnail(string rutaImagen, string nombreArchivo)
        {
            
            byte[] fileBytes = File.ReadAllBytes(Path.Combine(raiz, rutaImagen));

            Image original;
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(fileBytes, 0, fileBytes.Length);
                original = Image.FromStream(ms);
            }
            
            int nuevoAlto = 195 * original.Height / original.Width;
            Image thumbnail = original.GetThumbnailImage(195, nuevoAlto, () => false, IntPtr.Zero);
            string nombreThumbnail = Path.GetFileNameWithoutExtension(nombreArchivo) + sufijoMiniatura
                                        + Path.GetExtension(nombreArchivo);
            thumbnail.Save(Path.Combine(raiz, nombreThumbnail));
        }

        protected override async Task DocumentoAsync(Documento documento, Stream stream)
        {
            string absoluta = Path.Combine(raiz, documento.Ruta);
            using (var reader = File.OpenRead(absoluta))
            {
                await reader.CopyToAsync(stream);
            }
        }

        protected override FileStream Documento(Documento documento)
        {
            string absoluta = Path.Combine(raiz, documento.Ruta);
            return File.OpenRead(absoluta);
        }

        protected override async Task MiniaturaAsync(Documento documento, Stream stream)
        {
            string relativa = Path.GetFileNameWithoutExtension(documento.Ruta) + "_thumb" + Path.GetExtension(documento.Ruta);
            string absoluta = Path.Combine(raiz, relativa);
            if(!File.Exists(absoluta))
            {
                absoluta = Path.Combine(raiz, documento.Ruta);
            }
            using (var reader = File.OpenRead(absoluta))
            {
                await reader.CopyToAsync(stream);
            }
        }


        protected override FileStream Miniatura(Documento documento)
        {
            string relativa = Path.GetFileNameWithoutExtension(documento.Ruta) + "_thumb" + Path.GetExtension(documento.Ruta);
            string absoluta = Path.Combine(raiz, relativa);
            if (!File.Exists(absoluta))
            {
                absoluta = Path.Combine(raiz, documento.Ruta);
            }
            return File.OpenRead(absoluta);
        }
    }
}