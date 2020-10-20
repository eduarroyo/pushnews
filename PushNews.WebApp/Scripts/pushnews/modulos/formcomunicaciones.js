(function ($, k, util, app) {
    if (app === null) {
        console.error("Imposible registrar el módulo de formulario de comunicaciones: la aplicación es null.");
        return;
    }

    app.modulos["formcomunicaciones"] = function (contenedor, url) {

        var _contenedor = contenedor;
        var _url = url;
        var textosFormComunicacion;
        var formPublicar, uploadDocumento, uploadImagen;
        var permitirUbicacion = false;
        var comunicacionModel = k.observable({
            ComunicacionID: 0,
            Titulo: "",
            Descripcion: "",
            Autor: "",
            CategoriaID: null,
            Destacado: false,
            FechaPublicacion: new Date(),
            Recordatorio: false,
            RecordatorioTitulo: "",
            RecordatorioFecha: null,
            ImagenTitulo: "",
            ImagenDocumentoID: null,
            ImagenUrl: "",
            ImagenNombre: "",
            AdjuntoTitulo: "",
            AdjuntoDocumentoID: null,
            AdjuntoUrl: "",
            AdjuntoNombre: "",
            EnlaceTitulo: "",
            Enlace: "",
            YoutubeTitulo: "",
            Youtube: "",
            urlVideo: "",
            urlImagenVideo: function () {
                var code = comunicacionModel.get("Youtube");
                return code && code.length 
                    ? "http://img.youtube.com/vi/" + code + "/hqdefault.jpg"
                    : false;
            },
            GeoPosicionTitulo: "",
            GeoPosicionLatitud: null,
            GeoPosicionLongitud: null,
            GeoPosicionLocalidad: "",
            GeoPosicionProvincia: "",
            GeoPosicionPais: "",
            Activo: true,
            cancelar: function(e) {
                e.preventDefault();
                window.history.back();
            },
            guardar: function (e) {
                e.preventDefault();
                var validator = formPublicar.data("kendoValidator");
                if (validator.validate()) {
                    guardarComunicacion(this);
                }
            },
            toggleDestacado: function (e) {
                this.set("Destacado", !this.get("Destacado"));
            },
            quitarAdjunto: function (e) {
                var adjuntoID = this.get("AdjuntoDocumentoID");
                if (adjuntoID) {
                    quitarAdjunto(adjuntoID);
                }
            },
            quitarImagen: function(e) {
                var imagenID = this.get("ImagenDocumentoID");
                if(imagenID) {
                    quitarImagen(imagenID);
                }
            },
            mostrarOcultarCoordenadas: function (e) {
                if (!permitirUbicacion) return;

                if (_contenedor.find("#MostrarCoordenadas").prop("checked")) {
                    contenedorCoordenadas.collapse('show');
                    _contenedor.find("#GeoPosicionTitulo").parent(".form-group").show();
                    if (!coordenadas.modelo.inicializado()) {
                        coordenadas.init();
                    }
                    coordenadas.centrarMapa();
                } else {
                    contenedorCoordenadas.collapse('hide');
                    _contenedor.find("#GeoPosicionTitulo").parent(".form-group").hide();
                    comunicacionModel.set("GeoPosicionTitulo", "");
                }
            },
            cambioCategoria: function (ev) {
            }
        });

        var contenedorCoordenadas, coordenadas;

        // Capturar el evento set del viewmodel para añadir "http://" al texto del enlace cuando se modifica.
        comunicacionModel.bind("set", aniadirProtocolo);

        function quitarAdjunto(documentoID) {
            util.resetUpload(uploadDocumento);
            comunicacionModel.set("", "");
            comunicacionModel.set("AdjuntoTitulo", "");
            comunicacionModel.set("AdjuntoDocumentoID", null);
            comunicacionModel.set("AdjuntoUrl", "");
            comunicacionModel.set("AdjuntoNombre", "");
        }

        function quitarImagen(imagenID) {
            util.resetUpload(uploadImagen);
            comunicacionModel.set("", "");
            comunicacionModel.set("ImagenTitulo", "");
            comunicacionModel.set("ImagenDocumentoID", null);
            comunicacionModel.set("ImagenUrl", "");
            comunicacionModel.set("ImagenNombre", "");
        }

        function actualizarCoordenadas() {
            if (!permitirUbicacion) return;
            if (_contenedor.find("#MostrarCoordenadas").prop("checked")) {
                var longitud = coordenadas.modelo.get("coordenadas.longitud");
                comunicacionModel.set("GeoPosicionLongitud", longitud);
                var latitud = coordenadas.modelo.get("coordenadas.latitud");
                comunicacionModel.set("GeoPosicionLatitud", latitud);
                var direccion = coordenadas.modelo.get("direccion.direccion");
                comunicacionModel.set("GeoPosicionDireccion", direccion);
                var localidad = coordenadas.modelo.get("direccion.localidad");
                comunicacionModel.set("GeoPosicionLocalidad", localidad);
                var provincia = coordenadas.modelo.get("direccion.provincia");
                comunicacionModel.set("GeoPosicionProvincia", provincia);
                var pais = coordenadas.modelo.get("direccion.pais");
                comunicacionModel.set("GeoPosicionPais", pais);
            } else {
                comunicacionModel.set("GeoPosicionLongitud", null);
                comunicacionModel.set("GeoPosicionLatitud", null);
                comunicacionModel.set("GeoPosicionDireccion", null);
                comunicacionModel.set("GeoPosicionLocalidad", null);
                comunicacionModel.set("GeoPosicionProvincia", null);
                comunicacionModel.set("GeoPosicionPais", null);
                comunicacionModel.set("GeoPosicionTitulo", null);

            }
        }

        function aniadirProtocolo(e) {
            if (e.field === "Enlace") {
                var valorActual = e.value;
                if (valorActual === "") {
                    return;
                }
                var regexProtocolo = /^(http|https|ftp|mailto|ldap|file|news|gopher|telnet|data):\/\//i;
                if (!regexProtocolo.test(valorActual)) {
                    // El setTimeout es para que la actualización se produzca un ciclo de ejecución después
                    // de la actualización del modelo. Si no se hace así, kendo actualiza el textbox con el
                    // valor original, antes de añadir el protocolo.
                    setTimeout(function() {
                        comunicacionModel.unbind("set", aniadirProtocolo);
                        comunicacionModel.set("Enlace", "http://" + valorActual);
                        comunicacionModel.bind("set", aniadirProtocolo);
                        var validator = formPublicar.data("kendoValidator");
                        validator.validateInput(formPublicar.find("#Enlace"));
                    })
                }
            }
        }

        function guardarComunicacion(com) {
            // Copiar las coordenadas y la dirección del modelo del módulo de coordenadas al modelo de 
            // comunicación.
            actualizarCoordenadas();

            // Esta copia del objeto es para poder dar formato a la fecha, porque serializando el objeto 
            // entero no la reconoce el servidor.
            var data = {
                FechaPublicacion: com.FechaPublicacion.toJSON(),
                Recordatorio: com.Recordatorio,
                RecordatorioTitulo: com.RecordatorioTitulo,
                RecordatorioFecha: com.RecordatorioFecha ? com.RecordatorioFecha.toJSON() : null,
                ComunicacionID: com.ComunicacionID,
                Titulo: com.Titulo,
                Descripcion: com.Descripcion,
                Autor: com.Autor,
                CategoriaID: com.CategoriaID,
                Destacado: com.Destacado,
                ImagenTitulo: com.ImagenTitulo,
                ImagenDocumentoID: com.ImagenDocumentoID,
                ImagenUrl: com.ImagenUrl,
                AdjuntoTitulo: com.AdjuntoTitulo,
                AdjuntoDocumentoID: com.AdjuntoDocumentoID,
                AdjuntoUrl: com.AdjuntoUrl,
                EnlaceTitulo: com.EnlaceTitulo,
                Enlace: com.Enlace,
                YoutubeTitulo: com.YoutubeTitulo,
                Youtube: com.Youtube,
                GeoPosicionTitulo: com.GeoPosicionTitulo,
                GeoPosicionLatitud: com.GeoPosicionLatitud
                    ? com.GeoPosicionLatitud.toString().replace(".", ",")
                    : null,
                GeoPosicionLongitud: com.GeoPosicionLongitud ?
                    com.GeoPosicionLongitud.toString().replace(".", ",")
                    : null,
                GeoPosicionDireccion: com.GeoPosicionDireccion,
                GeoPosicionLocalidad: com.GeoPosicionLocalidad,
                GeoPosicionProvincia: com.GeoPosicionProvincia,
                GeoPosicionPais: com.GeoPosicionPais,
                Activo: com.Activo
            };
            var req = $.ajax({
                url: "/Comunicaciones/CrearModificar",
                method: "POST",
                data: data
            }).success(function (data) {
                if (data) {
                    if (data.Errors) {
                        for (var e in data.Errors) {
                            if (data.Errors[e].hasOwnProperty("errors")) {
                                app.notificarError(data.Errors[e].errors.join("; "));
                            } else {
                                app.notificarError(data.Errors[e]);
                            }
                        }
                    } else {
                        app.notificarExito(textosFormComunicacion.PublicacionGuardada);
                        _contenedor.trigger("comunicacionGuardada");
                    }
                }
            });
        }

        function borrarDatosComunicacion() {
            comunicacionModel.set("Titulo", "");
            comunicacionModel.set("Descripcion", "");
            comunicacionModel.set("Autor", "");
            comunicacionModel.set("CategoriaID", null);
            comunicacionModel.set("Destacado", false);
            comunicacionModel.set("FechaPublicacion", new Date());
            comunicacionModel.set("Recordatorio", false);
            comunicacionModel.set("RecordatorioFecha", null);
            comunicacionModel.set("RecordatorioTitulo", "");
            comunicacionModel.set("Activo", true);
            comunicacionModel.set("ImagenTitulo", "");
            comunicacionModel.set("ImagenDocumentoID", null);
            comunicacionModel.set("ImagenUrl", "");
            comunicacionModel.set("ImagenNombre", "");
            comunicacionModel.set("AdjuntoTitulo", "");
            comunicacionModel.set("AdjuntoDocumentoID", null);
            comunicacionModel.set("AdjuntoUrl", "");
            comunicacionModel.set("AdjuntoNombre", "");
            comunicacionModel.set("EnlaceTitulo", "");
            comunicacionModel.set("Enlace", "");
            comunicacionModel.set("YoutubeTitulo", "");
            comunicacionModel.set("Youtube", "");
            comunicacionModel.set("GeoPosicionTitulo", "");
            comunicacionModel.set("GeoPosicionLatitud", null);
            comunicacionModel.set("GeoPosicionLongitud", null);
            comunicacionModel.set("GeoPosicionDireccion", "");
            comunicacionModel.set("GeoPosicionLocalidad", "");
            comunicacionModel.set("GeoPosicionProvincia", "");
            comunicacionModel.set("GeoPosicionPais", "");

            _contenedor.find("#MostrarCoordenadas").attr("checked", false);
            comunicacionModel.mostrarOcultarCoordenadas();

            util.resetUpload(uploadDocumento);
            util.resetUpload(uploadImagen);
        }

        function establecerDatosComunicacion(com) {
            if (com) {
                comunicacionModel.set("FechaPublicacion", new Date(com.FechaPublicacion));
                comunicacionModel.set("Recordatorio", com.Recordatorio);
                comunicacionModel.set("RecordatorioTitulo", com.RecordatorioTitulo);
                comunicacionModel.set("RecordatorioFecha", new Date(com.RecordatorioFecha));
                comunicacionModel.set("ComunicacionID", com.ComunicacionID);
                comunicacionModel.set("Titulo", com.Titulo);
                comunicacionModel.set("Descripcion", com.Descripcion);
                comunicacionModel.set("Autor", com.Autor);
                comunicacionModel.set("CategoriaID", com.CategoriaID);
                comunicacionModel.set("Destacado", com.Destacado);
                comunicacionModel.set("ImagenTitulo", com.ImagenTitulo);
                comunicacionModel.set("ImagenDocumentoID", com.ImagenDocumentoID);
                comunicacionModel.set("ImagenUrl", com.ImagenUrl);
                comunicacionModel.set("ImagenNombre", com.ImagenNombre);
                comunicacionModel.set("AdjuntoTitulo", com.AdjuntoTitulo);
                comunicacionModel.set("AdjuntoDocumentoID", com.AdjuntoDocumentoID);
                comunicacionModel.set("AdjuntoUrl", com.AdjuntoUrl);
                comunicacionModel.set("AdjuntoNombre", com.AdjuntoNombre);
                comunicacionModel.set("EnlaceTitulo", com.EnlaceTitulo);
                comunicacionModel.set("Enlace", com.Enlace);
                comunicacionModel.set("YoutubeTitulo", com.YoutubeTitulo);
                comunicacionModel.set("Youtube", com.Youtube);
                comunicacionModel.set("urlVideo", com.Youtube && com.Youtube.length ? "https://www.youtube.com/watch?v=" + com.Youtube : "");
                comunicacionModel.set("GeoPosicionTitulo", com.GeoPosicionTitulo);
                comunicacionModel.set("GeoPosicionLatitud", com.GeoPosicionLatitud);
                comunicacionModel.set("GeoPosicionLongitud", com.GeoPosicionLongitud);
                comunicacionModel.set("GeoPosicionDireccion", com.GeoPosicionDireccion);
                comunicacionModel.set("GeoPosicionLocalidad", com.GeoPosicionLocalidad);
                comunicacionModel.set("GeoPosicionProvincia", com.GeoPosicionProvincia);
                comunicacionModel.set("GeoPosicionPais", com.GeoPosicionPais);

                if (com.GeoPosicionLatitud !== null || com.GeoPosicionLongitud !== null
                    || (com.GeoPosicionDireccion && com.GeoPosicionDireccion.length)
                    || (com.GeoPosicionLocalidad && GeoPosicionLocalidad.length)
                    || (com.GeoPosicionProvincia && com.GeoPosicionProvincia.length)) {

                    coordenadas.modelo.set("coordenadas.latitud", com.GeoPosicionLatitud);
                    coordenadas.modelo.set("coordenadas.longitud", com.GeoPosicionLongitud);
                    coordenadas.modelo.set("direccion.direccion", com.GeoPosicionDireccion);
                    coordenadas.modelo.set("direccion.localidad", com.GeoPosicionLocalidad);
                    coordenadas.modelo.set("direccion.provincia", com.GeoPosicionProvincia);
                    coordenadas.modelo.set("direccion.pais", com.GeoPosicionPais);
                    _contenedor.find("#GeoPosicionTitulo").parent(".form-group").show();
                    _contenedor.find("#MostrarCoordenadas").prop("checked", true);
                    comunicacionModel.mostrarOcultarCoordenadas();
                }

                comunicacionModel.set("Activo", com.Activo);
            }
        }

        function init() {
            _contenedor.find("#GeoPosicionTitulo").parent(".form-group").hide();
            publicarComunicacion = _contenedor.find(".seccion-publicar");
            textosFormComunicacion = _contenedor.data("textos");
            formPublicar = _contenedor.find("form");
            formPublicar.kendoValidator();
            k.bind(_contenedor, comunicacionModel);
            
            // Obtener el objeto del control kendo upload para los documentos y manejar el evento de exito
            // para recuperar los datos devueltos por la acción (el id del documento creado).
            uploadDocumento = $("#DocumentoAdjunto").data("kendoUpload");
            uploadDocumento.bind("success", function (e) {
                if (!e.response || !e.response.DocumentoID) {
                    app.notificarError(textosFormComunicacion.ErrorSubirFichero);
                    comunicacionModel.set("AdjuntoDocumentoID", null);
                    comunicacionModel.set("AdjuntoNombre", "");
                    util.resetUpload(uploadDocumento);
                } else {
                    comunicacionModel.set("AdjuntoUrl", e.response.Url);
                    comunicacionModel.set("AdjuntoDocumentoID", e.response.DocumentoID);
                    comunicacionModel.set("AdjuntoNombre", e.response.Nombre);
                }
            });
            uploadDocumento.bind("error", function (e) {
                app.notificarError(textosFormComunicacion.ErrorSubirFichero);
            });

            // Obtener el objeto del control kendo upload para las imagenes y manejar el evento de exito
            // para recuperar los datos devueltos por la acción (el id del documento creado).
            uploadImagen = $("#ImagenAdjunta").data("kendoUpload");
            uploadImagen.bind("success", function (e) {
                if (!e.response || !e.response.DocumentoID) {
                    app.notificarError(textosFormComunicacion.ErrorSubirFichero);
                    comunicacionModel.set("ImagenDocumentoID", null);
                    comunicacionModel.set("ImagenNombre", "");
                    util.resetUpload(uploadImagen);
                } else {
                    comunicacionModel.set("ImagenUrl", e.response.Url);
                    comunicacionModel.set("ImagenDocumentoID", e.response.DocumentoID);
                    comunicacionModel.set("ImagenNombre", e.response.Nombre);
                }
            });
            uploadImagen.bind("error", function (e) {
                app.notificarError(textosFormComunicacion.ErrorSubirFichero);
            });

            contenedorCoordenadas = _contenedor.find("#contenedorCoordenadas");
            permitirUbicacion = !!contenedorCoordenadas.length;
            if (permitirUbicacion) {
                contenedorCoordenadas.on("error_geocoding", function (ev, mensaje) {
                    app.notificarError(textos.TituloErrorGeocoding, mensaje);
                });
                contenedorCoordenadas.on("alerta_geocoding", function (ev, mensaje) {
                    app.notificarAdvertencia(textos.TituloErrorGeocoding, mensaje);
                });

                coordenadas = new Coordenadas(contenedorCoordenadas);
            }

            comunicacionModel.bind("change", function (ev) {
                if (ev.field === "urlVideo") {
                    var erVideo = new RegExp(/https?:\/\/www\.youtube\.com\/watch\?v=(.+)/);
                    var grupos = erVideo.exec(comunicacionModel.get("urlVideo"));
                    if (grupos && grupos.length) {
                        comunicacionModel.set("Youtube", grupos[1]);
                    } else {
                        comunicacionModel.set("Youtube", "");
                    }
                    $(".swipebox").swipebox();
                } else if (ev.field === "Recordatorio") {
                    // Limpiar validación del formulario
                    util.limpiarValidacion(formPublicar);
                }
            });
        }

        function resize() {
        }

        function destroy() {
        }

        return {
            url: url,
            init: init,
            resize: resize,
            destroy: destroy,
            establecerDatosComunicacion: establecerDatosComunicacion,
            borrarDatosComunicacion: borrarDatosComunicacion
        }
    };

})(jQuery, kendo, window.Util, window.app);