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
    public class TextSummarization : PageModel
    {
        public TextSummarization()
        {
            _httpClient = new HttpClient();
        }
        public List<string>? Options = new List<string> {"Get", "Get By Id", "Search", "Delete"};
        private readonly HttpClient? _httpClient;

        public string? OptionValue { get; set; }

        public List<LlmTextSummarization>? llmTextSummarization { get; set; }



        private async Task<List<LlmTextSummarization>> QueryOptions(string option_values, string optionValue){
            switch (option_values)
            {
                case "Get":
                    llmTextSummarization = await _httpClient.GetFromJsonAsync<List<LlmTextSummarization>>($"http://localhost:5082/api/Transformers/get-summarization");
                    return llmTextSummarization ?? new List<LlmTextSummarization>();
                case "Get By Id":
                    llmTextSummarization = await _httpClient.GetFromJsonAsync<List<LlmTextSummarization>>($"http://localhost:5082/api/Transformers/get-summarization-by-id?id={optionValue}");
                    return llmTextSummarization ?? new List<LlmTextSummarization>();
                case "Search":
                    llmTextSummarization = await _httpClient.GetFromJsonAsync<List<LlmTextSummarization>>($"http://localhost:5082/api/Transformers/get-summarization-by-search?search={optionValue}");
                    return llmTextSummarization ?? new List<LlmTextSummarization>();
                case "Delete":
                    await _httpClient.GetFromJsonAsync<RequestResponse>($"http://localhost:5082/api/Transformers/delete-summarization?id={optionValue}");
                    return new List<LlmTextSummarization>();
                default:
                    return new List<LlmTextSummarization>();
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
                llmTextSummarization = await QueryOptions(options, OptionValue);
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching API data: {ex.Message}");
            }

            return Page();
        }
    }
}