using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Models
{
    public class NewPreference
    {
        [Required]
        public string Topic { get; set; } = null!;
    }
}