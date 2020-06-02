(function ($, k, util, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de GPSs: la aplicación es null.");
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
            modulosInternos.gpssGrid =
                ModuloGrilla(_contenedor, _url, "GpsID", _prefijoEventos, {
                    edicionInline: false,
                    confirmarActivarDesactivar: true,
                    textoConfirmacion: function (reg) {
                        return reg.Matricula;
                    }
                });
            modulosInternos.gpssGrid.init();

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

    })($("#target"), "/Backend/Gpss", "gpss", {});

    app.modulos["gpss"] = modulo;
})(jQuery, kendo, window.Util, window.app);