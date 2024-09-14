namespace ProjectClock.BusinessLogic.Email.Models.Email;

public class EmailModel
{
    public string EmailAdress { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public IEnumerable<EmailAttachment>? Attachments { get; set; }

    public class EmailAttachment
    {
        public string Name { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[] Data = new byte[0];
    }

}
