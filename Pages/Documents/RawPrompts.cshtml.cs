using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialMediaWebApp.Data;
using SocialMediaWebApp.Data.Authentication;
using SocialMediaWebApp.Data.Credentials;
using SocialMediaWebApp.Data.GoogleAPI.Gemini;

namespace SocialMediaWebApp.Pages.Documents
{
    [Authorize]
    public class RawPrompts : PageModel
    {
        public string? PromptString { get; set; }
        public string? TextPath { get; set; }
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly HttpClient? _httpClient;
        public RawPrompts(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _httpClient = new HttpClient();
        }
        private void SetAuthHeader(string _token){
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _token);
        }

        private async Task<Tokens> AccessTokenRequest(string userId, string username, string email, string passwordHash){
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(passwordHash))
            {
                return new Tokens();
            } else {
                Tokens token = await _httpClient.GetFromJsonAsync<Tokens>($"http://localhost:5082/api/YouTubeApiSearch/authenticate?userId={userId}&userName={username}&email={email}&passwordHash={passwordHash}");
                if (token != null)
                {
                    return token;
                } else {
                    return new Tokens();
                }
            }
        }
        [BindProperty]
        public UserModel? Input { get; set; }

        List<PromptRawResult> searchResults = new();

        
        [BindProperty]
        public List<PromptRawResult>? promptRawResults { get; set; }  
        public async Task<IActionResult> OnGetAsync(string promptString, string? textPath)
        {
            PromptString = promptString ?? string.Empty;
            TextPath = textPath ?? string.Empty;

            if (string.IsNullOrWhiteSpace(PromptString) || string.IsNullOrWhiteSpace(TextPath))
            {
                ModelState.AddModelError(string.Empty, "Prompt and TextPath are required.");
            }
            
            string userId = "19831cdf-3bf7-4d8e-ae5f-f69cbd039e87";
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            Input = new UserModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                HashedPassword = user.PasswordHash,                
            };
            try
            {
                Tokens token = await AccessTokenRequest(Input.UserId, Input.UserName, Input.Email, Input.HashedPassword);
                SetAuthHeader(token.Token);
                await _httpClient.GetFromJsonAsync<RequestResponse>($"http://localhost:5082/api/GeminiApi/generate-prompt?promptString={PromptString}&txtPath={TextPath}");                
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching API data: {ex.Message}");
            }
            return Page();
        }
    }
}