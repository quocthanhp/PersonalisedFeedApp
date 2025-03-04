using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SocialMedia : ContentItem
    {
        public string? Thumbnail { get; set; }
        public int? Upvotes { get; set; }
    }
}