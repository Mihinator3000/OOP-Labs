using System.Collections.Generic;
using Banks.Entities.Clients.Passport;

namespace Banks.Entities.Clients
{
    public interface IClient
    {
        string Name { get; }

        string Surname { get; }

        string Address { get; set; }

        PassportInfo Passport { get; set; }

        decimal Balance { get; set; }

        List<string> Notifications { get; }

        void ClearNotifications();

        bool IsDubious();
    }
}