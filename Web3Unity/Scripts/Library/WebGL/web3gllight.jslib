mergeInto(LibraryManager.library, {
  /*Web3Connect: function () {
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
  },*/
  
  SendAsyncJs: function (method, parameters) {
    window.web3gl.sendAsync(
      UTF8ToString(method),
      UTF8ToString(parameters)
    );
  },
  
  SetSendAsyncResponse: function (value) {
    window.web3gl.sendAsyncResponse = value;
  },
  
  SendAsyncResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.sendAsyncResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.sendAsyncResponse, buffer, bufferSize);
    return buffer;
  },
  
  SetSendAsyncError: function (value) {
    window.web3gl.sendAsyncError = value;
  },
  
  SendAsyncError: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.sendAsyncError) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.sendAsyncError, buffer, bufferSize);
    return buffer;
  }
});
