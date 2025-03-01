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
using SocialMediaWebApp.Data.Authentication;
using SocialMediaWebApp.Data.Credentials;
using SocialMediaWebApp.Data.GoogleAPI;

namespace SocialMediaWebApp.Pages.GoogleSearch
{
    [Authorize]
    public class GoogleApiSearch : PageModel
    {
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly HttpClient? _httpClient;
        public string? Search {get;set;}

        public GoogleApiSearch(UserManager<IdentityUser> userManager)
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


        [BindProperty]
        public List<GoogleSearchResult>? SearchResults { get; set; }   
        public async Task<IActionResult> OnGetAsync(string search)
        {
            Search = search ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(search))
            {
                ModelState.AddModelError(string.Empty, $"Search is {search} Empty.");
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
                List<GoogleSearchResult>? searchResult = await _httpClient.GetFromJsonAsync<List<GoogleSearchResult>>($"http://localhost:5082/api/GoogleSearchApi/search?query={Search}");

                if (searchResult != null)
                {
                    SearchResults = searchResult;
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