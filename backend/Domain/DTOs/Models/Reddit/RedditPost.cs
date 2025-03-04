using System.Text.Json.Serialization;

namespace Domain.DTOs.Models.Reddit
{
    public class RedditPost
    {
        public string? Title { get; set; }
        public string? Selftext { get; set; }
        public string? Thumbnail { get; set; }

        [JsonPropertyName("created_utc")]
        public long CreatedUtc { get; set; }

        public int Ups { get; set; }
    }
}