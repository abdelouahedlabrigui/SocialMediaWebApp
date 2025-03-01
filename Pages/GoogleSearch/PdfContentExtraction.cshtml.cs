using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialMediaWebApp.Data.Documents;

namespace SocialMediaWebApp.Pages.GoogleSearch
{
    public class PdfContentExtraction : PageModel
    {
        private readonly HttpClient _httpClient;

        public PdfContentExtraction()
        {
            _httpClient = new HttpClient();
        }
        public string? Search { get; set; }
        [BindProperty]
        public List<SearchedDocument>? searchedDocuments { get; set; }
        public async Task<IActionResult> OnGetAsync(string search){
            Search = search ?? string.Empty;
            try
            {
                List<SearchedDocument>? saveDocuments = await _httpClient.GetFromJsonAsync<List<SearchedDocument>>($"http://localhost:5082/api/PdfApi/search-pdfs?search={search}");

                if (saveDocuments != null)
                {
                    searchedDocuments = saveDocuments;
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