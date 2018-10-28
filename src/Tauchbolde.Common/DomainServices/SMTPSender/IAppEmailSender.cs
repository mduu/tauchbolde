using System.Threading.Tasks;

namespace Tauchbolde.Common.DomainServices.SMTPSender
{
    public interface IAppEmailSender
    {
        Task Send(string receiverName, string receiverEmail, string senderName, string senderEmail, string subject, string content);
    }
}