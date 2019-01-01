namespace Tauchbolde.Common.DomainServices.SMTPSender
{
    public class SmtpSenderConfiguration
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 25;

        /// <summary>
        /// Gets or sets the security method.
        /// Possible values are: "None", "Auto", "SslOnConnect", "StartTls", "StartTlsWhenAvailable".
        /// </summary>
        public string Ssl { get; set; } = "None";
        
        public bool UseAuth { get; set; }
        public string AuthUsername { get; set; }
        public string AuthPassword { get; set; }

        public string SystemSenderName { get; set; } = "Webmaster";
        public string SystemSenderEmailAddress { get; set; }
    }
}
