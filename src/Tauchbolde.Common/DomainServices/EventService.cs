using System;
using System.IO;
using System.Text;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices
{
    public class EventService : IEventService
    {
        public Stream CreateIcalForEvent(Event evt)
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));

            var sb = new StringBuilder();
            var DateFormat = "yyyyMMddTHHmmssZ";
            var now = DateTime.Now.ToUniversalTime().ToString(DateFormat);

            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("PRODID:-//Tauchbolde//TauchboldeWebsite//EN");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("METHOD:PUBLISH");


            var evtEndTime = evt.EndTime ?? evt.StartTime.AddHours(4);
            var dtStart = Convert.ToDateTime(evt.StartTime);
            var dtEnd = Convert.ToDateTime(evtEndTime);

            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine("DTSTART:" + dtStart.ToUniversalTime().ToString(DateFormat));
            sb.AppendLine("DTEND:" + dtEnd.ToUniversalTime().ToString(DateFormat));
            sb.AppendLine("DTSTAMP:" + now);
            sb.AppendLine("UID:" + evt.Id);
            sb.AppendLine("CREATED:" + now);
            sb.AppendLine("X-ALT-DESC;FMTTYPE=text/html:" + evt.Description);
            sb.AppendLine("DESCRIPTION:" + evt.Description);
            sb.AppendLine("LAST-MODIFIED:" + now);
            sb.AppendLine("LOCATION:" + evt.Location);
            sb.AppendLine("SEQUENCE:0");
            sb.AppendLine("STATUS:CONFIRMED");
            sb.AppendLine("SUMMARY:" + evt.Name);
            sb.AppendLine("TRANSP:OPAQUE");
            sb.AppendLine("END:VEVENT");
            //}

            sb.AppendLine("END:VCALENDAR");

            var calendarBytes = Encoding.UTF8.GetBytes(sb.ToString());

            return new MemoryStream(calendarBytes);
        }
    }
}
