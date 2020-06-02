(function ($, k, util, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de rutas: la aplicación es null.");
        return;
    }

    var modulo = (function (contenedor, url, prefijoEventos, opciones) {
        var _contenedor = contenedor, _url = url, _prefijoEventos = prefijoEventos,
            opDefault = {};
        var _opciones = $.extend({}, opDefault, opciones);
        var modulosInternos = {};
        var textos = {
            TituloErrorGeocoding: "[] Error de geolocalización"
        };
        var botonSeleccionarCoordenadas, dialogoCoordenadas, controlCoordenadas;

        function init() {
            $.extend(textos, _contenedor.data("textos"));
            $.extend(_opciones, _contenedor.data("opciones"));

            modulosInternos.rutasGrid =
                ModuloGrilla(_contenedor, _url, "RutaID", _prefijoEventos, {
                    edicionInline: false,
                    confirmarActivarDesactivar: true,
                    textoConfirmacion: function (reg) {
                        return reg.Descripcion;
                    }
                });
            modulosInternos.rutasGrid.init();

            configurarDialogoCoordenadas();
        }

        function configurarDialogoCoordenadas() {
            dialogoCoordenadas = _contenedor.find("#DialogoCoordenadas");
            botonSeleccionarCoordenadas = _contenedor.find("#btSeleccionarCoordenadasCO");

            if(!dialogoCoordenadas.length) {
                console.error("No se encontró el componente selección de coordenadas en el DOM.");
                return;
            }
            
            dialogoCoordenadas.on("error_geocoding", function (ev, mensaje) {
                app.notificarError(textos.TituloErrorGeocoding, mensaje);
            });

            dialogoCoordenadas.on("alerta_geocoding", function (ev, mensaje) {
                app.notificarAdvertencia(textos.TituloErrorGeocoding, mensaje);
            });
                
            botonSeleccionarCoordenadas.on("click", abrirDialogoCoordenadas);
            dialogoCoordenadas.on("hide.bs.modal", actualizarCoordenadas);
        }

        function abrirDialogoCoordenadas(ev) {
            ev.preventDefault();
            var ruta = modulosInternos.rutasGrid.registroEnEdicion();
            controlCoordenadas = new window.Coordenadas2(
                dialogoCoordenadas, {
                    latitud: ruta.EntradaEnCarreraOficialLatitud,
                    longitud: ruta.EntradaEnCarreraOficialLongitud
                });
            controlCoordenadas.abrirModal();
        }

        function actualizarCoordenadas() {
            if (controlCoordenadas) {
                var resultado = controlCoordenadas.resultado();
                if (resultado.resultadoDialogo === "ACEPTAR") {
                    var ruta = modulosInternos.rutasGrid.registroEnEdicion();
                    //console.debug("Modificación de coordenadas: ",
                    //   "(" + ruta.get("EntradaEnCarreraOficialLatitud") + ", " +
                    //   ruta.get("EntradaEnCarreraOficialLongitud") + ") => (" +
                    //   resultado.latitud + ", " + resultado.longitud + ")");
                    ruta.set("EntradaEnCarreraOficialLatitud", resultado.latitud);
                    ruta.set("EntradaEnCarreraOficialLongitud", resultado.longitud);
                }
                controlCoordenadas = null;
            }
        }

        function resize() {
        }

        function destroy() {
            _contenedor.off(_prefijoEventos + "fin_edicion");
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
        };

    })($("#target"), "/Backend/Rutas", "rutas", {});

    app.modulos["rutas"] = modulo;
})(jQuery, kendo, window.Util, window.app);