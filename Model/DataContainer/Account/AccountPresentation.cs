namespace TheBank.Model
{
    public class AccountPresentation
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string accountNumber { get; set; }
        public string name { get; set; }
        public double balance { get; set; }
        public bool card { get; set; }
        public double interest { get; set; }
        public double limit { get; set; }
        public double yearlyFee { get; set; }
        public bool active { get; set; }
    }
}