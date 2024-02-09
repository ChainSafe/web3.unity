using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Web3.Core.Logout
{
    public class LogoutManager : ILogoutManager
    {
        private readonly IEnumerable<ILogoutHandler> handlers;
        private readonly ILogWriter logWriter;

        public LogoutManager(ILogWriter logWriter)
        {
            this.logWriter = logWriter;
            handlers = null;
        }

        public LogoutManager(IEnumerable<ILogoutHandler> handlers, ILogWriter logWriter)
        {
            this.logWriter = logWriter;
            this.handlers = handlers;
        }

        public async Task Logout()
        {
            if (handlers == null || !handlers.Any())
            {
                logWriter.Log("No logout handlers bound. Skipping the logout procedure.");
                return;
            }

            logWriter.Log("Logging out..");

            foreach (var handler in handlers)
            {
                await handler.OnLogout();
            }
        }
    }
}