namespace PushNews.Dominio.Migrations
{
    using PushNews.Dominio;
    using PushNews.Dominio.Entidades;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    internal sealed class Configuration : DbMigrationsConfiguration<PushNewsModel>
    {
        private Random rnd = new Random(DateTime.Now.Millisecond);

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PushNewsModel context)
        {
#if DEBUG
            // Inserción de datos de prueba
            //SeedPrueba(context);
#else
            // Inserción de los datos de producción.
            //SeedProduccion(context);            
            //SeedPrueba(context);
#endif
        }

        /// <summary>
        /// Vacía una tabla y resetea la identidad si fuera necesario
        /// </summary>
        /// <param name="nombreTabla"></param>
        /// <param name="identidad"></param>
        /// <param name="context"></param>
        private void LimpiarTabla(string nombreTabla, bool identidad, PushNewsModel context)
        {
            context.ExecuteCommand($"DELETE FROM [{nombreTabla}]");
            if (identidad)
            {
                context.ExecuteCommand($"DBCC CHECKIDENT ('{nombreTabla}', RESEED, 0)");
            }
        }

        /// <summary>
        /// Vacía de registros toda la base de datos, resetea los índices autonuméricos e inserta el conjunto
        /// de datos de prueba.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        void SeedPrueba(PushNewsModel context)
        {
            try
            {
                context.ExecuteCommand("UPDATE Aplicaciones SET LogotipoID = NULL");

                // tablas es una lista de los nombres de las tablas de la base de datos con un booleano que 
                // indica si la tabla tiene o no identidad.
                Tuple<string, bool>[] tablas =
                {
                    #region Lista de tablas de la base de datos
                    new Tuple<string, bool>("Empresas", context.Empresas.Any()),
                    new Tuple<string, bool>("AplicacionesAplicacionesAmigas", false),
                    new Tuple<string, bool>("ComunicacionesAccesos", context.Accesos.Any()),
                    new Tuple<string, bool>("Comunicaciones", context.Comunicaciones.Any()),
                    new Tuple<string, bool>("UsuariosPerfiles", false),
                    new Tuple<string, bool>("PerfilesRoles", false),
                    new Tuple<string, bool>("Roles", context.Roles.Any()),
                    new Tuple<string, bool>("Perfiles", context.Perfiles.Any()),
                    new Tuple<string, bool>("Terminales", context.Terminales.Any()),
                    new Tuple<string, bool>("UsuariosCategorias", false),
                    new Tuple<string, bool>("Categorias", context.Categorias.Any()),
                    new Tuple<string, bool>("AplicacionesUsuarios", false),
                    new Tuple<string, bool>("Parametros", context.Parametros.Any()),
                    new Tuple<string, bool>("Usuarios", context.Usuarios.Any()),
                    new Tuple<string, bool>("Documentos", context.Documentos.Any()),
                    new Tuple<string, bool>("Telefonos", context.Telefonos.Any()),
                    new Tuple<string, bool>("Localizaciones", context.Localizaciones.Any()),
                    new Tuple<string, bool>("AplicacionesAplicacionesCaracteristicas", false),
                    new Tuple<string, bool>("AplicacionesCaracteristicas", false),
                    new Tuple<string, bool>("Aplicaciones", context.Aplicaciones.Any()),
                #endregion
                };
                foreach (Tuple<string, bool> tabla in tablas)
                {
                    LimpiarTabla(tabla.Item1, tabla.Item2, context);
                }
                context.SaveChanges();
                var roles = Roles();
                var perfiles = Perfiles(roles);
                var caracteristicas = Caracteristicas();
                var aplicaciones = Aplicaciones(caracteristicas);
                var usuarios = Usuarios(perfiles, aplicaciones);
                var categorias = Categorias(usuarios);
                var comunicaciones = Comunicaciones(usuarios, categorias);
                var parametros = Parametros();
                var telefonos = Telefonos(aplicaciones);
                var localizaciones = Localizaciones(aplicaciones);

                context.Roles.AddRange(roles);
                context.Perfiles.AddRange(perfiles);
                context.AplicacionesCaracteristicas.AddRange(caracteristicas);
                context.Aplicaciones.AddRange(aplicaciones);
                context.Usuarios.AddRange(usuarios);
                context.Categorias.AddRange(categorias);
                context.Comunicaciones.AddRange(comunicaciones);
                context.Parametros.AddRange(parametros);
                context.Telefonos.AddRange(telefonos);
                context.Localizaciones.AddRange(localizaciones);

                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                ExtraerErrores(ex);
            }
        }

        void SeedProduccion(PushNewsModel context)
        { }

        private IEnumerable<Telefono> Telefonos(IEnumerable<Aplicacion> aplicaciones)
        {
            // Aplicaciones[0]: Ayuntamiento de Santaella
            // Aplicaciones[1]: Mancomunidad Campiña Sur

            return new List<Telefono>
            {
                new Telefono
                {
                    Aplicacion = aplicaciones.ElementAt(0),
                    Descripcion = "Ayuntamiento",
                    Numero = "957313003",
                    Activo = true,
                    Fecha = DateTime.Now
                },
                new Telefono
                {
                    Aplicacion = aplicaciones.ElementAt(0),
                    Descripcion = "CONSULTORIO MÉDICO",
                    Numero = "957307501",
                    Activo = true,
                    Fecha = DateTime.Now
                },
                new Telefono
                {
                    Aplicacion = aplicaciones.ElementAt(0),
                    Descripcion = "FARMACIA RIDER",
                    Numero = "957313057",
                    Activo = true,
                    Fecha = DateTime.Now
                },
                new Telefono
                {
                    Aplicacion = aplicaciones.ElementAt(0),
                    Descripcion = "FARMACIA RUEDA",
                    Numero = "957313134",
                    Activo = true,
                    Fecha = DateTime.Now
                },
                new Telefono
                {
                    Aplicacion = aplicaciones.ElementAt(0),
                    Descripcion = "GUARDIA CIVIL",
                    Numero = "957313010",
                    Activo = true,
                    Fecha = DateTime.Now
                },
                new Telefono
                {
                    Aplicacion = aplicaciones.ElementAt(0),
                    Descripcion = "POLICIA LOCAL",
                    Numero = "957313125",
                    Activo = true,
                    Fecha = DateTime.Now
                },
                new Telefono
                {
                    Aplicacion = aplicaciones.ElementAt(0),
                    Descripcion = "PABELLÓN CUBIERTO",
                    Numero = "957313534",
                    Activo = true,
                    Fecha = DateTime.Now
                },
                new Telefono
                {
                    Aplicacion = aplicaciones.ElementAt(1),
                    Descripcion = "Teléfono 1",
                    Numero = "957662090",
                    Activo = true,
                    Fecha = DateTime.Now
                },
                new Telefono
                {
                    Aplicacion = aplicaciones.ElementAt(1),
                    Descripcion = "Teléfono 2",
                    Numero = "957662091",
                    Activo = true,
                    Fecha = DateTime.Now
                }
            };

        }

        private IEnumerable<Localizacion> Localizaciones(IEnumerable<Aplicacion> aplicaciones)
        {
            // Aplicaciones[0]: Ayuntamiento de Santaella
            // Aplicaciones[1]: Mancomunidad Campiña Sur

            return new List<Localizacion>
            {
                new Localizacion
                {
                    Aplicacion = aplicaciones.ElementAt(0),
                    Descripcion = "Ayuntamiento",
                    Latitud = 37.566313,
                    Longitud = -4.845434,
                    Fecha = DateTime.Now,
                    Activo = true
                },
                new Localizacion
                {
                    Aplicacion = aplicaciones.ElementAt(0),
                    Descripcion = "Cuartel de la Guardia Civil",
                    Latitud = 37.564093,
                    Longitud = -4.840436,
                    Fecha = DateTime.Now,
                    Activo = true
                },
                new Localizacion
                {
                    Aplicacion = aplicaciones.ElementAt(0),
                    Descripcion = "Jefatura de Policía Local",
                    Latitud = 37.560615,
                    Longitud = -4.843751,
                    Fecha = DateTime.Now,
                    Activo = true
                },
                new Localizacion
                {
                    Aplicacion = aplicaciones.ElementAt(1),
                    Descripcion = "Sede de la Mancomunidad",
                    Latitud = 37.520505,
                    Longitud = -4.656619,
                    Fecha = DateTime.Now,
                    Activo = true
                }
            };
        }

        private IEnumerable<Parametro> Parametros()
        {
            return new List<Parametro>
            {
                new Parametro
                {
                    Nombre = "SubdominioDepuracion",
                    Valor = "santaella",
                    Descripcion = "Subdominio para depuración"
                },
                new Parametro
                {
                    Nombre = "SubdominioGenerico",
                    Valor = "app",
                    Descripcion = "Subdominio para depuración"
                },
                new Parametro
                {
                    Nombre = "RaizDocumentos",
                    Valor = @"C:\PushNews\documentos",
                    Descripcion = "Raíz de documentos de PushNews"
                },
                //new Parametro
                //{
                //    Nombre = "AzureBlobContainer",
                //    Valor = "pushnews",
                //    Descripcion = "Nombre del contenedor del servicio de blobstorage para los documentos."
                //},
                //new Parametro
                //{
                //    Nombre = "AzureBlobAccountName",
                //    Valor = "pushnews",
                //    Descripcion = "Nombre de cuenta de acceso al blob storage de Azure"
                //},
                //new Parametro
                //{
                //    Nombre = "AzureBlobAccountKey",
                //    Valor = "Poner la clave cuando se cree la cuenta de blob storage",
                //    Descripcion = "Clave de acceso al blob storage de Azure"
                //}
            };
        }

        private IEnumerable<Rol> Roles()
        {
            return new List<Rol>
            {
                new Rol() { Nombre = "LeerComunicaciones", Modulo = "Comunicaciones" },
                new Rol() { Nombre = "CrearComunicaciones", Modulo = "Comunicaciones" },
                new Rol() { Nombre = "ModificarComunicaciones", Modulo = "Comunicaciones" },
                new Rol() { Nombre = "EliminarComunicaciones", Modulo = "Comunicaciones" },
                new Rol() { Nombre = "LeerCategorias", Modulo = "Categorias" },
                new Rol() { Nombre = "CrearCategorias", Modulo = "Categorias" },
                new Rol() { Nombre = "ModificarCategorias", Modulo = "Categorias" },
                new Rol() { Nombre = "EliminarCategorias", Modulo = "Categorias" },
                new Rol() { Nombre = "LeerAplicaciones", Modulo = "Aplicaciones" },
                new Rol() { Nombre = "CrearAplicaciones", Modulo = "Aplicaciones" },
                new Rol() { Nombre = "ModificarAplicaciones", Modulo = "Aplicaciones" },
                new Rol() { Nombre = "EliminarAplicaciones", Modulo = "Aplicaciones" },
                new Rol() { Nombre = "LeerTerminales", Modulo = "Terminales" },
                new Rol() { Nombre = "CrearTerminales", Modulo = "Terminales" },
                new Rol() { Nombre = "ModificarTerminales", Modulo = "Terminales" },
                new Rol() { Nombre = "EliminarTerminales", Modulo = "Terminales" },
                new Rol() { Nombre = "LeerAccesos", Modulo = "Accesos" },
                new Rol() { Nombre = "CrearAccesos", Modulo = "Accesos" },
                new Rol() { Nombre = "ModificarAccesos", Modulo = "Accesos" },
                new Rol() { Nombre = "EliminarAccesos", Modulo = "Accesos" },
                new Rol() { Nombre = "LeerParametros", Modulo = "Parametros" },
                new Rol() { Nombre = "CrearParametros", Modulo = "Parametros" },
                new Rol() { Nombre = "ModificarParametros", Modulo = "Parametros" },
                new Rol() { Nombre = "EliminarParametros", Modulo = "Parametros" },
                new Rol() { Nombre = "LeerUsuarios", Modulo = "Usuarios" },
                new Rol() { Nombre = "CrearUsuarios", Modulo = "Usuarios" },
                new Rol() { Nombre = "ModificarUsuarios", Modulo = "Usuarios" },
                new Rol() { Nombre = "EliminarUsuarios", Modulo = "Usuarios" },
                new Rol() { Nombre = "LeerPerfiles", Modulo = "Perfiles" },
                new Rol() { Nombre = "CrearPerfiles", Modulo = "Perfiles" },
                new Rol() { Nombre = "ModificarPerfiles", Modulo = "Perfiles" },
                new Rol() { Nombre = "EliminarPerfiles", Modulo = "Perfiles" },
                new Rol() { Nombre = "LeerTelefonos", Modulo = "Telefonos" },
                new Rol() { Nombre = "CrearTelefonos", Modulo = "Telefonos" },
                new Rol() { Nombre = "ModificarTelefonos", Modulo = "Telefonos" },
                new Rol() { Nombre = "EliminarTelefonos", Modulo = "Telefonos" },
                new Rol() { Nombre = "LeerLocalizaciones", Modulo = "Localizaciones" },
                new Rol() { Nombre = "CrearLocalizaciones", Modulo = "Localizaciones" },
                new Rol() { Nombre = "ModificarLocalizaciones", Modulo = "Localizaciones" },
                new Rol() { Nombre = "EliminarLocalizaciones", Modulo = "Localizaciones" },
                new Rol() { Nombre = "LeerAplicacionesCaracteristicas", Modulo = "AplicacionesCaracteristicas" },
                //new Rol() { Nombre = "CrearAplicacionesCaracteristicas", Modulo = "AplicacionesCaracteristicas" },
                //new Rol() { Nombre = "ModificarAplicacionesCaracteristicas", Modulo = "AplicacionesCaracteristicas" },
                //new Rol() { Nombre = "EliminarAplicacionesCaracteristicas", Modulo = "AplicacionesCaracteristicas" },
                new Rol() { Nombre = "LeerAsociados", Modulo = "Asociados" },
                new Rol() { Nombre = "CrearAsociados", Modulo = "Asociados" },
                new Rol() { Nombre = "ModificarAsociados", Modulo = "Asociados" },
                new Rol() { Nombre = "EliminarAsociados", Modulo = "Asociados" },
                new Rol() { Nombre = "LeerInfoPush", Modulo = "Comunicaciones" },
                new Rol() { Nombre = "LeerEmpresas", Modulo = "Empresas" },
                new Rol() { Nombre = "CrearEmpresas", Modulo = "Empresas" },
                new Rol() { Nombre = "ModificarEmpresas", Modulo = "Empresas" },
                new Rol() { Nombre = "EliminarEmpresas", Modulo = "Empresas" },
                new Rol() { Nombre = "LeerHermandades", Modulo = "Hermandades" },
                new Rol() { Nombre = "CrearHermandades", Modulo = "Hermandades" },
                new Rol() { Nombre = "ModificarHermandades", Modulo = "Hermandades" },
                new Rol() { Nombre = "EliminarHermandades", Modulo = "Hermandades" },
                new Rol() { Nombre = "LeerGpss", Modulo = "Gpss" },
                new Rol() { Nombre = "CrearGpss", Modulo = "Gpss" },
                new Rol() { Nombre = "ModificarGpss", Modulo = "Gpss" },
                new Rol() { Nombre = "EliminarGpss", Modulo = "Gpss" },
                new Rol() { Nombre = "LeerRutas", Modulo = "Rutas" },
                new Rol() { Nombre = "CrearRutas", Modulo = "Rutas" },
                new Rol() { Nombre = "ModificarRutas", Modulo = "Rutas" },
                new Rol() { Nombre = "EliminarRutas", Modulo = "Rutas" }
            };
        }

        private IEnumerable<Perfil> Perfiles(IEnumerable<Rol> roles)
        {
            List<Rol> rolesEditor = roles
                .Where(r => r.Modulo == "Categorias" || r.Modulo == "Comunicaciones"
                         || r.Modulo == "Telefonos" || r.Modulo == "Localizaciones"
                         || r.Modulo == "Empresas" || r.Modulo == "Hermandades")
                .ToList();

            return new List<Perfil>
            {
                new Perfil { Nombre = "Administrador", Activo = true, Roles = roles.ToList() },
                new Perfil { Nombre = "Editor", Activo = true, Roles = rolesEditor },
            };
        }

        private IEnumerable<AplicacionCaracteristica> Caracteristicas()
        {
            return ((IEnumerable<PushNews.Dominio.Enums.AplicacionCaracteristica>)Enum.GetValues(typeof(PushNews.Dominio.Enums.AplicacionCaracteristica)))
                .Select(c => new AplicacionCaracteristica
                {
                    AplicacionCaracteristicaID = (long) c,
                    Nombre = Enum.GetName(typeof(PushNews.Dominio.Enums.AplicacionCaracteristica), c),
                    Activo = true
                })
                .ToList();
        }

        private IEnumerable<Aplicacion> Aplicaciones(IEnumerable<AplicacionCaracteristica> caracteristicas)
        {
            Aplicacion ap1 = new Aplicacion
            {
                Nombre = "Ayuntamiento de Santaella",
                Version = "1.0.0.0",
                Activo = true,
                SubDominio = "santaella",
                Caracteristicas = caracteristicas.ToList()
            };
            Aplicacion ap2 = new Aplicacion
            {
                Nombre = "Mancomunidad Campiña Sur Cordobesa",
                Version = "1.0.0.0",
                Activo = true,
                SubDominio = "campinasur",
                Caracteristicas = caracteristicas.ToList()
            };
            return new List<Aplicacion> { ap1, ap2 };
        }

        private IEnumerable<Usuario> Usuarios(IEnumerable<Perfil> perfiles, IEnumerable<Aplicacion> aplicaciones)
        {
            return new List<Usuario>
            {
                new Usuario()
                {
                    Email = "administrador@pushnews.com",
                    Clave = @"AHkDIVe+IWL4OkmhKrsS68f4iRlnlVcnKdv2Z8CxioCkz8WBpTBXowsGchdqDk/qJQ==",
                    MarcaSeguridad = @"26796fd7-dd47-4bd3-82e7-979253ec386a",
                    Activo = true,
                    Creado = DateTime.Now,
                    Actualizado = DateTime.Now,
                    Perfiles = new List<Perfil>() { perfiles.SingleOrDefault(p => p.Nombre == "Administrador") },
                    Aplicaciones = aplicaciones.ToList(),
                    Nombre = "Fulano",
                    Apellidos = "de Copas"
                },
                new Usuario()
                {
                    Email = "editor1@pushnews.com",
                    Clave = @"AHkDIVe+IWL4OkmhKrsS68f4iRlnlVcnKdv2Z8CxioCkz8WBpTBXowsGchdqDk/qJQ==",
                    MarcaSeguridad = @"26796fd7-dd47-4bd3-82e7-979253ec386a",
                    Activo = true,
                    Creado = DateTime.Now,
                    Actualizado = DateTime.Now,
                    Perfiles = new List<Perfil>() { perfiles.SingleOrDefault(p => p.Nombre == "Editor") },
                    Aplicaciones = aplicaciones.Take(1).ToList(),
                    Nombre = "Mengano",
                    Apellidos = "de Oros"
                },
                new Usuario()
                {
                    Email = "editor2@pushnews.com",
                    Clave = @"AHkDIVe+IWL4OkmhKrsS68f4iRlnlVcnKdv2Z8CxioCkz8WBpTBXowsGchdqDk/qJQ==",
                    MarcaSeguridad = @"26796fd7-dd47-4bd3-82e7-979253ec386a",
                    Activo = true,
                    Creado = DateTime.Now,
                    Actualizado = DateTime.Now,
                    Perfiles = new List<Perfil>() { perfiles.SingleOrDefault(p => p.Nombre == "Editor") },
                    Aplicaciones = aplicaciones.ToList(),
                    Nombre = "Zutano",
                    Apellidos = "de Espadas"
                }
            };
        }

        private IEnumerable<Categoria> Categorias(IEnumerable<Usuario> editores)
        {
            Usuario editor1 = editores.Single(u => u.Email == "editor1@pushnews.com");
            Usuario editor2 = editores.Single(u => u.Email == "editor2@pushnews.com");

            return new List<Categoria>
            {
                new Categoria
                {
                    Creador = editor1,
                    Aplicacion = editor1.Aplicaciones.First(),
                    Nombre = "Eventos",
                    Orden = 3,
                    Activo = true,
                    Icono = Iconos[rnd.Next(0, Iconos.Length-1)]
                },
                new Categoria
                {
                    Creador = editor1,
                    Aplicacion = editor1.Aplicaciones.First(),
                    Nombre = "Noticias",
                    Orden = 2,
                    Activo = true,
                    Icono = Iconos[rnd.Next(0, Iconos.Length-1)]
                },
                new Categoria
                {
                    Creador = editor1,
                    Aplicacion = editor1.Aplicaciones.First(),
                    Nombre = "Oficial",
                    Orden = 1,
                    Activo = true,
                    Icono = Iconos[rnd.Next(0, Iconos.Length-1)]
                },
                new Categoria
                {
                    Creador = editor2,
                    Aplicacion = editor2.Aplicaciones.Last(),
                    Nombre = "Sucesos",
                    Orden = 3,
                    Activo = true,
                    Icono = Iconos[rnd.Next(0, Iconos.Length-1)]
                },
                new Categoria
                {
                    Creador = editor2,
                    Aplicacion = editor2.Aplicaciones.Last(),
                    Nombre = "Ultima hora",
                    Orden = 2,
                    Activo = true,
                    Icono = Iconos[rnd.Next(0, Iconos.Length-1)]
                },
                new Categoria
                {
                    Creador = editor2,
                    Aplicacion = editor2.Aplicaciones.Last(),
                    Nombre = "Comunicados",
                    Orden = 1,
                    Activo = true,
                    Icono = Iconos[rnd.Next(0, Iconos.Length-1)]
                }
            };
        }

