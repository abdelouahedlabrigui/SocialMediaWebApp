using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.YoutubeAPI
{
    public class QueryResult
    {
        public int Id { get; set; }
        public string? VideoId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PublishedAt { get; set; }
        public string? ChannelTitle { get; set; }
        public string? ThumbnailUrl { get; set; } // Added ThumbnailUrl
        public TimeSpan Duration { get; set; } //Added Duration
    }
}