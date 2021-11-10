using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Entities.Clients;
using Banks.Entities.Clients.Passport;
using Banks.Tools;

namespace Banks.Entities.Banks
{
    public class MainBank : IMainBank
    {
        private event EventHandler Notify;

        public List<IBank> Banks { get; } = new ();

        public List<IClient> Clients { get; } = new ();

        public IBank RegisterBank(IBank bank)
        {
            Banks.Add(bank);
            Notify += bank.AccrualOfInterestOrCommission;
            return bank;
        }

        public IBank GetBank(int id)
        {
            if (id < 0 || id >= Banks.Count)
                throw new BanksException("Incorrect bank id");

            return Banks[id];
        }

        public void DeleteBank(IBank bank)
        {
            Banks.Remove(bank);
            Notify -= bank.AccrualOfInterestOrCommission;
        }

        public IClient AddClient(IClient client)
        {
            Clients.Add(client);
            return client;
        }

        public List<IClient> FindClients(string name, string surname)
        {
            return Clients
                .Where(u => u.Name == name
                            && u.Surname == surname)
                .ToList();
        }

        public IClient GetClient(PassportInfo passportInfo)
        {
            return Clients
                .FirstOrDefault(u =>
                    u.Passport.Equals(passportInfo));
        }

        public void DeleteClient(IClient client)
        {
            Clients.Remove(client);
            Banks.ForEach(u =>
            {
                u.FindAccounts(client)
                    .ForEach(a => u.Accounts
                        .Remove(a));
            });
        }

        public void AccruePercentage()
        {
            Notify?.Invoke(this, EventArgs.Empty);
        }
    }
}