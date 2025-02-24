namespace Domain.Entities
{
    public class UserContentItem
    {
        public int UserId;
        public int ContentItemId;
        public User User { get; set; } = null!;
        public ContentItem ContentItem { get; set; } = null!;
    }
}