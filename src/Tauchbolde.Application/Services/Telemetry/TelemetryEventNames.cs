namespace Tauchbolde.Application.Services.Telemetry
{
    public static class TelemetryEventNames
    {
        public const string NewEvent = nameof(NewEvent);
        public const string EditEvent = nameof(EditEvent);
        public const string NewEventComment = nameof(NewEventComment);
        public const string EditEventComment = nameof(EditEventComment);
        public const string DeleteEventComment = nameof(DeleteEventComment);
        public const string ParticipationChanged = nameof(ParticipationChanged);
        public const string IcalRequested = nameof(IcalRequested);
        public const string LogbookEntryCreated = nameof(LogbookEntryCreated);
        public const string LogbookEntryEdited = nameof(LogbookEntryEdited);
        public const string LogbookEntryPublished = nameof(LogbookEntryPublished);
        public const string LogbookEntryUnpublished = nameof(LogbookEntryUnpublished);
        public const string LogbookEntryDeleted = nameof(LogbookEntryDeleted);
        public const string IdentityMailSent = nameof(IdentityMailSent);
        public const string UserProfileEdited = nameof(UserProfileEdited);
        public const string AvatarChanged = nameof(AvatarChanged);
    }
}