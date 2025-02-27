using Infrastructure.External.Enums;

namespace Infrastructure.External.Models
{
    public class NewsAPIResponse
    {
        public Status Status { get; set; }
        public List<Article> Articles { get; set; } = null!;
        public int TotalResults { get; set; }
        public string Message { get; set; } = null!;
    }
}