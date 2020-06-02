window.app = (function ($, k) {
    var moduloActual = null;
    var nombreModuloActual = "";
    var modulos = {};
    var menu, menuUsuario, menuAplicaciones, notificaciones, buscador;
    var contenedorModulo;
    var navegacion;
    var permitirNavegacion = true, urlActual, mensajeConfirmacion;
    var notificationManager;

    $(document).ajaxError(function (event, jqxhr, settings, thrownError) {
        //console.log("Error AJAX: ", event, jqxhr, settings, thrownError);

        if (jqxhr.status === 500 || jqxhr.status === 403) {
            var data = jqxhr.responseJSON;
            if (data && data.Titulo && data.Mensaje) {
                notificarError(data.Titulo, data.Mensaje);
            } else {
                notificarError("Error", "Ocurrió un error en el servidor: " + jqxhr.responseText);
            }
        }

        window.ajax = $.ajax;
    });

    function init() {
        contenedorModulo = $("#target");
        menu = $("#mainMenu");
        menuUsuario = $(".menu-usuario");
        menuAplicaciones = $(".menu-aplicaciones");

        configurarBloqueoPantalla();

        notificationManager = configurarNotificaciones($("#notificacion"));

        // Eventos del menú de usuario.
        menuUsuario.on("click", ".logout-button", logout);
        menuUsuario.on("click", "a:not(.logout-button)", function (ev) {
            ev.preventDefault();
            document.location = $(ev.target).attr("href");
        });

        // Eventos del menú de clínicas.
        menuAplicaciones.on("click", "a", cambiarAplicacionClick);

        // Cargar módulo de escritorio.
        navegacion.run('Backend#/escritorio');


    }

    // Configuración de navegación virtual
    // Dentro del callback de sammy se configuran las rutas virtuales y las operaciones asociadas a ellas.
    navegacion = $.sammy(function () {
        // Ruta de módulos con parámetros, por ejemplo pushnews.pushnews.com/Backend#/comunicacion/65
        this.get("Backend#/:modulo/:id", function (context) {
            var nombreModulo = this.params['modulo'];
            if (modulos[nombreModulo]) {
                cargarModulo(nombreModulo, this.params['id']);
                actualizarMenu(nombreModulo);
            } else {
                // Si el módulo no se encuentra, se redirige al escritorio.
                console.error("Módulo no encontrado: " + nombreModulo + ". Redirigiendo al escritorio.");
                context.redirect("Backend#/escritorio");
            }
        });

        // Ruta de módulos sin parámetros, por ejemplo: pushnews.pushnews.com/Backend#/comunicaciones
        this.get("Backend#/:modulo", function (context) {
            var nombreModulo = this.params['modulo'];
            if (modulos[nombreModulo]) {
                if (nombreModuloActual !== nombreModulo) {
                    cargarModulo(nombreModulo);
                    actualizarMenu(nombreModulo);
                }
            } else {
                // Si el módulo no se encuentra, se redirige al escritorio.
                console.error("Módulo no encontrado: " + nombreModulo + ". Redirigiendo al escritorio.");
                context.redirect("Backend#/escritorio");
            }
        });

        // Para cualquier otro formato de url, redirigir al escritorio.
        this.get(/Backend.*/, function (context) {
            console.error("URL desconocida '" + context.url + "'. Redirigiendo al escritorio.");
            context.redirect("Backend#/escritorio");
        });

        // Para cualquier url, antes de aplicar la regla comprobar si se está en modo de prevenir navegación
        // (Por ejemplo en medio de una edición de un formulario). Si es así, antes de continuar se pide 
        // confirmación al usuario, ya que se perderán los datos no guardados.
        this.before(/Backend.*/, function (context) {
            if (!permitirNavegacion) {
                if (confirm(mensajeConfirmacion)) {
                    prevenirNavegacion(false);
                }
                else {
                    permitirNavegacion = true;
                    context.app.setLocation(urlActual);
                    permitirNavegacion = false;
                    return false;
                }
            }
            urlActual = context.app.getLocation();
        })
    });

    function configurarBloqueoPantalla() {
        var timer;

        // Establecemos una closure para mantener el objeto timer.
        // Al iniciarse cualquier solicitud ajax, se establece el timer para bloquear la pantalla
        // a un tiempo de 1s. Si la solicitud no concluye antes de un segundo, se mostrará la 
        // pantalla de bloqueo. Si por el contrario la solicitud ajax termina antes, se cancela el
        // timer y no se mostrará el bloqueo. En cualquier caso, al terminar la solicitud se oculta
        // el bloqueo.
        $(document)
            .ajaxStart(function () {
                timer = setTimeout(bloquearPantalla, 1000); // Descomentar para utilizar timer.
                //bloquearPantalla(); // Comentar para utilizar timer.
            })
            .ajaxStop(function () {
                clearTimeout(timer); // Descomentar para utilizar timer.
                desbloquearPantalla();
            })
            .ajaxError(function() {
                desbloquearPantalla();
            });
    }

    // Manejador del evento de click en un elemento de la lista de clínicas.
    // Obtiene el id de la clínica del atributo data-aplicacionid de la etiqueta
    // seleccionada e invoca a cambiar clínica con ese dato.
    function cambiarAplicacionClick(ev) {
        ev.preventDefault();
        var target = $(ev.target);
        if(!target.hasClass("seleccionada")) {
            var aplicacionID = $(ev.target).data("aplicacionid");
            cambiarAplicacion(aplicacionID);
        }
    }

    // Establece la clínica de trabajo.
    // Para ello se realiza una solicitud ajax que actualiza la clínica de la sesión
    // y luego se realiza una redirección a la url recibida en la request.
    // Si hay un error se notifica mediante un mensaje emergente.
    function cambiarAplicacion(aplicacionID) {
        $.ajax({
            url: "/Aplicaciones/EstablecerAplicacion",
            dataType: 'json',
            data: { aplicacionID: aplicacionID, urlActual: document.location.href },
            type: 'post' 
        }).success(function (data) {            
            if (data.resul) {
                // En caso de éxito, informar al usuario y cargar la url indicada en la respuesta.
                notificarMensaje(data.titulo, data.mensaje);

                // Recuperar el redireccionamiento a data.urlDestino en caso de que volvamos a tener alguna  
                // url incompatible con el cambio de aplicación. Ver también el controlador de aplicaciones
                //var urlDestinoCompleto = document.location.protocol + "//" + document.location.host + "/" + data.urlDestino;
                //if (document.location.href === urlDestinoCompleto) {
                //    document.location.reload();
                //}
                //document.location = data.urlDestino;
                document.location.reload();
            } else {
                // En caso de error notificar al usuario.
                notificarError(data.titulo, data.mensaje);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            // Si la solicitud falla, notificar al usuario.
            notificarError(textStatus);
        });
    }

    function bloquearPantalla() {
        $.blockUI({
            message: $("#bloqueoTemplate").html(),
            fadeOut: 200,
            fadeIn: 200,
            css: {width: '200px', border: 'none', backgroundColor: 'rgba(0,0,0,0)', zIndex: '10001'}
        });
    }

    function desbloquearPantalla() {
        $.unblockUI();
    }

    // Establece la cultura de kendo en el cliente.
    function setCulture(culture) {
        _culture = culture;
        k.culture(_culture);
    }

    // Preparar el sistema de notificaciones de la interfaz de usuario.
    function configurarNotificaciones(contenedorNotificaciones) {
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

        contenedorModulo.on("notificacion", function (e, titulo, mensaje, tipo) {
            notificar(titulo, mensaje, tipo);
        });

        return manager;
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
        notificationManager.show({
            title: title,
            message: message
        }, tipo);
    }
    
    function cargarModulo(nombreModulo, id) {
        closeModule();

        // Identificador del parámetro para cada módulo
        var parametrosModulos = {
            comunicacion: "comunicacionID",
            editar: "comunicacionID"
        };

        // Establecer el módulo actual.
        nombreModuloActual = nombreModulo
        moduloActual = modulos[nombreModuloActual];

        // Datos que se envían en la solicitud. Obtenemos los parámetros requeridos
        // por nombre del módulo del dicionario parametrosModulos. Si hay un parámetro
        // se añade como campo a datos y se asigna como valor id.
        var datos = {};
        if (parametrosModulos.hasOwnProperty(nombreModulo)) {
            var parametro = parametrosModulos[nombreModulo];
            datos[parametro] = id;
        }

        $.ajax(moduloActual.url, { data: datos }).done(function (data) {
            contenedorModulo.html(data);
            moduloActual.init(id);
        });
    }

    // Se encarga de actualizar el aspecto del menú después de cargar un módulo.
    function actualizarMenu(nombreModulo) {
        // Enlace es el enlace dentro del menú correspondiente al módulo actual.
        var enlace = menu.find("a[href='Backend#/" + nombreModulo + "']");

        // Lista es la etiqueta ul que contiene el li que a su vez contiene el enlace.
        var lista = enlace.parents("ul");
        var nuevoElementoActivo;

        // Obtener el elemento principal del menú. Se obtiene de distinta forma según si lista es
        // un submenú o un menú principal.
        if(lista.hasClass("sub-menu")) {
            nuevoElementoActivo = lista.parent().addClass("active");
        } else {
            nuevoElementoActivo = enlace.parent().addClass("active");
        }
        
        // Cerrar y eliminar clase activo del elemento activo anterior.
        var activoAnterior = menu.find(".active").not(nuevoElementoActivo);
        activoAnterior.removeClass("active").removeClass("open");

        // Si el elemento era un submenú hay que colapsarlo.
        if (activoAnterior.children('ul.sub-menu').length) {
            activoAnterior.children("ul.sub-menu").slideUp(200, function () {
                runContainerHeight();
            });
        }

        // Añadir clase activo al nuevo elemento activo del menú.
        nuevoElementoActivo.addClass("active");
    }

    // Ajustar la altura del contenedor principal.
    function runContainerHeight() {
        var $windowWidth = $(window).width();
        var $windowHeight = $(window).height();
        var $pageArea = $windowHeight - $('body > .navbar').outerHeight() - $('body > .footer').outerHeight();
        var mainContainer = $('.main-content > .container');
        var mainNavigation = $('.main-navigation');
        //if ($pageArea < 629) {
        //    $pageArea = 629;
        //}
        //if (mainContainer.outerHeight() < mainNavigation.outerHeight() && mainNavigation.outerHeight() > $pageArea) {
        //    mainContainer.css('height', mainNavigation.outerHeight());
        //} else {
        //    mainContainer.css('height', $pageArea);
        //}
        //if ($windowWidth < 768) {
        //    mainNavigation.css('height', $windowHeight - $('body > .navbar').outerHeight());
        //}
        //moduloActual.resize();

        if ($pageArea < 760) {
            $pageArea = 760;
        }
        if (mainContainer.outerHeight() < mainNavigation.outerHeight() && mainNavigation.outerHeight() > $pageArea) {
            mainContainer.css('min-height', mainNavigation.outerHeight());
        } else {
            mainContainer.css('min-height', $pageArea);
        }
        if ($windowWidth < 768) {
            mainNavigation.css('min-height', $windowHeight - $('body > .navbar').outerHeight());
        }
    };

    // Al cerrar una pestaña, eliminar del tabStrip y seleccionar la más cercana.
    function closeModule() {

        var contenidos = contenedorModulo.children();
        for (var i = 0; i < contenidos.length; i++) {
            $(contenidos[i]).detach();
        }

        if (moduloActual && moduloActual.destroy) {
            moduloActual.destroy();
        }
    }

    function logout() {
        $("#logOutForm").submit();
    }

    function listaEtiquetas(lista, max) {
        var elementosLista = lista.length;
        max = max && elementosLista < max ? elementosLista : max;

        var html = '';
        for (var i = 0; i < elementosLista; i++) {
            html += '<span class="label label-default etiqueta-lista ' + (i >= max ? 'ocultar hidden' : '')
                + '">' + lista[i] + '</span>';
        }
        if (elementosLista > max) {
            html += '<span class="label label-default etiqueta-lista mostrarExceso">...</span>';
        }
        return html;
    }

    function mostrarOcultarEtiquetasHermanas(etiqueta) {
        etiqueta.parent().children(".etiqueta-lista + .ocultar").toggleClass("hidden");
    }

    // Activa desactiva la prevención de navegación.
    // Antes de aplicar una nueva regla de navegación (ver arriba navegación = $.sammy) se comprueba si la prevención
    // de navegación está activa. Si está activa se pide confirmación antes de navegar.
    // Si se invoca pasando un mensaje se activa la prevención y se utilizará el mensaje para la confirmación.
    // Si se invoca con false o sin parámetro se desactiva la prevención de navegación.
    function prevenirNavegacion(param) {
        if (!param) {
            $(window).off("beforeunload");
            permitirNavegacion = true;
        }
        else {
            mensajeConfirmacion = param;
            $(window).on("beforeunload", function () {
                return mensajeConfirmacion;
            });
            permitirNavegacion = false;
        }
    }

    return {
        init: init,
        modulos: modulos,
        setCulture: setCulture,
        getCulture: function () { return _culture; },
        notificarExito: notificarExito,
        notificarError: notificarError,
        notificarMensaje: notificarMensaje,
        notificarAdvertencia: notificarAdvertencia,
        bloquearPantalla: bloquearPantalla,
        desbloquearPantalla: desbloquearPantalla,
        listaEtiquetas: listaEtiquetas,
        mostrarOcultarEtiquetasHermanas: mostrarOcultarEtiquetasHermanas,
        prevenirNavegacion: prevenirNavegacion,
        logout: logout
    }

})(jQuery, kendo);

$(document).ready(function() {
    app.init();
});