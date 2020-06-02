(function ($, k, util, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de empresas: la aplicación es null.");
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
            $.extend(textos, contenedor.data("textos"));
            modulosInternos.empresasGrid =
                ModuloGrilla(_contenedor, _url, "EmpresaID", _prefijoEventos, {
                    edicionInline: false,
                    confirmarActivarDesactivar: true,
                    textoConfirmacion: function (reg) {
                        return reg.Nombre;
                    }
                });
            modulosInternos.empresasGrid.init();
            _contenedor.on(_prefijoEventos + "_fin_edicion", function () {
                util.resetUpload(uploadImagen);
            });

            // Obtener el objeto del control kendo upload para el logotipo y manejar el evento de exito
            // para recuperar los datos devueltos por la acción (el id y la url del documento creado).
            uploadImagen = $("#LogotipoAdjunto").data("kendoUpload");
            uploadImagen.bind("success", function (e) {
                var model = modulosInternos.empresasGrid.registroEnEdicion();
                if (!e.response || !e.response.DocumentoID) {
                    app.notificarError(textos.errorSubirFichero);
                    model.set("LogotipoDocumentoID", null);
                    model.set("LogotipoUrl", null);
                    util.resetUpload(uploadImagen);
                } else {
                    model.set("LogotipoUrl", e.response.Url);
                    model.set("LogotipoDocumentoID", e.response.DocumentoID);
                }
            });
            uploadImagen.bind("error", function (e) {
                app.notificarError(textos.errorSubirFichero);
            });

            // Obtener el objeto del control kendo upload para el logotipo y manejar el evento de exito
            // para recuperar los datos devueltos por la acción (el id y la url del documento creado).
            uploadImagen2 = $("#BannerAdjunto").data("kendoUpload");
            uploadImagen2.bind("success", function (e) {
                var model = modulosInternos.empresasGrid.registroEnEdicion();
                if (!e.response || !e.response.DocumentoID) {
                    app.notificarError(textos.errorSubirFichero);
                    model.set("BannerDocumentoID", null);
                    model.set("BannerUrl", null);
                    util.resetUpload(uploadImagen2);
                } else {
                    model.set("BannerUrl", e.response.Url);
                    model.set("BannerDocumentoID", e.response.DocumentoID);
                }
            });
            uploadImagen2.bind("error", function (e) {
                app.notificarError(textos.errorSubirFichero);
            });

            configurarDialogoCoordenadas();
        }

        function configurarDialogoCoordenadas() {
            dialogoCoordenadas = _contenedor.find("#DialogoCoordenadas");
            botonSeleccionarCoordenadas = _contenedor.find("#btSeleccionarCoordenadasEmpresa");

            if (!dialogoCoordenadas.length) {
                console.error("No se encontró el componente selección de coordenadas en el DOM.");
                return;
            }

            dialogoCoordenadas.on("error_geocoding", function (ev, mensaje) {
                app.notificarError(textos.tituloErrorGeocoding, mensaje);
            });

            dialogoCoordenadas.on("alerta_geocoding", function (ev, mensaje) {
                app.notificarAdvertencia(textos.tituloErrorGeocoding, mensaje);
            });

            botonSeleccionarCoordenadas.on("click", abrirDialogoCoordenadas);
            dialogoCoordenadas.on("hide.bs.modal", actualizarCoordenadas);
        }

        function abrirDialogoCoordenadas(ev) {
            ev.preventDefault();
            var empresa = modulosInternos.empresasGrid.registroEnEdicion();
            controlCoordenadas = new window.Coordenadas2(
                dialogoCoordenadas, {
                    latitud: empresa.Latitud,
                    longitud: empresa.Longitud,
                    direccion: empresa.Direccion,
                    codigoPostal: empresa.CodigoPostal,
                    localidad: empresa.Localidad,
                    provincia: empresa.Provincia
                },
                empresa.Direccion.length && !(empresa.Latitud && empresa.Longitud) ? empresa.Direccion : "");
            controlCoordenadas.abrirModal();
        }

        function actualizarCoordenadas() {
            if (controlCoordenadas) {
                var resultado = controlCoordenadas.resultado();
                if (resultado.resultadoDialogo === "ACEPTAR") {
                    var empresa = modulosInternos.empresasGrid.registroEnEdicion();
                    //console.debug("Modificación de coordenadas: ",
                    //   "(" + empresa.get("EntradaEnCarreraOficialLatitud") + ", " +
                    //   empresa.get("EntradaEnCarreraOficialLongitud") + ") => (" +
                    //   resultado.latitud + ", " + resultado.longitud + ")");
                    empresa.set("Latitud", resultado.latitud);
                    empresa.set("Longitud", resultado.longitud);
                    empresa.set("Direccion", resultado.direccion);
                    empresa.set("CodigoPostal", resultado.codigoPostal);
                    empresa.set("Localidad", resultado.localidad);
                    empresa.set("Provincia", resultado.provincia);
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

    })($("#target"), "/Backend/Empresas", "empresas", {});

    app.modulos["empresas"] = modulo;
})(jQuery, kendo, window.Util, window.app);