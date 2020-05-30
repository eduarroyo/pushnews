namespace PushNews.WebApp.Models.UI
{
    public class BotonGridModel
    {
        public enum Posiciones { Inicial, Media, Final }

        public BotonGridModel(string texto, string icono, string claseFuncion, string claseBoton = "", string atributos = "")
            : this(texto, icono, claseFuncion, Posiciones.Media, claseBoton, atributos)
        {}

        public BotonGridModel(string texto, string icono, string claseFuncion, Posiciones posicion, string claseBoton = "", string atributos = "")
        {
            Texto = texto;
            Icono = icono;
            ClaseFuncion = claseFuncion;
            ClaseBoton = claseBoton;
            Atributos = atributos;
            Posicion = posicion;
        }

        public string Texto { get; set; }

        public string Icono { get; set; }

        public string ClaseBoton { get; set; }

        public Posiciones Posicion { get; set; }

        public string ClaseFuncion { get; set; }

        public string Atributos { get; set; }
    }
}