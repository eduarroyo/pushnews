namespace PushNews.Dominio.Entidades
{
    public class Claim
    {
        public Claim()
        {}

        public Claim(long usuarioID, System.Security.Claims.Claim claim)
        {
            UsuarioID = usuarioID;
            Tipo = claim.Type;
            Valor = claim.Value;
        }

        public long ClaimID { get; set; }
        public string Tipo { get; set; }
        public string Valor { get; set; }
        public long UsuarioID { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}