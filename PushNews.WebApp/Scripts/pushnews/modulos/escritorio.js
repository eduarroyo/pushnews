(function ($, k, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de escritorio: la aplicación es null.");
        return;
    }

    app.modulos["escritorio"] = (function (contenedor, url) {

        var _contenedor = contenedor;
        var _url = url;
        var publicarComunicacion, estadisticas, ultimasComunicaciones;
        var listaUltimasComunicaciones, formPublicar, estadisticas;
        var modulosInternos = {
            formComunicaciones: {}
        };

        function init() {
            publicarComunicacion = _contenedor.find(".seccion-publicar");
            if (publicarComunicacion.length) {
                modulosInternos["formComunicaciones"] = app.modulos["formcomunicaciones"](publicarComunicacion);

                publicarComunicacion.find(".btCancelar").remove();

                modulosInternos["formComunicaciones"].init();

                publicarComunicacion.on("comunicacionGuardada", function () {
                    modulosInternos["formComunicaciones"].borrarDatosComunicacion();
                    listaUltimasComunicaciones.dataSource.read();
                });
            }
            
            ultimasComunicaciones = _contenedor.find(".seccion-ultimas-comunicaciones");
            if (ultimasComunicaciones.length) {
                listaUltimasComunicaciones = ultimasComunicaciones.find("#UltimasComunicaciones").data("kendoGrid");
                listaUltimasComunicaciones.bind("dataBound", function () {
                    $(".timeago").timeago();
                });
            }

            estadisticas = _contenedor.find(".seccion-estadisticas");
            if (estadisticas.length) {
                $.ajax({
                    url: "/Backend/Escritorio/Estadisticas",
                    dataType: "json"
                })
                .success(function (data) {
                    k.bind(estadisticas, k.observable(data));
                });
            }

            // Evitar navegación al hacer click en los links de la galería.
            $("#galeria").on("click", "a", function (e) { e.preventDefault(); });
        }

        function resize() {
            for (var mi in modulosInternos) {
                modulosInternos[mi].resize();
            }
        }

        function destroy() {
            for (var mi in modulosInternos) {
                modulosInternos[mi].destroy();
            }
        }

        return {
            url: url,
            init: init,
            resize: resize,
            destroy: destroy
        }
    })($("#target"), "/Backend/Escritorio");

})(jQuery, kendo, app);