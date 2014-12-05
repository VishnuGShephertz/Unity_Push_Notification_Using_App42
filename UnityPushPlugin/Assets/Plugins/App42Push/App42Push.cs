

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using AssemblyCSharpfirstpass;
using System.Runtime.InteropServices;

public class App42Push : MonoBehaviour
{
	private static App42Push mInstance = null;
	private static App42NativePushListener app42Listener;
	public static void registerForPush (string projectNo)
	{
		#if UNITY_ANDROID
		registerOnGCM(projectNo);
		#endif
		#if UNITY_IPHONE
		registerOnApple();
		#endif
		#if UNITY_WINDOWS
		
		#endif
	}
	public static string getLastPushMessage(){
		string message = "";
		#if UNITY_ANDROID
		message=getLastAndroidMessage();
		#endif
		#if UNITY_IPHONE
	
		#endif
		#if UNITY_WINDOWS
		
		#endif
		return message;
		}
	private static void setNativeCallback ()
	{
		if (mInstance == null) {
			GameObject receiverObject = new GameObject ("App42Push");
			DontDestroyOnLoad (receiverObject);
			mInstance = receiverObject.AddComponent<App42Push> ();
		}
	}
	public static string getDeviceType(){
		#if UNITY_ANDROID
		return "Android";
		#endif
		#if UNITY_IPHONE
		return "IOS";
		#endif
		#if UNITY_WINDOWS
		return "Windows";
		#endif
		return null;
	}
	public static void setApp42PushListener (App42NativePushListener listener)
	{
		  app42Listener = listener;
		  setNativeCallback ();
	}
	
	public void onPushNotificationsReceived (String message)
	{ 
		Debug.Log (message);
		if (app42Listener != null) {
			app42Listener.onMessage(message);

		}
	}
public void onErrorFromNative(string error)
	{
		Debug.Log(error);
		if (app42Listener != null) {
		if (error != null && error.Length!=0) 
		{
			app42Listener.onError(error);
		}
		}
	}
	public void onDeviceToekenFromNative (String deviceToken)
	{ 
		Debug.Log (deviceToken);
		if (app42Listener != null) {
		if (deviceToken != null && deviceToken.Length!=0) 
		{
			app42Listener.onDeviceToken(deviceToken);
		}
		}
	}

#if UNITY_ANDROID
		public static void registerOnGCM(string projectNo) {
      if(Application.platform != RuntimePlatform.Android) return;
      AndroidJNIHelper.debug = false; 
		using (AndroidJavaClass jc = new AndroidJavaClass("com.shephertz.app42Push.App42UnityHelper")) { 
			jc.CallStatic("registerOnGCM", projectNo);
          }
        }
		public static string getLastAndroidMessage() {
			if(Application.platform != RuntimePlatform.Android) return null;
			AndroidJNIHelper.debug = false; 
		using (AndroidJavaClass jc = new AndroidJavaClass("com.shephertz.app42Push.App42UnityHelper")) { 
			return jc.CallStatic<string>("getLastMessage");
			}
	}
	#endif

	#if UNITY_IPHONE

	public static void registerOnApple() {
		[System.Runtime.InteropServices.DllImport("__Internal")]
	 registerForRemoteNotifications();
	}
	#endif

	/* #if UNITY_WINDOWS
	public static void registerOnWindows(string userName) {
		if(Application.platform != RuntimePlatform.Android) return null;
		AndroidJNIHelper.debug = false; 
		using (AndroidJavaClass jc = new AndroidJavaClass("com.shephertz.android.apphype.extensions.unity.UnityHelper")) { 
			jc.CallStatic("intialize", apiKey, secretkey);
		}
	}
	public static string getLastWindowsMessage() {
		if(Application.platform != RuntimePlatform.Android) return false;
		AndroidJNIHelper.debug = false; 
		using (AndroidJavaClass jc = new AndroidJavaClass("com.shephertz.android.apphype.extensions.unity.UnityHelper")) { 
			return jc.CallStatic<string>("isAvailable",adCode);
		}
	}
	#endif */
}
	