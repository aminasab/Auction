using Microsoft.AspNetCore.SignalR;
using OnlineAuction.Models;

namespace OnlineAuction.Hubs
{
    public class MandarinsHub : Hub
    {
        public async Task SendMandarinUpdate(Mandarin mandarin)
        {
            await Clients.All.SendAsync("ReceiveMandarinUpdate", mandarin);
        }

        public async Task UpdateMandarin(int id, decimal price)
        {
            await Clients.All.SendAsync("UpdateMandarin", id, price);
        }
    }
}