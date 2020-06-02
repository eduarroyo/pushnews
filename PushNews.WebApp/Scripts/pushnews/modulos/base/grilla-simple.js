function ModuloGrilla(contenedor, url, campoID, prefijoEventos, opcionesjs) {
    var _campoID = campoID, _url = url, _prefijoEventos = prefijoEventos, borrando = false,
        _contenedor = contenedor;
    var dialogoConfirmacion, dialogoConfirmacionActivarDesactivar, camposBuscar, txtBuscar, grid;
    var contenedorGrilla, capaEditor, capaEditorMultiple;
    var registroEdicion, modoEdicionActivado = false;
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
        mantenerFilaNuevoRegistro: false,
        ocultarGrillaAlEditar: true,
        confirmarEliminacion: true,
        confirmarActivarDesactivar: false,
        accionAlternativaActivarDesactivar: null,
        registroEdicionMultiple: { }
    };
    var registrosEdicionMultiple = [];

    var validationOptions;
    var _opciones = $.extend({}, opDefault, opcionesjs), _textos;
    var edicionMultipleViewModel = kendo.observable(_opciones.registroEdicionMultiple);

    function init(id) {
        // Obtener configuración en atributos unobtrusive del contenedor del módulo.
        var contenedorModulo = _contenedor.find(".contenedor-modulo");

        // No inicializar los componentes si no se ha cargado el módulo en el DOM.
        if (!contenedorModulo.length)
        {
            return;
        }

        // Recuperar las opciones del atributo data-opciones del marcado.
        var op = contenedorModulo.data("opciones");

        // Las opciones se combinan con las recibidas en el constructor del módulo y con las
        // definidas por defecto.
        $.extend(_opciones, op);

        // La opción "mantenerFilaNuevoRegistro" está condicionada a que las opciones de agregar
        // registros y la edición inline estén activadas.
        _opciones.mantenerFilaNuevoRegistro = _opciones.mantenerFilaNuevoRegistro
                                           && _opciones.edicionInline
                                           && _opciones.agregar;

        // Obtener los textos internacionalizados en atributos unobtrusive del contenedor del módulo.
        _textos = contenedorModulo.data("textos");

        // Componentes
        plantillaMensajeValidacion = kendo.template($(_opciones.plantillaMensajeValidacion).html());
        grid = contenedorModulo.find(".k-grid").data("kendoGrid");
        dialogoConfirmacion = _contenedor.find("#DialogoConfirmacion");
        dialogoConfirmacionActivarDesactivar = _contenedor.find("#ConfirmarActivarDesactivar");
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

                // En grillas con detalle, evitar que se despliegue la vista de detalle de una fila
                // nueva en edición (registro recién creado y sin guardar).
                if (e.model.isNew()) {
                    grid.tbody.find(".k-grid-edit-row").on("click", ".k-hierarchy-cell", function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                    });
                }
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
        grid.tbody.on("ifToggled", ".icheck", activarDesactivarClick);
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

        // Eventos para iniciar la creación de registros
        if(_opciones.agregar) {
            if (_opciones.mantenerFilaNuevoRegistro) {
                // Si la opción mantenerFilanuevoRegistro está activada, mantendremos siempre una
                // fila vacía al principio que servirá para agregar nuevos registros.
                grid.dataSource.bind("requestEnd", aniadirFilaNuevoRegistro);
            } else {
                // Si la opción mantenerFilanuevoRegistro NO está activada, asociamos el botón
                // btAgregar a la función de crear una fila para un registro nuevo.
                _contenedor.on("click", ".btAgregar", btAgregarClick);
            }
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

    // Mantiene activa la edición de un registro si falla al guardar los cambios.
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

    // Añadir la fila de nuevo registro en la página 1.
    function aniadirFilaNuevoRegistro(ev) {
        setTimeout(function () {
            agregarFilaNuevoRegistro();
        });
    }

    function agregarFilaNuevoRegistro() {
        if (grid.dataSource.page() === 1 && !modoEdicionActivado) {
            // Si hay cambios pendientes en el datasource, los cancelamos antes de crear la
            // fila para el nuevo registro.
            if (grid.dataSource.hasChanges()) {
                grid.dataSource.cancelChanges();
            }

            grid.dataSource.insert(0, {});
            var fila = grid.tbody.find("tr:nth(0)");
            grid.editRow($(fila, grid.tbody));
            fila.find(".btCancelar").remove();            
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
        //var switches = grid.element.find('.make-switch');
        //if (switches.length) {
        //    switches.bootstrapSwitch();
        //}

        // Inicializar los icheck.
        var ichecks = _contenedor.find(".icheck");
        ichecks.each(function (index, item) {
            $(item).iCheck({
                checkboxClass: 'icheckbox_flat-orange',
                radioClass: 'iradio_flat-orange'
            });
        });
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
    function activarDesactivarClick(ev) {
        var filaSeleccionada, registro;
        ev.preventDefault();

        // Al hacer click se actualiza el dato en el registro.
        filaSeleccionada = $(ev.target).parents("tr");

        // Si el módulo tiene activa la opción de confirmación de activación/desactivación, 
        // se abre el diálogo. Si no, se solicita al servidor guardar los cambios.
        if (_opciones.confirmarActivarDesactivar) {
            if (dialogoConfirmacionActivarDesactivar.length) {
                abrirDialogoConfirmacionActivarDesactivar(filaSeleccionada);
            }
        } else {
            registro = grid.dataItem(filaSeleccionada);
            activarDesactivarRegistro(registro);
        }
    }

    function activarDesactivarRegistro(registro) {
        if (_opciones.accionAlternativaActivarDesactivar && _opciones.accionAlternativaActivarDesactivar.length) {
            var datos = {};
            datos[_campoID] = registro[_campoID];
            $.post({
                url: _opciones.accionAlternativaActivarDesactivar,
                data: datos
            }).done(function (data) {
                if (data) {
                    if(data.Error) {
                        app.notificarError(data.Error);
                        grid.dataSource.cancelChanges();
                    } else {
                        grid.dataSource.read();
                    }
                }
            });
        } else {
            // Activar
            registro.set('Activo', !registro.Activo);
            grid.dataSource.sync();
        }
    }

    // Prepara los textos y los eventos del diálogo de confirmación para activar/desactivar registros.
    // Requiere que se hayan definido los textos confirmacionDesactivar y confirmacionActivar, así como que 
    // se haya incluido el diálogo de confirmación en la vista.
    function abrirDialogoConfirmacionActivarDesactivar(fila) {
        var registro = grid.dataItem(fila);
        var texto = registro.Activo ? _textos.confirmacionDesactivar : _textos.confirmacionActivar;
        texto += "<strong>&nbsp" + _opciones.textoConfirmacion(registro) + "</strong>";
        dialogoConfirmacionActivarDesactivar.find(".cuerpo").html(texto);

        // Enlazar los eventos de los botones. Si pulsa sí, guardar los cambios y cerrar.
        dialogoConfirmacionActivarDesactivar
            .on("click", ".yes-button", function (e) {
                activarDesactivarRegistro(registro);
                dialogoConfirmacionActivarDesactivar.modal("hide");
            });

        // Si pulsa no, cancelar los cambios y cerrar.
        dialogoConfirmacionActivarDesactivar
            .on("click", ".no-button", function (e) {
                grid.dataSource.cancelChanges();
                dialogoConfirmacionActivarDesactivar.modal("hide");
            });

        // Al cerrar el diálogo, los eventos de los botones se desactivan.
        dialogoConfirmacionActivarDesactivar.one("hide.bs.modal", function (e) {
            dialogoConfirmacionActivarDesactivar.off("click");
        });

        dialogoConfirmacionActivarDesactivar.modal("show");
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

    // Recargar el datasource y luego lanzar la edición.
    function iniciarEdicion(reg) {
        grid.one("dataBound", function () { editarTrasRecargar(reg[_campoID]); }, [reg]);
        grid.dataSource.read();
    }

    // Iniciar edición tras la recarga. En id tenemos el id de la fila que queremos editar.
    // hay que buscarla y pasar a modo de edición.
    function editarTrasRecargar(id) {
        var i, datoEditar = null;
        idEditarAux = null;        

        // Obtener los datos de la grilla de esta forma en lugar de la habitual sirve para
        // el caso de grillas con agrupamiento. En esos casos, hay que descender a través de
        // grid.dataSource.data en forma de árbol por los grupos para encontrar el registro
        // que vamos a editar. Con este sistema ahorramos lógica.
        // La idea es obtener las filas de la grilla que no son encabezados o piés de grupos.
        var filasDatos = grid.tbody.find("tr:not(.k-grouping-row):not(.k-group-footer)")

        // Para cada fila, obtener el dato y verificar si el id coincide. En el caso de que
        // coincida, esa es la fila a editar.
        for (i = 0; i < filasDatos.length && !datoEditar; i++) {
            datoFila = grid.dataItem(filasDatos[i]);
            if (id === datoFila.get(_campoID)) {
                datoEditar = datoFila;
            }
        }

        // Si se ha encontrado el registro a editar, pasar al modo de edición correspondiente.
        if (datoEditar) {
            editarRegistro(datoEditar);
        }
    }

    function editarRegistro(registro) {
        modoEdicion();
        registroEdicion = registro;
        if (_opciones.edicionInline) {
            editarInline(registro);
        } else {
            editar(registro);
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

        if (_opciones.mantenerFilaNuevoRegistro) {
            agregarFilaNuevoRegistro();
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

        // E. A. R. 18/11/2015
        // Al introducir las grillas con agrupamiento he visto que no sincroniza automáticamente al eliminar
        // Mediante este condicional se detecta si la grilla admite grupos y en caso de que los admita
        // se sicroniza explícitamente.
        if (grid.groupable) {
            // Reeleer los datos de la grilla para actualizar los valores de los agregados
            // después de la eliminación.
            grid.dataSource.one("sync", function () { grid.dataSource.read(); });
            grid.dataSource.sync();
        }
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
        registroEdicion = nuevo;
        prepararFormEdicion(nuevo);
    }

    function editar(registro) {
        capaEditor.find(".no-edicion").hide();
        capaEditor.find(".no-creacion").show();
        registroEdicion = registro;
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
        if (_opciones.ocultarGrillaAlEditar) {
            contenedorGrilla.hide();
        }
        _contenedor.trigger(_prefijoEventos + "_edit");
    }

    function limpiarValidacion(form) {
        var validator = capaEditor.find("form").kendoValidator(validationOptions).data("kendoValidator");
        if (validator) {
            validator.hideMessages();
        }
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
        registroEdicion = null;
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
        registroEdicion = grid.dataSource.add();
        grid.editRow(grid.table.find("tbody tr:first"));
    }

    function editarInline(registro) {
        var fila = grid.tbody.find(">tr[data-uid='" + registro.uid + "']");
        //grid.select(fila);
        grid.editRow(fila);

        _contenedor.trigger(_prefijoEventos + "_edit");
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
        modoEdicionActivado = true
        app.prevenirNavegacion(_textos.confirmacionNavegacion);
        _contenedor.find(".deshabilitar-en-edicion:not('input')").addClass("disabled");
        _contenedor.find("input.deshabilitar-en-edicion").attr("readonly", "readonly");
    }

    function finModoEdicion() {
        registroEdicion = null;
        modoEdicionActivado = false;
        _contenedor.find(".deshabilitar-en-edicion").not('input').removeClass("disabled");
        _contenedor.find("input.deshabilitar-en-edicion").removeAttr("readonly");
        app.prevenirNavegacion(false);
        _contenedor.trigger(_prefijoEventos + "_fin_edicion");
    }

    return {
        init: init,
        url: _url,
        contenedor: _contenedor,
        grid: function () { return grid; },
        registroEnEdicion: function() {
            return modoEdicionActivado ? registroEdicion : null;
        },
        filtroExterno: _opciones.filtroExterno,
        resetBusqueda: resetBusqueda,
        resize: resize,
        destroy: destroy,
        editarRegistro: editarRegistro,
        editar: iniciarEdicion,
        nuevo: iniciarAgregar,
        cancelarEdicion: cancelarEdicion
    };
}