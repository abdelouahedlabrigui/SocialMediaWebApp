using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.LanguageProcessing
{
    public class LanguageFeature
    {
        public List<NounChunk>? NounChunksList { get; set; }
        public List<Sentiment>? SentimentsList { get; set; }
        public List<Entity>? EntitiesList { get; set; }
    }
}