﻿using System.Collections.Generic;

namespace PushNews.Dominio.Entidades
{
    public class Categoria
    {
        public Categoria()
        {
            Comunicaciones = new List<Comunicacion>(0);
        }

        public long CategoriaID { get; set; }
        public long AplicacionID { get; set; }
        public long UsuarioID { get; set; }
        public string Nombre { get; set; }
        public bool Privada { get; set; } = false;
        public string Icono { get; set; }
        public int Orden { get; set; }
        public bool Activo { get; set; }

        public virtual Aplicacion Aplicacion { get; set; }
        public virtual Usuario Creador { get; set; }

        public virtual ICollection<Comunicacion> Comunicaciones { get; private set; }
    }
}