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
    public class NounChunks : PageModel
    {
        private readonly HttpClient? _httpClient;

        public NounChunks()
        {
            _httpClient = new HttpClient();
        }
        public List<NounChunk>? chunksData { get; set; }
        public List<NounChunksCount>? nounChunksCounts { get; set; }

        private List<NounChunksCount>? GetNounChunks()
        {
            return chunksData
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
                chunksData = await _httpClient.GetFromJsonAsync<List<NounChunk>?>($"http://localhost:5082/api/TextAnalysisApi/get-noun-chunks-from-db?title={Title}&searchString={SearchString}");   
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