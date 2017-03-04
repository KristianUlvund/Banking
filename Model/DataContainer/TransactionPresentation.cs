namespace TheBank.Model
{
    public class TransactionPresentation
    {
        public int ID { get; set; }
        public string date { get; set; }
        public double amount { get; set; }
        public string message { get; set; }
        public string toAccount { get; set; }
        public string fromAccount { get; set; }
    }
}