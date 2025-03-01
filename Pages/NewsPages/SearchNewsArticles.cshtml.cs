using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialMediaWebApp.Data.NewsAPI;

namespace SocialMediaWebApp.Pages.NewsPages
{
    public class SearchNewsArticles : PageModel
    {
        
        private readonly HttpClient? _httpClient;

        public SearchNewsArticles()
        {
            _httpClient = new HttpClient();
        }

        public List<LatestNews>? newsArticles { get; set; }
        public string? SearchQuery { get; set; }

        public string? ByDate { get; set; }

        public async Task<IActionResult> OnGetAsync(string search, string byDate){
            

            if (string.IsNullOrWhiteSpace(search) || string.IsNullOrWhiteSpace(byDate))
            {
                ModelState.AddModelError(string.Empty, "Fields are required.");
            }
            
            SearchQuery = search ?? string.Empty;
            ByDate = byDate ?? string.Empty;

            try
            {
                newsArticles = await _httpClient.GetFromJsonAsync<List<LatestNews>?>($"http://localhost:5082/api/NewsApi/search-news-articles?search={SearchQuery}&byDate={ByDate}");   
                
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching API data: {ex.Message}");
            }

            return Page();
        }

    }
}