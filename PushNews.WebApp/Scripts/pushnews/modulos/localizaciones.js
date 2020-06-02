(function ($, k, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de localizaciones: la aplicación es null.");
        return;
    }
    app.modulos["localizaciones"] = (function (contenedor, url, prefijoEventos, opciones) {
        var _contenedor = contenedor, _url = url, _prefijoEventos = prefijoEventos,
            opDefault = {};
        var _opciones = $.extend({}, opDefault, opciones);
        var modulosInternos = {};
        var textos = {};
        var contenedorCoordenadas, coordenadas;
        
        function init() {
            modulosInternos.moduloGrilla = ModuloGrilla(_contenedor, _url, "LocalizacionID", _prefijoEventos,
                {
                    edicionInline: false,
                    textoConfirmacion: function (reg) {
                        return reg.Descripcion;
                    }
                });

            modulosInternos.moduloGrilla.init();

            contenedorCoordenadas = _contenedor.find("#contenedorCoordenadas");
            contenedorCoordenadas.on("error_geocoding", function (ev, mensaje) {
                app.notificarError(textos.TituloErrorGeocoding, mensaje);
            });
            contenedorCoordenadas.on("alerta_geocoding", function (ev, mensaje) {
                app.notificarAdvertencia(textos.TituloErrorGeocoding, mensaje);
            });
            _contenedor.on(_prefijoEventos + "_edit", function () {
                var registro = modulosInternos.moduloGrilla.registroEnEdicion();
                coordenadas.init();
                coordenadas.modelo.set("coordenadas.longitud", registro.get("Longitud"));
                coordenadas.modelo.set("coordenadas.latitud", registro.get("Latitud"));
                coordenadas.centrarMapa();
            });
            _contenedor.on(_prefijoEventos + "_fin_edicion", function () {
                coordenadas.modelo.set("coordenadas.longitud", 0);
                coordenadas.modelo.set("coordenadas.latitud", 0);
                coordenadas.modelo.set("direccion.direccion", '');
                coordenadas.modelo.set("direccion.localidad", '');
                coordenadas.modelo.set("direccion.provincia", '');
                coordenadas.modelo.set("direccion.pais", '');
                coordenadas.modelo.set("direccion.codigoPostal", '');
                coordenadas.modelo.set("direccion.direccionCompelta", '');
                coordenadas.centrarMapa();
            });

            coordenadas = new Coordenadas(contenedorCoordenadas);
            coordenadas.modelo.bind("change", actualizarDatos);
        }

        function actualizarDatos(ev) {
            var reg = modulosInternos.moduloGrilla.registroEnEdicion();
            if (reg) {
                // Los eventos con "coordenadas." son los que se producen al establecer los valores desde 
                // código o desde los numeric-text-boxes.
                if (ev.field === "coordenadas.latitud") {
                    var latitud = coordenadas.modelo.get("coordenadas.latitud");
                    reg.set("Latitud", redondearCoordenada(latitud));
                } else if (ev.field === "coordenadas.longitud") {
                    var longitud = coordenadas.modelo.get("coordenadas.longitud");
                    reg.set("Longitud", redondearCoordenada(longitud));
                } else if (ev.field === "coordenadas") {
                    // Este evento es el que se produce al mover el marcador sobre el mapa.
                    var longitud = coordenadas.modelo.get("coordenadas.longitud");
                    var latitud = coordenadas.modelo.get("coordenadas.latitud");
                    reg.set("Longitud", redondearCoordenada(longitud));
                    reg.set("Latitud", redondearCoordenada(latitud));
                }/*else if (ev.field === "coordenadas.direccionCompleta") {
                    reg.set("Direccion", direccion);
                }*/

            }
        }

        function redondearCoordenada(coordenada) {
            return Math.round(coordenada * 1000000) / 1000000;
        }

        function resize() {
        }

        function destroy() {
            coordenadas.modelo.unbind("change", actualizarDatos);
            _contenedor.off();
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

    })($("#target"), "/Backend/Localizaciones", "localizaciones", {});

})(jQuery, kendo, window.app);