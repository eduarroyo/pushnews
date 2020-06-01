using RestSharp;
using System;
using System.Net;
using Newtonsoft.Json;

namespace PushNews.AppceleratorPush
{
    public class CloudPush: IDisposable
    {
        private string url = "https://api.cloud.appcelerator.com";
        private string key;
        private string usuario;
        private string clave;
        private RestClient client;

        public string Resultado { get; set; }

        public CloudPush(string key, string usuario, string clave)
        {
            this.key = key;
            this.usuario = usuario;
            this.clave = clave;
        }

        public Respuesta EnviarMensaje(string titulo, string mensaje, bool vibrar, string canal, string id)
        {
            if (client == null)
            {
                Login();
            }

            client.BaseUrl = new Uri(url);
            //string par = $"{{\"title\": \"{titulo}\", \"badge\": 1, \"alert\": \"{mensaje}\", \"sound\": \"default\", \"vibrate\": {(vibrar ? "true" : "false")}, \"ID\": \"{id}\", \"icon\": \"appicon\"}}";

            Payload p = new Payload
            {
                title = titulo,
                badge = 1,
                alert = mensaje,
                sound = "default",
                vibrate = vibrar,
                ID = id,
                icon = "appicon"
            };
            string payload = JsonConvert.SerializeObject(p);

            RestRequest request = new RestRequest($"/v1/push_notification/notify.json?key={key}", Method.POST);
            request.AddUrlSegment("appkey", key);
            request.AddParameter("channel", canal);
            request.AddParameter("payload", payload);
            request.AddParameter("to_ids", "everyone");
            
            IRestResponse response = client.Execute(request);
            Respuesta r = JsonConvert.DeserializeObject<Respuesta>(response.Content);
            return r;
        }

        private void Login()
        {
            client = new RestClient(url);
            client.CookieContainer = new CookieContainer();
            RestRequest request = new RestRequest($"/v1/users/login.json?key={key}", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddUrlSegment("appkey", key);
            request.AddParameter("login", usuario);
            request.AddParameter("password", clave);
            var response = client.Execute(request);
        }

        public void Dispose()
        {

        }

    }
}
