using System;
using System.Collections;
using System.IO;

namespace Banking
{
    public class FileDatabase
    {
        private string filename;
        private StreamWriter outFile;
        private StreamReader inFile;
        public const string DELIMETER = ",";

        public FileDatabase()
        {
            this.filename = @"..\..\..\Banking\Data\Database.txt";
        }

        public bool AddNewCustomer(int customerID, string firstName, string lastName, decimal openingDeposit, Account.ACCOUNTTYPE type)
        {
            int accountID = GetAccounts().Count + 1;
            using (outFile = File.AppendText(filename))
            {
                string output = accountID + DELIMETER + customerID + DELIMETER + firstName + DELIMETER + lastName + DELIMETER + openingDeposit + DELIMETER + Convert.ToInt32(type);
                outFile.WriteLine(output);

                outFile.Close();
            }
            return true;
        }
        public bool UpdateExistingAccount(char transactionType, Account account, decimal amount)
        {
            bool success = false;
            try
            {
                switch (transactionType)
                {
                    case 'D':
                        //account.Balance += amount;
                        account.deposit(amount);
                        break;
                    case 'W':
                        //account.Balance -= amount;
                        account.withdrawl(amount);
                        break;
                }

                ArrayList accounts = GetAccounts();
                int accountID = account.AccountID;

                // Find and replace the account
                for (int i = 0; i < accounts.Count; ++i)
                {
                    if (accountID == ((Account) accounts[i]).AccountID)
                    {
                        accounts[i] = account;
                    }
                }

                using (outFile = new StreamWriter(filename))
                {
                    foreach (Account acct in accounts)
                    {
                        outFile.WriteLine(acct.AccountID + DELIMETER + acct.Customer.CustomerID + DELIMETER + acct.Customer.FirstName + DELIMETER + acct.Customer.LastName + DELIMETER + acct.Balance + DELIMETER + Convert.ToInt32(acct.AccountType));
                    }
                    outFile.Close();
                }
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                success = false;
            }
            return success;
        }
 
        public ArrayList GetAccounts()
        {
            ArrayList accounts = new ArrayList();

            using (inFile = new StreamReader(filename))
            {
                int customerID;
                int accountID;
                string firstName;
                string lastName;
                decimal balance;
                int accountType;
                string line = string.Empty;

                while ((line = inFile.ReadLine()) != null)
                {
                    string [] data = line.Split(DELIMETER.ToCharArray()[0]);

                    customerID = Convert.ToInt32(data[0]);
                    accountID = Convert.ToInt32(data[1]);
                    firstName = Convert.ToString(data[2]);
                    lastName = Convert.ToString(data[3]);
                    balance = Convert.ToDecimal(data[4]);
                    accountType = Convert.ToInt32(data[5]);

                    Customer customer = new Customer(customerID, firstName, lastName);
                    Account account;

                    Account.ACCOUNTTYPE type = (Account.ACCOUNTTYPE) accountType;
                    if (type == Account.ACCOUNTTYPE.CHECKING)
                    {
                        account = new Checking(customer, accountID, Convert.ToDecimal(balance));
                    }
                    else
                    {
                        account = new Savings(customer, accountID, Convert.ToDecimal(balance));
                    }
                    
                    accounts.Add(account);
                }
                inFile.Close();
            }
            return accounts;
        }
    }
}
