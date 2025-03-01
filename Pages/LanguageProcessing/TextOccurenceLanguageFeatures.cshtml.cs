using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialMediaWebApp.Data.LanguageProcessing;
using SocialMediaWebApp.Data.LanguageProcessing.NlpQueries;

namespace SocialMediaWebApp.Pages.LanguageProcessing
{
    public class TextOccurenceLanguageFeatures : PageModel
    {
        private readonly HttpClient? _httpClient;

        public TextOccurenceLanguageFeatures()
        {
            _httpClient = new HttpClient();
        }

        public List<NounChunk>? Chunks { get; set; }
        public List<Entity>? Entities { get; set; }
        public List<Sentiment>? Sentiments { get; set; }
        [BindProperty]
        public List<NounChunksCount>? nounChunksCounts { get; set; }

        [BindProperty]
        public LanguageFeature? languageFeatures { get; set; }

        public string? Title { get; set; }
        public string? SearchString { get; set; }

        private List<NounChunksCount>? GetNounChunks()
        {
            return languageFeatures.NounChunksList
                .GroupBy(o => new 
                        { o.Title, o.SearchString, o.RootHead })        
                .Select(s => new NounChunksCount
                {
                    Title = s.Key.Title,
                    SearchString = s.Key.SearchString,
                    RootHead = s.Key.RootHead,
                    RootText = string.Join(", ", s.Select(x => x.RootText).Distinct()), // Concatenate RootText values
                    Count = s.Count()
                })
                .OrderByDescending(o => o.Count)
                .ToList();
        }


        public async Task<IActionResult> OnGetAsync(string title, string searchString){
            

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(searchString))
            {
                ModelState.AddModelError(string.Empty, "Fields are required.");
            }
            
            Title = title ?? string.Empty;
            SearchString = searchString ?? string.Empty;

            try
            {
                languageFeatures = await _httpClient.GetFromJsonAsync<LanguageFeature>($"http://localhost:5082/api/TextAnalysisApi/get-language-features?title={Title}&searchString={SearchString}");   
                nounChunksCounts = GetNounChunks();
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching API data: {ex.Message}");
            }

            return Page();
        }
    }
}