using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;
public class User : IdentityUser
{
    public virtual ICollection<Preference> Preferences { get; set; } = new HashSet<Preference>();
    //public virtual ICollection<UserContentItem> UserContentItems { get; set; } = new HashSet<UserContentItem>();
}