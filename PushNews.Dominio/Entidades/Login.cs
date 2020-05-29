namespace PushNews.Dominio.Entidades
{
    public class Login
    {
        public string ProveedorLogin { get; set; }
        public string ProveedorClave { get; set; }
        public long UsuarioID { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}