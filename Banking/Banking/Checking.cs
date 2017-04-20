namespace Banking
{
    public class Checking : Account
    {
        public Checking(Customer customer, int accountID, decimal balance)
            :base(customer, ACCOUNTTYPE.CHECKING, accountID, balance)
        {

        }
        public override void CalculateInterest() 
        {

        }
    }
}
