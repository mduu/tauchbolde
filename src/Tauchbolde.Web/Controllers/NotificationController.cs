using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Driver.DataAccessSql;

namespace Tauchbolde.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ILogger logger;
        [NotNull] private readonly ApplicationDbContext context;
        private readonly INotificationFormatter formatter;
        private readonly INotificationSubmitter submitter;
        private readonly INotificationSender notificationSender;

        public NotificationController(
            [NotNull] ApplicationDbContext context,
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] INotificationSender sender,
            [NotNull] INotificationFormatter formatter,
            [NotNull] INotificationSubmitter submitter)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            this.context = context ?? throw new ArgumentNullException(nameof(context));
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
                            submitter,
                            async () => await context.SaveChangesAsync());

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