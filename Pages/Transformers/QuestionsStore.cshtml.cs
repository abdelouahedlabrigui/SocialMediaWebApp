using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialMediaWebApp.Data;

namespace SocialMediaWebApp.Pages.Transformers
{
    public class QuestionsStore : PageModel
    {

        public QuestionsStore()
        {
            _httpClient = new HttpClient();
        }
        
        public string? TextPath { get; set; }
        public string? Title { get; set; }
        public string? SearchString { get; set; }
        public string? Question { get; set; }


        public RequestResponse? requestResponse = new();
        private readonly HttpClient? _httpClient;

        public async Task<IActionResult> OnGetAsync(string? textPath, string? title, string? searchString, string? question){
            

            if (string.IsNullOrWhiteSpace(textPath) || string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(searchString) || string.IsNullOrWhiteSpace(question))
            {
                ModelState.AddModelError(string.Empty, "Fields are required.");
            }
            TextPath = textPath ?? string.Empty;
            Title = title ?? string.Empty;
            SearchString = searchString ?? string.Empty;
            Question = question ?? string.Empty;

            try
            {
                requestResponse = await _httpClient.GetFromJsonAsync<RequestResponse>($"http://localhost:5082/api/NewsQuestions/post-question?textPath={TextPath}&title={Title}&searchString={SearchString}&question={Question}");   
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching API data: {ex.Message}");
            }

            return Page();
        }

    }
}