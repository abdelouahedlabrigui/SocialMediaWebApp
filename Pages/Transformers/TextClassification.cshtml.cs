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
    public class TextClassification : PageModel
    {

        public TextClassification()
        {
            _httpClient = new HttpClient();
        }
        public List<string>? Options = new List<string> {"Get", "Get By Id", "Search", "Delete"};
        private readonly HttpClient? _httpClient;

        public string? OptionValue { get; set; }

        public List<LlmTextClassification>? llmTextClassifications { get; set; }



        private async Task<List<LlmTextClassification>> QueryOptions(string option_values, string optionValue){
            switch (option_values)
            {
                case "Get":
                    llmTextClassifications = await _httpClient.GetFromJsonAsync<List<LlmTextClassification>>($"http://localhost:5082/api/Transformers/get-text-classification");
                    return llmTextClassifications ?? new List<LlmTextClassification>();
                case "Get By Id":
                    llmTextClassifications = await _httpClient.GetFromJsonAsync<List<LlmTextClassification>>($"http://localhost:5082/api/Transformers/get-text-classification-by-id?id={optionValue}");
                    return llmTextClassifications ?? new List<LlmTextClassification>();
                case "Search":
                    llmTextClassifications = await _httpClient.GetFromJsonAsync<List<LlmTextClassification>>($"http://localhost:5082/api/Transformers/get-text-classification-by-search?search={optionValue}");
                    return llmTextClassifications ?? new List<LlmTextClassification>();
                case "Delete":
                    await _httpClient.GetFromJsonAsync<RequestResponse>($"http://localhost:5082/api/Transformers/delete-text-classification?id={optionValue}");
                    return new List<LlmTextClassification>();
                default:
                    return new List<LlmTextClassification>();
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
                llmTextClassifications = await QueryOptions(options, OptionValue);
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching API data: {ex.Message}");
            }

            return Page();
        }
    }
}