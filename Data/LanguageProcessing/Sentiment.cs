using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.LanguageProcessing
{
    public class Sentiment
    {
        public int Id { get; set; }
        public string? Sentence { get; set; }
        public string? Title { get; set; } 
        public string? SearchString { get; set; }   
        public string? Positive { get; set; }
        public string? Negative { get; set; }
        public string? Neutral { get; set; }
        public string? Compound { get; set; }
        public string? CreatedAT { get; set; }
    }
}