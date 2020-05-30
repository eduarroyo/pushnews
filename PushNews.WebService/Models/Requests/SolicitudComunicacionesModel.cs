namespace PushNews.WebService.Models
{
    public class SolicitudComunicacionesModel: SolicitudModel
    {
        public string UID { get; set; }
        public long TimeStamp { get; set; }
        public long? CategoriaID { get; set; }
        public long? ComunicacionID { get; set; }
    }

}
