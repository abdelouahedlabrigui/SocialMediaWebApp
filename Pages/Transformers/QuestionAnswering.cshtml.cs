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
    public class QuestionAnswering : PageModel
    {
        public QuestionAnswering()
        {
            _httpClient = new HttpClient();
        }
        public List<string>? Options = new List<string> {"Get", "Get By Id", "Search", "Delete"};
        private readonly HttpClient? _httpClient;

        public string? OptionValue { get; set; }

        public List<LlmQuestionAnswering>? llmQuestionAnswering { get; set; }



        private async Task<List<LlmQuestionAnswering>> QueryOptions(string option_values, string optionValue){
            switch (option_values)
            {
                case "Get":
                    llmQuestionAnswering = await _httpClient.GetFromJsonAsync<List<LlmQuestionAnswering>>($"http://localhost:5082/api/Transformers/get-question-answering");
                    return llmQuestionAnswering ?? new List<LlmQuestionAnswering>();
                case "Get By Id":
                    llmQuestionAnswering = await _httpClient.GetFromJsonAsync<List<LlmQuestionAnswering>>($"http://localhost:5082/api/Transformers/get-question-answering-by-id?id={optionValue}");
                    return llmQuestionAnswering ?? new List<LlmQuestionAnswering>();
                case "Search":
                    llmQuestionAnswering = await _httpClient.GetFromJsonAsync<List<LlmQuestionAnswering>>($"http://localhost:5082/api/Transformers/get-question-answering-by-search?search={optionValue}");
                    return llmQuestionAnswering ?? new List<LlmQuestionAnswering>();
                case "Delete":
                    await _httpClient.GetFromJsonAsync<RequestResponse>($"http://localhost:5082/api/Transformers/delete-question-answering?id={optionValue}");
                    return new List<LlmQuestionAnswering>();
                default:
                    return new List<LlmQuestionAnswering>();
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
                llmQuestionAnswering = await QueryOptions(options, OptionValue);
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching API data: {ex.Message}");
            }

            return Page();
        }
    }
}