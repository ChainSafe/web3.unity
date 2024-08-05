mergeInto(LibraryManager.library, {
    Save: function (key, value) {
        window.localStorage.setItem(UTF8ToString(key), UTF8ToString(value));
    },
    Load: function (key) {
        var loadedItem = window.localStorage.getItem(UTF8ToString(key));
        var bufferSize = lengthBytesUTF8(loadedItem) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(loadedItem, buffer, bufferSize);
        return buffer;
    },
    Clear: function (key) {
        window.localStorage.removeItem(UTF8ToString(key));
    }
});