        private IEnumerable<Comunicacion> Comunicaciones(IEnumerable<Usuario> editores, IEnumerable<Categoria> categorias)
        {
            Usuario editor1 = editores.Skip(1).Take(1).ElementAt(0);
            List<Categoria> categorias1 = categorias.Take(3).ToList();
            int numCategorias1 = categorias1.Count();
            Usuario editor2 = editores.Skip(2).Take(1).ElementAt(0);
            List<Categoria> categorias2 = categorias.Skip(3).Take(3).ToList();
            int numCategorias2 = categorias1.Count();

            // Respetar mala indentación para evitar espacios indeseados en las RAW STRINGS.
            return new List<Comunicacion>
{
    new Comunicacion
    {
        Usuario = editor2,
        Autor = "Carol Nicholson Gonzalez",
        Titulo = "Eu nostrud pariatur ad consequat minim.",
        Descripcion = @"Occaecat sint reprehenderit mollit officia magna est Lorem occaecat cupidatat nostrud. Eiusmod sint ea laboris esse ut do pariatur amet. Laborum sit minim sit eiusmod excepteur labore deserunt ea qui aliqua occaecat ea quis fugiat. Ad est sint aliquip laboris do laboris deserunt pariatur ut. Excepteur sint nulla pariatur adipisicing aliqua. Elit sit voluptate adipisicing magna anim ipsum aute duis quis duis aute exercitation.Anim fugiat fugiat aliqua eiusmod elit excepteur irure aliquip cillum amet ullamco irure deserunt amet. Mollit sunt labore proident mollit do ex sint tempor sint aliqua. Id cillum et eiusmod non. Irure amet tempor nisi ex esse est dolore. Deserunt enim elit reprehenderit nisi id irure exercitation. Nisi voluptate velit ad officia qui laboris qui amet ipsum anim incididunt magna anim qui. Dolor labore fugiat non et est.
Elit est ut fugiat nulla enim qui est nulla aute. Laboris sit exercitation incididunt veniam. Deserunt incididunt nostrud cillum incididunt elit qui consectetur minim enim. Excepteur enim Lorem ea ea fugiat duis enim enim aliquip.",
        FechaPublicacion = new DateTime(2016, 01, 26, 10, 56, 24),
        FechaCreacion = new DateTime(2016, 01, 26, 10, 56, 24),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion
    {
        Usuario = editor2,
        Autor = "King Benjamin Hughes",
        Titulo = "Ea officia cupidatat mollit occaecat aliqua ea.",
        Descripcion = @"Voluptate elit eu eiusmod proident nulla anim aliqua incididunt irure magna velit. Magna consequat do exercitation veniam duis elit dolor eu occaecat. Consequat ex est ex nisi elit enim ullamco irure duis amet elit. Ad deserunt anim ipsum fugiat aliqua. Voluptate id laborum consequat consequat exercitation amet reprehenderit nulla laborum ipsum. Esse in proident duis occaecat cillum ex consequat commodo aliqua. Non esse sit ad mollit laboris aliqua ex ex et eiusmod enim sit nisi.Eiusmod nisi officia sint culpa laborum non cupidatat proident adipisicing tempor enim. Esse magna deserunt fugiat duis. Nostrud excepteur veniam ullamco esse fugiat amet adipisicing exercitation sit est deserunt irure adipisicing. Nisi in incididunt incididunt ipsum do.
Officia ex anim fugiat do cillum. Id non non ullamco ut incididunt. Sunt irure dolore minim duis dolor esse excepteur ut laborum do irure occaecat exercitation sunt.",
        FechaPublicacion = new DateTime(2016, 01, 08, 01, 20, 58),
        FechaCreacion = new DateTime(2016, 01, 08, 01, 20, 58),
        CategoriaID = 6,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Kristina Shaffer Rios",
        Titulo = "Excepteur aliqua et exercitation dolor.",
        Descripcion = @"Veniam minim proident tempor irure et eiusmod in. Sint ut ipsum ullamco dolore aute eiusmod minim quis mollit amet nulla. Labore ullamco sunt ullamco est nulla excepteur consequat non sunt labore ut. Minim non mollit enim elit ex. Exercitation reprehenderit Lorem velit deserunt cillum ut cillum voluptate amet enim deserunt sit id. Ut reprehenderit nisi sit sit sit excepteur. Pariatur ea ea minim veniam aliquip.Consectetur ipsum irure consectetur veniam tempor deserunt quis aliqua eu ad magna ea eu mollit. Fugiat sint magna voluptate eu veniam ut nostrud adipisicing quis aliquip quis. Id incididunt incididunt veniam consectetur in nostrud nisi ullamco adipisicing in sit elit. Non aute amet exercitation fugiat ullamco. Non consequat sunt dolore esse excepteur consectetur excepteur ex nostrud reprehenderit id anim non. Velit duis labore elit pariatur.
Anim voluptate nulla duis veniam ea culpa proident officia mollit est. Veniam ex laborum commodo officia ex consequat ullamco est elit cillum. Laboris aliqua culpa voluptate ut. Nisi eiusmod ad elit sit ullamco veniam minim enim magna pariatur labore non. Do eu sint ad ut commodo culpa commodo exercitation proident sunt anim. Eiusmod enim Lorem non nulla laborum amet anim eiusmod sunt aliquip nulla aute et pariatur.",
        FechaPublicacion = new DateTime(2016, 01, 13, 09, 36, 13),
        FechaCreacion = new DateTime(2016, 01, 13, 09, 36, 13),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Perry Colon Kaufman",
        Titulo = "Pariatur in in ullamco ipsum culpa cillum aliqua commodo ex.",
        Descripcion = @"Aliquip reprehenderit id velit ex mollit irure id incididunt consectetur proident commodo amet sunt. Sunt id mollit ipsum qui laborum culpa magna dolor aliquip incididunt laboris qui cillum. Ad esse non mollit officia occaecat sit aliquip excepteur.Non eu exercitation eiusmod aliquip voluptate exercitation voluptate elit enim ex velit fugiat sint. Et culpa ad dolor nulla tempor ut in cillum ullamco enim nostrud cupidatat do. Deserunt labore magna fugiat consequat do qui id.
Culpa aliquip qui enim elit est. Ad Lorem consectetur amet cillum et consequat elit Lorem adipisicing. Non culpa voluptate non ad eiusmod nulla anim. Ea commodo magna veniam velit qui proident reprehenderit labore ipsum ut ex. Irure ex exercitation dolore amet est et labore mollit pariatur adipisicing velit ad laboris.",
        FechaPublicacion = new DateTime(2016, 01, 16, 02, 15, 03),
        FechaCreacion = new DateTime(2016, 01, 16, 02, 15, 03),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Gomez Sherman Morgan",
        Titulo = "Adipisicing ea ex consectetur reprehenderit veniam officia ex sunt ipsum.",
        Descripcion = @"Qui ad adipisicing eu veniam. Minim minim laborum dolor mollit nisi eiusmod amet id. Eiusmod irure consequat ea elit mollit et tempor sit reprehenderit pariatur excepteur nulla reprehenderit cupidatat. Nostrud labore quis consectetur Lorem proident aute officia duis nostrud ullamco. Dolor non in mollit qui aliqua cillum. Lorem dolore pariatur ipsum quis adipisicing. Ullamco laborum velit laborum duis quis laboris incididunt labore tempor nisi.Irure consequat ullamco ullamco minim quis tempor quis dolore ex cillum adipisicing duis non fugiat. Enim culpa aute excepteur eu sunt fugiat et amet Lorem. Consectetur eu veniam veniam duis cillum. Velit esse incididunt aliquip et.
Deserunt enim aliquip qui dolore laboris sit anim ea duis dolor. Occaecat mollit dolore incididunt dolor est tempor cupidatat ad reprehenderit anim incididunt laboris non do. Do excepteur et est id adipisicing laboris magna culpa proident officia irure aute. Commodo enim incididunt exercitation eu ut esse elit officia quis sunt adipisicing sint. Non anim magna culpa nisi veniam ut fugiat consectetur incididunt tempor. Occaecat Lorem proident anim mollit reprehenderit aute commodo sit ad esse irure dolore.",
        FechaPublicacion = new DateTime(2016, 01, 23, 03, 11, 07),
        FechaCreacion = new DateTime(2016, 01, 23, 03, 11, 07),
        CategoriaID = 4,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Dickerson Hanson Warner",
        Titulo = "Consequat dolor excepteur consequat adipisicing in occaecat non do tempor.",
        Descripcion = @"Esse laborum ex voluptate veniam irure laboris elit ut non culpa duis. Enim anim veniam ad excepteur eu esse tempor. Occaecat sunt est officia nisi ea qui sunt sunt dolor Lorem duis ea officia magna. Dolore eiusmod eiusmod enim reprehenderit ea culpa aliqua. Ad nulla voluptate sint aliqua incididunt excepteur mollit pariatur officia. Laborum labore proident sint culpa. Reprehenderit consectetur et reprehenderit non aliqua reprehenderit voluptate occaecat incididunt tempor.Sunt pariatur laboris cillum do do magna cupidatat velit commodo excepteur do mollit esse incididunt. Occaecat velit magna adipisicing nulla commodo deserunt est cupidatat laborum consectetur. Consequat officia nisi qui non laboris reprehenderit commodo eiusmod tempor labore ex duis qui. Incididunt in amet irure ipsum veniam deserunt. Veniam consectetur laboris qui Lorem fugiat deserunt nostrud.
Tempor dolor incididunt ut incididunt elit officia adipisicing laborum pariatur cillum. Adipisicing in et occaecat id. Lorem et nostrud reprehenderit id consectetur excepteur qui nisi qui. Sunt irure excepteur cupidatat veniam esse adipisicing id aute culpa Lorem. Incididunt ad est et tempor laborum duis nulla nostrud ut.",
        FechaPublicacion = new DateTime(2016, 01, 22, 02, 25, 12),
        FechaCreacion = new DateTime(2016, 01, 22, 02, 25, 12),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Delaney Mcgowan Pennington",
        Titulo = "Laborum veniam commodo dolor ut proident elit anim consequat nisi ea non irure id adipisicing.",
        Descripcion = @"Occaecat proident commodo eiusmod in ut do velit. Consectetur tempor dolor magna eiusmod. Ipsum ipsum non proident ad fugiat reprehenderit elit amet laboris anim incididunt qui consectetur. Aliqua id consequat magna nisi voluptate proident veniam sit ullamco. Aliqua cillum non irure minim eu eiusmod eiusmod sint officia non consequat culpa qui. Ut consequat magna est duis deserunt mollit reprehenderit ea in.Commodo duis sit labore laboris velit eu nostrud eiusmod mollit cupidatat officia laborum excepteur non. Nulla et irure deserunt culpa tempor eiusmod ullamco id. Ea ut ex in dolore sint culpa do laboris. Mollit aliquip qui irure id aute duis voluptate qui sint esse magna. Nostrud incididunt minim culpa cillum consequat exercitation adipisicing amet et dolor veniam laboris aute. Consequat ut adipisicing anim ipsum laborum nisi proident pariatur culpa. Nostrud voluptate aliqua fugiat occaecat Lorem.
Non est enim eu officia sit dolor minim aute magna consequat. Laborum ex fugiat magna dolore nulla eiusmod mollit aute enim esse. Aliquip elit mollit nostrud excepteur. Labore adipisicing velit ut est. Aute cillum aliqua sunt aliquip et ipsum magna.",
        FechaPublicacion = new DateTime(2016, 01, 02, 05, 46, 56),
        FechaCreacion = new DateTime(2016, 01, 02, 05, 46, 56),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Macias Pittman Wilkinson",
        Titulo = "Elit ut consequat sunt minim tempor commodo excepteur ad occaecat in laboris est quis.",
        Descripcion = @"Sunt ipsum anim labore adipisicing consectetur eiusmod. Esse veniam occaecat qui excepteur qui amet reprehenderit aute amet. Pariatur sunt reprehenderit anim enim qui et dolore Lorem. Dolore aliquip est aliquip excepteur irure excepteur eiusmod.Minim nulla est nulla et in mollit laboris labore magna laboris officia. Et in duis aute est sint elit sunt id. Reprehenderit nisi reprehenderit sit voluptate velit officia quis consectetur in. Duis sit velit ut consectetur. Ad quis eu occaecat qui. Cillum officia laboris irure voluptate duis ex reprehenderit ex aliqua.
Duis labore irure duis excepteur magna elit occaecat aliquip. Cupidatat do dolor esse non elit commodo cillum cillum occaecat. Sunt adipisicing cupidatat sit Lorem culpa. Ullamco dolor mollit quis cillum minim cupidatat aliquip sit ullamco. Irure Lorem nisi minim do.",
        FechaPublicacion = new DateTime(2016, 01, 05, 10, 36, 24),
        FechaCreacion = new DateTime(2016, 01, 05, 10, 36, 24),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
        },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Vickie Salas Sanchez",
        Titulo = "Reprehenderit consequat enim aliqua eiusmod exercitation ad minim laborum commodo sit.",
        Descripcion = @"Proident commodo nostrud ullamco est Lorem dolore proident veniam amet laboris. Laborum sint eu fugiat exercitation officia elit. Exercitation exercitation anim eu sint non consectetur magna laboris ullamco ex exercitation pariatur.Reprehenderit dolor et anim magna. Qui ut excepteur aliqua magna cupidatat dolore in aliqua Lorem. Reprehenderit exercitation minim adipisicing fugiat reprehenderit laboris minim. Deserunt anim occaecat ut excepteur labore veniam magna eu consequat.
Minim ex adipisicing tempor exercitation occaecat dolore veniam laboris qui aliquip cupidatat aliquip duis. Deserunt do quis cupidatat nostrud. Quis aliqua aliquip ex laborum sunt excepteur in. Ea nostrud Lorem id ad reprehenderit sunt excepteur laborum occaecat.",
        FechaPublicacion = new DateTime(2016, 01, 12, 02, 00, 20),
        FechaCreacion = new DateTime(2016, 01, 12, 02, 00, 20),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Alison Solis Pena",
        Titulo = "Ipsum exercitation fugiat laboris labore.",
        Descripcion = @"Sit proident occaecat esse commodo velit pariatur ex occaecat incididunt nulla magna mollit sunt. Amet cillum velit eu voluptate commodo deserunt veniam fugiat nulla magna sint dolor cupidatat. Aute consequat mollit excepteur in ad id mollit do. Cillum nisi adipisicing aliqua minim eu nostrud sit laboris eu ea ex dolore non proident.Ullamco tempor duis quis labore nostrud. Est sunt Lorem eiusmod Lorem enim ut fugiat velit. Sint dolor mollit duis esse exercitation irure sint officia voluptate. Id dolore adipisicing quis in. Consequat cupidatat nulla ut ea non proident minim culpa in. Aute aliquip occaecat ut do esse irure incididunt.
Cillum aliquip ex dolor duis commodo laborum nulla aliqua laboris laborum ut in. Aliquip et tempor fugiat culpa est. Lorem ullamco ipsum ex ut fugiat mollit est fugiat cupidatat non. Aute do sunt laboris nulla id laborum. Minim aute elit tempor nulla deserunt laboris quis irure labore cillum quis. Id ut irure ex cillum enim duis in do occaecat.",
        FechaPublicacion = new DateTime(2016, 01, 16, 06, 03, 43),
        FechaCreacion = new DateTime(2016, 01, 16, 06, 03, 43),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Bonita Mclean Hartman",
        Titulo = "Deserunt Lorem reprehenderit eiusmod excepteur sunt ea minim dolor et deserunt.",
        Descripcion = @"Do veniam exercitation ea incididunt occaecat. Adipisicing sunt id elit laborum ex amet. Quis dolor nisi non eiusmod amet qui esse pariatur elit ex. Minim minim aliqua duis minim minim consectetur tempor velit exercitation minim fugiat esse consectetur. Sit ad pariatur ex officia. Sint cillum cupidatat irure dolor eiusmod. Aliquip irure et irure magna nisi cillum fugiat incididunt id dolor mollit.Commodo proident minim anim ex excepteur excepteur veniam ex id ad in nisi duis. Officia fugiat reprehenderit proident commodo. Minim anim ut laborum aute nulla consectetur dolore.
Amet adipisicing exercitation et est sunt reprehenderit consectetur incididunt. Dolor laboris aliquip occaecat Lorem est elit. Laboris amet irure dolor ullamco magna aliquip id ad ad cillum amet aliqua. Consequat esse non et quis eu Lorem elit et labore. Qui mollit aliquip reprehenderit ex culpa laborum laborum. Est anim irure voluptate eu veniam aliquip do sint fugiat cupidatat aliquip.",
        FechaPublicacion = new DateTime(2016, 01, 17, 12, 59, 33),
        FechaCreacion = new DateTime(2016, 01, 17, 12, 59, 33),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Michael Gross Michael",
        Titulo = "Aliquip proident est tempor nostrud pariatur ullamco irure magna ut et et.",
        Descripcion = @"Sint aute cupidatat irure officia et sit. Ea labore elit consequat quis magna deserunt in ex ex labore eu sunt ex. Magna in dolor deserunt dolore ipsum dolor laboris tempor est non elit. Adipisicing occaecat quis voluptate non eiusmod nulla do ullamco fugiat cupidatat non exercitation anim deserunt. Tempor magna anim aute culpa qui cupidatat occaecat eu exercitation cillum est adipisicing. Esse aliqua quis est ullamco magna sint incididunt tempor ullamco mollit et.Tempor excepteur magna voluptate nulla nulla. Quis nisi aliquip ea Lorem ipsum velit minim amet consectetur ex nulla esse aliquip. Labore eu eu minim consectetur consectetur aliquip fugiat nostrud. Fugiat non nisi aute qui minim minim. Aute culpa veniam anim occaecat do ea quis irure. Non commodo cupidatat magna nisi sint nulla exercitation exercitation reprehenderit do mollit quis nulla nisi. Duis cillum dolor occaecat occaecat sit et tempor.
Occaecat veniam exercitation fugiat dolor eu elit commodo et et culpa enim quis. Aute mollit exercitation est aute ad amet sit. Dolore consectetur consectetur do officia est ullamco. Dolore et et adipisicing velit aliqua aliqua minim id. Aliquip officia commodo non anim velit deserunt velit laboris.",
        FechaPublicacion = new DateTime(2016, 01, 28, 12, 48, 46),
        FechaCreacion = new DateTime(2016, 01, 28, 12, 48, 46),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Beasley Glenn Nunez",
        Titulo = "Sunt aliquip exercitation velit commodo magna culpa occaecat qui labore exercitation.",
        Descripcion = @"Aute incididunt anim in magna commodo minim cupidatat magna voluptate velit. Cupidatat aliquip fugiat deserunt proident ipsum fugiat dolore ea culpa commodo. Dolore qui excepteur ut sit incididunt et et nostrud mollit ullamco. Minim incididunt fugiat exercitation voluptate amet et do adipisicing. Exercitation nulla sint anim qui sunt sit fugiat non est.Aute qui sit ad pariatur deserunt. Qui est cupidatat Lorem elit reprehenderit officia laboris. Aliquip ut id cupidatat nisi id duis. In anim adipisicing labore dolor elit anim nulla dolore nostrud excepteur. Culpa velit laboris voluptate exercitation. Commodo dolore sit duis duis occaecat pariatur velit et est aliquip do minim cupidatat pariatur. Mollit sint tempor officia dolor est.
Excepteur in excepteur sunt nostrud occaecat ipsum est magna duis ipsum nisi minim. Quis anim laboris proident eiusmod velit irure magna. Occaecat laboris laborum ex ad cillum elit tempor sit magna in. Elit Lorem enim irure cillum aliquip aliquip et minim. Incididunt non ad dolor aute. Exercitation qui proident occaecat id cupidatat amet magna laborum eu officia commodo culpa in qui. Eiusmod cupidatat mollit enim anim quis.",
        FechaPublicacion = new DateTime(2016, 01, 21, 07, 00, 23),
        FechaCreacion = new DateTime(2016, 01, 21, 07, 00, 23),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Simmons Donaldson Gallagher",
        Titulo = "Non ut amet ea occaecat est occaecat velit minim mollit minim eu.",
        Descripcion = @"Non est tempor tempor aliquip veniam do eiusmod id quis deserunt. Voluptate incididunt sit cupidatat ea ipsum aute in enim. Elit reprehenderit officia minim eu magna anim exercitation pariatur officia commodo tempor commodo culpa. Consectetur ullamco eiusmod non exercitation occaecat consequat aute elit do dolore ad in do. Velit magna est eiusmod ad aliqua esse et. Aliqua qui commodo reprehenderit cupidatat.Est ullamco sunt quis sint exercitation pariatur dolor. Voluptate aliqua non incididunt veniam fugiat reprehenderit commodo consequat laboris. Excepteur minim et est laborum pariatur esse fugiat do eu incididunt tempor. Labore commodo deserunt minim elit eu.
Anim consequat veniam proident ullamco pariatur id aute et consectetur in. Elit eiusmod minim nulla officia labore in reprehenderit aliquip mollit velit ut incididunt. Aliquip adipisicing qui exercitation proident amet quis ex anim nostrud elit sit excepteur aute. Duis voluptate reprehenderit quis minim labore velit id officia qui sit est commodo. Qui culpa fugiat consectetur nisi. Ex ullamco elit dolor irure occaecat enim. Anim commodo ipsum culpa officia sint do officia elit laboris commodo quis voluptate fugiat.",
        FechaPublicacion = new DateTime(2016, 01, 12, 01, 03, 27),
        FechaCreacion = new DateTime(2016, 01, 12, 01, 03, 27),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Vinson Powers Durham",
        Titulo = "Occaecat qui ipsum commodo dolor id in sint elit cillum in incididunt eu.",
        Descripcion = @"Ullamco do voluptate irure exercitation ut. Excepteur occaecat officia est amet voluptate qui adipisicing laborum labore exercitation ut in nisi. Consequat fugiat sunt dolore esse nostrud exercitation nulla mollit anim quis aliquip. Elit aute dolore dolore nostrud exercitation duis nostrud occaecat elit voluptate pariatur do. Excepteur eu tempor anim occaecat ut exercitation ipsum nisi. Excepteur tempor esse incididunt cupidatat.In culpa voluptate ad elit. Est eiusmod ullamco aliqua ea Lorem pariatur. Occaecat eiusmod sint qui in ea.
Labore eu et ipsum in commodo reprehenderit. Fugiat velit non nostrud voluptate nostrud cupidatat anim est sint officia anim consectetur aliqua. Irure adipisicing ea adipisicing ut enim veniam laborum reprehenderit officia. Culpa ex Lorem amet ipsum sit amet pariatur minim nulla veniam.",
        FechaPublicacion = new DateTime(2016, 01, 24, 10, 53, 15),
        FechaCreacion = new DateTime(2016, 01, 24, 10, 53, 15),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Johnston Farrell Santiago",
        Titulo = "Aute ullamco tempor eu in id tempor irure mollit incididunt minim qui culpa.",
        Descripcion = @"Voluptate ipsum adipisicing adipisicing deserunt ex culpa adipisicing. Dolore cupidatat culpa anim dolore voluptate consequat ad deserunt eu pariatur consequat eiusmod adipisicing. Magna mollit minim eiusmod sint laboris aliquip velit ea sunt do voluptate.Officia tempor ad mollit ex eu laborum ipsum ullamco et dolor nisi cillum. Ullamco sit nostrud enim irure enim. Amet sunt duis reprehenderit labore excepteur anim enim quis. Officia duis amet ad elit consequat elit laboris. Nisi velit culpa elit adipisicing nisi est anim cupidatat.
Incididunt et sit ipsum eiusmod est Lorem cupidatat esse Lorem sit elit ullamco. Mollit est sunt cupidatat aliqua minim irure sunt mollit veniam enim anim officia. Elit culpa minim ea ipsum est Lorem magna duis mollit voluptate reprehenderit irure. Ex Lorem veniam tempor in.",
        FechaPublicacion = new DateTime(2016, 01, 08, 09, 25, 42),
        FechaCreacion = new DateTime(2016, 01, 08, 09, 25, 42),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Kline Hale Knox",
        Titulo = "Duis incididunt veniam consectetur ex.",
        Descripcion = @"Reprehenderit qui aute id veniam sint cillum ipsum magna reprehenderit anim occaecat elit proident. Ex sint officia cupidatat aliqua aliqua velit ea ipsum ut occaecat. Ad pariatur nulla commodo in ea dolor culpa enim sit ad consequat amet. Cupidatat excepteur ad sunt duis exercitation reprehenderit voluptate ipsum ipsum sint duis tempor. Amet eu sint esse duis aliqua exercitation incididunt ea sit consequat ipsum laboris fugiat aliquip. Eu Lorem do eiusmod et. Reprehenderit officia amet aliqua sunt amet ea.Ex laborum ea aliquip magna. Adipisicing elit consequat duis eu proident nostrud occaecat enim irure. Ea id occaecat pariatur ut proident. Nisi voluptate labore dolor laborum ea incididunt et exercitation. Consequat ipsum incididunt ipsum in dolor. Irure aliquip nostrud reprehenderit cupidatat mollit minim aliqua magna laborum.
Incididunt excepteur dolore in elit esse proident quis. Cillum anim in aute voluptate ex eiusmod exercitation esse cupidatat fugiat. Dolor ex nisi exercitation adipisicing tempor fugiat nulla aute proident amet deserunt irure. Dolore aliqua ex nisi quis proident dolor. Laboris officia commodo fugiat exercitation dolore do est ullamco est.",
        FechaPublicacion = new DateTime(2016, 01, 14, 02, 48, 56),
        FechaCreacion = new DateTime(2016, 01, 14, 02, 48, 56),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Mathis Ingram Mckee",
        Titulo = "In aliquip amet irure consequat sint minim commodo aliquip.",
        Descripcion = @"Pariatur duis Lorem voluptate non et labore ipsum commodo laborum aute. Duis do culpa excepteur et in dolore laboris eiusmod occaecat laborum sunt ad. Officia excepteur aliquip labore commodo cillum ad aliquip irure. Consectetur culpa exercitation sunt veniam consequat.Et dolor nostrud laboris non excepteur. In aute culpa fugiat officia nisi proident ut. Lorem ex id minim duis deserunt occaecat magna velit cillum aliquip enim. Eu sunt occaecat veniam minim nisi culpa magna. Do magna consectetur sunt et id eu voluptate irure nostrud. Nisi id exercitation cillum esse nulla ex non occaecat tempor excepteur aute.
Cupidatat aliqua velit non magna laborum aliqua ullamco. Culpa non dolore Lorem sit labore ullamco cupidatat nisi. Laborum deserunt consectetur adipisicing eiusmod aute et tempor deserunt proident veniam. Consequat nisi in nulla ut quis cupidatat cupidatat sunt non duis. Veniam mollit exercitation minim fugiat proident ex qui. Occaecat enim minim deserunt consectetur voluptate irure voluptate ullamco incididunt Lorem cillum elit. Nulla nisi officia aliqua et dolore cupidatat non occaecat exercitation deserunt adipisicing minim sit nulla.",
        FechaPublicacion = new DateTime(2016, 01, 21, 04, 45, 02),
        FechaCreacion = new DateTime(2016, 01, 21, 04, 45, 02),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Mcmahon Gibson Branch",
        Titulo = "Consectetur eiusmod ex officia duis.",
        Descripcion = @"Occaecat occaecat aute excepteur do reprehenderit nostrud cupidatat. Magna laborum amet tempor officia consectetur. In aute exercitation dolore ea nostrud nulla incididunt. Eiusmod et in pariatur sint sunt culpa nostrud mollit. Ea laborum est commodo magna culpa sint magna labore sunt.Aute non aliqua culpa mollit mollit do eiusmod eiusmod adipisicing sunt sint adipisicing. Fugiat non labore tempor occaecat aliquip sint non ullamco minim consequat cupidatat reprehenderit. Voluptate non proident voluptate pariatur deserunt.
Duis enim nisi id ut Lorem ut qui ipsum mollit ut pariatur aute ex ea. Sint ea occaecat ad deserunt culpa nulla reprehenderit nostrud ad tempor. Enim eiusmod aute elit enim labore aute laborum incididunt occaecat do est elit aliquip incididunt. Sit Lorem sunt irure ipsum voluptate deserunt esse cillum culpa Lorem esse. Adipisicing amet mollit dolore elit. Irure veniam aliqua sunt est labore qui et elit esse ad labore ea.",
        FechaPublicacion = new DateTime(2016, 01, 08, 11, 19, 49),
        FechaCreacion = new DateTime(2016, 01, 08, 11, 19, 49),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Day Valenzuela Blackburn",
        Titulo = "Lorem do do aliquip eiusmod deserunt amet commodo laborum.",
        Descripcion = @"In esse ad do commodo non aliqua esse laboris sint cillum do ea. Non dolore laboris proident adipisicing duis consectetur consequat eu sit mollit deserunt. Consequat excepteur incididunt et nostrud reprehenderit. Commodo magna nisi ut exercitation incididunt esse eu tempor. Aliquip ad velit do fugiat Lorem irure nisi id amet.Velit aute minim aliquip do. Dolor eiusmod ad anim laboris consectetur consectetur eiusmod exercitation mollit officia ea. Esse irure laboris ut ad laborum deserunt pariatur occaecat ullamco elit esse in culpa. Sunt deserunt minim nulla pariatur reprehenderit.
Pariatur aliquip qui magna consectetur cupidatat sit officia eu dolor cillum consequat. Adipisicing Lorem aliquip laboris eiusmod quis aliquip Lorem. Dolore cillum minim adipisicing minim.",
        FechaPublicacion = new DateTime(2016, 01, 08, 06, 54, 39),
        FechaCreacion = new DateTime(2016, 01, 08, 06, 54, 39),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion
    {
        Usuario = editor2,
        Autor = "Carol Nicholson Gonzalez",
        Titulo = "Eu nostrud pariatur ad consequat minim.",
        Descripcion = @"Occaecat sint reprehenderit mollit officia magna est Lorem occaecat cupidatat nostrud. Eiusmod sint ea laboris esse ut do pariatur amet. Laborum sit minim sit eiusmod excepteur labore deserunt ea qui aliqua occaecat ea quis fugiat. Ad est sint aliquip laboris do laboris deserunt pariatur ut. Excepteur sint nulla pariatur adipisicing aliqua. Elit sit voluptate adipisicing magna anim ipsum aute duis quis duis aute exercitation.Anim fugiat fugiat aliqua eiusmod elit excepteur irure aliquip cillum amet ullamco irure deserunt amet. Mollit sunt labore proident mollit do ex sint tempor sint aliqua. Id cillum et eiusmod non. Irure amet tempor nisi ex esse est dolore. Deserunt enim elit reprehenderit nisi id irure exercitation. Nisi voluptate velit ad officia qui laboris qui amet ipsum anim incididunt magna anim qui. Dolor labore fugiat non et est.
Elit est ut fugiat nulla enim qui est nulla aute. Laboris sit exercitation incididunt veniam. Deserunt incididunt nostrud cillum incididunt elit qui consectetur minim enim. Excepteur enim Lorem ea ea fugiat duis enim enim aliquip.",
        FechaPublicacion = new DateTime(2016, 01, 26, 10, 56, 24),
        FechaCreacion = new DateTime(2016, 01, 26, 10, 56, 24),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion
    {
        Usuario = editor2,
        Autor = "King Benjamin Hughes",
        Titulo = "Ea officia cupidatat mollit occaecat aliqua ea.",
        Descripcion = @"Voluptate elit eu eiusmod proident nulla anim aliqua incididunt irure magna velit. Magna consequat do exercitation veniam duis elit dolor eu occaecat. Consequat ex est ex nisi elit enim ullamco irure duis amet elit. Ad deserunt anim ipsum fugiat aliqua. Voluptate id laborum consequat consequat exercitation amet reprehenderit nulla laborum ipsum. Esse in proident duis occaecat cillum ex consequat commodo aliqua. Non esse sit ad mollit laboris aliqua ex ex et eiusmod enim sit nisi.Eiusmod nisi officia sint culpa laborum non cupidatat proident adipisicing tempor enim. Esse magna deserunt fugiat duis. Nostrud excepteur veniam ullamco esse fugiat amet adipisicing exercitation sit est deserunt irure adipisicing. Nisi in incididunt incididunt ipsum do.
Officia ex anim fugiat do cillum. Id non non ullamco ut incididunt. Sunt irure dolore minim duis dolor esse excepteur ut laborum do irure occaecat exercitation sunt.",
        FechaPublicacion = new DateTime(2016, 01, 08, 01, 20, 58),
        FechaCreacion = new DateTime(2016, 01, 08, 01, 20, 58),
        CategoriaID = 6,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Kristina Shaffer Rios",
        Titulo = "Excepteur aliqua et exercitation dolor.",
        Descripcion = @"Veniam minim proident tempor irure et eiusmod in. Sint ut ipsum ullamco dolore aute eiusmod minim quis mollit amet nulla. Labore ullamco sunt ullamco est nulla excepteur consequat non sunt labore ut. Minim non mollit enim elit ex. Exercitation reprehenderit Lorem velit deserunt cillum ut cillum voluptate amet enim deserunt sit id. Ut reprehenderit nisi sit sit sit excepteur. Pariatur ea ea minim veniam aliquip.Consectetur ipsum irure consectetur veniam tempor deserunt quis aliqua eu ad magna ea eu mollit. Fugiat sint magna voluptate eu veniam ut nostrud adipisicing quis aliquip quis. Id incididunt incididunt veniam consectetur in nostrud nisi ullamco adipisicing in sit elit. Non aute amet exercitation fugiat ullamco. Non consequat sunt dolore esse excepteur consectetur excepteur ex nostrud reprehenderit id anim non. Velit duis labore elit pariatur.
Anim voluptate nulla duis veniam ea culpa proident officia mollit est. Veniam ex laborum commodo officia ex consequat ullamco est elit cillum. Laboris aliqua culpa voluptate ut. Nisi eiusmod ad elit sit ullamco veniam minim enim magna pariatur labore non. Do eu sint ad ut commodo culpa commodo exercitation proident sunt anim. Eiusmod enim Lorem non nulla laborum amet anim eiusmod sunt aliquip nulla aute et pariatur.",
        FechaPublicacion = new DateTime(2016, 01, 13, 09, 36, 13),
        FechaCreacion = new DateTime(2016, 01, 13, 09, 36, 13),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Perry Colon Kaufman",
        Titulo = "Pariatur in in ullamco ipsum culpa cillum aliqua commodo ex.",
        Descripcion = @"Aliquip reprehenderit id velit ex mollit irure id incididunt consectetur proident commodo amet sunt. Sunt id mollit ipsum qui laborum culpa magna dolor aliquip incididunt laboris qui cillum. Ad esse non mollit officia occaecat sit aliquip excepteur.Non eu exercitation eiusmod aliquip voluptate exercitation voluptate elit enim ex velit fugiat sint. Et culpa ad dolor nulla tempor ut in cillum ullamco enim nostrud cupidatat do. Deserunt labore magna fugiat consequat do qui id.
Culpa aliquip qui enim elit est. Ad Lorem consectetur amet cillum et consequat elit Lorem adipisicing. Non culpa voluptate non ad eiusmod nulla anim. Ea commodo magna veniam velit qui proident reprehenderit labore ipsum ut ex. Irure ex exercitation dolore amet est et labore mollit pariatur adipisicing velit ad laboris.",
        FechaPublicacion = new DateTime(2016, 01, 16, 02, 15, 03),
        FechaCreacion = new DateTime(2016, 01, 16, 02, 15, 03),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Gomez Sherman Morgan",
        Titulo = "Adipisicing ea ex consectetur reprehenderit veniam officia ex sunt ipsum.",
        Descripcion = @"Qui ad adipisicing eu veniam. Minim minim laborum dolor mollit nisi eiusmod amet id. Eiusmod irure consequat ea elit mollit et tempor sit reprehenderit pariatur excepteur nulla reprehenderit cupidatat. Nostrud labore quis consectetur Lorem proident aute officia duis nostrud ullamco. Dolor non in mollit qui aliqua cillum. Lorem dolore pariatur ipsum quis adipisicing. Ullamco laborum velit laborum duis quis laboris incididunt labore tempor nisi.Irure consequat ullamco ullamco minim quis tempor quis dolore ex cillum adipisicing duis non fugiat. Enim culpa aute excepteur eu sunt fugiat et amet Lorem. Consectetur eu veniam veniam duis cillum. Velit esse incididunt aliquip et.
Deserunt enim aliquip qui dolore laboris sit anim ea duis dolor. Occaecat mollit dolore incididunt dolor est tempor cupidatat ad reprehenderit anim incididunt laboris non do. Do excepteur et est id adipisicing laboris magna culpa proident officia irure aute. Commodo enim incididunt exercitation eu ut esse elit officia quis sunt adipisicing sint. Non anim magna culpa nisi veniam ut fugiat consectetur incididunt tempor. Occaecat Lorem proident anim mollit reprehenderit aute commodo sit ad esse irure dolore.",
        FechaPublicacion = new DateTime(2016, 01, 23, 03, 11, 07),
        FechaCreacion = new DateTime(2016, 01, 23, 03, 11, 07),
        CategoriaID = 4,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Dickerson Hanson Warner",
        Titulo = "Consequat dolor excepteur consequat adipisicing in occaecat non do tempor.",
        Descripcion = @"Esse laborum ex voluptate veniam irure laboris elit ut non culpa duis. Enim anim veniam ad excepteur eu esse tempor. Occaecat sunt est officia nisi ea qui sunt sunt dolor Lorem duis ea officia magna. Dolore eiusmod eiusmod enim reprehenderit ea culpa aliqua. Ad nulla voluptate sint aliqua incididunt excepteur mollit pariatur officia. Laborum labore proident sint culpa. Reprehenderit consectetur et reprehenderit non aliqua reprehenderit voluptate occaecat incididunt tempor.Sunt pariatur laboris cillum do do magna cupidatat velit commodo excepteur do mollit esse incididunt. Occaecat velit magna adipisicing nulla commodo deserunt est cupidatat laborum consectetur. Consequat officia nisi qui non laboris reprehenderit commodo eiusmod tempor labore ex duis qui. Incididunt in amet irure ipsum veniam deserunt. Veniam consectetur laboris qui Lorem fugiat deserunt nostrud.
Tempor dolor incididunt ut incididunt elit officia adipisicing laborum pariatur cillum. Adipisicing in et occaecat id. Lorem et nostrud reprehenderit id consectetur excepteur qui nisi qui. Sunt irure excepteur cupidatat veniam esse adipisicing id aute culpa Lorem. Incididunt ad est et tempor laborum duis nulla nostrud ut.",
        FechaPublicacion = new DateTime(2016, 01, 22, 02, 25, 12),
        FechaCreacion = new DateTime(2016, 01, 22, 02, 25, 12),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Delaney Mcgowan Pennington",
        Titulo = "Laborum veniam commodo dolor ut proident elit anim consequat nisi ea non irure id adipisicing.",
        Descripcion = @"Occaecat proident commodo eiusmod in ut do velit. Consectetur tempor dolor magna eiusmod. Ipsum ipsum non proident ad fugiat reprehenderit elit amet laboris anim incididunt qui consectetur. Aliqua id consequat magna nisi voluptate proident veniam sit ullamco. Aliqua cillum non irure minim eu eiusmod eiusmod sint officia non consequat culpa qui. Ut consequat magna est duis deserunt mollit reprehenderit ea in.Commodo duis sit labore laboris velit eu nostrud eiusmod mollit cupidatat officia laborum excepteur non. Nulla et irure deserunt culpa tempor eiusmod ullamco id. Ea ut ex in dolore sint culpa do laboris. Mollit aliquip qui irure id aute duis voluptate qui sint esse magna. Nostrud incididunt minim culpa cillum consequat exercitation adipisicing amet et dolor veniam laboris aute. Consequat ut adipisicing anim ipsum laborum nisi proident pariatur culpa. Nostrud voluptate aliqua fugiat occaecat Lorem.
Non est enim eu officia sit dolor minim aute magna consequat. Laborum ex fugiat magna dolore nulla eiusmod mollit aute enim esse. Aliquip elit mollit nostrud excepteur. Labore adipisicing velit ut est. Aute cillum aliqua sunt aliquip et ipsum magna.",
        FechaPublicacion = new DateTime(2016, 01, 02, 05, 46, 56),
        FechaCreacion = new DateTime(2016, 01, 02, 05, 46, 56),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Macias Pittman Wilkinson",
        Titulo = "Elit ut consequat sunt minim tempor commodo excepteur ad occaecat in laboris est quis.",
        Descripcion = @"Sunt ipsum anim labore adipisicing consectetur eiusmod. Esse veniam occaecat qui excepteur qui amet reprehenderit aute amet. Pariatur sunt reprehenderit anim enim qui et dolore Lorem. Dolore aliquip est aliquip excepteur irure excepteur eiusmod.Minim nulla est nulla et in mollit laboris labore magna laboris officia. Et in duis aute est sint elit sunt id. Reprehenderit nisi reprehenderit sit voluptate velit officia quis consectetur in. Duis sit velit ut consectetur. Ad quis eu occaecat qui. Cillum officia laboris irure voluptate duis ex reprehenderit ex aliqua.
Duis labore irure duis excepteur magna elit occaecat aliquip. Cupidatat do dolor esse non elit commodo cillum cillum occaecat. Sunt adipisicing cupidatat sit Lorem culpa. Ullamco dolor mollit quis cillum minim cupidatat aliquip sit ullamco. Irure Lorem nisi minim do.",
        FechaPublicacion = new DateTime(2016, 01, 05, 10, 36, 24),
        FechaCreacion = new DateTime(2016, 01, 05, 10, 36, 24),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
        },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Vickie Salas Sanchez",
        Titulo = "Reprehenderit consequat enim aliqua eiusmod exercitation ad minim laborum commodo sit.",
        Descripcion = @"Proident commodo nostrud ullamco est Lorem dolore proident veniam amet laboris. Laborum sint eu fugiat exercitation officia elit. Exercitation exercitation anim eu sint non consectetur magna laboris ullamco ex exercitation pariatur.Reprehenderit dolor et anim magna. Qui ut excepteur aliqua magna cupidatat dolore in aliqua Lorem. Reprehenderit exercitation minim adipisicing fugiat reprehenderit laboris minim. Deserunt anim occaecat ut excepteur labore veniam magna eu consequat.
Minim ex adipisicing tempor exercitation occaecat dolore veniam laboris qui aliquip cupidatat aliquip duis. Deserunt do quis cupidatat nostrud. Quis aliqua aliquip ex laborum sunt excepteur in. Ea nostrud Lorem id ad reprehenderit sunt excepteur laborum occaecat.",
        FechaPublicacion = new DateTime(2016, 01, 12, 02, 00, 20),
        FechaCreacion = new DateTime(2016, 01, 12, 02, 00, 20),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Alison Solis Pena",
        Titulo = "Ipsum exercitation fugiat laboris labore.",
        Descripcion = @"Sit proident occaecat esse commodo velit pariatur ex occaecat incididunt nulla magna mollit sunt. Amet cillum velit eu voluptate commodo deserunt veniam fugiat nulla magna sint dolor cupidatat. Aute consequat mollit excepteur in ad id mollit do. Cillum nisi adipisicing aliqua minim eu nostrud sit laboris eu ea ex dolore non proident.Ullamco tempor duis quis labore nostrud. Est sunt Lorem eiusmod Lorem enim ut fugiat velit. Sint dolor mollit duis esse exercitation irure sint officia voluptate. Id dolore adipisicing quis in. Consequat cupidatat nulla ut ea non proident minim culpa in. Aute aliquip occaecat ut do esse irure incididunt.
Cillum aliquip ex dolor duis commodo laborum nulla aliqua laboris laborum ut in. Aliquip et tempor fugiat culpa est. Lorem ullamco ipsum ex ut fugiat mollit est fugiat cupidatat non. Aute do sunt laboris nulla id laborum. Minim aute elit tempor nulla deserunt laboris quis irure labore cillum quis. Id ut irure ex cillum enim duis in do occaecat.",
        FechaPublicacion = new DateTime(2016, 01, 16, 06, 03, 43),
        FechaCreacion = new DateTime(2016, 01, 16, 06, 03, 43),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Bonita Mclean Hartman",
        Titulo = "Deserunt Lorem reprehenderit eiusmod excepteur sunt ea minim dolor et deserunt.",
        Descripcion = @"Do veniam exercitation ea incididunt occaecat. Adipisicing sunt id elit laborum ex amet. Quis dolor nisi non eiusmod amet qui esse pariatur elit ex. Minim minim aliqua duis minim minim consectetur tempor velit exercitation minim fugiat esse consectetur. Sit ad pariatur ex officia. Sint cillum cupidatat irure dolor eiusmod. Aliquip irure et irure magna nisi cillum fugiat incididunt id dolor mollit.Commodo proident minim anim ex excepteur excepteur veniam ex id ad in nisi duis. Officia fugiat reprehenderit proident commodo. Minim anim ut laborum aute nulla consectetur dolore.
Amet adipisicing exercitation et est sunt reprehenderit consectetur incididunt. Dolor laboris aliquip occaecat Lorem est elit. Laboris amet irure dolor ullamco magna aliquip id ad ad cillum amet aliqua. Consequat esse non et quis eu Lorem elit et labore. Qui mollit aliquip reprehenderit ex culpa laborum laborum. Est anim irure voluptate eu veniam aliquip do sint fugiat cupidatat aliquip.",
        FechaPublicacion = new DateTime(2016, 01, 17, 12, 59, 33),
        FechaCreacion = new DateTime(2016, 01, 17, 12, 59, 33),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Michael Gross Michael",
        Titulo = "Aliquip proident est tempor nostrud pariatur ullamco irure magna ut et et.",
        Descripcion = @"Sint aute cupidatat irure officia et sit. Ea labore elit consequat quis magna deserunt in ex ex labore eu sunt ex. Magna in dolor deserunt dolore ipsum dolor laboris tempor est non elit. Adipisicing occaecat quis voluptate non eiusmod nulla do ullamco fugiat cupidatat non exercitation anim deserunt. Tempor magna anim aute culpa qui cupidatat occaecat eu exercitation cillum est adipisicing. Esse aliqua quis est ullamco magna sint incididunt tempor ullamco mollit et.Tempor excepteur magna voluptate nulla nulla. Quis nisi aliquip ea Lorem ipsum velit minim amet consectetur ex nulla esse aliquip. Labore eu eu minim consectetur consectetur aliquip fugiat nostrud. Fugiat non nisi aute qui minim minim. Aute culpa veniam anim occaecat do ea quis irure. Non commodo cupidatat magna nisi sint nulla exercitation exercitation reprehenderit do mollit quis nulla nisi. Duis cillum dolor occaecat occaecat sit et tempor.
Occaecat veniam exercitation fugiat dolor eu elit commodo et et culpa enim quis. Aute mollit exercitation est aute ad amet sit. Dolore consectetur consectetur do officia est ullamco. Dolore et et adipisicing velit aliqua aliqua minim id. Aliquip officia commodo non anim velit deserunt velit laboris.",
        FechaPublicacion = new DateTime(2016, 01, 28, 12, 48, 46),
        FechaCreacion = new DateTime(2016, 01, 28, 12, 48, 46),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Beasley Glenn Nunez",
        Titulo = "Sunt aliquip exercitation velit commodo magna culpa occaecat qui labore exercitation.",
        Descripcion = @"Aute incididunt anim in magna commodo minim cupidatat magna voluptate velit. Cupidatat aliquip fugiat deserunt proident ipsum fugiat dolore ea culpa commodo. Dolore qui excepteur ut sit incididunt et et nostrud mollit ullamco. Minim incididunt fugiat exercitation voluptate amet et do adipisicing. Exercitation nulla sint anim qui sunt sit fugiat non est.Aute qui sit ad pariatur deserunt. Qui est cupidatat Lorem elit reprehenderit officia laboris. Aliquip ut id cupidatat nisi id duis. In anim adipisicing labore dolor elit anim nulla dolore nostrud excepteur. Culpa velit laboris voluptate exercitation. Commodo dolore sit duis duis occaecat pariatur velit et est aliquip do minim cupidatat pariatur. Mollit sint tempor officia dolor est.
Excepteur in excepteur sunt nostrud occaecat ipsum est magna duis ipsum nisi minim. Quis anim laboris proident eiusmod velit irure magna. Occaecat laboris laborum ex ad cillum elit tempor sit magna in. Elit Lorem enim irure cillum aliquip aliquip et minim. Incididunt non ad dolor aute. Exercitation qui proident occaecat id cupidatat amet magna laborum eu officia commodo culpa in qui. Eiusmod cupidatat mollit enim anim quis.",
        FechaPublicacion = new DateTime(2016, 01, 21, 07, 00, 23),
        FechaCreacion = new DateTime(2016, 01, 21, 07, 00, 23),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Simmons Donaldson Gallagher",
        Titulo = "Non ut amet ea occaecat est occaecat velit minim mollit minim eu.",
        Descripcion = @"Non est tempor tempor aliquip veniam do eiusmod id quis deserunt. Voluptate incididunt sit cupidatat ea ipsum aute in enim. Elit reprehenderit officia minim eu magna anim exercitation pariatur officia commodo tempor commodo culpa. Consectetur ullamco eiusmod non exercitation occaecat consequat aute elit do dolore ad in do. Velit magna est eiusmod ad aliqua esse et. Aliqua qui commodo reprehenderit cupidatat.Est ullamco sunt quis sint exercitation pariatur dolor. Voluptate aliqua non incididunt veniam fugiat reprehenderit commodo consequat laboris. Excepteur minim et est laborum pariatur esse fugiat do eu incididunt tempor. Labore commodo deserunt minim elit eu.
Anim consequat veniam proident ullamco pariatur id aute et consectetur in. Elit eiusmod minim nulla officia labore in reprehenderit aliquip mollit velit ut incididunt. Aliquip adipisicing qui exercitation proident amet quis ex anim nostrud elit sit excepteur aute. Duis voluptate reprehenderit quis minim labore velit id officia qui sit est commodo. Qui culpa fugiat consectetur nisi. Ex ullamco elit dolor irure occaecat enim. Anim commodo ipsum culpa officia sint do officia elit laboris commodo quis voluptate fugiat.",
        FechaPublicacion = new DateTime(2016, 01, 12, 01, 03, 27),
        FechaCreacion = new DateTime(2016, 01, 12, 01, 03, 27),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Vinson Powers Durham",
        Titulo = "Occaecat qui ipsum commodo dolor id in sint elit cillum in incididunt eu.",
        Descripcion = @"Ullamco do voluptate irure exercitation ut. Excepteur occaecat officia est amet voluptate qui adipisicing laborum labore exercitation ut in nisi. Consequat fugiat sunt dolore esse nostrud exercitation nulla mollit anim quis aliquip. Elit aute dolore dolore nostrud exercitation duis nostrud occaecat elit voluptate pariatur do. Excepteur eu tempor anim occaecat ut exercitation ipsum nisi. Excepteur tempor esse incididunt cupidatat.In culpa voluptate ad elit. Est eiusmod ullamco aliqua ea Lorem pariatur. Occaecat eiusmod sint qui in ea.
Labore eu et ipsum in commodo reprehenderit. Fugiat velit non nostrud voluptate nostrud cupidatat anim est sint officia anim consectetur aliqua. Irure adipisicing ea adipisicing ut enim veniam laborum reprehenderit officia. Culpa ex Lorem amet ipsum sit amet pariatur minim nulla veniam.",
        FechaPublicacion = new DateTime(2016, 01, 24, 10, 53, 15),
        FechaCreacion = new DateTime(2016, 01, 24, 10, 53, 15),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Johnston Farrell Santiago",
        Titulo = "Aute ullamco tempor eu in id tempor irure mollit incididunt minim qui culpa.",
        Descripcion = @"Voluptate ipsum adipisicing adipisicing deserunt ex culpa adipisicing. Dolore cupidatat culpa anim dolore voluptate consequat ad deserunt eu pariatur consequat eiusmod adipisicing. Magna mollit minim eiusmod sint laboris aliquip velit ea sunt do voluptate.Officia tempor ad mollit ex eu laborum ipsum ullamco et dolor nisi cillum. Ullamco sit nostrud enim irure enim. Amet sunt duis reprehenderit labore excepteur anim enim quis. Officia duis amet ad elit consequat elit laboris. Nisi velit culpa elit adipisicing nisi est anim cupidatat.
Incididunt et sit ipsum eiusmod est Lorem cupidatat esse Lorem sit elit ullamco. Mollit est sunt cupidatat aliqua minim irure sunt mollit veniam enim anim officia. Elit culpa minim ea ipsum est Lorem magna duis mollit voluptate reprehenderit irure. Ex Lorem veniam tempor in.",
        FechaPublicacion = new DateTime(2016, 01, 08, 09, 25, 42),
        FechaCreacion = new DateTime(2016, 01, 08, 09, 25, 42),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Kline Hale Knox",
        Titulo = "Duis incididunt veniam consectetur ex.",
        Descripcion = @"Reprehenderit qui aute id veniam sint cillum ipsum magna reprehenderit anim occaecat elit proident. Ex sint officia cupidatat aliqua aliqua velit ea ipsum ut occaecat. Ad pariatur nulla commodo in ea dolor culpa enim sit ad consequat amet. Cupidatat excepteur ad sunt duis exercitation reprehenderit voluptate ipsum ipsum sint duis tempor. Amet eu sint esse duis aliqua exercitation incididunt ea sit consequat ipsum laboris fugiat aliquip. Eu Lorem do eiusmod et. Reprehenderit officia amet aliqua sunt amet ea.Ex laborum ea aliquip magna. Adipisicing elit consequat duis eu proident nostrud occaecat enim irure. Ea id occaecat pariatur ut proident. Nisi voluptate labore dolor laborum ea incididunt et exercitation. Consequat ipsum incididunt ipsum in dolor. Irure aliquip nostrud reprehenderit cupidatat mollit minim aliqua magna laborum.
Incididunt excepteur dolore in elit esse proident quis. Cillum anim in aute voluptate ex eiusmod exercitation esse cupidatat fugiat. Dolor ex nisi exercitation adipisicing tempor fugiat nulla aute proident amet deserunt irure. Dolore aliqua ex nisi quis proident dolor. Laboris officia commodo fugiat exercitation dolore do est ullamco est.",
        FechaPublicacion = new DateTime(2016, 01, 14, 02, 48, 56),
        FechaCreacion = new DateTime(2016, 01, 14, 02, 48, 56),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Mathis Ingram Mckee",
        Titulo = "In aliquip amet irure consequat sint minim commodo aliquip.",
        Descripcion = @"Pariatur duis Lorem voluptate non et labore ipsum commodo laborum aute. Duis do culpa excepteur et in dolore laboris eiusmod occaecat laborum sunt ad. Officia excepteur aliquip labore commodo cillum ad aliquip irure. Consectetur culpa exercitation sunt veniam consequat.Et dolor nostrud laboris non excepteur. In aute culpa fugiat officia nisi proident ut. Lorem ex id minim duis deserunt occaecat magna velit cillum aliquip enim. Eu sunt occaecat veniam minim nisi culpa magna. Do magna consectetur sunt et id eu voluptate irure nostrud. Nisi id exercitation cillum esse nulla ex non occaecat tempor excepteur aute.
Cupidatat aliqua velit non magna laborum aliqua ullamco. Culpa non dolore Lorem sit labore ullamco cupidatat nisi. Laborum deserunt consectetur adipisicing eiusmod aute et tempor deserunt proident veniam. Consequat nisi in nulla ut quis cupidatat cupidatat sunt non duis. Veniam mollit exercitation minim fugiat proident ex qui. Occaecat enim minim deserunt consectetur voluptate irure voluptate ullamco incididunt Lorem cillum elit. Nulla nisi officia aliqua et dolore cupidatat non occaecat exercitation deserunt adipisicing minim sit nulla.",
        FechaPublicacion = new DateTime(2016, 01, 21, 04, 45, 02),
        FechaCreacion = new DateTime(2016, 01, 21, 04, 45, 02),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Mcmahon Gibson Branch",
        Titulo = "Consectetur eiusmod ex officia duis.",
        Descripcion = @"Occaecat occaecat aute excepteur do reprehenderit nostrud cupidatat. Magna laborum amet tempor officia consectetur. In aute exercitation dolore ea nostrud nulla incididunt. Eiusmod et in pariatur sint sunt culpa nostrud mollit. Ea laborum est commodo magna culpa sint magna labore sunt.Aute non aliqua culpa mollit mollit do eiusmod eiusmod adipisicing sunt sint adipisicing. Fugiat non labore tempor occaecat aliquip sint non ullamco minim consequat cupidatat reprehenderit. Voluptate non proident voluptate pariatur deserunt.
Duis enim nisi id ut Lorem ut qui ipsum mollit ut pariatur aute ex ea. Sint ea occaecat ad deserunt culpa nulla reprehenderit nostrud ad tempor. Enim eiusmod aute elit enim labore aute laborum incididunt occaecat do est elit aliquip incididunt. Sit Lorem sunt irure ipsum voluptate deserunt esse cillum culpa Lorem esse. Adipisicing amet mollit dolore elit. Irure veniam aliqua sunt est labore qui et elit esse ad labore ea.",
        FechaPublicacion = new DateTime(2016, 01, 08, 11, 19, 49),
        FechaCreacion = new DateTime(2016, 01, 08, 11, 19, 49),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Day Valenzuela Blackburn",
        Titulo = "Lorem do do aliquip eiusmod deserunt amet commodo laborum.",
        Descripcion = @"In esse ad do commodo non aliqua esse laboris sint cillum do ea. Non dolore laboris proident adipisicing duis consectetur consequat eu sit mollit deserunt. Consequat excepteur incididunt et nostrud reprehenderit. Commodo magna nisi ut exercitation incididunt esse eu tempor. Aliquip ad velit do fugiat Lorem irure nisi id amet.Velit aute minim aliquip do. Dolor eiusmod ad anim laboris consectetur consectetur eiusmod exercitation mollit officia ea. Esse irure laboris ut ad laborum deserunt pariatur occaecat ullamco elit esse in culpa. Sunt deserunt minim nulla pariatur reprehenderit.
Pariatur aliquip qui magna consectetur cupidatat sit officia eu dolor cillum consequat. Adipisicing Lorem aliquip laboris eiusmod quis aliquip Lorem. Dolore cillum minim adipisicing minim.",
        FechaPublicacion = new DateTime(2016, 01, 08, 06, 54, 39),
        FechaCreacion = new DateTime(2016, 01, 08, 06, 54, 39),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion
    {
        Usuario = editor2,
        Autor = "Carol Nicholson Gonzalez",
        Titulo = "Eu nostrud pariatur ad consequat minim.",
        Descripcion = @"Occaecat sint reprehenderit mollit officia magna est Lorem occaecat cupidatat nostrud. Eiusmod sint ea laboris esse ut do pariatur amet. Laborum sit minim sit eiusmod excepteur labore deserunt ea qui aliqua occaecat ea quis fugiat. Ad est sint aliquip laboris do laboris deserunt pariatur ut. Excepteur sint nulla pariatur adipisicing aliqua. Elit sit voluptate adipisicing magna anim ipsum aute duis quis duis aute exercitation.Anim fugiat fugiat aliqua eiusmod elit excepteur irure aliquip cillum amet ullamco irure deserunt amet. Mollit sunt labore proident mollit do ex sint tempor sint aliqua. Id cillum et eiusmod non. Irure amet tempor nisi ex esse est dolore. Deserunt enim elit reprehenderit nisi id irure exercitation. Nisi voluptate velit ad officia qui laboris qui amet ipsum anim incididunt magna anim qui. Dolor labore fugiat non et est.
Elit est ut fugiat nulla enim qui est nulla aute. Laboris sit exercitation incididunt veniam. Deserunt incididunt nostrud cillum incididunt elit qui consectetur minim enim. Excepteur enim Lorem ea ea fugiat duis enim enim aliquip.",
        FechaPublicacion = new DateTime(2016, 01, 26, 10, 56, 24),
        FechaCreacion = new DateTime(2016, 01, 26, 10, 56, 24),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion
    {
        Usuario = editor2,
        Autor = "King Benjamin Hughes",
        Titulo = "Ea officia cupidatat mollit occaecat aliqua ea.",
        Descripcion = @"Voluptate elit eu eiusmod proident nulla anim aliqua incididunt irure magna velit. Magna consequat do exercitation veniam duis elit dolor eu occaecat. Consequat ex est ex nisi elit enim ullamco irure duis amet elit. Ad deserunt anim ipsum fugiat aliqua. Voluptate id laborum consequat consequat exercitation amet reprehenderit nulla laborum ipsum. Esse in proident duis occaecat cillum ex consequat commodo aliqua. Non esse sit ad mollit laboris aliqua ex ex et eiusmod enim sit nisi.Eiusmod nisi officia sint culpa laborum non cupidatat proident adipisicing tempor enim. Esse magna deserunt fugiat duis. Nostrud excepteur veniam ullamco esse fugiat amet adipisicing exercitation sit est deserunt irure adipisicing. Nisi in incididunt incididunt ipsum do.
Officia ex anim fugiat do cillum. Id non non ullamco ut incididunt. Sunt irure dolore minim duis dolor esse excepteur ut laborum do irure occaecat exercitation sunt.",
        FechaPublicacion = new DateTime(2016, 01, 08, 01, 20, 58),
        FechaCreacion = new DateTime(2016, 01, 08, 01, 20, 58),
        CategoriaID = 6,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Kristina Shaffer Rios",
        Titulo = "Excepteur aliqua et exercitation dolor.",
        Descripcion = @"Veniam minim proident tempor irure et eiusmod in. Sint ut ipsum ullamco dolore aute eiusmod minim quis mollit amet nulla. Labore ullamco sunt ullamco est nulla excepteur consequat non sunt labore ut. Minim non mollit enim elit ex. Exercitation reprehenderit Lorem velit deserunt cillum ut cillum voluptate amet enim deserunt sit id. Ut reprehenderit nisi sit sit sit excepteur. Pariatur ea ea minim veniam aliquip.Consectetur ipsum irure consectetur veniam tempor deserunt quis aliqua eu ad magna ea eu mollit. Fugiat sint magna voluptate eu veniam ut nostrud adipisicing quis aliquip quis. Id incididunt incididunt veniam consectetur in nostrud nisi ullamco adipisicing in sit elit. Non aute amet exercitation fugiat ullamco. Non consequat sunt dolore esse excepteur consectetur excepteur ex nostrud reprehenderit id anim non. Velit duis labore elit pariatur.
Anim voluptate nulla duis veniam ea culpa proident officia mollit est. Veniam ex laborum commodo officia ex consequat ullamco est elit cillum. Laboris aliqua culpa voluptate ut. Nisi eiusmod ad elit sit ullamco veniam minim enim magna pariatur labore non. Do eu sint ad ut commodo culpa commodo exercitation proident sunt anim. Eiusmod enim Lorem non nulla laborum amet anim eiusmod sunt aliquip nulla aute et pariatur.",
        FechaPublicacion = new DateTime(2016, 01, 13, 09, 36, 13),
        FechaCreacion = new DateTime(2016, 01, 13, 09, 36, 13),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Perry Colon Kaufman",
        Titulo = "Pariatur in in ullamco ipsum culpa cillum aliqua commodo ex.",
        Descripcion = @"Aliquip reprehenderit id velit ex mollit irure id incididunt consectetur proident commodo amet sunt. Sunt id mollit ipsum qui laborum culpa magna dolor aliquip incididunt laboris qui cillum. Ad esse non mollit officia occaecat sit aliquip excepteur.Non eu exercitation eiusmod aliquip voluptate exercitation voluptate elit enim ex velit fugiat sint. Et culpa ad dolor nulla tempor ut in cillum ullamco enim nostrud cupidatat do. Deserunt labore magna fugiat consequat do qui id.
Culpa aliquip qui enim elit est. Ad Lorem consectetur amet cillum et consequat elit Lorem adipisicing. Non culpa voluptate non ad eiusmod nulla anim. Ea commodo magna veniam velit qui proident reprehenderit labore ipsum ut ex. Irure ex exercitation dolore amet est et labore mollit pariatur adipisicing velit ad laboris.",
        FechaPublicacion = new DateTime(2016, 01, 16, 02, 15, 03),
        FechaCreacion = new DateTime(2016, 01, 16, 02, 15, 03),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Gomez Sherman Morgan",
        Titulo = "Adipisicing ea ex consectetur reprehenderit veniam officia ex sunt ipsum.",
        Descripcion = @"Qui ad adipisicing eu veniam. Minim minim laborum dolor mollit nisi eiusmod amet id. Eiusmod irure consequat ea elit mollit et tempor sit reprehenderit pariatur excepteur nulla reprehenderit cupidatat. Nostrud labore quis consectetur Lorem proident aute officia duis nostrud ullamco. Dolor non in mollit qui aliqua cillum. Lorem dolore pariatur ipsum quis adipisicing. Ullamco laborum velit laborum duis quis laboris incididunt labore tempor nisi.Irure consequat ullamco ullamco minim quis tempor quis dolore ex cillum adipisicing duis non fugiat. Enim culpa aute excepteur eu sunt fugiat et amet Lorem. Consectetur eu veniam veniam duis cillum. Velit esse incididunt aliquip et.
Deserunt enim aliquip qui dolore laboris sit anim ea duis dolor. Occaecat mollit dolore incididunt dolor est tempor cupidatat ad reprehenderit anim incididunt laboris non do. Do excepteur et est id adipisicing laboris magna culpa proident officia irure aute. Commodo enim incididunt exercitation eu ut esse elit officia quis sunt adipisicing sint. Non anim magna culpa nisi veniam ut fugiat consectetur incididunt tempor. Occaecat Lorem proident anim mollit reprehenderit aute commodo sit ad esse irure dolore.",
        FechaPublicacion = new DateTime(2016, 01, 23, 03, 11, 07),
        FechaCreacion = new DateTime(2016, 01, 23, 03, 11, 07),
        CategoriaID = 4,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Dickerson Hanson Warner",
        Titulo = "Consequat dolor excepteur consequat adipisicing in occaecat non do tempor.",
        Descripcion = @"Esse laborum ex voluptate veniam irure laboris elit ut non culpa duis. Enim anim veniam ad excepteur eu esse tempor. Occaecat sunt est officia nisi ea qui sunt sunt dolor Lorem duis ea officia magna. Dolore eiusmod eiusmod enim reprehenderit ea culpa aliqua. Ad nulla voluptate sint aliqua incididunt excepteur mollit pariatur officia. Laborum labore proident sint culpa. Reprehenderit consectetur et reprehenderit non aliqua reprehenderit voluptate occaecat incididunt tempor.Sunt pariatur laboris cillum do do magna cupidatat velit commodo excepteur do mollit esse incididunt. Occaecat velit magna adipisicing nulla commodo deserunt est cupidatat laborum consectetur. Consequat officia nisi qui non laboris reprehenderit commodo eiusmod tempor labore ex duis qui. Incididunt in amet irure ipsum veniam deserunt. Veniam consectetur laboris qui Lorem fugiat deserunt nostrud.
Tempor dolor incididunt ut incididunt elit officia adipisicing laborum pariatur cillum. Adipisicing in et occaecat id. Lorem et nostrud reprehenderit id consectetur excepteur qui nisi qui. Sunt irure excepteur cupidatat veniam esse adipisicing id aute culpa Lorem. Incididunt ad est et tempor laborum duis nulla nostrud ut.",
        FechaPublicacion = new DateTime(2016, 01, 22, 02, 25, 12),
        FechaCreacion = new DateTime(2016, 01, 22, 02, 25, 12),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Delaney Mcgowan Pennington",
        Titulo = "Laborum veniam commodo dolor ut proident elit anim consequat nisi ea non irure id adipisicing.",
        Descripcion = @"Occaecat proident commodo eiusmod in ut do velit. Consectetur tempor dolor magna eiusmod. Ipsum ipsum non proident ad fugiat reprehenderit elit amet laboris anim incididunt qui consectetur. Aliqua id consequat magna nisi voluptate proident veniam sit ullamco. Aliqua cillum non irure minim eu eiusmod eiusmod sint officia non consequat culpa qui. Ut consequat magna est duis deserunt mollit reprehenderit ea in.Commodo duis sit labore laboris velit eu nostrud eiusmod mollit cupidatat officia laborum excepteur non. Nulla et irure deserunt culpa tempor eiusmod ullamco id. Ea ut ex in dolore sint culpa do laboris. Mollit aliquip qui irure id aute duis voluptate qui sint esse magna. Nostrud incididunt minim culpa cillum consequat exercitation adipisicing amet et dolor veniam laboris aute. Consequat ut adipisicing anim ipsum laborum nisi proident pariatur culpa. Nostrud voluptate aliqua fugiat occaecat Lorem.
Non est enim eu officia sit dolor minim aute magna consequat. Laborum ex fugiat magna dolore nulla eiusmod mollit aute enim esse. Aliquip elit mollit nostrud excepteur. Labore adipisicing velit ut est. Aute cillum aliqua sunt aliquip et ipsum magna.",
        FechaPublicacion = new DateTime(2016, 01, 02, 05, 46, 56),
        FechaCreacion = new DateTime(2016, 01, 02, 05, 46, 56),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Macias Pittman Wilkinson",
        Titulo = "Elit ut consequat sunt minim tempor commodo excepteur ad occaecat in laboris est quis.",
        Descripcion = @"Sunt ipsum anim labore adipisicing consectetur eiusmod. Esse veniam occaecat qui excepteur qui amet reprehenderit aute amet. Pariatur sunt reprehenderit anim enim qui et dolore Lorem. Dolore aliquip est aliquip excepteur irure excepteur eiusmod.Minim nulla est nulla et in mollit laboris labore magna laboris officia. Et in duis aute est sint elit sunt id. Reprehenderit nisi reprehenderit sit voluptate velit officia quis consectetur in. Duis sit velit ut consectetur. Ad quis eu occaecat qui. Cillum officia laboris irure voluptate duis ex reprehenderit ex aliqua.
Duis labore irure duis excepteur magna elit occaecat aliquip. Cupidatat do dolor esse non elit commodo cillum cillum occaecat. Sunt adipisicing cupidatat sit Lorem culpa. Ullamco dolor mollit quis cillum minim cupidatat aliquip sit ullamco. Irure Lorem nisi minim do.",
        FechaPublicacion = new DateTime(2016, 01, 05, 10, 36, 24),
        FechaCreacion = new DateTime(2016, 01, 05, 10, 36, 24),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
        },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Vickie Salas Sanchez",
        Titulo = "Reprehenderit consequat enim aliqua eiusmod exercitation ad minim laborum commodo sit.",
        Descripcion = @"Proident commodo nostrud ullamco est Lorem dolore proident veniam amet laboris. Laborum sint eu fugiat exercitation officia elit. Exercitation exercitation anim eu sint non consectetur magna laboris ullamco ex exercitation pariatur.Reprehenderit dolor et anim magna. Qui ut excepteur aliqua magna cupidatat dolore in aliqua Lorem. Reprehenderit exercitation minim adipisicing fugiat reprehenderit laboris minim. Deserunt anim occaecat ut excepteur labore veniam magna eu consequat.
Minim ex adipisicing tempor exercitation occaecat dolore veniam laboris qui aliquip cupidatat aliquip duis. Deserunt do quis cupidatat nostrud. Quis aliqua aliquip ex laborum sunt excepteur in. Ea nostrud Lorem id ad reprehenderit sunt excepteur laborum occaecat.",
        FechaPublicacion = new DateTime(2016, 01, 12, 02, 00, 20),
        FechaCreacion = new DateTime(2016, 01, 12, 02, 00, 20),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Alison Solis Pena",
        Titulo = "Ipsum exercitation fugiat laboris labore.",
        Descripcion = @"Sit proident occaecat esse commodo velit pariatur ex occaecat incididunt nulla magna mollit sunt. Amet cillum velit eu voluptate commodo deserunt veniam fugiat nulla magna sint dolor cupidatat. Aute consequat mollit excepteur in ad id mollit do. Cillum nisi adipisicing aliqua minim eu nostrud sit laboris eu ea ex dolore non proident.Ullamco tempor duis quis labore nostrud. Est sunt Lorem eiusmod Lorem enim ut fugiat velit. Sint dolor mollit duis esse exercitation irure sint officia voluptate. Id dolore adipisicing quis in. Consequat cupidatat nulla ut ea non proident minim culpa in. Aute aliquip occaecat ut do esse irure incididunt.
Cillum aliquip ex dolor duis commodo laborum nulla aliqua laboris laborum ut in. Aliquip et tempor fugiat culpa est. Lorem ullamco ipsum ex ut fugiat mollit est fugiat cupidatat non. Aute do sunt laboris nulla id laborum. Minim aute elit tempor nulla deserunt laboris quis irure labore cillum quis. Id ut irure ex cillum enim duis in do occaecat.",
        FechaPublicacion = new DateTime(2016, 01, 16, 06, 03, 43),
        FechaCreacion = new DateTime(2016, 01, 16, 06, 03, 43),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Bonita Mclean Hartman",
        Titulo = "Deserunt Lorem reprehenderit eiusmod excepteur sunt ea minim dolor et deserunt.",
        Descripcion = @"Do veniam exercitation ea incididunt occaecat. Adipisicing sunt id elit laborum ex amet. Quis dolor nisi non eiusmod amet qui esse pariatur elit ex. Minim minim aliqua duis minim minim consectetur tempor velit exercitation minim fugiat esse consectetur. Sit ad pariatur ex officia. Sint cillum cupidatat irure dolor eiusmod. Aliquip irure et irure magna nisi cillum fugiat incididunt id dolor mollit.Commodo proident minim anim ex excepteur excepteur veniam ex id ad in nisi duis. Officia fugiat reprehenderit proident commodo. Minim anim ut laborum aute nulla consectetur dolore.
Amet adipisicing exercitation et est sunt reprehenderit consectetur incididunt. Dolor laboris aliquip occaecat Lorem est elit. Laboris amet irure dolor ullamco magna aliquip id ad ad cillum amet aliqua. Consequat esse non et quis eu Lorem elit et labore. Qui mollit aliquip reprehenderit ex culpa laborum laborum. Est anim irure voluptate eu veniam aliquip do sint fugiat cupidatat aliquip.",
        FechaPublicacion = new DateTime(2016, 01, 17, 12, 59, 33),
        FechaCreacion = new DateTime(2016, 01, 17, 12, 59, 33),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Michael Gross Michael",
        Titulo = "Aliquip proident est tempor nostrud pariatur ullamco irure magna ut et et.",
        Descripcion = @"Sint aute cupidatat irure officia et sit. Ea labore elit consequat quis magna deserunt in ex ex labore eu sunt ex. Magna in dolor deserunt dolore ipsum dolor laboris tempor est non elit. Adipisicing occaecat quis voluptate non eiusmod nulla do ullamco fugiat cupidatat non exercitation anim deserunt. Tempor magna anim aute culpa qui cupidatat occaecat eu exercitation cillum est adipisicing. Esse aliqua quis est ullamco magna sint incididunt tempor ullamco mollit et.Tempor excepteur magna voluptate nulla nulla. Quis nisi aliquip ea Lorem ipsum velit minim amet consectetur ex nulla esse aliquip. Labore eu eu minim consectetur consectetur aliquip fugiat nostrud. Fugiat non nisi aute qui minim minim. Aute culpa veniam anim occaecat do ea quis irure. Non commodo cupidatat magna nisi sint nulla exercitation exercitation reprehenderit do mollit quis nulla nisi. Duis cillum dolor occaecat occaecat sit et tempor.
Occaecat veniam exercitation fugiat dolor eu elit commodo et et culpa enim quis. Aute mollit exercitation est aute ad amet sit. Dolore consectetur consectetur do officia est ullamco. Dolore et et adipisicing velit aliqua aliqua minim id. Aliquip officia commodo non anim velit deserunt velit laboris.",
        FechaPublicacion = new DateTime(2016, 01, 28, 12, 48, 46),
        FechaCreacion = new DateTime(2016, 01, 28, 12, 48, 46),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Beasley Glenn Nunez",
        Titulo = "Sunt aliquip exercitation velit commodo magna culpa occaecat qui labore exercitation.",
        Descripcion = @"Aute incididunt anim in magna commodo minim cupidatat magna voluptate velit. Cupidatat aliquip fugiat deserunt proident ipsum fugiat dolore ea culpa commodo. Dolore qui excepteur ut sit incididunt et et nostrud mollit ullamco. Minim incididunt fugiat exercitation voluptate amet et do adipisicing. Exercitation nulla sint anim qui sunt sit fugiat non est.Aute qui sit ad pariatur deserunt. Qui est cupidatat Lorem elit reprehenderit officia laboris. Aliquip ut id cupidatat nisi id duis. In anim adipisicing labore dolor elit anim nulla dolore nostrud excepteur. Culpa velit laboris voluptate exercitation. Commodo dolore sit duis duis occaecat pariatur velit et est aliquip do minim cupidatat pariatur. Mollit sint tempor officia dolor est.
Excepteur in excepteur sunt nostrud occaecat ipsum est magna duis ipsum nisi minim. Quis anim laboris proident eiusmod velit irure magna. Occaecat laboris laborum ex ad cillum elit tempor sit magna in. Elit Lorem enim irure cillum aliquip aliquip et minim. Incididunt non ad dolor aute. Exercitation qui proident occaecat id cupidatat amet magna laborum eu officia commodo culpa in qui. Eiusmod cupidatat mollit enim anim quis.",
        FechaPublicacion = new DateTime(2016, 01, 21, 07, 00, 23),
        FechaCreacion = new DateTime(2016, 01, 21, 07, 00, 23),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Simmons Donaldson Gallagher",
        Titulo = "Non ut amet ea occaecat est occaecat velit minim mollit minim eu.",
        Descripcion = @"Non est tempor tempor aliquip veniam do eiusmod id quis deserunt. Voluptate incididunt sit cupidatat ea ipsum aute in enim. Elit reprehenderit officia minim eu magna anim exercitation pariatur officia commodo tempor commodo culpa. Consectetur ullamco eiusmod non exercitation occaecat consequat aute elit do dolore ad in do. Velit magna est eiusmod ad aliqua esse et. Aliqua qui commodo reprehenderit cupidatat.Est ullamco sunt quis sint exercitation pariatur dolor. Voluptate aliqua non incididunt veniam fugiat reprehenderit commodo consequat laboris. Excepteur minim et est laborum pariatur esse fugiat do eu incididunt tempor. Labore commodo deserunt minim elit eu.
Anim consequat veniam proident ullamco pariatur id aute et consectetur in. Elit eiusmod minim nulla officia labore in reprehenderit aliquip mollit velit ut incididunt. Aliquip adipisicing qui exercitation proident amet quis ex anim nostrud elit sit excepteur aute. Duis voluptate reprehenderit quis minim labore velit id officia qui sit est commodo. Qui culpa fugiat consectetur nisi. Ex ullamco elit dolor irure occaecat enim. Anim commodo ipsum culpa officia sint do officia elit laboris commodo quis voluptate fugiat.",
        FechaPublicacion = new DateTime(2016, 01, 12, 01, 03, 27),
        FechaCreacion = new DateTime(2016, 01, 12, 01, 03, 27),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Vinson Powers Durham",
        Titulo = "Occaecat qui ipsum commodo dolor id in sint elit cillum in incididunt eu.",
        Descripcion = @"Ullamco do voluptate irure exercitation ut. Excepteur occaecat officia est amet voluptate qui adipisicing laborum labore exercitation ut in nisi. Consequat fugiat sunt dolore esse nostrud exercitation nulla mollit anim quis aliquip. Elit aute dolore dolore nostrud exercitation duis nostrud occaecat elit voluptate pariatur do. Excepteur eu tempor anim occaecat ut exercitation ipsum nisi. Excepteur tempor esse incididunt cupidatat.In culpa voluptate ad elit. Est eiusmod ullamco aliqua ea Lorem pariatur. Occaecat eiusmod sint qui in ea.
Labore eu et ipsum in commodo reprehenderit. Fugiat velit non nostrud voluptate nostrud cupidatat anim est sint officia anim consectetur aliqua. Irure adipisicing ea adipisicing ut enim veniam laborum reprehenderit officia. Culpa ex Lorem amet ipsum sit amet pariatur minim nulla veniam.",
        FechaPublicacion = new DateTime(2016, 01, 24, 10, 53, 15),
        FechaCreacion = new DateTime(2016, 01, 24, 10, 53, 15),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Johnston Farrell Santiago",
        Titulo = "Aute ullamco tempor eu in id tempor irure mollit incididunt minim qui culpa.",
        Descripcion = @"Voluptate ipsum adipisicing adipisicing deserunt ex culpa adipisicing. Dolore cupidatat culpa anim dolore voluptate consequat ad deserunt eu pariatur consequat eiusmod adipisicing. Magna mollit minim eiusmod sint laboris aliquip velit ea sunt do voluptate.Officia tempor ad mollit ex eu laborum ipsum ullamco et dolor nisi cillum. Ullamco sit nostrud enim irure enim. Amet sunt duis reprehenderit labore excepteur anim enim quis. Officia duis amet ad elit consequat elit laboris. Nisi velit culpa elit adipisicing nisi est anim cupidatat.
Incididunt et sit ipsum eiusmod est Lorem cupidatat esse Lorem sit elit ullamco. Mollit est sunt cupidatat aliqua minim irure sunt mollit veniam enim anim officia. Elit culpa minim ea ipsum est Lorem magna duis mollit voluptate reprehenderit irure. Ex Lorem veniam tempor in.",
        FechaPublicacion = new DateTime(2016, 01, 08, 09, 25, 42),
        FechaCreacion = new DateTime(2016, 01, 08, 09, 25, 42),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Kline Hale Knox",
        Titulo = "Duis incididunt veniam consectetur ex.",
        Descripcion = @"Reprehenderit qui aute id veniam sint cillum ipsum magna reprehenderit anim occaecat elit proident. Ex sint officia cupidatat aliqua aliqua velit ea ipsum ut occaecat. Ad pariatur nulla commodo in ea dolor culpa enim sit ad consequat amet. Cupidatat excepteur ad sunt duis exercitation reprehenderit voluptate ipsum ipsum sint duis tempor. Amet eu sint esse duis aliqua exercitation incididunt ea sit consequat ipsum laboris fugiat aliquip. Eu Lorem do eiusmod et. Reprehenderit officia amet aliqua sunt amet ea.Ex laborum ea aliquip magna. Adipisicing elit consequat duis eu proident nostrud occaecat enim irure. Ea id occaecat pariatur ut proident. Nisi voluptate labore dolor laborum ea incididunt et exercitation. Consequat ipsum incididunt ipsum in dolor. Irure aliquip nostrud reprehenderit cupidatat mollit minim aliqua magna laborum.
Incididunt excepteur dolore in elit esse proident quis. Cillum anim in aute voluptate ex eiusmod exercitation esse cupidatat fugiat. Dolor ex nisi exercitation adipisicing tempor fugiat nulla aute proident amet deserunt irure. Dolore aliqua ex nisi quis proident dolor. Laboris officia commodo fugiat exercitation dolore do est ullamco est.",
        FechaPublicacion = new DateTime(2016, 01, 14, 02, 48, 56),
        FechaCreacion = new DateTime(2016, 01, 14, 02, 48, 56),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Mathis Ingram Mckee",
        Titulo = "In aliquip amet irure consequat sint minim commodo aliquip.",
        Descripcion = @"Pariatur duis Lorem voluptate non et labore ipsum commodo laborum aute. Duis do culpa excepteur et in dolore laboris eiusmod occaecat laborum sunt ad. Officia excepteur aliquip labore commodo cillum ad aliquip irure. Consectetur culpa exercitation sunt veniam consequat.Et dolor nostrud laboris non excepteur. In aute culpa fugiat officia nisi proident ut. Lorem ex id minim duis deserunt occaecat magna velit cillum aliquip enim. Eu sunt occaecat veniam minim nisi culpa magna. Do magna consectetur sunt et id eu voluptate irure nostrud. Nisi id exercitation cillum esse nulla ex non occaecat tempor excepteur aute.
Cupidatat aliqua velit non magna laborum aliqua ullamco. Culpa non dolore Lorem sit labore ullamco cupidatat nisi. Laborum deserunt consectetur adipisicing eiusmod aute et tempor deserunt proident veniam. Consequat nisi in nulla ut quis cupidatat cupidatat sunt non duis. Veniam mollit exercitation minim fugiat proident ex qui. Occaecat enim minim deserunt consectetur voluptate irure voluptate ullamco incididunt Lorem cillum elit. Nulla nisi officia aliqua et dolore cupidatat non occaecat exercitation deserunt adipisicing minim sit nulla.",
        FechaPublicacion = new DateTime(2016, 01, 21, 04, 45, 02),
        FechaCreacion = new DateTime(2016, 01, 21, 04, 45, 02),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Mcmahon Gibson Branch",
        Titulo = "Consectetur eiusmod ex officia duis.",
        Descripcion = @"Occaecat occaecat aute excepteur do reprehenderit nostrud cupidatat. Magna laborum amet tempor officia consectetur. In aute exercitation dolore ea nostrud nulla incididunt. Eiusmod et in pariatur sint sunt culpa nostrud mollit. Ea laborum est commodo magna culpa sint magna labore sunt.Aute non aliqua culpa mollit mollit do eiusmod eiusmod adipisicing sunt sint adipisicing. Fugiat non labore tempor occaecat aliquip sint non ullamco minim consequat cupidatat reprehenderit. Voluptate non proident voluptate pariatur deserunt.
Duis enim nisi id ut Lorem ut qui ipsum mollit ut pariatur aute ex ea. Sint ea occaecat ad deserunt culpa nulla reprehenderit nostrud ad tempor. Enim eiusmod aute elit enim labore aute laborum incididunt occaecat do est elit aliquip incididunt. Sit Lorem sunt irure ipsum voluptate deserunt esse cillum culpa Lorem esse. Adipisicing amet mollit dolore elit. Irure veniam aliqua sunt est labore qui et elit esse ad labore ea.",
        FechaPublicacion = new DateTime(2016, 01, 08, 11, 19, 49),
        FechaCreacion = new DateTime(2016, 01, 08, 11, 19, 49),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Day Valenzuela Blackburn",
        Titulo = "Lorem do do aliquip eiusmod deserunt amet commodo laborum.",
        Descripcion = @"In esse ad do commodo non aliqua esse laboris sint cillum do ea. Non dolore laboris proident adipisicing duis consectetur consequat eu sit mollit deserunt. Consequat excepteur incididunt et nostrud reprehenderit. Commodo magna nisi ut exercitation incididunt esse eu tempor. Aliquip ad velit do fugiat Lorem irure nisi id amet.Velit aute minim aliquip do. Dolor eiusmod ad anim laboris consectetur consectetur eiusmod exercitation mollit officia ea. Esse irure laboris ut ad laborum deserunt pariatur occaecat ullamco elit esse in culpa. Sunt deserunt minim nulla pariatur reprehenderit.
Pariatur aliquip qui magna consectetur cupidatat sit officia eu dolor cillum consequat. Adipisicing Lorem aliquip laboris eiusmod quis aliquip Lorem. Dolore cillum minim adipisicing minim.",
        FechaPublicacion = new DateTime(2016, 01, 08, 06, 54, 39),
        FechaCreacion = new DateTime(2016, 01, 08, 06, 54, 39),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion
    {
        Usuario = editor2,
        Autor = "Carol Nicholson Gonzalez",
        Titulo = "Eu nostrud pariatur ad consequat minim.",
        Descripcion = @"Occaecat sint reprehenderit mollit officia magna est Lorem occaecat cupidatat nostrud. Eiusmod sint ea laboris esse ut do pariatur amet. Laborum sit minim sit eiusmod excepteur labore deserunt ea qui aliqua occaecat ea quis fugiat. Ad est sint aliquip laboris do laboris deserunt pariatur ut. Excepteur sint nulla pariatur adipisicing aliqua. Elit sit voluptate adipisicing magna anim ipsum aute duis quis duis aute exercitation.Anim fugiat fugiat aliqua eiusmod elit excepteur irure aliquip cillum amet ullamco irure deserunt amet. Mollit sunt labore proident mollit do ex sint tempor sint aliqua. Id cillum et eiusmod non. Irure amet tempor nisi ex esse est dolore. Deserunt enim elit reprehenderit nisi id irure exercitation. Nisi voluptate velit ad officia qui laboris qui amet ipsum anim incididunt magna anim qui. Dolor labore fugiat non et est.
Elit est ut fugiat nulla enim qui est nulla aute. Laboris sit exercitation incididunt veniam. Deserunt incididunt nostrud cillum incididunt elit qui consectetur minim enim. Excepteur enim Lorem ea ea fugiat duis enim enim aliquip.",
        FechaPublicacion = new DateTime(2016, 01, 26, 10, 56, 24),
        FechaCreacion = new DateTime(2016, 01, 26, 10, 56, 24),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion
    {
        Usuario = editor2,
        Autor = "King Benjamin Hughes",
        Titulo = "Ea officia cupidatat mollit occaecat aliqua ea.",
        Descripcion = @"Voluptate elit eu eiusmod proident nulla anim aliqua incididunt irure magna velit. Magna consequat do exercitation veniam duis elit dolor eu occaecat. Consequat ex est ex nisi elit enim ullamco irure duis amet elit. Ad deserunt anim ipsum fugiat aliqua. Voluptate id laborum consequat consequat exercitation amet reprehenderit nulla laborum ipsum. Esse in proident duis occaecat cillum ex consequat commodo aliqua. Non esse sit ad mollit laboris aliqua ex ex et eiusmod enim sit nisi.Eiusmod nisi officia sint culpa laborum non cupidatat proident adipisicing tempor enim. Esse magna deserunt fugiat duis. Nostrud excepteur veniam ullamco esse fugiat amet adipisicing exercitation sit est deserunt irure adipisicing. Nisi in incididunt incididunt ipsum do.
Officia ex anim fugiat do cillum. Id non non ullamco ut incididunt. Sunt irure dolore minim duis dolor esse excepteur ut laborum do irure occaecat exercitation sunt.",
        FechaPublicacion = new DateTime(2016, 01, 08, 01, 20, 58),
        FechaCreacion = new DateTime(2016, 01, 08, 01, 20, 58),
        CategoriaID = 6,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Kristina Shaffer Rios",
        Titulo = "Excepteur aliqua et exercitation dolor.",
        Descripcion = @"Veniam minim proident tempor irure et eiusmod in. Sint ut ipsum ullamco dolore aute eiusmod minim quis mollit amet nulla. Labore ullamco sunt ullamco est nulla excepteur consequat non sunt labore ut. Minim non mollit enim elit ex. Exercitation reprehenderit Lorem velit deserunt cillum ut cillum voluptate amet enim deserunt sit id. Ut reprehenderit nisi sit sit sit excepteur. Pariatur ea ea minim veniam aliquip.Consectetur ipsum irure consectetur veniam tempor deserunt quis aliqua eu ad magna ea eu mollit. Fugiat sint magna voluptate eu veniam ut nostrud adipisicing quis aliquip quis. Id incididunt incididunt veniam consectetur in nostrud nisi ullamco adipisicing in sit elit. Non aute amet exercitation fugiat ullamco. Non consequat sunt dolore esse excepteur consectetur excepteur ex nostrud reprehenderit id anim non. Velit duis labore elit pariatur.
Anim voluptate nulla duis veniam ea culpa proident officia mollit est. Veniam ex laborum commodo officia ex consequat ullamco est elit cillum. Laboris aliqua culpa voluptate ut. Nisi eiusmod ad elit sit ullamco veniam minim enim magna pariatur labore non. Do eu sint ad ut commodo culpa commodo exercitation proident sunt anim. Eiusmod enim Lorem non nulla laborum amet anim eiusmod sunt aliquip nulla aute et pariatur.",
        FechaPublicacion = new DateTime(2016, 01, 13, 09, 36, 13),
        FechaCreacion = new DateTime(2016, 01, 13, 09, 36, 13),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Perry Colon Kaufman",
        Titulo = "Pariatur in in ullamco ipsum culpa cillum aliqua commodo ex.",
        Descripcion = @"Aliquip reprehenderit id velit ex mollit irure id incididunt consectetur proident commodo amet sunt. Sunt id mollit ipsum qui laborum culpa magna dolor aliquip incididunt laboris qui cillum. Ad esse non mollit officia occaecat sit aliquip excepteur.Non eu exercitation eiusmod aliquip voluptate exercitation voluptate elit enim ex velit fugiat sint. Et culpa ad dolor nulla tempor ut in cillum ullamco enim nostrud cupidatat do. Deserunt labore magna fugiat consequat do qui id.
Culpa aliquip qui enim elit est. Ad Lorem consectetur amet cillum et consequat elit Lorem adipisicing. Non culpa voluptate non ad eiusmod nulla anim. Ea commodo magna veniam velit qui proident reprehenderit labore ipsum ut ex. Irure ex exercitation dolore amet est et labore mollit pariatur adipisicing velit ad laboris.",
        FechaPublicacion = new DateTime(2016, 01, 16, 02, 15, 03),
        FechaCreacion = new DateTime(2016, 01, 16, 02, 15, 03),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Gomez Sherman Morgan",
        Titulo = "Adipisicing ea ex consectetur reprehenderit veniam officia ex sunt ipsum.",
        Descripcion = @"Qui ad adipisicing eu veniam. Minim minim laborum dolor mollit nisi eiusmod amet id. Eiusmod irure consequat ea elit mollit et tempor sit reprehenderit pariatur excepteur nulla reprehenderit cupidatat. Nostrud labore quis consectetur Lorem proident aute officia duis nostrud ullamco. Dolor non in mollit qui aliqua cillum. Lorem dolore pariatur ipsum quis adipisicing. Ullamco laborum velit laborum duis quis laboris incididunt labore tempor nisi.Irure consequat ullamco ullamco minim quis tempor quis dolore ex cillum adipisicing duis non fugiat. Enim culpa aute excepteur eu sunt fugiat et amet Lorem. Consectetur eu veniam veniam duis cillum. Velit esse incididunt aliquip et.
Deserunt enim aliquip qui dolore laboris sit anim ea duis dolor. Occaecat mollit dolore incididunt dolor est tempor cupidatat ad reprehenderit anim incididunt laboris non do. Do excepteur et est id adipisicing laboris magna culpa proident officia irure aute. Commodo enim incididunt exercitation eu ut esse elit officia quis sunt adipisicing sint. Non anim magna culpa nisi veniam ut fugiat consectetur incididunt tempor. Occaecat Lorem proident anim mollit reprehenderit aute commodo sit ad esse irure dolore.",
        FechaPublicacion = new DateTime(2016, 01, 23, 03, 11, 07),
        FechaCreacion = new DateTime(2016, 01, 23, 03, 11, 07),
        CategoriaID = 4,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Dickerson Hanson Warner",
        Titulo = "Consequat dolor excepteur consequat adipisicing in occaecat non do tempor.",
        Descripcion = @"Esse laborum ex voluptate veniam irure laboris elit ut non culpa duis. Enim anim veniam ad excepteur eu esse tempor. Occaecat sunt est officia nisi ea qui sunt sunt dolor Lorem duis ea officia magna. Dolore eiusmod eiusmod enim reprehenderit ea culpa aliqua. Ad nulla voluptate sint aliqua incididunt excepteur mollit pariatur officia. Laborum labore proident sint culpa. Reprehenderit consectetur et reprehenderit non aliqua reprehenderit voluptate occaecat incididunt tempor.Sunt pariatur laboris cillum do do magna cupidatat velit commodo excepteur do mollit esse incididunt. Occaecat velit magna adipisicing nulla commodo deserunt est cupidatat laborum consectetur. Consequat officia nisi qui non laboris reprehenderit commodo eiusmod tempor labore ex duis qui. Incididunt in amet irure ipsum veniam deserunt. Veniam consectetur laboris qui Lorem fugiat deserunt nostrud.
Tempor dolor incididunt ut incididunt elit officia adipisicing laborum pariatur cillum. Adipisicing in et occaecat id. Lorem et nostrud reprehenderit id consectetur excepteur qui nisi qui. Sunt irure excepteur cupidatat veniam esse adipisicing id aute culpa Lorem. Incididunt ad est et tempor laborum duis nulla nostrud ut.",
        FechaPublicacion = new DateTime(2016, 01, 22, 02, 25, 12),
        FechaCreacion = new DateTime(2016, 01, 22, 02, 25, 12),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Delaney Mcgowan Pennington",
        Titulo = "Laborum veniam commodo dolor ut proident elit anim consequat nisi ea non irure id adipisicing.",
        Descripcion = @"Occaecat proident commodo eiusmod in ut do velit. Consectetur tempor dolor magna eiusmod. Ipsum ipsum non proident ad fugiat reprehenderit elit amet laboris anim incididunt qui consectetur. Aliqua id consequat magna nisi voluptate proident veniam sit ullamco. Aliqua cillum non irure minim eu eiusmod eiusmod sint officia non consequat culpa qui. Ut consequat magna est duis deserunt mollit reprehenderit ea in.Commodo duis sit labore laboris velit eu nostrud eiusmod mollit cupidatat officia laborum excepteur non. Nulla et irure deserunt culpa tempor eiusmod ullamco id. Ea ut ex in dolore sint culpa do laboris. Mollit aliquip qui irure id aute duis voluptate qui sint esse magna. Nostrud incididunt minim culpa cillum consequat exercitation adipisicing amet et dolor veniam laboris aute. Consequat ut adipisicing anim ipsum laborum nisi proident pariatur culpa. Nostrud voluptate aliqua fugiat occaecat Lorem.
Non est enim eu officia sit dolor minim aute magna consequat. Laborum ex fugiat magna dolore nulla eiusmod mollit aute enim esse. Aliquip elit mollit nostrud excepteur. Labore adipisicing velit ut est. Aute cillum aliqua sunt aliquip et ipsum magna.",
        FechaPublicacion = new DateTime(2016, 01, 02, 05, 46, 56),
        FechaCreacion = new DateTime(2016, 01, 02, 05, 46, 56),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Macias Pittman Wilkinson",
        Titulo = "Elit ut consequat sunt minim tempor commodo excepteur ad occaecat in laboris est quis.",
        Descripcion = @"Sunt ipsum anim labore adipisicing consectetur eiusmod. Esse veniam occaecat qui excepteur qui amet reprehenderit aute amet. Pariatur sunt reprehenderit anim enim qui et dolore Lorem. Dolore aliquip est aliquip excepteur irure excepteur eiusmod.Minim nulla est nulla et in mollit laboris labore magna laboris officia. Et in duis aute est sint elit sunt id. Reprehenderit nisi reprehenderit sit voluptate velit officia quis consectetur in. Duis sit velit ut consectetur. Ad quis eu occaecat qui. Cillum officia laboris irure voluptate duis ex reprehenderit ex aliqua.
Duis labore irure duis excepteur magna elit occaecat aliquip. Cupidatat do dolor esse non elit commodo cillum cillum occaecat. Sunt adipisicing cupidatat sit Lorem culpa. Ullamco dolor mollit quis cillum minim cupidatat aliquip sit ullamco. Irure Lorem nisi minim do.",
        FechaPublicacion = new DateTime(2016, 01, 05, 10, 36, 24),
        FechaCreacion = new DateTime(2016, 01, 05, 10, 36, 24),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
        },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Vickie Salas Sanchez",
        Titulo = "Reprehenderit consequat enim aliqua eiusmod exercitation ad minim laborum commodo sit.",
        Descripcion = @"Proident commodo nostrud ullamco est Lorem dolore proident veniam amet laboris. Laborum sint eu fugiat exercitation officia elit. Exercitation exercitation anim eu sint non consectetur magna laboris ullamco ex exercitation pariatur.Reprehenderit dolor et anim magna. Qui ut excepteur aliqua magna cupidatat dolore in aliqua Lorem. Reprehenderit exercitation minim adipisicing fugiat reprehenderit laboris minim. Deserunt anim occaecat ut excepteur labore veniam magna eu consequat.
Minim ex adipisicing tempor exercitation occaecat dolore veniam laboris qui aliquip cupidatat aliquip duis. Deserunt do quis cupidatat nostrud. Quis aliqua aliquip ex laborum sunt excepteur in. Ea nostrud Lorem id ad reprehenderit sunt excepteur laborum occaecat.",
        FechaPublicacion = new DateTime(2016, 01, 12, 02, 00, 20),
        FechaCreacion = new DateTime(2016, 01, 12, 02, 00, 20),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Alison Solis Pena",
        Titulo = "Ipsum exercitation fugiat laboris labore.",
        Descripcion = @"Sit proident occaecat esse commodo velit pariatur ex occaecat incididunt nulla magna mollit sunt. Amet cillum velit eu voluptate commodo deserunt veniam fugiat nulla magna sint dolor cupidatat. Aute consequat mollit excepteur in ad id mollit do. Cillum nisi adipisicing aliqua minim eu nostrud sit laboris eu ea ex dolore non proident.Ullamco tempor duis quis labore nostrud. Est sunt Lorem eiusmod Lorem enim ut fugiat velit. Sint dolor mollit duis esse exercitation irure sint officia voluptate. Id dolore adipisicing quis in. Consequat cupidatat nulla ut ea non proident minim culpa in. Aute aliquip occaecat ut do esse irure incididunt.
Cillum aliquip ex dolor duis commodo laborum nulla aliqua laboris laborum ut in. Aliquip et tempor fugiat culpa est. Lorem ullamco ipsum ex ut fugiat mollit est fugiat cupidatat non. Aute do sunt laboris nulla id laborum. Minim aute elit tempor nulla deserunt laboris quis irure labore cillum quis. Id ut irure ex cillum enim duis in do occaecat.",
        FechaPublicacion = new DateTime(2016, 01, 16, 06, 03, 43),
        FechaCreacion = new DateTime(2016, 01, 16, 06, 03, 43),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Bonita Mclean Hartman",
        Titulo = "Deserunt Lorem reprehenderit eiusmod excepteur sunt ea minim dolor et deserunt.",
        Descripcion = @"Do veniam exercitation ea incididunt occaecat. Adipisicing sunt id elit laborum ex amet. Quis dolor nisi non eiusmod amet qui esse pariatur elit ex. Minim minim aliqua duis minim minim consectetur tempor velit exercitation minim fugiat esse consectetur. Sit ad pariatur ex officia. Sint cillum cupidatat irure dolor eiusmod. Aliquip irure et irure magna nisi cillum fugiat incididunt id dolor mollit.Commodo proident minim anim ex excepteur excepteur veniam ex id ad in nisi duis. Officia fugiat reprehenderit proident commodo. Minim anim ut laborum aute nulla consectetur dolore.
Amet adipisicing exercitation et est sunt reprehenderit consectetur incididunt. Dolor laboris aliquip occaecat Lorem est elit. Laboris amet irure dolor ullamco magna aliquip id ad ad cillum amet aliqua. Consequat esse non et quis eu Lorem elit et labore. Qui mollit aliquip reprehenderit ex culpa laborum laborum. Est anim irure voluptate eu veniam aliquip do sint fugiat cupidatat aliquip.",
        FechaPublicacion = new DateTime(2016, 01, 17, 12, 59, 33),
        FechaCreacion = new DateTime(2016, 01, 17, 12, 59, 33),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Michael Gross Michael",
        Titulo = "Aliquip proident est tempor nostrud pariatur ullamco irure magna ut et et.",
        Descripcion = @"Sint aute cupidatat irure officia et sit. Ea labore elit consequat quis magna deserunt in ex ex labore eu sunt ex. Magna in dolor deserunt dolore ipsum dolor laboris tempor est non elit. Adipisicing occaecat quis voluptate non eiusmod nulla do ullamco fugiat cupidatat non exercitation anim deserunt. Tempor magna anim aute culpa qui cupidatat occaecat eu exercitation cillum est adipisicing. Esse aliqua quis est ullamco magna sint incididunt tempor ullamco mollit et.Tempor excepteur magna voluptate nulla nulla. Quis nisi aliquip ea Lorem ipsum velit minim amet consectetur ex nulla esse aliquip. Labore eu eu minim consectetur consectetur aliquip fugiat nostrud. Fugiat non nisi aute qui minim minim. Aute culpa veniam anim occaecat do ea quis irure. Non commodo cupidatat magna nisi sint nulla exercitation exercitation reprehenderit do mollit quis nulla nisi. Duis cillum dolor occaecat occaecat sit et tempor.
Occaecat veniam exercitation fugiat dolor eu elit commodo et et culpa enim quis. Aute mollit exercitation est aute ad amet sit. Dolore consectetur consectetur do officia est ullamco. Dolore et et adipisicing velit aliqua aliqua minim id. Aliquip officia commodo non anim velit deserunt velit laboris.",
        FechaPublicacion = new DateTime(2016, 01, 28, 12, 48, 46),
        FechaCreacion = new DateTime(2016, 01, 28, 12, 48, 46),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Beasley Glenn Nunez",
        Titulo = "Sunt aliquip exercitation velit commodo magna culpa occaecat qui labore exercitation.",
        Descripcion = @"Aute incididunt anim in magna commodo minim cupidatat magna voluptate velit. Cupidatat aliquip fugiat deserunt proident ipsum fugiat dolore ea culpa commodo. Dolore qui excepteur ut sit incididunt et et nostrud mollit ullamco. Minim incididunt fugiat exercitation voluptate amet et do adipisicing. Exercitation nulla sint anim qui sunt sit fugiat non est.Aute qui sit ad pariatur deserunt. Qui est cupidatat Lorem elit reprehenderit officia laboris. Aliquip ut id cupidatat nisi id duis. In anim adipisicing labore dolor elit anim nulla dolore nostrud excepteur. Culpa velit laboris voluptate exercitation. Commodo dolore sit duis duis occaecat pariatur velit et est aliquip do minim cupidatat pariatur. Mollit sint tempor officia dolor est.
Excepteur in excepteur sunt nostrud occaecat ipsum est magna duis ipsum nisi minim. Quis anim laboris proident eiusmod velit irure magna. Occaecat laboris laborum ex ad cillum elit tempor sit magna in. Elit Lorem enim irure cillum aliquip aliquip et minim. Incididunt non ad dolor aute. Exercitation qui proident occaecat id cupidatat amet magna laborum eu officia commodo culpa in qui. Eiusmod cupidatat mollit enim anim quis.",
        FechaPublicacion = new DateTime(2016, 01, 21, 07, 00, 23),
        FechaCreacion = new DateTime(2016, 01, 21, 07, 00, 23),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Simmons Donaldson Gallagher",
        Titulo = "Non ut amet ea occaecat est occaecat velit minim mollit minim eu.",
        Descripcion = @"Non est tempor tempor aliquip veniam do eiusmod id quis deserunt. Voluptate incididunt sit cupidatat ea ipsum aute in enim. Elit reprehenderit officia minim eu magna anim exercitation pariatur officia commodo tempor commodo culpa. Consectetur ullamco eiusmod non exercitation occaecat consequat aute elit do dolore ad in do. Velit magna est eiusmod ad aliqua esse et. Aliqua qui commodo reprehenderit cupidatat.Est ullamco sunt quis sint exercitation pariatur dolor. Voluptate aliqua non incididunt veniam fugiat reprehenderit commodo consequat laboris. Excepteur minim et est laborum pariatur esse fugiat do eu incididunt tempor. Labore commodo deserunt minim elit eu.
Anim consequat veniam proident ullamco pariatur id aute et consectetur in. Elit eiusmod minim nulla officia labore in reprehenderit aliquip mollit velit ut incididunt. Aliquip adipisicing qui exercitation proident amet quis ex anim nostrud elit sit excepteur aute. Duis voluptate reprehenderit quis minim labore velit id officia qui sit est commodo. Qui culpa fugiat consectetur nisi. Ex ullamco elit dolor irure occaecat enim. Anim commodo ipsum culpa officia sint do officia elit laboris commodo quis voluptate fugiat.",
        FechaPublicacion = new DateTime(2016, 01, 12, 01, 03, 27),
        FechaCreacion = new DateTime(2016, 01, 12, 01, 03, 27),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Vinson Powers Durham",
        Titulo = "Occaecat qui ipsum commodo dolor id in sint elit cillum in incididunt eu.",
        Descripcion = @"Ullamco do voluptate irure exercitation ut. Excepteur occaecat officia est amet voluptate qui adipisicing laborum labore exercitation ut in nisi. Consequat fugiat sunt dolore esse nostrud exercitation nulla mollit anim quis aliquip. Elit aute dolore dolore nostrud exercitation duis nostrud occaecat elit voluptate pariatur do. Excepteur eu tempor anim occaecat ut exercitation ipsum nisi. Excepteur tempor esse incididunt cupidatat.In culpa voluptate ad elit. Est eiusmod ullamco aliqua ea Lorem pariatur. Occaecat eiusmod sint qui in ea.
Labore eu et ipsum in commodo reprehenderit. Fugiat velit non nostrud voluptate nostrud cupidatat anim est sint officia anim consectetur aliqua. Irure adipisicing ea adipisicing ut enim veniam laborum reprehenderit officia. Culpa ex Lorem amet ipsum sit amet pariatur minim nulla veniam.",
        FechaPublicacion = new DateTime(2016, 01, 24, 10, 53, 15),
        FechaCreacion = new DateTime(2016, 01, 24, 10, 53, 15),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Johnston Farrell Santiago",
        Titulo = "Aute ullamco tempor eu in id tempor irure mollit incididunt minim qui culpa.",
        Descripcion = @"Voluptate ipsum adipisicing adipisicing deserunt ex culpa adipisicing. Dolore cupidatat culpa anim dolore voluptate consequat ad deserunt eu pariatur consequat eiusmod adipisicing. Magna mollit minim eiusmod sint laboris aliquip velit ea sunt do voluptate.Officia tempor ad mollit ex eu laborum ipsum ullamco et dolor nisi cillum. Ullamco sit nostrud enim irure enim. Amet sunt duis reprehenderit labore excepteur anim enim quis. Officia duis amet ad elit consequat elit laboris. Nisi velit culpa elit adipisicing nisi est anim cupidatat.
Incididunt et sit ipsum eiusmod est Lorem cupidatat esse Lorem sit elit ullamco. Mollit est sunt cupidatat aliqua minim irure sunt mollit veniam enim anim officia. Elit culpa minim ea ipsum est Lorem magna duis mollit voluptate reprehenderit irure. Ex Lorem veniam tempor in.",
        FechaPublicacion = new DateTime(2016, 01, 08, 09, 25, 42),
        FechaCreacion = new DateTime(2016, 01, 08, 09, 25, 42),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Kline Hale Knox",
        Titulo = "Duis incididunt veniam consectetur ex.",
        Descripcion = @"Reprehenderit qui aute id veniam sint cillum ipsum magna reprehenderit anim occaecat elit proident. Ex sint officia cupidatat aliqua aliqua velit ea ipsum ut occaecat. Ad pariatur nulla commodo in ea dolor culpa enim sit ad consequat amet. Cupidatat excepteur ad sunt duis exercitation reprehenderit voluptate ipsum ipsum sint duis tempor. Amet eu sint esse duis aliqua exercitation incididunt ea sit consequat ipsum laboris fugiat aliquip. Eu Lorem do eiusmod et. Reprehenderit officia amet aliqua sunt amet ea.Ex laborum ea aliquip magna. Adipisicing elit consequat duis eu proident nostrud occaecat enim irure. Ea id occaecat pariatur ut proident. Nisi voluptate labore dolor laborum ea incididunt et exercitation. Consequat ipsum incididunt ipsum in dolor. Irure aliquip nostrud reprehenderit cupidatat mollit minim aliqua magna laborum.
Incididunt excepteur dolore in elit esse proident quis. Cillum anim in aute voluptate ex eiusmod exercitation esse cupidatat fugiat. Dolor ex nisi exercitation adipisicing tempor fugiat nulla aute proident amet deserunt irure. Dolore aliqua ex nisi quis proident dolor. Laboris officia commodo fugiat exercitation dolore do est ullamco est.",
        FechaPublicacion = new DateTime(2016, 01, 14, 02, 48, 56),
        FechaCreacion = new DateTime(2016, 01, 14, 02, 48, 56),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Mathis Ingram Mckee",
        Titulo = "In aliquip amet irure consequat sint minim commodo aliquip.",
        Descripcion = @"Pariatur duis Lorem voluptate non et labore ipsum commodo laborum aute. Duis do culpa excepteur et in dolore laboris eiusmod occaecat laborum sunt ad. Officia excepteur aliquip labore commodo cillum ad aliquip irure. Consectetur culpa exercitation sunt veniam consequat.Et dolor nostrud laboris non excepteur. In aute culpa fugiat officia nisi proident ut. Lorem ex id minim duis deserunt occaecat magna velit cillum aliquip enim. Eu sunt occaecat veniam minim nisi culpa magna. Do magna consectetur sunt et id eu voluptate irure nostrud. Nisi id exercitation cillum esse nulla ex non occaecat tempor excepteur aute.
Cupidatat aliqua velit non magna laborum aliqua ullamco. Culpa non dolore Lorem sit labore ullamco cupidatat nisi. Laborum deserunt consectetur adipisicing eiusmod aute et tempor deserunt proident veniam. Consequat nisi in nulla ut quis cupidatat cupidatat sunt non duis. Veniam mollit exercitation minim fugiat proident ex qui. Occaecat enim minim deserunt consectetur voluptate irure voluptate ullamco incididunt Lorem cillum elit. Nulla nisi officia aliqua et dolore cupidatat non occaecat exercitation deserunt adipisicing minim sit nulla.",
        FechaPublicacion = new DateTime(2016, 01, 21, 04, 45, 02),
        FechaCreacion = new DateTime(2016, 01, 21, 04, 45, 02),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Mcmahon Gibson Branch",
        Titulo = "Consectetur eiusmod ex officia duis.",
        Descripcion = @"Occaecat occaecat aute excepteur do reprehenderit nostrud cupidatat. Magna laborum amet tempor officia consectetur. In aute exercitation dolore ea nostrud nulla incididunt. Eiusmod et in pariatur sint sunt culpa nostrud mollit. Ea laborum est commodo magna culpa sint magna labore sunt.Aute non aliqua culpa mollit mollit do eiusmod eiusmod adipisicing sunt sint adipisicing. Fugiat non labore tempor occaecat aliquip sint non ullamco minim consequat cupidatat reprehenderit. Voluptate non proident voluptate pariatur deserunt.
Duis enim nisi id ut Lorem ut qui ipsum mollit ut pariatur aute ex ea. Sint ea occaecat ad deserunt culpa nulla reprehenderit nostrud ad tempor. Enim eiusmod aute elit enim labore aute laborum incididunt occaecat do est elit aliquip incididunt. Sit Lorem sunt irure ipsum voluptate deserunt esse cillum culpa Lorem esse. Adipisicing amet mollit dolore elit. Irure veniam aliqua sunt est labore qui et elit esse ad labore ea.",
        FechaPublicacion = new DateTime(2016, 01, 08, 11, 19, 49),
        FechaCreacion = new DateTime(2016, 01, 08, 11, 19, 49),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Day Valenzuela Blackburn",
        Titulo = "Lorem do do aliquip eiusmod deserunt amet commodo laborum.",
        Descripcion = @"In esse ad do commodo non aliqua esse laboris sint cillum do ea. Non dolore laboris proident adipisicing duis consectetur consequat eu sit mollit deserunt. Consequat excepteur incididunt et nostrud reprehenderit. Commodo magna nisi ut exercitation incididunt esse eu tempor. Aliquip ad velit do fugiat Lorem irure nisi id amet.Velit aute minim aliquip do. Dolor eiusmod ad anim laboris consectetur consectetur eiusmod exercitation mollit officia ea. Esse irure laboris ut ad laborum deserunt pariatur occaecat ullamco elit esse in culpa. Sunt deserunt minim nulla pariatur reprehenderit.
Pariatur aliquip qui magna consectetur cupidatat sit officia eu dolor cillum consequat. Adipisicing Lorem aliquip laboris eiusmod quis aliquip Lorem. Dolore cillum minim adipisicing minim.",
        FechaPublicacion = new DateTime(2016, 01, 08, 06, 54, 39),
        FechaCreacion = new DateTime(2016, 01, 08, 06, 54, 39),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion
    {
        Usuario = editor2,
        Autor = "Carol Nicholson Gonzalez",
        Titulo = "Eu nostrud pariatur ad consequat minim.",
        Descripcion = @"Occaecat sint reprehenderit mollit officia magna est Lorem occaecat cupidatat nostrud. Eiusmod sint ea laboris esse ut do pariatur amet. Laborum sit minim sit eiusmod excepteur labore deserunt ea qui aliqua occaecat ea quis fugiat. Ad est sint aliquip laboris do laboris deserunt pariatur ut. Excepteur sint nulla pariatur adipisicing aliqua. Elit sit voluptate adipisicing magna anim ipsum aute duis quis duis aute exercitation.Anim fugiat fugiat aliqua eiusmod elit excepteur irure aliquip cillum amet ullamco irure deserunt amet. Mollit sunt labore proident mollit do ex sint tempor sint aliqua. Id cillum et eiusmod non. Irure amet tempor nisi ex esse est dolore. Deserunt enim elit reprehenderit nisi id irure exercitation. Nisi voluptate velit ad officia qui laboris qui amet ipsum anim incididunt magna anim qui. Dolor labore fugiat non et est.
Elit est ut fugiat nulla enim qui est nulla aute. Laboris sit exercitation incididunt veniam. Deserunt incididunt nostrud cillum incididunt elit qui consectetur minim enim. Excepteur enim Lorem ea ea fugiat duis enim enim aliquip.",
        FechaPublicacion = new DateTime(2016, 01, 26, 10, 56, 24),
        FechaCreacion = new DateTime(2016, 01, 26, 10, 56, 24),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion
    {
        Usuario = editor2,
        Autor = "King Benjamin Hughes",
        Titulo = "Ea officia cupidatat mollit occaecat aliqua ea.",
        Descripcion = @"Voluptate elit eu eiusmod proident nulla anim aliqua incididunt irure magna velit. Magna consequat do exercitation veniam duis elit dolor eu occaecat. Consequat ex est ex nisi elit enim ullamco irure duis amet elit. Ad deserunt anim ipsum fugiat aliqua. Voluptate id laborum consequat consequat exercitation amet reprehenderit nulla laborum ipsum. Esse in proident duis occaecat cillum ex consequat commodo aliqua. Non esse sit ad mollit laboris aliqua ex ex et eiusmod enim sit nisi.Eiusmod nisi officia sint culpa laborum non cupidatat proident adipisicing tempor enim. Esse magna deserunt fugiat duis. Nostrud excepteur veniam ullamco esse fugiat amet adipisicing exercitation sit est deserunt irure adipisicing. Nisi in incididunt incididunt ipsum do.
Officia ex anim fugiat do cillum. Id non non ullamco ut incididunt. Sunt irure dolore minim duis dolor esse excepteur ut laborum do irure occaecat exercitation sunt.",
        FechaPublicacion = new DateTime(2016, 01, 08, 01, 20, 58),
        FechaCreacion = new DateTime(2016, 01, 08, 01, 20, 58),
        CategoriaID = 6,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Kristina Shaffer Rios",
        Titulo = "Excepteur aliqua et exercitation dolor.",
        Descripcion = @"Veniam minim proident tempor irure et eiusmod in. Sint ut ipsum ullamco dolore aute eiusmod minim quis mollit amet nulla. Labore ullamco sunt ullamco est nulla excepteur consequat non sunt labore ut. Minim non mollit enim elit ex. Exercitation reprehenderit Lorem velit deserunt cillum ut cillum voluptate amet enim deserunt sit id. Ut reprehenderit nisi sit sit sit excepteur. Pariatur ea ea minim veniam aliquip.Consectetur ipsum irure consectetur veniam tempor deserunt quis aliqua eu ad magna ea eu mollit. Fugiat sint magna voluptate eu veniam ut nostrud adipisicing quis aliquip quis. Id incididunt incididunt veniam consectetur in nostrud nisi ullamco adipisicing in sit elit. Non aute amet exercitation fugiat ullamco. Non consequat sunt dolore esse excepteur consectetur excepteur ex nostrud reprehenderit id anim non. Velit duis labore elit pariatur.
Anim voluptate nulla duis veniam ea culpa proident officia mollit est. Veniam ex laborum commodo officia ex consequat ullamco est elit cillum. Laboris aliqua culpa voluptate ut. Nisi eiusmod ad elit sit ullamco veniam minim enim magna pariatur labore non. Do eu sint ad ut commodo culpa commodo exercitation proident sunt anim. Eiusmod enim Lorem non nulla laborum amet anim eiusmod sunt aliquip nulla aute et pariatur.",
        FechaPublicacion = new DateTime(2016, 01, 13, 09, 36, 13),
        FechaCreacion = new DateTime(2016, 01, 13, 09, 36, 13),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Perry Colon Kaufman",
        Titulo = "Pariatur in in ullamco ipsum culpa cillum aliqua commodo ex.",
        Descripcion = @"Aliquip reprehenderit id velit ex mollit irure id incididunt consectetur proident commodo amet sunt. Sunt id mollit ipsum qui laborum culpa magna dolor aliquip incididunt laboris qui cillum. Ad esse non mollit officia occaecat sit aliquip excepteur.Non eu exercitation eiusmod aliquip voluptate exercitation voluptate elit enim ex velit fugiat sint. Et culpa ad dolor nulla tempor ut in cillum ullamco enim nostrud cupidatat do. Deserunt labore magna fugiat consequat do qui id.
Culpa aliquip qui enim elit est. Ad Lorem consectetur amet cillum et consequat elit Lorem adipisicing. Non culpa voluptate non ad eiusmod nulla anim. Ea commodo magna veniam velit qui proident reprehenderit labore ipsum ut ex. Irure ex exercitation dolore amet est et labore mollit pariatur adipisicing velit ad laboris.",
        FechaPublicacion = new DateTime(2016, 01, 16, 02, 15, 03),
        FechaCreacion = new DateTime(2016, 01, 16, 02, 15, 03),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Gomez Sherman Morgan",
        Titulo = "Adipisicing ea ex consectetur reprehenderit veniam officia ex sunt ipsum.",
        Descripcion = @"Qui ad adipisicing eu veniam. Minim minim laborum dolor mollit nisi eiusmod amet id. Eiusmod irure consequat ea elit mollit et tempor sit reprehenderit pariatur excepteur nulla reprehenderit cupidatat. Nostrud labore quis consectetur Lorem proident aute officia duis nostrud ullamco. Dolor non in mollit qui aliqua cillum. Lorem dolore pariatur ipsum quis adipisicing. Ullamco laborum velit laborum duis quis laboris incididunt labore tempor nisi.Irure consequat ullamco ullamco minim quis tempor quis dolore ex cillum adipisicing duis non fugiat. Enim culpa aute excepteur eu sunt fugiat et amet Lorem. Consectetur eu veniam veniam duis cillum. Velit esse incididunt aliquip et.
Deserunt enim aliquip qui dolore laboris sit anim ea duis dolor. Occaecat mollit dolore incididunt dolor est tempor cupidatat ad reprehenderit anim incididunt laboris non do. Do excepteur et est id adipisicing laboris magna culpa proident officia irure aute. Commodo enim incididunt exercitation eu ut esse elit officia quis sunt adipisicing sint. Non anim magna culpa nisi veniam ut fugiat consectetur incididunt tempor. Occaecat Lorem proident anim mollit reprehenderit aute commodo sit ad esse irure dolore.",
        FechaPublicacion = new DateTime(2016, 01, 23, 03, 11, 07),
        FechaCreacion = new DateTime(2016, 01, 23, 03, 11, 07),
        CategoriaID = 4,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Dickerson Hanson Warner",
        Titulo = "Consequat dolor excepteur consequat adipisicing in occaecat non do tempor.",
        Descripcion = @"Esse laborum ex voluptate veniam irure laboris elit ut non culpa duis. Enim anim veniam ad excepteur eu esse tempor. Occaecat sunt est officia nisi ea qui sunt sunt dolor Lorem duis ea officia magna. Dolore eiusmod eiusmod enim reprehenderit ea culpa aliqua. Ad nulla voluptate sint aliqua incididunt excepteur mollit pariatur officia. Laborum labore proident sint culpa. Reprehenderit consectetur et reprehenderit non aliqua reprehenderit voluptate occaecat incididunt tempor.Sunt pariatur laboris cillum do do magna cupidatat velit commodo excepteur do mollit esse incididunt. Occaecat velit magna adipisicing nulla commodo deserunt est cupidatat laborum consectetur. Consequat officia nisi qui non laboris reprehenderit commodo eiusmod tempor labore ex duis qui. Incididunt in amet irure ipsum veniam deserunt. Veniam consectetur laboris qui Lorem fugiat deserunt nostrud.
Tempor dolor incididunt ut incididunt elit officia adipisicing laborum pariatur cillum. Adipisicing in et occaecat id. Lorem et nostrud reprehenderit id consectetur excepteur qui nisi qui. Sunt irure excepteur cupidatat veniam esse adipisicing id aute culpa Lorem. Incididunt ad est et tempor laborum duis nulla nostrud ut.",
        FechaPublicacion = new DateTime(2016, 01, 22, 02, 25, 12),
        FechaCreacion = new DateTime(2016, 01, 22, 02, 25, 12),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Delaney Mcgowan Pennington",
        Titulo = "Laborum veniam commodo dolor ut proident elit anim consequat nisi ea non irure id adipisicing.",
        Descripcion = @"Occaecat proident commodo eiusmod in ut do velit. Consectetur tempor dolor magna eiusmod. Ipsum ipsum non proident ad fugiat reprehenderit elit amet laboris anim incididunt qui consectetur. Aliqua id consequat magna nisi voluptate proident veniam sit ullamco. Aliqua cillum non irure minim eu eiusmod eiusmod sint officia non consequat culpa qui. Ut consequat magna est duis deserunt mollit reprehenderit ea in.Commodo duis sit labore laboris velit eu nostrud eiusmod mollit cupidatat officia laborum excepteur non. Nulla et irure deserunt culpa tempor eiusmod ullamco id. Ea ut ex in dolore sint culpa do laboris. Mollit aliquip qui irure id aute duis voluptate qui sint esse magna. Nostrud incididunt minim culpa cillum consequat exercitation adipisicing amet et dolor veniam laboris aute. Consequat ut adipisicing anim ipsum laborum nisi proident pariatur culpa. Nostrud voluptate aliqua fugiat occaecat Lorem.
Non est enim eu officia sit dolor minim aute magna consequat. Laborum ex fugiat magna dolore nulla eiusmod mollit aute enim esse. Aliquip elit mollit nostrud excepteur. Labore adipisicing velit ut est. Aute cillum aliqua sunt aliquip et ipsum magna.",
        FechaPublicacion = new DateTime(2016, 01, 02, 05, 46, 56),
        FechaCreacion = new DateTime(2016, 01, 02, 05, 46, 56),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Macias Pittman Wilkinson",
        Titulo = "Elit ut consequat sunt minim tempor commodo excepteur ad occaecat in laboris est quis.",
        Descripcion = @"Sunt ipsum anim labore adipisicing consectetur eiusmod. Esse veniam occaecat qui excepteur qui amet reprehenderit aute amet. Pariatur sunt reprehenderit anim enim qui et dolore Lorem. Dolore aliquip est aliquip excepteur irure excepteur eiusmod.Minim nulla est nulla et in mollit laboris labore magna laboris officia. Et in duis aute est sint elit sunt id. Reprehenderit nisi reprehenderit sit voluptate velit officia quis consectetur in. Duis sit velit ut consectetur. Ad quis eu occaecat qui. Cillum officia laboris irure voluptate duis ex reprehenderit ex aliqua.
Duis labore irure duis excepteur magna elit occaecat aliquip. Cupidatat do dolor esse non elit commodo cillum cillum occaecat. Sunt adipisicing cupidatat sit Lorem culpa. Ullamco dolor mollit quis cillum minim cupidatat aliquip sit ullamco. Irure Lorem nisi minim do.",
        FechaPublicacion = new DateTime(2016, 01, 05, 10, 36, 24),
        FechaCreacion = new DateTime(2016, 01, 05, 10, 36, 24),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
        },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Vickie Salas Sanchez",
        Titulo = "Reprehenderit consequat enim aliqua eiusmod exercitation ad minim laborum commodo sit.",
        Descripcion = @"Proident commodo nostrud ullamco est Lorem dolore proident veniam amet laboris. Laborum sint eu fugiat exercitation officia elit. Exercitation exercitation anim eu sint non consectetur magna laboris ullamco ex exercitation pariatur.Reprehenderit dolor et anim magna. Qui ut excepteur aliqua magna cupidatat dolore in aliqua Lorem. Reprehenderit exercitation minim adipisicing fugiat reprehenderit laboris minim. Deserunt anim occaecat ut excepteur labore veniam magna eu consequat.
Minim ex adipisicing tempor exercitation occaecat dolore veniam laboris qui aliquip cupidatat aliquip duis. Deserunt do quis cupidatat nostrud. Quis aliqua aliquip ex laborum sunt excepteur in. Ea nostrud Lorem id ad reprehenderit sunt excepteur laborum occaecat.",
        FechaPublicacion = new DateTime(2016, 01, 12, 02, 00, 20),
        FechaCreacion = new DateTime(2016, 01, 12, 02, 00, 20),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Alison Solis Pena",
        Titulo = "Ipsum exercitation fugiat laboris labore.",
        Descripcion = @"Sit proident occaecat esse commodo velit pariatur ex occaecat incididunt nulla magna mollit sunt. Amet cillum velit eu voluptate commodo deserunt veniam fugiat nulla magna sint dolor cupidatat. Aute consequat mollit excepteur in ad id mollit do. Cillum nisi adipisicing aliqua minim eu nostrud sit laboris eu ea ex dolore non proident.Ullamco tempor duis quis labore nostrud. Est sunt Lorem eiusmod Lorem enim ut fugiat velit. Sint dolor mollit duis esse exercitation irure sint officia voluptate. Id dolore adipisicing quis in. Consequat cupidatat nulla ut ea non proident minim culpa in. Aute aliquip occaecat ut do esse irure incididunt.
Cillum aliquip ex dolor duis commodo laborum nulla aliqua laboris laborum ut in. Aliquip et tempor fugiat culpa est. Lorem ullamco ipsum ex ut fugiat mollit est fugiat cupidatat non. Aute do sunt laboris nulla id laborum. Minim aute elit tempor nulla deserunt laboris quis irure labore cillum quis. Id ut irure ex cillum enim duis in do occaecat.",
        FechaPublicacion = new DateTime(2016, 01, 16, 06, 03, 43),
        FechaCreacion = new DateTime(2016, 01, 16, 06, 03, 43),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Bonita Mclean Hartman",
        Titulo = "Deserunt Lorem reprehenderit eiusmod excepteur sunt ea minim dolor et deserunt.",
        Descripcion = @"Do veniam exercitation ea incididunt occaecat. Adipisicing sunt id elit laborum ex amet. Quis dolor nisi non eiusmod amet qui esse pariatur elit ex. Minim minim aliqua duis minim minim consectetur tempor velit exercitation minim fugiat esse consectetur. Sit ad pariatur ex officia. Sint cillum cupidatat irure dolor eiusmod. Aliquip irure et irure magna nisi cillum fugiat incididunt id dolor mollit.Commodo proident minim anim ex excepteur excepteur veniam ex id ad in nisi duis. Officia fugiat reprehenderit proident commodo. Minim anim ut laborum aute nulla consectetur dolore.
Amet adipisicing exercitation et est sunt reprehenderit consectetur incididunt. Dolor laboris aliquip occaecat Lorem est elit. Laboris amet irure dolor ullamco magna aliquip id ad ad cillum amet aliqua. Consequat esse non et quis eu Lorem elit et labore. Qui mollit aliquip reprehenderit ex culpa laborum laborum. Est anim irure voluptate eu veniam aliquip do sint fugiat cupidatat aliquip.",
        FechaPublicacion = new DateTime(2016, 01, 17, 12, 59, 33),
        FechaCreacion = new DateTime(2016, 01, 17, 12, 59, 33),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Michael Gross Michael",
        Titulo = "Aliquip proident est tempor nostrud pariatur ullamco irure magna ut et et.",
        Descripcion = @"Sint aute cupidatat irure officia et sit. Ea labore elit consequat quis magna deserunt in ex ex labore eu sunt ex. Magna in dolor deserunt dolore ipsum dolor laboris tempor est non elit. Adipisicing occaecat quis voluptate non eiusmod nulla do ullamco fugiat cupidatat non exercitation anim deserunt. Tempor magna anim aute culpa qui cupidatat occaecat eu exercitation cillum est adipisicing. Esse aliqua quis est ullamco magna sint incididunt tempor ullamco mollit et.Tempor excepteur magna voluptate nulla nulla. Quis nisi aliquip ea Lorem ipsum velit minim amet consectetur ex nulla esse aliquip. Labore eu eu minim consectetur consectetur aliquip fugiat nostrud. Fugiat non nisi aute qui minim minim. Aute culpa veniam anim occaecat do ea quis irure. Non commodo cupidatat magna nisi sint nulla exercitation exercitation reprehenderit do mollit quis nulla nisi. Duis cillum dolor occaecat occaecat sit et tempor.
Occaecat veniam exercitation fugiat dolor eu elit commodo et et culpa enim quis. Aute mollit exercitation est aute ad amet sit. Dolore consectetur consectetur do officia est ullamco. Dolore et et adipisicing velit aliqua aliqua minim id. Aliquip officia commodo non anim velit deserunt velit laboris.",
        FechaPublicacion = new DateTime(2016, 01, 28, 12, 48, 46),
        FechaCreacion = new DateTime(2016, 01, 28, 12, 48, 46),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Beasley Glenn Nunez",
        Titulo = "Sunt aliquip exercitation velit commodo magna culpa occaecat qui labore exercitation.",
        Descripcion = @"Aute incididunt anim in magna commodo minim cupidatat magna voluptate velit. Cupidatat aliquip fugiat deserunt proident ipsum fugiat dolore ea culpa commodo. Dolore qui excepteur ut sit incididunt et et nostrud mollit ullamco. Minim incididunt fugiat exercitation voluptate amet et do adipisicing. Exercitation nulla sint anim qui sunt sit fugiat non est.Aute qui sit ad pariatur deserunt. Qui est cupidatat Lorem elit reprehenderit officia laboris. Aliquip ut id cupidatat nisi id duis. In anim adipisicing labore dolor elit anim nulla dolore nostrud excepteur. Culpa velit laboris voluptate exercitation. Commodo dolore sit duis duis occaecat pariatur velit et est aliquip do minim cupidatat pariatur. Mollit sint tempor officia dolor est.
Excepteur in excepteur sunt nostrud occaecat ipsum est magna duis ipsum nisi minim. Quis anim laboris proident eiusmod velit irure magna. Occaecat laboris laborum ex ad cillum elit tempor sit magna in. Elit Lorem enim irure cillum aliquip aliquip et minim. Incididunt non ad dolor aute. Exercitation qui proident occaecat id cupidatat amet magna laborum eu officia commodo culpa in qui. Eiusmod cupidatat mollit enim anim quis.",
        FechaPublicacion = new DateTime(2016, 01, 21, 07, 00, 23),
        FechaCreacion = new DateTime(2016, 01, 21, 07, 00, 23),
        CategoriaID = 2,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Simmons Donaldson Gallagher",
        Titulo = "Non ut amet ea occaecat est occaecat velit minim mollit minim eu.",
        Descripcion = @"Non est tempor tempor aliquip veniam do eiusmod id quis deserunt. Voluptate incididunt sit cupidatat ea ipsum aute in enim. Elit reprehenderit officia minim eu magna anim exercitation pariatur officia commodo tempor commodo culpa. Consectetur ullamco eiusmod non exercitation occaecat consequat aute elit do dolore ad in do. Velit magna est eiusmod ad aliqua esse et. Aliqua qui commodo reprehenderit cupidatat.Est ullamco sunt quis sint exercitation pariatur dolor. Voluptate aliqua non incididunt veniam fugiat reprehenderit commodo consequat laboris. Excepteur minim et est laborum pariatur esse fugiat do eu incididunt tempor. Labore commodo deserunt minim elit eu.
Anim consequat veniam proident ullamco pariatur id aute et consectetur in. Elit eiusmod minim nulla officia labore in reprehenderit aliquip mollit velit ut incididunt. Aliquip adipisicing qui exercitation proident amet quis ex anim nostrud elit sit excepteur aute. Duis voluptate reprehenderit quis minim labore velit id officia qui sit est commodo. Qui culpa fugiat consectetur nisi. Ex ullamco elit dolor irure occaecat enim. Anim commodo ipsum culpa officia sint do officia elit laboris commodo quis voluptate fugiat.",
        FechaPublicacion = new DateTime(2016, 01, 12, 01, 03, 27),
        FechaCreacion = new DateTime(2016, 01, 12, 01, 03, 27),
        CategoriaID = 3,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Vinson Powers Durham",
        Titulo = "Occaecat qui ipsum commodo dolor id in sint elit cillum in incididunt eu.",
        Descripcion = @"Ullamco do voluptate irure exercitation ut. Excepteur occaecat officia est amet voluptate qui adipisicing laborum labore exercitation ut in nisi. Consequat fugiat sunt dolore esse nostrud exercitation nulla mollit anim quis aliquip. Elit aute dolore dolore nostrud exercitation duis nostrud occaecat elit voluptate pariatur do. Excepteur eu tempor anim occaecat ut exercitation ipsum nisi. Excepteur tempor esse incididunt cupidatat.In culpa voluptate ad elit. Est eiusmod ullamco aliqua ea Lorem pariatur. Occaecat eiusmod sint qui in ea.
Labore eu et ipsum in commodo reprehenderit. Fugiat velit non nostrud voluptate nostrud cupidatat anim est sint officia anim consectetur aliqua. Irure adipisicing ea adipisicing ut enim veniam laborum reprehenderit officia. Culpa ex Lorem amet ipsum sit amet pariatur minim nulla veniam.",
        FechaPublicacion = new DateTime(2016, 01, 24, 10, 53, 15),
        FechaCreacion = new DateTime(2016, 01, 24, 10, 53, 15),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Johnston Farrell Santiago",
        Titulo = "Aute ullamco tempor eu in id tempor irure mollit incididunt minim qui culpa.",
        Descripcion = @"Voluptate ipsum adipisicing adipisicing deserunt ex culpa adipisicing. Dolore cupidatat culpa anim dolore voluptate consequat ad deserunt eu pariatur consequat eiusmod adipisicing. Magna mollit minim eiusmod sint laboris aliquip velit ea sunt do voluptate.Officia tempor ad mollit ex eu laborum ipsum ullamco et dolor nisi cillum. Ullamco sit nostrud enim irure enim. Amet sunt duis reprehenderit labore excepteur anim enim quis. Officia duis amet ad elit consequat elit laboris. Nisi velit culpa elit adipisicing nisi est anim cupidatat.
Incididunt et sit ipsum eiusmod est Lorem cupidatat esse Lorem sit elit ullamco. Mollit est sunt cupidatat aliqua minim irure sunt mollit veniam enim anim officia. Elit culpa minim ea ipsum est Lorem magna duis mollit voluptate reprehenderit irure. Ex Lorem veniam tempor in.",
        FechaPublicacion = new DateTime(2016, 01, 08, 09, 25, 42),
        FechaCreacion = new DateTime(2016, 01, 08, 09, 25, 42),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Kline Hale Knox",
        Titulo = "Duis incididunt veniam consectetur ex.",
        Descripcion = @"Reprehenderit qui aute id veniam sint cillum ipsum magna reprehenderit anim occaecat elit proident. Ex sint officia cupidatat aliqua aliqua velit ea ipsum ut occaecat. Ad pariatur nulla commodo in ea dolor culpa enim sit ad consequat amet. Cupidatat excepteur ad sunt duis exercitation reprehenderit voluptate ipsum ipsum sint duis tempor. Amet eu sint esse duis aliqua exercitation incididunt ea sit consequat ipsum laboris fugiat aliquip. Eu Lorem do eiusmod et. Reprehenderit officia amet aliqua sunt amet ea.Ex laborum ea aliquip magna. Adipisicing elit consequat duis eu proident nostrud occaecat enim irure. Ea id occaecat pariatur ut proident. Nisi voluptate labore dolor laborum ea incididunt et exercitation. Consequat ipsum incididunt ipsum in dolor. Irure aliquip nostrud reprehenderit cupidatat mollit minim aliqua magna laborum.
Incididunt excepteur dolore in elit esse proident quis. Cillum anim in aute voluptate ex eiusmod exercitation esse cupidatat fugiat. Dolor ex nisi exercitation adipisicing tempor fugiat nulla aute proident amet deserunt irure. Dolore aliqua ex nisi quis proident dolor. Laboris officia commodo fugiat exercitation dolore do est ullamco est.",
        FechaPublicacion = new DateTime(2016, 01, 14, 02, 48, 56),
        FechaCreacion = new DateTime(2016, 01, 14, 02, 48, 56),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor1,
        Autor = "Mathis Ingram Mckee",
        Titulo = "In aliquip amet irure consequat sint minim commodo aliquip.",
        Descripcion = @"Pariatur duis Lorem voluptate non et labore ipsum commodo laborum aute. Duis do culpa excepteur et in dolore laboris eiusmod occaecat laborum sunt ad. Officia excepteur aliquip labore commodo cillum ad aliquip irure. Consectetur culpa exercitation sunt veniam consequat.Et dolor nostrud laboris non excepteur. In aute culpa fugiat officia nisi proident ut. Lorem ex id minim duis deserunt occaecat magna velit cillum aliquip enim. Eu sunt occaecat veniam minim nisi culpa magna. Do magna consectetur sunt et id eu voluptate irure nostrud. Nisi id exercitation cillum esse nulla ex non occaecat tempor excepteur aute.
Cupidatat aliqua velit non magna laborum aliqua ullamco. Culpa non dolore Lorem sit labore ullamco cupidatat nisi. Laborum deserunt consectetur adipisicing eiusmod aute et tempor deserunt proident veniam. Consequat nisi in nulla ut quis cupidatat cupidatat sunt non duis. Veniam mollit exercitation minim fugiat proident ex qui. Occaecat enim minim deserunt consectetur voluptate irure voluptate ullamco incididunt Lorem cillum elit. Nulla nisi officia aliqua et dolore cupidatat non occaecat exercitation deserunt adipisicing minim sit nulla.",
        FechaPublicacion = new DateTime(2016, 01, 21, 04, 45, 02),
        FechaCreacion = new DateTime(2016, 01, 21, 04, 45, 02),
        CategoriaID = 1,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Mcmahon Gibson Branch",
        Titulo = "Consectetur eiusmod ex officia duis.",
        Descripcion = @"Occaecat occaecat aute excepteur do reprehenderit nostrud cupidatat. Magna laborum amet tempor officia consectetur. In aute exercitation dolore ea nostrud nulla incididunt. Eiusmod et in pariatur sint sunt culpa nostrud mollit. Ea laborum est commodo magna culpa sint magna labore sunt.Aute non aliqua culpa mollit mollit do eiusmod eiusmod adipisicing sunt sint adipisicing. Fugiat non labore tempor occaecat aliquip sint non ullamco minim consequat cupidatat reprehenderit. Voluptate non proident voluptate pariatur deserunt.
Duis enim nisi id ut Lorem ut qui ipsum mollit ut pariatur aute ex ea. Sint ea occaecat ad deserunt culpa nulla reprehenderit nostrud ad tempor. Enim eiusmod aute elit enim labore aute laborum incididunt occaecat do est elit aliquip incididunt. Sit Lorem sunt irure ipsum voluptate deserunt esse cillum culpa Lorem esse. Adipisicing amet mollit dolore elit. Irure veniam aliqua sunt est labore qui et elit esse ad labore ea.",
        FechaPublicacion = new DateTime(2016, 01, 08, 11, 19, 49),
        FechaCreacion = new DateTime(2016, 01, 08, 11, 19, 49),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    },
    new Comunicacion {
        Usuario = editor2,
        Autor = "Day Valenzuela Blackburn",
        Titulo = "Lorem do do aliquip eiusmod deserunt amet commodo laborum.",
        Descripcion = @"In esse ad do commodo non aliqua esse laboris sint cillum do ea. Non dolore laboris proident adipisicing duis consectetur consequat eu sit mollit deserunt. Consequat excepteur incididunt et nostrud reprehenderit. Commodo magna nisi ut exercitation incididunt esse eu tempor. Aliquip ad velit do fugiat Lorem irure nisi id amet.Velit aute minim aliquip do. Dolor eiusmod ad anim laboris consectetur consectetur eiusmod exercitation mollit officia ea. Esse irure laboris ut ad laborum deserunt pariatur occaecat ullamco elit esse in culpa. Sunt deserunt minim nulla pariatur reprehenderit.
Pariatur aliquip qui magna consectetur cupidatat sit officia eu dolor cillum consequat. Adipisicing Lorem aliquip laboris eiusmod quis aliquip Lorem. Dolore cillum minim adipisicing minim.",
        FechaPublicacion = new DateTime(2016, 01, 08, 06, 54, 39),
        FechaCreacion = new DateTime(2016, 01, 08, 06, 54, 39),
        CategoriaID = 5,
        UltimaEdicionIP = "127.0.0.1",
        Activo = true
    }
};
        }

        private string LoremPalabras(int palabras, int? longitudMaxima = null)
        {
            char separador = ' ';

            string[] split = lorem.Split(separador);
            int maxPalabras = split.Count();
            if (maxPalabras < palabras)
            {
                palabras = maxPalabras;
            }

            string resul = string.Join(
                separador.ToString(),
                split.Skip(rnd.Next(0, maxPalabras - palabras)).Take(palabras));


            return longitudMaxima.HasValue && resul.Length > longitudMaxima.Value
                ? resul.Substring(0, longitudMaxima.Value - 1)
                : resul;
        }

        private string LoremFrases(int frases, int? longitudMaxima = null)
        {
            char separador = '.';
            string[] split = lorem.Split(separador);
            int maxFrases = split.Count();
            if (maxFrases < frases)
            {
                frases = maxFrases;
            }
            string resul = string.Join(separador.ToString(),
                split.Skip(rnd.Next(0, maxFrases - frases)).Take(frases));

            return longitudMaxima.HasValue && resul.Length > longitudMaxima.Value
                ? resul.Substring(0, longitudMaxima.Value - 1)
                : resul;
        }

        string lorem =
        #region LoremIpsum
            @"Lorem ipsum dolor sit amet, consectetur adipiscing elit.Integer in orci mi. Cras vestibulum vestibulum suscipit. Duis semper vitae justo id pulvinar. Praesent commodo, eros vitae porttitor bibendum, dolor ante porttitor massa, at iaculis ex massa lobortis nunc.Morbi lacinia accumsan lectus, sit amet viverra tellus feugiat vel.Vivamus enim ipsum, interdum sed viverra at, porta id leo. Vestibulum dapibus, magna at vestibulum sodales, lectus augue tempor mauris, hendrerit faucibus purus tellus ut magna.Pellentesque efficitur lorem sit amet neque lacinia consequat. Praesent euismod luctus lectus id facilisis.
Integer porta lectus sollicitudin blandit ullamcorper. Nam vitae viverra est, quis vestibulum ante. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nulla sit amet lacus orci.Vivamus posuere in ex vel efficitur.Nunc aliquet mollis lorem, vel elementum nisi faucibus eget. Proin vel tempor nisi, quis posuere augue. Aliquam efficitur, dolor sed convallis fringilla, nisl ipsum lacinia libero, vel elementum turpis eros eu eros.Vestibulum at nibh in arcu condimentum ullamcorper a vitae sapien.
Phasellus molestie neque vel pretium ornare. Morbi porta tortor consequat fringilla elementum. Donec congue placerat magna eget rhoncus. Fusce gravida est ut eros accumsan maximus.Cras pulvinar pretium nibh, sit amet tempor metus.Morbi eleifend orci sollicitudin interdum fringilla. Phasellus augue turpis, luctus sit amet mattis quis, efficitur auctor augue.In libero turpis, porttitor non ullamcorper vel, faucibus vel ex. Maecenas ultricies erat id sem consequat eleifend.Morbi luctus condimentum velit, at convallis libero fermentum ac. Vestibulum lacinia erat metus, at scelerisque diam mattis in. Sed in imperdiet eros. Morbi quam arcu, tincidunt a neque sed, lobortis mollis odio. Nulla eget ullamcorper eros, dapibus imperdiet erat.
Integer scelerisque, dolor at interdum pretium, lacus risus vestibulum purus, ut aliquam neque odio ac arcu.Nam gravida ex ac diam mattis, porttitor tempor sapien porta.Aliquam luctus pellentesque libero vel porta. Aenean imperdiet eget felis vitae rutrum. Morbi pretium velit non enim dignissim, quis mollis lectus tempor.Ut ultricies pretium condimentum. Vestibulum aliquam quam sit amet tortor vulputate, sit amet egestas mauris porttitor.Vivamus enim libero, mattis quis vestibulum vitae, vulputate non lacus.
Pellentesque quis urna interdum, vehicula metus nec, molestie orci.Aliquam vulputate porttitor mauris, ac convallis quam volutpat id. Curabitur eleifend venenatis orci, vitae faucibus dolor efficitur finibus. Mauris risus orci, auctor tincidunt dapibus vitae, placerat vel nunc. Interdum et malesuada fames ac ante ipsum primis in faucibus.Integer quam augue, ultrices ac tempus eleifend, cursus nec lacus. Vivamus id arcu eu nulla vulputate finibus.Fusce finibus mauris nisi, vel aliquet justo dignissim in.
Ut laoreet magna non lobortis tristique. In a accumsan odio, in ornare mauris. Vestibulum ac erat venenatis, posuere metus eu, vulputate dolor.Donec eleifend auctor neque in dignissim.Quisque pretium nisi eu odio vulputate, et luctus urna varius.Mauris lacinia ornare felis. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Etiam lacus dolor, ultrices eget orci ut, dapibus dictum odio. Aenean id euismod elit.
Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Ut a purus eget arcu facilisis laoreet in et lacus. Integer quis ipsum ac velit tincidunt convallis.In varius felis dui, at aliquet lacus eleifend id. Etiam hendrerit sapien quis mauris ornare lacinia.Sed gravida nulla ac mi tincidunt, eget eleifend lacus auctor.Nullam non lorem ante. Aliquam vitae nisl risus. Sed sit amet justo id ligula efficitur blandit. Quisque consectetur ligula ut pellentesque ultricies. Ut mattis lectus quis enim venenatis lacinia.Etiam eget eros sit amet elit mattis pellentesque.
Pellentesque augue nunc, maximus vitae ultrices imperdiet, aliquam non massa. Morbi eu arcu risus. Aliquam ut mi commodo, elementum erat non, mattis lectus.Vivamus eget aliquam orci. Proin efficitur iaculis aliquet. In malesuada pharetra ligula, non bibendum metus tristique quis. Vivamus ultricies cursus aliquet. Nam neque tellus, aliquet tincidunt mauris vel, sagittis molestie libero. Vivamus a pellentesque odio, nec porta turpis. Aliquam consectetur sodales tempor. Donec aliquam egestas elit a sagittis. Maecenas pharetra enim volutpat tortor mattis, vitae vehicula arcu malesuada.Nulla at elit convallis, posuere urna eu, fermentum risus.Nullam vulputate ultricies elit, at sodales tellus lacinia vitae. Morbi libero tortor, ornare nec lacus sed, aliquam hendrerit ligula. In sed ligula vitae augue placerat faucibus eget quis mauris.
Ut a auctor quam, sed maximus neque. Nunc nunc lorem, mollis ut suscipit sed, blandit et leo. In interdum arcu eget orci dictum, et hendrerit mi commodo.Integer egestas eleifend varius. Nullam tempor ante in rhoncus semper. Aenean molestie tempor semper. Fusce non egestas felis. Etiam porta mi at faucibus sagittis.
Ut efficitur magna ligula, vitae volutpat ante pulvinar non. Nam massa arcu, fringilla quis diam eget, faucibus hendrerit metus. Phasellus tristique quam ante, pharetra mollis nibh commodo aliquet. Nam viverra, magna et iaculis accumsan, nulla nulla mollis lacus, nec dictum turpis velit eget odio.Fusce pulvinar commodo eros in pretium.Quisque scelerisque hendrerit ante nec viverra. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nam tempor sit amet turpis sodales eleifend.Aliquam erat volutpat.Quisque nec massa eleifend, blandit libero mollis, convallis tellus.Nam pretium justo eget magna posuere, faucibus dapibus magna malesuada.Sed sagittis dapibus bibendum. Vivamus quis ante est. Donec aliquam sem mollis risus feugiat lobortis."
        #endregion
        ;

        // La lista de Iconos se ha tomado de la última versión de font-awesome en la web pero el paquete
        // nuget de font awesome instalado no es una versión más antigua, por lo tanto los Iconos
        // incorporados en la última versión de font-awesome no son visibles y por eso se han comentado.
        private string[] Iconos = {
            #region Lista de Iconos de font-awesome
            "fa-adjust",
            "fa-anchor",
            "fa-archive",
            "fa-area-chart",
            "fa-arrows",
            "fa-arrows-h",
            "fa-arrows-v",
            "fa-asterisk",
            "fa-at",
            "fa-automobile",
            "fa-balance-scale",
            "fa-ban",
            "fa-bank ",
            "fa-bar-chart",
            "fa-bar-chart-o",
            "fa-barcode",
            "fa-bars",
            "fa-battery-0",
            "fa-battery-1",
            "fa-battery-2",
            "fa-battery-3",
            "fa-battery-4",
            "fa-battery-empty",
            "fa-battery-full",
            "fa-battery-half",
            "fa-battery-quarter",
            "fa-battery-three-quarters",
            "fa-bed",
            "fa-beer",
            "fa-bell",
            "fa-bell-o",
            "fa-bell-slash",
            "fa-bell-slash-o",
            "fa-bicycle",
            "fa-binoculars",
            "fa-birthday-cake",
            //"fa-bluetooth",
            //"fa-bluetooth-b",
            "fa-bolt",
            "fa-bomb",
            "fa-book",
            "fa-bookmark",
            "fa-bookmark-o",
            "fa-briefcase",
            "fa-bug",
            "fa-building",
            "fa-building-o",
            "fa-bullhorn",
            "fa-bullseye",
            "fa-bus",
            "fa-cab",
            "fa-calculator",
            "fa-calendar",
            "fa-calendar-check-o",
            "fa-calendar-minus-o",
            "fa-calendar-o",
            "fa-calendar-plus-o",
            "fa-calendar-times-o",
            "fa-camera",
            "fa-camera-retro",
            "fa-car",
            "fa-caret-square-o-down",
            "fa-caret-square-o-left",
            "fa-caret-square-o-right",
            "fa-caret-square-o-up",
            "fa-cart-arrow-down",
            "fa-cart-plus",
            "fa-cc",
            "fa-certificate",
            "fa-check",
            "fa-check-circle",
            "fa-check-circle-o",
            "fa-check-square",
            "fa-check-square-o",
            "fa-child",
            "fa-circle",
            "fa-circle-o",
            "fa-circle-o-notch",
            "fa-circle-thin",
            "fa-clock-o",
            "fa-clone",
            "fa-close",
            "fa-cloud",
            "fa-cloud-download",
            "fa-cloud-upload",
            "fa-code",
            "fa-code-fork",
            "fa-coffee",
            "fa-cog",
            "fa-cogs",
            "fa-comment",
            "fa-comment-o",
            "fa-commenting",
            "fa-commenting-o",
            "fa-comments",
            "fa-comments-o",
            "fa-compass",
            "fa-copyright",
            "fa-creative-commons",
            "fa-credit-card",
            //"fa-credit-card-alt",
            "fa-crop",
            "fa-crosshairs",
            "fa-cube",
            "fa-cubes",
            "fa-cutlery",
            "fa-dashboard",
            "fa-database",
            "fa-desktop",
            "fa-diamond",
            "fa-dot-circle-o",
            "fa-download",
            "fa-edit",
            "fa-ellipsis-h",
            "fa-ellipsis-v",
            "fa-envelope",
            "fa-envelope-o",
            "fa-envelope-square",
            "fa-eraser",
            "fa-exchange",
            "fa-exclamation",
            "fa-exclamation-circle",
            "fa-exclamation-triangle",
            "fa-external-link",
            "fa-external-link-square",
            "fa-eye",
            "fa-eye-slash",
            "fa-eyedropper",
            "fa-fax",
            "fa-feed",
            "fa-female",
            "fa-fighter-jet",
            "fa-file-archive-o",
            "fa-file-audio-o",
            "fa-file-code-o",
            "fa-file-excel-o",
            "fa-file-image-o",
            "fa-file-movie-o",
            "fa-file-pdf-o",
            "fa-file-photo-o",
            "fa-file-picture-o",
            "fa-file-powerpoint-o",
            "fa-file-sound-o",
            "fa-file-video-o",
            "fa-file-word-o",
            "fa-file-zip-o",
            "fa-film",
            "fa-filter",
            "fa-fire",
            "fa-fire-extinguisher",
            "fa-flag",
            "fa-flag-checkered",
            "fa-flag-o",
            "fa-flash",
            "fa-flask",
            "fa-folder",
            "fa-folder-o",
            "fa-folder-open",
            "fa-folder-open-o",
            "fa-frown-o",
            "fa-futbol-o",
            "fa-gamepad",
            "fa-gavel",
            "fa-gear",
            "fa-gears",
            "fa-gift",
            "fa-glass",
            "fa-globe",
            "fa-graduation-cap",
            "fa-group",
            "fa-hand-grab-o",
            "fa-hand-lizard-o",
            "fa-hand-paper-o",
            "fa-hand-peace-o",
            "fa-hand-pointer-o",
            "fa-hand-rock-o",
            "fa-hand-scissors-o",
            "fa-hand-spock-o",
            "fa-hand-stop-o",
            //"fa-hashtag",
            "fa-hdd-o",
            "fa-headphones",
            "fa-heart",
            "fa-heart-o",
            "fa-heartbeat",
            "fa-history",
            "fa-home",
            "fa-hotel",
            "fa-hourglass",
            "fa-hourglass-1",
            "fa-hourglass-2",
            "fa-hourglass-3",
            "fa-hourglass-end",
            "fa-hourglass-half",
            "fa-hourglass-o",
            "fa-hourglass-start",
            "fa-i-cursor",
            "fa-image",
            "fa-inbox",
            "fa-industry",
            "fa-info",
            "fa-info-circle",
            "fa-institution",
            "fa-key",
            "fa-keyboard-o",
            "fa-language",
            "fa-laptop",
            "fa-leaf",
            "fa-legal",
            "fa-lemon-o",
            "fa-level-down",
            "fa-level-up",
            "fa-life-bouy",
            "fa-life-buoy",
            "fa-life-ring",
            "fa-life-saver",
            "fa-lightbulb-o",
            "fa-line-chart",
            "fa-location-arrow",
            "fa-lock",
            "fa-magic",
            "fa-magnet",
            "fa-mail-forward",
            "fa-mail-reply",
            "fa-mail-reply-all",
            "fa-male",
            "fa-map",
            "fa-map-marker",
            "fa-map-o",
            "fa-map-pin",
            "fa-map-signs",
            "fa-meh-o",
            "fa-microphone",
            "fa-microphone-slash",
            "fa-minus",
            "fa-minus-circle",
            "fa-minus-square",
            "fa-minus-square-o",
            "fa-mobile",
            "fa-mobile-phone",
            "fa-money",
            "fa-moon-o",
            "fa-mortar-board",
            "fa-motorcycle",
            "fa-mouse-pointer",
            "fa-music",
            "fa-navicon ",
            "fa-newspaper-o",
            "fa-object-group",
            "fa-object-ungroup",
            "fa-paint-brush",
            "fa-paper-plane",
            "fa-paper-plane-o",
            "fa-paw",
            "fa-pencil",
            "fa-pencil-square",
            "fa-pencil-square-o",
            //"fa-percent",
            "fa-phone",
            "fa-phone-square",
            "fa-photo",
            "fa-picture-o",
            "fa-pie-chart",
            "fa-plane",
            "fa-plug",
            "fa-plus",
            "fa-plus-circle",
            "fa-plus-square",
            "fa-plus-square-o",
            "fa-power-off",
            "fa-print",
            "fa-puzzle-piece",
            "fa-qrcode",
            "fa-question",
            "fa-question-circle",
            "fa-quote-left",
            "fa-quote-right",
            "fa-random",
            "fa-recycle",
            "fa-refresh",
            "fa-registered",
            "fa-remove",
            "fa-reorder",
            "fa-reply",
            "fa-reply-all",
            "fa-retweet",
            "fa-road",
            "fa-rocket",
            "fa-rss",
            "fa-rss-square",
            "fa-search",
            "fa-search-minus",
            "fa-search-plus",
            "fa-send",
            "fa-send-o",
            "fa-server",
            "fa-share",
            "fa-share-alt",
            "fa-share-alt-square",
            "fa-share-square",
            "fa-share-square-o",
            "fa-shield",
            "fa-ship",
            //"fa-shopping-bag",
            //"fa-shopping-basket",
            "fa-shopping-cart",
            "fa-sign-in",
            "fa-sign-out",
            "fa-signal",
            "fa-sitemap",
            "fa-sliders",
            "fa-smile-o",
            "fa-soccer-ball-o",
            "fa-sort",
            "fa-sort-alpha-asc",
            "fa-sort-alpha-desc",
            "fa-sort-amount-asc",
            "fa-sort-amount-desc",
            "fa-sort-asc",
            "fa-sort-desc",
            "fa-sort-down",
            "fa-sort-numeric-asc",
            "fa-sort-numeric-desc",
            "fa-sort-up",
            "fa-space-shuttle",
            "fa-spinner",
            "fa-spoon",
            "fa-square",
            "fa-square-o",
            "fa-star",
            "fa-star-half",
            "fa-star-half-empty",
            "fa-star-half-full",
            "fa-star-half-o",
            "fa-star-o",
            "fa-sticky-note",
            "fa-sticky-note-o",
            "fa-street-view",
            "fa-suitcase",
            "fa-sun-o",
            "fa-support",
            "fa-tablet",
            "fa-tachometer",
            "fa-tag",
            "fa-tags",
            "fa-tasks",
            "fa-taxi",
            "fa-television",
            "fa-terminal",
            "fa-thumb-tack",
            "fa-thumbs-down",
            "fa-thumbs-o-down",
            "fa-thumbs-o-up",
            "fa-thumbs-up",
            "fa-ticket",
            "fa-times",
            "fa-times-circle",
            "fa-times-circle-o",
            "fa-tint",
            "fa-toggle-down",
            "fa-toggle-left",
            "fa-toggle-off",
            "fa-toggle-on",
            "fa-toggle-right",
            "fa-toggle-up",
            "fa-trademark",
            "fa-trash",
            "fa-trash-o",
            "fa-tree",
            "fa-trophy",
            "fa-truck",
            "fa-tty",
            "fa-tv",
            "fa-umbrella",
            "fa-university",
            "fa-unlock",
            "fa-unlock-alt",
            "fa-unsorted",
            "fa-upload",
            "fa-user",
            "fa-user-plus",
            "fa-user-secret",
            "fa-user-times",
            "fa-users",
            "fa-video-camera",
            "fa-volume-down",
            "fa-volume-off",
            "fa-volume-up",
            "fa-warning",
            "fa-wheelchair",
            "fa-wifi",
            "fa-wrench",
            "fa-hand-grab-o",
            "fa-hand-lizard-o",
            "fa-hand-o-down",
            "fa-hand-o-left",
            "fa-hand-o-right",
            "fa-hand-o-up",
            "fa-hand-paper-o",
            "fa-hand-peace-o",
            "fa-hand-pointer-o",
            "fa-hand-rock-o",
            "fa-hand-scissors-o",
            "fa-hand-spock-o",
            "fa-hand-stop-o",
            "fa-thumbs-down",
            "fa-thumbs-o-down",
            "fa-thumbs-o-up",
            "fa-thumbs-up",
            "fa-ambulance",
            "fa-automobile",
            "fa-bicycle",
            "fa-bus",
            "fa-cab",
            "fa-car",
            "fa-fighter-jet",
            "fa-motorcycle",
            "fa-plane",
            "fa-rocket",
            "fa-ship",
            "fa-space-shuttle",
            "fa-subway",
            "fa-taxi",
            "fa-train",
            "fa-truck",
            "fa-wheelchair",
            "fa-genderless",
            "fa-intersex",
            "fa-mars",
            "fa-mars-double",
            "fa-mars-stroke",
            "fa-mars-stroke-h",
            "fa-mars-stroke-v",
            "fa-mercury",
            "fa-neuter",
            "fa-transgender",
            "fa-transgender-alt",
            "fa-venus",
            "fa-venus-double",
            "fa-venus-mars",
            "fa-file",
            "fa-file-archive-o",
            "fa-file-audio-o",
            "fa-file-code-o",
            "fa-file-excel-o",
            "fa-file-image-o",
            "fa-file-movie-o",
            "fa-file-o",
            "fa-file-pdf-o",
            "fa-file-photo-o",
            "fa-file-picture-o",
            "fa-file-powerpoint-o",
            "fa-file-sound-o",
            "fa-file-text",
            "fa-file-text-o",
            "fa-file-video-o",
            "fa-file-word-o",
            "fa-file-zip-o",
            "fa-circle-o-notch",
            "fa-cog",
            "fa-gear ",
            "fa-refresh",
            "fa-spinner",
            "fa-check-square-o",
            "fa-circle",
            "fa-circle-o",
            "fa-dot-circle-o",
            "fa-minus-square",
            "fa-minus-square-o",
            "fa-plus-square",
            "fa-plus-square-o",
            "fa-square",
            "fa-square-o",
            "fa-cc-amex",
            "fa-cc-diners-club",
            "fa-cc-discover",
            "fa-cc-jcb",
            "fa-cc-mastercard",
            "fa-cc-paypal",
            "fa-cc-stripe",
            "fa-cc-visa",
            "fa-credit-card",
            //"fa-credit-card-alt",
            "fa-google-wallet",
            "fa-paypal",
            "fa-area-chart",
            "fa-bar-chart",
            "fa-bar-chart-o",
            "fa-line-chart",
            "fa-pie-chart",
            "fa-bitcoin",
            "fa-btc",
            "fa-cny",
            "fa-dollar",
            "fa-eur",
            "fa-euro",
            "fa-gbp",
            "fa-gg",
            "fa-gg-circle",
            "fa-ils",
            "fa-inr",
            "fa-jpy",
            "fa-krw",
            "fa-money",
            "fa-rmb",
            "fa-rouble",
            "fa-rub",
            "fa-ruble",
            "fa-rupee",
            "fa-shekel",
            "fa-sheqel",
            "fa-try",
            "fa-turkish-lira",
            "fa-usd",
            "fa-won",
            "fa-yen",
            "fa-align-center",
            "fa-align-justify",
            "fa-align-left",
            "fa-align-right",
            "fa-bold",
            "fa-chain",
            "fa-chain-broken",
            "fa-clipboard",
            "fa-columns",
            "fa-copy",
            "fa-cut",
            "fa-dedent",
            "fa-eraser",
            "fa-file",
            "fa-file-o",
            "fa-file-text",
            "fa-file-text-o",
            "fa-files-o",
            "fa-floppy-o",
            "fa-font",
            "fa-header",
            "fa-indent",
            "fa-italic",
            "fa-link",
            "fa-list",
            "fa-list-alt",
            "fa-list-ol",
            "fa-list-ul",
            "fa-outdent",
            "fa-paperclip",
            "fa-paragraph",
            "fa-paste",
            "fa-repeat",
            "fa-rotate-left",
            "fa-rotate-right",
            "fa-save",
            "fa-scissors",
            "fa-strikethrough",
            "fa-subscript",
            "fa-superscript",
            "fa-table",
            "fa-text-height",
            "fa-text-width",
            "fa-th",
            "fa-th-large",
            "fa-th-list",
            "fa-underline",
            "fa-undo",
            "fa-unlink",
            "fa-angle-double-down",
            "fa-angle-double-left",
            "fa-angle-double-right",
            "fa-angle-double-up",
            "fa-angle-down",
            "fa-angle-left",
            "fa-angle-right",
            "fa-angle-up",
            "fa-arrow-circle-down",
            "fa-arrow-circle-left",
            "fa-arrow-circle-o-down",
            "fa-arrow-circle-o-left",
            "fa-arrow-circle-o-right",
            "fa-arrow-circle-o-up",
            "fa-arrow-circle-right",
            "fa-arrow-circle-up",
            "fa-arrow-down",
            "fa-arrow-left",
            "fa-arrow-right",
            "fa-arrow-up",
            "fa-arrows",
            "fa-arrows-alt",
            "fa-arrows-h",
            "fa-arrows-v",
            "fa-caret-down",
            "fa-caret-left",
            "fa-caret-right",
            "fa-caret-square-o-down",
            "fa-caret-square-o-left",
            "fa-caret-square-o-right",
            "fa-caret-square-o-up",
            "fa-caret-up",
            "fa-chevron-circle-down",
            "fa-chevron-circle-left",
            "fa-chevron-circle-right",
            "fa-chevron-circle-up",
            "fa-chevron-down",
            "fa-chevron-left",
            "fa-chevron-right",
            "fa-chevron-up",
            "fa-exchange",
            "fa-hand-o-down",
            "fa-hand-o-left",
            "fa-hand-o-right",
            "fa-hand-o-up",
            "fa-long-arrow-down",
            "fa-long-arrow-left",
            "fa-long-arrow-right",
            "fa-long-arrow-up",
            "fa-toggle-down",
            "fa-toggle-left",
            "fa-toggle-right",
            "fa-toggle-up",
            "fa-arrows-alt",
            "fa-backward",
            "fa-compress",
            "fa-eject",
            "fa-expand",
            "fa-fast-backward",
            "fa-fast-forward",
            "fa-forward",
            "fa-pause",
            //"fa-pause-circle",
            //"fa-pause-circle-o",
            "fa-play",
            "fa-play-circle",
            "fa-play-circle-o",
            "fa-random",
            "fa-step-backward",
            "fa-step-forward",
            "fa-stop",
            //"fa-stop-circle",
            //"fa-stop-circle-o",
            "fa-youtube-play",
            "fa-adn",
            "fa-amazon",
            "fa-android",
            "fa-angellist",
            "fa-apple",
            //"fa-ehance",
            //"fa-ehance-square",
            "fa-bitbucket",
            "fa-bitbucket-square",
            "fa-bitcoin ",
            "fa-black-tie",
            //"fa-bluetooth",
            //"fa-bluetooth-b",
            "fa-btc",
            "fa-buysellads",
            "fa-cc-amex",
            "fa-cc-diners-club",
            "fa-cc-discover",
            "fa-cc-jcb",
            "fa-cc-mastercard",
            "fa-cc-paypal",
            "fa-cc-stripe",
            "fa-cc-visa",
            "fa-chrome",
            "fa-codepen",
            //"fa-codiepie",
            "fa-connectdevelop",
            "fa-contao",
            "fa-css3",
            "fa-dashcube",
            "fa-delicious",
            "fa-deviantart",
            "fa-digg",
            "fa-dribbble",
            "fa-dropbox",
            "fa-drupal",
            //"fa-edge",
            "fa-empire",
            "fa-expeditedssl",
            "fa-facebook",
            "fa-facebook-f",
            "fa-facebook-official",
            "fa-facebook-square",
            "fa-firefox",
            "fa-flickr",
            "fa-fonticons",
            //"fa-font-awesome",
            "fa-forumbee",
            "fa-foursquare",
            "fa-ge",
            "fa-get-pocket",
            "fa-gg",
            "fa-gg-circle",
            "fa-git",
            "fa-git-square",
            "fa-github",
            "fa-github-alt",
            "fa-github-square",
            "fa-gittip",
            "fa-google",
            "fa-google-plus",
            "fa-google-plus-square",
            "fa-google-wallet",
            "fa-gratipay",
            "fa-hacker-news",
            "fa-houzz",
            "fa-html5",
            "fa-instagram",
            "fa-internet-explorer",
            "fa-ioxhost",
            "fa-joomla",
            "fa-jsfiddle",
            "fa-lastfm",
            "fa-lastfm-square",
            "fa-leanpub",
            "fa-linkedin",
            "fa-linkedin-square",
            "fa-linux",
            "fa-maxcdn",
            "fa-meanpath",
            "fa-medium",
            //"fa-mixcloud",
            //"fa-modx",
            "fa-odnoklassniki",
            "fa-odnoklassniki-square",
            "fa-opencart",
            "fa-openid",
            "fa-opera",
            "fa-optin-monster",
            "fa-pagelines",
            "fa-paypal",
            "fa-pied-piper",
            "fa-pied-piper-alt",
            "fa-pinterest",
            "fa-pinterest-p",
            "fa-pinterest-square",
            //"fa-product-hunt",
            "fa-qq",
            "fa-ra",
            "fa-rebel",
            "fa-reddit",
            //"fa-reddit-alien",
            "fa-reddit-square",
            "fa-renren",
            "fa-safari",
            //"fa-scribd",
            "fa-sellsy",
            "fa-share-alt",
            "fa-share-alt-square",
            "fa-shirtsinbulk",
            "fa-simplybuilt",
            "fa-skyatlas",
            "fa-skype",
            "fa-slack",
            "fa-slideshare",
            "fa-soundcloud",
            "fa-spotify",
            "fa-stack-exchange",
            "fa-stack-overflow",
            "fa-steam",
            "fa-steam-square",
            "fa-stumbleupon",
            "fa-stumbleupon-circle",
            "fa-tencent-weibo",
            "fa-trello",
            "fa-tripadvisor",
            "fa-tumblr",
            "fa-tumblr-square",
            "fa-twitch",
            "fa-twitter",
            "fa-twitter-square",
            //"fa-usb",
            "fa-viacoin",
            "fa-vimeo",
            "fa-vimeo-square",
            "fa-vine",
            "fa-vk",
            "fa-wechat",
            "fa-weibo",
            "fa-weixin",
            "fa-whatsapp",
            "fa-wikipedia-w",
            "fa-windows",
            "fa-wordpress",
            "fa-xing",
            "fa-xing-square",
            "fa-y-combinator",
            "fa-y-combinator-square",
            "fa-yahoo",
            "fa-yc",
            "fa-yc-square",
            "fa-yelp",
            "fa-youtube",
            "fa-youtube-play",
            "fa-youtube-square",
            "fa-ambulance",
            "fa-h-square",
            "fa-heart",
            "fa-heart-o",
            "fa-heartbeat",
            "fa-hospital-o",
            "fa-medkit",
            "fa-plus-square",
            "fa-stethoscope",
            "fa-user-md",
            "fa-wheelchair"
            #endregion
        };

        private void ExtraerErrores(DbEntityValidationException ex)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var failure in ex.EntityValidationErrors)
            {
                sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                foreach (var error in failure.ValidationErrors)
                {
                    sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                    sb.AppendLine();
                }
            }

            throw new DbEntityValidationException(
                "Entity Validation Failed - errors follow:\n" +
                sb.ToString(), ex
            ); // Add the original exception as the innerException
        }
    }
}