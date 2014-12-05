package com.shephertz.app42Push;

import android.content.SharedPreferences;
import android.util.Log;

import com.google.android.gcm.GCMRegistrar;
import com.unity3d.player.UnityPlayer;

public class App42UnityHelper {

	private final static  String KeyLastMessage = "lastMessage";
	private final static  String AppName = "PushUnityNotification";
	private final static  String GameObject="App42Push";
	private final static String OnMessage="onPushNotificationsReceived";
	private final static String OnRegistrationId="onDeviceTokenFromNative";
	private final static String OnGCMError="onErrorFromNative";

	/*
	 * This function allows to register device for PushNotification service
	 */
	public  static void registerOnGCM(String projectNo) {
		System.out.println("#####################");
		try {
			GCMRegistrar.checkDevice(UnityPlayer.currentActivity);
			GCMRegistrar.checkManifest(UnityPlayer.currentActivity);
			final String regId = GCMRegistrar
					.getRegistrationId(UnityPlayer.currentActivity);
			if (regId.equals("")) {
				GCMRegistrar.register(UnityPlayer.currentActivity, projectNo);
			} else {
				sendGCMRegId(regId);
			}
		} catch (Throwable e) {
			e.printStackTrace();
		}
	}

	public static void sendPushMessage(String message) {
		saveLastMessage(message);
		UnityPlayer.UnitySendMessage(GameObject, OnMessage, message);
		Log.i(" Message Sent : OnMessage", message);
	}

	
	public static void sendGCMRegId(String regId) {
		System.out.println("Sending RegIDDDDDDDDDDDDDDDDDDDD");
		UnityPlayer.UnitySendMessage(GameObject, OnRegistrationId, regId);
		Log.i(" Message Sent : OnRegistrationId", regId);
	}
	public static void sendGCMError(String error) {
		UnityPlayer.UnitySendMessage(GameObject, OnGCMError, error);
		Log.i(" GCM Error :", error);
	}

	public static String getLastMessage() {
		resetCount();
		SharedPreferences sharedPreference = UnityPlayer.currentActivity
				.getSharedPreferences(AppName,
						UnityPlayer.currentActivity.MODE_PRIVATE);
		return sharedPreference.getString(KeyLastMessage, "");
	}

	private static void saveLastMessage(String projectNo) {
		SharedPreferences sharedPreference = UnityPlayer.currentActivity
				.getSharedPreferences(AppName,
						UnityPlayer.currentActivity.MODE_PRIVATE);
		SharedPreferences.Editor prefEditor = sharedPreference.edit();
		prefEditor.putString(KeyLastMessage, projectNo);
		prefEditor.commit();
	}

	/*
	 * Call This Function from Unity after Message is shown on Unity Screen This
	 * function reset PushMessage Count to zero
	 */
	public static void resetCount() {
		App42GCMService.msgCount = 0;
	}

}
