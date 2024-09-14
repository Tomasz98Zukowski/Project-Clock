using ProjectClock.BusinessLogic.Email.Models.Email;
using static ProjectClock.BusinessLogic.Email.Models.Email.EmailModel;

namespace ProjectClock.BusinessLogic.Email.EmailSender;

internal interface IEmailSender
{
    Task SendEmail(string email, string subject, string body, List<EmailAttachment>? emailAttachment = null);
    Task SendEmail(EmailModel emailModel);

}
