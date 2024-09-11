#import <UIKit/UIKit.h>

// Define a function pointer for the callback
static void (*clipboardCallback)(const char* text) = NULL;

// Set the callback function that will be called when pasting from clipboard
void SetPasteCallback(void (*callback)(const char* text)) {
    clipboardCallback = callback;
}

// Copy text to the clipboard
void CopyToClipboard(const char* text) {
    NSString *textToCopy = [NSString stringWithUTF8String:text];
    UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
    pasteboard.string = textToCopy;
}

// Paste from the clipboard and call the callback
void PasteFromClipboard() {
    UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
    NSString *pastedText = pasteboard.string;

    // Call the callback function if it is set
    if (clipboardCallback != NULL) {
        clipboardCallback([pastedText UTF8String]);
    }
}
