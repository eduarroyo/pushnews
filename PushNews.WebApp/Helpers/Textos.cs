using System;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;
using Txt = PushNews.WebApp.App_LocalResources;
using PushNews.Dominio.Enums;

namespace PushNews.WebApp.Helpers
{
    public static class Textos
    {

        /// <summary>
        /// Obtiene el valor del atributo Display de una propiedad de modelo. Sirve para obtener
        /// el texto de etiqueta de un campo a partir de su propiedad de modelo correspondiente.
        /// </summary>
        /// <typeparam name="TModel">Tipo del modelo.</typeparam>
        /// <param name="model">Instancia del modelo.</param>
        /// <param name="propertyName">Nombre de la propiedad cuyo display name queremos obtener.</param>
        /// <returns>Display name de la propiedad.</returns>
        /// <remarks>http://stackoverflow.com/questions/5474460/get-displayname-attribute-of-a-property-in-strongly-typed-way</remarks>
        public static string GetDisplayName<TModel>(this TModel model, string propertyName)
        {
            Type modelType = model.GetType();
            DisplayAttribute attr;
            attr = (DisplayAttribute) modelType.GetProperty(propertyName)
                .GetCustomAttributes(typeof(DisplayAttribute), inherit: true)
                .SingleOrDefault();

            if (attr == null)
            {
                var metadataType = (MetadataTypeAttribute) modelType
                    .GetCustomAttributes(typeof(MetadataTypeAttribute), inherit: true)
                    .FirstOrDefault();
                if (metadataType != null)
                {
                    PropertyInfo property = metadataType.MetadataClassType.GetProperty(propertyName);
                    if (property != null)
                    {
                        attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayNameAttribute), inherit: true).SingleOrDefault();
                    }
                }
            }
            return (attr != null) ? attr.Name : String.Empty;
        }

        /// <summary>
        /// Obtiene el valor del atributo Display de una propiedad de modelo. Sirve para obtener
        /// el texto de etiqueta de un campo a partir de su propiedad de modelo correspondiente.
        /// </summary>
        /// <typeparam name="TModel">Tipo del modelo.</typeparam>
        /// <typeparam name="TProperty">Tipo de la propiedad del modelo cuyo display name queremos obtener.</typeparam>
        /// <param name="model">Instancia del modelo.</param>
        /// <param name="expression">Lambda para acceder a la propiedad cuyo display name queremos obtener.</param>
        /// <returns>Display name de la propiedad.</returns>
        /// <remarks>http://stackoverflow.com/questions/5474460/get-displayname-attribute-of-a-property-in-strongly-typed-way</remarks>
        public static string GetDisplayName<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            var memberExpression = (MemberExpression)expression.Body;
            return model.GetDisplayName(memberExpression.Member.Name);
        }

        public static string AplicacionCaracteristica(AplicacionCaracteristica caracteristica)
        {
            switch (caracteristica)
            {
                case Dominio.Enums.AplicacionCaracteristica.DirectorioComercial:
                    return Txt.Listas.AplicacionCaracteristicaDirectorioComercial;

                case Dominio.Enums.AplicacionCaracteristica.ImagenesMultiples:
                    return Txt.Listas.AplicacionCaracteristicaImagenesMultiples;

                case Dominio.Enums.AplicacionCaracteristica.AdjuntosMultiples:
                    return Txt.Listas.AplicacionCaracteristicaAdjuntosMultiples;

                case Dominio.Enums.AplicacionCaracteristica.AdjuntarVideo:
                    return Txt.Listas.AplicacionCaracteristicaAdjuntarVideo;

                case Dominio.Enums.AplicacionCaracteristica.Empresas:
                    return Txt.Listas.AplicacionCaracteristicaEmpresas;

                default:
                    return "";
            }
        }

        public static string EstadoComunicacion(EstadosPublicacion estado)
        {
            switch (estado)
            {
                case EstadosPublicacion.Atencion:
                    return Txt.Listas.EstadosPublicacionAtencion;
                case EstadosPublicacion.Enviado:
                    return Txt.Listas.EstadosPublicacionEnviado;
                case EstadosPublicacion.Planificado:
                    return Txt.Listas.EstadosPublicacionPlanificado;
                case EstadosPublicacion.Recordar:
                    return Txt.Listas.EstadosPublicacionRecordar;
                case EstadosPublicacion.Caducado:
                    return Txt.Listas.EstadosPublicacionCaducado;
                default:
                    return "";
            }
        }

        public static string EstadoComunicacionTitle(EstadosPublicacion estado)
        {
            switch (estado)
            {
                case EstadosPublicacion.Atencion:
                    return Txt.Listas.EstadosPublicacionAtencionTitle;
                case EstadosPublicacion.Enviado:
                    return Txt.Listas.EstadosPublicacionEnviadoTitle;
                case EstadosPublicacion.Planificado:
                    return Txt.Listas.EstadosPublicacionPlanificadoTitle;
                case EstadosPublicacion.Recordar:
                    return Txt.Listas.EstadosPublicacionRecordarTitle;
                case EstadosPublicacion.Caducado:
                    return Txt.Listas.EstadosPublicacionCaducadoTitle;
                default:
                    return "";
            }
        }

        public static string EstadoComunicacionIcono(EstadosPublicacion estado)
        {
            switch (estado)
            {
                case EstadosPublicacion.Atencion:
                    return "fa-exclamation-triangle";
                case EstadosPublicacion.Caducado:
                    return "fa-exclamation-circle";
                case EstadosPublicacion.Enviado:
                    return "fa-send";
                case EstadosPublicacion.Planificado:
                    return "fa-hourglass-2";
                case EstadosPublicacion.Recordar:
                    return "fa-calendar";
                default:
                    return "fa-question";
            }
        }
    }
}