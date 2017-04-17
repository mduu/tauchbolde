using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.DomainServices.Notifications;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationFormatter _formatter;
        private readonly INotificationSubmitter _submitter;
        private readonly IApplicationUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ApplicationDbContext _databaseContext;
        private readonly INotificationSender _notificationSender;

        public NotificationController(
            ApplicationDbContext databaseContext,
            INotificationRepository notificationRepository,
            IApplicationUserRepository userRepository,
            INotificationSender sender,
            INotificationFormatter formatter,
            INotificationSubmitter submitter)
        {
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            _submitter = submitter ?? throw new ArgumentNullException(nameof(submitter));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _notificationSender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProcessNotification()
        {
            await _notificationSender.Send(
                _notificationRepository,
                _userRepository,
                _formatter,
                _submitter);

            return StatusCode(200);
        }
    }
}