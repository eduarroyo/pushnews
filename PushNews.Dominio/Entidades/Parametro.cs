namespace PushNews.Dominio.Entidades
{
    public class Parametro
    {
        public long ParametroID { get; set; }
        public long? AplicacionID { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }

        public virtual Aplicacion Aplicacion { get; set; }
    }
}