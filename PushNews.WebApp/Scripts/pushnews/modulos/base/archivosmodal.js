function ArchivosModal(contenedor, campoID, opcionesjs) {
    var _modal = contenedor;
    var _opDefault = {
        parametrosAdicionalesUpload: {},
        prefijoNombreFoto: "",
        urlGuardarFotos: "",
        urlGuardarFoto: ""
    };
    var _opciones = $.extend({}, _opDefault, opcionesjs), textos = { errorWebcam: '' };
    var identificador;

    var tabPanel, panelDocumentos, panelSubir, panelCapturar;
    var uploader, lvCapturas, auxCanvas, capturaTemplate, btSeleccionar;
    var modulosInternos = {};

    function init() {
        tabPanel = _modal.find(".tabbable");
        identificador = _modal.attr("id");

        $.extend(textos, _modal.data("textos"));
        
        _modal.on("hidden.bs.modal", function () {
            eliminarCapturas();
            resetUpload();

            // Si se ha especificado función onClose en las opciones, invocar.
            if (_opciones.onClose && typeof (_opciones.onClose) === "function") {
                _opciones.onClose();
            }
        });

        // Inicializar apartado de selección de documentos.
        panelDocumentos = _modal.find("#" + identificador + "_panel_seleccionar");
        if (panelDocumentos.length) {
            _opciones.opDocumentos = panelDocumentos.data("configuracion");
            modulosInternos.listaDocumentos = ModuloGrilla(panelDocumentos, "", "DocumentoID",
                "documentos", { eliminar: false, editar: false, agregar: false, edicionInline: false });
            modulosInternos.listaDocumentos.init();
            btSeleccionar = _modal.find(".btSeleccionar");
            if (btSeleccionar.length) {
                btSeleccionar.on("click", clickSeleccionar);
            }
            
            if (_opciones.opDocumentos && _opciones.opDocumentos.FiltroCategoriasArchivos && _opciones.opDocumentos.FiltroCategoriasArchivos.length) {
                var nuevoFiltro = {
                    logic: "or",
                    filters: []
                };
                var ds = modulosInternos.listaDocumentos.grid().dataSource;
                $.each(_opciones.opDocumentos.FiltroCategoriasArchivos, function (i, item) {
                    nuevoFiltro.filters.push({ field: "DocumentoTipoID", operator: "eq", value: item });
                });
                ds.filter(nuevoFiltro);
            }
        }

        panelSubir = _modal.find("#" + identificador + "_panel_subir");
        if (panelSubir.length) {
            _opciones.opSubir = panelSubir.data("configuracion");
            uploader = panelSubir
                .find("input[type='file']")
                .data("kendoUpload");
            uploader.bind("upload", onUpload);
            uploader.bind("success", onSuccess);
            panelSubir.on("click", ".limpiarCompletados", limpiarCompletados);
        }

        // Inicializar apartado de capturas con la webcam
        panelCapturar = _modal.find("#" + identificador + "_panel_capturar")
        if (panelCapturar) {
            _opciones.opCapturar = panelCapturar.data("configuracion");
            tabPanel.on("show.bs.tab", "#tab_" + identificador + "_panel_capturar", iniciarVideo);
            tabPanel.on("hidden.bs.tab", "#tab_" + identificador + "_panel_capturar", pararVideo);
            contenedorWebcam = panelCapturar.find(".WebcamCapture");
            _modal.on("hidden.bs.modal", pararVideo);
            lvCapturas = contenedorWebcam.find("#capturas");
            auxCanvas = contenedorWebcam.find("#auxCanvas");
            capturaTemplate = kendo.template(contenedorWebcam.find("#CapturaTemplate").html());
            modulosInternos.capturar = Capturar(contenedorWebcam, {
                beforeGetUserMedia: app.bloquearPantalla,
                afterGetUserMedia: camaraOK,
                errorGetUserMedia: camaraError
            });
            modulosInternos.capturar.init();
            contenedorWebcam.on("click", "#snap", capturar);
            lvCapturas.on("click", ".captura-eliminar", clickEliminarCaptura);
            lvCapturas.on("click", ".captura-guardar", guardarUno);
            panelCapturar.on("click", "#btGuardarTodo", guardarTodo);
        }
    }

    function camaraOK() {
        app.desbloquearPantalla();
        panelCapturar.find(".errorCamara").addClass("hidden");
        panelCapturar.find(".WebcamCapture").removeClass("hidden");
    }

    function camaraError() {
        app.desbloquearPantalla();

        panelCapturar.find(".WebcamCapture").addClass("hidden");
        panelCapturar.find(".errorCamara").removeClass("hidden");
    }

    function clickSeleccionar(ev) {
        ev.preventDefault();
        var pestanaSeleccionar = _modal.find("#tab_" + identificador + "_panel_seleccionar").parent();
        if (pestanaSeleccionar.hasClass("active")) {
            // TODO comprobar que hay registros seleccionados y finalizar.
            var seleccion = seleccionados();
            if (seleccion) {
                _opciones.seleccionar(seleccion);
            }
        } else {
            _modal.find("#tab_" + identificador + "_panel_seleccionar").tab("show");
        }
    }

    function abrirSeleccion() {
        modulosInternos.listaDocumentos.grid().dataSource.read();
        _modal.modal("show");
        _modal.find("#tab_"+ identificador + "_panel_seleccionar").tab("show");
    }

    function abrirSubida() {
        _modal.modal("show");
        _modal.find("#tab_" + identificador + "_panel_subir").tab("show");
    }

    // Añadir parámetro adicional en la solicitud de subida de archivos.
    function onUpload(e) {
        e.data = _opciones.opSubir.ParametrosAdicionalesUpload;
    };

    // Al terminar con éxito la subida de archivos, actualiza la grilla.
    function onSuccess(/*ev*/) {
        if (modulosInternos.listaDocumentos) {
            var documentosGrid = modulosInternos.listaDocumentos.grid();
            if (documentosGrid) {
                documentosGrid.dataSource.read();
            }
        }
    }

    function limpiarCompletados() {
        panelSubir.find("li.k-file-success").detach();
    }

    // Quita el historial de ficheros del widget de subida.
    function resetUpload() {
        panelSubir.find(".k-upload-files").remove();
        panelSubir.find(".k-upload-status").remove();
        panelSubir.find(".k-upload.k-header").addClass("k-upload-empty");
        panelSubir.find(".k-upload-button").removeClass("k-state-focused");
    }

    function iniciarVideo() {
        modulosInternos.capturar.start();
    }

    function pararVideo() {
        modulosInternos.capturar.stop();
    }

    function abrirCamara() {
        _modal.modal("show");
        _modal.find("a[href=#" + identificador + "_panel_capturar]").tab("show");
        modulosInternos.capturar.start();
    }

    function eliminarCapturas() {
        lvCapturas.find(".captura-item").remove();
    }

    // Guardar todas las fotos de la webcam
    function guardarTodo(/*ev*/) {
        var listaCanvas = lvCapturas.find(".captura-thumb");
        var data = [];
        for (var i = 0; i < listaCanvas.length; i++) {
            var canvas = $(listaCanvas[i]);
            var file = [
                nombreFoto(_opciones.opCapturar.PrefijoNombreFoto, canvas.data("fecha")),
                canvas.data("imgData")
            ];
            data.push(file);
        }
        guardarListaCanvas(data);
        modalCamara.modal("hide");
    }

    // Guardar una foto de la webcam
    function guardarUno(ev) {
        if (ev) {
            ev.preventDefault();
        }
        var itemCaptura = $(this).parents(".captura-item");
        canvas = itemCaptura.children("canvas");
        var imgData = canvas.data("imgData");
        var fecha = canvas.data("fecha");
        var nombreFichero = nombreFoto(_opciones.opCapturar.PrefijoNombreFoto, fecha);
        guardarCanvas(imgData, nombreFichero, function (data) {
            eliminarCaptura(itemCaptura);
        });
    }

    function nombreFoto(paciente, fecha) {
        return paciente + "_"
            + lpad(fecha.getFullYear(), 4, "0")
            + lpad(1 + fecha.getMonth(), 2, "0")
            + lpad(fecha.getDate(), 2, "0")
            + lpad(fecha.getHours(), 2, "0")
            + lpad(fecha.getMinutes(), 2, "0")
            + lpad(fecha.getSeconds(), 2, "0")
            + ".png";
    }

    function lpad(num, length, char) {
        var s = num.toString();
        while (s.length < length) {
            s = char + s;
        }
        return s;
    }

    //
    function guardarCanvas(datos, nombre) {

        var data1 = { file: datos, nombre: nombre };
        var data = $.extend({}, data1, _opciones.opCapturar.ParametrosAdicionalesUpload);

        $.ajax({
            url: _opciones.opCapturar.UrlSubirArchivo,
            data: data,
            type: 'POST'
        }).success(onSuccess);
    }

    function guardarListaCanvas(datos) {

        var data1 = { files: datos };
        var data = $.extend({}, data1, _opciones.opCapturar.ParametrosAdicionalesUpload);

        $.ajax({
            url: _opciones.opCapturar.UrlSubirArchivos,
            data: data,
            type: 'POST'
        }).success(onSuccess);
    }

    function clickEliminarCaptura(ev) {
        eliminarCaptura($(this).parents(".captura-item"));
    }

    // Eliminar una captura de la webcam
    function eliminarCaptura(itemCaptura) {
        itemCaptura.remove();
    }

    // Capturar imagen de webcam
    function capturar(/*ev*/) {
        var v = modulosInternos.capturar.capture();
        var ancho = $(v).width(), alto = $(v).height();
        auxCanvas.attr("width", ancho);
        auxCanvas.attr("height", alto);
        var auxContext = auxCanvas[0].getContext("2d");
        auxContext.drawImage(v, 0, 0, ancho, alto);
        var imgData = auxCanvas[0].toDataURL("image/png");
        imgData = imgData.replace('data:image/png;base64,', '');
        auxContext.clearRect(0, 0, auxCanvas.width, auxCanvas.height);

        var cap = capturaTemplate({});
        lvCapturas.prepend($(cap));
        var canvas = lvCapturas.find(".captura-item").first().find("canvas");
        canvas.data("imgData", imgData);
        var context = canvas[0].getContext("2d");
        canvas.data("fecha", new Date());
        context.drawImage(v, 0, 0, 640, 480, 0, 0, 213, 160);
    }

    function destroy() {
        modulosInternos.capturar.stop();
    }

    function seleccionados() {
        if (panelDocumentos.length) {
            var grid = modulosInternos.listaDocumentos.grid();
            var seleccionados = grid.select();
            if (seleccionados) {
                var datos = $.map(seleccionados, function (item) {
                    return grid.dataItem(item);
                });
                return datos;
            } else return [];
        }
    }

    return {
        init: init,
        destroy: destroy,
        abrirCamara: abrirCamara,
        abrirSubida: abrirSubida,
        abrirSeleccion: abrirSeleccion,
        seleccionados: seleccionados
    };
}