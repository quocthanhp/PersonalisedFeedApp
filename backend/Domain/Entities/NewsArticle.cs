using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class NewsArticle : ContentItem
{
    [StringLength(100)]
    public string? Title { set; get; }
}