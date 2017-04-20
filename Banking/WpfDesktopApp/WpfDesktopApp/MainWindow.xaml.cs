using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Banking;

namespace WpfDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Account currentAccount = null;
        public ArrayList accounts { get; set; }
        public FileDatabase database { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            database = new FileDatabase();
            BankRead();
        }

        private void BankRead()
        {
            accounts = database.GetAccounts();
            dgAccountList.ItemsSource = accounts;
            dgAccountList.SelectionMode = DataGridSelectionMode.Single;
            dgAccountList.SelectedIndex = 0;
        }

        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            int customerID = dgAccountList.Items.Count; //dataGridView1.Rows.Count;

            Account.ACCOUNTTYPE type = (bool) rdbChecking.IsChecked
                ? Account.ACCOUNTTYPE.CHECKING
                : Account.ACCOUNTTYPE.SAVINGS;

            database.AddNewCustomer(++customerID, this.txtFirstName.Text, this.txtLastName.Text,
                Convert.ToDecimal(this.txtOpeningDeposit.Text), type);

            MessageBox.Show("New Customer Added");

            BankRead();

            SelectAccountInDataGrid(customerID.ToString());

        }

        private void dgAccountList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentAccount = (Account) dgAccountList.SelectedItem;
            if (currentAccount == null)
            {
                return;
            }

            this.txtAccountSelected.Content = currentAccount.ToString();
            this.txtBalance.Content = currentAccount.Balance.ToString("C");
        }

        private void btnProcessTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (currentAccount == null)
            {
                MessageBox.Show("You must select an account first");
                return;
            }

            decimal amount = Convert.ToDecimal(txtTransactionAmount.Text);
            char transactionType = (bool) rdbDeposit.IsChecked ? 'D' : 'W';

            database.UpdateExistingAccount(transactionType, currentAccount, amount);

            MessageBox.Show("Account updated");

            currentAccount = database.GetAccounts()
                .Cast<Account>()
                .ToList()
                .SingleOrDefault(x => x.AccountID == currentAccount.AccountID);
            this.txtBalance.Content = currentAccount.Balance;
            var accountNumber = currentAccount.AccountID.ToString();
            BankRead();

            SelectAccountInDataGrid(accountNumber);
            this.txtTransactionAmount.Clear();

        }

        private void SelectAccountInDataGrid(string id)
        {
            for (int i = 0; i < dgAccountList.Items.Count; i++)
            {

                dgAccountList.ScrollIntoView(dgAccountList.Items[i]);
                DataGridRow row = (DataGridRow) dgAccountList.ItemContainerGenerator.ContainerFromIndex(i);
                TextBlock cellContent = dgAccountList.Columns[0].GetCellContent(row) as TextBlock;
                if (cellContent != null && cellContent.Text.Equals(id))
                {
                    object item = dgAccountList.Items[i];
                    dgAccountList.SelectedItem = item;
                    dgAccountList.ScrollIntoView(item);
                    row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    break;
                }
            }
        }
    }
}
