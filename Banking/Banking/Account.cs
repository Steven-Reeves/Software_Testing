namespace Banking
{
    public abstract class Account
    {
        public int AccountID { get; set; }
        public ACCOUNTTYPE AccountType { get; set; }
        public Customer Customer { get; set; }
        public decimal Balance { get; set; }
  
        public enum ACCOUNTTYPE
        {
            CHECKING, SAVINGS
        }

        public Account(Customer customer, ACCOUNTTYPE type, int accountID, decimal balance)
        {
            this.AccountID = accountID;
            this.Customer = customer;
            this.Balance = balance;
            this.AccountType = type;
        }

        public virtual void CalculateInterest()
        {

        }

        public decimal deposit(decimal depositAmount) 
        {
            Balance += depositAmount;

            return Balance;
        }

        public decimal withdrawl(decimal withdrawalAmount)
        {
            Balance -= withdrawalAmount;

            return Balance;
        }

        public override string ToString()
        {
            string output = Customer.FirstName + " " + Customer.LastName + " [Account: " + AccountID + "]";
            output += " Balance: " + Balance;

            return output;
            
        }

    }
}
