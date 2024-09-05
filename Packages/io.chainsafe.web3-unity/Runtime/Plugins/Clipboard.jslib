var ClipboardManager = {
    $ClipboardManager: {},

    SetPasteCallback: function (callback) {
        ClipboardManager.callback = callback; // Use ClipboardManager instead of this.ClipboardManager
    },

    PasteFromClipboard: function () {
        navigator.clipboard.readText().then(
            clipText => {
                Module.dynCall_vi(ClipboardManager.callback, stringToNewUTF8(clipText));
            }
        ).catch(err => {
            console.error('Failed to read clipboard contents: ', err);
        });
    },

    CopyToClipboard: function (text) {
        var str = UTF8ToString(text);
        navigator.clipboard.writeText(str).then(function () {
        }).catch(function (error) {
            console.error('Failed to copy text: ', error);
        });
    },
};

autoAddDeps(ClipboardManager, '$ClipboardManager');
mergeInto(LibraryManager.library, ClipboardManager);
