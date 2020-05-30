var Login = function () {

    var manager;

    var runLoginButtons = function () {
        $('.forgot').bind('click', function () {
            $('.box-login').hide();
            $('.box-forgot').show();
        });
        $('.register').bind('click', function () {
            $('.box-login').hide();
            $('.box-register').show();
        });
        $('.go-back').click(function () {
            $('.box-login').show();
            $('.box-forgot').hide();
            $('.box-register').hide();
        });
    };
    var runSetDefaultValidation = function () {
        $.validator.setDefaults({
            errorElement: "span", // contain the error msg in a small tag
            errorClass: 'help-block',
            errorPlacement: function (error, element) { // render error placement for each input type
                if (element.attr("type") == "radio" || element.attr("type") == "checkbox") { // for chosen elements, need to insert the error after the chosen container
                    error.insertAfter($(element).closest('.form-group').children('div').children().last());
                } else if (element.attr("name") == "card_expiry_mm" || element.attr("name") == "card_expiry_yyyy") {
                    error.appendTo($(element).closest('.form-group').children('div'));
                } else {
                    error.insertAfter(element);
                    // for other inputs, just perform default behavior
                }
            },
            ignore: ':hidden',
            highlight: function (element) {
                $(element).closest('.help-block').removeClass('valid');
                // display OK icon
                $(element).closest('.form-group').removeClass('has-success').addClass('has-error').find('.symbol').removeClass('ok').addClass('required');
                // add the Bootstrap error class to the control group
            },
            unhighlight: function (element) { // revert the change done by hightlight
                $(element).closest('.form-group').removeClass('has-error');
                // set error class to the control group
            },
            success: function (label, element) {
                label.addClass('help-block valid');
                // mark the current input as valid and display OK icon
                $(element).closest('.form-group').removeClass('has-error');
            },
            highlight: function (element) {
                $(element).closest('.help-block').removeClass('valid');
                // display OK icon
                $(element).closest('.form-group').addClass('has-error');
                // add the Bootstrap error class to the control group
            },
            unhighlight: function (element) { // revert the change done by hightlight
                $(element).closest('.form-group').removeClass('has-error');
                // set error class to the control group
            }
        });
    };
    var runLoginValidator = function () {
        var form = $('.form-login');
        var errorHandler = $('.errorHandler', form);
        form.validate({
            rules: {
                username: {
                    minlength: 2,
                    required: true
                },
                password: {
                    minlength: 6,
                    required: true
                }
            },
            submitHandler: function (form) {
                errorHandler.hide();
                form.submit();
            },
            invalidHandler: function (event, validator) { //display error alert on form submit
                errorHandler.show();
            }
        });
    };
    var runForgotValidator = function () {
        var form2 = $('.form-forgot').kendoValidator();
        var val = form2.data("kendoValidator");

        form2.submit(function (ev) {
            var token, email;
            ev.preventDefault();
            if (!val.validate()) {
                alert("datos no válidos");
            } else {

                token = form2.find('input[name="__RequestVerificationToken"]').val();
                email = form2.find("#Email").val();

                $.ajax({
                    url: "/Account/RecuperarClave",
                    method: "POST",
                    data: {
                        __RequestVerificationToken: token,
                        email: email
                    },
                }).done(function(data) {
                    if (data.resul) {
                        notificarExito(data.mensaje);
                        form2.find("#Email").val('');
                        $('.box-login').show();
                        $('.box-forgot').hide();
                        $('.box-register').hide();
                    } else {
                        notificarError(data.mensaje)
                    }
                });
            }
        });
    };
    function runNotificationManager(contenedorNotificaciones) {
        // Si necesitamos aplicar configuración personal a las notificaciones, como
        // ocultar automáticamente a los x segundos, podemos utilizar el atributo data-opciones.
        //var conf = contenedorNotificaciones.data("opciones");

        manager = $("#notification").kendoNotification({
            position: {
                pinned: false,
                top: 30,
                right: 30
            },
            autoHideAfter: 5000,
            stacking: "down",
            templates: [{
                type: "info",
                template: $("#infoNotificationTemplate").html()
            }, {
                type: "error",
                template: $("#errorNotificationTemplate").html()
            }, {
                type: "success",
                template: $("#successNotificationTemplate").html()
            }, {
                type: "warning",
                template: $("#warningNotificationTemplate").html()
            }]

        }).data("kendoNotification");
    }

    function notificarExito(title, message) {
        var tipo = "success";
        if (arguments.length === 1) {
            notificar(arguments[0], "", tipo);
        } else if (arguments.length === 2) {
            notificar(arguments[0], arguments[1], tipo);
        }
    }

    function notificarError(title, message) {
        var tipo = "error";
        if (arguments.length === 1) {
            notificar(arguments[0], "", tipo);
        } else if (arguments.length === 2) {
            notificar(arguments[0], arguments[1], tipo);
        }
    }

    function notificarMensaje(title, message) {
        var tipo = "info";
        if (arguments.length === 1) {
            notificar(arguments[0], "", tipo);
        } else if (arguments.length === 2) {
            notificar(arguments[0], arguments[1], tipo);
        }
    }

    function notificarAdvertencia(title, message) {
        var tipo = "warning";
        if (arguments.length === 1) {
            notificar(arguments[0], "", tipo);
        } else if (arguments.length === 2) {
            notificar(arguments[0], arguments[1], tipo);
        }
    }

    function notificar(title, message, tipo) {
        manager.show({
            title: title,
            message: message
        }, tipo);
    }

    function mostrarExito(mensaje) {
        $(".mensajes").clean();
        $(".mensajes").append("<div class='alert alert-success' role='alert'>" + mensaje + "</div>");
    }

    function mostrarError(mensaje) {
        $(".mensajes").clean();
        $(".mensajes").append("<div class='alert alert-warning' role='alert'>" + mensaje + "</div>");
    }

    var runRegisterValidator = function () {
        var form3 = $('.form-register');
        var errorHandler3 = $('.errorHandler', form3);
        form3.validate({
            rules: {
                full_name: {
                    minlength: 2,
                    required: true
                },
                address: {
                    minlength: 2,
                    required: true
                },
                city: {
                    minlength: 2,
                    required: true
                },
                gender: {
                    required: true
                },
                email: {
                    required: true
                },
                password: {
                    minlength: 6,
                    required: true
                },
                password_again: {
                    required: true,
                    minlength: 5,
                    equalTo: "#password"
                },
                agree: {
                    minlength: 1,
                    required: true
                }
            },
            submitHandler: function (form) {
                errorHandler3.hide();
                form3.submit();
            },
            invalidHandler: function (event, validator) { //display error alert on form submit
                errorHandler3.show();
            }
        });
    };
    return {
        //main function to initiate template pages
        init: function () {
            runLoginButtons();
            //runSetDefaultValidation();
            //runLoginValidator();
            runForgotValidator();
            //runRegisterValidator();
            runNotificationManager();
        }
    };
}();