namespace Tauchbolde.Common.DomainServices.SMTPSender
{
    public class SmtpSenderConfiguration
    {
        public SmtpSenderConfiguration()
        {
            Host = "localhost";
            Port = 25;
            SystemSenderName = "Webmaster";
            Ssl = "None";
        }
        
        public string Host { get; set; }
        public int Port { get; set; }
        /// <summary>
        /// Gets or sets the security method.
        /// Possible values are: "None", "Auto", "SslOnConnect", "StartTls", "StartTlsWhenAvailable".
        /// </summary>
        public string Ssl { get; set; }
        public bool UseAuth { get; set; }
        public string AuthUsername { get; set; }
        public string AuthPassword { get; set; }
        
        public string SystemSenderName { get; set; }
        public string SystemSenderEmailAddress { get; set; }
    }
}
