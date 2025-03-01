using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialMediaWebApp.Data;

namespace SocialMediaWebApp.Pages.Trading
{
    public class VolumeBasedFeatures : PageModel
    {
        private readonly HttpClient? _httpClient;
        public VolumeBasedFeatures()
        {
            _httpClient = new HttpClient();
        }
        
        public List<string>? options = new List<string>() {
            "Stock Volume vs. Moving Average", 
            "Daily Volume Change (%)",
            "Stock Price vs. Volume Surges",
            "RSI for Stocks",
            "MACD for Stocks",
            "Volume Prediction",
            "Prophet Volume Predictions for Multiple Stocks"
        };
        public string? Company;
        public string? StartDate;
        public string? EndDate;
        public string? FileName;
        public string? Option;

        public RequestResponse? requestResponse = new();
        

        public async Task<IActionResult> OnGetAsync(string option, string? company, string? fileName, string? startDate, string? endDate)
        {            
            try
            {
                if (option != null && company != null && fileName != null && startDate != null && endDate != null)
                {
                    Company = company ?? string.Empty;
                    StartDate = startDate ?? string.Empty;
                    EndDate = endDate ?? string.Empty;
                    FileName = fileName ?? string.Empty;
                    Option = option ?? string.Empty;

                    requestResponse = await _httpClient?.GetFromJsonAsync<RequestResponse>($"http://localhost:5082/api/StockPricesApi/generate-volume-based-features-pdf?tickers={Company}&start_date={StartDate}&end_date={EndDate}&filename={FileName}&options={Option}");                  
                }
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching API data: {ex.Message}");
            }
            return Page();
        }

    }
}