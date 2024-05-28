#if __has_include(<Cocoa/Cocoa.h>)
#import <Cocoa/Cocoa.h>
#endif

extern "C"
{
    typedef void (*FileCallback)(const char* path);

    void ShowOpenFileDialog(const char* title, const char** allowedFileTypes, int allowedFileTypesCount, FileCallback callback)
    {
        dispatch_async(dispatch_get_main_queue(), ^{
            NSOpenPanel* openPanel = [NSOpenPanel openPanel];
            openPanel.title = [NSString stringWithUTF8String:title];
            openPanel.canChooseFiles = YES;
            openPanel.canChooseDirectories = NO;
            openPanel.allowsMultipleSelection = NO;
            
            NSMutableArray* fileTypes = [NSMutableArray array];
            for (int i = 0; i < allowedFileTypesCount; ++i)
            {
                [fileTypes addObject:[NSString stringWithUTF8String:allowedFileTypes[i]]];
            }
            openPanel.allowedFileTypes = fileTypes;

            if ([openPanel runModal] == NSModalResponseOK)
            {
                NSString* filePath = [[openPanel URL] path];
                callback([filePath UTF8String]);
            }
            else
            {
                callback(NULL);
            }
        });
    }
}