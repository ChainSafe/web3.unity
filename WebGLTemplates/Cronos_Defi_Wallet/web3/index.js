// load network.js to get network/chain id
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "./network.js" }));
// load web3modal to connect to wallet
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "./web3/lib/web3modal.js" }));
// load web3js to create transactions
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "./web3/lib/web3.min.js" }));
// uncomment to enable torus wallet
// document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "https://unpkg.com/@toruslabs/torus-embed" }));
// uncomment to enable walletconnect
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "https://unpkg.com/@walletconnect/web3-provider@1.8.0/dist/umd/index.min.js" }));
// Load defi connect
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "https://unpkg.com/deficonnect@1.1.13/dist/index.umd.js" }));

// load web3gl to connect to unity
window.web3gl = {
  networkId: 0,
  connect,
  connectAccount: "",
  signMessage,
  signMessageResponse: "",
  callContract,
  callContractResponse:"",
  callContractError:"",
  sendTransaction,
  sendTransactionResponse: "",
  sha3Message,
  hashMessageResponse: "",
  sendTransactionResponse: "",
  sendTransactionData,
  sendTransactionResponseData:"",
  sendContract,
  sendContractResponse: "",
};

// will be defined after connect()
let provider;
let web3;

/*
paste this in inspector to connect to wallet:
window.web3gl.connect()
*/
async function connect() {
  // uncomment to enable torus and walletconnect
  const providerOptions = {
    walletconnect: {
       package: window.WalletConnectProvider.default,
       options: {
         infuraId: "31a6f00d65554d31825dfeb9c5c265fe",
       },
    },
    "custom-example": {
      display: {
        logo:
          "data:image/gif;base64,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",
        name: "Defi Wallet",
        description: "Connect to your Defi Wallet account"
      },
      package: window.DeFiConnect.DeFiWeb3Connector,
      options: {
        supportedChainIds: [1],
        rpc: {
          1: "https://mainnet.infura.io/v3/31a6f00d65554d31825dfeb9c5c265fe",
          25: "https://evm.cronos.org/" // cronos mainet
        },
        pollingInterval: 15000
      },
      connector: async (ProviderPackage, options) => {
          const connector = new ProviderPackage(options);
          await connector.activate();
          provider = await connector.getProvider() 
          return provider;
      }
    },
  };

  const web3Modal = new window.Web3Modal.default({
    providerOptions,
  });

  web3Modal.clearCachedProvider();

  // set provider
  provider = await web3Modal.connect();
  web3 = new Web3(provider);

  // set current network id
  web3gl.networkId = parseInt(provider.chainId);

  // if current network id is not equal to network id, then switch
  if (web3gl.networkId != window.web3ChainId) {
    try {
      await window.ethereum.request({
        method: "wallet_switchEthereumChain",
        params: [{ chainId: `0x${window.web3ChainId.toString(16)}` }], // chainId must be in hexadecimal numbers
      });
    } catch {
      // if network isn't added, pop-up metamask to add
      await addEthereumChain();
    }
  }

  // set current account
  web3gl.connectAccount = (await web3.eth.getAccounts())[0]

  // refresh page if player changes account
  provider.on("accountsChanged", (accounts) => {
    window.location.reload();
  });

  // update if player changes network
  provider.on("chainChanged", (chainId) => {
    web3gl.networkId = parseInt(chainId);
  });
}

/*
Will calculate the sha3 of the input.
window.web3gl.sha3Message("hello")
*/
async function sha3Message(message) {
    try {
        const hashedMessage = await web3.utils.sha3(message);
        window.web3gl.hashMessageResponse = hashedMessage;
    } catch (error) {
        window.web3gl.hashMessageResponse = error.message;
    }
}

/*
paste this in inspector to connect to sign message:
window.web3gl.signMessage("hello")
*/
async function signMessage(message) {
  try {
    const from = (await web3.eth.getAccounts())[0];
    const signature = await web3.eth.personal.sign(message, from, "")
      window.web3gl.signMessageResponse = signature;
  } catch (error) {
    window.web3gl.signMessageResponse = error.message;
  }
}

/*
paste this in inspector to send eth:
const to = "0xdD4c825203f97984e7867F11eeCc813A036089D1"
const value = "12300000000000000"
const gasLimit = "21000" // gas limit
const gasPrice = "33333333333"
window.web3gl.sendTransaction(to, value, gasLimit, gasPrice);
*/
async function sendTransaction(to, value, gasLimit, gasPrice) {
  const from = (await web3.eth.getAccounts())[0];
  web3.eth
      .sendTransaction({
        from,
        to,
        value,
        gas: gasLimit ? gasLimit : undefined,
        gasPrice: gasPrice ? gasPrice : undefined,
      })
      .on("transactionHash", (transactionHash) => {
        window.web3gl.sendTransactionResponse = transactionHash;
      })
      .on("error", (error) => {
        window.web3gl.sendTransactionResponse = error.message;
      });
}

