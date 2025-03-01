using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.LanguageProcessing
{
    public class NounChunk
    {
        public int Id { get; set; }
        public string? Title { get; set; } 
        public string? SearchString { get; set; } 
        public string? Text { get; set; }
        public string? RootText { get; set; }
        public string? RootDep { get; set; }
        public string? RootHead { get; set; }
        public string? CreatedAT { get; set; }
    }
}