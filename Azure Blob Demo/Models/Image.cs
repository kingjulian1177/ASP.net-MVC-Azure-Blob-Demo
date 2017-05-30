using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Blob_Demo.Models
{
    public class Image
    {
        public string Id { get; set; }

        public string ImageUrl { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}