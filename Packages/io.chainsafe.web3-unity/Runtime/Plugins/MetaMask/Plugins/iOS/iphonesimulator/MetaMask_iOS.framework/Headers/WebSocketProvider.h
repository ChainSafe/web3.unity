//
//  WebSocketProvider.h
//  MetaMask.iOS
//
//  Created by Hasan Bayat on 2/23/23.
//

#ifndef WebSocketProvider_h
#define WebSocketProvider_h

#import "WebSocketProvider.h"

@class WebSocketClient;

#ifdef __cplusplus
extern "C" {
#endif
typedef void (*WebSocketTextMessageReceivedCallback)(int instanceId, const char* _Nonnull message);
typedef void (*WebSocketBinaryMessageReceivedCallback)(int instanceId, uint8_t* _Nonnull message);
typedef void (*WebSocketOpenedCallback)(int instanceId);
typedef void (*WebSocketClosedCallback)(int instanceId, int code);
typedef void (*WebSocketErrorCallback)(int instanceId, const char* _Nonnull error);
#ifdef __cplusplus
}
#endif

@protocol WebSocketProvider

+(id _Nonnull)createWebSocket:(NSString*_Nonnull) url;

@required
@property(nonatomic, readwrite)int instanceId;
@property(nonatomic, strong, nonnull)NSString *url;
@property(nonatomic, readwrite, nullable)WebSocketTextMessageReceivedCallback textMessageReceived;
@property(nonatomic, readwrite, nullable)WebSocketBinaryMessageReceivedCallback binaryMessageReceived;
@property(nonatomic, readwrite, nullable)WebSocketOpenedCallback opened;
@property(nonatomic, readwrite, nullable)WebSocketClosedCallback closed;
@property(nonatomic, readwrite, nullable)WebSocketErrorCallback errorOccured;

-(id _Nonnull)initWithUrl:(nonnull NSString*)url;
-(void)connect;
-(void)disconnect;
-(void)sendText:(nonnull NSString*) text;
-(void)sendBytes:(nonnull NSData*) bytes;

@end

#endif /* WebSocketProvider_h */
