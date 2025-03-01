using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.LanguageProcessing.NlpQueries
{
    public class SentimentSearch
    {
        public int Id { get; set; }
        public string? PromptString { get; set; }
        public string? FilePath { get; set; }
        public string? State { get; set; }
        public string? ConcatenatedSentences { get; set; }
        public int TotalSentences { get; set; }
        public string? CreatedAT { get; set; }
    }
}