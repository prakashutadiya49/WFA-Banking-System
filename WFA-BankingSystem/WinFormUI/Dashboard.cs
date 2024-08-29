using DemoLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormUI
{
    public partial class Dashboard : Form
    {
        Customer customer = new Customer();

        public Dashboard()
        {
            InitializeComponent();

            LoadTestingData();

            WireUpForm();
        }

        private void LoadTestingData()
        {
            customer.CustomerName = "prakash Utadiya";
            customer.CheckingAccount = new Account();
            customer.SavingsAccount = new Account();

            customer.CheckingAccount.AccountName = "Prakash Utadiya Checking Account";
            customer.SavingsAccount.AccountName = "Prakash Utadiya Saving Account";

            customer.CheckingAccount.AddDeposit("Initial Balance", 1500M);
            customer.SavingsAccount.AddDeposit("Initial Balance", 5550.50M);
        }

        private void WireUpForm()
        {
            customerText.Text = customer.CustomerName;
            checkingTransactions.DataSource = customer.CheckingAccount.Transactions;
            savingsTransactions.DataSource = customer.SavingsAccount.Transactions;
            checkingBalanceValue.Text = string.Format("{0:C2}", customer.CheckingAccount.Balance);
            savingsBalanceValue.Text = string.Format("{0:C2}", customer.SavingsAccount.Balance);

            //event Listners:
            //for cheaking account:

            customer.CheckingAccount.TransactionApprovedEvent += CheckingAccount_TransactionApprovedEvent;

            //for saving account:

            customer.SavingsAccount.TransactionApprovedEvent += SavingsAccount_TransactionApprovedEvent;

            //for overdraft event:

            customer.CheckingAccount.OverDraftEvent += CheckingAccount_OverDraftEvent;

            //for overAmount event:

            customer.CheckingAccount.OverAmountEvent += this.CheckingAccount_OverAmountEvent;


        }


        // this run when enter amount> saving+cheaking account balance.

        private void CheckingAccount_OverAmountEvent(object sender, OverAmountEventArgs obj)
        {
            errorMessage.Text = obj.message;
            errorMessage.Visible = true;
        }

        //this event run when overdraft is happen......
        private void CheckingAccount_OverDraftEvent(object sender, decimal e)
        {
            errorMessage.Text = $"You had an overdraft protection transfer of {string.Format("{0:C2}",e)} ";
            errorMessage.Visible = true;
        }

        //saving account event listner:
        //this code run when saving account balance changed:
        private void SavingsAccount_TransactionApprovedEvent(object sender, string e)
        {
            savingsTransactions.DataSource = null;
            savingsTransactions.DataSource = customer.SavingsAccount.Transactions;
            savingsBalanceValue.Text = string.Format("{0:C2}", customer.SavingsAccount.Balance);
           

        }

        //cheaking account event listner: 
        //this code run when event occur.
        private void CheckingAccount_TransactionApprovedEvent(object sender, string e)
        {
            checkingTransactions.DataSource = null;
            checkingTransactions.DataSource = customer.CheckingAccount.Transactions;
            checkingBalanceValue.Text = string.Format("{0:C2}", customer.CheckingAccount.Balance);


        }

        private void recordTransactionsButton_Click(object sender, EventArgs e)
        {
            Transactions transactions = new Transactions(customer);
            transactions.Show();
        }

        private void errorMessage_Click(object sender, EventArgs e)
        {
            errorMessage.Visible = false;
        }

        private void customerLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
