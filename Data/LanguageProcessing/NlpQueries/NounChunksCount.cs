using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.LanguageProcessing.NlpQueries
{
    public class NounChunksCount
    {
        public string? Title { get; set; } 
        public string? SearchString { get; set; } 
        public string? RootText { get; set; }
        public string? RootDep { get; set; }
        public string? RootHead { get; set; }
        public int Count { get; set; }
    }
}