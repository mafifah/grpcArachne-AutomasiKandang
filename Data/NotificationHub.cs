using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace grpcArachne.Data
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
