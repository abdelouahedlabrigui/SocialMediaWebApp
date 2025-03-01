using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.LanguageProcessing
{
    public class Entity
    {
        public int Id { get; set; }  
        public string? Title { get; set; } 
        public string? SearchString { get; set; }   
        public string? Text { get; set; }
        public string? StartChat { get; set; }
        public string? EndChar { get; set; }
        public string? Label { get; set; }
        public string? CreatedAT { get; set; }
    }
}