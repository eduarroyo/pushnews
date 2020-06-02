/* Módulo para entrada de coordenadas/direcciones.
 *
 * Depedencias: jquery, google maps y kendo.
 * Google maps: "http://maps.google.com/maps/api/js?sensor=true&languaje=<culture>"
 * Requiere que el contenedor (ver parámetro) contenga un div con la clase mapa-entrada.
 * El contenedor puede tener el atributo data-opciones, cuyo valor será un objeto en formato json.
 * Los textos para internacionalización se pueden especificar mediante el atributo data-textos del contenedor
 * en formato json. Los textos por defecto están en la variable _textosDefault. 
 *
 * El marcador del mapa y las coordenadas del modelo siempre están sincronizadas.
 * Para sincronizar la dirección con las coordenadas o viceversa, utilizar las funicones actualizarDireccion
 * y actualizarCoordenadas del modelo.
 * 
 * El módulo emite eventos cuando se producen errores de geocoding. Los eventos se lanzan
 * desde el contenedor y se llaman "error_geocoding" y "alerta_geocoding"
 * 
 * El módulo se controla a través del objeto modelo, que es devuelto por el constructor y tiene la siguiente 
 * estructura:
 * {
 *     coordenadas: {
 *         longitud: Longitud seleccionada
 *         latitud: Latitud seleccionada
 *     },
 *     direccion: {
 *         direccion: Dirección obtenida (calle y número)
 *         localidad:
 *         provincia: 
 *         pais: 
 *         codigoPostal: 
 *         direccionCompleta: Dirección formateada, obtenida del reverse geocoding, en el idioma indicado en
 *                            el link del script.
 *     },
 *     actualizarDireccion: función para calcular la dirección a partir de las coordenadas,
 *     actualizarCoordenadas: función para calcular las coordenadas a partir de la dirección. 
 * }
 *
 * El módulo también expone un método para centrar el mapa. Es útil cuando el mapa está oculto y luego se 
 * muestra.
 */
