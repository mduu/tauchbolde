using System;
using System.Collections.Generic;
using System.Text;

namespace Tauchbolde.Common.Model
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public ApplicationUser User { get; set; }
        public Comment ParrentComment { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
