using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.DomainServices.Notifications;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.DataAccess;
using Microsoft.Extensions.Logging;

namespace Tauchbolde.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ILogger logger;
        private readonly INotificationFormatter formatter;
        private readonly INotificationSubmitter submitter;
        private readonly IDiverRepository userRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly ApplicationDbContext databaseContext;
        private readonly INotificationSender notificationSender;

        public NotificationController(
            ILoggerFactory loggerFactory,
            ApplicationDbContext databaseContext,
            INotificationRepository notificationRepository,
            IDiverRepository userRepository,
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
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
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
                            notificationRepository,
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