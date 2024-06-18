#import <UIKit/UIKit.h>

extern "C" {
    void CopyToClipboard(const char* text) {
        NSString *textToCopy = [NSString stringWithUTF8String:text];
        UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
        pasteboard.string = textToCopy;
    }

    const char* PasteFromClipboard() {
        UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
        NSString *pastedText = pasteboard.string;
        return pastedText.UTF8String;
    }
}