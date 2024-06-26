var Web3AuthWebGLNoModal =  {
    $Web3AuthWebGLNoModal : {},
    cs_inject_web3auth: function () {
        console.log("Injecting Web3Auth scripts...")
        const scripts = [ "https://cdn.jsdelivr.net/npm/@web3auth/no-modal",
        "https://cdn.jsdelivr.net/npm/@web3auth/wallet-services-plugin",
        "https://cdn.jsdelivr.net/npm/@web3auth/openlogin-adapter",
        "https://cdn.jsdelivr.net/npm/@web3auth/ethereum-provider"];
        scripts.forEach((scriptUrl) => {
            const script = document.createElement("script");
            script.src = scriptUrl;
            document.head.appendChild(script);
        });
    },

    cs_web3auth_setLoginCallback: function (login) {
        Web3AuthWebGLNoModal.loginCallback = function(sessionId){
            var returnStr = sessionId;
            var bufferSize = lengthBytesUTF8(returnStr) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(returnStr, buffer, bufferSize);
            Module.dynCall_vi(login, [buffer]);
        };
    },
    InitWeb3Auth: function (clientId, chainId, rpcTarget, displayName, blockExplorerUrl, ticker, tickerName, network) {
        window.web3auth = null;
        window.walletServicesPlugin = null;

        (async function init() {
            console.log("Initializing Web3Auth...");

            const chainConfig = {
                chainNamespace: "eip155", // Default to Ethereum namespace
                chainId: UTF8ToString(chainId),
                rpcTarget: UTF8ToString(rpcTarget),
                displayName: UTF8ToString(displayName),
                blockExplorerUrl: UTF8ToString(blockExplorerUrl),
                ticker: UTF8ToString(ticker),
                tickerName: UTF8ToString(tickerName),
            };

            const privateKeyProvider = new window.EthereumProvider.EthereumPrivateKeyProvider({ config: { chainConfig } });
            window.web3auth = new window.NoModal.Web3AuthNoModal({
                clientId: UTF8ToString(clientId),
                privateKeyProvider,
                web3AuthNetwork: UTF8ToString(network),
            });

            window.web3auth.on("connected", (data) => {
                const openLoginStore = localStorage.getItem("openlogin_store");
                const openLoginStoreObj = JSON.parse(openLoginStore);
                const sessionId = openLoginStoreObj.sessionId;

                Web3AuthWebGLNoModal.loginCallback(sessionId);
            });

            const openloginAdapter = new window.OpenloginAdapter.OpenloginAdapter();
            window.web3auth.configureAdapter(openloginAdapter);

            await window.web3auth.init();

            console.log("Web3Auth Initialized Successfully!");
        })();
    },

    Web3AuthLogin: async function (provider, rememberMe) {
        try {
            await window.web3auth.connectTo("openlogin", {
                loginProvider: UTF8ToString(provider)
            });
            if (!rememberMe){
                localStorage.removeItem("openlogin_store");
            }
        } catch (error) {
            console.log(error.message);
        }
    },
};

autoAddDeps(Web3AuthWebGLNoModal, '$Web3AuthWebGLNoModal');
mergeInto(LibraryManager.library,Web3AuthWebGLNoModal);
