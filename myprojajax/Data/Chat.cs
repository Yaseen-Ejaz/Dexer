using Microsoft.AspNet.SignalR;


public class ChatHub : Hub
{
    public void Send(string message)
    {
        // Broadcast the message to all connected clients.
        Clients.All.SendAsync("Send", message);
    }
}

