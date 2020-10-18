(function ($, k, util, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de aplicaciones: la aplicación es null.");
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
            modulosInternos.aplicacionesGrid =
                ModuloGrilla(_contenedor, _url, "AplicacionID", _prefijoEventos, {
                    edicionInline: false,
                    confirmarActivarDesactivar: true,
                    textoConfirmacion: function (reg) {
                        return reg.Nombre;
                    }
                });
            modulosInternos.aplicacionesGrid.init();
            _contenedor.on(_prefijoEventos + "_fin_edicion", function () {
                util.resetUpload(uploadImagen);
            });

            _contenedor.on(_prefijoEventos + "_edit", function () {
                var regEdicion = modulosInternos.aplicacionesGrid.registroEnEdicion();

                if (regEdicion.get("AplicacionID") === 0) {
                    regEdicion.set("ApiKey", util.generarCadenaAleatoria(20));
                }
            });

            modulosInternos.aplicacionesGrid.grid().element.on("click", ".btClonar", clonarAplicacion);
            
            // Obtener el objeto del control kendo upload para el logotipo y manejar el evento de exito
            // para recuperar los datos devueltos por la acción (el id y la url del documento creado).
            uploadImagen = $("#LogotipoAdjunto").data("kendoUpload");
            uploadImagen.bind("success", function (e) {
                var model = modulosInternos.aplicacionesGrid.registroEnEdicion();
                if (!e.response || !e.response.DocumentoID) {
                    app.notificarError(textosFormComunicacion.ErrorSubirFichero);
                    model.set("LogotipoID", null);
                    model.set("LogotipoUrl", null);
                    util.resetUpload(uploadImagen);
                } else {
                    model.set("LogotipoUrl", e.response.Url);
                    model.set("LogotipoID", e.response.DocumentoID);
                }
            });
            uploadImagen.bind("error", function (e) {
                app.notificarError(textosFormComunicacion.ErrorSubirFichero);
            });
        }

        function clonarAplicacion(ev) {
            var fila = $(ev.target),
                grid = modulosInternos.aplicacionesGrid.grid(),
                dato;

            if (!fila.is("tr")) {
                fila = fila.parents("tr");
            }

            dato = grid.dataItem(fila);
            var nuevo = grid.dataSource.insert();
            nuevo.set("Tipo", dato.get("Tipo"));
            nuevo.set("Caracteristicas", dato.get("Caracteristicas"));
            nuevo.set("Activo", true);
            modulosInternos.aplicacionesGrid.editarRegistro(nuevo);
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

    })($("#target"), "/Backend/Aplicaciones", "aplicaciones", {});

    app.modulos["aplicaciones"] = modulo;
})(jQuery, kendo, window.Util, window.app);