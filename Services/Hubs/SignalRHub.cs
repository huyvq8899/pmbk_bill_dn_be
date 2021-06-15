using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Services.Hubs
{
    [Authorize]
    public class SignalRHub : Hub<ISignalRHub>
    {
        public SignalRHub()
        {
        }
    }
}
