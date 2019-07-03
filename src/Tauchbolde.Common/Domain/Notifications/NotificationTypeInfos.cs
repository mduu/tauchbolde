using System.Collections.Generic;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Domain.Notifications
{
    public class NotificationTypeInfos : INotificationTypeInfos
    {
        private static readonly IDictionary<NotificationType, NotificationTypeInfo> Infos = 
            new Dictionary<NotificationType, NotificationTypeInfo> {
                // ReSharper disable StringLiteralTypo
                { NotificationType.NewEvent, new NotificationTypeInfo { Caption = "Neue Aktivität" }},
                { NotificationType.CancelEvent, new NotificationTypeInfo { Caption = "Aktivität abgesagt" }},
                { NotificationType.EditEvent, new NotificationTypeInfo { Caption = "Aktivität geändert" }},
                { NotificationType.Commented, new NotificationTypeInfo { Caption = "Neuer Kommentar" }},
                { NotificationType.Accepted, new NotificationTypeInfo { Caption = "Zusage" }},
                { NotificationType.Declined, new NotificationTypeInfo { Caption = "Absage" }},
                { NotificationType.Tentative, new NotificationTypeInfo {Caption = "Vorbehalt" }},
                { NotificationType.Neutral, new NotificationTypeInfo { Caption = "Unklar" }},
                { NotificationType.NewLogbookEntry, new NotificationTypeInfo { Caption = "Neuer Logbucheintrag" }},
                // ReSharper restore StringLiteralTypo
            };

        /// <inheritdoc />
        public string GetCaption(NotificationType notificationType)
        {
            return Infos.ContainsKey(notificationType)
                ? Infos[notificationType].Caption
                // ReSharper disable once StringLiteralTypo
                : "<unbekannt>";
        }
    }
}