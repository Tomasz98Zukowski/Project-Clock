using Microsoft.Extensions.Hosting;
using ProjectClock.BusinessLogic.Email.EmailProvider;
using ProjectClock.BusinessLogic.Email.EmailSender;
using ProjectClock.BusinessLogic.Email.Models.Email;
using System.Threading.Tasks.Dataflow;

namespace ProjectClock.BusinessLogic.Services.EmailHostedServices;

public class EmailHostedServices : IHostedService, IDisposable, IEmailHostedServices
{
    private Task? _sendTask;
    private CancellationTokenSource? _cancellationToken;
    private readonly BufferBlock<EmailModel> _model;
    private readonly IEmailSender _mailSender;

    public EmailHostedServices()
    {
        _model = new BufferBlock<EmailModel>();
        _cancellationToken = new CancellationTokenSource();
        _mailSender = new MailJetProvider();
    }

    public async Task SendMailAsync(EmailModel model) => await _model.SendAsync(model);

    public void Dispose()
    {
        DestroyTask();
    }

    private void DestroyTask()
    {
        try
        {
            if (_cancellationToken != null)
            {
                _cancellationToken.Cancel();
                _cancellationToken = null;
            }
            Console.WriteLine("[EMAIL SERVICE] DESTROY SERVICE");
        }
        catch
        {
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("[EMAIL SERVICE] START SERVICE");
        _sendTask = BackgroundSendEmailAsync(_cancellationToken!.Token);
        return Task.CompletedTask;
    }

    private async Task? BackgroundSendEmailAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var email = await _model.ReceiveAsync();
                await _mailSender.SendEmail(email);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EMAIL SERVICE] Exeption: {ex.Message}");
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        DestroyTask();
        await Task.WhenAny(_sendTask!, Task.Delay(Timeout.Infinite, cancellationToken));
    }
}
