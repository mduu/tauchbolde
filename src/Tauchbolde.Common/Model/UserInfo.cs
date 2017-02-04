using System;
using System.Collections.Generic;
using System.Text;

namespace Tauchbolde.Common.Model
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string WebsiteUrl { get; set; }
        public string TwitterHandle { get; set; }
        public string SkypeId { get; set; }
        public string Slogan { get; set; }
        public string Ausbildungen { get; set; }
        public string Experience { get; set; }

        public int NotificationIntervalInHours { get; set; }
    }
}
