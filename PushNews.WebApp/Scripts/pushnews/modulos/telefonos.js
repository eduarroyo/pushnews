(function ($, k, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de teléfonos: la aplicación es null.");
        return;
    }
    var modulo = ModuloGrilla($("#target"), "/Backend/Telefonos", "TelefonoID", "telefonos",
        {
            edicionInline: false,
            textoConfirmacion: function (reg) {
                return reg.Descripcion;
            }
        });
    app.modulos["telefonos"] = modulo;
})(jQuery, kendo, window.app);