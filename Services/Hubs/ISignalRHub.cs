using System.Threading.Tasks;

namespace Services.Hubs
{
    public interface ISignalRHub
    {
        Task ReceiveProgressMessage(int total, int count);
    }
}
