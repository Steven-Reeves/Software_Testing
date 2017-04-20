namespace Banking
{
    public class Savings : Account
    {
        public Savings(Customer customer, int accountID, decimal balance)
            :base(customer, ACCOUNTTYPE.SAVINGS, accountID, balance)
        {

        }
        public override void CalculateInterest() 
        {

        }
    }
}
