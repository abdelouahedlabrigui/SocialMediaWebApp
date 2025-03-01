using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialMediaWebApp.Data;

namespace SocialMediaWebApp.Pages.FredAPI
{
    public class FredAPIEndpoint : PageModel
    {
        private readonly HttpClient? _httpClient;

        public FredAPIEndpoint()
        {
            _httpClient = new HttpClient(); 
        }
        [BindProperty]
        public List<FredAPIEndpoint>? FredAPIEndpoints { get; set; }
        [BindProperty]
        public RequestResponse? requestResponse { get; set; }
        public string? Description { get; set; }
        public string? GetRequest { get; set; }

        public async Task<IActionResult> OnGetAsync(string description, string getRequest)
        {
            Description = description ?? string.Empty;
            GetRequest = getRequest ?? string.Empty;
            
            try
            {
                if (description != null && getRequest != null)
                {
                    requestResponse = 
                        await _httpClient.GetFromJsonAsync<RequestResponse>($"http://localhost:5082/api/FredApi/post-fred-api-endpoints?description={description}&getRequest={getRequest.Replace("&", "and")}");                    
                } else {
                    requestResponse = new RequestResponse{
                        Message = "Couldn't process API request."
                    };
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