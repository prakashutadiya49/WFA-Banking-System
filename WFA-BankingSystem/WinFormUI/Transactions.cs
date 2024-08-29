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
    public partial class Transactions : Form
    {
        private Customer _customer;
        public Transactions(Customer customer)
        {
            InitializeComponent();
            _customer = customer;

            customerText.Text = _customer.CustomerName;

            //overdraft event change for transaction UI.

            _customer.CheckingAccount.OverDraftEvent += CheckingAccount_OverDraftEvent;

            //overAmount event  change for transaction UI.

            _customer.CheckingAccount.OverAmountEvent += CheckingAccount_OverAmountEvent;

        }

        //code for overdraft event:
        private void CheckingAccount_OverAmountEvent(object sender, OverAmountEventArgs obj)
        {
            errorMessage.Text = $"Both Account Balance Sum only {obj.totalbalance}!";
            errorMessage.Visible = true;
        }

        private void CheckingAccount_OverDraftEvent(object sender, decimal e)
        {
            errorMessage.Visible = true;
        }

        private void makePurchaseButton_Click(object sender, EventArgs e)
        {
            bool paymentResult = _customer.CheckingAccount.MakePayment("Credit Card Purchase", amountValue.Value, _customer.SavingsAccount);
            amountValue.Value = 0;
        }

        private void customerText_Click(object sender, EventArgs e)
        {

        }

        private void errorMessage_Click(object sender, EventArgs e)
        {
            errorMessage.Visible = false;
        }
    }
}
