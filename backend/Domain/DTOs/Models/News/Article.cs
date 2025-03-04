namespace Domain.DTOs.Models.News
{
    public class Article
    {
        public Source Source { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string UrlToImage { get; set; } = null!;
        public DateTime? PublishedAt { get; set; }
        public string Content { get; set; } = null!;
    }
}