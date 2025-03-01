using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialMediaWebApp.Data;
using SocialMediaWebApp.Data.NewsAPI;

namespace SocialMediaWebApp.Pages.Documents
{
    public class NewsArticlePrompts : PageModel
    {
        private readonly HttpClient _httpClient;
        public NewsArticlePrompts()
        {
            _httpClient = new HttpClient(); 
        }
        public string? Title { get; set; }
        public string? Keywords { get; set; }
        public string? PromptString { get; set; }

        [BindProperty]
        public List<NewsPrompt>? newsPrompts { get; set; }
        public RequestResponse? requestResponse { get; set; }
        public async Task<IActionResult> OnGetAsync(string title, string keywords, string promptString){
            try
            {
                Title = title;
                Keywords = keywords;
                PromptString = promptString;
                requestResponse = await _httpClient.GetFromJsonAsync<RequestResponse>($"http://localhost:5082/api/NewsPromptsApi/generate-news-prompt?title={title}&keywords={keywords}&promptString={promptString}");                
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching API data: {ex.Message}");
            }
            return Page();
        }
    }
}