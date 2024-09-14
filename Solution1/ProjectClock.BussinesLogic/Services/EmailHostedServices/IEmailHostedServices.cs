using ProjectClock.BusinessLogic.Email.Models.Email;

namespace ProjectClock.BusinessLogic.Services.EmailHostedServices
{
    public interface IEmailHostedServices
    {
        void Dispose();
        Task SendMailAsync(EmailModel model);
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}