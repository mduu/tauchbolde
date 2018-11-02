using System.Threading.Tasks;

namespace Tauchbolde.Common.DomainServices.SMTPSender
{
    public interface IAppEmailSender
    {
        Task Send(
            string receiverName,
            string receiverEmail,
            string subject,
            string content,
            string senderEmail = null,
            string senderName = null);
    }
}