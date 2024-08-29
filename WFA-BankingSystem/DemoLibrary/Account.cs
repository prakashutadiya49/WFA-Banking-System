using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Acoount class:
namespace DemoLibrary
{
    public class Account
    {
        //event when user widrow over limit amount.

        public event EventHandler<OverAmountEventArgs> OverAmountEvent;

        //event declare and send from where balance is change.

        public event EventHandler<string> TransactionApprovedEvent;

        //overdraft event:

        public event EventHandler<decimal> OverDraftEvent;

        public string AccountName { get; set; }
        public decimal Balance { get; private set; }

        private List<string> _transactions = new List<string>();

        public IReadOnlyList<string> Transactions
        {
            get { return _transactions.AsReadOnly(); }
        }

        // add amount method:
        public bool AddDeposit(string depositName, decimal amount)
        {
            _transactions.Add($"Deposited { string.Format("{0:C2}", amount) } for { depositName }");
            Balance += amount;
            TransactionApprovedEvent?.Invoke(this, depositName);
            return true;
        }

        // pay amount method: 
        public bool MakePayment(string paymentName, decimal amount, Account backupAccount = null)
        {
            // money is available or not. if yes-->
            if (Balance >= amount)
            {
                _transactions.Add($"Withdrew {string.Format("{0:C2}", amount)} for {paymentName}");
                Balance -= amount;
                TransactionApprovedEvent?.Invoke(this, paymentName);
                return true;
            }
            //if no--->
            else
            {

                // we check to see if we have a backup account or not;
                if (backupAccount != null)
                {
                    // Checks to see if we have enough money in the backup account
                    if ((backupAccount.Balance + Balance) >amount)
                    {
                        // We have enough backup funds so transfar the amount to this account
                        // and then complete the transaction.
                        decimal amountNeeded = amount - Balance;
                        bool overdraftSucceeded = backupAccount.MakePayment("Overdraft Protection", amountNeeded);

                        // This should always be true but we will check anyway
                        if (overdraftSucceeded == false)
                        {
                            // The overdraft failed so this transaction failed.
                            OverAmountEvent?.Invoke(this, new OverAmountEventArgs(backupAccount.Balance + Balance, amount, $"Cheaking and Saving Account Balance Sum {string.Format("{0:C2}", backupAccount.Balance + Balance)} less than {amount} "));
                            return false;
                        }

                        //money of backup account is added in current account.

                        AddDeposit("Overdraft Protection Deposit", amountNeeded);

                        _transactions.Add($"Withdrew {string.Format("{0:C2}", amount)} for {paymentName}");
                        Balance -= amount;
                        TransactionApprovedEvent?.Invoke(this, paymentName);
                        OverDraftEvent?.Invoke(this, amountNeeded);
                        return true;
                    }
                    else
                    {
                        OverAmountEvent?.Invoke(this, new OverAmountEventArgs(backupAccount.Balance + Balance, amount, $"Cheaking and Saving Account Balance Sum {string.Format("{0:C2}", backupAccount.Balance + Balance)} less than {amount} "));

                        return false;
                    }
                }
                else
                {
                    // No backup so we fail and do nothing
                    // Not enough backup funds to do anything
                   
                    return false;
                }
            }
        }
    }
}
