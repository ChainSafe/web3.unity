# Unity ^2020.1.0 Web3GL Template

## Methods in Browser

![](https://user-images.githubusercontent.com/19412160/149653806-b2c4ca54-1986-4481-bd61-721dbd8a7967.png)

Dispay login modal:

```javascript
window.web3gl.connect();
```

Get Network:

```javascript
window.web3gl.networkId;
```

Get Connected Address:

```javascript
window.web3gl.connectAccount;
```

To Send Transaction:

```javascript
const to = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
const value = "12300000000000000";
const gasLimit = "21000"; // gas limit
const gasPrice = "33333333333";
window.web3gl.sendTransaction(to, value, gasLimit, gasPrice);
```

To Interact with Contract:

```javascript
const method = "increment";
const abi = `[ { "inputs": [], "name": "increment", "outputs": [], "stateMutability": "nonpayable", "type": "function" }, { "inputs": [], "name": "x", "outputs": [ { "internalType": "uint256", "name": "", "type": "uint256" } ], "stateMutability": "view", "type": "function" } ]`;
const contract = "0xB6B8bB1e16A6F73f7078108538979336B9B7341C";
const args = "[]";
const value = "0";
const gasLimit = "222222"; // gas limit
const gasPrice = "333333333333";
window.web3gl.sendContract(method, abi, contract, args, value, gasLimit, gasPrice);
```

## Enable Torus

In `web3/index.js`

Uncomment

```javascript
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "https://unpkg.com/@toruslabs/torus-embed" }));
```

```javascript
  const providerOptions = {
    torus: {
      package: Torus,
    }
  };
```

## Enable WalletConnect

In `web3/index.js`

Uncomment

```javascript
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "https://unpkg.com/@walletconnect/web3-provider@1.2.1/dist/umd/index.min.js" }));
```

```javascript
  const providerOptions = {
    walletconnect: {
      package: window.WalletConnectProvider.default,
      options: {
        infuraId: "00000000000000000000000000000000",
      },
    }
  };
```

Replace `infuraId: "00000000000000000000000000000000"`

![](https://user-images.githubusercontent.com/19412160/149654154-3a9a5066-1c8b-42cd-90f9-204014b56154.png)
