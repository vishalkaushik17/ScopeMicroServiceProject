namespace DependencyInjection.SignalRMessaging
{
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        //public ChatHub(ibsa bsUser)
        //{

        //}
        //public async Task SendMessage(string user, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}

        //public async Task SendMessageToUser(string fromUser, string toUser, string message)
        //{
        //    var user = UserController.GetUserByUsername(toUser);
        //    if (user != null)
        //    {
        //        await Clients.Client(user.ConnectionId).SendAsync("ReceiveMessage", fromUser, message);
        //    }
        //}

        //public async Task SendMessageToGroup(string groupName, string user, string message)
        //{
        //    await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
        //}

        //public async Task AddToGroup(string groupName)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //}

        //public async Task RemoveFromGroup(string groupName)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        //}

        //public override async Task OnConnectedAsync()
        //{
        //    var username = Context.GetHttpContext().Request.Query["username"];
        //    UserController.UpdateUserConnectionId(username, Context.ConnectionId);
        //    await base.OnConnectedAsync();
        //}

        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    var username = Context.GetHttpContext().Request.Query["username"];
        //    UserController.UpdateUserConnectionId(username, null);
        //    await base.OnDisconnectedAsync(exception);
        //}
    }

}
