using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tauchbolde.Common.Model
{
    public class PostImage
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }
        public Post ParentPost { get; set; }

        public string Caption { get; set; }

        public string ImageUrlThumbnail { get; set; }

        [Required]
        public string ImageUrlLarge { get; set; }
    }
}
