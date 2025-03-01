using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialMediaWebApp.Data;
using SocialMediaWebApp.Data.Trading;

namespace SocialMediaWebApp.Pages.Trading
{
    [Authorize]
    public class ArimaStockPrice : PageModel
    {
        private readonly HttpClient? _httpClient;

        public ArimaStockPrice()
        {
            _httpClient = new HttpClient(); 
        }
        [BindProperty]
        public RequestResponse? requestResponse { get; set; }
        [BindProperty]
        public List<StockPrice>? stockPrices { get; set; }
        public string? Company { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Filename { get; set; }
        public async Task<IActionResult> OnGetAsync(string? company, string? start_date, string? end_date, string? filename)
        {            
            try
            {
                if (company != null || start_date != null || end_date != null || filename != null)
                {
                    Company = company ?? string.Empty;
                    StartDate = start_date ?? string.Empty;
                    EndDate = end_date ?? string.Empty;
                    Filename = filename ?? string.Empty;

                    requestResponse = await _httpClient.GetFromJsonAsync<RequestResponse>($"http://localhost:5082/api/StockPricesApi/download-stock-price?company{Company}&start_date={StartDate}&end_date={EndDate}&filename={Filename}");                    
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