using System;

namespace Tauchbolde.Domain.Helpers
{
    public static class DateTimeExtensions
    {
        public const string SwissDateFormat = "dd.MM.yyyy";
        public const string SwissTimeFormat = "HH:mm";
        public const string SwissDateTimeFormat = "dd.MM.yyyy HH:mm";

        public static string ToStringSwissDate(this DateTime dateTime)
            => dateTime.ToString(SwissDateFormat);

        public static string ToStringSwissDate(this DateTime? dateTime) 
            => dateTime.HasValue ? dateTime.Value.ToStringSwissDate() : "";

        public static string ToStringSwissTime(this DateTime dateTime)
            => dateTime.ToString(SwissTimeFormat);

        public static string ToStringSwissTime(this DateTime? dateTime)
            => dateTime.HasValue ? dateTime.Value.ToStringSwissTime() : "";

        public static string ToStringSwissDateTime(this DateTime dateTime)
            => dateTime.ToString(SwissDateTimeFormat);

        public static string ToStringSwissDateTime(this DateTime? dateTime)
            => dateTime.HasValue ? dateTime.Value.ToStringSwissDateTime() : "";
    }
}
