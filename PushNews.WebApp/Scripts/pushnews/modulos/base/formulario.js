function Formulario(contenedor, url, datos, prefijoEventos, opcionesjs) {
    var _contenedor = contenedor, form;
    var _url = url, _prefijoEventos = prefijoEventos;
    var _opciones = $.extend({}, opcionesjs);
    var regBind = kendo.observable({});
    setData(datos);

    function init() {
        form = _contenedor.find("form");
        kendo.bind(contenedor, regBind);
    }

    function setData(data) {
        for(var p in data) {
            regBind.set(p, data[p]);
        }
    }

    function destroy() {
        _contenedor.off();
    }

    function validate() {
        if (form) {
            var val = form.kendoValidator().data("kendoValidator");
            return val && val.validate();
        }
        return false;
    }

    function sendForm(){
        if (validate()) {

            var obj = regBind.toJSON();
            for (var field in obj) {
                if(obj[field] instanceof Date) {
                    obj[field] = obj[field].toJSON();
                }
            }

            return $.ajax({
                url: _url,
                method: form.attr("method"),
                data: obj,
                dataType: "json",
                success: onFormSent
            });
        }
        else return null;
    }

    function onFormSent(data, status, jqXHR) {
        _contenedor.trigger(_prefijoEventos + "_onFormSent", [data, status, jqXHR]);
    }

    return {
        init: init,
        destroy: destroy,
        sendForm: sendForm,
        validate: validate,
        setData: setData
    };
}