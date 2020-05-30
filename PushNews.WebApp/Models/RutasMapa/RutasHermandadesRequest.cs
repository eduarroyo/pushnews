namespace PushNews.WebApp.Models.RutasMapa
{
    public class RutasHermandadesRequest
    {
        public string Subdominio { get; set; }
        public long? Latitud { get; set; }
        public long? Longitud { get; set; }
        public double Distancia { get; set; }
    }
}