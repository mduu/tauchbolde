using System;
using System.IO;
using System.Text;

namespace Tauchbolde.Application.Services
{
    internal class IcalBuilder
    {
        private DateTime? createTime;
        private DateTime startTime;
        private DateTime? endTime;
        private Guid id;
        private string title;
        private string description;
        private string location;
        private string meetingPoint;

        public IcalBuilder CreateTime(DateTime? createTime = null)
        {
            this.createTime = createTime;
            return this;
        }

        public IcalBuilder StartTime(DateTime startTime)
        {
            this.startTime = startTime;
            return this;
        }

        public IcalBuilder EndTime(DateTime? endTime)
        {
            this.endTime = endTime;
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
            var sb = new StringBuilder();
            const string dateFormat = "yyyyMMddTHHmmssZ";
            var createAt = createTime ?? DateTimeOffset.Now;
            var createAtString = createAt.ToString(dateFormat);
            var evtEndTime = endTime ?? startTime.AddHours(3);
            var dtStart = startTime.ToLocalTime();
            var dtEnd = evtEndTime.ToLocalTime();
            var desc = !string.IsNullOrWhiteSpace(meetingPoint)
                ? $"Treffpunkt: {meetingPoint}" + Environment.NewLine + description
                : description;
            
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("PRODID:-//Tauchbolde//TauchboldeWebsite//EN");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("METHOD:PUBLISH");

            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine("DTSTART:" + dtStart.ToString(dateFormat));
            sb.AppendLine("DTEND:" + dtEnd.ToString(dateFormat));
            sb.AppendLine("DTSTAMP:" + createAtString);
            sb.AppendLine("UID:" + id);
            sb.AppendLine("CREATED:" + createAtString);
            sb.AppendLine("X-ALT-DESC;FMTTYPE=text/html:" + desc);
            sb.AppendLine("DESCRIPTION:" + desc);
            sb.AppendLine("LAST-MODIFIED:" + createAtString);
            sb.AppendLine("LOCATION:" + location);
            sb.AppendLine("SEQUENCE:0");
            sb.AppendLine("STATUS:CONFIRMED");
            sb.AppendLine("SUMMARY:" + title);
            sb.AppendLine("TRANSP:OPAQUE");
            sb.AppendLine("END:VEVENT");
            sb.AppendLine("END:VCALENDAR");

            return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
        }
    }
}