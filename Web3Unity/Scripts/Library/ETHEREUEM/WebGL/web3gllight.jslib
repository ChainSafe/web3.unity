mergeInto(LibraryManager.library, {
  ClearResponseJs: function () {
    window.web3gl.clearResponse();
  },
  
  SendAsyncJs: function (method, parameters) {
    window.web3gl.sendAsync(
      UTF8ToString(method),
      UTF8ToString(parameters)
    );
  },
  
  SendAsyncResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.sendAsyncResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.sendAsyncResponse, buffer, bufferSize);
    return buffer;
  },
  
  SendAsyncError: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.sendAsyncError) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.sendAsyncError, buffer, bufferSize);
    return buffer;
  }
});
