(function ($, k, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de parámetros: la aplicación es null.");
        return;
    }
    var modulo = ModuloGrilla($("#target"), "/Backend/Parametros", "ParametroID", "parametros",
        {
            textoConfirmacion: function (reg) {
                return reg.Nombre;
            }
        });
    app.modulos["parametros"] = modulo;
})(jQuery, kendo, window.app);