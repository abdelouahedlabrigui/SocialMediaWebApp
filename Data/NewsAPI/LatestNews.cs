using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.NewsAPI
{
    public class LatestNews
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? UrlToImage { get; set; }
        public string? PublishedAt { get; set; }
        public string? CreatedAT { get; set; }
    }
}