using Microsoft.AspNet.SignalR;

namespace com.AppliedLine.CargoCanal.WebAPI.Hubs
{
    [Microsoft.AspNet.SignalR.Hubs.HubName("accountHub")]
    public class AccountHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}