#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

NSString *ToNSString(char* string) {
    return [NSString stringWithUTF8String:string];
}

bool _CanOpenURL (char* url) {
    return [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:ToNSString(url)]];
}
