using Application.Interfaces;
using DotNetEnv;
using Domain.DTOs.Models.News;
using Domain.DTOs.Enums;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class FetchNewsService : IFetchNewsService
    {
        private readonly HttpClient _httpClient;
        public FetchNewsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("PersonalisedFeedApp/1.0");
            Env.Load("../../../.env"); 
        }

        public async Task<NewsAPIResponse> FetchNewsAsync(string topic)
        {
            string? apiKey = Environment.GetEnvironmentVariable("API_KEY");

            if (string.IsNullOrEmpty(apiKey))
            {
                WriteLine("API key is missing!");
                return new NewsAPIResponse
                {
                    Status = Status.Error,
                    Message = "API key is missing!"
                };
            }

            WriteLine("API key: " + apiKey); // For debugging

            string url = $"https://newsapi.org/v2/everything?q={topic}&"
             + "from=2025-02-26&"
             + "sortBy=popularity&"
             + "language=en&"
             + $"apiKey={apiKey}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    var apiResponse = JsonConvert.DeserializeObject<NewsAPIResponse>(responseContent);
                    if (apiResponse is null)
                    {
                        WriteLine("API response is null");
                        return new NewsAPIResponse
                        {
                            Status = Status.Error,
                            Message = "API response is null"
                        };
                    }

                    //Return top 10 papers only
                    apiResponse.Articles = apiResponse.Articles.Take(10).ToList();
                    apiResponse.TotalResults = 10;
                    
                    WriteLine("Total Results: " + apiResponse.TotalResults);

                    return apiResponse;
                }

            }
            catch (HttpRequestException e)
            {
                WriteLine($"Request error: {e.Message}");
            }

            return new NewsAPIResponse
            {
                Status = Status.Error,
                Message = "Failed to fetch news"
            };
        }
    }
}