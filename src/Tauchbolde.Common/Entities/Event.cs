using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Tauchbolde.Common.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Organisator { get; set; }
        public string Location { get; set; }
        public string MeetingPoint { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
