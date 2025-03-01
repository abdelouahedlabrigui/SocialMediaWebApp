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
    public class TextTranslation : PageModel
    {
        public TextTranslation()
        {
            _httpClient = new HttpClient();
        }
        public List<string>? Options = new List<string> {"Get", "Get By Id", "Search", "Delete"};
        private readonly HttpClient? _httpClient;

        public string? OptionValue { get; set; }

        public List<LlmTextTranslation>? llmTextTranslation { get; set; }



        private async Task<List<LlmTextTranslation>> QueryOptions(string option_values, string optionValue){
            switch (option_values)
            {
                case "Get":
                    llmTextTranslation = await _httpClient.GetFromJsonAsync<List<LlmTextTranslation>>($"http://localhost:5082/api/Transformers/get-translation");
                    return llmTextTranslation ?? new List<LlmTextTranslation>();
                case "Get By Id":
                    llmTextTranslation = await _httpClient.GetFromJsonAsync<List<LlmTextTranslation>>($"http://localhost:5082/api/Transformers/get-translation-by-id?id={optionValue}");
                    return llmTextTranslation ?? new List<LlmTextTranslation>();
                case "Search":
                    llmTextTranslation = await _httpClient.GetFromJsonAsync<List<LlmTextTranslation>>($"http://localhost:5082/api/Transformers/get-translation-by-search?search={optionValue}");
                    return llmTextTranslation ?? new List<LlmTextTranslation>();
                case "Delete":
                    await _httpClient.GetFromJsonAsync<RequestResponse>($"http://localhost:5082/api/Transformers/delete-translation?id={optionValue}");
                    return new List<LlmTextTranslation>();
                default:
                    return new List<LlmTextTranslation>();
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
                llmTextTranslation = await QueryOptions(options, OptionValue);
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching API data: {ex.Message}");
            }

            return Page();
        }
    }
}