(function ($, g) {
    $(document).ready(function () {
        /** Opciones por defecto */
        var _opciones = {
            url: "",
            latitud: 37.892394,
            longitud: -4.780709,
            zoom: 14,
            tipoMapa: "G_NORMAL_MAP",
            animacionMarcador: "DROP",
            refresco: 1.5, // Minutos
            polilineaGrosor: 2,
            refrescoPosicionUsuario: 1.5, // Minutos
            marcadorCola: true,
            trazoRecorrido: true,
            filtroInicial: "",
            bannersHabilitados: true,
            periodoBanner: 5000,
            marcadoresPatrocinadores: true
        };

        /** Textos por defecto */
        var _textos = {
            errorCapaMapaNoEncontrada: "Capa del mapa no encontrada.",
            salida: "Salida",
            carreraOficial: "Carrera Oficial",
            inicioCarreraOficial: "Inicio de la carrera oficial",
            finCarreraOficial: "Fin de la carrera oficial",
            fin: "Fin",
            cruzDeGuia: "Cruz de guía",
            finDeCortejo: "Fin de cortejo",
            posicionCruzGuia: "Posición cruz de guía",
            posicionCola: "Posición cola",
            distanciaRecorrida: "Distancia recorrida",
            velocidadMedia: "Velocidad media",
            iglesiaNombre: "Iglesia",
            iglesiaDireccion: "Dirección",
            patrocinadorDireccion: "Dirección",
            patrocinadorTelefono: "Teléfono",
            patrocinadorWeb: "Web",
            patrocinadorFacebook: "Facebook",
            patrocinadorTwitter: "Twitter",
            filtroTitulo: "Filtrar hermandades",
            hermandadesTodas: "Todas las hermandades",
            domingoRamos: "Domingo de Ramos",
            lunesSanto: "Lunes Santo",
            martesSanto: "Martes Santo",
            miercolesSanto: "Miércoles Santo",
            juevesSanto: "Jueves Santo",
            viernesSanto: "Viernes Santo",
            sabadoSanto: "Sábado Santo",
            domingoResurreccion: "Domingo de Resurrección",
            atras: "Atrás"
        };

        var carreraOficial = [
            { lat: 37.8776482, lng: -4.7790434 },
            { lat: 37.8777973, lng: -4.7791667 },
            { lat: 37.8780895, lng: -4.7794765 },
            { lat: 37.8779063, lng: -4.7797112 },
            { lat: 37.878153, lng: -4.7798681 },
            { lat: 37.8783389, lng: -4.7799642 },
            { lat: 37.8793841, lng: -4.7806527 },
            { lat: 37.8795461, lng: -4.780717 },
            { lat: 37.8795546, lng: -4.7806071 },
            { lat: 37.8796487, lng: -4.7802953 },
            { lat: 37.8794963, lng: -4.7801927 },
            { lat: 37.8791162, lng: -4.7799258 },
            { lat: 37.8791745, lng: -4.7799667 },
            { lat: 37.8795059, lng: -4.779211 },
            { lat: 37.8795789, lng: -4.7792606 },
            { lat: 37.8796604, lng: -4.7790956 },
            { lat: 37.878993, lng: -4.7785994 },
            { lat: 37.878605, lng: -4.7783332 }
        ];

        /** Div del mapa. */
        var mapaDiv = $("#mapa");
        //  Si no está en el DOM, salir de la closure.
        if (!mapaDiv.length) {
            console.error(_textos.errorCapaMapaNoEncontrada);
            return;
        }

        // Combinar las opciones por defecto con las que vienen en el atributo data-opciones del
        // div del mapa, en formato JSON. Lo mismo para los textos, con el atributo data-textos.
        var opciones = $.extend({}, _opciones, mapaDiv.data("opciones"));
        var textos = $.extend({}, _textos, mapaDiv.data("textos"));
        var timerPosicionUsuario, marcadorUsuario;

        var arrMenu = [{
            title: textos.filtroTitulo,
                id: 'menuID',
                icon: 'fa fa-reorder',
                items: [{
                    name: textos.hermandadesTodas,
                    id: 'TODAS',
                    link: '#'
                }, {
                    name: textos.domingoRamos,
                    id: "DOMINGORAMOS",
                    link: '#',
                    items: [{
                        title: textos.domingoRamos,
                        items: []
                    }]
                }, {
                    name: textos.lunesSanto,
                    id: "LUNESSANTO",
                    link: '#',
                    items: [{
                        title: textos.lunesSanto,
                        items: []
                    }]
                }, {
                    name: textos.martesSanto,
                    id: "MARTESSANTO",
                    link: '#',
                    items: [{
                        title: textos.martesSanto,
                        items: []
                    }]
                }, {
                    name: textos.miercolesSanto,
                    id: "MIERCOLESSANTO",
                    link: '#',
                    items: [{
                        title: textos.miercolesSanto,
                        items: []
                    }]
                }, {
                    name: textos.juevesSanto,
                    id: "JUEVESSANTO",
                    link: '#',
                    items: [{
                        title: textos.juevesSanto,
                        items: []
                    }]
                }, {
                    name: textos.viernesSanto,
                    id: "VIERNESSANTO",
                    link: '#',
                    items: [{
                        title: textos.viernesSanto,
                        items: []
                    }]
                }, /*{
                    name: textos.sabadoSanto,
                    id: "SABADOSANTO",
                    link: '#',
                    items: [{
                        title: textos.sabadoSanto,
                        items: []
                    }]
                },*/ {
                    name: textos.domingoResurreccion,
                    id: "DOMINGORESURRECCION",
                    link: '#',
                    items: [{
                        title: textos.domingoResurreccion,
                        items: []
                    }]
                }
            ]}
        ];
        var menuHabilitado = false;
        var ultimoMenu = "", textoSubmenu = "";
        var filtroActual = (opciones.filtroInicial && opciones.filtroInicial.length)
            ? opciones.filtroInicial
            : "TODAS";

        var tagDiaSeleccionado;
        var filtrosplit = filtroActual.split("_");
        if (filtrosplit.length > 1) {
            tagDiaSeleccionado = filtrosplit[1];
        } else {
            tagDiaSeleccionado = "";
        }

        var textoBoton;
        if (filtroActual === "TODAS") {
            textoBoton = textos.hermandadesTodas;
        } else if (filtroActual === "TODAS_DOMINGORAMOS") {
            textoBoton = textos.domingoRamos;
        } else if (filtroActual === "TODAS_LUNESSANTO") {
            textoBoton = textos.lunesSanto;
        } else if (filtroActual === "TODAS_MARTESSANTO") {
            textoBoton = textos.martesSanto;
        } else if (filtroActual === "TODAS_MIERCOLESSANTO") {
            textoBoton = textos.miercolesSanto;
        } else if (filtroActual === "TODAS_JUEVESSANTO") {
            textoBoton = textos.juevesSanto;
        } else if (filtroActual === "TODAS_VIERNESSANTO") {
            textoBoton = textos.viernesSanto;
        } else if (filtroActual === "TODAS_SABADOSANTO") {
            textoBoton = textos.sabadoSanto;
        } else if (filtroActual === "TODAS_DOMINGORESURRECCION") {
            textoBoton = textos.domingoResurreccion;
        }
        $("#filtroActual").text(textoBoton);

        var botonMenu = $("#BotonMenu");
        var menu = $("#menu");

        if (menu) {
            menu.multilevelpushmenu({
                menu: arrMenu,
                menuWidth: '80%',
                menuHeight: '100%',
                fullCollapse: true,
                collapsed: true,
                direction: 'rtl',
                backText: textos.atras,
                onGroupItemClick: cargarSubmenu,
                onItemClick: filtrarPorGrupo
            });

            // Si hay un filtro por
            if (filtroActual !== "TODAS") {
                ultimoMenu = $('#menu').multilevelpushmenu('findmenusbytitle', textoBoton);
            }

            $(window).resize(function () {
                menu.multilevelpushmenu('redraw');
            });

        } else {
            botonMenu.hide();
        };

        /** Catálogo de iconos que se utilizan en el mapa. */
        var iconos = {
            'iglesia.a': {
                url: '/img/RutasMapa/church.a.png',
                origin: new g.maps.Point(0, 0),
                anchor: new g.maps.Point(12, 12),
                size: new g.maps.Size(128, 128),
                scaledSize: new g.maps.Size(24, 24)
            },
            'iglesia.b': {
                url: '/img/RutasMapa/church.b.png',
                origin: new g.maps.Point(0, 0),
                anchor: new g.maps.Point(12, 12),
                size: new g.maps.Size(128, 128),
                scaledSize: new g.maps.Size(24, 24)
            },
            'info.a': {
                url: '/img/RutasMapa/info.a.png',
                origin: new g.maps.Point(0, 0),
                anchor: new g.maps.Point(12, 12),
                size: new g.maps.Size(128, 128),
                scaledSize: new g.maps.Size(24, 24)
            },
            'info.b': {
                url: '/img/RutasMapa/info.b.png',
                origin: new g.maps.Point(0, 0),
                anchor: new g.maps.Point(12, 12),
                size: new g.maps.Size(82, 82),
                scaledSize: new g.maps.Size(24, 24)
            },
            'cruz1': {
                url: '/img/RutasMapa/cruz.png',
                origin: new g.maps.Point(0, 0),
                anchor: new g.maps.Point(12, 12),
                size: new g.maps.Size(96, 96),
                scaledSize: new g.maps.Size(24, 24)
            },
            'cruz2': {
                url: '/img/RutasMapa/cruz2.png',
                origin: new g.maps.Point(0, 0),
                anchor: new g.maps.Point(12, 12),
                size: new g.maps.Size(293, 239),
                scaledSize: new g.maps.Size(24, 24)
            },
            'inicioRuta': {
                url: 'http://maps.google.com/mapfiles/kml/paddle/red-diamond-lv.png'
            },
            'finRuta': {
                url: 'http://maps.google.com/mapfiles/kml/paddle/red-square-lv.png'
            },
            'carreraOficial': {
                url: '/img/RutasMapa/corona.png',
                origin: new g.maps.Point(0, 0),
                anchor: new g.maps.Point(12, 12),
                size: new g.maps.Size(64, 64),
                scaledSize: new g.maps.Size(24, 24)
            },
            'corona1': {
                url: '/img/RutasMapa/corona.png',
                origin: new g.maps.Point(0, 0),
                anchor: new g.maps.Point(12, 12),
                size: new g.maps.Size(64, 64),
                scaledSize: new g.maps.Size(24, 24)
            },
            'corona2': {
                url: '/img/RutasMapa/crown.2.png',
                origin: new g.maps.Point(0, 0),
                anchor: new g.maps.Point(12, 12),
                size: new g.maps.Size(64, 64),
                scaledSize: new g.maps.Size(24, 24)
            },
            'corona3': {
                url: '/img/RutasMapa/crown.3.png',
                origin: new g.maps.Point(0, 0),
                anchor: new g.maps.Point(12, 12),
                size: new g.maps.Size(64, 64),
                scaledSize: new g.maps.Size(24, 24)
            },
            'conoraEspinas': {
                url: '/img/RutasMapa/crown-of-thorns.png',
                origin: new g.maps.Point(0, 0),
                anchor: new g.maps.Point(12, 0),
                size: new g.maps.Size(64, 64),
                scaledSize: new g.maps.Size(24, 24)
            },
            'usuario': {
                url: '/img/RutasMapa/usuario.png',
                origin: new g.maps.Point(0, 0),
                anchor: new g.maps.Point(12, 20),
                size: new g.maps.Size(64, 64),
                scaledSize: new g.maps.Size(24, 24)
            }
        }

        /** Lista de colores para las rutas. */
        var coloresRutas = ["#055CB3", "#B62E2E", "#30BCF8", "#068404", "#FF7F2A", "#6F72C1",
                            "#C26B42", "#800E0E", "#3CCC78"];

        /** Contador cíclico para el color de la ruta actual. */
        var colorActualRuta = 0;

        /** Lista de marcadores del mapa. */
        var marcadores = {};

        /** Marcadores de hermandades clasificados por día de la semana santa */
        var hermandadesPorDia = {};

        /** Lista de rutas del mapa. */
        var rutas = {};

        /** InfoWindow abierta. Cuando se abre un globo se asigna a este identificador. */
        infoWindowAbierta = null;

        g.maps.visualRefresh = true;

        // Inicializar el mapa.
        var mapa = new g.maps.Map(mapaDiv[0], {
            zoom: opciones.zoom,
            center: new g.maps.LatLng(opciones.latitud, opciones.longitud),
            mapTypeId: g.maps.MapTypeId[opciones.tipoMapa]
        });

        // Cerrar los globos abiertos al hacer click en cualquier parte del mapa.
        mapa.addListener('click', function () {
            if (infoWindowAbierta) {
                infoWindowAbierta.close();
            }
        });

        var bannerPatrocinadores = $("#banners");
        var patrocinadores = [];
        var intervaloPatrocinadores = null;
        var patrocinadorActualIndice = null;
        
        if (mapaDiv.length && !opciones.bannersHabilitados) {
            bannerPatrocinadores.hide();
            mapaDiv.css({ height: "100%" });
        }

        /** Solicita los datos de hermandades, patrocinadores y rutas al servidor.
         * En caso de éxito, invoca a la función actualizarMapa.
         * En caso de error invoca a la función error.
         */
        function cargarDatos() {
            $.ajax({
                url: opciones.url,
                method: "POST",
                data: {ps: getParameterByName("ps")},
                success: actualizarMapa,
                error: error
            });
        }

        timerPosicionUsuario = setInterval(actualizarPosicionUsuario,
                                           opciones.refrescoPosicionUsuario * 60000);
        
        /** Obtiene las coordenadas del usuario y llama al callback.
         * En caso de obtener las coordenadas correctamente, invoca a posicionUsuario.
         * Si la obtención falla invoca a cancelarTimer.
         */
        function actualizarPosicionUsuario() {
            navigator.geolocation.getCurrentPosition(posicionUsuario, cancelarTimer);
        }

        /** Actualiza las coordenadas del marcador que indica la ubicación del usuario.
         * @param {object} posicionUsuario Objeto con las coordenadas del usuario y la precisión.
        */
        function posicionUsuario(posicionUsuario) {
            if (posicionUsuario) {
                var latitud = posicionUsuario.coords.latitude;
                var longitud = posicionUsuario.coords.longitude;

                if (!marcadorUsuario) {
                    var icono = iconos["usuario"];
                    marcadorUsuario = crearMarcador(latitud, longitud, "", icono, 0);
                }
                marcadorUsuario.setPosition(new g.maps.LatLng(latitud, longitud));
            }
        }

        /** Cancela el ciclo de actualización de la posición del usuario. */
        function cancelarTimer() {
            clearInterval(timerPosicionUsuario);
            delete timerPosicionUsuario;
        }

        function habilitarMenu() {
            botonMenu.on("click", function () {
                if (ultimoMenu.length) {
                    // cargarSubmenu
                    cargarSubmenu(ultimoMenu);
                    menu.multilevelpushmenu("expand", ultimoMenu);
                } else {
                    menu.multilevelpushmenu("expand");
                }
            });
            menuHabilitado = true;
        }

        /** Limpia el mapa de marcadores y rutas y luego genera todos los marcadores y rutas del
         * objeto data. Se usa como callback del caso de éxito de cargarDatos.
         * @param {object} data Datos de marcadores y rutas.
         */
        function actualizarMapa(data) {

            // Si el servicio indica que hay que redirigir...
            if (data && data.redirectUrl) {
                window.location = data.redirectUrl;
                return;
            }


            if(!menuHabilitado) {
                habilitarMenu();
            }

            quitarMarcadores();

            // Reiniciar el ciclo de colores.
            colorActualRuta = 0;

            // Pintar cada ruta.
            for (var i = 0; i < data.Rutas.length; i++) {
                var hermandadRuta = data.Hermandades.find(function (h) {
                    return h.HermandadID == data.Rutas[i].HermandadID;
                });
                nuevaRuta(data.Rutas[i], hermandadRuta);
            }

            if (data.Rutas.length) {
                pintarCarreraOficial();
            }

            // Pintar cada marcador de hermandad que no tenga una ruta activa.
            for (var i = 0; i < data.Hermandades.length; i++) {
                // Sólo crear los marcadores de la hermandad si no tiene una ruta activa.
                if (!rutas[data.Hermandades[i].HermandadID]) {
                    nuevoMarcadorHermandad(data.Hermandades[i]);
                }
            }

            for (var i = 0; i < data.Patrocinadores.length; i++) {
                // Pintar cada marcador de patrocinador si la configuración lo indica.
                if (opciones.marcadoresPatrocinadores) {
                    nuevoMarcadorPatrocinador(data.Patrocinadores[i]);
                }

                // Clasificar los patrocinadores
                if (!patrocinadores[data.Patrocinadores[i].EmpresaID]) {
                    patrocinadores.push(data.Patrocinadores[i]);
                }
            }

            if (patrocinadores.length && opciones.bannersHabilitados) {
                if (!intervaloPatrocinadores) {
                    iniciarCicloPatrocinadores();
                }
                bannerPatrocinadores.show();
            } else {
                clearInterval(intervaloPatrocinadores);
                patrocinadorActualIndice = null;
                bannerPatrocinadores.empty();
                bannerPatrocinadores.hide();
            }

            filtrarPorTag(filtroActual);
        }

        function iniciarCicloPatrocinadores() {
            actualizarBanner();
            intervaloPatrocinadores = setInterval(actualizarBanner, opciones.periodoBanner);
        }

        /** Determina cuál es el siguiente banner a mostrar y actualiza el banner */
        function actualizarBanner() {
            
            if (patrocinadorActualIndice != null && patrocinadorActualIndice >= 0) {
                patrocinadorActualIndice = (1 + patrocinadorActualIndice) % patrocinadores.length;
            } else {
                patrocinadorActualIndice = 0;
            }
            establecerBanner(patrocinadores[patrocinadorActualIndice]);
        }

        function establecerBanner(patrocinador) {
            bannerPatrocinadores.empty();
            console.log("Mostrando patrocinador: " + patrocinador.EmpresaID);
            bannerPatrocinadores.append("<img src='" + patrocinador.BannerUrl + "'/>");
        }

        /** Escribe por consola los datos de error, si ocurre un error en cargarDatos */
        function error(requestObject, error, errorThrown) {
            console.error(requestObject, error, errorThrown);
        }

        /** Elimina del mapa todos los marcadores y rutas. */
        function quitarMarcadores() {
            for (var idm in marcadores) {
                marcadores[idm].marcador.setMap(null);
                delete marcadores[idm];
            }
            for (var idr in rutas) {
                if (rutas[idr].puntos && rutas[idr].puntos.length) {
                    rutas[idr].puntos.forEach(function (p) {
                       // console.log(p);
                        p.setMap(null);
                    });

                    delete rutas[idr];
                }
            }
            for (var tag in hermandadesPorDia) {
                var hermandadesDia = hermandadesPorDia[tag];
                for (var hermandadId in hermandadesDia) {
                    var hermandadActual = hermandadesDia[hermandadId];
                    while(hermandadActual.elementosMapa.length) {
                        hermandadActual.elementosMapa.pop();
                    }
                }
            }
        }

        function pintarCarreraOficial() {
            var inicioCO = carreraOficial[0];
            var marcadorInicio = crearMarcador(inicioCO.lat, inicioCO.lng,
                textos.inicioCarreraOficial, iconos["conoraEspinas"], 1);
            /*var infoWindowInicio = */crearInfoWindow(textos.inicioCarreraOficial,
                inicioCO.lat, inicioCO.lng, -21, 0, marcadorInicio);
            marcadores["co1"] = { marcador: marcadorInicio/*, infoWindow: infoWindowInicio*/ };

            var finCO = carreraOficial.slice(-1)[0];
            var marcadorFin = crearMarcador(finCO.lat, finCO.lng,
                textos.finCarreraOficial, iconos["conoraEspinas"], 1);
            /*var infoWindowFin =*/ crearInfoWindow(textos.finCarreraOficial,
                finCO.lat, finCO.lng, -21, 0, marcadorFin);
            marcadores["co1"] = { marcador: marcadorFin/*, infoWindow: infoWindowFin*/ };

            // Trazo de la carrera oficial
            var polyline = new g.maps.Polyline({
                path: carreraOficial,
                geodesic: true,
                zindex:0,
                strokeColor: "#F57C00",
                strokeOpacity: 1.0,
                strokeWeight: opciones.polilineaGrosor,
                map: mapa,
                title: textos.carreraOficial,
                clickable: false
            });

            // Click en el trazo de la ruta abre el globo en la posición del marcador de cabeza.
            rutas["carreraOficial"] = { polyline };
        }

        /** Genera una nueva ruta en el mapa, con su polilínea, marcadores, infoWindow y eventos.
         * @param {object} ruta Datos de la ruta.
         * @param {object} hermandad Datos de la hermandad.
          */
        function nuevaRuta(ruta, hermandad) {

            // Trazo de recorrido: desde la cabeza hasta la cola si hay dato de cola.
            // Desde la cabeza hasta el origen si no hay cola.
            // Sólo se dibuja si la opción está activada.
            if (opciones.trazoRecorrido) {
                rutas[ruta.HermandadID] = { ruta: ruta, puntos: [] };

                // La polilínea de ruta se dibuja desde al cola hasta la cabeza.
                // Si hay posición de cola, ese es el primer punto. Luego se añaden todos los puntos de
                // cabeza en el orden en que vienen. El último coincidirá siempre con la posición 
                // actual de cabeza, y el marcador de cruz de guía.
                puntosPolilinea = [];
                
                var color = coloresRutas[(colorActualRuta++) % coloresRutas.length];
                ruta.Posiciones.forEach(function (p) {
                    if (p.Latitud !== ruta.CabezaLatitud || p.Longitud !== ruta.CabezaLongitud) {

                        var posActual = { lat: p.Latitud, lng: p.Longitud };
                        var circulo = new google.maps.Marker({
                            position: posActual,
                            map: mapa,
                            //optimized: false, // para que funcione el zindex
                            zIndex: 1,
                            icon: {
                                path: google.maps.SymbolPath.CIRCLE,
                                fillOpacity: 1,
                                fillColor: color,
                                strokeOpacity: 1.0,
                                strokeColor: color,
                                strokeWeight: 3.0,
                                scale: 5 //pixels
                            }
                        });

                        rutas[ruta.HermandadID].puntos.push(circulo)
                        //clasificarMarcadorHermandad(hermandad, circulo);
                    }
                });

                if (ruta.ColaLatitud && ruta.ColaLongitud) {
                    var cola = new google.maps.Marker({
                        position: new google.maps.LatLng(ruta.ColaLatitud, ruta.ColaLongitud),
                        map: mapa,
                        //optimized: false, // para que funcione el zindex
                        zIndex: 1,
                        icon: {
                            path: google.maps.SymbolPath.CIRCLE,
                            fillOpacity: 1,
                            fillColor: color,
                            strokeOpacity: 1.0,
                            strokeColor: color,
                            strokeWeight: 3.0,
                            scale: 5 //pixels
                        }
                    });

                    rutas[ruta.HermandadID].puntos.push(cola)
                    clasificarMarcadorHermandad(hermandad, cola);
                }

                clasificarMarcadorHermandad(hermandad, ruta);
            }

            // Marcador de cabeza: aparece en las coordenadas del punto de ruta más reciente.
            var marcadorCabeza = crearMarcadorCabezaRuta(ruta);
            /*var infoWindowCabeza = */crearInfoWindow(contentInfoWindowRuta(ruta, hermandad),
                ruta.CabezaLatitud, ruta.CabezaLongitud, -36, 0, marcadorCabeza);
            marcadores["rcab" + ruta.HermandadID] = { marcador: marcadorCabeza, ruta/*, infoWindow: infoWindowCabeza*/ };

            // No clasificamos las rutas para que no se vean afectadas por el filtro.
            //clasificarMarcadorHermandad(hermandad, marcadorCabeza);

            // Marcador de cola: sólo si la opción está activada.
            if (opciones.marcadorCola) {
                //Si hay coordenadas de cola, dibujar un círculo del mismo color que la ruta en esas
                //coordenadas
                if (ruta.ColaLatitud && ruta.ColaLongitud) {
                    var marcadorCola = crearMarcadorColaRuta(ruta);
                    /* var infoWindowCola = */crearInfoWindow(contentInfoWindowCola(ruta, hermandad),
                        ruta.ColaLatitud, ruta.ColaLongitud, -20, 0, marcadorCola);
                    marcadores["rcol" + ruta.HermandadID] = { marcador: marcadorCola, ruta/*, infoWindow: infoWindowCola */};

                    // No clasificamos las rutas para que no se vean afectadas por el filtro.
                    //clasificarMarcadorHermandad(hermandad, marcadorCola);
                }
            }
        }

        function crearMarcadorCabezaRuta(ruta) {
            return crearMarcador(ruta.CabezaLatitud, ruta.CabezaLongitud,
                          ruta.CabezaDireccion, nuevoIconoCruz(ruta.HermandadID), 10);
        }

        function crearMarcadorColaRuta(ruta) {
            return crearMarcador(ruta.ColaLatitud, ruta.ColaLongitud, ruta.ColaDireccion, nuevoIconoCorona(ruta.HermandadID), 5);
        }

        function nuevoIconoCruz(hermandadId) {
            var idHermandadStr = ("0000" + hermandadId.toString()).slice(-4);
            return {
                url: '/img/RutasMapa/cruz2.png',
                origin: new g.maps.Point(0, 0),
                anchor: new g.maps.Point(12, 12),
                size: new g.maps.Size(96, 96),
                scaledSize: new g.maps.Size(24, 24)
            };
        }

        function nuevoIconoCorona(hermandadId) {
            var idHermandadStr = ("0000" + hermandadId.toString()).slice(-4);
            return {
                url: '/img/RutasMapa/crown.3.png',
                origin: new g.maps.Point(0, 0),
                anchor: new g.maps.Point(12, 12),
                size: new g.maps.Size(64, 64),
                scaledSize: new g.maps.Size(24, 24)
            };
        }
        
        function clasificarMarcadorHermandad(hermandad, marcador) {
            if(hermandad) {
                var hermandadId = hermandad.HermandadID;
                var tagsDia = hermandad.Tags.split(",");
                for (var i = 0; i < tagsDia.length; i++) {
                    var tag = tagsDia[i].trim();
                    if (!hermandadesPorDia[tag]) {
                        hermandadesPorDia[tag] = {};
                    }
                    if (!hermandadesPorDia[tag][hermandadId]) {
                        hermandadesPorDia[tag][hermandadId] = { hermandad: hermandad, elementosMapa: [] };
                    }
                    hermandadesPorDia[tag][hermandadId].elementosMapa.push(marcador);
                }
            }
        }

        /** Genera el HTML para el contenido del globo de una ruta.
         * @param {object} ruta Datos de la ruta.
         * @param {object} hermandad Datos de la hermandad que realiza la ruta.
         */
        function contentInfoWindowRuta(ruta, hermandad) {
            var content = "";
            if (hermandad) {
                content =
                    "<div class='info-window-content'>" +
                        "<div class='col-izquierda'>" +
                            "<img src='/" + hermandad.LogotipoUrl + "'/>" +
                        "</div>" +
                        "<div class='col-derecha'>" +
                            "<h2>" + hermandad.Nombre + "</h2>" +
                            "<h3>" + textos.cruzDeGuia + "</h3>" +
                            "<dl>" +
                                strContenidoRuta(ruta) +
                            "</dl>" +
                        "</div>" +
                    "</div>";
            }
            return content;
        };

        function contentInfoWindowCola(ruta, hermandad) {
            var content = ""
            if (hermandad) {
                content =
                    "<div class='info-window-content'>" +
                        "<div class='col-izquierda'>" +
                            "<img src='/" + hermandad.LogotipoUrl + "'/>" +
                        "</div>" +
                        "<div class='col-derecha'>" +
                            "<h2>" + hermandad.Nombre + "</h2>" +
                            "<h3>" + textos.finDeCortejo + "</h3>" +
                            "<dl>" +
                                strContenidoRuta(ruta) +
                            "</dl>" +
                        "</div>" +
                    "</div>";
            }
            return content;
        }

        /** Genera parte del HTML del contenido del globo de una ruta.
         * @param {object} ruta Datos de la ruta.car
         */
        function strContenidoRuta(ruta) {
            var resul = "";
            resul += itemInfoWindow(textos.salida, ruta.InicioDescripcion + " " + parseJsonDate(ruta.InicioFecha).toLocaleString());
            resul += itemInfoWindow(textos.carreraOficial, ruta.EntradaEnCarreraOficial);
            resul += itemInfoWindow(textos.fin, ruta.FinDescripcion + " " +
                (ruta.FinFecha && ruta.FinFecha.length ? parseJsonDate(ruta.FinFecha).toLocaleString() : ""));
            resul += itemInfoWindow(textos.distanciaRecorrida, .01 * Math.round(.1 * ruta.Distancia).toString().replace(".", ",") + " Km");
            resul += itemInfoWindow(textos.velocidadMedia, Math.round(ruta.Velocidad).toString() + " m/h");
            return resul;
        }

        /** Crea un marcador asociado al mapa.
         * @param {float} latitud Coordenadas del marcador: Latitud.
         * @param {float} longitud Coordenadas del marcador: Longitud.
         * @param {string} titulo Texto emergente del marcador.
         * @param {object} icono Icono para el marcador.
         */
        function crearMarcador(latitud, longitud, titulo, icono, zIndex) {
            var marcador = new g.maps.Marker({
                position: new g.maps.LatLng(latitud, longitud),
                title: titulo,
                icon: icono,
                //optimized: false, // para que funcione el zindex
                zIndex: zIndex ? zIndex : 0
            });

            if (latitud !== null && latitud !== 0 && longitud !== null && longitud !== 0) {
                marcador.setMap(mapa);
            }

            return marcador;
        }

        /** Crea un infoWindow asociado al mapa y registra el evento que cierra cualquier otra
         * infoWindow abierta y abre esta, al hacer click en el marcador asociado.
         * @param {string} contenido Contenido HTML de la infoWindow.
         * @param {float} latitud Coordenadas de la infoWindow: Latitud.
         * @param {float} longitud Coordenadas de la infoWindow: Longitud.
         * @param {int} offsetX Desplazamiento horizontal de la infoWindow.
         * @param {int} offsetY Desplazamiento vertical de la infoWindow.
         * @param {object} marcador Objeto correspondiente al marcador, instancia de la clase 
         * google.maps.Marker, al que se asociará el infoWindow.
         */
        function crearInfoWindow(contenido, latitud, longitud, offsetX, offsetY, marcador) {
            if (marcador) {
                g.maps.event.addListener(marcador, 'click', function () {
                    if (infoWindowAbierta) {
                        infoWindowAbierta.close();
                    }

                    var infoWindow = new g.maps.InfoWindow({
                        content: contenido,
                        position: new g.maps.LatLng(latitud, longitud),
                        'pixelOffset': new g.maps.Size(offsetX, offsetY)
                    });

                    infoWindowAbierta = infoWindow;
                    infoWindow.open(mapa, marcador);
                });
            }
        }

        /** Crea un marcador de hermandad con su infowindow, y lo deja registrado en la lista de 
         * marcadores.
         * @param {object} hermandad Datos de la hermandad.
         */
        function nuevoMarcadorHermandad(hermandad) {
            var marcador = crearMarcador(hermandad.IglesiaLatitud, hermandad.IglesiaLongitud,
                                         hermandad.Nombre, iconos['iglesia.a'], 1);
            /*var infoWindow = */crearInfoWindow(contentInfoWindowHermandad(hermandad),
                hermandad.Latitud, hermandad.Longitud, -53, 0, marcador);
            marcadores["h" + hermandad.HermandadID] = { marcador, hermandad/*, infoWindow */};
            clasificarMarcadorHermandad(hermandad, marcador);
        }

        /** Genera el HTML del globo de una hermandad.
         * @param {object} hermandad Datos de la hermandad.
         */
        function contentInfoWindowHermandad(hermandad) {
            var content = "";
            if (hermandad) {
                content =
                    "<div class='info-window-content'>" +
                        "<div class='col-izquierda'>" +
                            "<img src='/" + hermandad.MiniaturaUrl + "'/>" +
                        "</div>" +
                        "<div class='col-derecha'>" +
                            "<h2>" + hermandad.Nombre + "</h2>" +
                            "<dl>" +
                                itemInfoWindow(textos.iglesiaNombre, hermandad.IglesiaNombre) +
                                itemInfoWindow(textos.iglesiaDireccion, hermandad.IglesiaDireccion) +
                            "</dl>" +
                        "</div>" +
                    "</div>";
            }
            return content;
        };

        /** Crea un marcador de un patrocinador con su infowindow, y lo deja registrado en la lista
         * de marcadores.
         * @param {object} patrocinador Datos del patrocinador.
         */
        function nuevoMarcadorPatrocinador(patrocinador) {
            var marcador = crearMarcador(patrocinador.Latitud, patrocinador.Longitud,
                                         patrocinador.Nombre, iconos['info.b'], 1);
            /*var infoWindow =*/ crearInfoWindow(contentInfoWindowPatrocinador(patrocinador),
                patrocinador.Latitud, patrocinador.Longitud, -30, 0, marcador);
            marcadores["p" + patrocinador.EmpresaID] = { marcador, patrocinador/*, infoWindow*/ };
        }

        /** Genera el HTML del globo de un patrocinador.
         * @param {object} patrocinador Datos del patrocinador.
         */
        function contentInfoWindowPatrocinador(patrocinador) {
            var content = "";
            if(patrocinador) {
                content = 
                    "<div>" +
                        "<div style='float: left; width: 100px; margin-top: 16px;'>" +
                            "<img width='100px' src='/" + patrocinador.LogotipoUrl + "'/>" +
                        "</div>" +
                        "<div style='float: left; padding-left: 10px;'>" +
                            "<h2>" + patrocinador.Nombre + "</h2>" +
                            patrocinador.Descripcion +
                            "<dl>" +
                                itemInfoWindow(textos.patrocinadorDireccion, patrocinador.Direccion) +
                                itemInfoWindow(textos.patrocinadorTelefono, patrocinador.Telefono) +
                                itemInfoWindowLink(textos.patrocinadorWeb, patrocinador.Web) +
                                itemInfoWindowLink(textos.patrocinadorFacebook, patrocinador.Facebook) +
                                itemInfoWindowLink(textos.patrocinadorTwitter, patrocinador.Twitter) +
                            "</dl>" +
                        "</div>" +
                    "</div>";
            }
            return content;
        };

        /** Genera un elemento de la lista de características de los globos de información
         * @param {string} texto Título del elemento
         * @param {string} valor Contenido del elemento
         */
        function itemInfoWindow(titulo, valor) {
            if (valor && valor.trim().length) {
                return "<dt>" + titulo + "</dt>" +
                        "<dd>" + valor + "</dd>";
            } else {
                return "";
            }
        }

        /** Genera un enlace para la lista de características de los globos de información
         * @param {string} texto Texto del enlace
         * @param {string} url Url del enlace
         */
        function itemInfoWindowLink(texto, url) {
            if (url && url.length) {
                return '<p><a target="_blank" href="http://' + url + '">' + texto + '</a><p>';
            } else {
                return "";
            }
        }

        /** Ajusta la posición y el zoom del mapa para mostrar todos los marcadores de la lista de 
         * marcadores.
         */
        function centrarMarcadores() {
            var bounds = new g.maps.LatLngBounds();
            var idsMarcadores = Object.keys(marcadores);
            var marcadoresVisibles = 0;

            idsMarcadores.forEach(function (marcadorId) {
                var infoMarcador = marcadores[marcadorId];
                if (infoMarcador.marcador.map !== null) {
                    marcadoresVisibles += 1;
                    bounds.extend(infoMarcador.marcador.position);
                }
            });

            if (marcadoresVisibles === 0) {
                //console.log("Como no hay marcadores, utilizar las coordenadas por defecto para centrar el mapa.");
                mapa.setCenter(new g.maps.LatLng(opciones.latitud, opciones.longitud))
            } else {
                //console.log("Como hay al menos un marcador, se utiliza bounds.getCenter() para centrar el mapa.");
                mapa.setCenter(bounds.getCenter());
            }

            if (marcadoresVisibles > 1) {
                //console.log("Como más de un marcador, se utiliza fitBounds para establecer el zoom.");
                mapa.fitBounds(bounds);
            } else {
                // console.log("Como hay un marcador o ninguno, utilizamos el zoom por defecto.");
                mapa.setZoom(opciones.zoom);
            }
        }

        /** Obtiene un objeto Date a partir de una fecha en formato json: /Date(...)/
         * @param {string} jsonDate Fecha en formato JSON "/Date(0000000)/ tal como se recibe del servidor."
         */
        function parseJsonDate(jsonDate) {
            // Obtener el número entre paréntesis, convertido en entero.
            var num = jsonDate.match(/\d+/)[0] * 1;
            return new Date(num);
        };

        function cargarSubmenu() {
            //console.log(arguments);

            var ulSubmenu;
            if (arguments.length === 4) {
                textoSubmenu = arguments[0].target.text;
                ulSubmenu = arguments[0].target.nextSibling.childNodes[2];
                tagDiaSeleccionado = arguments[2][0].id;
            } else if (arguments.length === 1) {
                //console.log("Llamada con 1 argumento.", textoBoton, filtroActual, ultimoMenu, ultimoMenu.id);
                if (filtroActual === "TODAS") {
                    // Si el menú se va a abrir en el primer
                    // nivel no hay que generar ningún submenú.
                    return;
                } else {
                    var filtroSplit = filtroActual.split("_");
                    if (filtroSplit.length === 1) {
                        // Es una hermandad. Hay que buscar la el día al que pertenece.

                    } else if (filtroSplit[0] === "TODAS") {
                        // Es un día
                        tagDiaSeleccionado = filtroSplit[1];
                        textoSubmenu = textoBoton;
                    }
                }
                
                ulSubmenu = ultimoMenu[0].childNodes[2];
            } else {
                console.error("Nº de argumentos incorrecto para la función cargarSubmenu.");
            }

            console.log(tagDiaSeleccionado, textoSubmenu, ulSubmenu);

            
            // Quitar los elementos anteriores excepto el de "Todas las hermandades".
            for (var i = ulSubmenu.childNodes.length - 1; i >= 0; i--) {
                var liActual = ulSubmenu.childNodes[i];
                //console.log("Quitar: ", liActual);
                menu.multilevelpushmenu('removeitems', liActual);
            }

            var hermandadesParaSubmenu = hermandadesPorDia[tagDiaSeleccionado];
            var nuevosLi = [];

            // Las hermandades se recorren al revés porque al insertar cada una en el menú se hace
            // por arriba, empujando al resto hacia abajo, es decir, la primera que se inserta
            // quedará abajo del todo y la última arriba del todo.
            if (hermandadesParaSubmenu) {
                var keysHermandades = Object.keys(hermandadesParaSubmenu);                
                for (var i = keysHermandades.length - 1; i >= 0; i--) {
                    var hermandad = hermandadesPorDia[tagDiaSeleccionado][keysHermandades[i]].hermandad;
                    //console.log("Submenú " + tagDiaSeleccionado, "Añadir hermandad: " + hermandad.Nombre);
                    nuevosLi.push({ name: hermandad.Nombre, id: hermandad.Nombre, link: "#" });
                }

                // Añadir elemento "Todas las hermandades"
                nuevosLi.push({ name: textos.hermandadesTodas, id: "TODAS_" + tagDiaSeleccionado, link: "#" });
                //console.log(nuevosLi);
            }
            var submenu = menu.multilevelpushmenu('findmenusbytitle', textoSubmenu);
            menu.multilevelpushmenu('additems', nuevosLi, submenu);
        }

        function filtrarPorGrupo(ev, menuLevelHolder, item) {
            //console.log(ev, menuLevelHolder, item);
            var tag = item[0].id;
            textoBoton = ev.target.text;
            if (textoBoton === textos.hermandadesTodas && menuLevelHolder[0].dataset.level !== "0") {
                textoBoton = menuLevelHolder[0].childNodes[0].childNodes[1].textContent;
            }
            $("#filtroActual").text(textoBoton);
            filtrarPorTag(tag);

            // Obtener el nombre del último menú abierto para que el menú se abra la próxima vez por el mimsmo sitio.
            ultimoMenu = $('#menu').multilevelpushmenu('activemenu');
            menu.multilevelpushmenu('collapse');
        }

        function filtrarPorTag(tag) {
            filtroActual = tag;
            if (tag === "TODAS") {
                tagDiaSeleccionado = "";
                for (var tag in hermandadesPorDia) {
                    var hermandadesDia = hermandadesPorDia[tag];
                    for (var hermandadId in hermandadesDia) {
                        var hermandadActual = hermandadesDia[hermandadId];
                        hermandadActual.elementosMapa.forEach(function (m) {
                            if ((typeof m.setMap) === "function") {
                                m.setMap(mapa);
                            }
                        });
                    }
                }
            } else {
                var tagPartido = tag.split("_");
                if (tagPartido.length === 1) {

                    // Mostrar sólo una hermandad
                    for (var tag in hermandadesPorDia) {
                        var hermandadesDia = hermandadesPorDia[tag];
                        for (var hermandadId in hermandadesDia) {
                            var hermandadActual = hermandadesDia[hermandadId];
                            hermandadActual.elementosMapa.forEach(function (m) {
                                var mostrarMarcador = hermandadActual.hermandad.Nombre === tagPartido[0];
                                //console.log(mostrarMarcador ? "MOSTRAR " : "OCULTAR", hermandadActual.hermandad.Nombre, m);
                                if ((typeof m.setMap) === "function") {
                                    m.setMap(mostrarMarcador ? mapa : null);
                                }
                            });
                        }
                    }
                } else if (tagPartido[0] === "TODAS") {
                    //Ocultar todos los elementos del mapa
                    for (var tag in hermandadesPorDia) {
                        var hermandadesDia = hermandadesPorDia[tag];
                        for (var hermandadId in hermandadesDia) {
                            var hermandadActual = hermandadesDia[hermandadId];
                            hermandadActual.elementosMapa.forEach(function (m) {
                                //console.log(mostrarMarcadores ? "MOSTRAR " : "OCULTAR", hermandadActual.hermandad.Nombre, m);
                                if ((typeof m.setMap) === "function") {
                                    m.setMap(null);
                                }
                            });
                        }
                    }

                    // Mostrar los elementos del mapa correspondientes a las hermandades que tengan
                    // el tag seleccionado.
                    tagDiaSelecionado = tagPartido[1];
                    var hermandadesDiaMostrar = hermandadesPorDia[tagDiaSelecionado];
                    if (hermandadesDiaMostrar) {
                        for (var hermandadId in hermandadesDiaMostrar) {
                            var hermandadActual = hermandadesDiaMostrar[hermandadId];
                            hermandadActual.elementosMapa.forEach(function (m) {
                                if ((typeof m.setMap) === "function") {
                                    m.setMap(mapa);
                                }
                            });
                        }
                    }
                }
            }
        }

        // Primera carga de datos
        cargarDatos();

        // recargar cada X minutos.
        setInterval(cargarDatos, opciones.refresco * 60000);

        function getParameterByName(name, url) {
            if (!url) {
                url = window.location.href;
            }
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }
    });
})(jQuery, google);