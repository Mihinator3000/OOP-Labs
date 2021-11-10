using System;
using Banks.Entities.Accounts;
using Banks.Entities.Transactions;

namespace Banks.Client.ViewModels
{
    public class TransactionViewModel : ViewModel
    {
        public TransactionViewModel(ITransaction transaction)
        {
            TransactionInfo = transaction;

            switch (transaction)
            {
                case Replenishment:
                    Type = "Replenishment";
                    break;

                case Withdrawal:
                    Type = "Withdraw";
                    break;

                case Transaction t:
                    Type = "Transaction";
                    IsTransaction = true;
                    ReceiverAccount = t.Receiver;
                    break;
                    
                default:
                    throw new ArgumentException(nameof(transaction));
            }
        }

        public ITransaction TransactionInfo { get; }

        public bool NotCancelled =>
            !(TransactionInfo.Cancelled
            || ReceiverAccount is DepositAccount);

        public string Type { get; }

        public bool IsTransaction { get; }

        public AbstractAccount ReceiverAccount { get; }

        public void NotifyCancelledChanged()
        {
            OnPropertyChanged(nameof(NotCancelled));
        }
    }
}