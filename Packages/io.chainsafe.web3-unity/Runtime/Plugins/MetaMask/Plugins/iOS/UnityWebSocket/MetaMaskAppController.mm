//
//  UnityWebSocket.m
//  MetaMask.iOS
//
//  Created by Hasan Bayat on 2/20/23.
//

#import <UIKit/UIKit.h>

#import <MetaMask_iOS/WebSocketProvider.h>
#import <MetaMask_iOS/MetaMask_iOS-Swift.h>

#import "UnityAppController.h"

extern bool         _unityAppReady;

@interface MetaMaskAppController : UnityAppController
{
}

- (BOOL)application:(UIApplication *)application
willFinishLaunchingWithOptions:(NSDictionary<UIApplicationLaunchOptionsKey, id> *)launchOptions;
@end

@implementation MetaMaskAppController
static MetaMaskAppController* instance;

+(MetaMaskAppController*)getInstance {
    return instance;
}

- (BOOL)application:(UIApplication *)application
willFinishLaunchingWithOptions:(NSDictionary<UIApplicationLaunchOptionsKey, id> *)launchOptions
{
    return [super application: application willFinishLaunchingWithOptions: launchOptions];
}

@end


IMPL_APP_CONTROLLER_SUBCLASS(MetaMaskAppController)
