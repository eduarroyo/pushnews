(function ($, k, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de perfil: la aplicación es null.");
        return;
    }

    var modulo = (function (contenedor, url, prefijoEventos, opciones) {

        var _contenedor = contenedor, _url = url, _prefijoEventos = prefijoEventos,
            opDefault = {
                Nombre: "",
                Apellidos: ""
            };
        var _textos = {};
        var _opciones = $.extend({}, opDefault, opciones);
        var contenedorModulo, contenedorPerfil, validadorPerfil, contenedorCambiarClave, validadorCambiarClave;

        var perfilModel = k.observable({
            Nombre: "",
            Apellidos: "",
            guardar: function () {
                if (validadorPerfil.validate()) {
                    $.ajax({
                        type: 'POST',
                        url: "/Backend/Perfiles/GuardarPerfil",
                        data: JSON.stringify(perfilModel),
                        dataType: "json",
                        contentType: "application/json"
                    }).done(function (data) {
                        if (data.Errors && data.Errors.length) {
                            for (var e in data.Errors) {
                                app.notificarError(data.Errors[e]);
                            }
                        } else {
                            app.notificarExito(_textos.perfilActualizado);
                            $(".navbar .username").html(perfilModel.get("Nombre") + " " + perfilModel.get("Apellidos"));
                        }

                    });
                }
            }
        });

        var cambiarClaveModel = k.observable({
            ClaveActual: "",
            ClaveNueva: "",
            ConfirmarClave: "",
            guardar: function () {
                if (validadorCambiarClave.validate()) {
                    $.ajax({
                        type: 'POST',
                        url: "/Backend/Account/CambiarClave",
                        data: JSON.stringify(cambiarClaveModel),
                        dataType: "json",
                        contentType: "application/json"
                    }).done(function (data) {
                        if (data.Errors && data.Errors.length) {
                            for (var e in data.Errors) {
                                app.notificarError(data.Errors[e]);
                            }
                        } else {
                            app.notificarExito(_textos.perfilActualizado);
                            cambiarClaveModel.set("ClaveActual", "");
                            cambiarClaveModel.set("ClaveNueva", "");
                            cambiarClaveModel.set("ConfirmarClave", "");
                        }
                    });
                }
            }
        });

        function init() {
            contenedorModulo = _contenedor.find(".contenedor-modulo");
            $.extend(_textos, contenedorModulo.data("textos"));
            _opciones = $.extend({}, opDefault, contenedorModulo.data("opciones"), opciones)
            perfilModel.set("Nombre", _opciones.Nombre);
            perfilModel.set("Apellidos", _opciones.Apellidos);
            contenedorPerfil = _contenedor.find(".cambiar-nombre");
            validadorPerfil = contenedorPerfil.find("form").kendoValidator().data("kendoValidator");
            k.bind(contenedorPerfil, perfilModel);

            contenedorCambiarClave = _contenedor.find(".cambiar-clave");
            validadorCambiarClave = contenedorCambiarClave.find("form").kendoValidator().data("kendoValidator");
            k.bind(contenedorCambiarClave, cambiarClaveModel);


        }
        
        function resize() {
        }

        function destroy() {
        }

        return {
            // Acciones del escritorio
            url: url,
            init: init,
            resize: resize,
            destroy: destroy,
            contenedor: _contenedor
        }
    })($("#target"), "/Backend/Perfiles/Perfil", "perfil");

    app.modulos["perfil"] = modulo;

})(jQuery, kendo, app);