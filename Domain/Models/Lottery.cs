namespace Domain.Models
{
    public class Lottery : SmartContract
    {
        public string? Owner { get; set; }
        public List<string>? Players { get; set; }
        public int? LotteryId { get; set; }
        public string? LotteryHistory { get; set; }

    }
}
