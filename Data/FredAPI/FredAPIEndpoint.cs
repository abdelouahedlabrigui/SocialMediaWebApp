using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.FredAPI
{
    public class FredAPIEndpoint
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? GetRequest { get; set; }
        public string? CreatedAT { get; set; }
    }
}