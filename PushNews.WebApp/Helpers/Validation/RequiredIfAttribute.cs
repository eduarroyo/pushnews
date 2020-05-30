using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Helpers;
using System.Web.Mvc;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Helpers.Validation
{
    public class RequiredIfAttribute : CustomValidationBaseAttribute, IClientValidatable
    {
        /// <summary>
        /// Lista de nombres de propiedades del modelo que deben ser true para que se aplique la
        /// validación.
        /// </summary>
        public IEnumerable<string> Condicionales { get; private set; }

        /// <summary>
        /// Opción para indicar si se considera o no válida una cadena vacía. Si es true, la cadena
        /// vacía es válida. El valor null nunca se considera válido. Todo esto se aplica sólo en
        /// el caso de que todos los condicionales sean true.
        /// </summary>
        public bool AllowEmptyStrings { get; set; }

        /// <summary>
        /// Establece que la propiedad debe tener un valor siempre y cuando todas las propiedades
        /// indicadas en <paramref name="condicionales"/> tengan valor true.
        /// </summary>
        /// <param name="condicionales">Lista de nombres de propiedades booleanas del modelo que deben ser true para que se aplique la validación. Si al menos una es false, entonces la propiedad se considera válida sea cual sea su valor.</param>
        /// <param name="allowEmptyStrings">Si vale true, se dan por válidas las cadenas vacías.</param>
        /// <param name="errorMessageResourceType">Establece el tipo de recurso del que se obtiene el texto de error de validación.</param>
        /// <param name="errorMessageResourceName">Establece el nombre de la propiedad de la clase de recursos que contiene el texto de error de validación.</param>
        /// <param name="errorMessage">Establece el texto de error de validación si no se da valor para los parámetros <paramref name="errorMessageResourceType"/> y <paramref name="errorMessageResourceName"/>.</param>
        public RequiredIfAttribute(string condicionales, bool allowEmptyStrings = false,
            Type errorMessageResourceType = null, string errorMessageResourceName = "",
            string errorMessage = "")
            : base()
        {
            AllowEmptyStrings = allowEmptyStrings;
            ErrorMessage = errorMessage;
            ErrorMessageResourceType = errorMessageResourceType;
            ErrorMessageResourceName = errorMessageResourceName;
            Condicionales = condicionales.Split(new char[] { ',' }).Select(c => c.Trim());
        }

        /// <summary>
        /// Aplica la lógica de validación a una propiedad.
        /// </summary>
        /// <returns>null si es válido y ValidationResult si no.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IEnumerable<PropertyInfo> propiedades = Condicionales.Select(p => validationContext.ObjectType.GetProperty(p));
            IEnumerable<bool> valoresCondicionales = propiedades.Select(p => p.GetValue(validationContext.ObjectInstance, index: null)).OfType<bool>();

            // Lógica de RequiredIf:
            // 1. Si el campo es distinto de null:
            //     1.1 Si el campo tiene contenido, entonces es válido.
            //     1.2 Si el campo es cadena vacía es válido si AllowEmptyStrings es true.
            if (value != null)
            {
                return !AllowEmptyStrings && value.ToString() == string.Empty
                    ? ErrorValidacion(validationContext)
                    : null;
            }

            // 2. Si el campo es null:
            //    2.1 El campo es válido si todas las propiedades indicadas en Condicionales valen true.
            else // valor == null
            {
                return valoresCondicionales.All(v => v == true)
                    ? ErrorValidacion(validationContext)
                    : null;
            }
        }

        /// <summary>
        /// Proporciona la configuración para la validación en cliente.
        /// </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "requiredif"
            };

            rule.ValidationParameters["condicionales"] = Json.Encode(Condicionales);
            rule.ValidationParameters["allowemptystrings"] = Json.Encode(AllowEmptyStrings);

            yield return rule;
        }
    }
}