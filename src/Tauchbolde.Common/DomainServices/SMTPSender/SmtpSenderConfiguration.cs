using System.ComponentModel.DataAnnotations;

namespace Tauchbolde.Common.DomainServices.SMTPSender
{
    public class SmtpSenderConfiguration
    {
        /// <summary>
        /// Gets or sets the SMTP host / server name or address used to send emails.
        /// </summary>
        /// <value>The SMTP host.</value>
        [Required]
        public string Host { get; set; } = "localhost";
        
        /// <summary>
        /// Gets or sets the SMTP port number.
        /// </summary>
        /// <value>The SMTP port.</value>
        [Required]
        public int Port { get; set; } = 25;

        /// <summary>
        /// Gets or sets the name that appears as 'sender' in system email messages.
        /// </summary>
        /// <value>The sender name for emails.</value>
        [Required]
        public string SystemSenderName { get; set; } = "Webmaster";
        
        /// <summary>
        /// Gets or sets the sender email address in system email messages.
        /// </summary>
        /// <value>The sender email address.</value>
        [Required]
        public string SystemSenderEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the root URL used in Links / Urls generated in email messages.
        /// </summary>
        /// <value>The root URL.</value>
        [Required]
        public string RootUrl { get; set; }

        /// <summary>
        /// Gets or sets the security method.
        /// Possible values are: "None", "Auto", "SslOnConnect", "StartTls", "StartTlsWhenAvailable".
        /// </summary>
        public string Ssl { get; set; } = "None";
        
        /// <summary>
        /// Gets or sets a value indicating whether SMTP authentication must be used.
        /// </summary>
        /// <value><c>true</c> if use SMTP auth; otherwise, <c>false</c>.</value>
        public bool UseAuth { get; set; }
        
        /// <summary>
        /// Gets or sets the SMTP authentication username.
        /// </summary>
        /// <value>The SMTP auth username.</value>
        public string AuthUsername { get; set; }
        
        /// <summary>
        /// Gets or sets the SMTP authentication password.
        /// </summary>
        /// <value>The SMTP auth password.</value>
        public string AuthPassword { get; set; }

       
    }
}
