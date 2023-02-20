// load network.js to get network/chain id
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "./network.js" }));
// load web3modal to connect to wallet
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "./web3/lib/web3modal.js" }));
// load web3js to create transactions
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "./web3/lib/web3.min.js" }));
// uncomment to enable torus wallet
// document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "https://unpkg.com/@toruslabs/torus-embed" }));
// uncomment to enable walletconnect
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "https://unpkg.com/@walletconnect/web3-provider@1.2.1/dist/umd/index.min.js" }));

// load web3gl to connect to unity
window.web3gl = {
  networkId: 0,
  connect,
  connectAccount: "",
  clearResponse,
  sendAsync,
  sendAsyncResponse: "",
  sendAsyncError: "",
};

// will be defined after connect()
let provider;
let web3;

/*
paste this in inspector to connect to wallet:
window.web3gl.connect()
*/
async function connect() {
  // uncomment to enable torus
  const providerOptions = {
    // torus: {
    //   package: Torus,
    // },
  };
  
  if (window.web3InfuraId !== undefined && window.web3InfuraId !== "" &&  window.web3InfuraId !== "00000000000000000000000000000000") {
    providerOptions.walletconnect = {
      package: window.WalletConnectProvider.default,
      options: {
        infuraId: window.web3InfuraId
      },
    }
  }

  const web3Modal = new window.Web3Modal.default({
    providerOptions,
  });

  web3Modal.clearCachedProvider();

  // set provider
  provider = await web3Modal.connect();
  web3 = new Web3(provider);

  // set current account
  // provider.selectedAddress works for metamask and torus
  // provider.accounts[0] works for walletconnect
  web3gl.connectAccount = provider.selectedAddress || provider.accounts[0];

  // refresh page if player changes account
  provider.on("accountsChanged", (accounts) => {
    window.location.reload();
  });

  // update if player changes network
  provider.on("chainChanged", (chainId) => {
    web3gl.networkId = parseInt(chainId);
  });
}

function clearResponse() {
  window.web3gl.sendAsyncResponse = "";
  window.web3gl.sendAsyncError = "";
}

async function sendAsync(method, params) {
  web3.currentProvider.sendAsync(
    {
      jsonrpc: "2.0",
      method: method,
      params: JSON.parse(params),
      id: new Date().getTime(),
    },
    async (error, result) => {
      if (error) {
        window.web3gl.sendAsyncError = JSON.stringify(error);
      } else {
        window.web3gl.sendAsyncResponse = JSON.stringify(result.result);
      }
    }
  );
}
