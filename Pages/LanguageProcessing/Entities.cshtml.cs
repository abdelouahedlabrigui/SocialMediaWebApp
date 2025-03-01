using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialMediaWebApp.Data.LanguageProcessing;

namespace SocialMediaWebApp.Pages.LanguageProcessing
{
    public class Entities : PageModel
    {
        private readonly HttpClient? _httpClient;

        public Entities()
        {
            _httpClient = new HttpClient();
        }
        public List<Entity>? entitiesData { get; set; }
        public string? Title { get; set; }
        public string? SearchString { get; set; }

        public async Task<IActionResult> OnGetAsync(string title, string searchString){
            

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(searchString))
            {
                ModelState.AddModelError(string.Empty, "Fields are required.");
            }
            
            Title = title ?? string.Empty;
            SearchString = searchString ?? string.Empty;

            try
            {
                entitiesData = await _httpClient.GetFromJsonAsync<List<Entity>?>($"http://localhost:5082/api/TextAnalysisApi/get-entities-from-db?title={Title}&searchString={SearchString}");   
                
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching API data: {ex.Message}");
            }

            return Page();
        }
    }
}