package com.web3auth.unity.android;

import android.app.Activity;
import android.content.Intent;
import android.content.pm.ResolveInfo;
import android.content.pm.PackageManager;
import android.net.Uri;

import androidx.browser.customtabs.CustomTabsIntent;
import androidx.browser.customtabs.CustomTabsService;

import java.util.ArrayList;
import java.util.List;

public class BrowserView {

    static String[] customTabsAllowed = new String[] {
        "com.android.chrome",
        "com.google.android.apps.chrome",
        "com.chrome.beta",
        "com.chrome.dev"
    };

    public static String getDefaultBrowser(Activity context) {
        Intent intent = new Intent(Intent.ACTION_VIEW, Uri.parse("https://web3auth.io"));
        ResolveInfo resolveInfo = context.getPackageManager().resolveActivity(intent,PackageManager.MATCH_DEFAULT_ONLY);

        if (resolveInfo != null && !resolveInfo.activityInfo.packageName.isEmpty()) {
            return resolveInfo.activityInfo.packageName;
        }

        return null;
    }

    public static List<String> getCustomTabsBrowsers(Activity context) {
        ArrayList<String> customTabBrowsers = new ArrayList<>();
        for (String browser : customTabsAllowed) {
            Intent customTabIntent = new Intent();
            customTabIntent.setAction(CustomTabsService.ACTION_CUSTOM_TABS_CONNECTION);
            customTabIntent.setPackage(browser);

            if (context.getPackageManager().resolveService(customTabIntent, 0) != null) {
                customTabBrowsers.add(browser);
            }
        }

        return customTabBrowsers;
    }

    public static void launchUrl(Activity context, String url) {
        String defaultBrowser = getDefaultBrowser(context);
        List<String> customTabBrowsers = getCustomTabsBrowsers(context);

        if (customTabBrowsers.contains(defaultBrowser)) {
            CustomTabsIntent customTabsIntent = new CustomTabsIntent.Builder().build();

            customTabsIntent.intent.setPackage(defaultBrowser);
            customTabsIntent.launchUrl(context, Uri.parse(url));
        } else if (!customTabBrowsers.isEmpty()) {
            CustomTabsIntent customTabsIntent = new CustomTabsIntent.Builder().build();

            customTabsIntent.intent.setPackage(customTabBrowsers.get(0));
            customTabsIntent.launchUrl(context, Uri.parse(url));
        } else {
            context.startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse(url)));
        }
    }
}