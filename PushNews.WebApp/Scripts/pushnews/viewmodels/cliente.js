if (!window.Viewmodels) {
    window.Viewmodels = {};
}

Viewmodels.Cliente = function () {
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

    this.abreviarSiEsLargo = function (texto, longitudMaxima) {
        return texto.length > longitudMaxima
            ? texto.substr(0, longitudMaxima - 3) + "..."
            : texto;
    }

    this.comoNosConocioAbr = function () {
        var comoNosConocio = this.get('ComoNosConocio');
        if (!comoNosConocio) {
            comoNosConocio = "";
        }

        // Garantizar que el texto no será más largo de 50 caracteres.
        // Tiene en cuenta los puntos suspensivos que se agregan sólo si se abrevia.
        return this.abreviarSiEsLargo(comoNosConocio, 50);
    }
}