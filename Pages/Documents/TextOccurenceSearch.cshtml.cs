using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialMediaWebApp.Data;
using SocialMediaWebApp.Data.Documents;

namespace SocialMediaWebApp.Pages.Documents
{
    public class TextOccurenceSearch : PageModel
    {
        private readonly HttpClient _httpClient;

        public TextOccurenceSearch()
        {
            _httpClient = new HttpClient(); 
        }
        [BindProperty]
        public List<OccurenceSearch>? occurenceSearches {get; set;}

        public string? QuestionTxtFilePath { get; set; } = $@"C:\Users\dell\Entrepreneurship\Instructor\TechNews\Questions\Question_{DateTime.Now.ToString().Replace(':', '_').Replace("/", "-")}.txt";

        public string? TextFilePath { get; set; }
        public string? SearchString { get; set; }
        public List<string> sectorsList = new List<string>{ "medical", "spatial", "technology", "coding"};
        public int ContextLength { get; set; }
        public string? Sector { get; set; }

        public async Task<IActionResult> OnGetAsync(string textFilePath, string searchString, int contextLength, string sectors){
            TextFilePath = textFilePath ?? string.Empty;
            SearchString = searchString ?? string.Empty;
            ContextLength = contextLength;
            Sector = sectors ?? string.Empty;

            if (string.IsNullOrWhiteSpace(textFilePath) || string.IsNullOrWhiteSpace(searchString) || string.IsNullOrWhiteSpace(contextLength.ToString()) || string.IsNullOrWhiteSpace(sectors?.ToString()))
            {
                ModelState.AddModelError(string.Empty, "Fields are required.");
            }

            try
            {
                occurenceSearches = await _httpClient.GetFromJsonAsync<List<OccurenceSearch>>($"http://localhost:5082/api/TextAnalysisApi/analyze-text?textFilePath={TextFilePath}&searchString={SearchString}&contextLength={ContextLength}&sector={Sector}");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching API data: {ex.Message}");
            }

            return Page();
        }
    }
}