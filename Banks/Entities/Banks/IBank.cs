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

        decimal MaxSummForDubiousClients { get; set; }
        decimal CreditComission { get; set; }
        decimal CreditLimit { get; set; }
        decimal DebitInterest { get; set; }

        AbstractAccount AddAccount(IClient client, AccountTypes accountType, decimal balance = 0);
        List<AbstractAccount> FindAccounts(IClient client);
        void DeleteAccount(AbstractAccount account);

        void Replenish(AbstractAccount account, decimal amount);
        void Transact(AbstractAccount sender, AbstractAccount receiver, decimal amount);
        void Transact(AbstractAccount sender, Guid receiverId, decimal amount);
        void Withdraw(AbstractAccount account, decimal amount);

        void AccrualOfInterestOrCommission(object sender, EventArgs args);
    }
}