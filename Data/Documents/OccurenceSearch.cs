using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.Documents
{
    public class OccurenceSearch
    {
        public string? Title { get; set; }
        public string? SearchString { get; set; }
        public int Occurrence { get; set; }
        public string? Context { get; set; }
        public string? Sector { get; set; }
    }
}