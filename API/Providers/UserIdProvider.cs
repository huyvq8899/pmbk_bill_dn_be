using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace API.Providers
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            string id = connection.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            return id;
        }
    }
}
