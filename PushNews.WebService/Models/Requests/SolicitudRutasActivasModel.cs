namespace PushNews.WebService.Models.Requests
{
    public class SolicitudRutasActivasModel : SolicitudModel
    {
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public double Distancia { get; set; }
    }
}