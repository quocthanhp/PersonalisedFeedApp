using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public abstract class ContentItem
{
    [Key]
    public string ContentItemId { get; set; } = null!;

    [StringLength(255)]
    public string? Title { set; get; }

    [Required]
    [StringLength(30)]
    public string Source { get; set; } = null!;

    [StringLength(30)]
    public string? Author { get; set; }

    [Required]
    [StringLength(2000)]
    public string Content { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Topic { get; set; } = null!;

    [Required]
    public DateTime PublishedAt { get; set; }

    //public virtual ICollection<UserContentItem> UserContentItems { get; set; } = new HashSet<UserContentItem>();
}