using System.Collections.Generic;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    public class NotificationTypeInfos : INotificationTypeInfos
    {
        private const string TheRedColor = "#cc0000";
        private const string TheGreenColor = "#006600";
        private const string TheBlueColor = "#003399";

        private static readonly IDictionary<NotificationType, NotificationTypeInfo> Infos = 
            new Dictionary<NotificationType, NotificationTypeInfo> {
                { NotificationType.NewEvent, new NotificationTypeInfo { Color = TheGreenColor, Caption = "Neue Aktivität" }},
                { NotificationType.CancelEvent, new NotificationTypeInfo { Color = TheRedColor, Caption = "Aktivität abgesagt" }},
                { NotificationType.EditEvent, new NotificationTypeInfo { Color = TheBlueColor, Caption = "Aktivität geändert" }},
                { NotificationType.Commented, new NotificationTypeInfo { Color = TheGreenColor, Caption = "Neuer Kommentar" }},
                { NotificationType.Accepted, new NotificationTypeInfo { Color = TheGreenColor, Caption = "Zusage" }},
                { NotificationType.Declined, new NotificationTypeInfo { Color = TheRedColor, Caption = "Absage" }},
                { NotificationType.Tentative, new NotificationTypeInfo { Color = TheBlueColor, Caption = "Vorbehalt" }},
                { NotificationType.Neutral, new NotificationTypeInfo { Color = TheBlueColor, Caption = "Unklar" }},
            };

        /// <inheritdoc />
        public NotificationTypeInfo GetInfo(NotificationType notificationType)
        {
            return Infos[notificationType];
        }
    }
}