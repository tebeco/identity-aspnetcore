using System.Threading.Tasks;

namespace IdentityByExamples.EmailService
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
