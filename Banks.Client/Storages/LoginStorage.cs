using System.Collections.Generic;
using System.Linq;
using Banks.Entities.Clients;

namespace Banks.Client.Storages
{
    public class LoginStorage
    {
        private static LoginStorage _instance;

        private readonly Dictionary<string, IClient> _clients;

        private LoginStorage()
        {
            _clients = new Dictionary<string, IClient>
            {
                { "m||k", new Entities.Clients.Client("Mikhail", "Koshkin") }
            };
        }

        private static LoginStorage GetInstance =>
            _instance ??= new LoginStorage();

        public static IClient GetClient(string loginPass)
        {
            return GetInstance._clients.ContainsKey(loginPass)
                ? GetInstance._clients[loginPass]
                : null;
        }

        public static bool AddClient(string loginPass, IClient client)
        {
            if (GetInstance._clients.ContainsKey(loginPass))
                return false;

            GetInstance._clients[loginPass] = client;
            return true;
        }

        public static bool LoginExists(string login)
        {
            return login == "bank"
                   || GetInstance._clients.Any(u =>
                       u.Key.Split("||")[0] == login);
        }
    }
}