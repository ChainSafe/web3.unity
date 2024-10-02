var Web3AuthWebGLNoModal =  {
    $Web3AuthWebGLNoModal : {},

    InitWeb3Auth: async function (clientId, chainId, rpcTarget, displayName, blockExplorerUrl, ticker, tickerName, network, callback, fallback) {
        
        window.web3auth = null;
        window.walletServicesPlugin = null;

        try 
        {
            console.log("Initializing Web3Auth...");
            
            const chainConfig = 
            {
                chainNamespace: "eip155",
                chainId: UTF8ToString(chainId),
                rpcTarget: UTF8ToString(rpcTarget),
                displayName: UTF8ToString(displayName),
                blockExplorerUrl: UTF8ToString(blockExplorerUrl),
                ticker: UTF8ToString(ticker),
                tickerName: UTF8ToString(tickerName),
            };
            
            const privateKeyProvider = new window.EthereumProvider.EthereumPrivateKeyProvider({ config: { chainConfig } });
            window.web3auth = new window.NoModal.Web3AuthNoModal(
            {
                clientId: UTF8ToString(clientId),
                privateKeyProvider,
                web3AuthNetwork: UTF8ToString(network),
            });
        
            window.web3auth.on("connected", (data) => 
            {
                const openLoginStore = localStorage.getItem("openlogin_store");
                const openLoginStoreObj = openLoginStore ? JSON.parse(openLoginStore) : null;
                if (openLoginStoreObj && openLoginStoreObj.sessionId)
                {
                    const sessionId = openLoginStoreObj.sessionId;
                    if (Web3AuthWebGLNoModal.loginCallback !== undefined)
                    {
                        Web3AuthWebGLNoModal.loginCallback(sessionId);
                    }
                }
                else
                {
                    console.log("No session ID found in localStorage.");
                }
            });
        
            const openloginAdapter = new window.OpenloginAdapter.OpenloginAdapter({ privateKeyProvider });
            window.web3auth.configureAdapter(openloginAdapter);
            
            await window.web3auth.init();
            
            Module.dynCall_v(callback);
        } 
        catch (error)
        {
            var stringToReturn = error.message;
            var bufferSize = lengthBytesUTF8(stringToReturn) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(stringToReturn, buffer, bufferSize);
            Module.dynCall_vi(fallback, [buffer]);
        }
    },
    Web3AuthLogin: async function (provider, rememberMe, callback, fallback) {
        
        try {
            Web3AuthWebGLNoModal.loginCallback = function(sessionId){
                var bufferSize = lengthBytesUTF8(sessionId) + 1;
                var buffer = _malloc(bufferSize);
                stringToUTF8(sessionId, buffer, bufferSize);
                Module.dynCall_vi(callback, [buffer]);
            };
            
            await window.web3auth.connectTo("openlogin", {
                loginProvider: UTF8ToString(provider)
            });
            if (!rememberMe){
                localStorage.removeItem("openlogin_store");
            }
        } catch (error) {
            var stringToReturn = error.message;
            var bufferSize = lengthBytesUTF8(error.message) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(stringToReturn, buffer, bufferSize);
            Module.dynCall_vi(fallback, [buffer]);
        }
    },
};

autoAddDeps(Web3AuthWebGLNoModal, '$Web3AuthWebGLNoModal');
mergeInto(LibraryManager.library,Web3AuthWebGLNoModal);
