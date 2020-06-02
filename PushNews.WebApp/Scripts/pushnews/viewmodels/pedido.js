if (!window.Viewmodels) {
    window.Viewmodels = {};
}

Viewmodels.Pedido = function () {

    this.getFecha = function () {
        var fechaJson = this.get('FechaCreacion');
        if (fechaJson && fechaJson.length) {
            var fecha = Util.parseJsonDate(fechaJson);
            return fecha.toLocaleDateString();
        }
        else {
            return "";
        }
    };
};