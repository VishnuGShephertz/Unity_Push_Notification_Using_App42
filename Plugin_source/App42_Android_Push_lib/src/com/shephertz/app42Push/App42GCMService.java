package com.shephertz.app42Push;

import java.io.IOException;
import java.io.InputStream;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageManager;
import android.content.res.AssetManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.app.NotificationCompat;
import android.util.Log;

import com.google.android.gcm.GCMBaseIntentService;

public class App42GCMService extends GCMBaseIntentService {

	public static int msgCount = 0;
	public static String notificationMessage = "";
	static String projectNo = "<Your Project No>";
	private static final String DisplayMessageAction = " com.shephertz.app42.android.pushservice.DISPLAY_MESSAGE";
	private static final String NotificationMessage = "message";

	public App42GCMService() {
		super(projectNo);
	}

	@Override
	protected void onError(Context arg0, String arg1) {
		Log.i(TAG, "Device registered: regId = " + arg1);
		try {
			App42UnityHelper.sendGCMError(arg1);
		} catch (Throwable e) {
			Log.e("App42 Exception", e.getMessage());
		}
	}

	@Override
	protected void onMessage(Context context, Intent intent) {

		Bundle b = intent.getExtras();
		String message = (String) b.get("message");
		Log.i(TAG, "Received message " + message);
		notificationMessage = message;
		displayMessage(context, message);
		generateNotification(context, message);
		try {
			App42UnityHelper.sendPushMessage(message);
		} catch (Throwable e) {
			Log.e("App42 Exception", e.getMessage());
		}
	}

	@Override
	protected void onDeletedMessages(Context context, int total) {
		Log.i(TAG, "Received deleted messages notification");
		String message = "" + total + "Message deleted ";
		displayMessage(context, message);
		generateNotification(context, message);
	}

	@Override
	protected void onRegistered(Context context, String registrationId) {

		Log.i(TAG, "Device registered: regId = " + registrationId);
		App42UnityHelper.sendGCMRegId(registrationId);

	}

	@Override
	protected void onUnregistered(Context arg0, String arg1) {
		Log.i(TAG, "onUnregistered " + arg1);
	}

	/**
	 * Issues a notification to inform the user that server has sent a message.
	 */
	private void generateNotification(Context context, String message) {
		long when = System.currentTimeMillis();
		Intent notificationIntent = new Intent();
		NotificationManager notificationManager = (NotificationManager) context
				.getSystemService(Context.NOTIFICATION_SERVICE);
		// set intent so it does not start a new activity
		notificationIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP
				| Intent.FLAG_ACTIVITY_SINGLE_TOP);
		PendingIntent intent = PendingIntent.getActivity(context, 0,
				notificationIntent, PendingIntent.FLAG_UPDATE_CURRENT);
		Notification notification = new NotificationCompat.Builder(context)
				.setContentTitle(getTitle(context)).setContentText(message)
				.setContentIntent(intent)
				.setSmallIcon(android.R.drawable.ic_dialog_info).setWhen(when)
				.setNumber(++msgCount).setLargeIcon(getBitmapFromAssets())
				.setLights(Color.YELLOW, 1, 2).setAutoCancel(true).build();

		notification.defaults |= Notification.DEFAULT_SOUND;
		notification.defaults |= Notification.DEFAULT_VIBRATE;
		notificationManager.notify(0, notification);
	}

	private String getTitle(Context context) {
		try {
			ApplicationInfo ai = context.getPackageManager()
					.getApplicationInfo(this.getPackageName(),
							PackageManager.GET_META_DATA);
			Bundle aBundle = ai.metaData;
			return aBundle.getString("push_title");
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return "App42PushSample";
		}

	}

	public Bitmap getBitmapFromAssets() {
		AssetManager assetManager = getAssets();
		InputStream istr;
		try {
			istr = assetManager.open("app_icon.png");
			Bitmap bitmap = BitmapFactory.decodeStream(istr);
			return bitmap;
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	/**
	 * Notifies UI to display a message.
	 * <p>
	 * This method is defined in the common helper because it's used both by the
	 * UI and the background service.
	 * 
	 * @param context
	 *            application's context.
	 * @param message
	 *            message to be displayed.
	 */
	public static void displayMessage(Context context, String message) {
		Intent intent = new Intent(DisplayMessageAction);
		intent.putExtra(NotificationMessage, message);
		context.sendBroadcast(intent);
	}

}
