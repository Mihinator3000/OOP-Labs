using System;
using System.Collections.Generic;
using Banks.Entities.Accounts;
using Banks.Entities.Clients;
using Banks.Entities.Transactions;
using Banks.Enums;

namespace Banks.Entities.Banks
{
    public interface IBank
    {
        List<AbstractAccount> Accounts { get; }
        List<ITransaction> Transactions { get; }

        public decimal MaxSummForDubiousClients { get; set; }
        public decimal CreditComission { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal DebitInterest { get; set; }

        public void AddAccount(IClient client, AccountTypes accountType, decimal balance);
        public List<AbstractAccount> FindAccounts(IClient client);
        public void DeleteAccount(AbstractAccount account);

        public void Replenish(AbstractAccount account, decimal amount);
        public void Transact(AbstractAccount sender, AbstractAccount receiver, decimal amount);
        public void Withdraw(AbstractAccount account, decimal amount);

        void AccrualOfInterestOrCommission(object sender, EventArgs args);
    }
}