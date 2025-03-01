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
using SocialMediaWebApp.Data.YoutubeAPI;

namespace SocialMediaWebApp.Pages.YouTubeSearch
{
    [Authorize]
    public class YouTubeAPISearchByDateIntervals : PageModel
    {
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly HttpClient? _httpClient;

        public string? Query {get;set;}
        public int? MaxResults { get; set; }
        public string? StartDate {get;set;}
        public string? EndDate { get; set; }

        public YouTubeAPISearchByDateIntervals(UserManager<IdentityUser> userManager)
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
        public List<QueryResult>? SearchResults { get; set; }   

        public async Task<IActionResult> OnGetAsync(string query, string? startDate, string? endDate, int maxResults)
        {
            Query = query ?? string.Empty;
            StartDate = startDate;
            EndDate = endDate;
            MaxResults = maxResults;

            if (string.IsNullOrWhiteSpace(Query))
            {
                ModelState.AddModelError(string.Empty, $"Query is {Query}.");
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
                List<QueryResult>? searchResult = await _httpClient.GetFromJsonAsync<List<QueryResult>>($"http://localhost:5082/api/YouTubeApiSearch/SearchLongVideosByDateIntervals?query={Query}&maxResults={maxResults}&publishedAfter={StartDate}&publishedBefore={EndDate}");

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