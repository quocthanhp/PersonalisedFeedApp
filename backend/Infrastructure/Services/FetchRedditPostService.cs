using System.Net.Http.Headers;
using Application.Interfaces;
using Domain.DTOs.Models.Reddit;
using DotNetEnv;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Domain.DTOs.Enums;

namespace Infrastructure.Services
{
    public class FetchRedditPostService : IFetchRedditPostService
    {
        private readonly HttpClient _httpClient;
        private readonly string clientId = null!;
        private readonly string clientSecret = null!;
        private readonly string username = null!;
        private readonly string password = null!;
        private readonly string userAgent = null!;
        private string accessToken = null!;

        public FetchRedditPostService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Env.Load("../../../.env");
            clientId = Environment.GetEnvironmentVariable("REDDIT_CLIENT_ID")!;
            clientSecret = Environment.GetEnvironmentVariable("REDDIT_CLIENT_SECRET")!;
            username = Environment.GetEnvironmentVariable("REDDIT_USERNAME")!;
            password = Environment.GetEnvironmentVariable("REDDIT_PASSWORD")!;
            userAgent = Environment.GetEnvironmentVariable("REDDIT_USER_AGENT")!;
        }

        public async Task AuthenticateAsync()
        {
            var authRequest = new HttpRequestMessage(HttpMethod.Post, "https://www.reddit.com/api/v1/access_token");
            authRequest.Headers.Add("User-Agent", userAgent);
            authRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}")));

            authRequest.Content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        });

            var response = await _httpClient.SendAsync(authRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);
            accessToken = jsonResponse["access_token"]?.ToString() ?? string.Empty;

            if (accessToken == string.Empty)
            {
                throw new Exception("Failed to authenticate with Reddit API");
            }
        }

        public async Task<RedditPostAPIResponse> FetchRedditPostAsync(string topic)
        {
            if (accessToken == null)
                await AuthenticateAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);

            // May change to personalised limit and time period
            var url = $"https://oauth.reddit.com/search?q={topic}&sort=relevance&t=day&limit=10";
            var response = await _httpClient.GetStringAsync(url);

            var redditResponse = JsonConvert.DeserializeObject<RedditPostAPIResponse>(response);

            if (redditResponse is null || redditResponse.Data.Children.Count == 0)
            {
                return new RedditPostAPIResponse
                {
                    Status = Status.Error,
                    Message = "Failed to fetch Reddit posts"
                };
            }

            redditResponse.Status = Status.Ok;
            redditResponse.Message = "Successfully fetched Reddit posts";

            return redditResponse;
        }
    }
}