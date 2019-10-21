using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Administration.GetMassMailUseCase
{
    public class GetMassMailDetailsOutput
    {
        public GetMassMailDetailsOutput([NotNull] IEnumerable<MailRecipient> mailRecipients)
        {
            MailRecipients = mailRecipients ?? throw new ArgumentNullException(nameof(mailRecipients));
        }

        public IEnumerable<MailRecipient> MailRecipients { get; }

        public class MailRecipient
        {
            public MailRecipient([NotNull] string fullname, [NotNull] string emailAddress)
            {
                Fullname = fullname ?? throw new ArgumentNullException(nameof(fullname));
                EmailAddress = emailAddress ?? throw new ArgumentNullException(nameof(emailAddress));
            }

            [NotNull] public string Fullname { get; }
            [NotNull] public string EmailAddress { get; }
        }
    }
}