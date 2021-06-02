# Documentation

Bridging Unity games to the blockchain.

## Introduction

[![Watch the video](https://user-images.githubusercontent.com/19412160/103559601-66aa0480-4e84-11eb-803a-6f854e93640a.png)](https://youtu.be/ry97OIwP8Zk)

## Login

There are two Login Scenes. One for webgl and the other for stand alone (iOS, Android, Standalone)

### iOS, Android, Standalone Login

[![](https://user-images.githubusercontent.com/19412160/109343259-0d829f80-783b-11eb-9324-9962cd2b0c8c.png)](https://www.youtube.com/watch?v=PSydAY9ps-I)

### WebGL Login

[![](https://user-images.githubusercontent.com/19412160/112086705-17aa6c00-8b63-11eb-80b5-ef75801d4e21.png)](https://www.youtube.com/watch?v=4KpftfYcaEY)

### \_Config

After Login Scene `_Config.cs` will store user info. This can be accessed in any scene.

```c#
string account = _Config.Account;
print(account);
```

## WebGL Build

[![](https://user-images.githubusercontent.com/19412160/118180075-1fb8b680-b404-11eb-99cd-486eae19fe0d.png)](https://www.youtube.com/watch?v=WkWYPuFHM5k)

### Call through WebGL

Call will execute a smart contract method without altering the smart contract state.

```c#
// smart contract method to call
string method = "x";
// abi in json format
string abi = "[ { \"inputs\": [], \"name\": \"increment\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"x\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
// address of contract
string contract = "0xB6B8bB1e16A6F73f7078108538979336B9B7341C";
// array of arguments for contract
string args = "[]";

// connects to user's browser wallet to call a transaction
string response = await Web3GL.Call(method, abi, contract, args);
```

### Send through WebGL

Send will execute a smart contract method, altering the smart contract state.

```c#
// smart contract method to call
string method = "increment";
// abi in json format
string abi = "[ { \"inputs\": [], \"name\": \"increment\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"x\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
// address of contract
string contract = "0xB6B8bB1e16A6F73f7078108538979336B9B7341C";
// array of arguments for contract
string args = "[]";
// value in wei
string value = "0";

// connects to user's browser wallet (metamask) to send a transaction
Web3GL.Send(method, abi, contract, args, value);
```

## Ethereum

### Balance Of

```c#
string network = "mainnet"; // mainnet ropsten kovan rinkeby goerli
string account = "0x99C85bb64564D9eF9A99621301f22C9993Cb89E3";

BigInteger wei = await Ethereum.BalanceOf(network, account);
print("wei: " + wei);
```

### Create Transaction

Create an unsigned transaction. User can sign offline.

```c#
string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
string from = "0x35706484aB20Cbf22F5c7a375D5764DA8166aE1c";
string to = "0x7E3bE66431ba73956213C40C0828355D1A7894D3";
string eth = "0.00111";

Transaction transaction = await Ethereum.CreateTransaction(network, from, to, eth);

print("network: " + transaction.network);
print("to: " + transaction.to);
print("wei: " + transaction.wei);
print("nonce: " + transaction.nonce);
print("gasLimit: " + transaction.gasLimit);
print("gasPrice: " + transaction.gasPrice);
print("hex: " + transaction.hex);
```

### Broadcast Transaction

Broadcast a signed transaction.

```c#
string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
string signedTransaction = "0xf86b04843b9aca0083989680947e3be66431ba73956213c40c0828355d1a7894d38703f18a03b36000802ca0043ab6289f2a44dd911bfb3658cfac12710354a3e0cef35544c9348b15f9f6f7a018d36b8d5b61dc00a54293528d0edd8a4a7c9f064817825c7e8cb8167b240860";

string transactionHash = await Ethereum.Broadcast(network, signedTransaction);
print (transactionHash);
```

### Verify

Verify a signed message.

```c#
string network = "mainnet"; // mainnet ropsten kovan rinkeby goerli
string message = "YOUR_MESSAGE";
string signature = "0x94bdbebbd0180195b89721a55c3a436a194358c9b3c4eafd22484085563ff55e49a4552904266a5b56662b280757f6aad3b2ab91509daceef4e5b3016afd34781b";

string account = await Ethereum.Verify(network, message, signature);
print (account); // 0xC52C1FB0B9681e1c80e5AdA8eEeD992C0C2706eE
```

## ERC1155

ERC-1155 contracts have two additonal fields.

- Contract address: The address that stores the balances of the token.

- Token Id: The specific token inside of the contract address.

For example. The contract address `0x5e30b1d6f920364c847512e2528efdadf72a97a9` can store all digital chess pieces. A pawn can be token id `17`.

### Balance Of

```c#
string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
string contract = "0x2ebecabbbe8a8c629b99ab23ed154d74cd5d4342";
string account = "0xaca9b6d9b1636d99156bb12825c75de1e5a58870";
string tokenId = "22";

BigInteger balance = await ERC1155.BalanceOf(network, contract, account, tokenId);

print (balance);
```

### Balance Of Batch

Balance of batch will get the balance of a list of accounts and token ids. For example:

Get the balance of account `0xaCA9B6D9B1636D99156bB12825c75De1E5a58870` with token id `17` and

balance of account `0xaCA9B6D9B1636D99156bB12825c75De1E5a58870` with token id `22`

```c#
string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
string contract = "0x2ebecabbbe8a8c629b99ab23ed154d74cd5d4342";
string[] accounts ={ "0xaCA9B6D9B1636D99156bB12825c75De1E5a58870", "0xaCA9B6D9B1636D99156bB12825c75De1E5a58870" };
string[] tokenIds = { "17", "22" };

List<BigInteger> batchBalances = await ERC1155.BalanceOfBatch(network, contract, accounts, tokenIds);

foreach (var balance in batchBalances)
{
  print ("batchBalance:" + balance);
}
```

### URI

Returns meta data about the token.

```c#
string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
string contract = "0x2ebecabbbe8a8c629b99ab23ed154d74cd5d4342";
string tokenId = "17";

string uri = await ERC1155.URI(network, contract, tokenId);

print (uri);
```

### Is Approved For All

Queries the approval status of an operator for a given owner

```c#
string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
string contract = "0x2ebecabbbe8a8c629b99ab23ed154d74cd5d4342";
string account = "0xaca9b6d9b1636d99156bb12825c75de1e5a58870";
string authorizedOperator = "0x3482549fca7511267c9ef7089507c0f16ea1dcc1";

bool isApproved = await ERC1155.IsApprovedForAll(network, contract, account, authorizedOperator);

print (isApproved);
```

## ERC721

### Balance Of

Counts all NFTs assigned to an owner

```c#
string network = "mainnet"; // mainnet ropsten kovan rinkeby goerli
string contract = "0x60f80121c31a0d46b5279700f9df786054aa5ee5";
string account = "0x6b2be2106a7e883f282e2ea8e203f516ec5b77f7";

BigInteger balance = await ERC721.BalanceOf(network, contract, account);

print (balance);
```

### Owner Of

Find the owner of a NFT

```c#
string network = "mainnet"; // mainnet ropsten kovan rinkeby goerli
string contract = "0xf5b0a3efb8e8e4c201e2a935f110eaaf3ffecb8d";
string tokenId = "721";

string account = await ERC721.OwnerOf(network, contract, tokenId);

print (account);
```

### URI

```c#
string network = "mainnet"; // mainnet ropsten kovan rinkeby goerli
string contract = "0xf5b0a3efb8e8e4c201e2a935f110eaaf3ffecb8d";
string tokenId = "721";

string uri = await ERC721.TokenURI(network, contract, tokenId);

print (uri);
```

## ERC20

### Balance Of

```c#
string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
string contract = "0xc778417e063141139fce010982780140aa0cd5ab";
string account = "0xaCA9B6D9B1636D99156bB12825c75De1E5a58870";

BigInteger balance = await ERC20.BalanceOf(network, contract, account);

print (balance);
```

### Name

Returns the name of the token. E.g. "Wrapped Ether"

```c#
string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
string contract = "0xc778417E063141139Fce010982780140Aa0cD5Ab";

string name = await ERC20.Name(network, contract);

print (name);
```

### Symbol

Returns the symbol of the token. E.g. “WETH”.

```c#
string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
string contract = "0xc778417E063141139Fce010982780140Aa0cD5Ab";

string symbol = await ERC20.Symbol(network, contract);

print (symbol);
```

### Decimals

Returns the number of decimals the token uses - e.g. 8, means to divide the token amount by 100000000 to get its user representation.

```c#
string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
string contract = "0xc778417E063141139Fce010982780140Aa0cD5Ab";

BigInteger decimals = await ERC20.Decimals(network, contract);

print (decimals);
```

### Total Supply

Returns the total token supply.

```c#
string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
string contract = "0xc778417e063141139fce010982780140aa0cd5ab";

BigInteger totalSupply = await ERC20.TotalSupply(network, contract);

print (totalSupply);
```

## Polygon

### Balance Of

```c#
string network = "mainnet"; // mainnet testnet
string account = "0x99C85bb64564D9eF9A99621301f22C9993Cb89E3";

BigInteger balance = await Polygon.BalanceOf(network, account);

print(balance);
```

### Verify

Verify a signed message.

```c#
string network = "mainnet"; // mainnet testnet
string message = "YOUR_MESSAGE";
string signature = "0x94bdbebbd0180195b89721a55c3a436a194358c9b3c4eafd22484085563ff55e49a4552904266a5b56662b280757f6aad3b2ab91509daceef4e5b3016afd34781b";

string account = await Polygon.Verify(network, message, signature);

print (account);
```

## Polygon1155

### Balance Of

```c#
string network = "mainnet"; // mainnet testnet
string contract = "0xfd1dBD4114550A867cA46049C346B6cD452ec919";
string account = "0x451bb3B5B93EE605daFA2D572AC170E9990eb8E4";
string tokenId = "141";

BigInteger balance = await Polygon1155.BalanceOf(network, contract, account, tokenId);

print (balance);
```

### Balance Of Batch

```c#
string network = "mainnet"; // mainnet testnet
string contract = "0xfd1dBD4114550A867cA46049C346B6cD452ec919";
string[] accounts =
{
  "0x451bb3B5B93EE605daFA2D572AC170E9990eb8E4",
  "0x451bb3B5B93EE605daFA2D572AC170E9990eb8E4"
};
string[] tokenIds = { "141", "142" };

List<BigInteger> batchBalances = await Polygon1155.BalanceOfBatch(network, contract, accounts, tokenIds);

foreach (var balance in batchBalances)
{
  print ("batchBalance:" + balance);
}
```

### URI

Returns meta data about the token.

```c#
string network = "mainnet"; // mainnet testnet
string contract = "0xfd1dBD4114550A867cA46049C346B6cD452ec919";
string tokenId = "141";

string uri = await Polygon1155.URI(network, contract, tokenId);

print (uri);
```

### Is Approved For All

Queries the approval status of an operator for a given owner

```c#
string network = "mainnet"; // mainnet testnet
string contract = "0xfd1dBD4114550A867cA46049C346B6cD452ec919";
string account = "0x72b8Df71072E38E8548F9565A322B04b9C752932";
string authorizedOperator = "0x35706484aB20Cbf22F5c7a375D5764DA8166aE1c";

bool isApproved = await Polygon1155.IsApprovedForAll(network, contract, account, authorizedOperator);

print (isApproved);
```

## Polygon721

### Balance Of

Counts all NFTs assigned to an owner

```c#
string network = "mainnet"; // mainnet testnet
string contract = "0xbCCaa7ACb552A2c7eb27C7eb77c2CC99580735b9";
string account = "0x8861399ee37626fcc020c49e5184d9b839ed854a";

BigInteger balance = await Polygon721.BalanceOf(network, contract, account);

print (balance);
```

### Owner Of

Find the owner of a NFT

```c#
string network = "mainnet"; // mainnet testnet
string contract = "0xbCCaa7ACb552A2c7eb27C7eb77c2CC99580735b9";
string tokenId = "965";

string account = await Polygon721.OwnerOf(network, contract, tokenId);

print (account);
```

### URI

```c#
string network = "mainnet"; // mainnet testnet
string contract = "0xbCCaa7ACb552A2c7eb27C7eb77c2CC99580735b9";
string tokenId = "965";

string uri = await Polygon721.TokenURI(network, contract, tokenId);

print (uri);
```

## Binance Smart Chain

### Balance Of

```c#
string network = "testnet"; // mainnet testnet
string account = "0x451bb3B5B93EE605daFA2D572AC170E9990eb8E4";

BigInteger balance = await Binance.BalanceOf(network, account);

print(balance);
```

### Verify

```c#
string network = "mainnet"; // mainnet testnet
string message = "YOUR_MESSAGE";
string signature = "0x94bdbebbd0180195b89721a55c3a436a194358c9b3c4eafd22484085563ff55e49a4552904266a5b56662b280757f6aad3b2ab91509daceef4e5b3016afd34781b";

string account = await Binance.Verify(network, message, signature);

print (account);
```

## BEP1155

Binance Smart Chain that extends ERC-1155

### Balance Of

```c#
string network = "mainnet"; // mainnet testnet
string contract = "0x3E31F70912c00AEa971A8b2045bd568D738C31Dc";
string account = "0xe91e3b8b25f41b215645813a33e39edf42ba25cf";
string tokenId = "770";

BigInteger balance = await BEP1155.BalanceOf(network, contract, account, tokenId);

print (balance);
```

### Balance Of Batch

```c#
string network = "mainnet"; // mainnet testnet
string contract = "0x3E31F70912c00AEa971A8b2045bd568D738C31Dc";
string[] accounts =
{
  "0xe91e3b8b25f41b215645813a33e39edf42ba25cf",
  "0xe91e3b8b25f41b215645813a33e39edf42ba25cf"
};
string[] tokenIds = { "770", "771" };

List<BigInteger> batchBalances = await BEP1155.BalanceOfBatch(network, contract, accounts, tokenIds);

foreach (var balance in batchBalances)
{
  print ("batchBalance:" + balance);
}
```

### URI

Returns meta data about the token.

```c#
string network = "mainnet"; // mainnet testnet
string contract = "0x3E31F70912c00AEa971A8b2045bd568D738C31Dc";
string tokenId = "770";

string uri = await BEP1155.URI(network, contract, tokenId);

print (uri);
```

### Is Approved For All

Queries the approval status of an operator for a given owner

```c#
string network = "mainnet"; // mainnet testnet
string contract = "0x3E31F70912c00AEa971A8b2045bd568D738C31Dc";
string account = "0xe91e3b8b25f41b215645813a33e39edf42ba25cf";
string authorizedOperator = "0x35706484aB20Cbf22F5c7a375D5764DA8166aE1c";

bool isApproved = await BEP1155.IsApprovedForAll(network, contract, account, authorizedOperator);

print (isApproved);
```

## BEP721

Binance Smart Chain that extends ERC-721

### Balance Of

Counts all NFTs assigned to an owner

```c#
string network = "mainnet"; // mainnet testnet
string contract = "0x3e855B7941fE8ef5F07DAd68C5140D6a3EC1b286";
string account = "0xf81035dd3945ee53f5862833844b69df339c7db4";

BigInteger balance = await BEP721.BalanceOf(network, contract, account);

print (balance);
```

### Owner Of

Find the owner of a NFT

```c#
string network = "mainnet"; // mainnet testnet
string contract = "0x3e855B7941fE8ef5F07DAd68C5140D6a3EC1b286";
string tokenId = "1008";

string account = await BEP721.OwnerOf(network, contract, tokenId);

print (account);
```

### URI

```c#
string network = "mainnet"; // mainnet testnet
string contract = "0x3e855B7941fE8ef5F07DAd68C5140D6a3EC1b286";
string account = "0xf81035dd3945ee53f5862833844b69df339c7db4";

BigInteger balance = await BEP721.BalanceOf(network, contract, account);

print (balance);
```

## BEP20

Binance Smart Chain that extends ERC-20

### Balance Of

```c#
string network = "testnet"; // mainnet testnet
string contract = "0xb6b8bb1e16a6f73f7078108538979336b9b7341c";
string account = "0x451bb3B5B93EE605daFA2D572AC170E9990eb8E4";

BigInteger balance = await BEP20.BalanceOf(network, contract, account);

print (balance);
```

### Name

Returns the name of the token. E.g. "Wrapped Ether"

```c#
string network = "testnet"; // mainnet testnet
string contract = "0xb6b8bb1e16a6f73f7078108538979336b9b7341c";

string name = await BEP20.Name(network, contract);

print (name);
```

### Symbol

Returns the symbol of the token. E.g. “WETH”.

```c#
string network = "testnet"; // mainnet testnet
string contract = "0xb6b8bb1e16a6f73f7078108538979336b9b7341c";

string symbol = await BEP20.Symbol(network, contract);

print (symbol);
```

### Decimals

Returns the number of decimals the token uses - e.g. 8, means to divide the token amount by 100000000 to get its user representation.

```c#
string network = "testnet"; // mainnet testnet
string contract = "0xb6b8bb1e16a6f73f7078108538979336b9b7341c";

BigInteger decimals = await BEP20.Decimals(network, contract);

print (decimals);
```

### Total Supply

Returns the total token supply.

```c#
string network = "testnet"; // mainnet testnet
string contract = "0xb6b8bb1e16a6f73f7078108538979336b9b7341c";

BigInteger totalSupply = await BEP20.TotalSupply(network, contract);

print (totalSupply);
```

## xDai

The first-ever USD stable blockchain and multi-chain staking token

### Balance Of

```c#
string account = "0x577b17c9A02B7A360e0cf945D623D6C1ace6074c";

BigInteger balance = await XDai.BalanceOf(account);

print(balance);
```

### Verify

```c#
string message = "YOUR_MESSAGE";
string signature = "0x94bdbebbd0180195b89721a55c3a436a194358c9b3c4eafd22484085563ff55e49a4552904266a5b56662b280757f6aad3b2ab91509daceef4e5b3016afd34781b";

string account = await XDai.Verify(message, signature);

print (account);
```

## xDai1155

### Balance Of

```c#
string contract = "0x93d0c9a35c43f6BC999416A06aaDF21E68B29EBA";
string account = "0xa63641e81D223F01d11343C67b77CB4f092acd5a";
string tokenId = "1344";

BigInteger balance = await XDai1155.BalanceOf(contract, account, tokenId);

print (balance);
```

### Balance Of Batch

```c#
string contract = "0x93d0c9a35c43f6BC999416A06aaDF21E68B29EBA";
string[] accounts =
{
  "0xa63641e81D223F01d11343C67b77CB4f092acd5a",
  "0xa63641e81D223F01d11343C67b77CB4f092acd5a"
};
string[] tokenIds = { "1344", "1345" };

List<BigInteger> batchBalances = await XDai1155.BalanceOfBatch(contract, accounts, tokenIds);

foreach (var balance in batchBalances)
{
  print ("batchBalance:" + balance);
}
```

### URI

Returns meta data about the token.

```c#
string contract = "0x93d0c9a35c43f6BC999416A06aaDF21E68B29EBA";
string tokenId = "1344";

string uri = await XDai1155.URI(contract, tokenId);

print (uri);
```

### Is Approved For All

Queries the approval status of an operator for a given owner

```c#
string contract = "0x93d0c9a35c43f6BC999416A06aaDF21E68B29EBA";
string account = "0xa63641e81D223F01d11343C67b77CB4f092acd5a";
string authorizedOperator = "0x35706484aB20Cbf22F5c7a375D5764DA8166aE1c";

bool isApproved = await XDai1155.IsApprovedForAll(contract, account, authorizedOperator);

print (isApproved);
```

## xDai721

### Balance Of

Counts all NFTs assigned to an owner

```c#
string contract = "0x90FdA259CFbdB74F1804e921F523e660bfBE698d";
string account = "0x525C18aB76A28C367c876BBDFaa16Bb96865F9fE";

BigInteger balance = await XDai721.BalanceOf(contract, account);

print (balance);
```

### Owner Of

Find the owner of a NFT

```c#
string contract = "0x90FdA259CFbdB74F1804e921F523e660bfBE698d";
string tokenId = "1582";

string account = await XDai721.OwnerOf(contract, tokenId);

print (account);
```

### URI

```c#
string contract = "0x90FdA259CFbdB74F1804e921F523e660bfBE698d";
string tokenId = "1582";

string uri = await XDai721.TokenURI(contract, tokenId);

print (uri);
```

## xDai20

### Balance Of

```c#
string contract = "0xa106739de31fa7a9df4a93c9bea3e1bade0924e2";
string account = "0x000000ea89990a17Ec07a35Ac2BBb02214C50152";

BigInteger balance = await XDai20.BalanceOf(contract, account);

print (balance);
```

### Name

Returns the name of the token. E.g. "Wrapped Ether"

```c#
string contract = "0xa106739de31fa7a9df4a93c9bea3e1bade0924e2";

string name = await XDai20.Name(contract);

print (name);
```

### Symbol

Returns the symbol of the token. E.g. “WETH”.

```c#
string contract = "0xa106739de31fa7a9df4a93c9bea3e1bade0924e2";

string symbol = await XDai20.Symbol(contract);

print (symbol);
```

### Decimals

Returns the number of decimals the token uses - e.g. 8, means to divide the token amount by 100000000 to get its user representation.

```c#
string contract = "0xa106739de31fa7a9df4a93c9bea3e1bade0924e2";

BigInteger decimals = await XDai20.Decimals(contract);

print (decimals);
```

### Total Supply

Returns the total token supply.

```c#
string contract = "0xa106739de31fa7a9df4a93c9bea3e1bade0924e2";

BigInteger totalSupply = await XDai20.TotalSupply(contract);

print (totalSupply);
```
