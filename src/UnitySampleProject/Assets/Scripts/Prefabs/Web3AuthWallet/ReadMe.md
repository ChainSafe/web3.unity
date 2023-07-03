# Web3AuthWallet

Web3AuthWallet is a C# library that provides functionality for interacting with web3 wallets and performing transactions on the Ethereum network.

## Usage

### Web3AuthWalletUI

The `Web3AuthWalletUI` class provides a user interface for the web3 wallet functionality. It contains the following methods:

- `OpenButton()`: Opens the wallet and displays the wallet user interface.
- `CloseButton()`: Closes the wallet and hides the wallet user interface.
- `GetData()`: Updates the wallet balances and custom tokens.
- `SetPK()`: Sets the private key for the wallet.
- `AcceptTX()`: Accepts a transaction and attempts to sign or broadcast it.
- `DeclineTx()`: Declines a transaction.
- `SendCustomTokens()`: Sends custom tokens to a specified wallet address.
- `OpenBlockExplorer(number)`: Opens the block explorer for a specified transaction.
- `CopyAddress()`: Copies the wallet address to the clipboard.

### TransactionService

The `TransactionService` class provides functionality for creating, sending, and broadcasting transactions. It contains the following methods:

- `CreateTransaction(txRequest, account, gasPrice, gasLimit, nonce)`: Creates a transaction.
- `SendTransaction(txRequest, signature)`: Sends a transaction.
- `BroadcastTransaction(txRequest, account, signature, gasPrice, gasLimit)`: Broadcasts a transaction.

### SignatureService

The `SignatureService` class provides methods for signing transactions and messages. It contains the following methods:

- `SignTransaction(privateKey, transaction)`: Signs a transaction using the provided private key and transaction data.
- `SignMessage(privateKey, message)`: Signs a message using the provided private key.

## Examples

Here are some examples of how to use the Web3AuthWallet library:

### Open the Wallet and Retrieve Data

```csharp
Web3AuthWalletUI wallet = new Web3AuthWalletUI();
wallet.OpenButton();
wallet.GetData();
```

### Send Custom Tokens
```csharp
Web3AuthWalletUI wallet = new Web3AuthWalletUI();
wallet.SetPK();
wallet.SendCustomTokens();
```

### Create and Send Transaction

```csharp
TransactionService transactionService = new TransactionService();
TransactionRequest txRequest = new TransactionRequest();
// Set transaction details

// Create the transaction
EVM.Response<string> createdTransaction = await transactionService.CreateTransaction(txRequest, account, gasPrice, gasLimit, nonce);

// Sign the transaction
string signature = signatureService.SignTransaction(privateKey, createdTransaction.response);

// Send the transaction
TransactionResponse transactionResponse = await transactionService.SendTransaction(txRequest, signature);
```






