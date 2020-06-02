(function ($, k, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de características de aplicaciones: la aplicación es null.");
        return;
    }
    var modulo = ModuloGrilla(
        $("#target"),
        "/Backend/AplicacionesCaracteristicas",
        "AplicacionCaracteristicaID",
        "aplicacionesCaracteristicas",
        {
            // edicionInline: false,
            // eliminar: false,
            textoConfirmacion: function (reg) {
                return reg.Nombre;
            }
        });
    app.modulos["aplicacionesCaracteristicas"] = modulo;
})(jQuery, kendo, window.app);