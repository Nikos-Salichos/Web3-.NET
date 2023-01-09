using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class User
    {
        [Required]
        public string? WalletAddress { get; set; }

        [Required]
        public string? PrivateKey { get; set; }

    }
}
