package com.shephertz.app42Push;

import com.google.android.gcm.GCMBroadcastReceiver;

import android.content.Context;

public class App42GCMReceiver extends GCMBroadcastReceiver{
	@Override
	protected String getGCMIntentServiceClassName(Context context) { 
		return "com.shephertz.app42Push.App42GCMService"; 
	} 
}
