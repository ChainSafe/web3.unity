
#import <Foundation/Foundation.h>

#import <UIKit/UIKit.h>

@interface UIApplication(SupressWarnings)

- (void)application:(UIApplication *)application countlydidRegisterUserNotificationSettings:(UIUserNotificationSettings *)notificationSettings;
- (void)application:(UIApplication *)application countlydidRegisterForRemoteNotificationsWithDeviceToken:(NSData *)devToken;
- (void)application:(UIApplication *)application countlydidFailToRegisterForRemoteNotificationsWithError:(NSError *)err;
- (void)application:(UIApplication *)application countlydidReceiveRemoteNotification:(NSDictionary *)userInfo;

- (BOOL)application:(UIApplication *)application countlydidFinishLaunchingWithOptions:(NSDictionary *)launchOptions;
@end
