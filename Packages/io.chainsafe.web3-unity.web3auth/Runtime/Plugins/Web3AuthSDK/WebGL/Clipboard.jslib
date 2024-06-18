mergeInto(LibraryManager.library, {
    CopyToClipboard: function (text) {
        var str = UTF8ToString(text);
        navigator.clipboard.writeText(str).then(function () {
            console.log('Text copied to clipboard: ' + str);
        }).catch(function (error) {
            console.error('Failed to copy text: ', error);
        });
    },
    
    PasteFromClipboard: function () {
        navigator.clipboard.readText().then(
            clipText => {
                UnityInstance.SendMessage('Web3AuthWalletGUIClipboardManager', 'OnPasteWebGL', clipText);
            }
        ).catch(err => {
            console.error('Failed to read clipboard contents: ', err);
        });
    }
});