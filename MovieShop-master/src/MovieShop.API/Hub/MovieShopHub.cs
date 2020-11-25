using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MovieShop.API.Hub
{
    public class MovieShopHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task SendMessageToClient()
        {
            // Broadcasting to all clients
            await Clients.All.SendAsync("discountNotification", "New Movie Discounts available");
        }
    }
}