using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.DomainServices.Notifications;
using Microsoft.Extensions.Logging;

namespace Tauchbolde.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ILogger logger;
        private readonly INotificationFormatter formatter;
        private readonly INotificationSubmitter submitter;
        private readonly INotificationSender notificationSender;

        public NotificationController(
            ILoggerFactory loggerFactory,
            INotificationSender sender,
            INotificationFormatter formatter,
            INotificationSubmitter submitter)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            this.formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            this.submitter = submitter ?? throw new ArgumentNullException(nameof(submitter));
            notificationSender = sender ?? throw new ArgumentNullException(nameof(sender));

            logger = loggerFactory.CreateLogger<NotificationController>();
        }

        public async Task<IActionResult> Process()
        {
            using (logger.BeginScope("Processing Pending Notification"))
            {
                try
                {
                    logger.LogTrace("Process and send notifications ...");
                    await notificationSender.SendAsync(
                            formatter,
                            submitter);

                    logger.LogInformation("Pending notifications processed successfully.");
                    return StatusCode(200);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while processing notifications!");
                    return BadRequest();
                }
            }
        }
    }
}