(function ($, g, k) {
    'use strict';

    if (!g) {
        console.error("No se han cargado las bibliotecas de google maps. ¿Problemas de conexión?");
        return null;
    }

    // Recibe los siguientes parámetros:
    // · contenedor: consulta jQuery que devuelva la capa contenedora de la vista del módulo
    // · opciones: objeto con las opciones específicas para instancia.
    // Devuelve: Objeto con las siguientes propiedades:
    // · opciones: opciones finales, calculadas a partir de las opciones del parámetro, las opciones del 
    //             atributo opciones de la etiqueta del contenedor y las opciones por defecto.
    // · función init: pone en funcionamiento el módulo. Sólo es necesario invocarla externamente si se 
    //                 la opción noIniciarMapa se pone a true.
    // · contenedor: objeto jquery del contenedor de la vista.
    // · modelo: kendo observable con los datos de dirección y coordenadas.
    window.Coordenadas = function (contenedor, opciones) {
        var self = this;
        var inicializado = false,
            mapa, // Objeto de google maps para el mapa.
            marcador, // Objeto de google maps para el marcador.
            geocoder, // Objeto de google maps para geolocalización y geolocalización inversa.
            _capaMapa, // Elemento del dom para el contenedor del mapa.
            mapaActualizado = false, // Variable utilizada como flag para detener la cadena de eventos al actualizar el mapa.
            coordenadasActualizadas = false; // Variable utilizada como flag para detener la cadena de eventos al actualizar las coordenadas del modelo.

        // Opciones por defecto. Estos son los valores para las opciones salvo que se especifiquen otros en
        // el parámetro opciones o bien en en atributo data-opciones del contenedor, en formato json.
        var _opcionesDefault = {
            noIniciarMapa: false, // Si se pone a true, hay que llamar a init desde fuera del módulo.
            latitudInicial: 0,
            longitudInicial: 0,
            direccionInicial: "",
            localidadInicial: "",
            provinciaInicial: "",
            paisInicial: "",
            codigoPostalInicial: "",
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
            errorGeocodingInverso: "Fallo al calcular las coordenadas asociadas a la dirección."
        }

        // PRIVADA.
        // Actualiza las coordenadas del marcador y del centro del mapa utilizando longitud y latitud del
        // modelo.
        var actualizarMapa = function () {
            self.marcador.setPosition(new g.maps.LatLng(
                self.modelo.get("coordenadas.latitud"),
                self.modelo.get("coordenadas.longitud")));
            self.centrarMapa();
        }

        // PRIVADA.
        // Acutaliza longitud y latitud del modelo a partir de las coordenadas del marcador del mapa.
        var actualizarModelo = function (latitud, longitud) {
            self.modelo.set("coordenadas", {
                "latitud": latitud,
                "longitud": longitud
            });
        }
        
        // PRIVADA.
        // Procesa los datos resultado de la geolocalización y la geolocalización inversa y los convierte 
        // a un objeto plano con las propiedades codigoPostal, localidad, provincia, pais, direccion, latitud
        // y longitud.
        // TODO Sería interesante proporcionar una colección de funciones que permitan convertir las claves
        // que utiliza google en los results del geocoder, y así poder personalizar la composición de los 
        // elementos. Por ejemplo:
        // function componerDireccion(geo) { return geo.route + " " + geo.street_number; }
        var procesarResutados = function (resultados) {
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
                codigoPostal: obj.zip,
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

        // PRIVADA.
        // Calcula las coordenadas a partir de la dirección utilizando geocoding inverso y actualiza el
        // modelo.
        var calcularCoordenadas = function (ev) {
            ev.preventDefault();
            var address = ([
                self.modelo.get("direccion.direccion"),
                self.modelo.get("direccion.localidad"),
                self.modelo.get("direccion.provincia"),
                self.modelo.get("direccion.pais")
            ]).filter(function (item) {
                return item && item.trim().length > 0;
            }).join(", ");

            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status === g.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        var datos = procesarResutados(results[0]);
                        actualizarModelo(datos.latitud, datos.longitud);
                        self.modelo.set("direccion", datos);
                    } else {
                        
                        self.contenedor.trigger("alerta_geocoding", [self.textos.geocodingInversoSinResultado]);
                    }
                } else if (status === g.maps.GeocoderStatus.ZERO_RESULTS) {
                    self.contenedor.trigger("alerta_geocoding", [self.textos.geocodingInversoSinResultado]);
                } else {
                    self.contenedor.trigger("error_geocoding", [self.textos.errorGeocodingInverso]);
                }
            })
        };

        // PRIVADA
        // Calcula la dirección a partir de las coordenadas utilizando geocoding y actualiza el modelo.
        var calcularDireccion = function (ev) {
            ev.preventDefault();
            var coordenadas = {
                lat: self.modelo.get("coordenadas.latitud"),
                lng: self.modelo.get("coordenadas.longitud")
            };

            geocoder.geocode({ 'location': coordenadas }, function (results, status) {
                if (status === g.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        var datos = procesarResutados(results[0]);
                        self.modelo.set("direccion", datos);
                    } else {
                        self.contenedor.trigger("alerta_geocoding", [self.textos.geocodingSinResultado]);
                    }
                } else if (status === g.maps.GeocoderStatus.ZERO_RESULTS) {
                    self.contenedor.trigger("alerta_geocoding", [self.textos.geocodingSinResultado]);
                } else {
                    self.contenedor.trigger("error_geocoding", [self.textos.errorGeocoding]);
                }
            })
        };
        
        // Inicializar el mapa, el geocoder, el binding del modelo y los eventos.
        this.init = function () {

            k.bind(self.contenedor, self.modelo);

            // Inicializar el mapa.
            g.maps.visualRefresh = true;

            // El objeto centro se construye a partir de las coordenadas iniciales del modelo y se utiliza
            // para centrar el mapa y para establecer las coordenadas del marcador.
            var centro = new g.maps.LatLng(self.modelo.coordenadas.latitud, self.modelo.coordenadas.longitud);
            var mapOptions = {
                zoom: self.opciones.zoom,
                center: centro,
                mapTypeId: g.maps.MapTypeId[self.opciones.tipoMapa]
            };
            self.mapa = new g.maps.Map(_capaMapa[0], mapOptions);

            // Inicializar el marcador del mapa
            self.marcador = new g.maps.Marker({
                position: centro,
                draggable: true,//self.opciones.permitirArrastrarMarcador,
                map: self.mapa,
                animation: g.maps.Animation[self.opciones.animacionMarcador]
            });

            // Registrar el evento dragend del marcador:
            // Al terminar de arrastrar el marcador, actualizar las coordenadas en el modelo.
            g.maps.event.addListener(self.marcador, 'dragend', function () {
                mapaActualizado = true;
                if (!coordenadasActualizadas) {
                    actualizarModelo(self.marcador.getPosition().lat(),
                                     self.marcador.getPosition().lng());
                }
                mapaActualizado = false;
            });

            // Al terminar de arrastrar el marcador, centrar el mapa en las coordenadas del marcador.
            g.maps.event.addListener(self.marcador, 'dragend', function () {
                self.centrarMapa();
            });

            // Objeto para geolocalización y geolocalización inversa.
            geocoder = new g.maps.Geocoder();

            // Al modificar el modelo, si se han cambiado las coordenadas, actualizar el mapa.
            this.modelo.bind("change", function (ev) {
                if (ev.field === "coordenadas" || ev.field === "coordenadas.latitud" || ev.field === "coordenadas.longitud") {
                    coordenadasActualizadas = true;
                    if (!mapaActualizado) {
                        actualizarMapa();
                    }
                    coordenadasActualizadas = false;
                }
            });
            inicializado = true;
        }

        // Si no se encuentra el contenedor, logea un error en consola y termina sin valor de retorno.
        this.contenedor = contenedor;
        if (!this.contenedor || !this.contenedor.length) {
            console.error(_textosDefault.contenedorNoEncontrado);
            return;
        }

        // Inicializar las opciones combinando las opciones por defecto con las opciones de la 
        // etiqueta y las del parámetro.
        this.opciones = $.extend(null, _opcionesDefault, this.contenedor.data("opciones"), opciones);
        this.textos = $.extend(null, _textosDefault, this.contenedor.data("textos"));

        // Si no se encuentra el contenedor del mapa, logea un error en consola y termina sin valor de retorno
        _capaMapa = contenedor.find(".mapa-entrada")
        if (!_capaMapa || !_capaMapa.length) {
            console.error(this.textos.capaMapaNoEncontrada);
            return;
        }

        // Inicializar el modelo de datos.
        this.modelo = k.observable({
            inicializado: function() {
                return inicializado;
            },
            // Apartado de coordenadas
            coordenadas: {
                longitud: this.opciones.longitudInicial,
                latitud: this.opciones.latitudInicial,
            },
            // Apartado de dirección
            direccion: {
                direccion: this.opciones.direccionInicial, // calle + número
                localidad: this.opciones.localidadInicial,
                provincia: this.opciones.provinicaInicial,
                pais: this.opciones.paisInicial,
                codigoPostal: this.opciones.codigoPostalInicial,
                direccionCompleta: "" // Dirección completa, se obtiene por geocoding.
            },
            // Calcula la dirección a partir de las coordenadas y actualiza el modelo.
            actualizarDireccion: calcularDireccion,
            // Calcula las coordenadas a partir de la dirección y actualiza el modelo.
            actualizarCoordenadas: calcularCoordenadas
        });

        this.centrarMapa = function () {
            self.mapa.setCenter(self.marcador.position);
        }
        
        // Inicializar el módulo si lo indica la configuración.
        if (!this.opciones.noIniciarMapa) {
            this.init();
        }
    };

})(jQuery, google, kendo);