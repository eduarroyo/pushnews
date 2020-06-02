if (!window.Viewmodels) {
    window.Viewmodels = {};
}

Viewmodels.Producto = function () {

    this.getFechaAlta = function () {
        var fechaJson = this.get('Alta');
        if (fechaJson && fechaJson.length) {
            var fecha = Util.parseJsonDate(fechaJson);
            return fecha.toLocaleDateString();
        }
        else {
            return "";
        }
    };

    this.getFechaBaja = function () {
        var fechaJson = this.get('Baja');
        if (fechaJson && fechaJson.length) {
            var fecha = Util.parseJsonDate(fechaJson);
            return fecha.toLocaleDateString();
        }
        else {
            return "";
        }
    };

    this.precioCompraNeto = function () {
        var precioCompraBruto = this.get("PrecioCompra");
        var impuesto1 = this.get("ImpuestoCompraIva");
        var productoTieneRE = this.get("RE");
        var impuesto2 = productoTieneRE ? this.get("ImpuestoCompraRE") : 0;
        return precioCompraBruto * (1 + impuesto1 + impuesto2);
    };

    this.precioVentaNeto = function () {
        var precioVentaBruto = this.get("PrecioVenta");
        var impuesto1 = this.get("ImpuestoVentaIva");
        var impuesto2 = this.get("ImpuestoVentaRE");
        return precioVentaBruto * (1 + impuesto1 + impuesto2);
    };
    
};