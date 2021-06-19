namespace Gridcoin.WebApi.Models
{
    public class Payment
    {
        public string Address { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
    }
}
