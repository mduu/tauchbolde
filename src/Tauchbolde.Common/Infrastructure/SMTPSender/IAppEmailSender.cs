using System.Threading.Tasks;

namespace Tauchbolde.Common.Infrastructure.SMTPSender
{
    public interface IAppEmailSender
    {
        Task SendAsync(
            string receiverName,
            string receiverEmail,
            string subject,
            string content,
            string senderEmail = null,
            string senderName = null);
    }
}