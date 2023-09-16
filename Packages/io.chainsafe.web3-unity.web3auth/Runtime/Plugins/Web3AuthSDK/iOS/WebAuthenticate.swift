import AuthenticationServices
import SafariServices
import UIKit


@objc public class WebAuthenticate: NSObject {
    public static let instance = WebAuthenticate();
    public static var authSession: ASWebAuthenticationSession? = nil;
    
    @objc public func call(_ url: String,_ redirectUri: String,_ objectName: String) {
        WebAuthenticate.authSession = ASWebAuthenticationSession(
            url: URL(string: url)!, callbackURLScheme: redirectUri) { callbackURL, authError in
                guard authError == nil, let callbackURL = callbackURL else {
                    return
                }

                let unity = UnityFramework.getInstance();
                unity?.sendMessageToGO(withName: objectName, functionName: "onDeepLinkActivated", message: callbackURL.absoluteString);
        }
        
        if #available(iOS 13.0, *) {
            WebAuthenticate.authSession?.presentationContextProvider = self
            WebAuthenticate.authSession?.prefersEphemeralWebBrowserSession = false
        }
        
        WebAuthenticate.authSession?.start();
    }
    
    @objc public static func launch(_ url: String,_ redirectUri: String,_ objectName: String) {
        instance.call(url, redirectUri, objectName);
    }
}


@available(iOS 12.0, *)
extension WebAuthenticate: ASWebAuthenticationPresentationContextProviding {
    public func presentationAnchor(for session: ASWebAuthenticationSession) -> ASPresentationAnchor {
        return UnityFramework.getInstance().appController().window;
    }
}
