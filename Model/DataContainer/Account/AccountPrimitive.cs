namespace TheBank.Model.DataContainer.Account
{
    public class AccountPrimitive
    {
        public AccountPrimitive(string number, string name, double balance, bool inActive)
        {
            accountNumber = number;
            accountName = name;
            accountBalance = balance;
            active = inActive;
        }
        public string accountNumber { get; set; }
        public string accountName { get; set; }
        public double accountBalance { get; set; }
        public bool active { get; set; }
    }
}