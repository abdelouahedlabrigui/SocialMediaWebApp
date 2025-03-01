using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.TransformersModels
{
    public class LlmQuestionAnswering
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Question { get; set; }
        public float Score { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string? Answer { get; set; }
        public string? CreatedAT { get; set; }
    }
}