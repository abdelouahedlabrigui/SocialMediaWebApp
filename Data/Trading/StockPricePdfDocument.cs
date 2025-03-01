using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.Trading
{
    public class StockPricePdfDocument
    {
        public int Id { get; set; }
        public string? Company { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? FileName { get; set; }
        public string? Base64Content { get; set; }
        public string? CreatedAT { get; set; }
    }
}