using System;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    public class ConsoleNotificationSubmitter : INotificationSubmitter
    {
        /// <inheritdoc />
        public async Task SubmitAsync(Diver recipient, string content)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine($"Sent notification to {recipient.User.Email}: {content}.");

            Console.ForegroundColor = oldColor;

            await Task.FromResult<object>(null);
        }
    }
}
