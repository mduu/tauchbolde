using System.Text;

namespace Tauchbolde.Application.Services
{
    internal class IcalBuilder
    {
        private string titlePrefix = "";
        private DateTime? createTime;
        private DateTime startTime;
        private DateTime? endTime;
        private Guid id;
        private string title;
        private string description;
        private string location;
        private string meetingPoint;

        public IcalBuilder TitlePrefix(string titlePrefix)
        {
            this.titlePrefix = titlePrefix;
            return this;
        }

        public IcalBuilder CreateTime(DateTime? createTime = null)
        {
            this.createTime = createTime;
            return this;
        }

        public IcalBuilder StartTime(DateTime startTime)
        {
            this.startTime = new DateTime(startTime.Ticks, DateTimeKind.Local);
            return this;
        }

        public IcalBuilder EndTime(DateTime? endTime)
        {
            this.endTime = endTime.HasValue ? new DateTime(endTime.Value.Ticks, DateTimeKind.Local) : (DateTime?) null;
            return this;
        }

        public IcalBuilder Id(Guid id)
        {
            this.id = id;
            return this;
        }

        public IcalBuilder Title(string title)
        {
            this.title = title;
            return this;
        }

        public IcalBuilder Description(string description)
        {
            this.description = description;
            return this;
        }

        public IcalBuilder Location(string location)
        {
            this.location = location;
            return this;
        }

        public IcalBuilder MeetingPoint(string meetingPoint)
        {
            this.meetingPoint = meetingPoint;
            return this;
        }

        public Stream Build()
        {
            const string dateFormat = "yyyyMMddTHHmmss";
            var createAt = createTime ?? DateTimeOffset.Now;
            var createAtString = createAt.ToString(dateFormat);
            var evtEndTime = endTime ?? startTime.AddHours(3);
            var desc = !string.IsNullOrWhiteSpace(meetingPoint)
                ? $"Treffpunkt: {meetingPoint}\\n" + description
                : description;
            desc = desc
                .Replace("\n", @"\\n")
                .Replace(",", @"\,")
                .Replace("_", "")
                .Replace("*", "");

            var sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("PRODID:-//Tauchbolde//TauchboldeWebsite//EN");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("METHOD:PUBLISH");

            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine("DTSTART:" + startTime.ToString(dateFormat));
            sb.AppendLine("DTEND:" + evtEndTime.ToString(dateFormat));
            sb.AppendLine("DTSTAMP:" + createAtString);
            sb.AppendLine("UID:" + id);
            sb.AppendLine("CREATED:" + createAtString);
            sb.AppendLine("DESCRIPTION:" + desc);
            sb.AppendLine("LAST-MODIFIED:" + createAtString);
            sb.AppendLine("LOCATION:" + location);
            sb.AppendLine("SEQUENCE:0");
            sb.AppendLine("STATUS:CONFIRMED");
            sb.AppendLine("X-ALT-DESC;FMTTYPE=text/html:" + titlePrefix + title);
            sb.AppendLine("SUMMARY:" + titlePrefix + title);
            sb.AppendLine("TRANSP:OPAQUE");
            sb.AppendLine("END:VEVENT");

            sb.AppendLine("END:VCALENDAR");
            return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
        }
    }
}