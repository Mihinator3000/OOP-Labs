using System.Threading.Tasks;

namespace Reports.Client
{
    internal class Program
    {
        private static async Task Main()
        {
            await new ClientService().Start();
        }
    }
}
