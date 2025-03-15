using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Models.Account
{
    public class Register
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}