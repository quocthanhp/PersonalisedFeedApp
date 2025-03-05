using Domain.DTOs.Enums;

namespace Domain.DTOs.Models.Reddit
{
    public class RedditPostAPIResponse
    {
        public Data Data { get; set; } = null!;
        public Status Status { get; set; }
        public string Message { get; set; } = null!;
    }
}