mergeInto(LibraryManager.library, {
    Save: function (key, value) {
        window.localStorage.setItem(UTF8ToString(key), UTF8ToString(value));
    },
    Load: function (key) {
        var loadedItem = window.localStorage.getItem(UTF8ToString(key));
        var json = JSON.stringify(loadedItem);
        var bufferSize = lengthBytesUTF8(json) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(json, buffer, bufferSize);
        return buffer;
    },
    Clear: function (key) {
        window.localStorage.removeItem(UTF8ToString(key));
    }
});