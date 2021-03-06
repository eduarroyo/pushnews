﻿using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PushNews.Negocio.Excepciones.Comunicaciones;

namespace PushNews.Negocio
{
    public class ComunicacionesServicio: BaseServicio<Comunicacion>, IComunicacionesServicio
    {
        private Aplicacion aplicacion;

        public ComunicacionesServicio(IPushNewsUnitOfWork db, Aplicacion aplicacion)
            : base(db)
        {
            this.aplicacion = aplicacion;
            if (this.aplicacion != null)
            {
                RestrictFilter = c => c.Categoria.AplicacionID == aplicacion.AplicacionID;
            }
        }

        public override void Insert(Comunicacion entity)
        {
                entity.Activo = true;
                entity.FechaCreacion = DateTime.Now;
                base.Insert(entity);

        }

        /// <summary>
        /// Marca como eliminado el registro. Actualiza la fecha de borrado.
        /// No elimina el registro de la base de datos.
        /// </summary>
        public override void Delete(Comunicacion eliminar)
        {
            eliminar.TimeStamp = Util.UnixTimeStamp(DateTime.Now);
            eliminar.Borrado = true;
            eliminar.FechaBorrado = DateTime.Now;
        }

        /// <summary>
        /// Marca como eliminado el registro. Actualiza la fecha de borrado.
        /// No elimina el registro de la base de datos.
        /// </summary>
        public override void Delete(object id)
        {
            if(Type.GetTypeCode(id.GetType()) == TypeCode.Int32)
            {
                Delete(GetSingle(c => c.ComunicacionID == (long) id));
            }
        }
        
        public IEnumerable<Comunicacion> Publicadas(long? categoriaID = null, bool soloDestacadas = false, long? timestamp = null)
        {
            return Get(Filtro(categoriaID, soloDestacadas, timestamp),
                pp => pp.OrderByDescending(p => p.Destacado) // destacadas primero
                        .ThenByDescending(p => p.FechaPublicacion)
                        .ThenBy(p => p.Categoria.Orden),
                includeProperties: "Categoria, Adjunto, Accesos"
            );
        }

        public async Task<IEnumerable<Comunicacion>> PublicadasAsync(long? categoriaID = null, bool soloDestacadas = false, long? timestamp = null)
        {
            return await GetAsync(Filtro(categoriaID, soloDestacadas, timestamp),
                pp => pp.OrderByDescending(p => p.Destacado) // destacadas primero
                        .ThenByDescending(p => p.FechaPublicacion)
                        .ThenBy(p => p.Categoria.Orden),
                includeProperties: "Categoria, Adjunto, Accesos"
            );
        }

        private Expression<Func<Comunicacion, bool>> Filtro(long? categoriaID = null, bool soloDestacadas = false, long? timestamp = null)
        {
            DateTime ahora = DateTime.Now;

            // Especificar el filtro en función de si se ha especificado o no categoriaID.
            Expression<Func<Comunicacion, bool>> filtro;
            if (categoriaID.HasValue)
            {
                if (soloDestacadas)
                {
                    if (timestamp.HasValue)
                    {
                        // Comunicaciones activas, no borradas, con categoría activa y pública,
                        // destacadas, con fecha de publicación anterior a la fecha actual, 
                        // pertenecientes a la categoría del parámetro y con timestamp posterior al del
                        // parámetro.
                        filtro = p => p.Activo && !p.Borrado && p.Categoria.Activo && p.Destacado
                                    && p.FechaPublicacion <= ahora && p.CategoriaID == categoriaID.Value
                                    && p.TimeStamp > timestamp;
                    }
                    else
                    {
                        // Comunicaciones activas, no borradas, con categoría activa y pública, 
                        // destacadas, con fecha depublicación anterior a la fecha actual y 
                        // pertenecientes a la categoría del parámetro.
                        filtro = p => p.Activo && !p.Borrado && p.Categoria.Activo && p.Destacado
                                && p.FechaPublicacion <= ahora && p.CategoriaID == categoriaID.Value;
                    }
                }
                else
                {
                    if (timestamp.HasValue)
                    {
                        // Comunicaciones activas, no borradas, con categoría activa y pública, con fecha
                        // de publicación anterior a la fecha actual, pertenecientes a la categoría del 
                        // parámetro y con timestamp posterior al del parámetro.
                        filtro = p => p.Activo && !p.Borrado && p.Categoria.Activo
                                    && p.FechaPublicacion <= ahora && p.CategoriaID == categoriaID.Value
                                    && p.TimeStamp > timestamp;
                    }
                    else
                    {
                        // Comunicaciones activas, no borradas, con categoría activa y pública, con fecha
                        // de publicación anterior a la fecha actual y pertenecientes a la categoría del
                        // parámetro.
                        filtro = p => p.Activo && !p.Borrado && p.Categoria.Activo
                                && p.FechaPublicacion <= ahora && p.CategoriaID == categoriaID.Value;
                    }
                }
            }
            else
            {
                if (soloDestacadas)
                {
                    if (timestamp.HasValue)
                    {
                        // Comunicaciones activas, no borradas, con categoría activa y pública, 
                        // destacadas, con fecha de publicación anterior a la fecha actual y con
                        // timestamp posterior al del parámetro.
                        filtro = p => p.Activo && !p.Borrado && p.Categoria.Activo && p.Destacado
                                    && p.FechaPublicacion <= ahora && p.TimeStamp > timestamp;
                    }
                    else
                    {
                        // Comunicaciones activas, no borradas, destacadas, con categoría activa y 
                        // pública y con fecha de publicaciónanterior a la fecha actual.
                        filtro = p => p.Activo && !p.Borrado && p.Categoria.Activo && p.Destacado
                                && p.FechaPublicacion <= ahora;
                    }
                }
                else
                {
                    if (timestamp.HasValue)
                    {
                        // Comunicaciones activas, no borradas, con categoría activa y pública, con fecha
                        // de publicación anterior a la fecha actual y con timestamp posterior al del
                        // parámetro.
                        filtro = p => p.Activo && !p.Borrado && p.Categoria.Activo
                                && p.FechaPublicacion <= ahora && p.TimeStamp > timestamp;
                    }
                    else
                    {
                        // Comunicaciones activas, no borradas, con categoría activa y pública y con
                        // fecha de publicación anterior a la fecha actual.
                        filtro = p => p.Activo && !p.Borrado && p.Categoria.Activo
                            && p.FechaPublicacion <= ahora;
                    }
                }
            }

            return filtro;
        }

        public Comunicacion ConsultarComunicacion(long comunicacionID, string uid, string ip)
        {
            Comunicacion com = GetSingle(c => c.ComunicacionID == comunicacionID && !c.Borrado && c.Activo);

            if(com == null)
            {
                throw new ComunicacionNoEncontradaException(comunicacionID, aplicacion);
            }

            IComunicacionesAccesosServicio asrv = new ComunicacionesAccesosServicio(unitOfWork, aplicacion);
            asrv.AccesoTerminal(com, uid, ip);

            return com;
        }
    }
}