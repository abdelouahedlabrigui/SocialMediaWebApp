using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SocialMediaWebApp.Pages.Documents
{
    [Authorize]
    public class FormatRawPrompts : PageModel
    {
        [BindProperty]
        public string? Text { get; set; }
        [BindProperty]    
        public string? Type { get; set; }
        [BindProperty]
        public string? JsonFile { get; set; }
        public string? Message { get; set; }
        
        public async Task<IActionResult> OnGetAsync(string text, string type, string jsonFile)
        {
            Text = text ?? string.Empty;
            Type = type ?? string.Empty;
            JsonFile = jsonFile ?? string.Empty;
            
            // Ensure Type and Text are not null or empty
            if (string.IsNullOrWhiteSpace(Type) || string.IsNullOrWhiteSpace(Text) || string.IsNullOrWhiteSpace(JsonFile))
            {
                Message = "Type or Text or Json file cannot be empty.";
                return Page();
            }
            // Normalize Type and Text inputs
            Type = Type?.ToLower();
            Text = Text?.Trim().Replace("\\n", "\n").Replace('"', '\"');

            var newObject = new { Type, Text };

            // Read the JSON file content
            string jsonContent;
            try
            {
                using (var reader = new StreamReader(jsonFile))
                {
                    jsonContent = await reader.ReadToEndAsync();
                }
            }
            catch
            {
                Message = "Failed to read the uploaded file.";
                return Page();
            }

            // Deserialize JSON and handle potential null content
            List<object> jsonObjects;
            try
            {
                jsonObjects = JsonConvert.DeserializeObject<List<object>>(jsonContent) ?? new List<object>();
            }
            catch
            {
                Message = "Invalid JSON input.";
                return Page();
            }

            // Append the new object and save the file
            jsonObjects.Add(newObject);
            string updatedJson = JsonConvert.SerializeObject(jsonObjects, Formatting.Indented);

            try
            {
                using (var writer = new StreamWriter(jsonFile))
                {
                    await writer.WriteAsync(updatedJson);
                }
            }
            catch
            {
                Message = "Failed to save the updated JSON file.";
                return Page();
            }

            Message = "JSON file updated successfully.";
            return Page();
        }

    }
}