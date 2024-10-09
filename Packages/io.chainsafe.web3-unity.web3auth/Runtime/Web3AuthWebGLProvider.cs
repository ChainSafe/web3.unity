using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.GamingSdk.Web3Auth;

/// <summary>
/// Connection provider for connecting via Web3Auth modal on WebGL.
/// </summary>
public class Web3AuthWebGLProvider : Web3AuthProvider
{
    private readonly Web3AuthWalletConfig _config;

    public Web3AuthWebGLProvider(Web3AuthWalletConfig config, Web3Environment environment, IChainConfig chainConfig) : base(config, environment, chainConfig)
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
