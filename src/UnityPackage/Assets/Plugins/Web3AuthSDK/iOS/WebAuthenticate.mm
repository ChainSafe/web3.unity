//
//  WebAuthenticate.m
//  Web3AuthUnitySwift
//
//  Created by Mac on 03/08/2022.
//

#import <Foundation/Foundation.h>
#import <AuthenticationServices/ASWebAuthenticationSession.h>
#import <UnityFramework/UnityFramework-Swift.h>
#import <Security/Security.h>

extern "C" {
    void web3auth_launch(const char *url, const char *redirectUri, const char *objectName) {
        [WebAuthenticate launch:[NSString stringWithUTF8String:url] :[NSString stringWithUTF8String:redirectUri] :[NSString stringWithUTF8String:objectName]];
    }

    int web3auth_keystore_set(const char* dataType, const char* value) {
        NSMutableDictionary* attributes = nil;
        NSMutableDictionary* query = [NSMutableDictionary dictionary];
        NSData* sata = [[NSString stringWithCString:value encoding:NSUTF8StringEncoding] dataUsingEncoding:NSUTF8StringEncoding];
    
        [query setObject:(id)kSecClassGenericPassword forKey:(id)kSecClass];
        [query setObject:(id)[NSString stringWithCString:dataType encoding:NSUTF8StringEncoding] forKey:(id)kSecAttrAccount];
    
        OSStatus err = SecItemCopyMatching((CFDictionaryRef)query, NULL);
    
        if (err == noErr) {
            attributes = [NSMutableDictionary dictionary];
            [attributes setObject:sata forKey:(id)kSecValueData];
            [attributes setObject:[NSDate date] forKey:(id)kSecAttrModificationDate];
    
            err = SecItemUpdate((CFDictionaryRef)query, (CFDictionaryRef)attributes);
            return (int)err;
        } else if (err == errSecItemNotFound) {
            attributes = [NSMutableDictionary dictionary];
            [attributes setObject:(id)kSecClassGenericPassword forKey:(id)kSecClass];
            [attributes setObject:(id)[NSString stringWithCString:dataType encoding:NSUTF8StringEncoding] forKey:(id)kSecAttrAccount];
            [attributes setObject:sata forKey:(id)kSecValueData];
            [attributes setObject:[NSDate date] forKey:(id)kSecAttrCreationDate];
            [attributes setObject:[NSDate date] forKey:(id)kSecAttrModificationDate];
            err = SecItemAdd((CFDictionaryRef)attributes, NULL);
            return (int)err;
        } else {
            return (int)err;
        }
    }

    char* web3auth_keystore_get(const char* dataType) {
        NSMutableDictionary* query = [NSMutableDictionary dictionary];
        [query setObject:(id)kSecClassGenericPassword forKey:(id)kSecClass];
        [query setObject:(id)[NSString stringWithCString:dataType encoding:NSUTF8StringEncoding] forKey:(id)kSecAttrAccount];
        [query setObject:(id)kCFBooleanTrue forKey:(id)kSecReturnData];
    
        CFDataRef cfresult = NULL;
        OSStatus err = SecItemCopyMatching((CFDictionaryRef)query, (CFTypeRef*)&cfresult);
    
        if (err == noErr) {
            NSData* passwordData = (__bridge_transfer NSData *)cfresult;
            const char* value = [[[NSString alloc] initWithData:passwordData encoding:NSUTF8StringEncoding] UTF8String];
            char *str = strdup(value);
            return str;
        } else {
            return NULL;
        }
    }

    int web3auth_keystore_delete(const char* dataType) {
        NSMutableDictionary* query = [NSMutableDictionary dictionary];
        [query setObject:(id)kSecClassGenericPassword forKey:(id)kSecClass];
        [query setObject:(id)[NSString stringWithCString:dataType encoding:NSUTF8StringEncoding] forKey:(id)kSecAttrAccount];
    
        OSStatus err = SecItemDelete((CFDictionaryRef)query);
    
        if (err == noErr) {
            return 0;
        } else {
            return (int)err;
        }
    }
}
