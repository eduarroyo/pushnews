(function ($, k, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de editar: la aplicación es null.");
        return;
    }

    var modulo = (function (contenedor, url, prefijoEventos, opciones) {

        var _contenedor = contenedor, _url = url, _prefijoEventos = prefijoEventos,
            opDefault = {};
        var _opciones = $.extend({}, opDefault, opciones);
        var modulosInternos = {};
        var publicarComunicacion;

        function init() {
            contenedorEditar = _contenedor.find("#editarComunicacion");
            modulosInternos["formComunicaciones"] = app.modulos["formcomunicaciones"](contenedorEditar);
            modulosInternos["formComunicaciones"].init();


            contenedorEditar.on("comunicacionGuardada", function (e) {
                window.history.back();
            });
            
            id = contenedorEditar.data("id");
            textos = contenedorEditar.data("textos");

            if (id !== 0) {
                $.ajax({
                    url: "/Comunicaciones/LeerUno",
                    data: { comunicacionID: id },
                    method: "GET"
                }).success(function (data) {
                    if (data) {
                        if (data.FechaPublicacion && typeof data.FechaPublicacion === 'string') {
                            data.FechaPublicacion = window.Util.parseJsonDate(data.FechaPublicacion);
                        }
                        if (data.RecordatorioFecha && typeof data.RecordatorioFecha === 'string') {
                            data.RecordatorioFecha = window.Util.parseJsonDate(data.RecordatorioFecha);
                        }
                        modulosInternos["formComunicaciones"].establecerDatosComunicacion(data);
                    } else {
                        app.notificarError(textos.ErrorCargarNotificacion);
                    }
                }).error(function () {
                    app.notificarError(textos.ErrorCargarNotificacion);
                })
            }
        }

        function resize() {
            for (var m in modulosInternos) {
                modulosInternos[m].resize();
            }
        }

        function destroy() {
            for (var m in modulosInternos) {
                modulosInternos[m].destroy();
            }
        }

        return {
            url: url,
            init: init,
            resize: resize,
            destroy: destroy,
            contenedor: _contenedor
        }
    })($("#target"), "/Comunicaciones/Editar", "editar");

    app.modulos["editar"] = modulo;

})(jQuery, kendo, app);