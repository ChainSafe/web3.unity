using System.Threading.Tasks;

namespace ChainSafe.Gaming.Lifecycle
{
    public interface ILifecycleParticipant
    {
        ValueTask WillStartAsync();

        ValueTask WillStopAsync();
    }
}
