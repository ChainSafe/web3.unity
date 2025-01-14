using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Operations;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.GamingSdk.Web3Auth;

/// <summary>
/// Connection provider for connecting via Web3Auth modal on WebGL.
/// </summary>
public class Web3AuthWebGLProvider : Web3AuthProvider
{
    private readonly IWeb3AuthConfig _config;

    public Web3AuthWebGLProvider(IWeb3AuthConfig config, Web3Environment environment, IChainConfig chainConfig, IOperationTracker operationTracker)
        : base(config, environment, chainConfig, operationTracker)
    {
        _config = config;
    }

    public override async Task<string> Connect()
    {
        string sessionId = await _config.SessionTask;

        KeyStoreManagerUtils.savePreferenceData(KeyStoreManagerUtils.SESSION_ID, sessionId);

        return await base.Connect();
    }
}
