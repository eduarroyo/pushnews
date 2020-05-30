using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PushNews.WebApp.Helpers
{
    public static class Util
    {
        /// <summary>
        /// Garantiza que la cadena no es null.
        /// </summary>
        public static string AsegurarNulos(string texto)
        {
            return texto == null ? "" : texto.Trim();
        }

        /// <summary>
        /// Devuelve el texto en modo título, según la cultura.
        /// Se utiliza como método de extensión de cualquier string.
        /// </summary>
        public static string Titulo(this string texto)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(texto);
        }

        /// <summary>
        /// Devuelve el texto en modo frase, capitalizando la primera letra y dejando en minúsculas
        /// el resto.
        /// Se utiliza como método de extensión de cualquier string.
        /// </summary>
        public static string Frase(this string texto)
        {
            if(string.IsNullOrEmpty(texto))
            {
                return string.Empty;
            }

            return char.ToUpper(texto[0]) + texto.Substring(startIndex: 1);
        }

        public static string QuitarMascaraCuentaBancaria(string cuenta)
        {
            return cuenta
                .Replace(oldValue: "-", newValue: "")
                .Replace(oldValue: " ", newValue: "");
        }

        public static string GenerarCadenaAleatoria(int longitud)
        {
            string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var builder = new System.Text.StringBuilder(longitud);
            var rnd = new Random(DateTime.Now.Millisecond);
            int top = caracteres.Length - 1;

            for(var i = 0; i < longitud; i++)
            {
                int indice = rnd.Next() % top;
                builder.Append(caracteres.ElementAt(indice));
            }

            return builder.ToString();
        }

        public static string GetMime(this HttpPostedFileBase file)
        {
            return GetMime(file.FileName);
        }

        public static string GetMime(string fileName)
        {
            return MimeMapping.GetMimeMapping(fileName);
        }

        public static string IconoFichero(string mime, string extension)
        {
            return "";
        }

        public static IEnumerable<string> SerializarErroresModelo(ModelStateDictionary modelState)
        {
            return modelState
                .Where(m => m.Value.Errors.Any()) // Filtrar los campos con error.
                .Select(m => m.Value.Errors.ToList()) // Obtener la lista de listas de errores (cada campo podría tener varios).
                .SelectMany(x => x) // Convertir la lista de listas en una sola lista de ModelError.
                .Select(e => e.ErrorMessage); // Nos quedamos sólo con el ErrorMessage de cada ModelError.
        }
    }
}