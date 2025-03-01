using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialMediaWebApp.Data;
using SocialMediaWebApp.Data.TransformersModels;

namespace SocialMediaWebApp.Pages.Transformers
{
    public class NamedEntityRecognition : PageModel
    {
        public NamedEntityRecognition()
        {
            _httpClient = new HttpClient();
        }
        public List<string>? Options = new List<string> {"Get", "Get By Id", "Search", "Delete"};
        private readonly HttpClient? _httpClient;

        public string? OptionValue { get; set; }

        public List<LlmNamedEntityRecognition>? llmNamedEntityRecognition { get; set; }



        private async Task<List<LlmNamedEntityRecognition>> QueryOptions(string option_values, string optionValue){
            switch (option_values)
            {
                case "Get":
                    llmNamedEntityRecognition = await _httpClient.GetFromJsonAsync<List<LlmNamedEntityRecognition>>($"http://localhost:5082/api/Transformers/get-ner-tagger");
                    return llmNamedEntityRecognition ?? new List<LlmNamedEntityRecognition>();
                case "Get By Id":
                    llmNamedEntityRecognition = await _httpClient.GetFromJsonAsync<List<LlmNamedEntityRecognition>>($"http://localhost:5082/api/Transformers/get-ner-tagger-by-id?id={optionValue}");
                    return llmNamedEntityRecognition ?? new List<LlmNamedEntityRecognition>();
                case "Search":
                    llmNamedEntityRecognition = await _httpClient.GetFromJsonAsync<List<LlmNamedEntityRecognition>>($"http://localhost:5082/api/Transformers/get-ner-tagger-by-search?id={optionValue}");
                    return llmNamedEntityRecognition ?? new List<LlmNamedEntityRecognition>();
                case "Delete":
                    await _httpClient.GetFromJsonAsync<RequestResponse>($"http://localhost:5082/api/Transformers/delete-ner-tagger?id={optionValue}");
                    return new List<LlmNamedEntityRecognition>();
                default:
                    return new List<LlmNamedEntityRecognition>();
            }
        }

        public async Task<IActionResult> OnGetAsync(string options, string optionValue){
            
            if (string.IsNullOrEmpty(options) || string.IsNullOrEmpty(optionValue))
            {
                ModelState.AddModelError(string.Empty, "Fields are required.");
            }
            OptionValue = optionValue ?? string.Empty;
            try
            {
                llmNamedEntityRecognition = await QueryOptions(options, OptionValue);
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching API data: {ex.Message}");
            }

            return Page();
        }
    }
}