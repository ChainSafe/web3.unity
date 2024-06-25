var Web3AuthWebGLNoModal =  {
    $Web3AuthWebGLNoModal : {},

     cs_web3auth_setLoginCallback: function (login) {
        Web3AuthWebGLNoModal.loginCallback = function(sessionId){
            var returnStr = sessionId;
            var bufferSize = lengthBytesUTF8(returnStr) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(returnStr, buffer, bufferSize);
            Module['dynCall_vi'](login, [buffer]);
        };
    },
    InitWeb3Auth: function (clientId, chainId, rpcTarget, displayName, blockExplorerUrl, ticker, tickerName, network) {
        // Initialize web3auth and other variables
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
                // Step 1: Retrieve the item from localStorage
                const openLoginStore = localStorage.getItem("openlogin_store");

                // Step 2: Parse the string into an object
                const openLoginStoreObj = JSON.parse(openLoginStore);

                // Step 3: Access the sessionId property
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
                localStorage.removeItem("openlogin_store", JSON.stringify({ sessionId: window.web3auth.sessionId }));
            }
        } catch (error) {
            console.log(error.message);
        }
    },

    Web3AuthLogout: function () {
        window.web3auth.logout().then(() => {
            $(".btn-logged-in").hide();
            $(".btn-logged-out").show();
        }).catch(error => {
            console.error(error.message);
        });
    }
};

autoAddDeps(Web3AuthWebGLNoModal, '$Web3AuthWebGLNoModal');
mergeInto(LibraryManager.library,Web3AuthWebGLNoModal);
