function ModuloGrillaArbol(contenedor, url, campoID, prefijoEventos, opcionesjs) {
    var _campoID = campoID, _url = url, _prefijoEventos = prefijoEventos, borrando = false,
        _contenedor = contenedor;
    var dialogoConfirmacion, camposBuscar, txtBuscar, grid;
    var contenedorGrilla, capaEditor, capaEditorMultiple;
    var opDefault = {
        vistaDetalle: false,
        edicionInline: true,
        buscar: true,
        plantillaMensajeValidacion: "#fieldInvalidTemplate",
        textoConfirmacion: function () { return ""; },
        filtroExterno: null,
        editar: true,
        eliminar: true,
        agregar: true,
        confirmarEliminacion: true,
        registroEdicionMultiple: { }
    };
    var registrosEdicionMultiple = [];

    var validationOptions;
    var _opciones = $.extend({}, opDefault, opcionesjs), _textos;
    var edicionMultipleViewModel = kendo.observable(_opciones.registroEdicionMultiple);

    function init(id) {
        // Obtener configuración en atributos unobtrusive del contenedor del módulo.
        var contenedorModulo = _contenedor.find(".contenedor-modulo");
        var op = contenedorModulo.data("opciones");

        // Las opciones se combinan con las recibidas en el constructor del módulo y con las
        // definidas por defecto.
        $.extend(_opciones, op);

        // Obtener los textos internacionalizados en atributos unobtrusive del contenedor del módulo.
        _textos = contenedorModulo.data("textos");

        // Componentes
        plantillaMensajeValidacion = kendo.template($(_opciones.plantillaMensajeValidacion).html());
        grid = contenedorModulo.find(".k-treelist").data("kendoTreeList");
        dialogoConfirmacion = _contenedor.find("#DialogoConfirmacion");
        txtBuscar = contenedorModulo.find(".txtBuscar");
        camposBuscar = txtBuscar.data("camposBuscar");
        contenedorGrilla = _contenedor.find(".contenedor-modulo");
        if (!_opciones.edicionInline) {
            capaEditor = _contenedor.find(".formulario-edicion").hide();
            capaEditorMultiple = _contenedor.find(".formulario-edicion-multiple").hide();
        }
        else {
            grid.bind("edit", function (e) {
                e.sender.editable.validatable._errorTemplate = plantillaMensajeValidacion;
            })
        }

        validationOptions = {
            errorTemplate: plantillaMensajeValidacion
        };
        
        // Eventos
        grid.dataSource.bind("error", prevenirCierreDialogo);
        grid.bind("dataBound", prepararControlesGrilla);
        grid.bind("change", clickFila);

        // Eventos de selección de filas.
        grid.tbody.on("change", ".cbActivarDesactivar", activarDesactivar);
        if (_opciones.eliminar) {
            grid.tbody.on("click", ".btEliminar", btEliminarClick);
        }
        grid.tbody.on("click", ".check-row", seleccionFila);
        grid.tbody.on("click", "tr", clickFila);
        grid.thead.on("click", ".btSeleccionarTodo", seleccionarTodo);

        // Eventos para lanzar la edición o la vista detalle en función de la opción vistaDetalle
        // al hacer doble click (o doble tap) sobre una fila.
        grid.tbody.on("doubletap", "tr input, tr a, tr .has-switch, .check-row", function (ev) {
            ev.stopPropagation();
        });
        if (_opciones.vistaDetalle) {
            grid.tbody.on("doubletap", "tr", iniciarDetalles);
        } else if(_opciones.editar) {
            grid.tbody.on("doubletap", "tr:not(.k-grid-edit-row)", clickEditarFila);
        }

        // Edición de registros al hacer click en el botón
        if (_opciones.editar) {
            grid.tbody.on("click", ".btEditar", clickEditarFila);
        }

        if (_opciones.edicionMultiple) {
            grid.element.on("click", ".btEdicionMultiple", clickEdicionMultiple);
            kendo.bind(capaEditorMultiple, edicionMultipleViewModel);
        }

        // Eventos para iniciar la creación de registros al hacer click en el botón
        if (_opciones.agregar) {
            _contenedor.on("click", ".btAgregar", btAgregarClick);
        }

        // Eventos para finalizar y cancelar la edición
        if (_opciones.agregar || _opciones.editar) {
            if (_opciones.edicionInline) {
                _contenedor.on("click", ".btGuardar", btGuardarClick);
                _contenedor.on("click", ".btCancelar", btCancelarClick);
            } else {
                capaEditor.on("click", ".btGuardar", btGuardarClick);
                capaEditor.on("click", ".btCancelar", btCancelarClick);

                if (capaEditorMultiple) {
                    capaEditorMultiple.on("click", ".btGuardar", btGuardarMultipleClick);
                    capaEditorMultiple.on("click", ".btCancelar", btCancelarMultipleClick);
                }
            }
        }

        // Eventos relacionados con la búsqueda
        _contenedor.on("keypress", ".txtBuscar", kpTxtBuscar);
        _contenedor.on("click", ".btBuscar", buscar);
        _contenedor.on("click", ".btLimpiarBusqueda", limpiarBusqueda);
        _contenedor.on("click", ".btExportar", function (ev) {
            ev.preventDefault();
            grid.saveAsExcel();
        });
        _contenedor.on("click", ".mostrarExceso", function (ev) {
            app.mostrarOcultarEtiquetasHermanas($(ev.target));
        });
        _contenedor.on("click", "#recargar", function (ev) {
            ev.preventDefault();
            grid.dataSource.read();
        });
        if (!_opciones.edicionInline) {
            capaEditor.on("keypress", ".campo-form:not(span.k-numerictextbox, span.k-multiselect)", function (ev) {
                if (ev.which === 13) {
                    ev.preventDefault();
                    siguienteElemento($(ev.target));
                }
            });
        }

        _contenedor.trigger(_prefijoEventos + "_init");
    }

    function siguienteElemento(inputActivo) {
        var inputs = capaEditor.find(".campo-form:visible");
        var indiceActual = inputs.index(inputActivo);
        var siguiente;

        if (indiceActual === inputs.length - 1) {
            // Foco al botón
            capaEditor.find(".btGuardar").focus();
        } else {
            // Foco al siguiente elemento del formulario
            siguiente = $(inputs[indiceActual + 1]);
            if (siguiente.hasClass("k-numerictextbox") || siguiente.hasClass("k-combobox") || siguiente.hasClass("k-multiselect")) {
                siguiente = siguiente.find("input:visible");
            }
            siguiente.focus();
        }
    }

    function prevenirCierreDialogo(e) {
        grid.unbind("dataBound", editarTrasRecargar);
        grid.unbind("dataBound", terminarEdicion);
        grid.unbind("dataBound", terminarEdicionMultiple);

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

    function clickFila(/*ev*/) {
        $(grid.tbody).find("tr").each(function () {
            var check = $(this).find(".check-row");
            if (check.length) {
                if ($(this).hasClass("k-state-selected")) {
                    check.prop("checked", true);
                } else {
                    check.prop("checked", false);
                }
            }
        });
    }

    function seleccionFila(ev) {
        ev.stopPropagation();
        var row = $(this).parents("tr");
        if ($(this).is(":checked")) {
            row.addClass("k-state-selected");
        } else {
            row.removeClass("k-state-selected");
        }
    }

    function seleccionarTodo(ev) {
        ev.stopPropagation();
        var checks = $(grid.tbody).find("tr .check-row");
        var rows = $(grid.tbody).find("tr");

        // Sin hay algún check sin marcar, marcamos todos. Si todos están marcados, desmarcamos todos.
        if (checks.not(":checked").length > 0) {
            checks.prop("checked", true);
            rows.addClass("k-state-selected");
        } else {
            checks.prop("checked", false);
            rows.removeClass("k-state-selected");
        }
    }

    // Inicializa componentes dentro de la grilla: los que aparecen repetidos por cada fila, que
    // se recargan con cada databound.
    function prepararControlesGrilla() {
        // Inicializar los switches para activar/desactivar registros.
        var switches = grid.element.find('.make-switch');
        if (switches.length) {
            switches.bootstrapSwitch();
        }
    }

    // Evento de pulsar una tecla cuando el foco está en la caja de búsqueda.
    function kpTxtBuscar(ev) {

        // Si se pulsa intro, lanzar la búsqueda.
        if (ev.which === 13) {
            buscar();
        }
    }

    // Maneja el evento de click en el botón de limpiar búsqueda.
    function limpiarBusqueda(ev) {
        ev.preventDefault();
        resetBusqueda();
    }

    function resetBusqueda() {
        txtBuscar.val("");
        buscar();
    }

    // Buscar registros por texto en el servidor.
    function buscar() {
        var valorFiltro = txtBuscar.val().trim();
        var ds = grid.dataSource;
        var nuevoFiltro = {
            logic: "or",
            filters: []
        };
        $.each(camposBuscar, function (i, item) {
            nuevoFiltro.filters.push({ field: item, operator: "contains", value: valorFiltro });
        });
        ds.filter(nuevoFiltro);

        if (valorFiltro === '') {
            $(".btLimpiarBusqueda").addClass("hidden");
        } else {
            $(".btLimpiarBusqueda").removeClass("hidden");
        }

    }

    // Activar desactivar un registro. Se ejecuta al hacer click en el switch de una fila.
    function activarDesactivar(ev) {
        ev.preventDefault();
        ev.stopPropagation();
        var motivoBajaSeleccionado = grid.dataItem($(ev.target).parents("tr"));
        motivoBajaSeleccionado.set('Activo', !motivoBajaSeleccionado.Activo);
        grid.dataSource.sync();
    }
    
    function btAgregarClick(ev) {
        ev.preventDefault()
        ev.stopPropagation();
        iniciarAgregar();
    }

    function iniciarAgregar() {
        modoEdicion();
        if (_opciones.edicionInline) {
            agregarInline();
        } else {
            agregar();
        }
    }

    function clickEditarFila(ev) {
        var seleccion;
        ev.preventDefault();
        ev.stopPropagation();
        if($(ev.target).is("tr")) {
            seleccion = $(ev.target);
        } else {
            seleccion = $(ev.target).parents("tr:not(.k-grid-edit-row)");
        }
        var reg = grid.dataItem(seleccion);
        iniciarEdicion(reg);
    }

    var idEditarAux;
    function iniciarEdicion(reg) {
        idEditarAux = reg[_campoID];
        grid.one("dataBound", editarTrasRecargar, [reg]);
        grid.dataSource.read();
    }

    function editarTrasRecargar() {
        var id = idEditarAux;
        idEditarAux = null;
        datoEditar = null;
        var pageData = grid.dataSource.view();
        for (var i = 0; i < pageData.length; i++) {
            if (id === pageData[i][_campoID]) {
                datoEditar = pageData[i];
                break;
            }
        }

        if (datoEditar) {
            modoEdicion();
            if (_opciones.edicionInline) {
                editarInline(datoEditar);
            } else {
                editar(datoEditar);
            }
        }
    }

    function clickEdicionMultiple(ev) {
        ev.preventDefault();
        var seleccion = grid.select();
        if (seleccion.length === 1) {
            iniciarEdicion(grid.dataItem(seleccion));
        } else if (seleccion.length > 1) {
            modoEdicion();
            registrosEdicionMultiple = seleccion;
            edicionMultiple();
        }
    }

    function btGuardarMultipleClick(ev) {
        ev.preventDefault();
        guardarMultiple();
        window.scrollTo(0, 0);
    }

    // Cancelar edición o creación de registros.
    function btCancelarMultipleClick(ev) {
        ev.preventDefault();
        finModoEdicion();
        cancelarMultiple();
        window.scrollTo(0, 0);
    }

    function iniciarDetalles(ev) {
        ev.preventDefault();
        ev.stopPropagation();
        var fila = $(ev.target).parents("tr:not(.k-grid-edit-row)");
        var reg = grid.dataItem(fila);
        document.location = document.location + "detalles/" + reg[_campoID];
    }

    function btGuardarClick(ev) {
        ev.preventDefault();
        ev.stopPropagation();
        var validador;
        if (_opciones.edicionInline) {
            var filaEdicion = $(ev.target).parents(".k-grid-edit-row");
            validador = filaEdicion.kendoValidator(validationOptions).data("kendoValidator");
            guardarInline(validador);
        } else {
            var formEdicion = capaEditor.find("form");
            validador = formEdicion.kendoValidator(validationOptions).data("kendoValidator");
            guardar(validador);
            window.scrollTo(0, 0);
        }
    }

    // Cancelar edición o creación de registros.
    function btCancelarClick(ev) {
        ev.preventDefault();
        ev.stopPropagation();
        finModoEdicion();
        if (_opciones.edicionInline) {
            cancelarEdicionInline();
        } else {
            cancelarEdicion();
        }
        window.scrollTo(0, 0);
    }

    // Click en el botón de eliminar en una fila.
    function btEliminarClick(ev) {
        ev.preventDefault();
        ev.stopPropagation();
        var seleccion = $(ev.target).parents("tr");

        // Mostrar diálogo de confirmación si está activado en las opciones y se encuentra en el DOM.
        if (_opciones.confirmarEliminacion && dialogoConfirmacion.length) {
            dialogoConfirmacion.find(".cuerpo")
                .html(_textos.confirmacionEliminarCuerpo
                        + " <strong>" + _opciones.textoConfirmacion(grid.dataItem(seleccion)) + "</strong>");

            // Si se pulsa sí, eliminar la fila y cerrar la ventana.
            dialogoConfirmacion
                .off("click", ".yes-button")
                .on("click", ".yes-button", function (e) {
                    eliminar(seleccion);
                    dialogoConfirmacion.modal("hide");
                });

            // Si se pulsa no, cerrar la ventana.
            dialogoConfirmacion
                .off("click", ".no-Button")
                .on("click", ".no-button", function (e) {
                    dialogoConfirmacion.modal("hide");
                });

            // Mostrar confirmacion
            dialogoConfirmacion.modal("show");
        } else {
            // Si no se ha configurado la confirmación o no se ha encontrado el diálogo en el DOM, eliminar sin preguntar.
            eliminar(seleccion);
        }
    }

    function eliminar(fila) {
        borrando = true;
        grid.removeRow(fila);
        // No hace falta el sync en las grillas que tienen .AutoBind(true)
        // Por esta línea se producían intentos de eliminación dobles.
        //grid.dataSource.sync();
    }

    function destroy() {
        _contenedor.off("click");
        _contenedor.off("keypress");
        _contenedor.trigger(_prefijoEventos + "_destroy");
    }

    /******************************* MÉTODOS PARA EDICIÓN EXTERNA ********************************/

    function agregar() {
        capaEditor.find(".no-edicion").show();
        capaEditor.find(".no-creacion").hide();
        var nuevo = grid.dataSource.insert();
        prepararFormEdicion(nuevo);
    }

    function editar(registro) {
        capaEditor.find(".no-edicion").hide();
        capaEditor.find(".no-creacion").show();
        prepararFormEdicion(registro);
    }

    function edicionMultiple() {
        var form = capaEditorMultiple.find("form");
        limpiarValidacion(form);
        borrarDatosViewModelEdicionMultiple();
        contenedorGrilla.hide();
        capaEditorMultiple.show();
    }

    function prepararFormEdicion(registro) {
        limpiarValidacion(capaEditor.find("form"));
        kendo.bind(capaEditor, registro);
        capaEditor.show();
        contenedorGrilla.hide();
    }

    function limpiarValidacion(form) {
        var validator = capaEditor.find("form").kendoValidator(validationOptions).data("kendoValidator");
        validator.hideMessages();
        form.find(".k-invalid").removeClass("k-invalid");
    }

    function guardar(validador) {
        if (validador.validate()) {
            grid.one("dataBound", terminarEdicion);
            grid.dataSource.sync();

        }
    }

    function terminarEdicion() {
        finModoEdicion();
        grid.dataSource.read();
        contenedorGrilla.show();
        capaEditor.hide();
    }

    function guardarMultiple() {
        var validador;
        var formEdicion = capaEditorMultiple.find("form");
        validador = formEdicion.kendoValidator(validationOptions).data("kendoValidator");
        if (validador.validate()) {
            var registros = registrosEdicionMultiple.map(function (i) {
                return grid.dataItem(registrosEdicionMultiple[i]);
            });
            
            var regMuestra = registros[0];
            for (var propiedad in regMuestra) {
                var campoForm = formEdicion.find("#Multiple_" + propiedad);
                if (campoForm.length) {
                    var valor = campoForm.val();
                    if (valor !== null && valor.length) {
                        for (var r in registros) {
                            if (r === "0" || !!parseInt(r)) {
                                registros[r].set(propiedad, campoForm.val());
                            }
                        }
                    }
                }
            }

            grid.one("dataBound", terminarEdicionMultiple);
            grid.dataSource.sync();
        }
    }

    function terminarEdicionMultiple() {
        finModoEdicion();
        borrarDatosViewModelEdicionMultiple();
        grid.dataSource.read();
        contenedorGrilla.show();
        capaEditorMultiple.hide();                                
    }

    function cancelarEdicion() {
        if (_opciones.edicionInline) {
            cancelarEdicionInline();
        } else {
            cancelarEdicionExterna();
        }
    }

    function cancelarEdicionExterna() {
        capaEditorMultiple.hide();
        capaEditorMultiple.find("form").off();
        capaEditor.hide();
        grid.dataSource.cancelChanges();
        contenedorGrilla.show();
    }

    function cancelarMultiple() {
        capaEditorMultiple.find("form").off();
        capaEditorMultiple.hide();
        borrarDatosViewModelEdicionMultiple();
        grid.dataSource.cancelChanges();
        contenedorGrilla.show();
    }

    /*********************************************************************************************/


    function borrarDatosViewModelEdicionMultiple() {
        for (var p in edicionMultipleViewModel) {
            if (p[0] !== "_" && edicionMultipleViewModel.hasOwnProperty(p) && typeof (edicionMultipleViewModel.p) !== "function") {
                edicionMultipleViewModel.set(p, null);
            }
        }
    }

    /******************************** MÉTODOS PARA EDICIÓN INLINE ********************************/

    function agregarInline() {
        grid.dataSource.add();
        grid.editRow(grid.table.find("tbody tr:first"));
    }

    function editarInline(registro) {
        var fila = grid.tbody.find(">tr[data-uid='" + registro.uid + "']");
        //grid.select(fila);
        grid.editRow(fila);
    }

    function guardarInline(validador) {
        if (validador.validate()) {
            grid.dataSource.sync();
            finModoEdicion();
        }
    }

    function cancelarEdicionInline() {
        grid.cancelChanges();
    }

    /*********************************************************************************************/

    function modoEdicion() {
        app.prevenirNavegacion(_textos.confirmacionNavegacion); // TODO Tomar este mensaje de algún data-attribute.
        _contenedor.find(".deshabilitar-en-edicion:not('input')").addClass("disabled");
        _contenedor.find("input.deshabilitar-en-edicion").attr("readonly", "readonly");
    }

    function finModoEdicion() {
        _contenedor.find(".deshabilitar-en-edicion").not('input').removeClass("disabled");
        _contenedor.find("input.deshabilitar-en-edicion").removeAttr("readonly");
        app.prevenirNavegacion(false);
    }

    return {
        init: init,
        url: _url,
        contenedor: _contenedor,
        grid: function () { return grid; },
        filtroExterno: _opciones.filtroExterno,
        resetBusqueda: resetBusqueda,
        resize: resize,
        destroy: destroy,
        editar: iniciarEdicion,
        nuevo: iniciarAgregar,
        cancelarEdicion: cancelarEdicion
    };
}