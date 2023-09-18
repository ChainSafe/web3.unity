using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChainSafe.Gaming.Web3.Core
{
    public interface ILifecycleParticipant
    {
        ValueTask WillStartAsync();

        ValueTask WillStopAsync();
    }
}
