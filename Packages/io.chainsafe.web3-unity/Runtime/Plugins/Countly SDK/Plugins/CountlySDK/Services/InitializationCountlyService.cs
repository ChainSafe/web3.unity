using System.Linq;
using System.Threading.Tasks;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Models;

namespace Plugins.CountlySDK.Services
{
    public class InitializationCountlyService : AbstractBaseService
    {
        private readonly LocationService _locationService;
        private readonly SessionCountlyService _sessionService;

        internal InitializationCountlyService(CountlyConfiguration configuration, CountlyLogHelper logHelper, LocationService locationService, SessionCountlyService sessionCountlyService, ConsentCountlyService consentService) : base(configuration, logHelper, consentService)
        {
            Log.Debug("[InitializationCountlyService] Initializing.");

            _locationService = locationService;
            _sessionService = sessionCountlyService;
        }

        internal async Task OnInitialisationComplete()
        {
            lock (LockObj) {
                _ = _consentService.SendConsentChanges();
                _ = _sessionService.StartSessionService();
            }

            await Task.CompletedTask;
        }
    }
}
