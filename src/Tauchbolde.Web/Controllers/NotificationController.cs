using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.DomainServices.Notifications;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;
using Microsoft.Extensions.Logging;

namespace Tauchbolde.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ILogger _logger;
        private readonly INotificationFormatter _formatter;
        private readonly INotificationSubmitter _submitter;
        private readonly IDiverRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ApplicationDbContext _databaseContext;
        private readonly INotificationSender _notificationSender;

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

            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            _submitter = submitter ?? throw new ArgumentNullException(nameof(submitter));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _notificationSender = sender ?? throw new ArgumentNullException(nameof(sender));

            _logger = loggerFactory.CreateLogger<NotificationController>();
        }

        public async Task<IActionResult> Process()
        {
            using (_logger.BeginScope("Processing Pending Notification"))
            {
                try
                {
                    _logger.LogTrace("Process and send notifications ...");
                    await _notificationSender.SendAsync(
                            _notificationRepository,
                            _formatter,
                            _submitter);

                    _logger.LogInformation("Pending notifications processed successfully.");
                    return StatusCode(200);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing notifications!");
                    return BadRequest();
                }
            }
        }
    }
}