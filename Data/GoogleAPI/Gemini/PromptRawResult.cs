using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.GoogleAPI.Gemini
{
    public class PromptRawResult
    {
        public int Id { get; set; }
        public string? PromptString { get; set; }
        public string? TextPath { get; set; }
        public string? CreatedAT { get; set; }

    }
}