mergeInto(LibraryManager.library, {
    // Global variable to store the loaded modules and configuration
    _appKitConfig: null,

    $SerializeJson: function (obj) {
        let cache = [];
        let resultJson = JSON.stringify(obj, (key, value) => {
            // Handle circular references
            if (typeof value === 'object' && value !== null) {
                if (cache.includes(value)) return;
                cache.push(value);
            }
            // Check if the value is a BigInt and convert it to a string
            if (typeof value === 'bigint') {
                return value.toString();
            }
            return value;
        });
        cache = null;
        return resultJson;
    },

    $ExecuteCall__deps: ['$SerializeJson'],
    $ExecuteCall: async function (callFn, id, methodNameStrPtr, parameterStrPtr, callbackPtr) {
        if (!_appKitConfig) {
            console.error("AppKit is not initialized. Call Initialize first.");
            return;
        }

        // Convert the method name and parameter to JS strings
        let methodName = UTF8ToString(methodNameStrPtr);
        let parameterStr = UTF8ToString(parameterStrPtr);
        
        let parameterObj = parameterStr === "" ? undefined : JSON.parse(parameterStr);

        try {
            // Call the method using the provided function
            let result = await callFn(_appKitConfig, methodName, parameterObj);

            if (result === undefined || result === null) {
                {{{makeDynCall('viii', 'callbackPtr')}}}(id, undefined, undefined);
                return;
            }

            let resultJson = SerializeJson(result);
            
            // Call the callback with the result
            let resultStrPtr = stringToNewUTF8(resultJson);
            {{{makeDynCall('viii', 'callbackPtr')}}}(id, resultStrPtr, undefined);
            _free(resultStrPtr);
        } catch (error) {
            console.error("[AppKit] Error executing call", error);
            let errorJson = JSON.stringify(error, ['name', 'message']);
            let errorStrPtr = stringToNewUTF8(errorJson);
            {{{makeDynCall('viii', 'callbackPtr')}}}(id, undefined, errorStrPtr);
            _free(errorStrPtr);
        }
    },

    // Preload the scripts from CDN, initialize the configuration and create the modal
    Initialize: function (parametersJsonPtr, callbackPtr) {
        const parametersJson = UTF8ToString(parametersJsonPtr);
        const parameters = JSON.parse(parametersJson);

        const projectId = parameters.projectId;
        const metadata = parameters.metadata;
        const chains = parameters.chains;

        const enableOnramp = parameters.enableOnramp;
        const enableAnalytics = parameters.enableAnalytics;

        // Load the scripts and initialize the configuration
        import("https://cdn.jsdelivr.net/npm/@web3modal/cdn@5.1.2/dist/wagmi.js").then(CDNW3M => {
            const WagmiCore = CDNW3M['WagmiCore'];
            const Chains = CDNW3M['Chains'];
            const Web3modal = CDNW3M['Web3modal'];
            const Connectors = CDNW3M['Connectors'];

            const createWeb3Modal = Web3modal['createWeb3Modal'];
            const coinbaseWallet = Connectors['coinbaseWallet'];
            const walletConnect = Connectors['walletConnect'];
            const injected = Connectors['injected'];
            const createConfig = WagmiCore['createConfig'];
            const http = WagmiCore['http'];
            const reconnect = WagmiCore['reconnect'];

            const chainsMap = chains.map(chainName => Chains[chainName]);

            const config = createConfig({
                chains: chainsMap,
                transports: chains.reduce((acc, chainName) => {
                    acc[Chains[chainName].id] = http();
                    return acc;
                }, {}),
                connectors: [
                    walletConnect({projectId, metadata, showQrModal: false}),
                    injected({shimDisconnect: true}),
                    coinbaseWallet({
                        appName: metadata.name,
                        appLogoUrl: metadata.icons[0]
                    })
                ]
            });

            reconnect(config);

            const modal = createWeb3Modal({
                wagmiConfig: config,
                projectId,
                enableOnramp: enableOnramp,
                enableAnalytics: enableAnalytics,
                disableAppend: true,
            });

            // Store the configuration and modal globally
            _appKitConfig = {
                config: config,
                modal: modal,
                wagmiCore: WagmiCore
            };


            // Insert the container into the DOM at the canvas's original position
            const canvas = document.getElementsByTagName('canvas')[0];
            const container = document.createElement('div');
            container.id = 'canvas-container';
            canvas.parentNode.insertBefore(container, canvas);
            container.appendChild(canvas);

            const appkit = document.createElement('w3m-modal')
            container.appendChild(appkit)

            // Add styles to enable fullscreen compatibility
            const addCanvasActiveStyles = () => {
                const styleElement = document.createElement('style');
                styleElement.id = 'canvas-active-styles';
                styleElement.innerHTML = `
                .canvas-active {
                    position: fixed !important;
                    top: 0 !important;
                    right: 0 !important;
                    bottom: 0 !important;
                    left: 0 !important;
                    width: 100% !important;
                    height: 100% !important;
                }
            `;
                document.head.appendChild(styleElement);
            };

            const removeCanvasActiveStyles = () => {
                const styleElement = document.getElementById('canvas-active-styles');
                if (styleElement) {
                    document.head.removeChild(styleElement);
                }
            };

            // Handle fullscreen changes
            container.addEventListener('fullscreenchange', () => {
                const canvas = document.querySelector('canvas');
                if (document.fullscreenElement) {
                    if (!canvas.classList.contains('canvas-active')) {
                        addCanvasActiveStyles();
                        canvas.classList.add('canvas-active');
                    }
                } else {
                    if (canvas.classList.contains('canvas-active')) {
                        canvas.classList.remove('canvas-active');
                        removeCanvasActiveStyles();
                    }
                }
            });

            {{{makeDynCall('v', 'callbackPtr')}}}();
        });
    },

    ModalCall__deps: ['$ExecuteCall'],
    ModalCall: async function (id, methodNameStrPtr, parameterStrPtr, callbackPtr) {
        const callFn = async (appKitConfig, methodName, parameterObj) => {
            return await appKitConfig.modal[methodName](parameterObj);
        };
        await ExecuteCall(callFn, id, methodNameStrPtr, parameterStrPtr, callbackPtr);
    },

    WagmiCall__deps: ['$ExecuteCall'],
    WagmiCall: async function (id, methodNameStrPtr, parameterStrPtr, callbackPtr) {
        const callFn = async (appKitConfig, methodName, parameterObj) => {
            return await appKitConfig.wagmiCore[methodName](appKitConfig.config, parameterObj);
        };
        await ExecuteCall(callFn, id, methodNameStrPtr, parameterStrPtr, callbackPtr);
    },

    WagmiWatchAccount__deps: ['$SerializeJson'],
    WagmiWatchAccount: function (callbackPtr) {
        _appKitConfig.wagmiCore.watchAccount(_appKitConfig.config, {
            onChange(data) {
                const dataStr = stringToNewUTF8(SerializeJson(data));
                {{{makeDynCall('vi', 'callbackPtr')}}}(dataStr);
                _free(dataStr);
            }
        });
    },

    WagmiWatchChainId__deps: ['$SerializeJson'],
    WagmiWatchChainId: function (callbackPtr) {
        _appKitConfig.wagmiCore.watchChainId(_appKitConfig.config, {
            onChange(data) {
                {{{makeDynCall('vi', 'callbackPtr')}}}(data);
                _free(dataStr);
            }
        });
    },

    ModalSubscribeState__deps: ['$SerializeJson'],
    ModalSubscribeState: function (callbackPtr) {
        _appKitConfig.modal.subscribeState(newState => {
            const newStateStr = stringToNewUTF8(SerializeJson(newState));
            {{{makeDynCall('vi', 'callbackPtr')}}}(newStateStr);
            _free(newStateStr);
        });
    },
});
