using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialMediaWebApp.Data.YoutubeAPI;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using SocialMediaWebApp.Data.Authentication;
using Microsoft.AspNetCore.Identity;
using SocialMediaWebApp.Data.Credentials;
using Microsoft.AspNetCore.Authorization;

namespace SocialMediaWebApp.Pages.YouTubeSearch
{
    [Authorize]
    public class VideoDetails : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly UserManager<IdentityUser>? _userManager;

        public VideoDetails(HttpClient httpClient, UserManager<IdentityUser> userManager)
        {
            _httpClient = httpClient;
            _userManager = userManager;
        }

        [BindProperty]
        public QueryResult Video {get; set;}

        [BindProperty]
        public UserModel? Input { get; set; }

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
        public async Task<IActionResult> OnGetAsync(string videoId){
            if (string.IsNullOrEmpty(videoId))
            {
                return NotFound("Video ID is required.");
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
                Video = await _httpClient.GetFromJsonAsync<QueryResult>($"http://localhost:5082/api/YouTubeApiSearch/GetVideoDetails?videoId={videoId}");
                
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, "Error fetching video details: " + ex.Message);
            }
            if (Video == null)
            {
                return NotFound("Video not found.");
            }
            return Page();
        }
    }
}