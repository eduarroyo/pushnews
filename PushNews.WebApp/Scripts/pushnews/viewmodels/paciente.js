if (!window.Viewmodels) {
    window.Viewmodels = {};
}

Viewmodels.Paciente = function () {
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

    this.getFechaNacimiento = function () {
        var fechaJson = this.get('Nacimiento');
        if (fechaJson && fechaJson.length) {
            var fecha = Util.parseJsonDate(fechaJson);
            return fecha.toLocaleDateString();
        }
        else {
            return "";
        }
    };

    this.getFechaDefuncion = function () {
        var fechaJson = this.get('Defuncion');
        if (fechaJson && fechaJson.length) {
            var fecha = Util.parseJsonDate(fechaJson);
            return fecha.toLocaleDateString();
        }
        else {
            return "";
        }
    };

    this.getUltimoPeso = function () {
        var peso = this.get('UltimoPeso');
        if (peso !== null && peso !== undefined) {
            return peso + " Kg";
        }
        else {
            return "";
        }
    }

    this.sexoMacho = function () {
        return this.get('SexoID') === 0;
    }

    this.getUltimoCelo = function () {
        var inicioUltimoCelo = this.get("InicioUltimoCelo");
        var finUltimoCelo = this.get("FinUltimoCelo");
        var texto = "";
        if (!this.sexoMacho() && (inicioUltimoCelo || finUltimoCelo)) {
            texto += inicioUltimoCelo ? Util.parseJsonDate(inicioUltimoCelo).toLocaleDateString() : "?";
            texto += " - ";
            texto += (finUltimoCelo ? Util.parseJsonDate(finUltimoCelo).toLocaleDateString() : "?");
        }
        return texto;
    }
}