using log4net;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PushNews.WebService.Services
{
    public class EmailService : IIdentityMessageService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EmailService).FullName);

        public async Task SendAsync(IdentityMessage message)
        {
            try
            {
                await ConfigurarEnviar(message);
            }
            catch (SmtpException ex)
            {
                log.Error(message: "Error al enviar email.", exception: ex);
                throw ex;
            }
        }

        public Task ConfigurarEnviar(IdentityMessage message)
        {
            var msg = new MailMessage();
            msg.To.Add(message.Destination);
            msg.From = new MailAddress(ConfigurationManager.AppSettings["mailAccount"],
                ConfigurationManager.AppSettings["mailName"], System.Text.Encoding.UTF8);
            msg.Subject = message.Subject;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = message.Body;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;

            var client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(
                ConfigurationManager.AppSettings["mailAccount"],
                ConfigurationManager.AppSettings["mailPassword"]);
            client.Port = int.Parse(ConfigurationManager.AppSettings["mailPort"]);
            client.Host = ConfigurationManager.AppSettings["mailHost"];
            client.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["mailSSL"]);

            return client.SendMailAsync(msg);
        }
    }
}
