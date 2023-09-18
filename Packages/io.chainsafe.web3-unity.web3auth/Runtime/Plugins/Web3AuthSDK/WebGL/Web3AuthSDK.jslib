mergeInto(LibraryManager.library, {
  GetCurrentURL: function() {
    var url = window.location.href;

    var bufferSize = lengthBytesUTF8(url) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(url, buffer, bufferSize);
    return buffer;
  },

  OpenURL: function (url) {
    url = UTF8ToString(url);
    window.location.href = url;
  },

  RemoveAuthCodeFromURL: function() {
    window.history.replaceState(null, null, ' ');
  },

});