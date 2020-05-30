using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Helpers.Validation
{
    public abstract class CustomValidationBaseAttribute: ValidationAttribute
    {
        protected virtual string TextoErrorPorDefecto { get; set; }

        /// <summary>
        /// Genera el objeto a devolver por IsValid cuando la propiedad tiene un valor no admitido.
        /// </summary>
        protected ValidationResult ErrorValidacion(ValidationContext validationContext)
        {
            string msg = FormatErrorMessage(validationContext.DisplayName);
            var vr = new ValidationResult(msg, new[] { validationContext.MemberName });
            return vr;
        }

        /// <summary>
        /// Genera el mensaje de error cuando la propiedad no es válida.
        /// Primero intenta obtener el texto de un recurso mediante ErrorMessageResourceType y ErrorMessageResourceName.
        /// Si no se han especificado valores para esas propiedades, se intenta obtener el texto de ErrorMessage.
        /// Si tampoco tiene valor, se utiliza un texto por defecto (internacionalizado).
        /// </summary>
        public override string FormatErrorMessage(string propertyDisplayName)
        {
            string plantillaMensajeError;
            if (ErrorMessageResourceType != null && !string.IsNullOrWhiteSpace(ErrorMessageResourceName))
            {
                PropertyInfo prop = ErrorMessageResourceType.GetProperty(ErrorMessageResourceName, BindingFlags.Public | BindingFlags.Static);
                plantillaMensajeError = (string)prop.GetValue(obj: null, index: null);
            }
            else if (ErrorMessage != null)
            {
                plantillaMensajeError = ErrorMessage;
            }
            else
            {
                plantillaMensajeError = TextoErrorPorDefecto;
            }
            return string.Format(plantillaMensajeError, propertyDisplayName);
        }
    }
}