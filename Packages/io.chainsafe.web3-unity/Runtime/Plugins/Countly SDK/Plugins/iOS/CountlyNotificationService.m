#import "CountlyNotificationService.h"

#if DEBUG
#define COUNTLY_EXT_LOG(fmt, ...) NSLog([@"%@ " stringByAppendingString:fmt], @"[CountlyNSE]", ##__VA_ARGS__)
#else
#define COUNTLY_EXT_LOG(...)
#endif

NSString* const kCountlyActionIdentifier = @"CountlyActionIdentifier";
NSString* const kCountlyCategoryIdentifier = @"CountlyCategoryIdentifier";

NSString* const kCountlyPNKeyCountlyPayload     = @"c";
NSString* const kCountlyPNKeyNotificationID     = @"i";
NSString* const kCountlyPNKeyButtons            = @"b";
NSString* const kCountlyPNKeyDefaultURL         = @"l";
NSString* const kCountlyPNKeyAttachment         = @"a";
NSString* const kCountlyPNKeyActionButtonIndex  = @"b";
NSString* const kCountlyPNKeyActionButtonTitle  = @"t";
NSString* const kCountlyPNKeyActionButtonURL    = @"l";

@implementation CountlyNotificationService
#if TARGET_OS_IOS
+ (void)didReceiveNotificationRequest:(UNNotificationRequest *)request withContentHandler:(void (^)(UNNotificationContent *))contentHandler
{
    COUNTLY_EXT_LOG(@"didReceiveNotificationRequest:withContentHandler:");

    NSDictionary* countlyPayload = request.content.userInfo[kCountlyPNKeyCountlyPayload];
    NSString* notificationID = countlyPayload[kCountlyPNKeyNotificationID];

    if (!notificationID)
    {
        COUNTLY_EXT_LOG(@"Countly payload not found in notification dictionary!");

        contentHandler(request.content);
        return;
    }

    COUNTLY_EXT_LOG(@"Checking for notification modifiers...");
    UNMutableNotificationContent* bestAttemptContent = request.content.mutableCopy;

    NSArray* buttons = countlyPayload[kCountlyPNKeyButtons];
    if (buttons.count)
    {
        COUNTLY_EXT_LOG(@"%d custom action buttons found.", (int)buttons.count);

        NSMutableArray* actions = NSMutableArray.new;

        [buttons enumerateObjectsUsingBlock:^(NSDictionary* button, NSUInteger idx, BOOL * stop)
        {
            NSString* actionIdentifier = [NSString stringWithFormat:@"%@%lu", kCountlyActionIdentifier, (unsigned long)idx + 1];
            UNNotificationAction* action = [UNNotificationAction actionWithIdentifier:actionIdentifier title:button[kCountlyPNKeyActionButtonTitle] options:UNNotificationActionOptionForeground];
            [actions addObject:action];
        }];

        NSString* categoryIdentifier = [kCountlyCategoryIdentifier stringByAppendingString:notificationID];

        UNNotificationCategory* category = [UNNotificationCategory categoryWithIdentifier:categoryIdentifier actions:actions intentIdentifiers:@[] options:UNNotificationCategoryOptionNone];

        [UNUserNotificationCenter.currentNotificationCenter setNotificationCategories:[NSSet setWithObject:category]];

        bestAttemptContent.categoryIdentifier = categoryIdentifier;
    
        COUNTLY_EXT_LOG(@"%d custom action buttons added.", (int)buttons.count);
    }

    NSString* attachmentURL = countlyPayload[kCountlyPNKeyAttachment];
    if (!attachmentURL.length)
    {
        COUNTLY_EXT_LOG(@"No attachment specified in Countly payload.");
        COUNTLY_EXT_LOG(@"Handling of notification finished.");
        contentHandler(bestAttemptContent);
        return;
    }

    COUNTLY_EXT_LOG(@"Attachment specified in Countly payload: %@", attachmentURL);

    [[NSURLSession.sharedSession downloadTaskWithURL:[NSURL URLWithString:attachmentURL] completionHandler:^(NSURL * location, NSURLResponse * response, NSError * error)
    {
        if (!error)
        {
            COUNTLY_EXT_LOG(@"Attachment download completed!");

            NSString* attachmentFileName = [NSString stringWithFormat:@"%@-%@", notificationID, response.suggestedFilename ?: response.URL.absoluteString.lastPathComponent];

            NSString* tempPath = [NSTemporaryDirectory() stringByAppendingPathComponent:attachmentFileName];

            if (location && tempPath)
            {
                [NSFileManager.defaultManager moveItemAtPath:location.path toPath:tempPath error:nil];

                NSError* attachmentError = nil;
                UNNotificationAttachment* attachment = [UNNotificationAttachment attachmentWithIdentifier:attachmentFileName URL:[NSURL fileURLWithPath:tempPath] options:nil error:&attachmentError];

                if (attachment && !attachmentError)
                {
                    bestAttemptContent.attachments = @[attachment];

                    COUNTLY_EXT_LOG(@"Attachment added to notification!");
                }
                else
                {
                    COUNTLY_EXT_LOG(@"Attachment creation error: %@", attachmentError);
                }
            }
            else
            {
                COUNTLY_EXT_LOG(@"Attachment `location` and/or `tempPath` is nil!");
            }
        }
        else
        {
            COUNTLY_EXT_LOG(@"Attachment download error: %@", error);
        }

        COUNTLY_EXT_LOG(@"Handling of notification finished.");
        contentHandler(bestAttemptContent);
    }] resume];
}
#endif
@end
