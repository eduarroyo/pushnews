(function ($, k, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de comunicaciones: la aplicación es null.");
        return;
    }

    var modulo = (function (contenedor, url, prefijoEventos, opciones) {
        var _contenedor = contenedor, _url = url, _prefijoEventos = prefijoEventos,
            opDefault = {};
        var _opciones = $.extend({}, opDefault, opciones);
        var modulosInternos = {};

        function init() {
            textos = _contenedor.data("textos");
            modulosInternos.comunicacionesGrilla = ModuloGrilla(_contenedor, url, "ComunicacionID", prefijoEventos,
            {
                accionAlternativaActivarDesactivar: "Comunicaciones/ActivarDesactivar",
                edicionInline: false,
                confirmarActivarDesactivar: true,
                textoConfirmacion: function (reg) {
                    return reg.Titulo;
                }
            });

            modulosInternos.comunicacionesGrilla.init();

            var grid = modulosInternos.comunicacionesGrilla.grid();
            grid.element.on("click", ".btAgregar", function (e) {
                e.preventDefault();
                document.location = "#/editar";
            });

            grid.tbody.off("click", ".btEditar");
            grid.tbody.on("click", ".btEditar", function (e) {
                e.preventDefault();
                var bt = $(e.target);
                var fila = bt.parents("tr");
                var registro = grid.dataItem(fila);

                document.location = "#/editar/" + registro.ComunicacionID;
            })
        }

        function resize() {
        }

        function destroy() {;
        }

        return {
            url: url,
            init: init,
            resize: resize,
            destroy: destroy,
            contenedor: _contenedor
        }
    })($("#target"), "/Backend/Comunicaciones", "comunicaciones");

    app.modulos["comunicaciones"] = modulo;

})(jQuery, kendo, window.app);