function ModuloDetalles(contenedor, url, campoID, prefijoEventos, opcionesjs) {
    var _url = url, _prefijoEventos = prefijoEventos, borrando = false,
        _contenedor = contenedor, cabecera;
    var dialogoConfirmacion;
    var _registroID,registro, _campoID = campoID;
    var opDefault = {
        urlModuloGrid: "",
        prototipoModelo: {}
    };

    var _opciones = $.extend({}, opDefault, opcionesjs), _textos;

    var cabeceraModel = kendo.observable({
        seccion: "",
        modulo: "",
        titulo: "",
        subtitulo: ""
    });

    function init(id) {

        // Obtener configuración en atributos unobtrusive del contenedor del módulo.
        var contenedorModulo = _contenedor.find(".contenedor-ficha").first();
        _registroID = _contenedor.data("id");

        // Las opciones se combinan con las recibidas en el constructor del módulo y con las
        // definidas por defecto.
        var op = contenedorModulo.data("opciones");
        _opciones = $.extend({}, _opciones, op);

        // Obtener los textos internacionalizados en atributos unobtrusive del contenedor del módulo.
        _textos = _contenedor.data("textos");
        if (_textos.hasOwnProperty("seccion")) {
            cabeceraModel.set("seccion", _textos.seccion);
        }
        if (_textos.hasOwnProperty("modulo")) {
            cabeceraModel.set("modulo", _textos.modulo);
        }
        if (_textos.hasOwnProperty("titulo")) {
            cabeceraModel.set("titulo", _textos.titulo);
        }
        if (_textos.hasOwnProperty("subtitulo")) {
            cabeceraModel.set("subtitulo", _textos.subtitulo);
        }

        // Componentes
        dialogoConfirmacion = _contenedor.find("#DialogoConfirmacion");
        
        // Eventos
        //_contenedor.on("click", ".btVolver", btVolverClick);
        _contenedor.on("click", "#recargar", function (ev) {
            ev.preventDefault();
            leerDato();
        });

        registro = kendo.observable(_opciones.prototipoModelo);
        kendo.bind(contenedorModulo, registro);
        leerDato();
        _contenedor.trigger(_prefijoEventos + "_init");
    }

    //function btVolverClick(ev) {
    //    ev.preventDefault();
    //    document.location = _opciones.urlModuloGrid;
    //}

    function leerDato() {
        var data = {};
        data[campoID] = _registroID;
        return $.get(url + "/LeerUno", data, function (data) {
            if (data) {
                for(var propiedad in data) {
                    if (!registro.hasOwnProperty(propiedad)) {
                        registro[propiedad] = "";
                    }
                    registro.set(propiedad, data[propiedad]);
                }
            }

            if (_opciones.campoSeccion && data.hasOwnProperty(_opciones.campoSeccion)) {
                cabeceraModel.set("seccion", data[_opciones.campoSeccion]);
            }
            if (_opciones.campoModulo && data.hasOwnProperty(_opciones.campoModulo)) {
                cabeceraModel.set("modulo", data[_opciones.campoModulo]);
            }
            if (_opciones.campoTitulo && data.hasOwnProperty(_opciones.campoTitulo)) {
                cabeceraModel.set("titulo", data[_opciones.campoTitulo]);
            }
            if (_opciones.campoSubtitulo && data.hasOwnProperty(_opciones.campoSubtitulo)) {
                cabeceraModel.set("subtitulo", data[_opciones.campoSubtitulo]);
            }
        });
    }

    function prevenirCierreDialogo(e) {
        if (e.errors) {
            grid.one("dataBinding", function (ev) {
                ev.preventDefault();
                $.each(e.errors, function (key, value) {
                    if (Object.prototype.toString.call(value.errors) === '[object Array]') {
                        for (var errors in value) {
                            _contenedor.trigger("notificacion", ["", value[errors].join("\n"), "error"]);
                        }
                    } else {
                        _contenedor.trigger("notificacion", ["", value, "error"]);
                    }
                });
                if (borrando) {
                    borrando = false;
                    grid.cancelChanges();
                }
            });
        }
    }

    function resize() {
    }

    function destroy() {
        $(contenedor).off("click");
        _contenedor.trigger(_prefijoEventos + "_destroy");
    }
    

    return {
        init: init,
        url: function () { return _url; },
        contenedor: _contenedor,
        resize: resize,
        destroy: destroy,
        getId: _registroID,
        registro: registro,
        getRegistro: function() { return registro; },
        cabecera: cabeceraModel,
        leerDato: leerDato
    };
}