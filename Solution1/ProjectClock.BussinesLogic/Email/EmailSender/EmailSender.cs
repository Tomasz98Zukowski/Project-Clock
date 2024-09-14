using Mailjet.Client;
using Newtonsoft.Json;
using ProjectClock.BusinessLogic.Email.Models.Email;
using ProjectClock.Database.Entities;
using static ProjectClock.BusinessLogic.Email.Models.Email.EmailModel;

namespace ProjectClock.BusinessLogic.Email.EmailSender;

public abstract class EmailSender : IEmailSender
{
    public static MailjetSettings DeserializedEmailSettings()
    {
        string jsonFilePath = "emailApi.json";
        string json = File.ReadAllText(jsonFilePath);

        var mailjetSettings = JsonConvert.DeserializeObject<Dictionary<string, MailjetSettings>>(json)["MailjetSettings"];

        return mailjetSettings;
    }
    public static MailjetClient CreateMailJetClient()
    {
        var mailjetSettings = DeserializedEmailSettings();
        return new MailjetClient(mailjetSettings.ApiKey, mailjetSettings.ApiSecret);
    }
    protected abstract Task Send(EmailModel emailModel);

    public async Task SendEmail(EmailModel emailModel)
    {
        await Send(emailModel);
    }

    public async Task SendEmail(string email, string subject, string body, List<EmailAttachment>? emailAttachment = null)
    {
        await Send(new EmailModel
        {
            EmailAdress = email,
            Subject = subject,
            Body = body,
            Attachments = emailAttachment
        });
    }
}