/*
paste this in inspector to send eth:
const to = "0x20E7D0C4182149ADBeFE446E82358A2b2D5244e9"
const value = "0"
const gasPrice = "1100000010"
const gasLimit = "228620" // gas limit
const data = "0xd0def521000000000000000000000000d25b827d92b0fd656a1c829933e9b0b836d5c3e20000000000000000000000000000000000000000000000000000000000000040000000000000000000000000000000000000000000000000000000000000002e516d586a576a6a4d55387233395543455a38343833614e6564774e5246524c767656396b7771314770436774686a000000000000000000000000000000000000"
window.web3gl.sendTransactionData(to, value, gasPrice, gasLimit, data);
*/
async function sendTransactionData(to, value, gasPrice, gasLimit, data) {
    const from = (await web3.eth.getAccounts())[0];
    web3.eth
        .sendTransaction({
            from,
            to,
            value,
            gasPrice: gasPrice ? gasPrice : undefined,
            gas: gasLimit ? gasLimit : undefined,
            data: data ? data : undefined,
        })
        .on("transactionHash", (transactionHash) => {
            window.web3gl.sendTransactionResponseData = transactionHash;
        })
        .on("error", (error) => {
            window.web3gl.sendTransactionResponseData = error.message;
        });
}

/*
calls a non-mutable contract method.
const method = "x"
const abi = `[ { "inputs": [], "name": "increment", "outputs": [], "stateMutability": "nonpayable", "type": "function" }, { "inputs": [], "name": "x", "outputs": [ { "internalType": "uint256", "name": "", "type": "uint256" } ], "stateMutability": "view", "type": "function" } ]`;
const contract = "0xB6B8bB1e16A6F73f7078108538979336B9B7341C"
const args = "[]"
window.web3gl.callContract(method, abi, contract, args)
*/
async function callContract(method, abi, contract, args) {
    const from = (await web3.eth.getAccounts())[0];
    new web3.eth.Contract(JSON.parse(abi), contract).methods[method](
        ...JSON.parse(args)
    ).call()
        .then((result) => window.web3gl.callContractResponse = result)
        .catch((error) => window.web3gl.callContractError = error.message);
}

/*
paste this in inspector to connect to interact with contract:
const method = "increment"
const abi = `[ { "inputs": [], "name": "increment", "outputs": [], "stateMutability": "nonpayable", "type": "function" }, { "inputs": [], "name": "x", "outputs": [ { "internalType": "uint256", "name": "", "type": "uint256" } ], "stateMutability": "view", "type": "function" } ]`;
const contract = "0xB6B8bB1e16A6F73f7078108538979336B9B7341C"
const args = "[]"
const value = "0"
const gasLimit = "222222" // gas limit
const gasPrice = "333333333333"
window.web3gl.sendContract(method, abi, contract, args, value, gasLimit, gasPrice)
*/
async function sendContract(method, abi, contract, args, value, gasLimit, gasPrice) {
  const from = (await web3.eth.getAccounts())[0];
  new web3.eth.Contract(JSON.parse(abi), contract).methods[method](...JSON.parse(args))
      .send({
        from,
        value,
        gas: gasLimit ? gasLimit : undefined,
        gasPrice: gasPrice ? gasPrice : undefined,
      })
      .on("transactionHash", (transactionHash) => {
        window.web3gl.sendContractResponse = transactionHash;
      })
      .on("error", (error) => {
        window.web3gl.sendContractResponse = error.message;
      });
}

// add new wallet to in metamask
async function addEthereumChain() {
  const account = (await web3.eth.getAccounts())[0];

  // fetch https://chainid.network/chains.json
  const response = await fetch("https://chainid.network/chains.json");
  const chains = await response.json();

  // find chain with network id
  const chain = chains.find((chain) => chain.chainId == window.web3ChainId);

  const params = {
    chainId: "0x" + chain.chainId.toString(16), // A 0x-prefixed hexadecimal string
    chainName: chain.name,
    nativeCurrency: {
      name: chain.nativeCurrency.name,
      symbol: chain.nativeCurrency.symbol, // 2-6 characters long
      decimals: chain.nativeCurrency.decimals,
    },
    rpcUrls: chain.rpc,
    blockExplorerUrls: [chain.explorers && chain.explorers.length > 0 && chain.explorers[0].url ? chain.explorers[0].url : chain.infoURL],
  };

  await window.ethereum
      .request({
        method: "wallet_addEthereumChain",
        params: [params, account],
      })
      .catch(() => {
        // I give up
        window.location.reload();
      });
}
