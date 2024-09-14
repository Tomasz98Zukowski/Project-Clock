using Mailjet.Client;
using Newtonsoft.Json.Linq;
using ProjectClock.BusinessLogic.Email.EmailSender;
using ProjectClock.BusinessLogic.Email.Models.Email;

namespace ProjectClock.BusinessLogic.Email.EmailProvider;

public class MailJetProvider : EmailSender.EmailSender, IEmailSender
{
    protected override async Task Send(EmailModel emailModel)
    {
        try
        {
            JArray jArray = new JArray();
            JArray attachments = new JArray();
            if(emailModel.Attachments != null && emailModel.Attachments.Count() > 0)
            {
                emailModel.Attachments.ToList().ForEach(attachment => attachments.Add(
                    new JObject()
                    {
                        new JProperty("Content-Type", attachment.ContentType),
                        new JProperty("Filename", attachment.Name),
                        new JProperty("Content", Convert.ToBase64String(attachment.Data)
                    )}));
            }
            jArray.Add(new JObject
            {
                new JProperty("FromEmail", "tomaszzukowskibp@gmail.com"),
                new JProperty("FromName", "Project Clock Team"),
                new JProperty("Recipients", new JArray
                {
                    new JObject
                    {
                        new JProperty("Email", emailModel.EmailAdress),
                        new JProperty("Name", emailModel.EmailAdress)
                    }
                }),
                new JProperty("Subject", emailModel.Subject),
                new JProperty("Text-part", emailModel.Body),
                new JProperty("Html-part", emailModel.Body),
                new JProperty("Attachments", attachments)
            });
            var client = EmailSender.EmailSender.CreateMailJetClient();
            var request = new MailjetRequest()
            {
                Resource = Mailjet.Client.Resources.Send.Resource
            }
            .Property(Mailjet.Client.Resources.Send.Messages, jArray);
            var response = await client.PostAsync(request);
            Console.WriteLine($"Send result {response.StatusCode} with message: {response.Content}");
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        }
    }

