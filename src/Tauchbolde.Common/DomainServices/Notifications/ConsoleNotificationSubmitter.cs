using System;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    public class ConsoleNotificationSubmitter : INotificationSubmitter
    {
        /// <inheritdoc />
#pragma warning disable 1998
        public async Task SubmitAsync(ApplicationUser recipient, string content)
#pragma warning restore 1998
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine($"Sent notification to {recipient.Email}: {content}.");

            Console.ForegroundColor = oldColor;
        }
    }
}
