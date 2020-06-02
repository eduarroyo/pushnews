if (!window.Viewmodels) {
    window.Viewmodels = {};
}

Viewmodels.AlbaranEntrada = function () {

    this.getFecha = function () {
        var fechaJson = this.get('Fecha');
        return fechaJson && fechaJson.length
            ? Util.parseJsonDate(fechaJson)
            : null;
    };
    this.getProximoPago= function () {
        var fechaJson = this.get('ProximoPago');
        return fechaJson && fechaJson.length
            ? Util.parseJsonDate(fechaJson)
            : null;
    };
};