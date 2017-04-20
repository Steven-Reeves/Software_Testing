using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Banking;
using System.Collections;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Account currentAccount = null;
        public ArrayList accounts { get; set; }

        public FileDatabase database { get; set; }


        public Form1()
        {
            InitializeComponent();
            this.InitBankRead();
        }

        private void InitBankRead()
        {
            try
            {
                database = new FileDatabase();

                this.accounts = database.GetAccounts();
                this.dataGridView1.DataSource = accounts;
                
                dataGridView1.Columns["Balance"].DefaultCellStyle.Format = "C";

                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    dataGridView1.Columns[column.Name].SortMode = DataGridViewColumnSortMode.Automatic;
                }
            }
            catch (Exception e)
            {
                this.lblErrors.Text = e.ToString();
                MessageBox.Show(e.ToString());
            }
            finally
            {

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            decimal amount = Convert.ToDecimal(txtTransactionAmount.Text);
            char transactionType = ' ';

            if (this.rdbDeposit.Checked)
            {
                transactionType = 'D';
            }
            else if (this.rdbWithdrawal.Checked)
            {
                transactionType = 'W';
            }


            database.UpdateExistingAccount(transactionType, currentAccount, amount);


            this.UpdateCurrentAccountInfo();



            MessageBox.Show("Account: " + currentAccount.AccountID + "\n" + "New Balance: " + currentAccount.Balance.ToString("C"));

            int rowIndex = -1;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["AccountID"].Value.ToString().Equals(currentAccount.AccountID.ToString()))
                {
                    rowIndex = row.Index;
                    break;
                }
            }
            InitBankRead();
            dataGridView1.Rows[rowIndex].Selected = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            txtTransactionAmount.Clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 | e.ColumnIndex < 0)
                return;

            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                int accountID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["AccountID"].Value.ToString());
                Account account = GetAccount(accountID);
                this.currentAccount = account;

                this.UpdateCurrentAccountInfo();

                this.rdbWithdrawal.Enabled = true;
                this.rdbDeposit.Enabled = true;

                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void UpdateCurrentAccountInfo()
        {
            Account account = currentAccount;

            this.lblAccountNo.Text = account.AccountID.ToString();
            this.lblCustomerName.Text = account.Customer.ToString();
            this.lblAccountType.Text = account.AccountType.ToString();
            this.lblCurrentBalance.Text = account.Balance.ToString("C");
        }
        private Account GetAccount(int accountID)
        {
            Account account = null;

            foreach (Account acct in accounts)
            {
                if (acct.AccountID == accountID)
                {
                    account = acct;
                    break;
                }
            }
            return account;
        }

        private void rdbDeposit_CheckedChanged(object sender, EventArgs e)
        {
            this.txtTransactionAmount.Enabled = true;
            this.btnProcessTransaction.Enabled = true;
           
        }

        private void rdbWithdrawal_CheckedChanged(object sender, EventArgs e)
        {
            this.txtTransactionAmount.Enabled = true;
            this.btnProcessTransaction.Enabled = true;
        }

        private void refreshDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitBankRead();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int customerID = dataGridView1.Rows.Count;

            Account.ACCOUNTTYPE type = rdbChecking.Checked ? Account.ACCOUNTTYPE.CHECKING : Account.ACCOUNTTYPE.SAVINGS;

            database.AddNewCustomer(++customerID, this.txtFirstName.Text, this.txtLastName.Text, Convert.ToDecimal(this.txtOpeningDeposit.Text), type);

            MessageBox.Show("New Customer Added");
           

            InitBankRead();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void rdbSQLServer_CheckedChanged(object sender, EventArgs e)
        {
            InitBankRead();
        }

        private void rdbAccess_CheckedChanged(object sender, EventArgs e)
        {
            InitBankRead();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int customerID = dataGridView1.Rows.Count;

            Account.ACCOUNTTYPE type = rdbChecking.Checked ? Account.ACCOUNTTYPE.CHECKING : Account.ACCOUNTTYPE.SAVINGS;

            database.AddNewCustomer(++customerID, this.txtFirstName.Text, this.txtLastName.Text, Convert.ToDecimal(this.txtOpeningDeposit.Text), type);

            MessageBox.Show("New Customer Added");


            InitBankRead();
        }
    }
}
