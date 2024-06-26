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
            console.log(json);
            nethereumUnityInstance.SendMessage(parsedObjectName, parsedCallback, json);
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
            console.log(e.message);
            nethereumUnityInstance.SendMessage(parsedObjectName, parsedFallback, json);
        }
    }
});