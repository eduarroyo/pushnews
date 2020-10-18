(function ($, k, app, g) {
    if (app === null) {
        console.error("Imposible registrar el módulo de comunicación: la aplicación es null.");
        return;
    }

    app.modulos["comunicacion"] = (function (contenedor, url) {

        var _contenedor = contenedor;
        var _url = url;
        var textos;
        var modulosInternos = {};
        var id, contenedorDetalle, contenedorMapa, mapa, marcador, infoWindow;
        var dialogoEstadisticas;

        function init() {
            contenedorDetalle = _contenedor.find("#detalleComunicacion");
            id = contenedorDetalle.data("id");
            textos = contenedorDetalle.data("textos");

            contenedorDetalle.find(".btVolver").on("click", function (e) {
                e.preventDefault();
                window.history.back();
            });

            _contenedor.find(".btEditar").on("click", function (e) {
                e.preventDefault();
                document.location = "Backend#/editar/" + id;
            });

            _contenedor.find(".botones-cabecera").on("click", "#recargar", cargarDatos);

            galeria = $("#galeria");
            youtubeTmpl = k.template($("#galeria-youtube").html());
            imagenTmpl = k.template($("#galeria-imagen").html());

            cargarDatos();
        }

        function cargarDatos() {
            $.ajax({
                url: "/Comunicaciones/LeerUnoDetalle",
                data: { comunicacionID: id },
                method: "GET"
            }).success(function (data) {
                if (data) {
                    if (data.FechaPublicacion && typeof data.FechaPublicacion === 'string') {
                        data.FechaPublicacion = window.Util.parseJsonDate(data.FechaPublicacion);
                    }
                    var vm = k.observable(data);
                    k.bind(contenedorDetalle, vm);

                    if (data.Youtube && data.Youtube.length) {
                        vm.set("videoUrl", "https://www.youtube.com/watch?v=" + data.Youtube);
                        vm.set("imagenVideoUrl", "http://img.youtube.com/vi/" + data.Youtube + "/hqdefault.jpg");
                        galeria.append(youtubeTmpl(vm));
                    } else {
                        vm.set("hayVideo", false);
                    }

                    if (vm.get("ImagenDocumentoID")) {
                        galeria.append(imagenTmpl(vm));
                    }

                    _contenedor.find(".swipebox").swipebox();

                    if (vm.get("GeoPosicionLongitud") !== null) {
                        contenedorMapa = contenedorDetalle.find(".mapa");
                        var centro = new g.maps.LatLng(vm.get("GeoPosicionLatitud"), vm.get("GeoPosicionLongitud"));
                        var mapOptions = {
                            zoom: 16,
                            center: centro,
                            mapTypeId: g.maps.MapTypeId.ROADMAP
                        };
                        mapa = new g.maps.Map(contenedorMapa[0], mapOptions);

                        // Inicializar el marcador del mapa
                        marcador = new g.maps.Marker({
                            position: centro,
                            draggable: false,//self.opciones.permitirArrastrarMarcador,
                            map: mapa,
                            title: vm.get("GeoPosicionDireccion")
                        });

                        var direccion = vm.get("GeoPosicionDireccion");
                        var localidad = vm.get("GeoPosicionLocalidad");

                        var contenido = "";
                        if (direccion && direccion.length) {
                            contenido = "<div>" + direccion + "<div>" + contenido;
                        }
                        if (localidad && localidad.length) {
                            contenido += "<div>" + localidad + "</div>";
                        }
                        contenido += "<div>" + vm.get("GeoPosicionLatitud") + ", "
                                     + vm.get("GeoPosicionLongitud") + "</div>";

                        infoWindow = new g.maps.InfoWindow({ content: contenido });

                        marcador.addListener("click", function () {
                            infoWindow.open(mapa, marcador);
                        });

                        mapa.addListener("click", function () {
                            infoWindow.close();
                        });
                        infoWindow.open(mapa, marcador);
                    }

                } else {
                    app.notificarError(textos.ErrorCargarNotificacion);
                }
            }).error(function () {
                app.notificarError(textos.ErrorCargarNotificacion);
            })
        }

        function abrirDialogoEstadisticas() {
            var grid = dialogoEstadisticas.find("#EstadisticasAsociadosGrid").data("kendoGrid");
            grid.dataSource.read();
            dialogoEstadisticas.modal("show");
        }

        function resize() {
            for (var mi in modulosInternos) {
                modulosInternos[mi].resize();
            }
        }

        function destroy() {
            for (var mi in modulosInternos) {
                modulosInternos[mi].destroy();
            }
        }

        return {
            url: url,
            init: init,
            resize: resize,
            destroy: destroy
        }
    })($("#target"), "/Comunicaciones/Comunicacion");

})(jQuery, kendo, app, google);