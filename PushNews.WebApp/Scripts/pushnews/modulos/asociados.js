(function ($, k, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de asociados: la aplicación es null.");
        return;
    }

    var prefijoEventos = "asociados";
    var campoID = "AsociadoID";
    var url = "/Backend/Asociados";
    var contenedor = $("#target");
    var modulo = ModuloGrilla(contenedor, url, campoID, prefijoEventos,
        {
            edicionInline: false,
            confirmarActivarDesactivar: true,
            textoConfirmacion: function (reg) {
                return reg.Codigo;
            }
        });

    contenedor.on(prefijoEventos + "_init", function () {
        modulo.grid().element.on("click", ".btCambiarClave", abrirDialogoCambiarClave);
    });

    function cambiarClaveGuardar(ev) {
        ev.preventDefault();
        var dialogo = $("#DialogoCambiarClave");
        var formCambiarClave = dialogo.find("form");
        var val = formCambiarClave.kendoValidator().data("kendoValidator");
        if (val.validate()) {
            var url = formCambiarClave.attr("action");
            var inputAsociadoID = formCambiarClave.find("#chPasswordModel_UsuarioID");
            $.post(url, {
                UsuarioID: inputAsociadoID.val(),
                Clave: formCambiarClave.find("#chPasswordModel_Clave").val(),
                ConfirmarClave: formCambiarClave.find("#chPasswordModel_ConfirmarClave").val()
            }).done(function (data) {
                if (data.errors.length) {
                    dialogo.one("click", ".btCambiarClaveGuardar", cambiarClaveGuardar);
                    modulo.contenedor.trigger("notificacion", ["", data.errors.join("\n"), "error"]);
                } else {
                    dialogo.modal("hide");
                }
            }).error(function (data) {
                dialogo.one("click", ".btCambiarClaveGuardar", cambiarClaveGuardar);
                console.error(data);
            });
        }
    }

    function abrirDialogoCambiarClave(ev) {
        ev.preventDefault();
        var filaEditar = $(ev.target).parents("tr:not(.k-grid-edit-row)");
        var asociadoID = modulo.grid().dataItem(filaEditar).AsociadoID;

        var dialogo = $("#DialogoCambiarClave");
        dialogo.find("#chPasswordModel_UsuarioID").val(asociadoID);
        dialogo.one('hidden.bs.modal', cerrarDialogoCambiarClave);
        dialogo.one("click", ".btCambiarClaveGuardar", cambiarClaveGuardar);
        dialogo.modal("show");
    }

    function cerrarDialogoCambiarClave() {
        var form = $(this).find("form");
        var validator = form.kendoValidator().data("kendoValidator");
        $(this).find("#chPasswordModel_Clave").val("");
        $(this).find("#chPasswordModel_ConfirmarClave").val("");
        $(this).find("#chPasswordModel_UsuarioID").val('');
        validator.hideMessages();
        $(this).find(".k-invalid").removeClass("k-invalid");
    }

    app.modulos["asociados"] = modulo;

})(jQuery, kendo, window.app);