namespace Tauchbolde.Application.Services.Notifications
{
    public interface INotificationSender
    {
        Task SendAsync(INotificationFormatter notificationFormatter,
            INotificationSubmitter notificationSubmitter,
            Func<Task> saver);
    }
}