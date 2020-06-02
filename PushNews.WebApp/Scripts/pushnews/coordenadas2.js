'use strict';

window.Coordenadas2 = function (contenedor, datos, textoBuscar) {
    var self = this;
    var mapa, // Objeto de google maps para el mapa.
        marcador, // Objeto de google maps para el marcador.
        _contenedor = contenedor,
        _capaMapa; // Elemento del dom para el contenedor del mapa.

    // Opciones por defecto. Estos son los valores para las opciones salvo que se especifiquen
    // otros en el parámetro opciones o bien en en atributo data-opciones del contenedor, en
    // formato json.
    var _opcionesDefault = {
        latitudInicial: 0,
        longitudInicial: 0,
        zoom: 14,
        tipoMapa: "G_NORMAL_MAP",
        animacionMarcador: "DROP",
        permitirArrastrarMarcador: true
    };

    var _textosDefault = {
        contenedorNoEncontrado: "No se ha encontrado el contenedor.",
        capaMapaNoEncontrada: "El módulo coordenadas requiere que el contenedor contenga un div con la clase 'mapa-entrada'.",
        geocodingSinResultado: "No se ha encontrado una dirección para las coordenadas.",
        errorGeocoding: "Fallo al calcular la dirección asociada a las coordenadas.",
        geocodingInversoSinResultado: "No se ha encontrado unas coordenadas asociadas a la dirección.",
        errorGeocodingInverso: "Fallo al calcular las coordenadas asociadas a la dirección.",
        titulo: "Seleccionar posición"
    };

    /** Configura el mapa con los valores iniciales */
    var prepararMapa = function (latitud, longitud) {

        google.maps.visualRefresh = true;

        // El objeto centro se construye a partir de las coordenadas iniciales del modelo y se utiliza
        // para centrar el mapa y para establecer las coordenadas del marcador.
        var centro = new google.maps.LatLng(latitud, longitud);

        var mapOptions = {
            zoom: self.opciones.zoom,
            center: centro,
            mapTypeId: google.maps.MapTypeId[self.opciones.tipoMapa]
        };
        self.mapa = new google.maps.Map(_capaMapa[0], mapOptions);

        // Inicializar el marcador del mapa
        self.marcador = new google.maps.Marker({
            position: centro,
            draggable: true,//self.opciones.permitirArrastrarMarcador,
            map: self.mapa,
            animation: google.maps.Animation[self.opciones.animacionMarcador]
        });

        // Registrar el evento dragend del marcador:
        // Al terminar de arrastrar el marcador, actualizar las coordenadas en el modelo.
        google.maps.event.addListener(self.marcador, 'dragend', function () {
            var latitudObtenida = self.marcador.getPosition().lat();
            var longitudObtenida = self.marcador.getPosition().lng()
            self.modelo.set("latitud", latitudObtenida);
            self.modelo.set("longitud", longitudObtenida);

            calcularDireccion(latitudObtenida, longitudObtenida, function (err, datos) {
                if (err) {
                    _contenedor.trigger("alerta_geocoding", [err]);
                } else {
                    // Hay que mantener las coordenadas exactas a donde se ha arrastrado el
                    // marcador, que pueden no ser exactamente las mismas que vengan en el
                    // resultado de calcular direccion (parámetro datos).
                    datos.latitud = latitudObtenida;
                    datos.longitud = longitudObtenida;
                    self.establecerCoordenadas(datos);
                }
            });
        });
    }

    /** Procesa el valor del campo dirección del modelo. Si contiene una par de floats separados
     * porespacios los toma como latitud y longitud. Si no, utiliza geoposicionamiento inverso para
     * establecer las coordenadas correspondientes a la dirección introducida.
     */
    var procesarDireccion = function () {
        var direccionBuscar = self.modelo.get("textoBuscar");
        if (direccionBuscar && direccionBuscar.length) {
            // Si direccionBuscar contiene unas coordenadas (latittud longitud), pasan directamente 
            // como nueva posición y se actualiza el mapa y  el formulario. Si no, se prueba con el
            // texto a obtener unas coordenadas mediante geocodificación inversa.
            var latLng = extraerCoordenadas(direccionBuscar);
            if (latLng && latLng.length === 2) {
                calcularDireccion(latLng[0], latLng[1], function (err, datos) {
                    if (err) {
                        _contenedor.trigger("alerta_geocoding", [err]);
                    } else {
                        //console.debug("Coordenadas buscadas vs obtenidas:",
                        //    "(" + latLng[0] + ", " + latLng[1] + ") => ("
                        //    + datos.latitud + ", " + datos.longitud);
                        self.marcador.setPosition(new google.maps.LatLng(datos.latitud, datos.longitud));
                        self.establecerCoordenadas(datos);
                    }
                });
            } else {
                calcularCoordenadas(direccionBuscar, function (err, datos) {
                    if (err) {
                        _contenedor.trigger("alerta_geocoding", [err]);
                    } else {
                        self.marcador.setPosition(
                            new google.maps.LatLng(datos.latitud, datos.longitud));
                        self.establecerCoordenadas(datos);
                    }
                });
            }
        }
    }

    var extraerCoordenadas = function (coordenadasStr) {
        var resul = null;
        var coordenadasSplit = coordenadasStr.split(" ").filter(function (c) { return c && c.length > 0; });
        if (coordenadasSplit.length === 2) {
            var lat = parseFloat(coordenadasSplit[0].replace(",", "."));
            var lng = parseFloat(coordenadasSplit[1].replace(",", "."));
            if (lat && lng && lat >= -90 && lat <= 90 && lng >= -180 && lng <= 180) {
                resul = [lat, lng];
            }
        }
        return resul;
    };

    /** Obtiene las coordenadas a partir de una dirección mediante geocodificación inversa */
    var calcularCoordenadas = function (address, callback) {
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                if (results[0]) {
                    var datos = procesarResutadosGeocoding(results[0]);
                    callback(null, datos);
                } else {
                    callback(self.textos.geocodingInversoSinResultado);
                }
            } else if (status === google.maps.GeocoderStatus.ZERO_RESULTS) {
                callback(self.textos.geocodingInversoSinResultado);
            } else {
                callback(self.textos.errorGeocodingInverso);
            }
        })
    };

    var calcularDireccion = function (latitud, longitud, callback) {
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'location': { lat: latitud, lng: longitud } }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                if (results[0]) {
                    var datos = procesarResutadosGeocoding(results[0]);
                    callback(null, datos);
                } else {
                    callback(self.textos.geocodingSinResultado);
                }
            } else if (status === google.maps.GeocoderStatus.ZERO_RESULTS) {
                callback(self.textos.geocodingSinResultado);
            } else {
                callback(self.textos.errorGeocoding);
            }
        })
    };

    /** Extrae los datos del resultado de geocodificación inversa y devuelve un objeto plano con
     * los campos codigoPostal, localidad, provincia, pais, direccion, direccionCompleta, latitud y
     * longitud.
     */
    var procesarResutadosGeocoding = function (resultados) {
        var obj = {};

        // Convertir los datos de geocoding en un objeto plano que tiene como propiedades el tipo de
        // cada apartado y como valor el texto largo. (ver 
        resultados.address_components.forEach(function (item) {
            obj[item.types[0]] = item.long_name;
        });

        // La dirección se compone de los campos route y el campo street_number si tiene valor.
        var direccion = ""
            + (obj.route && obj.route.length ? obj.route : "")
            + (obj.street_number && obj.street_number.length ? " " + obj.street_number : "");

        var datos = {
            codigoPostal: obj.postal_code,
            localidad: obj.locality,
            provincia: obj.administrative_area_level_2,
            pais: obj.country,
            direccion: direccion,
            direccionCompleta: resultados.formatted_address,
            latitud: resultados.geometry.location.lat(),
            longitud: resultados.geometry.location.lng()
        }
        return datos;
    };

    this.abrirModal = function () {
        _contenedor.modal("show");
    }

    /** Manejador del evento click del botón aceptar. Establece el resultado a "ACEPTAR" y cierra 
     *  el modal. */
    this.aceptar = function () {
        self.modelo.set("resultado", "ACEPTAR");
        _contenedor.modal("hide");
    }

    /** Manejador del evento click del botón cancelar. Recupera los valores originales, establece
     *  el resutlado a "Cancelar" y cierra el modal.
     */
    this.cancelar = function () {
        self.modelo.set("resultado", "CANCELAR");
        _contenedor.modal("hide");
    }

    /** Devuelve un array (lat, lng) correspondiente a las coordenadas seleccionadas. */
    this.resultado = function () {
        return {
            latitud: self.modelo.get("latitud"),
            longitud: self.modelo.get("longitud"),
            direccion: self.modelo.get("direccion"),
            codigoPostal: self.modelo.get("codigoPostal"),
            pais: self.modelo.get("pais"),
            direccionCompleta: self.modelo.get("direccionCompleta"),
            localidad: self.modelo.get("localidad"),
            provincia: self.modelo.get("provincia"),
            resultadoDialogo: self.modelo.get("resultado")
        };
    }

    /** Establece las coordenadas recibidas en el marcador del mapa y en los campos de edición */
    this.establecerCoordenadas = function (datos) {
        self.modelo.set("latitud", datos.latitud),
        self.modelo.set("longitud", datos.longitud),
        self.modelo.set("direccion", datos.direccion),
        self.modelo.set("codigoPostal", datos.codigoPostal),
        self.modelo.set("pais", datos.pais),
        self.modelo.set("direccionCompleta", datos.direccionCompleta),
        self.modelo.set("localidad", datos.localidad),
        self.modelo.set("provincia", datos.provincia)

        self.centrarMapa();
    }

    /** Centra el mapa en la posición seleccionada. */
    this.centrarMapa = function () {
        self.mapa.setCenter(self.marcador.position);
    }
    
    // Inicializar las opciones combinando las opciones por defecto con las opciones de la 
    // etiqueta y las del parámetro.
    this.opciones = $.extend(null, _opcionesDefault, _contenedor.data("opciones"));
    this.textos = $.extend(null, _textosDefault, _contenedor.data("textos"));
    
    this.modelo = kendo.observable({
        latitud: datos.latitud ? datos.latitud : this.opciones.latitudInicial,
        longitud: datos.longitud ? datos.longitud : this.opciones.longitudInicial,
        direccion: datos.direccion,
        codigoPostal: datos.codigoPostal,
        pais: datos.pais,
        direccionCompleta: "",
        localidad: datos.localidad,
        provincia: datos.provincia,
        textoBuscar: textoBuscar,
        resultado: "CANCELAR",
        buscarDireccion: procesarDireccion,
        fnAceptar: self.aceptar,
        fnCancelar: self.cancelar,
        titulo: self.textos.titulo
    });
    kendo.bind(_contenedor, this.modelo);

    _capaMapa = _contenedor.find("#mapa");
    _contenedor.one("shown.bs.modal", function () {
        prepararMapa(self.modelo.get("latitud"), self.modelo.get("longitud"));
    });
    _contenedor.one("hidden.bs.modal", function () {
        _capaMapa.html("");
    });
};