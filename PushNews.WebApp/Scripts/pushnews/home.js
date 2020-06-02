window.homeapp = (function ($, k, g, contenedor) {
    var _culture;
    var _contenedor = contenedor;
    var listaComunicaciones, comboCategorias, listaCategorias, contenedorMapa, mapa, marcador, infoWindow;
    var galeria, youtubeTmpl, imagenTmpl;
    var categoriasDataSource = new k.data.DataSource({
        schema: {
            model: { id: "CategoriaID"}
        },
        autoBind: false,
        data: _contenedor.data("categorias")
    });

    var model = k.observable({
        categoriaFiltrar: null,
        listaVisible: true,
        enBusqueda: false,

        // Muestra la lista de comunicaciones ocultando la vista de detalles.
        mostrarLista: function () {
            this.set("listaVisible", true);
            eliminarMetas("og");
            eliminarMetas("twitter");
            scrollToTop();
            _contenedor.removeClass("hidden");
            galeria.html("");
        },

        // Carga el detalle de una comunicación y muestra la vista de detalle, ocultando la lista.
        verDetalles: function (id) {
            var _self = this;
            this.set("comunicacionID", id);
            $.ajax({
                url: "Home/ComunicacionDetalle",
                method: "GET",
                dataType: "json",
                data: { comunicacionID: id }
            }).success(function (data) {
                var vm, centro, mapOptions, direccion, localidad, contenido;
                if (data) {
                    if (data.FechaPublicacion && typeof data.FechaPublicacion === 'string') {
                        data.FechaPublicacion = window.Util.parseJsonDate(data.FechaPublicacion);
                    }
                    vm = k.observable(data);
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

                    k.bind(_contenedor.find(".datos-comunicacion"), vm);

                    if (vm.get("hayVideo") || vm.get("ImagenDocumentoID")) {
                        $(".swipebox").swipebox();
                    }

                    if (vm.get("GeoPosicionLongitud") !== null) {
                        contenedorMapa = contenedor.find(".mapa");
                        centro = new g.maps.LatLng(vm.get("GeoPosicionLatitud"), vm.get("GeoPosicionLongitud"));

                        // Inicializar el mapa sólo la primera vez
                        if (!mapa) {
                            mapOptions = {
                                zoom: 16,
                                center: centro,
                                mapTypeId: g.maps.MapTypeId.ROADMAP
                            };
                            mapa = new g.maps.Map(contenedorMapa[0], mapOptions);
                            mapa.addListener("click", function () {
                                infoWindow.close();
                            });
                        }

                        // Inicializar el marcador del mapa sólo la primera vez
                        if (!marcador) {
                            marcador = new g.maps.Marker({
                                position: centro,
                                draggable: false,//self.opciones.permitirArrastrarMarcador,
                                map: mapa,
                                title: vm.get("GeoPosicionDireccion")
                            });
                            marcador.addListener("click", function () {
                                infoWindow.open(mapa, marcador);
                            });
                        }

                        // Inicializar el globo sólo la primera vez
                        if (!infoWindow) {
                            infoWindow = new g.maps.InfoWindow({});
                        }

                        // Establecer el centro del mapa, la posición del marcador y el contenido del globo.
                        mapa.setCenter(centro);
                        marcador.setPosition(centro);
                        direccion = vm.get("GeoPosicionDireccion");
                        localidad = vm.get("GeoPosicionLocalidad");
                                                
                        contenido = "";
                        if (direccion && direccion.length) {
                            contenido = "<div>" + direccion + "<div>" + contenido;
                        }
                        if (localidad && localidad.length) {
                            contenido += "<div>" + localidad + "</div>";
                        }

                        contenido += "<div>" + vm.get("GeoPosicionLatitud") + ", " + vm.get("GeoPosicionLongitud") + "</div>";
                        infoWindow.setContent(contenido);

                        // Abrir el globo.
                        infoWindow.open(mapa, marcador);
                    }

                    _self.set("listaVisible", false);
                    _contenedor.find(".swipebox").swipebox();
                    scrollToTop();
                    _contenedor.removeClass("hidden");
                }
            });
        },

        // Carga la lista de comunicaciones filtrando por categoría
        buscar: function () {
            this.set("enBusqueda", true);
            var categoria = this.get("categoriaFiltrar");
            var filtro = categoria.CategoriaID ? { field: "CategoriaID", operator: "eq", value: categoria.CategoriaID } : {};
            var ds = listaComunicaciones.dataSource;
            ds.filter(filtro);
        }

    });

    // Configuración de la navegación virtual
    var app = $.sammy(function () {

        // Ruta para la vista de listado de comunicados.
        this.get("/", function (context) {
            model.mostrarLista();
        });

        this.get("/:id", function (context) {
            var id = this.params['id'];
            if (id > 0) {
                model.verDetalles(id);
            } else {
                document.location = "/";
            }
        });

        // Ruta para la vista de detalles de un comunicado identificado por su id.
        this.get("#/:id", function (context) {
            var id = this.params['id'];
            if (id > 0) {
                model.verDetalles(id);
            } else {
                document.location = "/";
            }
        });
    });


    function init() {
        // Inicializar la navegación virtual
        galeria = $("#galeria");
        youtubeTmpl = k.template($("#galeria-youtube").html());
        imagenTmpl = k.template($("#galeria-imagen").html());

        // Evento para volver arriba al hacer click en el botón del pie de página.
        $(".go-top").on("click", scrollToTop);

        listaComunicaciones = _contenedor.find("#listaComunicaciones").data("kendoListView");
        listaComunicaciones.bind("dataBound", function () {
            $(".timeago").timeago(); // Reemplazar fechas por tiempos textuales de timeago.
            scrollToTop(); // Volver arriba con el scroll.
        });

        // Establecer datasource compartido para el combo y la lista de categorías.
        comboCategorias = _contenedor.find("#comboCategorias").data("kendoDropDownList");
        comboCategorias.setDataSource(categoriasDataSource);
        listaCategorias = _contenedor.find("#listaCategorias").data("kendoListView");
        listaCategorias.setDataSource(categoriasDataSource);

        // Emparejar el modelo con la vista.
        k.bind(_contenedor, model);

        // Buscar cuando cambie el valor del campo categoriaFiltrar del view model.
        model.bind("change", function (e) {
            if (e.field === "categoriaFiltrar") {
                model.buscar();
            }
        });

        // Seleccionar la categoría "todas" para lanzar la carga de comunicaciones.
        model.set("categoriaFiltrar", categoriasDataSource.data()[0]);

        // Evitar navegación al hacer click en las imágenes de la galería de fotos.
        galeria.on("click", "a", function (e) { e.preventDefault(); });


        app.run();
    }
    
    // Elimina los metas de la cabecera cuyos atributos "property" o "name" empiecen por el prefijo que se 
    // recibe como parámetro.
    function eliminarMetas(prefijo) {
        var metas = $("meta");
        metas.each(function (index, meta) {
            var attrProperty = $(meta).attr("property");
            var attrName = $(meta).attr("name");
            if ((attrProperty && attrProperty.startsWith("prefijo"))
                || (attrName && attrName.startsWith("prefijo"))) {
                $(meta).remove();
            }
        });
    }
    
    // Establece la cultura de kendo en el cliente.
    function setCulture(culture) {
        _culture = culture;
        k.culture(_culture);
    }

    // Obtiene la cultura establecida en el cliente.
    function getCulture() {
        return _culture;
    }

    // Scroll hasta arriba.
    function scrollToTop(e) {
        $("html, body").animate({
            scrollTop: 0
        }, "fast");
        if (e) {
            e.preventDefault();
        }
    }

    return {
        app: app,
        model: model,
        init: init,
        setCulture: setCulture,
        getCulture: getCulture
    };

})(jQuery, kendo, google, jQuery(".contenedor-modulo"));

$(function () {
    var culture = $("body").data("culture");
    window.homeapp.setCulture(culture);
    window.homeapp.init();
});
