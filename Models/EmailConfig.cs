namespace OnlineAuction.Models
{
    public class EmailConfig
    {
        public string? SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string? Email { get; set; }
        public string? AppPassword { get; set; }
    }
}
