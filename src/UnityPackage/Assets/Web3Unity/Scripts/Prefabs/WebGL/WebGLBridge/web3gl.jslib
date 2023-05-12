mergeInto(LibraryManager.library, {
  Web3Connect: function () {
    window.web3gl.connect();
  },

  ConnectAccount: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.connectAccount) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.connectAccount, buffer, bufferSize);
    return buffer;
  },

  SetConnectAccount: function (value) {
    window.web3gl.connectAccount = value;
  },

  SendContractJs: function (method, abi, contract, args, value, gasLimit, gasPrice) {
    window.web3gl.sendContract(
      UTF8ToString(method),
      UTF8ToString(abi),
      UTF8ToString(contract),
      UTF8ToString(args),
      UTF8ToString(value),
      UTF8ToString(gasLimit),
      UTF8ToString(gasPrice)
    );
  },

    SendContractResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.sendContractResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.sendContractResponse, buffer, bufferSize);
    return buffer;
  },

   EcRecoverJS: function (message,signature) {
    window.web3gl.ecRecover(
     UTF8ToString(message),
     UTF8ToString(signature)
    );
  },

  EcRecoverResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.ecRecoverAddressResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.ecRecoverAddressResponse, buffer, bufferSize);
    return buffer;
  },

  SetContractResponse: function (value) {
    window.web3gl.sendContractResponse = value;
  },

  SendTransactionJs: function (to, value, gasLimit, gasPrice) {
    window.web3gl.sendTransaction(
      UTF8ToString(to),
      UTF8ToString(value),
      UTF8ToString(gasLimit),
      UTF8ToString(gasPrice)
    );
  },

  SendTransactionJsData: function (to, value, gasLimit, gasPrice, data) {
    window.web3gl.sendTransactionData(
      UTF8ToString(to),
      UTF8ToString(value),
      UTF8ToString(gasLimit),
      UTF8ToString(gasPrice),
      UTF8ToString(data)
    );
  },

  SendTransactionResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.sendTransactionResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.sendTransactionResponse, buffer, bufferSize);
    return buffer;
  },

  SetTransactionResponse: function (value) {
    window.web3gl.sendTransactionResponse = value;
  },

  SetTransactionResponseData: function (value) {
    window.web3gl.sendTransactionResponseData = value;
  },

  SignMessage: function (message) {
    window.web3gl.signMessage(UTF8ToString(message));
  },

  HashMessage: function (message) {
    window.web3gl.sha3Message(UTF8ToString(message));
  },

  SignMessageResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.signMessageResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.signMessageResponse, buffer, bufferSize);
    return buffer;
  },

  HashMessageResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.hashMessageResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.hashMessageResponse, buffer, bufferSize);
    return buffer;
  },

  SetSignMessageResponse: function (value) {
    window.web3gl.signMessageResponse = value;
  },

  SetHashMessageResponse: function (value) {
      window.web3gl.hashMessageResponse = value;
  },

  GetNetwork: function () {
    return window.web3gl.networkId;
  }
});
