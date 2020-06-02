(function ($, k, util, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de hermandades: la aplicación es null.");
        return;
    }

    var modulo = (function (contenedor, url, prefijoEventos, opciones) {
        var _contenedor = contenedor, _url = url, _prefijoEventos = prefijoEventos,
            opDefault = {};
        var _opciones = $.extend({}, opDefault, opciones);
        var modulosInternos = {};
        var textos = {};
        var uploadImagen;

        function init() {
            modulosInternos.hermandadesGrid =
                ModuloGrilla(_contenedor, _url, "HermandadID", _prefijoEventos, {
                    edicionInline: false,
                    confirmarActivarDesactivar: true,
                    textoConfirmacion: function (reg) {
                        return reg.Nombre;
                    }
                });
            modulosInternos.hermandadesGrid.init();
            _contenedor.on(_prefijoEventos + "_fin_edicion", function () {
                util.resetUpload(uploadImagen);
            });

            // Obtener el objeto del control kendo upload para el logotipo y manejar el evento de exito
            // para recuperar los datos devueltos por la acción (el id y la url del documento creado).
            uploadImagen = $("#LogotipoAdjunto").data("kendoUpload");
            uploadImagen.bind("success", function (e) {
                var model = modulosInternos.hermandadesGrid.registroEnEdicion();
                if (!e.response || !e.response.DocumentoID) {
                    app.notificarError(textosFormComunicacion.ErrorSubirFichero);
                    model.set("LogotipoDocumentoID", null);
                    model.set("LogotipoUrl", null);
                    util.resetUpload(uploadImagen);
                } else {
                    model.set("LogotipoUrl", e.response.Url);
                    model.set("LogotipoDocumentoID", e.response.DocumentoID);
                }
            });
            uploadImagen.bind("error", function (e) {
                app.notificarError(textosFormComunicacion.ErrorSubirFichero);
            });
            configurarDialogoCoordenadas();
        }


        function configurarDialogoCoordenadas() {
            dialogoCoordenadas = _contenedor.find("#DialogoCoordenadas");
            botonSeleccionarCoordenadas = _contenedor.find("#btSeleccionarCoordenadasIglesia");

            if (!dialogoCoordenadas.length) {
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
            var hermandad = modulosInternos.hermandadesGrid.registroEnEdicion();
            controlCoordenadas = new window.Coordenadas2(
                dialogoCoordenadas, {
                    latitud: hermandad.IglesiaLatitud,
                    longitud: hermandad.IglesiaLongitud,
                    direccion: hermandad.IglesiaDireccion
                },
                hermandad.IglesiaDireccion.length && !(hermandad.IglesiaLatitud && hermandad.IglesiaLongitud) ? hermandad.IglesiaDireccion : "");
            controlCoordenadas.abrirModal();
        }

        function actualizarCoordenadas() {
            if (controlCoordenadas) {
                var resultado = controlCoordenadas.resultado();
                if (resultado.resultadoDialogo === "ACEPTAR") {
                    var hermandad = modulosInternos.hermandadesGrid.registroEnEdicion();
                    //console.debug("Modificación de coordenadas: ",
                    //   "(" + hermandad.get("EntradaEnCarreraOficialLatitud") + ", " +
                    //   hermandad.get("EntradaEnCarreraOficialLongitud") + ") => (" +
                    //   resultado.latitud + ", " + resultado.longitud + ")");
                    hermandad.set("IglesiaLatitud", resultado.latitud);
                    hermandad.set("IglesiaLongitud", resultado.longitud);
                    hermandad.set("IglesiaDireccion", resultado.direccion);
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

    })($("#target"), "/Backend/Hermandades", "hermandades", {});

    app.modulos["hermandades"] = modulo;
})(jQuery, kendo, window.Util, window.app);