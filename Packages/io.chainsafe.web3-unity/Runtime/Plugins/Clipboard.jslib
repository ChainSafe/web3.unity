var ClipboardManager = {
    $ClipboardManager: {},

    SetPasteCallback: function (callback) {
        console.log("SetPasteCallback");
        ClipboardManager.callback = callback; // Use ClipboardManager instead of this.ClipboardManager
    },

    PasteFromClipboard: function () {
        navigator.clipboard.readText().then(
            clipText => {
                console.log("PasteFromClipboard: " + clipText);
                Module.dynCall_vi(ClipboardManager.callback, stringToNewUTF8(clipText)); // Use ClipboardManager.callback
            }
        ).catch(err => {
            console.error('Failed to read clipboard contents: ', err);
        });
    },

    CopyToClipboard: function (text) {
        var str = UTF8ToString(text);
        navigator.clipboard.writeText(str).then(function () {
            console.log('Text copied to clipboard: ' + str);
        }).catch(function (error) {
            console.error('Failed to copy text: ', error);
        });
    },
};

autoAddDeps(ClipboardManager, '$ClipboardManager');
mergeInto(LibraryManager.library, ClipboardManager);
