using Microsoft.AspNetCore.SignalR;
namespace GenericFunction.GlobalService.NotificationServices.BrowserNotificationService.Services;
public class RealTimeHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
//using Microsoft.AspNetCore.SignalR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GenericFunction.GlobalService.NotificationServices.BrowserNotificationService.Services
//{
//    public class BrowserNotificationHub : Hub
//    {
//        public static int TotalViews { get; set; } = 0;
//        public static int TotalUsers { get; set; } = 0;

//        public override Task OnConnectedAsync()
//        {
//            TotalUsers++;
//            Clients.All.SendAsync("updateTotalUsers", TotalUsers).GetAwaiter().GetResult();
//            return base.OnConnectedAsync();
//        }

//        public override Task OnDisconnectedAsync(Exception? exception)
//        {
//            TotalUsers--;
//            Clients.All.SendAsync("updateTotalUsers", TotalUsers).GetAwaiter().GetResult();
//            return base.OnDisconnectedAsync(exception);
//        }


//        public async Task<string> NewWindowLoaded(string name)
//        {
//            TotalViews++;
//            //send update to all clients that total views have been updated
//            await Clients.All.SendAsync("updateTotalViews", TotalViews);
//            return $"total views from {name} - {TotalViews}";
//        }
//    }
//}
