using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace PushNews.WebService.Services
{
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Conecte el servicio SMS aquí para enviar un mensaje de texto.
            return Task.FromResult(result: 0);
        }
    }
}