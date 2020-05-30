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
    public class RequeridoUnoDelGrupoAttribute: CustomValidationBaseAttribute, IClientValidatable
    {
        public string[] Propiedades { get; private set; }
        
        /// <summary>
        /// Opción para indicar si se considera o no válida una cadena vacía. Si es true, la cadena
        /// vacía es válida. El valor null nunca se considera válido. Todo esto se aplica sólo en
        /// el caso de que todos los condicionales sean true.
        /// </summary>
        public bool AllowEmptyStrings { get; set; }

        public RequeridoUnoDelGrupoAttribute(string[] propiedades, bool allowEmptyStrings = false, Type errorMessageResourceType = null, string errorMessageResourceName = "")
        {
            this.Propiedades = propiedades;
            ErrorMessageResourceType = errorMessageResourceType;
            ErrorMessageResourceName = errorMessageResourceName;
            AllowEmptyStrings = allowEmptyStrings;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IEnumerable<PropertyInfo> propiedades = this.Propiedades.Select(validationContext.ObjectType.GetProperty);
            IEnumerable<string> values = propiedades.Select(p => p.GetValue(validationContext.ObjectInstance, index: null)).OfType<string>();

            // TODO No he conseguido obtener los DisplayNames de los tres campos del grupo.
            // El problema tiene que ver con la obtención mediante reflection de las propiedades,
            // porque el objeto que la instancia que obtenemos del validationContext es un Object
            // y no un objeto del tipo del modelo correspondiente (que debe ser genérico). El
            // resultado es que al obtener la propiedad tenemos null, y por lo tanto imposible
            // acceder a los metadatos y al displayName de la propiedad. Ahora mismo se están
            // utilizando provisionalmente los nombres de las propiedades tal como están en
            // programación. Los display names son los mismos textos traducidos que aparecen en las
            // etiquetas del formulario.
            //var m = Convert.ChangeType(validationContext.ObjectInstance, validationContext.ObjectType);
            //var displayNames = propiedades.Select(p => m.GetDisplayName(p.Name));

            IEnumerable<string> displayNames = propiedades.Select(p => p.Name); // <- Código provisional.

            if(values.All(string.IsNullOrWhiteSpace))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            TextoErrorPorDefecto = Txt.Validacion.RequeridoUnoDelGrupo;
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
            rule.ValidationType = "requeridounodelgrupo";

            rule.ValidationParameters["grupo"] = Json.Encode(Propiedades);
            rule.ValidationParameters["allowemptystrings"] = Json.Encode(AllowEmptyStrings);

            yield return rule;
        }
    }
}