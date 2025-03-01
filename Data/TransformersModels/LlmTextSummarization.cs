using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.TransformersModels
{
    public class LlmTextSummarization
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? SummaryText { get; set; }
        public string? CreatedAT { get; set; }
    }
}