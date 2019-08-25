namespace Tauchbolde.Application.Services.Telemetry
{
    public static class TelemetryEventNames
    {
        public const string LogbookEntryCreated = nameof(LogbookEntryCreated);
        public const string LogbookEntryEdited = nameof(LogbookEntryEdited);
        public const string LogbookEntryPublished = nameof(LogbookEntryPublished);
        public const string LogbookEntryUnpublished = nameof(LogbookEntryUnpublished);
        public const string LogbookEntryDeleted = nameof(LogbookEntryDeleted);
        public const string NewEventComment = nameof(NewEventComment);
        public const string EditEventComment = nameof(EditEventComment);
        public const string DeleteEventComment = nameof(DeleteEventComment);
        public const string ParticipationChanged = nameof(ParticipationChanged);
    }
}