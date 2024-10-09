mergeInto(LibraryManager.library, {
    Request: async function (message, gameObjectName, callback, fallback) {
        const parsedObjectName = UTF8ToString(gameObjectName);
        const parsedCallback = UTF8ToString(callback);
        const parsedFallback = UTF8ToString(fallback);
        let request = JSON.parse(UTF8ToString(message));
        try
        {
            const response = await window.ethereum.request(request);
            let rpcResponse = 
            {
                jsonrpc: "2.0",
                result: response,
                id: request.id,
                error: null
            }
            
            var json = JSON.stringify(rpcResponse);
            web3UnityInstance.SendMessage(parsedObjectName, parsedCallback, json);
        }
        catch(e)
        {
            let rpcResponse = 
            {
                jsonrpc: "2.0",
                id: request.id,
                error: 
                {
                    message: e.message
                }
            }
            var json = JSON.stringify(rpcResponse);
            web3UnityInstance.SendMessage(parsedObjectName, parsedFallback, json);
        }
    },
    Connect: async function (chain, gameObjectName, callback, fallback) {
        const parsedObjectName = UTF8ToString(gameObjectName);
        const parsedCallback = UTF8ToString(callback);
        const parsedFallback = UTF8ToString(fallback);
        const parsedChain = JSON.parse(UTF8ToString(chain));
        try
        {
            const accounts = await window.ethereum.request({ method: 'eth_requestAccounts' });
            
            if (window.ethereum.request({ method: 'eth_chainId' }) != parsedChain.chainId)
            {
                try
                {
                    await window.ethereum.request({ method: 'wallet_switchEthereumChain', params: [{ chainId: parsedChain.chainId }] });
                }
                catch(e)
                {
                    if (e.code == 4902)
                    {
                        await window.ethereum.request({ method: 'wallet_addEthereumChain', params: [parsedChain] });
                    }
                    
                    else
                    {
                        web3UnityInstance.SendMessage(parsedObjectName, parsedFallback, e.message);
                        
                        return null;
                    }
                }
            }
            
            web3UnityInstance.SendMessage(parsedObjectName, parsedCallback, accounts[0]);
        }
        catch(e)
        {
            web3UnityInstance.SendMessage(parsedObjectName, parsedFallback, e.message);
        }
    },
    IsMetaMask: function() {
        return window.ethereum.isMetaMask;
    }
});