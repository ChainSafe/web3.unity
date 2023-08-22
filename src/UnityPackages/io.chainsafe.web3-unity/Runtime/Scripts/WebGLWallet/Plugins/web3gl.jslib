mergeInto(LibraryManager.library, {

  JS_signMessage: function (message) {
    window.web3gl.signMessage(UTF8ToString(message));
  },

  JS_getSignMessageResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.signMessageResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.signMessageResponse, buffer, bufferSize);
    return buffer;
  },

  JS_resetSignMessageResponse: function () {
      window.web3gl.signMessageResponse = "";
  },

  JS_signTypedMessage: function (domain, types, value) {
    window.web3gl.signTypedMessage(
      UTF8ToString(domain),
      UTF8ToString(types),
      UTF8ToString(value)
    );
  },

  JS_getSignTypedMessageResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.signTypedMessageResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.signTypedMessageResponse, buffer, bufferSize);
    return buffer;
  },

  JS_resetSignTypedMessageResponse: function () {
      window.web3gl.signTypedMessageResponse = "";
  },

  JS_sendTransaction: function (to, value, gasLimit, gasPrice) {
    window.web3gl.sendTransaction(
        UTF8ToString(to),
        UTF8ToString(value),
        UTF8ToString(gasLimit),
        UTF8ToString(gasPrice)
    );
  },

  JS_getSendTransactionResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.sendTransactionResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.sendTransactionResponse, buffer, bufferSize);
    return buffer;
  },

  JS_resetSendTransactionResponse: function () {
    window.web3gl.sendTransactionResponse = "";
  },

  JS_sendTransactionData: function (to, value, gasLimit, gasPrice, data) {
    window.web3gl.sendTransactionData(
        UTF8ToString(to),
        UTF8ToString(value),
        UTF8ToString(gasLimit),
        UTF8ToString(gasPrice),
        UTF8ToString(data)
    );
  },

  JS_getSendTransactionResponseData: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.sendTransactionResponseData) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.sendTransactionResponseData, buffer, bufferSize);
    return buffer;
  },

  JS_resetSendTransactionResponseData: function () {
    window.web3gl.sendTransactionResponseData = "";
  },

  JS_web3Connect: function () {
    window.web3gl.connect();
  },

  JS_getConnectAccount: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.connectAccount) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.connectAccount, buffer, bufferSize);
    return buffer;
  },

  JS_resetConnectAccount: function () {
    window.web3gl.connectAccount = "";
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

  AddTokenFunction: function(_tokenAddress, _tokenSymbol, _tokenDecimals, _tokenImage){
    window.web3gl.addTokenFunction(
        UTF8ToString(_tokenAddress),
        UTF8ToString( _tokenSymbol),
        UTF8ToString(_tokenDecimals),
        UTF8ToString(_tokenImage),
    );
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

  HashMessage: function (message) {
    window.web3gl.sha3Message(UTF8ToString(message));
  },

  HashMessageResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.hashMessageResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.hashMessageResponse, buffer, bufferSize);
    return buffer;
  },

  SetHashMessageResponse: function (value) {
    window.web3gl.hashMessageResponse = value;
  },

  GetNetwork: function () {
    return window.web3gl.networkId;
  }
});
