(function ($, k, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de opciones: la aplicación es null.");
        return;
    }

    var modulo = (function (contenedor, url, prefijoEventos, opciones) {

        var _contenedor = contenedor, _url = url, _prefijoEventos = prefijoEventos,
            opDefault = {};
        var _opciones = $.extend({}, opDefault, opciones);

        function init() {
            var formularios = _contenedor.find(".form-opciones");
            formularios.submit(enviarFormOpciones);
            formularios.on("click", ".btGuardar", btGuardarFormClick);

            _contenedor.trigger(_prefijoEventos + "_init");
        }

        function btGuardarFormClick(ev) {
            ev.preventDefault();
            $(this).parents("form").submit();
        }

        function enviarFormOpciones(e) {
            var model = {}, form = $(this);
            e.preventDefault();

            $.each(form.serializeArray(), function () {
                model[this.name] = this.value;
            });

            $.ajax({
                type: 'POST',
                url: "/Perfiles/GuardarOpciones",
                data: JSON.stringify(model),
                dataType: "json",
                contentType: "application/json"
            }).done(function (data) {
                if (data.errores && data.errores.length) {
                    // ....
                } else {
                    if (form.data("reload")) {
                        location.reload();
                    }
                }
            });
        }

        function resize() {
        }

        function destroy() {
        }

        return {
            // Acciones del escritorio
            url: url,
            init: init,
            resize: resize,
            destroy: destroy,
            contenedor: _contenedor
        }
    })($("#target"), "/Backend/Perfiles/Opciones", "opciones");

    app.modulos["opciones"] = modulo;

})(jQuery, kendo, app);