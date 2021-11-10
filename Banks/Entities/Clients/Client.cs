using System.Collections.Generic;
using Banks.Entities.Clients.Passport;
using Banks.Tools;

namespace Banks.Entities.Clients
{
    public class Client : IClient
    {
        private decimal _balance;

        public Client(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        public Client(string name, string surname, string address, PassportInfo passport)
            : this(name, surname)
        {
            Address = address;
            Passport = passport;
        }

        public string Name { get; }

        public string Surname { get; }

        public string Address { get; set; }

        public PassportInfo Passport { get; set; }

        public decimal Balance
        {
            get => _balance;
            set
            {
                if (value < 0)
                    throw new BanksException("User's balance can not be less than 0");

                _balance = value;
            }
        }

        public List<string> Notifications { get; } = new ();

        public void AddNotification(string notification)
        {
            int outdatetNotificationIndex = Notifications.IndexOf(notification);
            if (outdatetNotificationIndex != -1)
                Notifications.RemoveAt(outdatetNotificationIndex);

            Notifications.Add(notification);
        }

        public void ClearNotifications()
        {
            Notifications.Clear();
        }

        public bool IsDubious()
        {
            return Address is null ||
                   Passport is null;
        }
    }
}