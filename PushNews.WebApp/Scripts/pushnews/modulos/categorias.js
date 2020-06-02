(function ($, k, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de categorias: la aplicación es null.");
        return;
    }
    var modulo = ModuloGrilla($("#target"), "/Backend/Categorias", "CategoriaID", "categorias",
        {
            eliminar: false,
            edicionInline: false,
            confirmarActivarDesactivar: true,
            textoConfirmacion: function (reg) {
                return reg.Nombre;
            }
        });
    app.modulos["categorias"] = modulo;
})(jQuery, kendo, window.app);