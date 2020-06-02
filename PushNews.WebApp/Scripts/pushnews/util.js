window.Util = window.Util || {};

// Produce un objeto Date a partir de una fecha en formato JSON "/Date(1293034567877)/"
window.Util.parseJsonDate = function (jsonDate) {
    // Obtener el número entre paréntesis, convertido en entero.
    var num = jsonDate.match(/\d+/)[0] * 1;
    return new Date(num);
};

window.Util.boolToCheck = function (value) {
    if (value) {
        return '<i class="fa fa-check"></i>';
    } else {
        return "";
    }
}

window.Util.lPad = function(cadena, longitud, caracter) {
    var relleno = "";
    if(!caracter) {
        caracter = "0";
    }
    for(var i = longitud; i--;) {
        relleno += caracter;
    }

    var resultado = (relleno + cadena.toString()).slice(-1*longitud);
    return resultado;
};

window.Util.formatearTimeSpan = function (timeSpan) {
    resultado = "";
    if (timeSpan) {
        var dias = timeSpan.Days;
        var horas = window.Util.lPad(timeSpan.Hours, 2, "0");
        var minutos = window.Util.lPad(timeSpan.Minutes, 2, "0");
        var segundos = window.Util.lPad(timeSpan.Seconds, 2, "0");
        resultado = (dias > 0 ? dias + "d + " : "") + horas + ":" + minutos + ":" + segundos;
    }
    return resultado;
}


window.Util.limpiarValidacion = function (form) {
    var validator = form.kendoValidator().data("kendoValidator");
    validator.hideMessages();
    form.find(".k-invalid").removeClass("k-invalid");
}

window.Util.resetUpload = function (upload) {
    upload.element.parents(".k-widget").find(".k-upload-status").remove();
    upload.element.parents(".k-widget").find(".k-upload-files").remove();
};

window.Util.generarCadenaAleatoria = function (longitud) {
    var caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    var cadenaAleatoria = "", aleatorio;

    for (var i = 0; i < longitud; i++) {
        aleatorio = Math.floor(Math.random() * (caracteres.length - 1));
        cadenaAleatoria += caracteres.substring(aleatorio, aleatorio + 1);
    }
    return cadenaAleatoria;
}