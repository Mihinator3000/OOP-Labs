using Banks.Entities.Accounts;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Enums;

namespace Banks.Client.Storages
{
    public class Storage
    {
        private static Storage _instance;

        public IMainBank MainBank { get; }

        public IBank Bank =>
            MainBank.GetBank(0);

        private Storage()
        {
            MainBank = new MainBank();

            MainBank.RegisterBank(
                new BankBuilder()
                    .SetMaxSummForDubiousClients(1000)
                    .SetCreditComission(10)
                    .SetDebitInterest(10)
                    .SetCreditLimit(-100)
                    .Build());

            IClient client = MainBank.AddClient(
                LoginStorage.GetClient("m||k"));

            AbstractAccount account = Bank.AddAccount(client, AccountTypes.Credit);
            account.NotifyClient = true;

            Bank.Withdraw(account, 10);
            AbstractAccount receiver = Bank.AddAccount(client, AccountTypes.Debit);
            Bank.Transact(account, receiver, 20);

        }

        public static Storage GetInstance =>
            _instance ??= new Storage();
    }
}