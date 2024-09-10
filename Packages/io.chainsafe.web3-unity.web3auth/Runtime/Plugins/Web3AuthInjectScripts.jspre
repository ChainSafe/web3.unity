// custom.jspre

Module.InjectExternalScripts = function() {
    const scripts = [
    "https://cdn.jsdelivr.net/npm/@web3auth/no-modal",
    "https://cdn.jsdelivr.net/npm/@web3auth/wallet-services-plugin",
    "https://cdn.jsdelivr.net/npm/@web3auth/openlogin-adapter",
    "https://cdn.jsdelivr.net/npm/@web3auth/ethereum-provider"
    ];

    scripts.forEach(src => {
    const script = document.createElement('script');
    script.src = src;
    document.body.appendChild(script);
    });
};

// Automatically call the function to inject the scripts
Module['onRuntimeInitialized'] = function() {
    Module.InjectExternalScripts();
};
