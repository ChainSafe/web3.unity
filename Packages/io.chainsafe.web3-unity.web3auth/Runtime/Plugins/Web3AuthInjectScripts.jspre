// custom.jspre

Module.InjectExternalScripts = function() {
    const scripts = [
    "https://cdn.jsdelivr.net/npm/@web3auth/no-modal@9.1.0",
    "https://cdn.jsdelivr.net/npm/@web3auth/wallet-services-plugin@9.1.0",
    "https://cdn.jsdelivr.net/npm/@web3auth/openlogin-adapter@8.12.4",
    "https://cdn.jsdelivr.net/npm/@web3auth/ethereum-provider@9.0.2"
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
