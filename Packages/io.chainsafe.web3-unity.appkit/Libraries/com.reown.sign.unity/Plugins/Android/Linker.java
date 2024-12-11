package com.reown.sign.unity;

import android.content.Context;
import android.content.Intent;
import android.net.Uri;

public class Linker {
    public static boolean canOpenURL(Context context, String url){
        Intent intent = new Intent(Intent.ACTION_VIEW, Uri.parse(url));
        return intent.resolveActivity(context.getPackageManager()) != null;
    }
}
