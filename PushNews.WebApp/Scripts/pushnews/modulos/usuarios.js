(function ($, k, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de usuarios: la aplicación es null.");
        return;
    }

    var prefijoEventos = "usuarios";
    var campoID = "UsuarioID";
    var url = "/Backend/Usuarios";
    var contenedor = $("#target");
    var modulo = ModuloGrilla(contenedor, url, campoID, prefijoEventos,
        {
            edicionInline: false,
            confirmarActivarDesactivar: true,
            textoConfirmacion: function (reg) {
                return reg.Email;
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
            var inputUsuarioID = formCambiarClave.find("#chPasswordModel_UsuarioID");
            $.post(url, {
                UsuarioID: inputUsuarioID.val(),
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
        var usuarioID = modulo.grid().dataItem(filaEditar).UsuarioID;

        var dialogo = $("#DialogoCambiarClave");
        dialogo.find("#chPasswordModel_UsuarioID").val(usuarioID);
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

    app.modulos["usuarios"] = modulo;

})(jQuery, kendo, window.app);