using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;
public class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(30)]
    public string UserName { get; set; } = null!;
    public virtual ICollection<Preference> Preferences { get; set; } = new HashSet<Preference>();
    public virtual ICollection<UserContentItem> UserContentItems { get; set; } = new HashSet<UserContentItem>();
}