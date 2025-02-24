using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Preference
{
    [Key]
    public int PreferenceId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [StringLength(50)]
    public string Topic { get; set; } = null!;

    public virtual User? User { set; get; }
}