using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using log4net;
using System.Configuration;
using System.Net.Mail;
using PushNews.WebApp.Areas.Backend.Models.PlantillasEmail;
using System.Text;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Services
{
    public class EmailService : IIdentityMessageService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EmailService).FullName);

        public async Task EnviarEmailRecuperarClave(string destinatario, string cuerpo, RestablecerClave modelo)
        {
            AlternateView html = AlternateView.CreateAlternateViewFromString(cuerpo, Encoding.UTF8, "text/html");
            LinkedResource logo = new LinkedResource(modelo.RutaLogo);
            logo.ContentId = "logo";
            html.LinkedResources.Add(logo);
            MailMessage msg = PrepararMensaje(destinatario, Txt.Account.RestablecerClaveEmailAsunto, true);
            msg.AlternateViews.Add(html);
            await SendAsync(msg);
        }

        public async Task SendAsync(MailMessage message)
        {
            try
            {
                SmtpClient client = GetSmtpClient();
                await client.SendMailAsync(message);
            }
            catch (SmtpException ex)
            {
                log.Error(message: "Error al enviar email.", exception: ex);
                throw ex;
            }
        }

        public async Task SendAsync(IdentityMessage message)
        {
            var msg = PrepararMensaje(message.Destination, message.Subject, false, message.Body);
            await SendAsync(msg);
        }

        private MailMessage PrepararMensaje(string destinatario, string asunto, bool html, string body = null)
        {
            var msg = new MailMessage();
            msg.From = new MailAddress(ConfigurationManager.AppSettings["mailAccount"],
                ConfigurationManager.AppSettings["mailName"], Encoding.UTF8);
            msg.SubjectEncoding = Encoding.UTF8;
            msg.BodyEncoding = Encoding.UTF8;
            msg.To.Add(destinatario);
            msg.Subject = asunto;
            if (!string.IsNullOrEmpty(body))
            {
                msg.Body = body;
            }
            msg.IsBodyHtml = html;
            return msg;
        }

        private SmtpClient GetSmtpClient()
        {
            var client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(
                ConfigurationManager.AppSettings["mailAccount"],
                ConfigurationManager.AppSettings["mailPassword"]);
            client.Port = int.Parse(ConfigurationManager.AppSettings["mailPort"]);
            client.Host = ConfigurationManager.AppSettings["mailHost"];
            client.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["mailSSL"]);
            return client;
        }
    }
}
