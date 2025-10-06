namespace AlertBakchich.Models
{
    public class Payment
    {
        public int paymentID { get; set; }
        public string? message { get; set; }
        public Donor? donor { get; set; }
        public double amount { get; set; }
        public Asset asset { get; set; } = new Asset();
        public string? webhookURL { get; set; }
    }
}

