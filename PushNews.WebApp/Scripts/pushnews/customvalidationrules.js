/*  Reglas de validación en cliente.
 *
 *  Este archivo contiene las reglas de validación en cliente para kendo-validator
 *  correspondientes a las reglas de validación en servidor que se encuentran en
 *  PushNews.WebApp/Helpers/Validation.
 *
 *  Todas las validaciones se realizan primero en cliente y luego en servidor.
 */

(function ($, kendo) {
    $.extend(true, kendo.ui.validator, {
        rules: {
            // Regla de validación "requerido" condicionada a uno o varios campos booleanos 
            // del formulario.
            // Esta regla valida el campo si tiene contenido o bien alguno de los campos
            // indicados en data-val-requiredif-condicionales tiene valor false.
            requiredif: function (input) {
                var condicionales, c, valorC, permiteBlanco, valor;
                if (input.is("[data-val-requiredif]")) {

                    // Valor del imput a validar.
                    valor = input.val();

                    // Opción de configuración allowemptystrings: distinguir cadena vacía de null.
                    // si es true, se considera válida la cadena vacía y si es false, no.
                    permiteBlanco = $(input).data("val-requiredif-allowemptystrings");

                    // Si el campo tiene contenido es válido y no hay que comprobar las condiciones.
                    if (valor !== null && (permiteBlanco || valor.length > 0)) {
                        return true;
                    } else {
                        // Comprobar si alguno de los condicionales vale false.
                        // en ese caso, el campo se dará por válido aunque no tenga valor.
                        condicionales = $(input).data("val-requiredif-condicionales");
                        for (c in condicionales) {
                            var item = $("#" + condicionales[c]);
                            if(item.is("[type=checkbox]")) {
                                valorC = item[0].checked;
                            } else {
                                valorC = item.val() === "true"; // La comparación con "true" proporciona el valor en bool. 
                            }
                            if (!valorC) {                                       // El campo del dom lo guarda en string.
                                return true;
                            }
                        }

                        // Llegados a este punto, sabemos que el campo no tiene valor y que
                        // las condicionales indican que debería tenerlo, por lo tanto se da por
                        // no válido devolviendo false.
                        return false;
                    }
                }

                return true;
            },

            // Regla de validación "requeridounodelgrupo" obliga a que se rellene al menos un campo
            // de un conjunto.
            requeridounodelgrupo: function (input) {
                var grupo, c, valorC, permiteBlanco, valor;
                if (input.is("[data-val-requeridounodelgrupo]")) {

                    // Opción de configuración allowemptystrings: distinguir cadena vacía de null.
                    // si es true, se considera válida la cadena vacía y si es false, no.
                    permiteBlanco = $(input).data("val-requeridounodelgrupo-allowemptystrings");

                    // Se comprueba primero el propio campo al que se le ha puesto la regla de validación.
                    // Si el campo contiene texto, es válido.
                    // Si el campo contiene cadena vacía y se ha configurado para aceptar cadena vácía, es válido.
                    // En cualquier otro caso, hay que comprobar el grupo de campos para ver si alguno tiene contenido.
                    valor = input.val();
                    if (valor !== null && (permiteBlanco || valor.length > 0)) {
                        return true;
                    }

                    // Obtener la lista de inputs que entran en el grupo a validar.
                    grupo = $(input).data("val-requeridounodelgrupo-grupo");

                    // Para cada elemento del grupo, se comprueba la validez de la misma forma que se hizo antes
                    // con el campo principal. Si uno es válido, se considera válido el campo principal.
                    for (c in grupo) {
                        valorC = $("#" + grupo[c]).val();
                        if (valorC !== null && (permiteBlanco || valorC.length > 0)) {
                            return true;
                        }
                    }

                    // Si se llega a este punto, es que no es válido el campo principal y tampoco ninguno de
                    // los campos del grupo. Indicamos campo no válido devolviendo false.
                    return false;
                }

                return true;
            }
        },
        messages: {
            // Obtener el mensaje de error para la regla de validación requiredif del atributo
            // data-val-requiredif del input.
            requiredif: function (input) {
                return input.attr("data-val-requiredif");
            },
            // Obtener el mensaje de error para la regla de validación requeridounodelgrupo del
            // atributo data-val-requeridounodelgrupo del input.
            requeridounodelgrupo: function (input) {
                return input.attr("data-val-requeridounodelgrupo");
            }
        }
    });
})(jQuery, kendo);