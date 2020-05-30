namespace PushNews.AppceleratorPush
{
    public class Respuesta
    {
        public Meta Meta { get; set; }
    }

    public class Meta
    {
        public int Code { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Method_Name { get; set; }
    }
}
