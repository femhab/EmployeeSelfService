namespace Business.Providers
{
    public class EmailConfig
    {
        public string Sender { get; set; }
        public string Bcc { get; set; }
        public string Cc { get; set; }
        public string Alias { get; set; }
        public string AdminEmail { get; set; }
        public SmtpConfig Smtp { get; set; }
    }

    public class SmtpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public bool UseSSL { get; set; }
    }
}
