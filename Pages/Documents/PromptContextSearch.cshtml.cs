using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialMediaWebApp.Data;
using SocialMediaWebApp.Data.LanguageProcessing.NlpQueries;

namespace SocialMediaWebApp.Pages.Documents
{
    public class PromptContextSearch : PageModel
    {
        private readonly HttpClient? _httpClient;

        public PromptContextSearch()
        {
            _httpClient = new HttpClient();
        }
        public string? PromptString { get; set; }
        public string? FilePath { get; set; }
        public string? State { get; set; }
        public List<SentimentSearch>? sentimentSearches { get; set; }
        public RequestResponse? requestResponse { get; set; }
        public async Task<IActionResult> OnGetAsync(string prompt, string file, string state){
            PromptString = prompt ?? string.Empty;
            FilePath = file ?? string.Empty;
            State = state ?? string.Empty;

            if (string.IsNullOrWhiteSpace(prompt) || string.IsNullOrWhiteSpace(file) || string.IsNullOrWhiteSpace(state))
            {
                ModelState.AddModelError(string.Empty, "Fields are required.");
            }

            try
            {
                requestResponse = await _httpClient.GetFromJsonAsync<RequestResponse>($"http://localhost:5082/api/TextAnalysisApi/generate-prompt-context?prompt={PromptString}&file={FilePath}&state={State}");  
                if (requestResponse?.Message == "Search added successfully.")
                {
                    sentimentSearches = await _httpClient.GetFromJsonAsync<List<SentimentSearch>>("http://localhost:5082/api/TextAnalysisApi/get-top-sentiment-search");
                    requestResponse.Message = $"{requestResponse.Message} Count Rows: {sentimentSearches?.Count}";
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