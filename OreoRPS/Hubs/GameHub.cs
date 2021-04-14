using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace OreoRPS.Hubs
{
    public class GameHub : Hub
    {
        public async Task SendMove(string user, string choice)
        {
            await Clients.All.SendAsync("ReceiveMove", user, choice);
        }
    }
}
