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
    public class TextGeneration : PageModel
    {
        
        public TextGeneration()
        {
            _httpClient = new HttpClient();
        }
        public List<string>? Options = new List<string> {"Get", "Get By Id", "Search", "Delete"};
        private readonly HttpClient? _httpClient;

        public string? OptionValue { get; set; }

        public List<LlmTextGeneration>? llmTextGeneration { get; set; }



        private async Task<List<LlmTextGeneration>> QueryOptions(string option_values, string optionValue){
            switch (option_values)
            {
                case "Get":
                    llmTextGeneration = await _httpClient.GetFromJsonAsync<List<LlmTextGeneration>>($"http://localhost:5082/api/Transformers/get-text-generation");
                    return llmTextGeneration ?? new List<LlmTextGeneration>();
                case "Get By Id":
                    llmTextGeneration = await _httpClient.GetFromJsonAsync<List<LlmTextGeneration>>($"http://localhost:5082/api/Transformers/get-text-generation-by-id?id={optionValue}");
                    return llmTextGeneration ?? new List<LlmTextGeneration>();
                case "Search":
                    llmTextGeneration = await _httpClient.GetFromJsonAsync<List<LlmTextGeneration>>($"http://localhost:5082/api/Transformers/get-text-generation-by-search?search={optionValue}");
                    return llmTextGeneration ?? new List<LlmTextGeneration>();
                case "Delete":
                    await _httpClient.GetFromJsonAsync<RequestResponse>($"http://localhost:5082/api/Transformers/delete-text-generation?id={optionValue}");
                    return new List<LlmTextGeneration>();
                default:
                    return new List<LlmTextGeneration>();
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
                llmTextGeneration = await QueryOptions(options, OptionValue);
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching API data: {ex.Message}");
            }

            return Page();
        }
    }
}