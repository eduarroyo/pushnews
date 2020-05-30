using System;
using System.Collections.Generic;
using System.Linq;

namespace PushNews.WebApp.Models.UI
{
    public class GridToolbarModel
    {
        public GridToolbarModel(IEnumerable<string> camposBuscar, string placeHolderBuscar, bool agregar = true)
        {
            Agregar = agregar;
            if (camposBuscar != null && camposBuscar.Count() > 0)
            {
                Buscar = true;
                CamposBuscar = camposBuscar;
                PlaceHolderBuscar = placeHolderBuscar;
            }
            else
            {
                Buscar = false;
                OpcionesExportar = null;
            }
        }

        public GridToolbarModel(IEnumerable<BotonGridModel> opcionesExportar,
            IEnumerable<string> camposBuscar = null, string placeHolderBuscar = null,
            bool agregar = true): this(camposBuscar, placeHolderBuscar, agregar)
        {
            OpcionesExportar = opcionesExportar;
        }

        public bool Agregar { get; set; }
        public bool Buscar { get; set; }
        public IEnumerable<string> CamposBuscar { get; set; }
        public string PlaceHolderBuscar {get; set;}
        public IEnumerable<BotonGridModel> OpcionesExportar { get; set; }

        public IEnumerable<BotonGridModel> BotonesExtra { get; set; }

        // JSON con los textos traducidos para la interfaz de usuario.
        public string Textos { get; set; }
    }
}