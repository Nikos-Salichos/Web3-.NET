using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class WalletOwner
    {
        [Required]
        public string? WalletAddress { get; set; }

        [Required]
        public string? PrivateKey { get; set; }
    }
}
