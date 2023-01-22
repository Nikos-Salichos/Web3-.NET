using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class WalletOwner
    {
        [Required]
        public string WalletAddress { get; set; } = string.Empty;

        [Required]
        public string PrivateKey { get; set; } = string.Empty;

        public IEnumerable<object> GetEqualityComponents()
        {
            yield return WalletAddress;
            yield return PrivateKey;
        }
    }
}
